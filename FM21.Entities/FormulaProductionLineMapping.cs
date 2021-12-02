using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FM21.Entities
{
    [Table("FormulaProductionLineMapping")]
    public class FormulaProductionLineMapping
    {
        [Key]
        public int FormulaProductionLineMapID { get; set; }
        public int FormulaID { get; set; }
        public int ProductionLineID { get; set; }
        public int? ProductionMixerID { get; set; }
        public decimal? Weight { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public int? UpdatedBy { get; set; }
        public DateTime? UpdatedOn { get; set; }

        public virtual FormulaMaster Formula { get; set; }
        public virtual ProductionLineMaster ProductionLine { get; set; }
        public virtual ProductionMixerMaster ProductionMixer { get; set; }
    }
}