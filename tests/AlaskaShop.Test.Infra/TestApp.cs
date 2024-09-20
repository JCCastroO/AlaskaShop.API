using AlaskaShop.Infra;
using Microsoft.EntityFrameworkCore;

namespace AlaskaShop.Test.Infra;

public class TestApp : IDisposable
{
    protected Context _context;
    private bool _disposedValue;

    public TestApp()
    {
        DbContextOptionsBuilder<Context> optionsBuilder = new();
        optionsBuilder.UseInMemoryDatabase(Guid.NewGuid().ToString());
        _context = new Context(optionsBuilder.Options);
        _context.Database.EnsureDeleted();
        _context.Database.EnsureCreated();
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