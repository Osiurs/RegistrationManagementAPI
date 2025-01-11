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

        [HttpPost("group")]
        public async Task<IActionResult> SendMessageToGroup([FromBody] GroupMessageDTO model)
        {
            if (model == null)
                return BadRequest("Invalid request.");

            var messageDto = await _messageService.SendMessageToGroupAsync(model.SenderId, model.CourseId, model.Content);
            return Ok(messageDto);
        }

        [HttpGet("course/{courseId}")]
        public async Task<IActionResult> GetMessagesByCourseId(int courseId)
        {
            var messages = await _messageService.GetMessagesByCourseIdAsync(courseId);
            return Ok(messages);
        }

        [HttpGet("unread/course/{courseId}/{userId}")]
        public async Task<IActionResult> GetUnreadMessagesByCourseId(int courseId, int userId)
        {
            var messages = await _messageService.GetUnreadMessagesForCourseAsync(courseId, userId);
            return Ok(messages);
        }

        [HttpPost("mark-as-read/{courseId}/{userId}")]
        public async Task<IActionResult> MarkMessagesAsRead(int courseId, int userId)
        {
            try
            {
                await _messageService.MarkMessagesAsReadAsync(courseId, userId);
                return Ok("Messages marked as read.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet("unread/{userId}")]
        public async Task<IActionResult> GetUnreadMessages(int userId)
        {
            try
            {
                var unreadMessages = await _messageService.GetUnreadMessagesAsync(userId);

                if (!unreadMessages.Any())
                {
                    return NotFound("No unread messages found.");
                }

                return Ok(unreadMessages);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }
}