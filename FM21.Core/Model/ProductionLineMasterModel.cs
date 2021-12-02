namespace FM21.Core.Model
{
    public class ProductionLineMasterModel
    {
        public int ProductionLineID { get; set; }
        public string LineCode { get; set; }
        public string LineDescription { get; set; }
        public int? ProductionMixerID { get; set; }
        public decimal? BatchWeight { get; set; }
        public int SiteID { get; set; }
    }
}