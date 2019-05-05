using System.ComponentModel;

namespace NPJe.FrontEnd.Enums
{
    public enum EspecialidadeEnum : int
    {
        [Description("SAJ - SERVIÇO DE ASSIST. JURÍDICA")]
        SAJ = -1,

        [Description("CONCILIARE")]
        Conciliare = -2,

        [Description("LIBERTARE")]
        Libertare = -3,

        [Description("CONSEMMA")]
        Consemma = -4,
    }
}