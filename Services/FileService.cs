using System;
using System.Collections.Generic;
using System.IO;
using learner_portal.DTO;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting;

namespace learner_portal.Services
{
    public class FileService : IFileService, IDisposable
    {
        private readonly IHostEnvironment _env;
        private bool _disposed;
        public FileService(IHostEnvironment env)
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
        
        
        public bool UploadFile(IFormFile file,string filePath)
        {
            if (!Directory.Exists(filePath))
            {
                Directory.CreateDirectory(filePath);
            }

            using var fileStream = new FileStream(Path.Combine(filePath,file.FileName),FileMode.Create,FileAccess.Write);
            file.CopyToAsync(fileStream);

            return true;
        }

        public bool UploadFiles(IEnumerable<IFormFile> files,string filePath)
        {
            if (!Directory.Exists(filePath))
            { 
                Directory.CreateDirectory(filePath);
            }

            foreach (var file in files)
            {
                using var fileStream = new FileStream(Path.Combine(filePath,file.FileName),FileMode.Create,FileAccess.Write);
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
                    dataFile.Data = File.ReadAllBytes(path);
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
            // Checks if the id.
            return File.Exists(path);

        }
    }
}