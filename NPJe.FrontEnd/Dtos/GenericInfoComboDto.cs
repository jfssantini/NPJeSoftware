using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NPJe.FrontEnd.Dtos
{
    public class GenericInfoComboDto
    {
        public GenericInfoComboDto(int id, string descricao)
        {
            Id = id;
            Descricao = descricao;
        }

        public int Id {  get; set; }

        public string Descricao { get; set; }
    }
}