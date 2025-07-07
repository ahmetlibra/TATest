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
    public class VehiclesController : ControllerBase
    {
        private readonly IVehicleService _vehicleService;
        private readonly IUserService _userService;

        public VehiclesController(IVehicleService vehicleService, IUserService userService)
        {
            _vehicleService = vehicleService;
            _userService = userService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<VehicleDto>>> GetAll()
        {
            var currentUserRole = User.FindFirst(ClaimTypes.Role)?.Value;
            
            // Admins can see all vehicles, regular users only see their own
            if (currentUserRole == UserRole.Admin.ToString() || currentUserRole == UserRole.SuperUser.ToString())
            {
                var vehicles = await _vehicleService.GetAllAsync();
                return Ok(vehicles);
            }
            else
            {
                var currentUserId = int.Parse(User.FindFirst(ClaimTypes.Name).Value);
                var userVehicles = await _vehicleService.GetByUserIdAsync(currentUserId);
                return Ok(userVehicles);
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<VehicleDto>> GetById(int id)
        {
            var vehicle = await _vehicleService.GetByIdAsync(id);
            if (vehicle == null)
                return NotFound();

            // Check if the current user owns this vehicle or is an admin
            var currentUserId = int.Parse(User.FindFirst(ClaimTypes.Name).Value);
            var currentUserRole = User.FindFirst(ClaimTypes.Role)?.Value;
            
            if (vehicle.UserId != currentUserId && 
                currentUserRole != UserRole.Admin.ToString() && 
                currentUserRole != UserRole.SuperUser.ToString())
            {
                return Forbid();
            }

            return Ok(vehicle);
        }

        [HttpPost]
        [Authorize(Roles = "Admin,SuperUser")]
        public async Task<ActionResult<VehicleDto>> Create([FromBody] VehicleRequest request)
        {
            try
            {
                var vehicle = await _vehicleService.CreateAsync(request);
                return CreatedAtAction(nameof(GetById), new { id = vehicle.Id }, vehicle);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] VehicleRequest request)
        {
            try
            {
                // Check if the current user owns this vehicle or is an admin
                var vehicle = await _vehicleService.GetByIdAsync(id);
                if (vehicle == null)
                    return NotFound();

                var currentUserId = int.Parse(User.FindFirst(ClaimTypes.Name).Value);
                var currentUserRole = User.FindFirst(ClaimTypes.Role)?.Value;
                
                if (vehicle.UserId != currentUserId && 
                    currentUserRole != UserRole.Admin.ToString() && 
                    currentUserRole != UserRole.SuperUser.ToString())
                {
                    return Forbid();
                }

                await _vehicleService.UpdateAsync(id, request);
                return NoContent();
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin,SuperUser")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await _vehicleService.DeleteAsync(id);
                return NoContent();
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPost("{id}/location")]
        public async Task<IActionResult> UpdateLocation(int id, [FromBody] LocationUpdateDto location)
        {
            try
            {
                // Check if the current user owns this vehicle or is an admin
                var vehicle = await _vehicleService.GetByIdAsync(id);
                if (vehicle == null)
                    return NotFound();

                var currentUserId = int.Parse(User.FindFirst(ClaimTypes.Name).Value);
                var currentUserRole = User.FindFirst(ClaimTypes.Role)?.Value;
                
                if (vehicle.UserId != currentUserId && 
                    currentUserRole != UserRole.Admin.ToString() && 
                    currentUserRole != UserRole.SuperUser.ToString())
                {
                    return Forbid();
                }

                await _vehicleService.UpdateLocationAsync(id, location.Latitude, location.Longitude);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet("user/{userId}")]
        public async Task<ActionResult<IEnumerable<VehicleDto>>> GetByUserId(int userId)
        {
            // Check if the current user is requesting their own vehicles or is an admin
            var currentUserId = int.Parse(User.FindFirst(ClaimTypes.Name).Value);
            var currentUserRole = User.FindFirst(ClaimTypes.Role)?.Value;
            
            if (userId != currentUserId && 
                currentUserRole != UserRole.Admin.ToString() && 
                currentUserRole != UserRole.SuperUser.ToString())
            {
                return Forbid();
            }

            var vehicles = await _vehicleService.GetByUserIdAsync(userId);
            return Ok(vehicles);
        }

        [HttpGet("type/{type}")]
        [Authorize(Roles = "Admin,SuperUser")]
        public async Task<ActionResult<IEnumerable<VehicleDto>>> GetByType(string type)
        {
            if (!Enum.TryParse(type, true, out VehicleType vehicleType))
                return BadRequest(new { message = "Invalid vehicle type" });

            var vehicles = await _vehicleService.GetByTypeAsync(vehicleType);
            return Ok(vehicles);
        }
    }
}
