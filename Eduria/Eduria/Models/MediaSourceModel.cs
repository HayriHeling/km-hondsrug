using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Eduria.Models
{
    public class MediaSourceModel
    {
        public int MediaSourceId { get; set; }
        public string Source { get; set; }
        public MediaType MediaType { get; set; }
    }
}
