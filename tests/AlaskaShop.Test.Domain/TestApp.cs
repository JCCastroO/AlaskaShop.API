using AlaskaShop.Domain.Services.AutoMapper.Auth;
using AlaskaShop.Infra;
using AlaskaShop.Infra.Entities;
using AlaskaShop.Shareable.Dtos.Auth;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using NSubstitute;

namespace AlaskaShop.Test.Domain;

public class TestApp : IDisposable
{
    protected Context _context;
    protected IMapper _mapper;
    private bool _disposedValue;

    public TestApp()
    {
        DbContextOptionsBuilder<Context> optionsBuilder = new();
        optionsBuilder.UseInMemoryDatabase(Guid.NewGuid().ToString());
        _context = new Context(optionsBuilder.Options);
        _context.Database.EnsureDeleted();
        _context.Database.EnsureCreated();
        var configuration = new MapperConfiguration(config =>
        {
            config.AddProfile<RegisterUserProfile>();
        });
        _mapper = new Mapper(configuration);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (!_disposedValue)
        {
            if (disposing)
            {
                _context.Dispose();
            }

            _disposedValue = true;
        }
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }
}