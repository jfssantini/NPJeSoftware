using NPJe.FrontEnd.Repository.Context;
using System.Linq;
using System.Web.Http;
using System.Web.Http.Cors;
using System.Linq.Dynamic;
using NPJe.FrontEnd.Repository.Queries;
using NPJe.FrontEnd.Dtos;
using Newtonsoft.Json;

namespace NPJe.FrontEnd.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class CRUDController : ApiController
    {
        [HttpGet]
        public object GetAlunoDtoGrid(int draw, int start, int length, string search, string order, string dir)
        {
            return new QueriesRepository().GetAlunoDtoGrid(draw, start, length, search, order, dir);
        }

        [HttpGet]
        public object GetGrupoDtoGrid(int draw, int start, int length, string search, string order, string dir)
        {
            return new QueriesRepository().GetGrupoDtoGrid(draw, start, length, search, order, dir);
        }
        
        [HttpGet]
        public object GetGrupoDto(long id)
        {
            return new QueriesRepository().GetGrupoDto(id);
        }

        [HttpGet]
        public bool SaveGrupo(string values)
        {
            var dto = JsonConvert.DeserializeObject<GrupoDto>(values);
            if(dto.Id > 0)
                return new QueriesRepository().EditGrupo(dto);
            else
                return new QueriesRepository().SaveGrupo(dto);               
        }

        [HttpGet]
        public bool RemoveGrupo(string values)
        {
            var dto = JsonConvert.DeserializeObject<GrupoDto>(values);

            return new QueriesRepository().RemoveGrupo(dto.Id);
        }
    }

    
}