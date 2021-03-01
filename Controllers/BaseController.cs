using learner_portal.Helpers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace learner_portal.Controllers
{
    public class BaseController: Controller     
    {   
        protected  void OnException(ExceptionContext filterContext)  
        {  

            var view = new ViewResult {ViewName = "Error"};

            filterContext.Result = view;  
            filterContext.ExceptionHandled = true;  
        }  
        
        public void Alert(string message, Enum.NotificationType notificationType)
        {
            var msg = "<script language='javascript'> swal('" + notificationType.ToString().ToUpper() + "', '" +
                      message + "','" + notificationType + "', { timer: 5000,closeModal: true,buttons: [true, 'OK!'], className: 'red-bg',})" + "</script>";
            TempData["notification"] = msg;
    }
        
        /// <summary>
        /// Sets the information for the system notification.
        /// </summary>
        /// <param name="message">The message to display to the user.</param>
        /// <param name="notifyType">The type of notification to display to the user: Success, Error or Warning.</param>
        public void Message(string message, Enum.NotificationType notifyType)
        {
            TempData["Notification2"] = message;
        
            switch (notifyType)
            {
                case Enum.NotificationType.success:
                    TempData["NotificationCSS"] = "alert alert-secondary";
                    break;
                case Enum.NotificationType.error:
                    TempData["NotificationCSS"] = "alert alert-danger";
                    break;
                case Enum.NotificationType.warning:
                    TempData["NotificationCSS"] = "alert alert-warning";
                    break;
        
                case Enum.NotificationType.info:
                    TempData["NotificationCSS"] = "alert-box notice";
                    break;
            }
        }
    }  
}