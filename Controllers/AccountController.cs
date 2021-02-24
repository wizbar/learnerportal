using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AspNetCoreHero.ToastNotification.Abstractions;
using learner_portal.DTO;
using learner_portal.Helpers;
using learner_portal.Models;
using learner_portal.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Logging;
using MimeKit;
 
using Enum = learner_portal.Helpers.Enum;
using Notification = learner_portal.Helpers.Notification;

namespace learner_portal.Controllers
{
    public class AccountController : BaseController
    {
        private readonly SignInManager<Users> _signInManager;
        private readonly UserManager<Users> _userManager;
        private readonly IEmailSender _emailSender;
        private readonly ILookUpService _lookUpService;
        private readonly LearnerContext _context;
        private readonly RoleManager<Roles> _roleManager;
        private readonly EmailConfiguration _emailConfig;    
        private readonly ILogger<AccountController> _logger;  
        private readonly INotyfService _notyf;
            
       //  private readonly IToastNotification _toastNotification;
         

        public AccountController(
            UserManager<Users> userManager,
            SignInManager<Users> signInManager,
            IEmailSender emailSender,
            RoleManager<Roles> roleManager,
            ILookUpService lookUpService,
            EmailConfiguration emailConfig,
            ILogger<AccountController> logger,
            LearnerContext context,
            INotyfService notyf

        )
        {  
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
            _emailSender = emailSender;
            _emailConfig = emailConfig;
            _roleManager = roleManager;
            _lookUpService = lookUpService;
            _context = context;
            _notyf = notyf;
           
        }
 
        
        // GET
        public IActionResult Index()
        {
           
            ViewData["Name"] = new SelectList(_roleManager.Roles, "Name", "Name");
            return View();
        }

        public IActionResult Register()
        {
            ViewData["Name"] = new SelectList(_roleManager.Roles, "Name", "Name");
            return View();
        }  
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterUserDto input)
        {
            ViewData["Name"] = new SelectList(_roleManager.Roles, "Name", "Name",input.Role);
            input.ActiveYn = "No";
            if (ModelState.IsValid)
            {
                /*if (_userManager.FindByNameAsync(input.UserName) != null || _userManager.FindByEmailAsync(input.Email) != null )
                    {   

                      _notyf.Error("Users already exists",10);
                    _logger.LogError("Users already exists");
                    return View();
                }*/

                _logger.LogInformation("--- Register(" + input.Email + ") Start --");
                
                //Create a User object
                var user = new Users {UserName = input.UserName, Email = input.Email};
                
                //Create a User on DB
                var result = await _userManager.CreateAsync(user, input.Password);
                user.ActiveYn = Const.FALSE;
                _context.User.Update(user);
                await _context.SaveChangesAsync();
                //Add User to role
                await _userManager.AddToRoleAsync(user,input.Role);
                
                if (result.Succeeded)
                {
                    _logger.LogInformation("User created a new account with password");
                    _logger.LogInformation("Send an activation email...");
                    await SendActivationMail(user);
                    
                    if (_userManager.Options.SignIn.RequireConfirmedAccount)
                    {
                        return View("Login");
                    }
                    else 
                    {  
                      
                       await _signInManager.SignInAsync(user, isPersistent: true);
                        
                        if (_lookUpService.GetPersonDetailsByEmail(input.Email) == null)
                        {
                            _notyf.Information("Your registration was successful",10);
                            _logger.LogInformation("--- Register(" + input.Email + ") Create Account Start --");
                            return RedirectToRoute("", 
                                new { controller = "Person", action = "CreateAccount" });
                        }
                        else
                        {
                            if (_roleManager.GetRoleNameAsync(new Roles{Name = input.Role}).Result.Equals(Const.ADMINISTRATOR_USER) )
                            {
                                
                                _logger.LogInformation("--- Register(" + input.Email + ") Person Index Start --");
                                return RedirectToRoute("",  
                                    new { controller = "Person", action = "Index" });
                            }
                            _logger.LogInformation("--- Register(" + input.Email + ") Login Start --");
                            return View("Login");
                        }
           
                    }
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }
   
            // If we got this far, something failed, redisplay form
     
            return RedirectToPage("/Dashboard/Dashboard");    
        } 
         
       private async Task<Mail> SendActivationMail(Users user) 
        {
            _logger.LogDebug($"Generate an Email Confirmation Token.");
            var token = await _signInManager.UserManager.GenerateEmailConfirmationTokenAsync(user);
            _logger.LogDebug("Base64 Url Encoding...");
            token = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(token));
            _logger.LogInformation("Base64 Url Id : " + token);  
            _logger.LogDebug("Build the Url..");

            var confirmationLink =  Url.Action(nameof(ActivateAccount), "Account", new { token, email = user.Email }, Request.Scheme);

            var toMail = new MailboxAddress(user.Email);
            var toList = new List<InternetAddress> {toMail};
                
            _logger.LogDebug("Getting Email Template from the DB...");
            var registrationEmailTemplate =
                _context.EmailTemplates.FirstOrDefault(e => e.Name == Const.ACTIVATE_ACCOUNT);
            if (registrationEmailTemplate != null)
            {
                var body = registrationEmailTemplate.EmailBody;

                _logger.LogInformation("Update the Url within the Email Body ...");

                body = body.Replace("##activate##", confirmationLink);
                

                _logger.LogInformation("Creating a Mail object ..." );
                var message = new Mail
                {
                    FromEmail = _emailConfig.From,
                    To = toList,
                    ToEmail = user.Email,
                    Subject = registrationEmailTemplate.Subject,
                    Body = body,
                    DateCreated = DateTime.Now,
                    CreatedBy = user.UserName,
                }; 

 
                _logger.LogInformation("Send an email...");
                _emailSender.SendEmail(message);
                return message;
            }

            _logger.LogError("Sending an email failed...");
            return null;
        }
       
       public async Task<IActionResult> ActivateAccount(string token, string userId)
           {
               var user = await _signInManager.UserManager.FindByIdAsync(userId);
               if (user == null)
               {
                   ViewBag.ErrorTitle = "Account Activation Failed";
                   ViewBag.ErrorMessage = "It does not look like this user was registered with us...";
                   return View("Error");
               }

               var result = await _signInManager.UserManager.ConfirmEmailAsync(user, token);

               if (result.Succeeded)
               {
                   Alert("Account for " + user.Email + " activated successfully...", Enum.NotificationType.success);
                  return View("Login");
               }

               ViewBag.ErrorTitle = "Account Activation Failed";
               ViewBag.ErrorMessage = "Please make sure that you click on the correct link....";
               return View("Error");
           }       
       [HttpGet]
       public IActionResult ResetPassword(string token, string email) 
       {
           var model = new ResetPasswordDTO { Token = token, Email = email };
           return View(model);
       }
       
       [HttpPost]
       [ValidateAntiForgeryToken]
       public async Task<IActionResult> ResetPassword(ResetPasswordDTO model)
       {
           if (ModelState.IsValid)
           {
               var user = await _userManager.FindByEmailAsync(model.Email);
               if (user != null)
               {
                  var result = await _userManager.ResetPasswordAsync(user, model.Token, model.Password);
                  if (result.Succeeded)
                  {
                      return View("ResetPasswordConfirmation");
                  }
                  foreach (var error in result.Errors)
                  {
                      ModelState.AddModelError(string.Empty, error.Description);
                  }
                  return View(model);
               }
               return View("ResetPasswordConfirmation");
           }

           return View(model);
       }
       [HttpGet]
       public IActionResult ResetPasswordConfirmation()
       {
           return View(); 
       }
        
       public async Task<IActionResult> ConfirmEmail(string userId, string code)
       {
           if (userId == null || code == null)
           {
               return RedirectToPage("/Index");
           }

           var user = await _userManager.FindByIdAsync(userId);
           if (user == null)
           {
               return NotFound($"Unable to load user with ID '{userId}'.");
           }

           code = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(code));
           var result = await _userManager.ConfirmEmailAsync(user, code);
           ModelState.AddModelError("",result.Succeeded? "Thank you for confirming your email." : "Error confirming your email.");
           return View();
       }
       
           public IActionResult Login() 
           {
               return View();    
           }    

         
        [HttpPost]        
        public async Task<IActionResult> Login(LoginDTO input) 
        {
           // returnUrl = returnUrl ?? Url.Content("~/");

            if (ModelState.IsValid)
            {
                // This doesn't count login failures towards account lockout
                // To enable password failures to trigger account lockout, set lockoutOnFailure: true
                var result = await _signInManager.PasswordSignInAsync(input.Username, input.Password, input.RememberMe, lockoutOnFailure: false);
                if (result.Succeeded)
                { 
                    var person = _lookUpService.GetPersonDetailsByUsername(input.Username).Result;

                    if(person == null){
                           _notyf.Warning("User logged out successful...", 5);
                           return RedirectToRoute("",   new { controller = "Person", action = "CreateAccount" } );
                       
                    }
                    else 
                    {
                        _notyf.Success("Login was successful",10);
                        return RedirectToRoute("",new {controller = "Person", action = "Details", Id = person.NationalID});
                      
                    }
                }
                else
                {
                    _notyf.Error("Login for " + input.Username + " failed...", 5);
                   
                  return View(input);
                }

            } 
            _notyf.Error("Please capture you information correctly...", 5);
            // If we got this far, something failed, redisplay form
            return View(input);
        }

        public  IActionResult CreateRole()
        {
            
            return View();
        }
        
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateRole(RoleDTO roleDto)
        {
            if (ModelState.IsValid)
            {
                await _roleManager.CreateAsync(new Roles{ Name = roleDto.Role});
            }

            return View();
        }
        
        public async Task<IActionResult> LogOut()
        {
            await _signInManager.SignOutAsync();
            _notyf.Warning("User logged out successful...", 5);
            _logger.LogInformation("User logged out");
            return View("Login");
        }
        
        public  IActionResult ForgotPassword()
        {
            return View();
        }
        
        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordDTO input)
        {
            if (ModelState.IsValid)
            { 
                var user = await _userManager.FindByEmailAsync(input.Email);
                if (user == null || !(await _userManager.IsEmailConfirmedAsync(user)))
                {  
                    // Don't reveal that the user does not exist or is not confirmed
                    _notyf.Information("We have sent you an email to reset your password",5);
                    return View("Login");
                }
               else {
                await SendForgotPasswordMail(user);
                
                // Send email
                _notyf.Information("We have sent you an email to reset your password",5);
                }
            }
            _notyf.Information("We have sent you an email to reset your password",5);
            return View("Login");
        }
        
          private async Task<Mail> SendForgotPasswordMail(Users user) 
        {
            _logger.LogDebug("Generate an Email Confirmation Token");
            var token = await _signInManager.UserManager.GenerateEmailConfirmationTokenAsync(user);
            _logger.LogDebug("Base64 Url Encoding...");
            token = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(token));
            _logger.LogInformation("Base64 Url Id : " + token);
            _logger.LogDebug("Build the Url..");
 
            var confirmationLink =
                Url.Page(
                    "/Account/ResetPassword",
                    pageHandler: null,
                    values: new {userId = user.Id, token = token},
                    protocol: Request.Scheme);

            var toMail = new MailboxAddress(user.Email);
            var toList = new List<InternetAddress> {toMail};
            
            _logger.LogDebug("Getting Email Template from the DB...");
            var registrationEmailTemplate =
                _context.EmailTemplates.FirstOrDefault(e => e.Name == Const.RESET_PASSWORD);
            if (registrationEmailTemplate != null)
            {
                var body = registrationEmailTemplate.EmailBody;

                _logger.LogInformation("Update the Url within the Email Body ...");
 
                body = body.Replace("##activate##", confirmationLink);

                var message = new Mail
                {
                    FromEmail = _emailConfig.From,
                    To = toList,
                    ToEmail = user.Email,
                    Subject = registrationEmailTemplate.Subject,
                    Body = body,
                    DateCreated = DateTime.Now,
                    CreatedBy = user.UserName,
                };


                _logger.LogInformation("Send an email...");
                _emailSender.SendEmail(message);
                return message;
            }

            _logger.LogError("Sending an email failed...");
            return null;
        }
        
    }
}
