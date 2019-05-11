﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NPJe.FrontEnd.Dtos
{
    public class GenericInfoComboDto
    {
        public GenericInfoComboDto()
        {

        }

        public GenericInfoComboDto(long id, string descricao)
        {
            this.id = id;
            text = descricao;
        }

        public long id {  get; set; }

        public string text { get; set; }
    }
}