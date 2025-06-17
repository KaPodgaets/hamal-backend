using Hamal.Application.Common.Interfaces;
using Hamal.Domain.Enums;
using Hamal.Infrastructure.Files;
using Hamal.Infrastructure.Persistence;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Hamal.Web.Controllers;

[ApiController]
[Route("api/admin/citizens")]
[Authorize(Roles = nameof(Role.Admin))]
public class AdminController(
    AppDbContext dbContext,
    IFileParser fileParser,
    IFileExporter fileExporter) : ControllerBase
{
    /// <summary>
    /// Delete all citizens' records from table citizens
    /// </summary>
    /// <response code="200">Returns a NoContentResult</response>
    /// <response code="401">Unauthorized</response>
    [ProducesResponseType(typeof(NoContentResult), 200)]
    [ProducesResponseType(401)]
    [HttpDelete("")]
    public async Task<IActionResult> ClearCitizens()
    {
        // This is a highly destructive operation. In a real app, this might be a soft delete or require extra confirmation.
        await dbContext.Citizens.ExecuteDeleteAsync();
        return NoContent();
    }
    
    /// <summary>
    /// Return csv file of all citizens' records from table citizens
    /// </summary>
    /// <response code="200">Returns a NonActionResult</response>
    /// <response code="401">Unauthorized</response>
    [ProducesResponseType(typeof(File), 200)]
    [ProducesResponseType(401)]
    [HttpGet("")]
    public async Task<IActionResult> ExportCitizens()
    {
        var citizens = await dbContext.Citizens.AsNoTracking().ToListAsync();
        var fileBytes = fileExporter.ExportToCsv(citizens);
        
        return File(fileBytes, "text/csv", $"citizens-export-{DateTime.UtcNow:yyyy-MM-dd-HH-mm-ss}.csv");
    }
    
    /// <summary>
    /// Upload new users
    /// </summary>
    /// <response code="200">Returns a message about successfully loaded records</response>
    /// <response code="401">Unauthorized</response>
    /// <response code="409">Conflict</response>
    /// <response code="500">BadRequest</response>
    [ProducesResponseType(200)]
    [ProducesResponseType(401)]
    [ProducesResponseType(409)]
    [ProducesResponseType(500)]
    [HttpPost("")]
    public async Task<IActionResult> UploadCitizens(IFormFile? file)
    {
        if (file is null || file.Length == 0)
        {
            return BadRequest("File is empty.");
        }

        // The workflow is clear -> upload. We enforce this by checking if the table is empty.
        if (await dbContext.Citizens.AnyAsync())
        {
            return Conflict("The Citizens table is not empty. Please clear the data before uploading.");
        }

        await using var transaction = await dbContext.Database.BeginTransactionAsync();
        try
        {
            var citizens = fileParser.ParseCitizens(file.OpenReadStream());
            
            dbContext.Citizens.AddRange(citizens);
            var count = await dbContext.SaveChangesAsync();
            
            await transaction.CommitAsync();
            return Ok(new { Message = $"Successfully uploaded and created {count} citizen records." });
        }
        catch (CsvParsingException ex)
        {
            await transaction.RollbackAsync();
            return BadRequest(new { Error = "CSV parsing failed.", Details = ex.Message });
        }
        catch (Exception ex)
        {
            await transaction.RollbackAsync();
            // In a real application, log the full exception `ex` here
            return StatusCode(500, "An unexpected error occurred during the upload process.");
        }
    }
} 