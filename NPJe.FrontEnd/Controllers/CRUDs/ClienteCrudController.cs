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
    public class ClienteCrudController : ApiController
    {
        [HttpGet]
        public RetornoDto GetClienteDtoGrid(int draw, int start, int length, string search, string order, string dir,
            bool apenasSemProcessoAndamento, bool apenasPF, bool apenasPJ)
        {
            return new ClienteRepository().GetClienteDtoGrid(draw, start, length, search, order, dir,
                apenasSemProcessoAndamento, apenasPF, apenasPJ);
        }

        [HttpGet]
        public ClienteDto GetClienteDto(long id)
        {
            return new ClienteRepository().GetClienteDto(id);
        }

        [HttpGet]
        public bool SaveCliente(string values)
        {
            var dto = JsonConvert.DeserializeObject<ClienteDto>(values);

            new ClienteValidator().Validate(dto, false);

            if (dto.Id > 0)
                return new ClienteRepository().EditCliente(dto);
            else
                return new ClienteRepository().SaveCliente(dto);
        }

        [HttpGet]
        public bool RemoveCliente(string values)
        {
            var dto = JsonConvert.DeserializeObject<ClienteDto>(values);
            new ClienteValidator().Validate(dto, true);
            return new ClienteRepository().RemoveCliente(dto.Id);
        }
    }
}