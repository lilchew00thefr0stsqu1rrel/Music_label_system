using System.ComponentModel.DataAnnotations;

namespace MvcMusicDistr.Models;

public class Beat
{
    public int Id { get; set; }
    [Required]
    public string? Name { get; set; }
    [Required]
    public string? Beatmaker { get; set; }
    //[DataType(DataType.Date)]
    //public DateTime ReleaseDate { get; set; }
    public string? Tonality { get; set; }
    public int BPM { get; set; }
    public decimal Price { get; set; }
    //[Required]
   
    //public string? YetAnotherProducer { get; set; }
    //public string? Beatmaker2 { get; set; }
    //public string? Beatmaker3 { get; set; }
}