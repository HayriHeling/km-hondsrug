﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Eduria.Models
{
    public class AnalyticDataAndUsersModel
    {
        public List<EduriaData.Models.User> UserModels { get; set; }
        public List<AnalyticDataModel> AnalyticDataModels { get; set; }
    }
}
