using System.Collections.Generic;

namespace FM21.Core.Model
{
    public class FormulaModel
    {
        public FormulaMasterModel FormulaMaster { get; set; }
        public List<FormulaDetailsModel> FormulaDetails { get; set; }
        public string[] AttributeCaption { get; set; }
        public List<ClaimModel> ClaimInfo { get; set; }
        public int[] CriteriaInfo { get; set; }
        public List<FormulaStatusUpdate> lstFormulaStatus { get; set; }
    }
}