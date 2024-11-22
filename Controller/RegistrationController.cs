using Microsoft.AspNetCore.Mvc;
using RegistrationManagementAPI.Entities;
using RegistrationManagementAPI.Services.Interface;

namespace RegistrationManagementAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RegistrationController : ControllerBase
    {
        private readonly IRegistrationService _registrationService;

        public RegistrationController(IRegistrationService registrationService)
        {
            _registrationService = registrationService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllRegistrations()
        {
            var registrations = await _registrationService.GetAllRegistrationsAsync();
            return Ok(registrations);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetRegistrationById(int id)
        {
            try
            {
                var registration = await _registrationService.GetRegistrationByIdAsync(id);
                return Ok(registration);
            }
            catch (InvalidOperationException ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }

        [HttpPost]
        public async Task<IActionResult> AddRegistration([FromBody] Registration registration)
        {
            try
            {
                var newRegistration = await _registrationService.AddRegistrationAsync(registration);
                return CreatedAtAction(nameof(GetRegistrationById), new { id = newRegistration.RegistrationId }, newRegistration);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateRegistration(int id, [FromBody] Registration registration)
        {
            try
            {
                await _registrationService.UpdateRegistrationAsync(id, registration);
                return NoContent();
            }
            catch (InvalidOperationException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRegistration(int id)
        {
            try
            {
                await _registrationService.DeleteRegistrationAsync(id);
                return NoContent();
            }
            catch (InvalidOperationException ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }
    }
}
