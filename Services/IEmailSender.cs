using learner_portal.Models;

namespace learner_portal.Services
{
    public interface IEmailSender
    {
        void SendEmail(Mail message);
        
    }
}