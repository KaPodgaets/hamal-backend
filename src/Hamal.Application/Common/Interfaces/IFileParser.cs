using Hamal.Domain.Entities;

namespace Hamal.Application.Common.Interfaces;

public interface IFileParser
{
    IEnumerable<Citizen> ParseCitizens(Stream stream);
} 