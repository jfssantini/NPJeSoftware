using NPJe.FrontEnd.Configs;
using NPJe.FrontEnd.Enums;

namespace NPJe.FrontEnd.Dtos
{
    public class GrupoDto
    {
        public long Id { get; set; }

        public EspecialidadeEnum IdEspecialidade { get; set; }

        public int Numero { get; set; }

        public string Especialidade { get { return IdEspecialidade.GetDescription(); } set { } }

        public int QuantidadeAlunos { get; set; }
    }
}