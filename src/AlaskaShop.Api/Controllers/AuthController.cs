using AlaskaShop.Api.Extensions;
using AlaskaShop.Shareable.Dtos.Auth;
using AlaskaShop.Shareable.Request.Auth;
using AlaskaShop.Shareable.Response.Auth;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace AlaskaShop.Api.Controllers;

public class AuthController : BaseController
{
    [HttpPost]
    [Route("/register")]
    [ProducesResponseType(typeof(RegisterUserResponse), StatusCodes.Status201Created)]
    public async Task<IResult> RegisterUser(IMediator m, [FromBody] RegisterUserDto data)
    {
        var request = new RegisterUserRequest(data);
        return await m.SendCommand(request);
    }
}
