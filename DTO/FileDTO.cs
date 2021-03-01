using Microsoft.AspNetCore.Http;

namespace learner_portal.DTO
{
    public class FileDTO
    {
        public byte[] Data { get; set; }
        public string Path { get; set; }
        public IFormFile File  { get; set; }
        
        public string DocumentTypeId { get; set; }
        
        
    }
}