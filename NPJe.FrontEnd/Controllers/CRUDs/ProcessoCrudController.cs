﻿using Newtonsoft.Json;
using NPJe.FrontEnd.Configs;
using NPJe.FrontEnd.Dtos;
using NPJe.FrontEnd.Dtos.Relatorios;
using NPJe.FrontEnd.Models;
using NPJe.FrontEnd.Repository.Queries;
using NPJe.FrontEnd.Validations;
using System.Collections.Generic;
using System.Web.Http;
using System.Web.Http.Cors;

namespace NPJe.FrontEnd.Controllers.CRUDs
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class ProcessoCrudController : ApiController
    {
        [HttpGet]
        public RetornoDto GetProcessoDtoGrid(int draw, int start, int length, string search, string order, string dir,
            bool somenteConcluidos, bool processos15dias, bool processos30dias, string dataDistribuicao, 
            long? idCliente = null, long? idGrupo = null, int? idSituacaoNpj = null, int? idSituacaoProjudi = null)
        {
            return new ProcessoRepository().GetProcessoDtoGrid(draw, start, length, search, order, dir,
                somenteConcluidos, processos15dias, processos30dias, dataDistribuicao, 
                idCliente, idGrupo, idSituacaoNpj, idSituacaoProjudi);
        }

        [HttpGet]
        public bool SaveProcesso(string values)
        {
            var dto = JsonConvert.DeserializeObject<ProcessoDto>(values);

            new ProcessoValidator().Validate(dto, false);

            if (dto.Id > 0)
                return new ProcessoRepository().EditProcesso(dto);
            else
                return new ProcessoRepository().SaveProcesso(dto);
        }

        [HttpGet]
        public bool ExcluirRegistrosInconsistentes()
        {
            return new ProcessoRepository().ExcluirRegistrosInconsistentes();
        }

        [HttpGet]
        public ProcessoDto GetProcessoDto(long id)
        {
            return new ProcessoRepository().GetProcessoDto(id);
        }

        [HttpGet]
        public bool RemoveProcesso(string values)
        {
            var dto = JsonConvert.DeserializeObject<ProcessoDto>(values);
            new ProcessoValidator().Validate(dto, true);
            return new ProcessoRepository().RemoveProcesso(dto);
        }

        [HttpGet]
        public RetornoDto GetAtendimentoGrid(string order, string dir, long? idProcesso = 0, bool consultar = false)
        {
            if (consultar)
                return new ProcessoRepository().GetAtendimentoGrid(order, dir, idProcesso == 0 ? null : idProcesso);
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

            new ProcessoValidator().Validate(dto, false);

            if (dto.Id > 0)
                return new ProcessoRepository().EditAtendimento(dto);
            else
                return new ProcessoRepository().SaveAtendimento(dto);
        }

        [HttpGet]
        public RetornoComboDto GetTipoAcaoComboDto(long? id = null, string search = null)
        {
            return new ProcessoRepository().GetTipoAcaoComboDto(id, search);
        }

        [HttpGet]
        public long SaveTipoAcao(string values)
        {
            var entity = JsonConvert.DeserializeObject<TipoAcao>(values);
            new ProcessoValidator().Validate(entity, false);
            return new ProcessoRepository().SaveTipoAcao(entity);
        }

        [HttpGet]
        public bool RemoveAtendimento(string values)
        {
            var dto = JsonConvert.DeserializeObject<AtendimentoDto>(values);
            new ProcessoValidator().Validate(dto, true);
            return new ProcessoRepository().RemoveAtendimento(dto.Id);
        }

        [HttpGet]
        public RetornoComboDto GetPastaComboDto(long? id = null, string search = null)
        {
            return new ProcessoRepository().GetPastaComboDto(id, search);
        }

        [HttpGet]
        public RetornoComboDto GetInformacoesPastas(long idPasta)
        {
            return new ProcessoRepository().GetInformacoesPastas(idPasta);
        }

        [HttpGet]
        public RelatorioProcessoDto GetRelatoriosProcessos(long? idAluno = null)
        {
            var repository = new ProcessoRepository();
            var result = repository.GetQuantidadeProcessos(idAluno);

            return result;
        }

        [HttpGet]
        public void SetFiltroProcesso(string value)
        {
            ReturnSession.ReturnObj = value;
        }
    }
}