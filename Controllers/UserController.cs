using colab_api.Models;
using colab_api.Requests;
using colab_api.Requests.user;
using colab_api.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using static System.Runtime.InteropServices.JavaScript.JSType;

[ApiController]
[Route("api/[controller]")]
public class UserController : ControllerBase
{
    private readonly UserService _userService;

    public UserController(UserService userService)
    {
        _userService = userService;
    }

    // GET: api/User

    [HttpGet]
    [Route("getById/{id}")]
    public async Task<ActionResult> GetUserById(int id)
    {
        try
        {
            var transaction_id = Guid.NewGuid().ToString("n");
            if (!ModelState.IsValid)
            {
                return StatusCode(400);
            }
            var result = await _userService.QueryUserById(id);
            return StatusCode(200, result);



        }
        catch (Exception ex)
        {
            return StatusCode(500);
        }
    }

    [HttpGet]
  
    public async Task<ActionResult> user([FromQuery] UsersRequest query)
    {
        try
        {
            var transaction_id = Guid.NewGuid().ToString("n");
            if (!ModelState.IsValid)
            {
                return StatusCode(400);
            }
            var result = await _userService.QueryUser(query);
            return StatusCode(200,result);



        }
        catch (Exception ex)
        {
            return StatusCode(500);
        }
    }
    [HttpPost]
    [Route("login")]
    public async Task<ActionResult> login([FromBody] loginRequest query)
    {
        try
        {
            var transaction_id = Guid.NewGuid().ToString("n");
            if (!ModelState.IsValid)
            {
                return StatusCode(400);
            }
            var result = await _userService.userAuthen(query);
            return StatusCode(200, result);



        }
        catch (Exception ex)
        {
            return StatusCode(500);
        }
    }
    [HttpPost]
    [Route("contact")]
    public async Task<ActionResult> Contact([FromBody] UserContactRequest query)
    {
        try
        {
            var transaction_id = Guid.NewGuid().ToString("n");
            if (!ModelState.IsValid)
            {
                return StatusCode(400);
            }
            var result = await _userService.UserContact(query);
            return StatusCode(200, result);



        }
        catch (Exception ex)
        {
            return StatusCode(500);
        }
    }
    [HttpPost]
    [Route("Upsert/{id}")]
    public async Task<ActionResult> UpsertUser(int id,[FromBody] UserUpsertRequest query)
    {
        try
        {
            var transaction_id = Guid.NewGuid().ToString("n");
            if (!ModelState.IsValid)
            {
                return StatusCode(400);
            }
            var result = await _userService.UserUpsert(id,query);
            return StatusCode(200, result);



        }
        catch (Exception ex)
        {
            return StatusCode(500);
        }
    }
    [HttpDelete]
    [Route("{id}")]
    public async Task<ActionResult> DeleteUser(int id)
    {
        try
        {
            var transaction_id = Guid.NewGuid().ToString("n");
            if (!ModelState.IsValid)
            {
                return StatusCode(400);
            }
            var result = await _userService.UserDelete(id);
            return StatusCode(200, result);



        }
        catch (Exception ex)
        {
            return StatusCode(500);
        }
    }

}
