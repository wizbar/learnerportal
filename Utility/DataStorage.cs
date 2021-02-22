using System.Collections.Generic;
using System.Linq;
using learner_portal.Models;

namespace learner_portal.Utility
{
    public class DataStorage
    {

        private static readonly LearnerContext learnerContext = new LearnerContext();
        


        public static IEnumerable<Person> GetAllPerson()
        {
         return  learnerContext.Person.Take(4).ToList();
        }
    }
}