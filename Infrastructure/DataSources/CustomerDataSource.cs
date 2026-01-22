using Application.Interfaces.DataSources;
using Infrastructure.DbContexts;
using Infrastructure.DbModels;
using Microsoft.EntityFrameworkCore;
using Shared.DTO.Categorie.Input;
using Shared.DTO.Customer.Request;
using System.Diagnostics.CodeAnalysis;

namespace Infrastructure.DataSources
{
    [ExcludeFromCodeCoverage]

    public class CustomerDataSource : ICustomerDataSource
    {
        private readonly AppDbContext _appDbContext;

        public CustomerDataSource(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public async Task Update(CustomerInputDto customer)
        {
            var customerDb = await _appDbContext.Customer.Where(x => x.Id == customer.Id).FirstOrDefaultAsync() ?? throw new Exception("Customer not find by Id.");
            customerDb.Cpf = customer.Cpf;
            customerDb.Name = customer.Name;
            customerDb.Mail = customer.Mail;

            _appDbContext.Update(customerDb);
            await _appDbContext.SaveChangesAsync();
        }
        public async Task Create(CustomerInputDto customer)
        {
            var customerDbModel = new CustomerDbModel(customer.Id, customer.Cpf, customer.Name, customer.Mail, customer.CustomerIdentified, customer.CreatedAt);

            await _appDbContext.AddAsync(customerDbModel);
            await _appDbContext.SaveChangesAsync();           
        }

        public async Task<CustomerInputDto?> GetByCpf(string cpf)
        {
            var customer = await _appDbContext.Customer.AsNoTracking().Where(x => x.Cpf == cpf).FirstOrDefaultAsync();

            return customer is not null ? new CustomerInputDto(customer.Id, customer.CreatedAt, customer.Cpf, customer.Name, customer.Mail, customer.CustomerIdentified) : null;
        }

        public async Task<CustomerInputDto?> GetById(Guid id)
        {

            var customer = await _appDbContext.Customer.AsNoTracking().Where(x => x.Id == id).FirstOrDefaultAsync();

            return customer is not null ? new CustomerInputDto(customer.Id, customer.CreatedAt, customer.Cpf, customer.Name, customer.Mail, customer.CustomerIdentified) : null;
        }        

    }
}
