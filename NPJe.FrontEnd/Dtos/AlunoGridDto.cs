using NPJe.FrontEnd.Configs;
using NPJe.FrontEnd.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NPJe.FrontEnd.Dtos
{
    public class AlunoGridDto
    {
        public long Id { get; set; }

        public string Nome { get; set; }

        public string Grupo { get { return string.Join(", ", Grupos); } set { } }

        public string Especialidade { get { return string.Join(", ", DescricaoEspecialidades); } set { } }

        public int Sexo { get; set; }

        public string DescricaoSexo { get { return Sexo == 1 ? "Masculino" : "Feminino"; } set { } }

        public DateTime DataNascimento { get; set; }

        public string DescricaoDataNascimento { get { return DataNascimento.ToString("dd/MM/yyyy"); } set { } }

        public string Matricula { get; set; }

        public List<string> Grupos { get; set; }

        public List<EspecialidadeEnum> Especialidades { get; set; }

        public List<string> DescricaoEspecialidades { get { return Especialidades.Select(x => x.GetDescription()).ToList(); } set { } }
    }
}