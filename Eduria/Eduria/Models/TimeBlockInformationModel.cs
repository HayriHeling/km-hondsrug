using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Eduria.Models
{
    public class TimeBlockInformationModel
    {
        public int TimeBlockInformationId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public bool BeforeChrist { get; set; }
        public int Year { get; set; }
        public List<MediaSourceModel> MediaSourceModels { get; set; }
    }
}
