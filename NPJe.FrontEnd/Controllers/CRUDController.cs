using NPJe.FrontEnd.Repository.Context;
using System.Linq;
using System.Web.Http;
using System.Web.Http.Cors;
using System.Linq.Dynamic;
using NPJe.FrontEnd.Repository.Queries;
using NPJe.FrontEnd.Dtos;
using Newtonsoft.Json;
using static NPJe.FrontEnd.Repository.Queries.QueriesRepository;
using System.Collections.Generic;

namespace NPJe.FrontEnd.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class CRUDController : ApiController
    {
        [HttpGet]
        public RetornoDto GetAlunoDtoGrid(int draw, int start, int length, string search, string order, string dir)
        {
            return new QueriesRepository().GetAlunoDtoGrid(draw, start, length, search, order, dir);
        }

        [HttpGet]
        public RetornoDto GetGrupoDtoGrid(int draw, int start, int length, string search, string order, string dir)
        {
            return new QueriesRepository().GetGrupoDtoGrid(draw, start, length, search, order, dir);
        }
        
        [HttpGet]
        public GrupoDto GetGrupoDto(long id)
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

        [HttpGet]
        public bool SaveDisponibilidade(string values)
        {
            var dto = JsonConvert.DeserializeObject<EspecialidadeDto>(values);
            if (dto.Id > 0)
                return new QueriesRepository().EditDisponibilidade(dto);
            else
                return new QueriesRepository().SaveDisponibilidade(dto);
        }

        [HttpGet]
        public RetornoDto GetEspecialidadeAlunoGrid(string order, string dir, long? idAluno = 0, bool consultar = false)
        {
            if(consultar)
                return new QueriesRepository().GetEspecialidadeAlunoGrid(order, dir, idAluno == 0 ? null : idAluno);
            return new RetornoDto() { data = new List<EspecialidadeDto>(), recordsFiltered = 0 };
        }

        [HttpGet]
        public RetornoDto GetGruposAlunoGrid(string order, string dir, long? idAluno = 0, bool consultar = false)
        {
            if (consultar)
                return new QueriesRepository().GetEspecialidadeAlunoGrid(order, dir, idAluno == 0 ? null : idAluno);
            return new RetornoDto() { data = new List<EspecialidadeDto>(), recordsFiltered = 0 };
        }
    }

    
}