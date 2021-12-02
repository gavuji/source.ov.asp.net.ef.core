namespace FM21.Core.Model
{
    public class FormulaChangeCodeParam
    {
        public string FormulaTypeCode { get; set; }
        public int CurrentSiteID { get; set; }
        public int ChangeSiteID { get; set; }
        public string FormulaCode { get; set; }
        public string ServingSize { get; set; }
        public int FormulaID { get; set; }
    }

    public class FormulaStatusUpdate

    {
        public int FormulaID { get; set; }
        public string FormulaStatus { get; set; }
    }

}