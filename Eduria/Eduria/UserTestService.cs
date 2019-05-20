using EduriaData.Models;
using System.Collections.Generic;
using System.Linq;

namespace Eduria
{
    public class UserTestService
    {
        private EduriaContext Context { get; set; }

        public UserTestService(EduriaContext context)
        {
            Context = context;
        }

        /// <summary>
        /// Add an UserTest with its data to the database.
        /// </summary>
        /// <param name="userTest"></param>
        public void Add(UserTest userTest)
        {
            Context.Add(userTest);
            Context.SaveChanges();
        }

        /// <summary>
        /// Delete an UserTest with its data from the database.
        /// </summary>
        /// <param name="id"></param>
        public void Delete(int id)
        {
            UserTest userTest = Context.UserTests.Find(id);
            if (userTest != null)
            {
                Context.Remove(userTest);
                Context.SaveChanges();
            }
        }

        /// <summary>
        /// Get all the UserTest data from the database.
        /// </summary>
        /// <returns>An list with the data.</returns>
        public IEnumerable<UserTest> GetAll()
        {
            return Context.UserTests;
        }

        public UserTest GetById(int id)
        {
            return GetAll().FirstOrDefault(usertest => usertest.Id == id); 
        }
    }
}
