using System;

namespace NPJe.FrontEnd.Dtos
{
    public class AlunoDto
    {
        public long Id { get; set; }

        public string Usuario { get; set; }

        public string Senha { get; set; }

        public string Nome { get; set; }

        public int Sexo { get; set; }

        public DateTime DataNascimento { get; set; }

        public string DescricaoDataNascimento { get; set; }

        public string Matricula { get; set; }

        public int Semestre { get; set; }

        public string CPF { get; set; }

        public string Telefone { get; set; }

        public string Celular { get; set; }

        public string Email { get; set; }

        public long IdUsuario { get; set; }

        public long? IdResponsavel { get; set; }

        public bool Ativo { get; set; }
    }
}