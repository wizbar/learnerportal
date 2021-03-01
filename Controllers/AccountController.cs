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
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MimeKit;
using Enum = learner_portal.Helpers.Enum;

namespace learner_portal.Controllers
{
    public class AccountController : BaseController
    {
        private readonly LearnerContext _context;
        private readonly EmailConfiguration _emailConfig;
        private readonly IEmailSender _emailSender;
        private readonly ILogger<AccountController> _logger;
        private readonly ILookUpService _lookUpService;
        private readonly INotyfService _notyf;
        private readonly RoleManager<Roles> _roleManager;
        private readonly SignInManager<Users> _signInManager;
        private readonly UserManager<Users> _userManager;

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
        public async Task<JsonResult> GetAllUsers()
        {
            var draw = HttpContext.Request.Query["draw"].FirstOrDefault();
            // Skiping number of Rows count  
            var start = Request.Query["start"].FirstOrDefault();
            // Paging Length 10,20   
            var length = Request.Query["length"].FirstOrDefault();
            // Sort Column Name  
            var sortColumn = Request
                .Query["columns[" + Request.Query["order[0][column]"].FirstOrDefault() + "][name]"]
                .FirstOrDefault();
            // Sort Column Direction ( asc ,desc)  
            var sortColumnDirection = Request.Query["order[0][dir]"].FirstOrDefault();
            // Search Value from (Search box)  
            var searchValue = Request.Query["search[value]"].FirstOrDefault();

            //Paging Size (10,20,50,100)  
            var pageSize = length != null ? Convert.ToInt32(length) : 0;
            var skip = start != null ? Convert.ToInt32(start) : 0;
            var recordsTotal = 0;


            var listOfUsers = await _lookUpService.GetUsers().ConfigureAwait(false);

            // Getting all Customer data  z 
            var allUsers = listOfUsers;

            //Search
            if (!string.IsNullOrEmpty(searchValue))
                listOfUsers = listOfUsers.Where(m =>
                        m.Email == searchValue ||
                        m.Username == searchValue)
                    as List<UserInfoDTO>;

            //total number of rows count   
            recordsTotal = listOfUsers.Count();
            //Paging   
            var data = listOfUsers.Skip(skip).Take(pageSize).ToList();
            //Returning Json Data  
            return Json(new
            {
                draw, recordsFiltered = recordsTotal, recordsTotal, data = listOfUsers
            });
        }

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
            try
            {
                if (!ModelState.IsValid)
                {
                    var messages = string.Join("; ", ModelState.Values
                        .SelectMany(x => x.Errors)
                        .Select(x => x.ErrorMessage));
                    throw new Exception("Please correct the following errors: " + Environment.NewLine + messages);
                }

                ViewData["Name"] = new SelectList(_roleManager.Roles, "Name", "Name", input.Role);
                input.ActiveYn = "No";
                if (await _userManager.FindByNameAsync(input.UserName) != null ||
                    await _userManager.FindByEmailAsync(input.Email) != null)
                {
                    _notyf.Error("Username : " + input.UserName + " or Email : " + input.Email + " already exists", 10);
                    _logger.LogError("Username or Email already exists");
                    return View();
                }

                _logger.LogInformation("--- Register(" + input.Email + ") Start --");

                //Create a User object
                var user = new Users {UserName = input.UserName, Email = input.Email};
                user.ActiveYn = Const.FALSE;
                //Create a User on DB
                var result = await _userManager.CreateAsync(user, input.Password);

                /*
                _context.User.Update(user);
                await _context.SaveChangesAsync();*/
                //Add User to role
                await _userManager.AddToRoleAsync(user, input.Role);

                if (result.Succeeded)
                {
                    _logger.LogInformation("User created a new account with password");
                    _logger.LogInformation("Send an activation email...");
                    await SendActivationMail(user);

                    if (_userManager.Options.SignIn.RequireConfirmedAccount) return View("Login");

                    await _signInManager.SignInAsync(user, true);

                    if (await _lookUpService.GetPersonDetailsByEmail(input.Email) == null)
                    {
                        _notyf.Information("Your registration was successful", 10);
                        _logger.LogInformation("--- Register(" + input.Email + ") Create Account Start --");
                        return RedirectToRoute("",
                            new {controller = "Person", action = "CreateAccount"});
                    }

                    if (_roleManager.GetRoleNameAsync(new Roles {Name = input.Role}).Result
                        .Equals(Const.ADMINISTRATOR_USER))
                    {
                        _logger.LogInformation("--- Register(" + input.Email + ") Person Index Start --");
                        return RedirectToRoute("",
                            new {controller = "Person", action = "Index"});
                    }

                    _logger.LogInformation("--- Register(" + input.Email + ") Login Start --");
                    return View("Login");
                }
            }
            catch (Exception ex)
            {
                _notyf.Error("Something went wrong...");
                return Json(new {Result = "ERROR", ex.Message});
            }
            // If we got this far, something failed, redisplay form

            return RedirectToPage("/Dashboard/Dashboard");
        }

        private async Task<Mail> SendActivationMail(Users user)
        {
            _logger.LogDebug("Generate an Email Confirmation Token.");
            var token = await _signInManager.UserManager.GenerateEmailConfirmationTokenAsync(user);
            _logger.LogDebug("Base64 Url Encoding...");
            token = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(token));
            _logger.LogInformation("Base64 Url Id : " + token);
            _logger.LogDebug("Build the Url..");

            var confirmationLink = Url.Action(nameof(ActivateAccount), "Account", new {token, email = user.Email},
                Request.Scheme);

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


                _logger.LogInformation("Creating a Mail object ...");
                var message = new Mail
                {
                    FromEmail = _emailConfig.From,
                    To = toList,
                    ToEmail = user.Email,
                    Subject = registrationEmailTemplate.Subject,
                    Body = body,
                    DateCreated = DateTime.Now,
                    CreatedBy = user.UserName
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
            var model = new ResetPasswordDTO {Token = token, Email = email};
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
                    if (result.Succeeded) return View("ResetPasswordConfirmation");
                    foreach (var error in result.Errors) ModelState.AddModelError(string.Empty, error.Description);
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
            if (userId == null || code == null) return RedirectToPage("/Index");

            var user = await _userManager.FindByIdAsync(userId);
            if (user == null) return NotFound($"Unable to load user with ID '{userId}'.");

            code = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(code));
            var result = await _userManager.ConfirmEmailAsync(user, code);
            ModelState.AddModelError("",
                result.Succeeded ? "Thank you for confirming your email." : "Error confirming your email.");
            return View();
        }

        public IActionResult Login()
        {
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginDTO input)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    var messages = string.Join("; ", ModelState.Values
                        .SelectMany(x => x.Errors)
                        .Select(x => x.ErrorMessage));
                    throw new Exception("Please correct the following errors: " + Environment.NewLine + messages);
                }

                // This doesn't count login failures towards account lockout
                // To enable password failures to trigger account lockout, set lockoutOnFailure: true
                var result =
                    await _signInManager.PasswordSignInAsync(input.Username, input.Password, input.RememberMe, false);
                if (result.Succeeded)
                {
                    var person = _lookUpService.GetPersonDetailsByUsername(input.Username).Result;

                    var user = await _userManager.Users.FirstOrDefaultAsync(a => a.UserName.Equals(input.Username));
                    var userRole = await _userManager.GetRolesAsync(user);

                    if (userRole.ToList()[0].Equals(Const.ADMINISTRATOR_USER))
                        return RedirectToRoute("", new {controller = "Dashboard", action = "Index"});

                    if (person == null)
                    {
                        _notyf.Warning("User logged out successful...", 10);
                        return RedirectToRoute("", new {controller = "Person", action = "CreateAccount"});
                    }

                    _notyf.Success("Login was successful", 10);
                    return RedirectToRoute("", new {controller = "Person", action = "Details", Id = person.NationalID});
                }

                _notyf.Error("Login for " + input.Username + " failed...", 10);

                return View(input);
            }
            catch (Exception ex)
            {
                _notyf.Error("Something went wrong...");
                return Json(new {Result = "ERROR", ex.Message});
            }
        }

        public IActionResult CreateRole()
        {
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateRole(RoleDTO roleDto)
        {
            if (ModelState.IsValid) await _roleManager.CreateAsync(new Roles {Name = roleDto.Role});

            return View();
        }

        public async Task<IActionResult> LogOut()
        {
            await _signInManager.SignOutAsync();
            _notyf.Warning("User logged out successful...", 5);
            _logger.LogInformation("User logged out");
            return View("Login");
        }

        public IActionResult ForgotPassword()
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
                if (user == null || !await _userManager.IsEmailConfirmedAsync(user))
                {
                    // Don't reveal that the user does not exist or is not confirmed
                    _notyf.Information("We have sent you an email to reset your password", 5);
                    return View("Login");
                }

                await SendForgotPasswordMail(user);

                // Send email
                _notyf.Information("We have sent you an email to reset your password", 5);
            }

            _notyf.Information("We have sent you an email to reset your password", 5);
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
                    null,
                    new {userId = user.Id, token},
                    Request.Scheme);

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
                    CreatedBy = user.UserName
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