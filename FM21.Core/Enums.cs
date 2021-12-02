using System.ComponentModel;

namespace FM21.Core
{
    public enum ResultType
    {
        Success = 0,
        Info = 1,
        Warning = 2,
        Error = 3
    }

    public enum RoleName
    {
        Admin = 1,
    }

    public enum PermissionName
    {
        [Description("Manage formula instruction list")]
        ManageFormulaInstruction = 2,
        [Description("Manage customer name list")]
        ManageCustomer = 3,
        [Description("Manage supplier name list")]
        ManageSupplier = 4,
        [Description("Edit Formula Ingredients")]
        EditFormulaIngredients = 5,
        [Description("Edit Formula Sub Designators/Instructions/Ingredient Order")]
        EditFormulaSubDesignatorsOrIngredient = 6,
        [Description("Edit formula attributes")]
        EditFormulaAttributes = 7,
        [Description("Delete/Archive a formula")]
        DeleteOrArchiveFormula = 8,
        [Description("Edit Ingredient data")]
        EditIngredient = 9,
        [Description("Delete/Archive an Ingredient")]
        DeleteOrArchiveIngredient = 10,
        [Description("Edit Food Safety data")]
        EditFoodSafetyData = 11,
        [Description("Print Batch Sheets: Production/Lab")]
        PrintBatchSheetsProductionOrLab = 12,
        [Description("View/Print formula reports")]
        ViewPrintformulareports = 13,
        [Description("View/Print ingredient reports")]
        ViewPrintingredientreports = 14
    }

    public enum SiteCode
    {
        ONT = 1,
        LAC = 2,
        ANJ = 3,
        ANA = 4,
        SLC = 5
    }

    public enum CodeType
    {
        Research_J, //Bar
        Research_K, //Powder
        Research_V, //VM
        Research_RF //Reference Food
    }

    public enum FAOProtein
    {
        Lyc = 58,
        Thr = 34,
        Leu = 66,
        Try = 11,
        Phe_Tyr_His = 74,
        Met_Cys = 25,
        Val = 35,
        Ile = 28
    }
}