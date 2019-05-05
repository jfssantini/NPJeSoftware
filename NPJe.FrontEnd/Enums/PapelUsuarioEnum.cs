using System.ComponentModel;

namespace NPJe.FrontEnd.Enums
{
    public enum PapelUsuarioEnum : int
    {
        [Description("ADMINISTRADOR")]
        Administrador = -1,

        [Description("ALUNO")]
        Aluno = -2,
    }
}