namespace Infrastructure.DbModels
{
    public class CustomerDbModel
    {
        protected CustomerDbModel()
        {

        }
        public CustomerDbModel(Guid id, string cpf, string name, string mail, bool customerIdentified, DateTime createdAt)
        {
            Cpf = cpf;
            Name = name;
            Mail = mail;
            CustomerIdentified = customerIdentified;
            Id = id;

            CreatedAt = createdAt;
        }




        public Guid Id { get; set; }
        public string? Cpf { get; set; }
        public string Name { get; set; } = string.Empty;

        public DateTime CreatedAt { get; private set; }

        public string Mail { get; set; } = string.Empty;

        public bool CustomerIdentified { get; private set; }

    }
}

 