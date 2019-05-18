namespace NPJe.FrontEnd.Dtos
{
    public class GenericInfoComboDto
    {
        public GenericInfoComboDto() { }

        public GenericInfoComboDto(long id, string descricao)
        {
            this.id = id;
            text = descricao;
        }

        public long id {  get; set; }

        public string text { get; set; }

        public string complement { get; set; }
    }
}