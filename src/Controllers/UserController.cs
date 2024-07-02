using RestApiSample.Middleware;
using JsonFlatFileDataStore;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RestApiSample.Models;
using RestApiSample.Services;
using RestApiSample.Interfaces;


namespace RestApiSample.Controllers;

[ApiController]
[Route("[controller]")]
public class UserController : BaseController
{

    private readonly ILogger<UserController> _logger;
    private readonly IDocumentCollection<User> _users;
    private readonly UserService _userService;


    public UserController(ILogger<UserController> logger, UserService userService)
    {
        _logger = logger;
        var store = new DataStore("db.json");
        _users = store.GetCollection<User>();
        _userService = userService;
    }

    [HttpPost, CustomAuthorizeAttribute(Roles.Admin)]
    public async Task<IActionResult> Post([FromBody] IUser user)
    {

        var email = getJwtPayload("email");
        var result = await _userService.createUser(email, user);

        return result.GetActionResult();
    }

    [HttpGet, CustomAuthorizeAttribute(Roles.Admin)]
    public IActionResult Get()
    {
        var users = _userService.getUsers();
        return users.GetActionResult();
    }

    [HttpGet("{id:int}"), CustomAuthorizeAttribute(Roles.Admin | Roles.User)]
    [Authorize]
    public async Task<IActionResult> GetById(int id)
    {
        // return _users.AsQueryable().FirstOrDefault(user => user.id == id);
        var result = await _userService.getUser(id);

        return result.GetActionResult();
    }

    [HttpPut("{id:int}"), CustomAuthorizeAttribute(Roles.Admin | Roles.User)]
    [Authorize]
    public async Task<IActionResult> Put(int id, [FromBody] User user)
    {
        // var findUser = _users.AsQueryable().FirstOrDefault(user => user.id == id);

        // if (findUser == null) return null;

        // findUser = user;
        // await _users.UpdateOneAsync(user => user.id == id, findUser);

        // return findUser;


        await _userService.updateUser(id, user);

        return Ok(user);
    }

    [HttpDelete("{id:int}"), CustomAuthorizeAttribute(Roles.Admin)]
    [Authorize]
    public async Task<IActionResult> delete(int id)
    {
        var deleteUser = await _userService.deleteUser(id);

        if (deleteUser is null)
        {
            return NotFound();
        }

        return Ok();
    }
}
