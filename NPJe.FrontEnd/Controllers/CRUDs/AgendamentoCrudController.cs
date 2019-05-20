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
    public class AgendamentoCrudController : ApiController
    {
        [HttpGet]
        public RetornoDto GetAgendamentoDtoGrid(int draw, int start, int length, string search, string order, string dir,
            bool somenteAlunos, bool somenteDoUsuario, string dataAgendamento, long? idAluno = null)
        {
            return new AgendamentoRepository().GetAgendamentoDtoGrid(draw, start, length, search, order, dir, somenteAlunos, somenteDoUsuario, dataAgendamento, idAluno);
        }

        [HttpGet]
        public AgendamentoDto GetAgendamentoDto(long id)
        {
            return new AgendamentoRepository().GetAgendamentoDto(id);
        }

        [HttpGet]
        public bool SaveAgendamento(string values)
        {
            var dto = JsonConvert.DeserializeObject<AgendamentoDto>(values);

            new AgendamentoValidator().Validate(dto);

            if (dto.Id > 0)
                return new AgendamentoRepository().EditAgendamento(dto);
            else
                return new AgendamentoRepository().SaveAgendamento(dto);
        }

        [HttpGet]
        public bool RemoveAgendamento(string values)
        {
            var dto = JsonConvert.DeserializeObject<AgendamentoDto>(values);

            return new AgendamentoRepository().RemoveAgendamento(dto.Id);
        }

        [HttpGet]
        public RetornoComboDto GetPastaComboDto(long? id = null, string search = null)
        {
            return new AgendamentoRepository().GetPastaComboDto(id, search);
        }
        
        [HttpGet]
        public RetornoComboDto GetAlunoComboDto(long? id = null, string search = null)
        {
            return new AlunoRepository().GetAlunoComboDto(id, search);
        }

        [HttpGet]
        public RetornoComboDto GetProcessoComboDto(long? id = null, string search = null, long? idPasta = null)
        {
            return idPasta.HasValue ? new AgendamentoRepository().GetProcessoComboDto(id, search, idPasta.Value) : new RetornoComboDto() { total = 0 };
        }

        [HttpGet]
        public List<GenericInfoComboDto> GetAtendimentosByIsuario()
        {
            return new AgendamentoRepository().GetAgendamentosByIsuario();
        }

        [HttpGet]
        public void GetAgendamentoDtoAuto(long value)
        {
            var dto = new AgendamentoRepository().GetAgendamentoDto(value);
            ReturnSession.ReturnObj = dto;
        }
    }
}