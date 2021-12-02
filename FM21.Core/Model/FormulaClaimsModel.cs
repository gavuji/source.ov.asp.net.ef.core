namespace FM21.Core.Model
{
    public class FormulaClaimsModel
    {
        public int FormulaID { get; set; }
        public int ClaimID { get; set; }
        public int? FormulaClaimMapID { get; set; }
        public string ClaimCode { get; set; }
        public string ClaimDescription { get; set; }
        public string ClaimGroupType { get; set; }
        public bool HasImpact { get; set; }
        public string Description { get; set; }
    }
}