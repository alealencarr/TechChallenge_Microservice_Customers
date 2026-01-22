using System.Diagnostics.CodeAnalysis;

namespace Shared.DTO.Categorie.Input;
[ExcludeFromCodeCoverage]

public record CustomerInputDto(Guid Id, DateTime CreatedAt, string Cpf, string Name, string Mail, bool CustomerIdentified); 