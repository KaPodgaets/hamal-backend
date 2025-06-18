using Hamal.Application.Common.Interfaces;
using Hamal.Domain.Entities;
using Hamal.Domain.Enums;

namespace Hamal.Infrastructure.Files;

public class CsvParsingException(string message) : Exception(message);

public class CsvParser : IFileParser
{
    public IEnumerable<CitizenRecord> ParseCitizens(Stream stream)
    {
        var citizens = new List<CitizenRecord>();
        using var reader = new StreamReader(stream);

        var header = reader.ReadLine();
        if (header is null) throw new CsvParsingException("CSV file is empty or has no header.");
        
        // A simple positional parser is used, assuming a minimal format for new data uploads:
        // StreetName, BuildingNumber, FlatNumber, FirstName, LastName, FamilyNumber, IsLonely

        int lineNumber = 1;
        while (!reader.EndOfStream)
        {
            lineNumber++;
            var line = reader.ReadLine();
            if (string.IsNullOrWhiteSpace(line)) continue;

            var values = line.Split(',');
            if (values.Length < 15) throw new CsvParsingException($"Line {lineNumber} has too few columns. Expected 15.");

            try
            {
                var citizen = new CitizenRecord
                {
                    Fid = int.Parse(values[0]),
                    FirstName = values[2].Trim(),
                    LastName = values[1].Trim(),
                    StreetName = values[3].Trim(),
                    BuildingNumber = values[4].Trim(),
                    FlatNumber = values[5].Trim(),
                    
                    FamilyNumber = int.Parse(values[6]),
                    
                    IsAnsweredTheCall = bool.Parse(values[7]),
                    
                    IsLonely = bool.Parse(values[8]),
                    IsAddressWrong = bool.Parse(values[9]),
                    
                    Phone1 = values[13].Trim(),
                    Phone2 = values[14].Trim(),
                    Phone3 = values[15].Trim(),
                    
                    StatusInCallCenter = CitizenStatus.Pending,
                    LastUpdatedAt = null,
                    CreatedAt = DateTime.UtcNow,
                    
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