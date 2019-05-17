using NPJe.FrontEnd.Enums;
using System;

namespace NPJe.FrontEnd.Models
{
    public class Atendimento
    {
        public long Id { get; set; }

        public long? IdPasta { get; set; }

        public Pasta Pasta { get; set; }

        public string Titulo { get; set; }

        public string Descricao { get; set; }

        public SituacaoNpjEnum? IdSituacaoNpj { get; set; }

        public SituacaoProjudiEnum? IdSituacaoProjudi { get; set; }

        public SituacaoAtendimentoEnum? IdSituacaoAtendimento { get; set; }

        public long? IdProcesso { get; set; }

        public Processo Processo { get; set; }

        public long IdUsuarioRegistro { get; set; }

        public Usuario UsuarioRegistro { get; set; }

        public DateTime DataHoraRegistro { get; set; }

        public DateTime DataHoraAlteracao { get; set; }

        public long? IdUsuarioExclusao { get; set; }

        public Usuario UsuarioExclusao { get; set; }

        public DateTime? DataHoraExclusao { get; set; }


        /*****Para Controle do sistema*****/
        public bool Temporario { get; set; }

        public bool Esconder { get; set; }

        public bool Excluir { get; set; }

        public long IdUsuario { get; set; }

        public Usuario Usuario { get; set; }
    }
}