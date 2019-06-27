using EduriaData.Models;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

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

        /// <summary>
        /// Get a user by the specified Token.
        /// </summary>
        /// <param name="token">The token from the user.</param>
        /// <returns>The user with the specific token</returns>
        public User GetUserByToken(string token)
        {
            return Context.Users.FirstOrDefault(x => x.Token == token);
        }
        /// <summary>
        /// Sets the token value of the given user.
        /// </summary>
        /// <param name="userMail"></param>
        /// <param name="token"></param>
        public void SetUserToken(string userMail, string token)
        {
            User user = Context.Users.First(x => x.Email == userMail);
            user.Token = token;
            Context.SaveChanges();
        }
        /// <summary>
        /// Sets password of given user.
        /// </summary>
        /// <param name="user"></param>
        public void SetPassword(User user)
        {
            Context.Entry(user).State = EntityState.Modified;
            Context.SaveChanges();
        }

        /// <summary>
        /// Gets all users with given usertype
        /// </summary>
        /// <param name="userType"> the type of user</param>
        /// <returns></returns>
        public IEnumerable<User> GetAllUsersByUserType(int userType)
        {
            return GetAll().Where(ut => ut.UserType == userType);
        }

        /// <summary>
        /// Get a user by the specified email.
        /// </summary>
        /// <param name="studNum">The student number from the user.</param>
        /// <returns>The user with the specific email</returns>
        public User GetUserByEmail(string email)
        {          
            return Context.Users.FirstOrDefault(x => x.Email.ToLower() == email.ToLower());
        }

        public int GetLoggedInUserId(ClaimsPrincipal user)
        {
            return int.Parse(user.FindFirst(ClaimTypes.NameIdentifier).Value);
        }
    }
}
