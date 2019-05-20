using NPJe.FrontEnd.Enums;
using System;
using System.Collections.Generic;

namespace NPJe.FrontEnd.Models
{
    public class Grupo
    {
        public Grupo()
        {
            Alunos = new List<AlunoGrupo>();
        }

        public long Id { get; set; }

        public int Numero { get; set; }

        public EspecialidadeEnum IdEspecialidade { get; set; }

        public string Observacoes { get; set; }

        public long? IdUsuarioExclusao { get; set; }

        public Usuario UsuarioExclusao { get; set; }

        public DateTime? DataHoraExclusao { get; set; }

        public List<AlunoGrupo> Alunos { get; set; }
    }
}