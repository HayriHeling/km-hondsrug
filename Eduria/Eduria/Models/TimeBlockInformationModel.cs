using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Eduria.Models
{
    public class TimeBlockInformationModel
    {
        public int TimeBlockInformationId { get; set; }
        [Display(Name = "Titel")]
        [DataType(DataType.Text)]
        [Required(ErrorMessage = "Een gebeurtenis moet een titel bevatten.")]
        public string Name { get; set; }
        [Display(Name = "Beschrijving")]
        [DataType(DataType.MultilineText)]
        [Required(ErrorMessage = "Een gebeurtenis moet een beschrijving bevatten.")]
        public string Description { get; set; }
        [Display(Name = "voor/na Christus")]
        public ChristNotation BeforeChrist { get; set; }
        [Display(Name = "Jaar")]
        [Required(ErrorMessage = "Een gebeurtenis moet een jaartal bevatten. Vul een schatting in wanneer jaartal niet duidelijk is.")]
        public int Year { get; set; }
        [Display(Name = "Tijdvak")]
        [Required]
        public TimeTableModel TimeTable { get; set; }
        public List<MediaSourceModel> MediaSourceModels { get; set; }
    }
}
