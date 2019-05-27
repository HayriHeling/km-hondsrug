using EduriaData.Models;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;

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
            return Context.Users.FirstOrDefault(x => x.UserId == id);
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

        /// <summary>
        /// Get a user by the specified email.
        /// </summary>
        /// <param name="studNum">The student number from the user.</param>
        /// <returns>The user with the specific student number</returns>
        public User GetUserByEmail(string email)
        {
            return Context.Users.FirstOrDefault(x => x.Email == email);
        }

        /// <summary>
        /// Get a user by the specified email.
        /// </summary>
        /// <param name="token">The student number from the user.</param>
        /// <returns>The user with the specific student number</returns>
        public User GetUserByToken(string token)
        {
            return Context.Users.FirstOrDefault(x => x.Token == token);
        }

        public void SetUserToken(string userMail, string token)
        {
            User user = Context.Users.First(x => x.Email == userMail);
            user.Token = token;
            Context.SaveChanges();
        }

        public void SetPassword(User user)
        {
            Context.Entry(user).State = EntityState.Modified;
            Context.SaveChanges();
        }
    }
}
