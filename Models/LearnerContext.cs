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

        public virtual DbSet<AccreditationStatuses> AccreditationStatuses => Set<AccreditationStatuses>();
        public virtual DbSet<ApplicationType> ApplicationType => Set<ApplicationType>();
        public virtual DbSet<Assessor> Assessor => Set<Assessor>();
        public virtual DbSet<Address> Address => Set<Address>();
        public virtual DbSet<AddressType> AddressType => Set<AddressType>();
        public virtual DbSet<CitizenshipStatus> CitizenshipStatus => Set<CitizenshipStatus>();
        public virtual DbSet<City> City => Set<City>();
        public virtual DbSet<Country> Country => Set<Country>();
        public virtual DbSet<Course> Course => Set<Course>();
        public virtual DbSet<DisabilityStatus> DisabilityStatus => Set<DisabilityStatus>();
        public virtual DbSet<Equity> Equity => Set<Equity>();
        public virtual DbSet<Evaluator> Evaluator => Set<Evaluator>();
        public virtual DbSet<Etqe> Etqe => Set<Etqe>();
        public virtual DbSet<Gender> Gender => Set<Gender>();
        public virtual DbSet<HomeLanguage> HomeLanguage => Set<HomeLanguage>();
        public virtual DbSet<Nationality> Nationality => Set<Nationality>();
        public virtual DbSet<Person> Person => Set<Person>();
        public virtual DbSet<Province> Province => Set<Province>();
        public virtual DbSet<Suburb> Suburb => Set<Suburb>();
        public virtual DbSet<Qualification> Qualification => Set<Qualification>();
        public virtual DbSet<Users> Users => Set<Users>();
        public virtual DbSet<Roles> Role => Set<Roles>();
        public virtual DbSet<UserRole> UserRole => Set<UserRole>();
        public virtual DbSet<Dashboard> Dashboard => Set<Dashboard>();

        public virtual DbSet<Institution> Institution => Set<Institution>();

        public virtual DbSet<InstitutionType> InstitutionType => Set<InstitutionType>();

        public virtual DbSet<Learner> Learner => Set<Learner>();

        public virtual DbSet<LearnerCourse> LearnerCourse => Set<LearnerCourse>();

        public virtual DbSet<School> School => Set<School>();

        public virtual DbSet<SchoolGrade> SchoolGrade => Set<SchoolGrade>();

        public virtual DbSet<Company> Company => Set<Company>();
        public virtual DbSet<Job> Jobs => Set<Job>();
        public virtual DbSet<JobSector> JobSector => Set<JobSector>();
        public virtual DbSet<JobType> JobType => Set<JobType>();
        public virtual DbSet<Mail> Mail => Set<Mail>();
        public virtual DbSet<Ofo> Ofo => Set<Ofo>();
        public virtual DbSet<Financialyear> Financialyear => Set<Financialyear>();
        public virtual DbSet<OfoUnit> OfoUnit => Set<OfoUnit>();
        public virtual DbSet<Sector> Sector => Set<Sector>();
        public virtual DbSet<OfoMinor> OfoMinor => Set<OfoMinor>();
		public virtual DbSet<JobApplications> JobApplications => Set<JobApplications>();
		public virtual DbSet<EmailTemplates> EmailTemplates => Set<EmailTemplates>();
		public virtual DbSet<Document> Document => Set<Document>();
		public virtual DbSet<DocumentType> DocumentType => Set<DocumentType>();

    }
}
