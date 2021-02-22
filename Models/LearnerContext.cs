using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace learner_portal.Models
{
    public class LearnerContext : IdentityDbContext
    {
        public LearnerContext()
        {
        }

        public LearnerContext(DbContextOptions<LearnerContext> options)
            : base(options)
        { 
        }
  
        public virtual DbSet<Address> Address { get; set; }
        public virtual DbSet<AddressType> AddressType { get; set; }
        public virtual DbSet<CitizenshipStatus> CitizenshipStatus { get; set; }
        public virtual DbSet<City> City { get; set; }
        public virtual DbSet<Country> Country { get; set; }
        public virtual DbSet<Course> Course { get; set; }
        public virtual DbSet<DisabilityStatus> DisabilityStatus { get; set; }
        public virtual DbSet<Equity> Equity { get; set; }
        public virtual DbSet<Gender> Gender { get; set; }
        public virtual DbSet<HomeLanguage> HomeLanguage { get; set; }
        public virtual DbSet<Nationality> Nationality { get; set; }
        public virtual DbSet<Person> Person { get; set; }
        public virtual DbSet<Province> Province { get; set; }
        public virtual DbSet<Suburb> Suburb { get; set; }
        public virtual DbSet<Users> User { get; set; }
        public virtual DbSet<Roles> Role { get; set; }
        public virtual DbSet<UserRole> UserRole { get; set; }
        public virtual DbSet<Dashboard> Dashboard { get; set; }

        public virtual DbSet<Institution> Institution { get; set; }

        public virtual DbSet<InstitutionType> InstitutionType { get; set; }

        public virtual DbSet<Learner> Learner { get; set; }

        public virtual DbSet<LearnerCourse> LearnerCourse { get; set; }

        public virtual DbSet<School> School { get; set; }

        public virtual DbSet<SchoolGrade> SchoolGrade { get; set; }

        public virtual DbSet<Company> Company { get; set; }
        public virtual DbSet<Job> Jobs { get; set; }
        public virtual DbSet<JobSector> JobSector { get; set; }
        public virtual DbSet<JobType> JobType { get; set; }
        public virtual DbSet<Mail> Mail { get; set; }
        public virtual DbSet<Ofo> Ofo { get; set; }
        public virtual DbSet<Financialyear> Financialyear { get; set; }
        public virtual DbSet<OfoUnit> OfoUnit { get; set; }
        public virtual DbSet<Sector> Sector { get; set; }
        public virtual DbSet<OfoMinor> OfoMinor { get; set; }
		public virtual DbSet<JobApplications> JobApplications { get; set; }
		public virtual DbSet<EmailTemplates> EmailTemplates { get; set; }
		public virtual DbSet<Document> Document { get; set; }
		public virtual DbSet<DocumentType> DocumentType { get; set; }

    }
}
