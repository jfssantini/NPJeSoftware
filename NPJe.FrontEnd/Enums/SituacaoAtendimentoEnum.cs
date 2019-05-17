using System.ComponentModel;

namespace NPJe.FrontEnd.Enums
{
    public enum SituacaoAtendimentoEnum : int
    {
        [Description("AGENDADO")]
        Agendado = -1,

        [Description("AGUARDANDO")]
        Aguardando = -2,

        [Description("DEFERIDO")]
        Deferido = -3,

        [Description("INDEFERIDO")]
        Indeferido = -4,

        [Description("PROJUDI")]
        Projudi = -5
    }
}