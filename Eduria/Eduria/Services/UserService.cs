using EduriaData.Models;
using System.Collections.Generic;
using System.Linq;

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
        /// <param name="userNum">The student number from the user.</param>
        /// <returns>The user with the specific student number</returns>
        public User GetUserByStudNum(int userNum)
        {
            return Context.Users.FirstOrDefault(x => x.UserNum == userNum);
        }

        public IEnumerable<User> GetAllUsersByUserType(int userType)
        {
            return GetAll().Where(ut => ut.UserType == userType);
        }

        public User GetUserByEmail(string email)
        {
            return Context.Users.FirstOrDefault(x => x.Email.ToLower() == email.ToLower());
        }
    }
}
