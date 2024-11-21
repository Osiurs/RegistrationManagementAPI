using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using RegistrationManagementAPI.DTOs;
using RegistrationManagementAPI.Entities;
using RegistrationManagementAPI.Services;

namespace RegistrationManagementAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RegistrationController : ControllerBase
    {
        private readonly IRegistrationService _registrationService;
        private readonly IMapper _mapper;

        public RegistrationController(IRegistrationService registrationService, IMapper mapper)
        {
            _registrationService = registrationService;
            _mapper = mapper;
        }

        /// <summary>
        /// Lấy danh sách tất cả các đăng ký khóa học.
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> GetAllRegistrations()
        {
            var registrations = await _registrationService.GetAllRegistrationsAsync();
            var registrationDTOs = _mapper.Map<IEnumerable<RegistrationDTO>>(registrations);
            return Ok(registrationDTOs);
        }

        /// <summary>
        /// Lấy danh sách các đăng ký theo StudentId.
        /// </summary>
        [HttpGet("student/{studentId}")]
        public async Task<IActionResult> GetRegistrationsByStudentId(int studentId)
        {
            var registrations = await _registrationService.GetRegistrationsByStudentIdAsync(studentId);
            if (!registrations.Any())
            {
                return NotFound(new { message = "No registrations found for the student." });
            }

            var registrationDTOs = _mapper.Map<IEnumerable<RegistrationDTO>>(registrations);
            return Ok(registrationDTOs);
        }

        /// <summary>
        /// Lấy thông tin chi tiết một đăng ký theo ID.
        /// </summary>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetRegistrationById(int id)
        {
            var registration = await _registrationService.GetRegistrationByIdAsync(id);
            if (registration == null)
            {
                return NotFound(new { message = "Registration not found." });
            }

            var registrationDTO = _mapper.Map<RegistrationDTO>(registration);
            return Ok(registrationDTO);
        }

        /// <summary>
        /// Thêm một đăng ký mới.
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> AddRegistration([FromBody] RegistrationDTO registrationDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var registration = _mapper.Map<Registration>(registrationDTO);
            var newRegistration = await _registrationService.AddRegistrationAsync(registration);
            var newRegistrationDTO = _mapper.Map<RegistrationDTO>(newRegistration);

            return CreatedAtAction(nameof(GetRegistrationById), new { id = newRegistrationDTO.RegistrationId }, newRegistrationDTO);
        }

        /// <summary>
        /// Cập nhật thông tin một đăng ký.
        /// </summary>
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateRegistration(int id, [FromBody] RegistrationDTO registrationDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var existingRegistration = await _registrationService.GetRegistrationByIdAsync(id);
            if (existingRegistration == null)
            {
                return NotFound(new { message = "Registration not found." });
            }

            var updatedRegistration = _mapper.Map(registrationDTO, existingRegistration);
            await _registrationService.UpdateRegistrationAsync(updatedRegistration);

            var updatedRegistrationDTO = _mapper.Map<RegistrationDTO>(updatedRegistration);
            return Ok(updatedRegistrationDTO);
        }

        /// <summary>
        /// Xóa một đăng ký.
        /// </summary>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRegistration(int id)
        {
            var existingRegistration = await _registrationService.GetRegistrationByIdAsync(id);
            if (existingRegistration == null)
            {
                return NotFound(new { message = "Registration not found." });
            }

            await _registrationService.DeleteRegistrationAsync(id);
            return NoContent();
        }
    }
}
