using Infrastructure.DbModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.ModelsConfiguration
{
    public class CustomerDbModelConfiguration : IEntityTypeConfiguration<CustomerDbModel>
    {
        public void Configure(EntityTypeBuilder<CustomerDbModel> builder)
        {
            builder.ToTable("Customers");

            builder.HasKey(c => c.Id);

            builder.Property(c => c.Id)
                   .IsRequired();

            builder.Property(c => c.Cpf)
                   .HasMaxLength(11);  

            builder.Property(c => c.Name)
                   .IsRequired()
                   .HasMaxLength(150);

            builder.Property(c => c.Mail)
                   .IsRequired()
                   .HasMaxLength(150);

            builder.Property(c => c.CustomerIdentified)
                   .IsRequired();

            builder.Property(c => c.CreatedAt)
                   .IsRequired();
 
        }
    }
}
