using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Eduria.Models.Interfaces
{
    public interface IQuestion
    {
        IActionResult GetView();
    }
}
