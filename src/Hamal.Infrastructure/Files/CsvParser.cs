using System.Globalization;
using Hamal.Application.Common.Interfaces;
using Hamal.Domain.Entities;
using Hamal.Domain.Enums;

namespace Hamal.Infrastructure.Files;

public class CsvParsingException(string message) : Exception(message);

public class CsvParser : IFileParser
{
    public IEnumerable<Citizen> ParseCitizens(Stream stream)
    {
        var citizens = new List<Citizen>();
        using var reader = new StreamReader(stream);

        var header = reader.ReadLine();
        if (header is null) throw new CsvParsingException("CSV file is empty or has no header.");
        
        // A simple positional parser is used, assuming a minimal format for new data uploads:
        // StreetName,BuildingNumber,FlatNumber,FirstName,LastName,FamilyNumber,IsLonely

        int lineNumber = 1;
        while (!reader.EndOfStream)
        {
            lineNumber++;
            var line = reader.ReadLine();
            if (string.IsNullOrWhiteSpace(line)) continue;

            var values = line.Split(',');
            if (values.Length < 7) throw new CsvParsingException($"Line {lineNumber} has too few columns. Expected 7.");

            try
            {
                var citizen = new Citizen
                {
                    StreetName = values[0].Trim(),
                    BuildingNumber = values[1].Trim(),
                    FlatNumber = values[2].Trim(),
                    FirstName = values[3].Trim(),
                    LastName = values[4].Trim(),
                    FamilyNumber = int.Parse(values[5], CultureInfo.InvariantCulture),
                    IsLonely = bool.Parse(values[6]),
                    Status = CitizenStatus.Pending,
                    LastUpdatedAt = DateTime.UtcNow
                };
                citizens.Add(citizen);
            }
            catch (Exception ex)
            {
                throw new CsvParsingException($"Error parsing line {lineNumber}: {ex.Message}");
            }
        }
        return citizens;
    }
} 