using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ShopPC.Models
{
    public class PcBuild: Products
    {
        //1 pcBuild có nhiều pcBuildItem
        public ICollection<PcBuildItem> pcBuildItem { get; set; } = new List<PcBuildItem>();
    }
}
