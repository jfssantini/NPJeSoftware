using NPJe.FrontEnd.Configs;
using NPJe.FrontEnd.Dtos;
using NPJe.FrontEnd.Enums;
using NPJe.FrontEnd.Repository.Queries;
using System;
using System.Linq;
using System.Text.RegularExpressions;

namespace NPJe.FrontEnd.Validations
{
    public class AlunoValidator
    {
        private string ValidEmailPattern = @"^(?!\.)(""([^""\r\\]|\\[""\r\\])*""|"
           + @"([-a-z0-9!#$%&'*+/=?^_`{|}~]|(?<!\.)\.)*)(?<!\.)"
           + @"@[a-z0-9][\w\.-]*[a-z0-9]\.[a-z][a-z\.]*[a-z]$";

        public void Validate(object dto, bool delete)
        {
            if(SessionUser.IdPapel != PapelUsuarioEnum.Administrador)
                throw new ApplicationException("Você não possui permissão para executar esta operação.");

            if(dto is AlunoDto)
                ValidateAluno(dto as AlunoDto, delete);
            if (dto is EspecialidadeDto)
                ValidateEspecialidade(dto as EspecialidadeDto);
            if (dto is AlunoGrupoDto)
                ValidateAlunoGrupo(dto as AlunoGrupoDto);
            
        }

        private void ValidateAluno(AlunoDto dto, bool delete)
        {
            if (!delete)
            {
                if (dto.Usuario.IsNullOrEmpty())
                    throw new ApplicationException("Aluno: O campo 'Login' é obrigatório.");

                if (!dto.Senha.IsNullOrEmpty() && dto.Senha.Length < 6)
                    throw new ApplicationException("Aluno: O campo 'Senha' deve ter no mínimo 6 caracteres.");

                if (dto.Nome.IsNullOrEmpty())
                    throw new ApplicationException("Aluno: O campo 'Nome' é obrigatório.");

                if (!dto.CPF.IsNullOrEmpty() && dto.CPF.Length < 14)
                    throw new ApplicationException("Aluno: O campo 'CPF' deve ter 14 caracteres.");

                if (!dto.Email.IsNullOrEmpty() && !(new Regex(ValidEmailPattern, RegexOptions.IgnoreCase).IsMatch(dto.Email)))
                    throw new ApplicationException("Aluno: O campo 'Email' precisa ter um email no formato válido.");

                if (dto.Sexo != 1 && dto.Sexo != 2)
                    throw new ApplicationException("Aluno: O campo 'Sexo' é obrigatório.");

                if (!dto.Matricula.IsNullOrEmpty() && int.Parse(dto.Matricula) <= 0)
                    throw new ApplicationException("Aluno: O campo 'Matrícula' não foi informado corretamente.");

                if (!dto.Matricula.IsNullOrEmpty() && dto.Semestre <= 0)
                    throw new ApplicationException("Aluno: O campo 'Semestre' não foi informado corretamente.");

                if (!dto.DescricaoDataNascimento.IsNullOrEmpty() && !DateTime.TryParse(dto.DescricaoDataNascimento, out DateTime dataNasc) && dataNasc >= DateTime.Now.Date)
                    throw new ApplicationException("Aluno: O campo 'Data de nascimento' não foi informado corretamente.");

                if (!dto.Telefone.IsNullOrEmpty() && dto.Telefone.Length < 14)
                    throw new ApplicationException("Aluno: O campo 'Telefone' deve ter 14 caracteres.");

                if (!dto.Telefone.IsNullOrEmpty() && dto.Celular.Length < 16)
                    throw new ApplicationException("Aluno: O campo 'Celular' deve ter 16 caracteres.");

                if (new AlunoRepository().IsAlunoRepetidoByCPF(dto.Id, dto.CPF))
                    throw new ApplicationException("Aluno: Já existe um registro de aluno com o CPF informado.");

                if (new AlunoRepository().IsUsuarioLoginRepetido(dto.Id, dto.Usuario))
                    throw new ApplicationException("Aluno: Já existe um registro de usuário com o login informado.");
            }
            
        }

        private void ValidateAlunoGrupo(AlunoGrupoDto dto)
        {
            if(dto.IdGrupo == 0)
                throw new ApplicationException("Grupos: O campo 'Grupo' é obrigatório.");
        }

        private void ValidateEspecialidade(EspecialidadeDto dto)
        {
            if(dto.DisponibilidadeGrid.Any(x => x.HorarioInicio.IsNullOrEmpty() || x.HorarioFim.IsNullOrEmpty()))
                throw new ApplicationException("Disponibilidades: Os horários de início e fim são obrigatórios.");

            if (dto.DisponibilidadeGrid.Any(x => x.IdDiaSemana == 0))
                throw new ApplicationException("Disponibilidades: O campo 'Dia da semana' é obrigatório");

            if(dto.DisponibilidadeGrid.Any(x => TimeSpan.Parse(x.HorarioFim) < TimeSpan.Parse(x.HorarioInicio)))
                throw new ApplicationException("Disponibilidades: O horário de início deve ser menor que o horário de fim.");

            if(dto.IdEspecialidade == 0)
                throw new ApplicationException("Especialidades: O campo 'Especialidade' é obrigatório.");
        }
    }
}