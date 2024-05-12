using colab_api.Models;
using colab_api.Repositories;
using colab_api.Requests.space;
using colab_api.Requests.user;
using colab_api.Responses.space;
using colab_api.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OfficeOpenXml;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using static System.Runtime.InteropServices.JavaScript.JSType;
using LicenseContext = OfficeOpenXml.LicenseContext;

[ApiController]
[Route("api/[controller]")]
public class SpaceController : ControllerBase
{
    private readonly SpaceService _spaceService;

    public SpaceController(SpaceService spaceService)
    {
        _spaceService = spaceService;
    }

    [HttpGet]
    [Route("getSpace/{id}")]
    public async Task<ActionResult> GetSpace([FromRoute] int id,[FromQuery] SpaceRequest query)
    {
        try
        {
            var transaction_id = Guid.NewGuid().ToString("n");
            if (!ModelState.IsValid)
            {
                return StatusCode(400);
            }
            var result = await _spaceService.QueryUserById(id, query);
            return StatusCode(200, result);



        }
        catch (Exception ex)
        {
            return StatusCode(500);
        }
    }
    // GET: api/User
   


    [HttpGet]
    public async Task<ActionResult> space([FromQuery] SpaceRequest query)
    {
        try
        {
            var transaction_id = Guid.NewGuid().ToString("n");
            if (!ModelState.IsValid)
            {
                return StatusCode(400);
            }
            var result = await _spaceService.QuerySpace(query);
            return StatusCode(200, result);



        }
        catch (Exception ex)
        {
            return StatusCode(500);
        }
    }
    [HttpPost]
    [Route("{id}")]
    public async Task<ActionResult> UpsertSpace(int id, SpaceUpsertRequest query)
    {
        try
        {
            var transaction_id = Guid.NewGuid().ToString("n");
            if (!ModelState.IsValid)
            {
                return StatusCode(400);
            }
            var result = await _spaceService.SpaceUpsert(id,query);
            return StatusCode(200, result);



        }
        catch (Exception ex)
        {
            return StatusCode(500);
        }
    }
    [HttpDelete]
    [Route("{id}")]
    public async Task<ActionResult> DeleteSpace(int id)
    {
        try
        {
            var transaction_id = Guid.NewGuid().ToString("n");
            if (!ModelState.IsValid)
            {
                return StatusCode(400);
            }
            var result = await _spaceService.SpaceDelete(id);
            return StatusCode(200, result);



        }
        catch (Exception ex)
        {
            return StatusCode(500);
        }
    }

}
