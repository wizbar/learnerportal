namespace learner_portal.Models
{
    public class AssessorViewModel
    {
        public Person Person { get; set; }
        public Address Address { get; set; } 
        public Assessor Assessor { get; set; }
        public Learner Learner { get; set; }
        public Qualification Qualification { get; set; }
    }
}