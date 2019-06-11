﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EduriaData.Models;
using EduriaData.Models.ExamLayer;

namespace Eduria.Services
{
    public class UserEQLogService : AService<UserEQLog>
    {
        public UserEQLogService(EduriaContext context)
        {
            Context = context;
        }
        public override IEnumerable<UserEQLog> GetAll()
        {
            return Context.UserEQLogs;
        }

        public override UserEQLog GetById(int id)
        {
            return Context.UserEQLogs.Find(id);
        }

        public IEnumerable<UserEQLog> GetAllByResultId(int id)
        {
            return Context.UserEQLogs.Where(x => x.ExamResultId == id);
        }
    }
}