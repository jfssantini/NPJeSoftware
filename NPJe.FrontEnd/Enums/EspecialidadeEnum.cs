using System.ComponentModel;

namespace NPJe.FrontEnd.Enums
{
    public enum EspecialidadeEnum : int
    {
        [Description("CONSEMMA")]
        Consemma = -1,

        [Description("CONCILIARE")]
        Conciliare = -2,

        [Description("LIBERTARE")]
        Libertare = -3,

        [Description("PROCON")]
        Procon = -4,

        [Description("SAJ")]
        Saj = -5,
    }
}