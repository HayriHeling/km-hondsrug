using Eduria.Services;
using EduriaData.Models;
using System.Collections.Generic;
using System.Linq;

namespace Eduria
{
    public class UserExamService : AService<UserExam>
    {
        public UserExamService(EduriaContext context)
        {
            Context = context;
        }

        /// <summary>
        /// Get all the UserTest data from the database.
        /// </summary>
        /// <returns>An list with the data.</returns>
        public override IEnumerable<UserExam> GetAll()
        {
            return Context.UserExams;
        }

        /// <summary>
        /// Get an UserTest with its data from the database by the specific id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns>The UserTest datamodel.</returns>
        public override UserExam GetById(int id)
        {
            return GetAll().FirstOrDefault(usertest => usertest.Id == id);
        }
    }
}
