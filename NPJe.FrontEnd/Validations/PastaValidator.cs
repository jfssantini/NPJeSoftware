using NPJe.FrontEnd.Configs;
using NPJe.FrontEnd.Dtos;
using System;

namespace NPJe.FrontEnd.Validations
{
    public class PastaValidator
    {
        public void Validate(object dto, bool delete)
        {
            if (dto is PastaDto)
                ValidatePasta(dto as PastaDto);
            if (dto is AtendimentoDto)
                ValidateAtendimento(dto as AtendimentoDto);
        }

        private void ValidateAtendimento(AtendimentoDto dto)
        {
            if (!dto.Titulo.IsNullOrEmpty())
                throw new ApplicationException("Atendimento: O campo 'Título' é obrigatório.");
        }

        private void ValidatePasta(PastaDto dto)
        {
            if (dto.IdAssunto == 0)
                throw new ApplicationException("Processo: O campo 'Assunto' é obrigatório.");

            if (dto.IdGrupo == 0)
                throw new ApplicationException("Processo: O campo 'Grupo' é obrigatório.");

            if (dto.IdCliente == 0)
                throw new ApplicationException("Processo: O campo 'Cliente' é obrigatório.");
        }
    }
}