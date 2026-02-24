using System.ComponentModel.DataAnnotations;

namespace MvcMusicDistr.Models.DistribModels
{
    public class DistributionDateGroup
    {
        [DataType(DataType.Date)]
        public int Month { get; set; }

        public decimal TotalValue { get; set; }
    }
}
