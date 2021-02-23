﻿using System.Collections.Generic;
using learner_portal.DTO;
using Microsoft.AspNetCore.Http;
             
namespace learner_portal.Services  
{    
    public interface IFileService 
    {     
        bool UploadFile(IFormFile file,string filePath);
        bool UploadFiles(IEnumerable<IFormFile> files,string filePath);

        FileDTO DownloadFile(string path);
         bool DeleteFile(string path);
         bool FileExists(string path);
    }
}