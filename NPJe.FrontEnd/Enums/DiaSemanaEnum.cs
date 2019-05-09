using System.ComponentModel;

namespace NPJe.FrontEnd.Enums
{
    public enum DiaSemanaEnum : int
    {
        [Description("Segunda-feira")]
        Segunda = -1,

        [Description("Terça-feira")]
        Terca = -2,

        [Description("Quarta-feira")]
        Quarta = -3,

        [Description("Quinta-feira")]
        Quinta = -4,

        [Description("Sexta-feira")]
        Sexta = -5,

        [Description("Sábado")]
        Sabado = -6,

        [Description("Domingo")]
        Domingo = -7
    }
}