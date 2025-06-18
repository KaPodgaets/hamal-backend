using Hamal.Domain.Entities;

namespace Hamal.Application.Common.Interfaces;

public interface IFileExporter
{
    byte[] ExportToCsv(IEnumerable<CitizenRecord> citizens);
} 