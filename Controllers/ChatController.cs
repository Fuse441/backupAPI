using colab_api.Models;
using colab_api.Repositories;
using colab_api.Requests.space;
using colab_api.Requests.user;
using colab_api.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

[ApiController]
[Route("api/[controller]")]
public class ChatController : ControllerBase
{
    private readonly ChatService _chatService;
  

    public ChatController(ChatService chatService)
    {
        _chatService = chatService;
        
    }

    
 
    [HttpGet]
    [Route("{id}")]
    public async Task<ActionResult> RenderChat(int id)
    {
        try
        {
            var transaction_id = Guid.NewGuid().ToString("n");
            if (!ModelState.IsValid)
            {
                return StatusCode(400);
            }
            var result = await _chatService.QueryChat(id);
            return StatusCode(200, result);



        }
        catch (Exception ex)
        {
            return StatusCode(500);
        }
    }

    //[HttpGet]
    //[Route("selectDate/{id}/{date}")]
    //public async Task<ActionResult> GetBookingSelectDate([FromRoute] int id, [FromRoute] string date)
    //{
    //    try
    //    {
    //        var transaction_id = Guid.NewGuid().ToString("n");
    //        if (!ModelState.IsValid)
    //        {
    //            return StatusCode(400);
    //        }
    //        var result = await _bookingService.QueryBookingSelectDate(id,date);
    //        return StatusCode(200, result);



    //    }
    //    catch (Exception ex)
    //    {
    //        return StatusCode(500);
    //    }
    //}


}
