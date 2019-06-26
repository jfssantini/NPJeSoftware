using NPJe.FrontEnd.Configs;
using NPJe.FrontEnd.Dtos;
using NPJe.FrontEnd.Models;
using System;

namespace NPJe.FrontEnd.Validations
{
    public class ProcessoValidator
    {
        public void Validate(object dto, bool delete)
        {
            if (dto is ProcessoDto)
                ValidateProcesso(dto as ProcessoDto, delete);
            if (dto is AtendimentoDto)
                ValidateAtendimento(dto as AtendimentoDto, delete);
            if (dto is TipoAcao)
                ValidateTipoAcao(dto as TipoAcao, delete);
        }

        private void ValidateTipoAcao(TipoAcao entity, bool delete)
        {
            if (entity.Descricao.IsNullOrEmpty() && !delete)
                throw new ApplicationException("Tipo de ação: O campo 'Descricao' é obrigatório.");
        }

        private void ValidateAtendimento(AtendimentoDto dto, bool delete)
        {
            if (dto.Titulo.IsNullOrEmpty() && !delete)
                throw new ApplicationException("Atendimento: O campo 'Título' é obrigatório.");
        }

        private void ValidateProcesso(ProcessoDto dto, bool delete)
        {
            if (!delete)
            {
                if (!DateTime.TryParse(dto.DescricaoDistribuicao, out DateTime data))
                    throw new ApplicationException("Processo: O campo 'Distribuição' possui um valor inválido.");

                if (dto.IdTipoAcao == 0)
                    throw new ApplicationException("Processo: O campo 'Tipo de ação' é obrigatório.");

                if (dto.IdPasta == 0)
                    throw new ApplicationException("Processo: O campo 'Referente ao caso' é obrigatório.");

                if (!dto.NumeroProcesso.IsNullOrEmpty() && dto.NumeroProcesso.Length < 25)
                    throw new ApplicationException("Processo: O campo 'Número do processo' deve ter 25 caracteres.");
            }
        }
    }
}