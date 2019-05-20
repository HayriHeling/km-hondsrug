using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Eduria.Interfaces
{
    public interface IQuestion
    {
        int Id { get; set; }
        string Text { get; set; }

        IActionResult GetView();
    }
}
