using NPJe.FrontEnd.Configs;
using NPJe.FrontEnd.Dtos;
using System;

namespace NPJe.FrontEnd.Validations
{
    public class ProcessoValidator
    {
        public void Validate(object dto, bool delete)
        {
            if (dto is ProcessoDto)
                ValidateProcesso(dto as ProcessoDto);
            if (dto is AtendimentoDto)
                ValidateAtendimento(dto as AtendimentoDto);
        }

        private void ValidateAtendimento(AtendimentoDto dto)
        {
            if (!dto.Titulo.IsNullOrEmpty())
                throw new ApplicationException("Atendimento: O campo 'Título' é obrigatório.");
        }

        private void ValidateProcesso(ProcessoDto dto)
        {
            if(DateTime.TryParse(dto.DescricaoDistribuicao, out DateTime data) && data < DateTime.Now.Date)
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