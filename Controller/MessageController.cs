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
    public async Task<IActionResult> SendMessage([FromBody] MessageDTO message)
    {
        await _messageService.SendMessageAsync(message);
        return Ok(new { message = "Message sent successfully" });
    }

    // Lấy thông báo của học viên
    [HttpGet("student/{studentId}")]
    public async Task<IActionResult> GetStudentMessages(int studentId)
    {
        var messages = await _messageService.GetStudentMessagesAsync(studentId);
        return Ok(messages);
    }
}

}