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
        sb.AppendLine("Id,FID,StreetName,BuildingNumber,FlatNumber,FirstName,LastName," +
                      "FamilyNumber,IsAnsweredTheCall,IsLonely,IsAddressWrong,NewStreetName," +
                      "NewBuildingNumber,NewFlatNumber,HasMamad,HasMiklatPrati,HasMiklatZiburi," +
                      "HasMobilityRestriction,IsDead,IsLeftTheCityPermanent,HasTemporaryAddress," +
                      "IsTemporaryAbroad,TemporaryStreetName,TemporaryBuildingNumber,TemporaryFlat," +
                      "Status,LockedByUserId,LockedUntil,LastUpdatedByUserId,LastUpdatedAt," +
                      "CreatedAt,AppearanceCount,FirstTimeAppearance,SecondTimeAppearance," +
                      "ThirdTimeAppearance");

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
            sb.Append(citizen.IsDead).Append(',');
            sb.Append(citizen.IsLeftTheCity).Append(',');
            sb.Append(citizen.HasTemporaryAddress).Append(',');
            sb.Append(citizen.IsTemporaryAbroad).Append(',');
            sb.Append(Escape(citizen.TemporaryStreetName)).Append(',');
            sb.Append(Escape(citizen.TemporaryBuildingNumber)).Append(',');
            sb.Append(Escape(citizen.TemporaryFlat)).Append(',');
            sb.Append(citizen.StatusInCallCenter).Append(',');
            sb.Append(citizen.LockedByUserId).Append(',');
            sb.Append(FormatDateTime(citizen.LockedUntil)).Append(',');
            sb.Append(citizen.LastUpdatedByUserId).Append(',');
            sb.Append(FormatDateTime(citizen.LastUpdatedAt)).Append(',');
            sb.Append(FormatDateTime(citizen.CreatedAt)).Append(',');
            sb.Append(citizen.AppearanceCount.ToString()).Append(',');
            sb.Append(FormatDateTime(citizen.FirstTimeAppearance)).Append(',');
            sb.Append(FormatDateTime(citizen.SecondTimeAppearance)).Append(',');
            sb.AppendLine(FormatDateTime(citizen.ThirdTimeAppearance));
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