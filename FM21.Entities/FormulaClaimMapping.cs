using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FM21.Entities
{
    [Table("FormulaClaimMapping")]
    public class FormulaClaimMapping
    {
        [Key]
        public int FormulaClaimMapID { get; set; }
        public int FormulaID { get; set; }
        public int ClaimID { get; set; }
        public string Description { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public int? UpdatedBy { get; set; }
        public DateTime? UpdatedOn { get; set; }

        public virtual ClaimMaster ClaimMaster { get; set; }
        public virtual FormulaMaster FormulaMaster { get; set; }
    }
}