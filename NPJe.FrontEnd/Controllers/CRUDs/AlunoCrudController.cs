using Newtonsoft.Json;
using NPJe.FrontEnd.Configs;
using NPJe.FrontEnd.Dtos;
using NPJe.FrontEnd.Repository.Queries;
using NPJe.FrontEnd.Validations;
using System.Collections.Generic;
using System.Web.Http;
using System.Web.Http.Cors;

namespace NPJe.FrontEnd.Controllers.CRUDs
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class AlunoCrudController : ApiController
    {
        [HttpGet]
        public RetornoDto GetAlunoDtoGrid(int draw, int start, int length, string search, string order, 
            string dir, bool incluiExcluidos, long? idGrupo = null, long? idEspecialidade = null)
        {
            return new AlunoRepository().GetAlunoDtoGrid(draw, start, length, search, order, dir, incluiExcluidos, idGrupo, idEspecialidade);
        }

        [HttpGet]
        public bool SaveAluno(string values)
        {
            var dto = JsonConvert.DeserializeObject<AlunoDto>(values);

            new AlunoValidator().Validate(dto, false);

            if (dto.Id > 0)
                return new AlunoRepository().EditAluno(dto);
            else
                return new AlunoRepository().SaveAluno(dto);
        }

        [HttpGet]
        public bool ExcluirRegistrosInconsistentes()
        {
            return new AlunoRepository().ExcluirRegistrosInconsistentes();
        }

        [HttpGet]
        public AlunoDto GetAlunoDto(long id)
        {
            return new AlunoRepository().GetAlunoDto(id);
        }

        [HttpGet]
        public bool RemoveAluno(string values)
        {
            var dto = JsonConvert.DeserializeObject<AlunoDto>(values);

            new AlunoValidator().Validate(dto, true);

            return new AlunoRepository().RemoveAluno(dto);
        }

        [HttpGet]
        public bool SaveAlunoEspecialidade(string values)
        {
            var dto = JsonConvert.DeserializeObject<EspecialidadeDto>(values);

            new AlunoValidator().Validate(dto, false);

            if (dto.Id > 0)
                return new AlunoRepository().EditAlunoEspecialidade(dto);
            else
                return new AlunoRepository().SaveAlunoEspecialidade(dto);
        }

        [HttpGet]
        public bool RemoveAlunoEspecialidade(string values)
        {
            var dto = JsonConvert.DeserializeObject<EspecialidadeDto>(values);
            return new AlunoRepository().RemoveAlunoEspecialidade(dto.Id);
        }

        [HttpGet]
        public RetornoDto GetAlunoEspecialidadeGrid(string order, string dir, long? idAluno = 0, bool consultar = false)
        {
            if (consultar)
                return new AlunoRepository().GetAlunoEspecialidadeGrid(order, dir, idAluno == 0 ? null : idAluno);
            return new RetornoDto() { data = new List<EspecialidadeDto>(), recordsFiltered = 0 };
        }

        [HttpGet]
        public RetornoDto GetAlunoGrupoGrid(string order, string dir, long? idAluno = 0, bool consultar = false)
        {
            if (consultar)
                return new AlunoRepository().GetAlunoGrupoGrid(order, dir, idAluno == 0 ? null : idAluno);
            return new RetornoDto() { data = new List<AlunoGrupoDto>(), recordsFiltered = 0 };
        }

        [HttpGet]
        public RetornoComboDto GetGrupoComboDto(long? id = null, string search = null)
        {
            return new GrupoRepository().GetGrupoComboDto(id, search,false);
        }

        [HttpGet]
        public RetornoComboDto GetAlunoComboDto(long? id = null, string search = null)
        {
            return new AlunoRepository().GetAlunoComboDto(id, search);
        }

        [HttpGet]
        public bool SaveAlunoGrupo(string values)
        {
            var dto = JsonConvert.DeserializeObject<AlunoGrupoDto>(values);

            new AlunoValidator().Validate(dto, false);

            if (dto.Id > 0)
                return new AlunoRepository().EditAlunoGrupo(dto);
            else
                return new AlunoRepository().SaveAlunoGrupo(dto);
        }

        [HttpGet]
        public bool RemoveAlunoGrupo(string values)
        {
            var dto = JsonConvert.DeserializeObject<AlunoGrupoDto>(values);
            return new AlunoRepository().RemoveAlunoGrupo(dto.Id);
        }
    }
}