using System.ComponentModel;

namespace NPJe.FrontEnd.Enums
{
    public enum TipoResponsavelEnum: int
    {
        [Description("RESPONSÁVEL NPJ")]
        ResponsavelNPJ = -1,

        [Description("PROFESSOR")]
        Professor = -2,
    }
}