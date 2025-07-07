using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TKApp.Business.DTOs;
using TKApp.Business.Interfaces;
using TKApp.Core.Enums;

namespace TKApp.API.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class LocationsController : ControllerBase
    {
        private readonly ILocationService _locationService;
        private readonly IVehicleService _vehicleService;

        public LocationsController(ILocationService locationService, IVehicleService vehicleService)
        {
            _locationService = locationService;
            _vehicleService = vehicleService;
        }

        [HttpGet("vehicles/{vehicleId}")]
        public async Task<ActionResult<LocationDto>> GetVehicleLocation(int vehicleId)
        {
            // Check if the current user has access to this vehicle
            if (!await HasAccessToVehicle(vehicleId))
                return Forbid();

            try
            {
                var location = await _locationService.GetVehicleLocationAsync(vehicleId);
                if (location == null)
                    return NotFound();

                return Ok(location);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet("vehicles/{vehicleId}/history")]
        public async Task<ActionResult<IEnumerable<LocationHistoryDto>>> GetVehicleLocationHistory(int vehicleId, [FromQuery] int count = 100)
        {
            // Check if the current user has access to this vehicle
            if (!await HasAccessToVehicle(vehicleId))
                return Forbid();

            try
            {
                var history = await _locationService.GetVehicleLocationHistoryAsync(vehicleId, count);
                return Ok(history);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPost("vehicles/{vehicleId}")]
        public async Task<IActionResult> UpdateVehicleLocation(int vehicleId, [FromBody] LocationUpdateDto location)
        {
            // Check if the current user has access to this vehicle
            if (!await HasAccessToVehicle(vehicleId))
                return Forbid();

            try
            {
                await _locationService.UpdateVehicleLocationAsync(vehicleId, location.Latitude, location.Longitude);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPost("batch-update")]
        [Authorize(Roles = "Admin,SuperUser")]
        public async Task<IActionResult> BatchUpdateLocations([FromBody] Dictionary<int, LocationUpdateDto> locationUpdates)
        {
            try
            {
                await _locationService.UpdateVehicleLocationsBatchAsync(locationUpdates);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        private async Task<bool> HasAccessToVehicle(int vehicleId)
        {
            var vehicle = await _vehicleService.GetByIdAsync(vehicleId);
            if (vehicle == null)
                return false;

            // Admins and superusers have access to all vehicles
            var currentUserRole = User.FindFirst(ClaimTypes.Role)?.Value;
            if (currentUserRole == UserRole.Admin.ToString() || currentUserRole == UserRole.SuperUser.ToString())
                return true;

            // Regular users can only access their own vehicles
            var currentUserId = int.Parse(User.FindFirst(ClaimTypes.Name).Value);
            return vehicle.UserId == currentUserId;
        }
    }
}
