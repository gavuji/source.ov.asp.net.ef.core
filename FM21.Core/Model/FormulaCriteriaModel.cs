namespace FM21.Core.Model
{
    public class FormulaCriteriaModel
    {
        public int FormulaID { get; set; }
        public int CriteriaID { get; set; }
        public int? FormulaCriteriaMapID { get; set; }
        public string CriteriaDescription { get; set; }
        public string ColorCode { get; set; }
        public string CriteriaOrder { get; set; }
    }
}