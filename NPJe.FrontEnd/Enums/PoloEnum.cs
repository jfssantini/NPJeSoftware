using System.ComponentModel;

namespace NPJe.FrontEnd.Enums
{
    public enum PoloEnum : int
    {
        [Description("ATIVO")]
        Ativo = -1,

        [Description("PASSIVO")]
        Passivo = -2,

        [Description("TERCEIRO")]
        Terceiro = -3,

        [Description("INTERVENIENTE")]
        Interveniente = -4,
    }
}