using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace MvcMusicDistr.Models
{
    public class Pair
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string? Name { get; set; }

       
        public int OnGroundLeft { get; set; }
        [DisplayName("Right")]
        public int OnGroundRight { get; set; }


        public Gait? Gait { get; set; }


        [ForeignKey("Gait")]
        public int GaitID { get; set; }
    }
}
