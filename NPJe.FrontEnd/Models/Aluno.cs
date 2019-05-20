using System;
using System.Collections.Generic;

namespace NPJe.FrontEnd.Models
{
    public class Aluno
    {
        public Aluno()
        {
            Especialidades = new List<AlunoEspecialidade>();
            Grupos = new List<AlunoGrupo>();
        }

        public long Id { get; set; }

        public string Nome { get; set; }

        public int Sexo { get; set; }

        public DateTime DataNascimento { get; set; }

        public string Telefone { get; set; }

        public string Celular { get; set; }

        public string Email { get; set; }

        public string Matricula { get; set; }

        public int Semestre { get; set; }

        public string RG { get; set; }

        public string CPF { get; set; }

        public long IdUsuario { get; set; }

        public Usuario Usuario { get; set; }

        public long? IdResponsavel { get; set; }

        public Responsavel Responsavel { get; set; }

        public bool Ativo { get; set; }

        public long? IdUsuarioExclusao { get; set; }

        public Usuario UsuarioExclusao { get; set; }

        public DateTime? DataHoraExclusao { get; set; }

        public List<AlunoEspecialidade> Especialidades { get; set; }

        public List<AlunoGrupo> Grupos { get; set; }

    }
}