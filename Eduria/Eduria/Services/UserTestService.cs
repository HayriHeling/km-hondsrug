using Eduria.Interfaces;
using EduriaData.Models;
using System.Collections.Generic;
using System.Linq;

namespace Eduria
{
    public class UserTestService : IService<UserTest>
    {
        public EduriaContext Context { get; set; }

        /// <summary>
        /// Get all the UserTest data from the database.
        /// </summary>
        /// <returns>An list with the data.</returns>
        public IEnumerable<UserTest> GetAll()
        {
            return Context.UserTests;
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
        /// Delete an UserTest wit its data from the database.
        /// </summary>
        /// <param name="userTest"></param>
        public void Delete(UserTest userTest)
        {
            Context.Remove(userTest);
            Context.SaveChanges();
        }

        /// <summary>
        /// Get an UserTest with its data from the database by the specific id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns>The UserTest datamodel.</returns>
        public UserTest GetById(int id)
        {
            return GetAll().FirstOrDefault(usertest => usertest.Id == id);
        }
    }
}
