using EduriaData.Models;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Eduria.Services
{
    public class UserService : AService<User>
    {
       public UserService(EduriaContext context)
        {
            Context = context;
        }

        /// <summary>
        /// Get all users from the database.
        /// </summary>
        /// <returns>A List with all users.</returns>
        public override IEnumerable<User> GetAll()
        {
            return Context.Users;
        }

        /// <summary>
        /// Get an specific user. 
        /// </summary>
        /// <param name="id">The id from the user.</param>
        /// <returns>The specific user.</returns>
        public override User GetById(int id)
        {
            return Context.Users.FirstOrDefault(x => x.Id == id);
        }

        /// <summary>
        /// Get a user by the specified student number.
        /// </summary>
        /// <param name="studNum">The student number from the user.</param>
        /// <returns>The user with the specific student number</returns>
        public User GetUserByStudNum(int studNum)
        {
            return Context.Users.FirstOrDefault(x => x.StudNum == studNum);
        }
    }
}
