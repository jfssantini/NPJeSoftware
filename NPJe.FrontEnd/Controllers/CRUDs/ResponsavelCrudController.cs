﻿using Newtonsoft.Json;
using NPJe.FrontEnd.Configs;
using NPJe.FrontEnd.Dtos;
using NPJe.FrontEnd.Repository.Queries;
using NPJe.FrontEnd.Validations;
using System.Web.Http;
using System.Web.Http.Cors;

namespace NPJe.FrontEnd.Controllers.CRUDs
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class ResponsavelCrudController : ApiController
    {
        [HttpGet]
        public RetornoDto GetResponsavelDtoGrid(int draw, int start, int length, string search, string order, string dir, 
            bool incluiExcluidos, long? idTipo = null)
        {
            return new ResponsavelRepository().GetResponsavelDtoGrid(draw, start, length, search, order, dir, incluiExcluidos, idTipo);
        }

        [HttpGet]
        public bool SaveResponsavel(string values)
        {
            var dto = JsonConvert.DeserializeObject<ResponsavelDto>(values);

            new ResponsavelValidator().Validate(dto, false);

            if (dto.Id > 0)
                return new ResponsavelRepository().EditResponsavel(dto);
            else
                return new ResponsavelRepository().SaveResponsavel(dto);
        }

        [HttpGet]
        public ResponsavelDto GetResponsavelDto(long id)
        {
            return new ResponsavelRepository().GetResponsavelDto(id);
        }

        [HttpGet]
        public bool RemoveResponsavel(string values)
        {
            var dto = JsonConvert.DeserializeObject<ResponsavelDto>(values);
            new ResponsavelValidator().Validate(dto, true);
            return new ResponsavelRepository().RemoveResponsavel(dto);
        }
    }
}