using AlaskaShop.Api.Extensions;
using AlaskaShop.Domain.Services.Token;
using AlaskaShop.Shareable.Dtos;
using AlaskaShop.Shareable.Dtos.Product;
using AlaskaShop.Shareable.Request.Product;
using AlaskaShop.Shareable.Response.Auth;
using AlaskaShop.Shareable.Response.Product;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AlaskaShop.Api.Controllers;

[Authorize]
public class ProductController : BaseController
{
    private readonly TokenValidator _validator;

    public ProductController(TokenValidator validator)
        => _validator = validator;

    [HttpPost]
    [Route("[controller]/register")]
    [ProducesResponseType(typeof(RegisterProductResponse), StatusCodes.Status200OK)]
    public async Task<IResult> RegisterProduct(IMediator m, [FromBody] RegisterProductDto data)
    {
        var token = await GetToken(Request.Headers.Authorization.ToString());
        var userIdentifier = _validator.Validate(token);
        var request = new RegisterProductRequest(data, userIdentifier);
        return await m.SendCommand(request);
    }

    [HttpGet]
    [Route("[controller]/list")]
    [ProducesResponseType(typeof(ListProductResponse), StatusCodes.Status200OK)]
    public async Task<IResult> ListProducts(IMediator m, [FromQuery] ListProductDto list, [FromQuery] int page, [FromQuery] int pageSize)
    {
        var pagination = new PaginationDto() { Page = page, PageSize = pageSize };
        var request = new ListProductRequest(list, pagination);
        return await m.SendCommand(request);
    }

    [HttpGet]
    [Route("[controller]/{id}")]
    [ProducesResponseType(typeof(ProductByIdResponse), StatusCodes.Status200OK)]
    public async Task<IResult> ListProducts(IMediator m, long id)
    {
        var request = new ProductByIdRequest(id);
        return await m.SendCommand(request);
    }

    private static async Task<string> GetToken(string authHeader)
    {
        await ValidateHeader(authHeader);
        var token = authHeader["Bearer ".Length..].Trim();
        return token;
    }

    private static async Task<IResult> ValidateHeader(string authHeader)
    {
        await Task.Run(() => { });
        if (authHeader is null || !authHeader.StartsWith("Bearer "))
            return Results.Unauthorized();

        return Results.Ok();
    }
}
