using NPJe.FrontEnd.Configs;
using NPJe.FrontEnd.Dtos;
using System;

namespace NPJe.FrontEnd.Validations
{
    public class AgendamentoValidator
    {
        public void Validate(AgendamentoDto dto)
        {
            if(dto.DescricaoDataAgendamento.IsNullOrEmpty())
                throw new ApplicationException("Agendamento: O campo 'Data' é obrigatório..");
        }
    }
}