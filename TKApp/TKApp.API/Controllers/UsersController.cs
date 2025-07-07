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
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly ITenantProvider _tenantProvider;

        public UsersController(IUserService userService, ITenantProvider tenantProvider)
        {
            _userService = userService;
            _tenantProvider = tenantProvider;
        }

        [HttpGet]
        [Authorize(Roles = "Admin,SuperUser")]
        public async Task<ActionResult<IEnumerable<UserResponse>>> GetAll()
        {
            var users = await _userService.GetAllAsync();
            return Ok(users);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<UserResponse>> GetById(int id)
        {
            // Users can view their own profile, admins can view any profile
            var currentUserId = int.Parse(User.FindFirst(ClaimTypes.Name).Value);
            var currentUserRole = User.FindFirst(ClaimTypes.Role)?.Value;
            
            if (id != currentUserId && currentUserRole != UserRole.Admin.ToString() && currentUserRole != UserRole.SuperUser.ToString())
                return Forbid();

            var user = await _userService.GetByIdAsync(id);
            if (user == null)
                return NotFound();

            return Ok(user);
        }

        [HttpPost]
        [Authorize(Roles = "Admin,SuperUser")]
        public async Task<ActionResult<UserResponse>> Create([FromBody] UserRequest request)
        {
            try
            {
                var user = await _userService.CreateAsync(request);
                return CreatedAtAction(nameof(GetById), new { id = user.Id }, user);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] UserRequest request)
        {
            // Users can update their own profile, admins can update any profile
            var currentUserId = int.Parse(User.FindFirst(ClaimTypes.Name).Value);
            var currentUserRole = User.FindFirst(ClaimTypes.Role)?.Value;
            
            if (id != currentUserId && currentUserRole != UserRole.Admin.ToString() && currentUserRole != UserRole.SuperUser.ToString())
                return Forbid();

            try
            {
                await _userService.UpdateAsync(id, request);
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
            var currentUserId = int.Parse(User.FindFirst(ClaimTypes.Name).Value);
            if (id == currentUserId)
                return BadRequest(new { message = "You cannot delete your own account" });

            try
            {
                await _userService.DeleteAsync(id);
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

        [HttpPost("{id}/roles/{role}")]
        [Authorize(Roles = "Admin,SuperUser")]
        public async Task<IActionResult> AddToRole(int id, string role)
        {
            if (!Enum.TryParse(role, out UserRole userRole))
                return BadRequest(new { message = "Invalid role" });

            try
            {
                await _userService.AddToRoleAsync(id, userRole);
                return Ok();
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

        [HttpDelete("{id}/roles/{role}")]
        [Authorize(Roles = "Admin,SuperUser")]
        public async Task<IActionResult> RemoveFromRole(int id, string role)
        {
            if (!Enum.TryParse(role, out UserRole userRole))
                return BadRequest(new { message = "Invalid role" });

            try
            {
                await _userService.RemoveFromRoleAsync(id, userRole);
                return Ok();
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
    }
}
