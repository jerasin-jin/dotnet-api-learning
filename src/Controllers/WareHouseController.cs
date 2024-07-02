using System.Net;
using JsonFlatFileDataStore;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RestApiSample.Interfaces;
using RestApiSample.Middleware;
using RestApiSample.Models;
using RestApiSample.Services;

namespace RestApiSample.Controllers;

[ApiController]
[Route("[controller]")]
public class WareHouseController : BaseController
{


    private readonly WareHouseService _wareHouseService;


    public WareHouseController(WareHouseService wareHouseService)
    {

        _wareHouseService = wareHouseService;
    }

    [HttpPost, CustomAuthorizeAttribute(Roles.Admin)]
    public async Task<IActionResult> Post([FromBody] IWareHouse wareHouse)
    {
        // _users.InsertOne(user);
        var email = getJwtPayload("email");
        var result = await _wareHouseService.createWareHouse(email, wareHouse);

        return result.GetActionResult();
    }

    [HttpGet, CustomAuthorizeAttribute(Roles.Admin | Roles.User)]
    public Object Get()
    {
        var wareHouses = _wareHouseService.getWareHouses();

        // var test = new
        // {
        //     status = "444"
        // };

        // return test;

        return wareHouses.GetActionResult();
    }

    [HttpGet("{id:int}"), CustomAuthorizeAttribute(Roles.Admin | Roles.User)]
    public IActionResult GetById(int id)
    {
        // return _users.AsQueryable().FirstOrDefault(user => user.id == id);
        var wareHouse = _wareHouseService.getWareHouse(id);

        if (wareHouse is null)
        {
            return NotFound(wareHouse);
        }

        return Ok(wareHouse);
    }

    [HttpPut("{id:int}"), CustomAuthorizeAttribute(Roles.Admin | Roles.User)]
    public async Task<IActionResult> Put(int id, [FromBody] WareHouse wareHouse)
    {
        // var findUser = _users.AsQueryable().FirstOrDefault(user => user.id == id);

        // if (findUser == null) return null;

        // findUser = user;
        // await _users.UpdateOneAsync(user => user.id == id, findUser);

        // return findUser;


        await _wareHouseService.updateWareHouse(id, wareHouse);

        return Ok(wareHouse);
    }

    [HttpDelete("{id:int}"), CustomAuthorizeAttribute(Roles.Admin)]
    [Authorize]
    public async Task<IActionResult> delete(int id)
    {
        var deleteUser = await _wareHouseService.deleteWareHouse(id);

        if (deleteUser is null)
        {
            return NotFound();
        }

        return Ok();
    }

    [HttpGet("product"), CustomAuthorizeAttribute(Roles.Admin | Roles.User)]
    public IActionResult wareHouseProducts()
    {
        var wareHouseProducts = _wareHouseService.WareHouseProducts();

        return Ok(wareHouseProducts);
    }
}
