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
        public override IEnumerable<User> GetAll()
        {
            return Context.Users;
        }

        public override User GetById(int id)
        {
            return Context.Users.FirstOrDefault(x => x.Id == id);
        }

        public User GetUserByStudNum(int studNum)
        {
            return Context.Users.FirstOrDefault(x => x.StudNum == studNum);
        }

        public User GetUserByStudNumAndPassword(int studNum, string password)
        {
            return Context.Users.Where(x => x.StudNum == studNum && x.Password == password).FirstOrDefault();
        }
    }
}
