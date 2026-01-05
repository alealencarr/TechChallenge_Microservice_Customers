using Domain.ValueObjects;

namespace Domain.Entities
{
    public class Customer
    {
        public Customer(Guid id, DateTime createdAt, string cpf, string name, string mail, bool customerIdentified)
        {
            Id = id;
            CreatedAt = createdAt;
            Cpf = new CpfVo(cpf);
            Name = name;
            Mail = mail;
            CustomerIdentified = customerIdentified;
        }
        public Customer(string cpf, string name, string mail)
        {
            if (string.IsNullOrEmpty(cpf))
                throw new ArgumentNullException("É necessário informar um Cpf para criar o cliente");
            if (string.IsNullOrEmpty(name))
                throw new ArgumentNullException("É necessário informar um Nome para criar o cliente");
            if (string.IsNullOrEmpty(mail))
                throw new ArgumentNullException("É necessário informar um E-mail para criar o cliente");

            Cpf = new CpfVo(cpf);
            Name = name;
            Mail = mail;
            CustomerIdentified = true;
            Id = Guid.NewGuid();
            CreatedAt = DateTime.Now;
        }


        protected Customer()
        {

        }

        public DateTime CreatedAt { get; private set; }

        public Guid Id { get; set; }
        public CpfVo? Cpf { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Mail { get; set; } = string.Empty;

        public bool CustomerIdentified { get; private set; }

    }
}
