using NPJe.FrontEnd.Enums;
using System;
using System.Collections.Generic;

namespace NPJe.FrontEnd.Models
{
    public class Atendimento
    {
        public Atendimento()
        {
            EtapasAtendimento = new List<RegistroEtapaAtendimento>();
        }

        public long Id { get; set; }

        public long IdCliente { get; set; }

        public Cliente Cliente { get; set; }

        public long IdGrupo { get; set; }

        public Grupo Grupo { get; set; }

        public long IdUsuarioCriacao { get; set; }

        public Usuario UsuarioCriacao { get; set; }

        public DateTime DataHoraCriacao { get; set; }

        public long IdUsuarioResponsavel { get; set; }

        public Usuario UsuarioResponsavel { get; set; }

        public List<RegistroEtapaAtendimento> EtapasAtendimento { get; set; }

        public EtapaAtendimentoEnum IdEtapaAtendimentoAtual { get; set; }

        public long? IdTipoAtendimento { get; set; }

        public TipoAtendimento TipoAtendimento { get; set; }

        public bool Concluido { get; set; }
    }
}