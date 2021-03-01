using System;
using System.Collections.Generic;
using System.IO;
using learner_portal.DTO;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;

namespace learner_portal.Services
{
    public class FileService : IFileService, IDisposable
    {
        private readonly IWebHostEnvironment _env;
        private bool _disposed;
        public FileService(IWebHostEnvironment env)
        {
            _env = env;
        }
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!this._disposed)
            {
                if (disposing)
                {
                   
                }
                this._disposed = true;
            }
        }
        
        
        public string UploadFile(IFormFile file,string path)
        {
            path = _env.WebRootPath + path;
            
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            using var fileStream = new FileStream(Path.Combine(path,file.FileName),FileMode.Create,FileAccess.Write);
            file.CopyToAsync(fileStream);

            return path + file.FileName;
        }

        public bool UploadFiles(IEnumerable<IFormFile> files,string path)
        {
            path = _env.WebRootPath + path;
            if (!Directory.Exists(path))
            { 
                Directory.CreateDirectory(path);
            }

            foreach (var file in files)
            {
                using var fileStream = new FileStream(Path.Combine(path,file.FileName),FileMode.Create,FileAccess.Write);
                file.CopyToAsync(fileStream);
            }
 
            return true;

        }
        
        
        public FileDTO  DownloadFile(string path)
        {
            FileDTO dataFile = new FileDTO();
            // Checks if the id.
            if (!string.IsNullOrEmpty(path))
            {
                try
                {
                    dataFile.Data = File.ReadAllBytes(_env.WebRootPath + path);
                    dataFile.Path = path;
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    throw;
                }
            }
            return dataFile;  
        }      
        
        public bool  DeleteFile(string path)
        {
            path = _env.WebRootPath + path;
            // Checks if the id.
            if (!string.IsNullOrEmpty(path))
            {
                try
                {
                    File.Delete(path);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    throw;
                }
            }
            return true;  
        }
        
        public bool  FileExists(string path)
        {
             path = _env.WebRootPath + path;
            // Checks if the id.
            return File.Exists(path);

        }
    }
}