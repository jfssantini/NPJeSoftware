using NPJe.FrontEnd.Configs;
using NPJe.FrontEnd.Enums;

namespace NPJe.FrontEnd.Dtos
{
    public class AlunoGrupoDto
    {
        public long Id { get; set; }

        public long? IdAluno { get; set; }

        public long IdGrupo { get; set; }

        public int Numero { get; set; }

        public EspecialidadeEnum IdEspecialidadeGrupo { get; set; }

        public string DescricaoGrupo { get { return Numero + " - " + IdEspecialidadeGrupo.GetDescription(); } set { } }
    }
}