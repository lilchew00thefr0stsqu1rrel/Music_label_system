using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;

namespace MvcMusicDistr.Models
{
    public class Gait
    {
        public int Id { get; set; }
        [Required]
        public string? Name { get; set; }

        [ForeignKey("Pair")]
        public int PairID { get; set; }

        [BindProperty]
        public ICollection<Pair>? Pairs { get; set; }
    }
}
