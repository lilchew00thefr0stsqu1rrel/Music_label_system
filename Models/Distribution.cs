using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MvcMusicDistr.Models
{
    public enum ReleaseType
    {
        Single, EP, Album
    }
    public class Distribution
    {
        [Key]
        public int Id { get; set; }
        [DisplayName("Дата документа")]
        public DateTime Date { get; set; }
        [Required]
        [DisplayName("Исполнитель")]
        public string? Performer { get; set; }

        [DisplayName("Тип релиза")]
        public ReleaseType? ReleaseType { get; set; }
        [RegularExpression(@"^\d+(\.\d{1,2})?$")]
        [DisplayName("Сумма")]
        public decimal Value { get; set; }
   
    }
}
