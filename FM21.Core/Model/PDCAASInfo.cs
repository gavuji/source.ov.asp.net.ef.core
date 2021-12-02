using System.Collections.Generic;

namespace FM21.Core.Model
{
    public class PDCAASInfo
    {
        public PDCAASInfo()
        {
            AminoAcids = new List<PDCAASNutrient>();
            AminoAcids.Add(new PDCAASNutrient() { NutrientShortName = "Lyc", NutrientName = NutrientName.L_Lysine_Lys, FAOProtein = FAOProtein.Lyc.GetHashCode() });
            AminoAcids.Add(new PDCAASNutrient() { NutrientShortName = "Thr", NutrientName = NutrientName.L_Threonine_Thr, FAOProtein = FAOProtein.Thr.GetHashCode() });
            AminoAcids.Add(new PDCAASNutrient() { NutrientShortName = "Leu", NutrientName = NutrientName.L_Leucine_Leu, FAOProtein = FAOProtein.Leu.GetHashCode() });
            AminoAcids.Add(new PDCAASNutrient() { NutrientShortName = "Try", NutrientName = NutrientName.L_Tryptophan_Trp, FAOProtein = FAOProtein.Try.GetHashCode() });
            AminoAcids.Add(new PDCAASNutrient() { NutrientShortName = "Phe/Tyr/His", NutrientName = string.Format("{0}, {1}, {2}", NutrientName.L_Phenylalanine_Phe, NutrientName.L_Tyrosine_Tyr, NutrientName.L_Histidine_His), FAOProtein = FAOProtein.Phe_Tyr_His.GetHashCode() });
            AminoAcids.Add(new PDCAASNutrient() { NutrientShortName = "Met/Cys", NutrientName = string.Format("{0}, {1}", NutrientName.L_Methionine_Met, NutrientName.L_Cystine_Cys), FAOProtein = FAOProtein.Met_Cys.GetHashCode() });
            AminoAcids.Add(new PDCAASNutrient() { NutrientShortName = "Val", NutrientName = NutrientName.L_Valine_Val, FAOProtein = FAOProtein.Val.GetHashCode() });
            AminoAcids.Add(new PDCAASNutrient() { NutrientShortName = "Ile", NutrientName = NutrientName.L_Isoleucine_Ile, FAOProtein = FAOProtein.Ile.GetHashCode() });
            ProteinAmount = 0;
        }
        public List<PDCAASNutrient> AminoAcids { get; set; }
        public decimal? ProteinDigestibility { get; set; }
        public decimal PDCAAS { get; set; }
        public decimal ProteinAmount { get; set; }
        public decimal AdjustedProtein { get; set; }
        public decimal RDIPercent { get; set; }
        public string MissingAAInfo { get; set; }
        public string MissingDigestInfo { get; set; }
    }
}