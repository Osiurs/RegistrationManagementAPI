using Microsoft.AspNetCore.Mvc;
using RegistrationManagementAPI.Entities;
using RegistrationManagementAPI.DTOs;
using RegistrationManagementAPI.Services.Interface;

namespace RegistrationManagementAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MessageController : ControllerBase
    {
        private readonly IMessageService _messageService;

        public MessageController(IMessageService messageService)
        {
            _messageService = messageService;
        }

        // Gửi thông báo
        [HttpPost]
        public async Task<IActionResult> SendMessage([FromBody] MessageDTO model)
        {
            if (model == null)
                return BadRequest("Invalid request.");

            var messageDto = await _messageService.SendMessageAsync(model.SenderId, model.ReceiverId, model.Content);

            if (messageDto != null)
                return Ok(messageDto);

            return StatusCode(500, "An error occurred while sending the message.");
        }

        [HttpGet("user/{userId}")]
        public async Task<IActionResult> GetMessagesByUserId(int userId)
        {
            var messages = await _messageService.GetMessagesByUserIdAsync(userId);
            return Ok(messages);
        }

    }

}