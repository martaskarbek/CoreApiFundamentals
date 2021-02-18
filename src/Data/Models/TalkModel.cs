using System.ComponentModel.DataAnnotations;

namespace CoreCodeCamp.Data.Models
{
    public class TalkModel
    {
        public int TalkId { get; set; }
        [Required]
        [StringLength(100)]
        public string Title { get; set; }
        [Required]
        [StringLength(4000, MinimumLength = 20)]
        public string Abstract { get; set; }
        [Range(100, 300)]
        public int Level { get; set; }
        
        public SpeakerModel Speaker { get; set; }
        //I added this line below to avoid error causing by
        //for member in camp profile
        public Camp Camp { get; set; }
    }
}