using System.Collections.Generic;

namespace NPJe.FrontEnd.Dtos.Relatorios
{
    public class RelatorioProcessoDto
    {

        public RelatorioProcessoDto()
        {
            QuantidadeAbertaMes = new List<int>();
            QuantidadeConcluidaMes = new List<int>();
        }

        public int QuantidadeProcessos { get; set; }

        public int QuantidadeProcessosConcluidos { get; set; }

        public int QuantidadeProcessosParados15Dias { get; set; }

        public int QuantidadeProcessosParados30Dias { get; set; }

        public List<int> QuantidadeAbertaMes { get; set; }

        public List<int> QuantidadeConcluidaMes { get; set; }
    }
}