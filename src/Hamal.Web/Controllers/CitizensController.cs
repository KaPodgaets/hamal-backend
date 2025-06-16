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
    [HttpGet("next")]
    public async Task<IActionResult> GetNextCitizen()
    {
        var userId = User.GetUserId();

        await using var transaction = await dbContext.Database.BeginTransactionAsync(System.Data.IsolationLevel.Serializable);

        try
        {
            var pendingStatus = CitizenStatus.Pending.ToString();
            var citizen = await dbContext.Citizens
                .FromSql($"SELECT * FROM \"Citizens\" WHERE \"Status\" = {pendingStatus} ORDER BY \"Id\" FOR UPDATE SKIP LOCKED LIMIT 1")
                .AsTracking()
                .FirstOrDefaultAsync();

            if (citizen is null)
            {
                await transaction.CommitAsync();
                return NoContent();
            }

            citizen.Status = CitizenStatus.InProgress;
            citizen.AssignedToUserId = userId;
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
    
    [HttpPut("{id:int}")]
    public async Task<IActionResult> UpdateCitizen(int id, [FromBody] UpdateCitizenRequest request)
    {
        var command = new UpdateCitizenCommand(
            id,
            request.FirstName,
            request.LastName,
            request.FamilyNumber,
            request.IsLonely,
            request.IsAddressWrong,
            request.NewStreetName,
            request.NewBuildingNumber,
            request.NewFlatNumber
        );

        var validationResult = await validator.ValidateAsync(command);
        if (!validationResult.IsValid)
        {
            return BadRequest(validationResult.Errors);
        }

        var userId = User.GetUserId();
        var citizen = await dbContext.Citizens.FindAsync(id);

        if (citizen is null) return NotFound();
        if (citizen.AssignedToUserId != userId) return Forbid("This record is not assigned to you.");
        if (citizen.Status != CitizenStatus.InProgress) return BadRequest("This record can no longer be updated.");
        if (citizen.LockedUntil < DateTime.UtcNow) return BadRequest("The lock on this record has expired.");

        // Map validated command to entity
        citizen.FirstName = command.FirstName;
        citizen.LastName = command.LastName;
        citizen.FamilyNumber = command.FamilyNumber;
        citizen.IsLonely = command.IsLonely;
        citizen.IsAddressWrong = command.IsAddressWrong;
        if (command.IsAddressWrong)
        {
            citizen.NewStreetName = command.NewStreetName;
            citizen.NewBuildingNumber = command.NewBuildingNumber;
            citizen.NewFlatNumber = command.NewFlatNumber.ToString();
        }
        
        citizen.Status = CitizenStatus.Updated;
        citizen.LastUpdatedAt = DateTime.UtcNow;

        await dbContext.SaveChangesAsync();
        
        return NoContent();
    }
    
    private static CitizenResponse MapToResponse(Citizen citizen) => new(
        citizen.Id,
        citizen.StreetName,
        citizen.BuildingNumber,
        citizen.FlatNumber,
        citizen.FirstName,
        citizen.LastName,
        citizen.FamilyNumber,
        citizen.IsLonely,
        citizen.IsAddressWrong,
        citizen.Status,
        citizen.LockedUntil,
        citizen.LastUpdatedAt
    );
} 