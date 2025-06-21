using System.Security.Claims;
using FluentValidation;
using Hamal.Application.Citizens.Commands;
using Hamal.Domain.Entities;
using Hamal.Domain.Enums;
using Hamal.Infrastructure.Persistence;
using Hamal.Web.Common.Extensions;
using Hamal.Web.Contracts.Citizens;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Hamal.Web.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class CitizensController(AppDbContext dbContext, IValidator<UpdateCitizenCommand> validator) : ControllerBase
{
    /// <summary>
    /// Create new user
    /// </summary>
    /// <response code="200">Returns a CreatedAtActionResult</response>
    /// <response code="204">Returns Not Found Response</response>
    /// <response code="401">Unauthorized</response>
    [ProducesResponseType(typeof(CitizenResponse), 200)]
    [ProducesResponseType(204)]
    [ProducesResponseType(401)]
    [HttpGet("next")]
    public async Task<IActionResult> GetNextCitizen()
    {
        var userId = User.GetUserId();

        await using var transaction =
            await dbContext.Database.BeginTransactionAsync(System.Data.IsolationLevel.Serializable);

        try
        {
            var pendingStatus = nameof(CitizenStatus.Pending);
            var citizen = await dbContext.Citizens
                .FromSqlRaw("""
                                SELECT * FROM "Citizens"
                                WHERE "StatusInCallCenter" = {0}
                                ORDER BY "Id"
                                FOR UPDATE SKIP LOCKED
                                LIMIT 1
                            """, pendingStatus)
                .AsTracking()
                .FirstOrDefaultAsync();

            if (citizen is null)
            {
                await transaction.CommitAsync();
                return NoContent();
            }

            citizen.StatusInCallCenter = CitizenStatus.InProgress;
            citizen.LockedByUserId = userId;
            citizen.LockedUntil = DateTime.UtcNow.AddMinutes(30);
            citizen.LastUpdatedAt = DateTime.UtcNow;

            await dbContext.SaveChangesAsync();
            await transaction.CommitAsync();

            return Ok(MapToResponse(citizen));
        }
        catch (Exception)
        {
            await transaction.RollbackAsync();
            // Log exception
            return StatusCode(500, "An error occurred while trying to get the next citizen.");
        }
    }

    /// <summary>
    /// Update citizen's form
    /// </summary>
    /// <response code="200">Ok</response>
    /// <response code="401">Unauthorized</response>
    /// <response code="404">NotFound</response>
    /// <response code="500">BadRequest</response>
    [ProducesResponseType(200)]
    [ProducesResponseType(401)]
    [ProducesResponseType(404)]
    [ProducesResponseType(500)]
    [HttpPut("{id:int}")]
    public async Task<IActionResult> UpdateCitizen(int id, [FromBody] UpdateCitizenRequest request)
    {
        // TODO: remove unnecessary fields (that is base fields)
        var command = new UpdateCitizenCommand(
            id,
            request.StreetName,
            request.BuildingNumber,
            request.FlatNumber,
            request.FirstName,
            request.LastName,
            request.FamilyNumber,
            request.IsLonely,
            request.IsAddressWrong,
            request.NewStreetName,
            request.NewBuildingNumber,
            request.NewFlatNumber,
            request.Phone1,
            request.Phone2,
            request.Phone3,
            request.IsAnsweredTheCall,
            request.HasMamad,
            request.HasMiklatPrati,
            request.HasMiklatZiburi,
            request.HasMobilityRestriction,
            request.IsDead,
            request.HasTemporaryAddress,
            request.TemporaryStreetName,
            request.TemporaryBuildingNumber,
            request.TemporaryFlat
        );

        var validationResult = await validator.ValidateAsync(command);
        if (!validationResult.IsValid)
        {
            return BadRequest(validationResult.Errors);
        }

        var userId = User.GetUserId();
        var citizen = await dbContext.Citizens.FindAsync(id);

        if (citizen is null) return NotFound();
        if (citizen.LockedByUserId != userId) return Forbid("This record is not assigned to you.");
        if (citizen.StatusInCallCenter != CitizenStatus.InProgress)
            return BadRequest("This record can no longer be updated.");
        if (citizen.LockedUntil < DateTime.UtcNow) return BadRequest("The lock on this record has expired.");

        // Map validated command to entity
        citizen.IsLonely = command.IsLonely;
        citizen.IsAddressWrong = command.IsAddressWrong;
        if (command.IsAddressWrong)
        {
            citizen.NewStreetName = command.NewStreetName;
            citizen.NewBuildingNumber = command.NewBuildingNumber;
            citizen.NewFlatNumber = command.NewFlatNumber;
        }

        citizen.IsAnsweredTheCall = command.IsAnsweredTheCall;
        
        citizen.HasMamad = command.HasMamad;
        citizen.HasMiklatPrati = command.HasMiklatPrati;
        citizen.HasMiklatZiburi = command.HasMiklatZiburi;
        citizen.HasMobilityRestriction = command.HasMobilityRestriction;
        
        citizen.IsDead = command.IsDead;
        citizen.HasTemporaryAddress = command.HasTemporaryAddress;
        if (command.HasTemporaryAddress)
        {
            citizen.TemporaryStreetName = command.TemporaryStreetName;
            citizen.TemporaryBuildingNumber = command.TemporaryBuildingNumber;
            citizen.TemporaryFlat = command.TemporaryFlat;
        }

        citizen.StatusInCallCenter = CitizenStatus.Updated;
        citizen.LastUpdatedAt = DateTime.UtcNow;

        await dbContext.SaveChangesAsync();

        return Ok();
    }

    private static CitizenResponse MapToResponse(CitizenRecord citizenRecord) => new(
        citizenRecord.Id,
        citizenRecord.StreetName,
        citizenRecord.BuildingNumber,
        citizenRecord.FlatNumber,
        citizenRecord.FirstName,
        citizenRecord.LastName,
        citizenRecord.FamilyNumber,
        citizenRecord.IsLonely,
        citizenRecord.IsAddressWrong,
        citizenRecord.NewStreetName,
        citizenRecord.NewBuildingNumber,
        citizenRecord.NewFlatNumber,
        citizenRecord.Phone1,
        citizenRecord.Phone2,
        citizenRecord.Phone3,
        citizenRecord.IsAnsweredTheCall,
        citizenRecord.HasMamad,
        citizenRecord.HasMiklatPrati,
        citizenRecord.HasMiklatZiburi,
        citizenRecord.HasMobilityRestriction,
        citizenRecord.IsDead,
        citizenRecord.HasTemporaryAddress,
        citizenRecord.TemporaryStreetName,
        citizenRecord.TemporaryBuildingNumber,
        citizenRecord.TemporaryFlat);
}