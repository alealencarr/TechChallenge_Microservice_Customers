using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace Shared.DTO.Categorie.Output
{
    [ExcludeFromCodeCoverage]

    public record CustomerOutputDto
    {
        public Guid Id { get; set; }

        public string Cpf { get; set; }

        public string Name { get; set; }

        public string Mail { get; set; }

    }


}
