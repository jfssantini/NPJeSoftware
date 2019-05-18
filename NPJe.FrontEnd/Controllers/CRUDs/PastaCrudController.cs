using Newtonsoft.Json;
using NPJe.FrontEnd.Configs;
using NPJe.FrontEnd.Dtos;
using NPJe.FrontEnd.Models;
using NPJe.FrontEnd.Repository.Queries;
using System.Collections.Generic;
using System.Web.Http;
using System.Web.Http.Cors;

namespace NPJe.FrontEnd.Controllers.CRUDs
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class PastaCrudController : ApiController
    {
        [HttpGet]
        public RetornoDto GetPastaDtoGrid(int draw, int start, int length, string search, string order, string dir)
        {
            return new PastaRepository().GetPastaDtoGrid(draw, start, length, search, order, dir);
        }

        [HttpGet]
        public bool SavePasta(string values)
        {
            var dto = JsonConvert.DeserializeObject<PastaDto>(values);
            if (dto.Id > 0)
                return new PastaRepository().EditPasta(dto);
            else
                return new PastaRepository().SavePasta(dto);
        }

        [HttpGet]
        public bool ExcluirRegistrosInconsistentes()
        {
            return new PastaRepository().ExcluirRegistrosInconsistentes();
        }


        [HttpGet]
        public PastaDto GetPastaDto(long id)
        {
            return new PastaRepository().GetPastaDto(id);
        }


        [HttpGet]
        public RetornoDto GetAtendimentoGrid(string order, string dir, long? idPasta = 0, bool consultar = false)
        {
            if (consultar)
                return new PastaRepository().GetAtendimentoGrid(order, dir, idPasta == 0 ? null : idPasta);
            return new RetornoDto() { data = new List<AtendimentoDto>(), recordsFiltered = 0 };
        }

        [HttpGet]
        public RetornoComboDto GetGrupoComboDto(long? id = null, string search = null)
        {
            return new GrupoRepository().GetGrupoComboDto(id, search, true);
        }

        [HttpGet]
        public RetornoComboDto GetClienteComboDto(long? id = null, string search = null)
        {
            return new ClienteRepository().GetClienteComboDto(id, search);
        }

        [HttpGet]
        public RetornoComboDto GetUsuarioResponsavelComboDto(long? id = null, string search = null)
        {
            return new ResponsavelRepository().GetUsuarioResponsavelComboDto(id, search);
        }

        [HttpGet]
        public bool SaveAtendimento(string values)
        {
            var dto = JsonConvert.DeserializeObject<AtendimentoDto>(values);
            if (dto.Id > 0)
                return new PastaRepository().EditAtendimento(dto);
            else
                return new PastaRepository().SaveAtendimento(dto);
        }

        [HttpGet]
        public RetornoComboDto GetAssuntoComboDto(long? id = null, string search = null)
        {
            return new PastaRepository().GetAssuntoComboDto(id, search);
        }

        [HttpGet]
        public long SaveAssunto(string values)
        {
            var entity = JsonConvert.DeserializeObject<Assunto>(values);
            return new PastaRepository().SaveAssunto(entity);
        }

        [HttpGet]
        public bool RemoveAtendimento(string values)
        {
            var dto = JsonConvert.DeserializeObject<AtendimentoDto>(values);
            return new PastaRepository().RemoveAtendimento(dto.Id);
        }
    }
}