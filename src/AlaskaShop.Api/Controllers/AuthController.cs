﻿using AlaskaShop.Api.Extensions;
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
    [ProducesResponseType(typeof(RegisterUserResponse), StatusCodes.Status200OK)]
    public async Task<IResult> RegisterUser(IMediator m, [FromBody] RegisterUserDto data)
    {
        var request = new RegisterUserRequest(data);
        return await m.SendCommand(request);
    }

    [HttpPost]
    [Route("/login")]
    [ProducesResponseType(typeof(LoginUserResponse), StatusCodes.Status200OK)]
    public async Task<IResult> LoginUser(IMediator m, [FromBody] LoginUserDto data)
    {
        var request = new LoginUserRequest(data);
        return await m.SendCommand(request);
    }
}
