using Newtonsoft.Json;
using NPJe.FrontEnd.Configs;
using NPJe.FrontEnd.Dtos;
using NPJe.FrontEnd.Repository.Queries;
using NPJe.FrontEnd.Validations;
using System.Web.Http;
using System.Web.Http.Cors;

namespace NPJe.FrontEnd.Controllers.CRUDs
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class GrupoCrudController : ApiController
    {
        [HttpGet]
        public RetornoDto GetGrupoDtoGrid(int draw, int start, int length, string search, string order, string dir)
        {
            return new GrupoRepository().GetGrupoDtoGrid(draw, start, length, search, order, dir);
        }

        [HttpGet]
        public GrupoDto GetGrupoDto(long id)
        {
            return new GrupoRepository().GetGrupoDto(id);
        }

        [HttpGet]
        public bool SaveGrupo(string values)
        {
            var dto = JsonConvert.DeserializeObject<GrupoDto>(values);
            new GrupoValidator().Validate(dto, false);
            if (dto.Id > 0)
                return new GrupoRepository().EditGrupo(dto);
            else
                return new GrupoRepository().SaveGrupo(dto);
        }

        [HttpGet]
        public bool RemoveGrupo(string values)
        {
            var dto = JsonConvert.DeserializeObject<GrupoDto>(values);
            new GrupoValidator().Validate(dto, true);
            return new GrupoRepository().RemoveGrupo(dto.Id);
        }

    }
}