using Hamal.Domain.Entities;

namespace Hamal.Application.Common.Interfaces;

public interface IFileParser
{
    IEnumerable<CitizenRecord> ParseCitizens(Stream stream);
} 