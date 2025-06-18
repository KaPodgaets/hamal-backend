using System.Globalization;
using System.Text;
using Hamal.Application.Common.Interfaces;
using Hamal.Domain.Entities;

namespace Hamal.Infrastructure.Files;

public class CsvExporter : IFileExporter
{
    public byte[] ExportToCsv(IEnumerable<CitizenRecord> citizens)
    {
        var sb = new StringBuilder();

        // Header
        sb.AppendLine(@"Id,FID,StreetName,BuildingNumber,FlatNumber,FirstName,LastName,FamilyNumber,IsAnsweredTheCall,IsLonely,IsAddressWrong,NewStreetName,NewBuildingNumber,NewFlatNumber,Status,LockedByUserId,LockedUntil,LastUpdatedByUserId,LastUpdatedAt,CreatedAt");

        foreach (var citizen in citizens)
        {
            sb.Append(citizen.Id).Append(',');
            sb.Append(citizen.Fid).Append(',');
            sb.Append(Escape(citizen.StreetName)).Append(',');
            sb.Append(Escape(citizen.BuildingNumber)).Append(',');
            sb.Append(Escape(citizen.FlatNumber)).Append(',');
            sb.Append(Escape(citizen.FirstName)).Append(',');
            sb.Append(Escape(citizen.LastName)).Append(',');
            sb.Append(citizen.FamilyNumber).Append(',');
            sb.Append(citizen.IsAnsweredTheCall).Append(',');
            sb.Append(citizen.IsLonely).Append(',');
            sb.Append(citizen.IsAddressWrong).Append(',');
            sb.Append(Escape(citizen.NewStreetName)).Append(',');
            sb.Append(Escape(citizen.NewBuildingNumber)).Append(',');
            sb.Append(Escape(citizen.NewFlatNumber)).Append(',');
            sb.Append(citizen.HasMamad).Append(',');
            sb.Append(citizen.HasMiklatPrati).Append(',');
            sb.Append(citizen.HasMiklatZiburi).Append(',');
            sb.Append(citizen.HasMobilityRestriction).Append(',');
            sb.Append(citizen.StatusInCallCenter).Append(',');
            sb.Append(citizen.LockedByUserId).Append(',');
            sb.Append(FormatDateTime(citizen.LockedUntil)).Append(',');
            sb.Append(citizen.LastUpdatedByUserId).Append(',');
            sb.Append(FormatDateTime(citizen.LastUpdatedAt)).Append(',');
            sb.AppendLine(FormatDateTime(citizen.CreatedAt));
            
        }

        return Encoding.UTF8.GetBytes(sb.ToString());
    }

    private static string Escape(string? value)
    {
        if (string.IsNullOrEmpty(value)) return "";
        if (value.Contains(',') || value.Contains('"') || value.Contains('\n'))
        {
            return $"\"{value.Replace("\"", "\"\"")}\"";
        }
        return value;
    }
    
    private static string FormatDateTime(DateTime? dt) => dt?.ToString("o", CultureInfo.InvariantCulture) ?? "";
} 