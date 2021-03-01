using System;

namespace learner_portal.Helpers
{
    public static class Utils
    {

        public static string GenerateAssetId() { 
            var assetId = Guid.NewGuid();
            return "ASS-" + assetId.ToString(); 
        }        
         
        public static string GenerateImageFolderId() {
            return "IMG-" + Guid.NewGuid().ToString("N").Substring(0,10);
        }  
        
        public static string GenerateJobTitleId() {
            return "JOB-" + Guid.NewGuid().ToString("N").Substring(0,5);
        }  
        
        public static string GenerateDocsFolderId() {
          ;
            return "DOCS-" + Guid.NewGuid().ToString("N").Substring(0,10);
        }

        /*public static string GenerateVinNo()
        { 
            Guid VehicleId = Guid.NewGuid();
            return "VIN-" + VehicleId.ToString();
        }*/
        
        /// <summary>  
        /// For calculating only age  
        /// </summary>  
        /// <param name="dateOfBirth">Date of birth</param>  
        /// <returns> age e.g. 26</returns>  
        public static int CalculateAge(DateTime dateOfBirth)  
        {  
            var age = 0;  
            age = DateTime.Now.Year - dateOfBirth.Year;  
            if (DateTime.Now.DayOfYear < dateOfBirth.DayOfYear)  
                age = age - 1;  
  
            return age;  
        }         
        public static DateTime GetDate(string idate)  
        {  
          return  Convert.ToDateTime(idate);
          
        } 
    }
}