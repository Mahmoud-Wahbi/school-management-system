using School.Application.Interfaces.Repositories;
using School.Infrastructure.Persistence.Context;
using School.Infrastructure.Persistence.Repositories;

namespace School.Infrastructure.Persistence.UnitOfWork;

public class UnitOfWork : IUnitOfWork
{
    private readonly SchoolDbContext _context;

    public IStudentRepository Students { get; }
    public IUserRepository Users { get; }
    public IRefreshTokenRepository RefreshTokens { get; }

    public UnitOfWork(SchoolDbContext context)
    {
        _context = context;
        Students = new StudentRepository(_context);
        Users = new UserRepository(_context);
        RefreshTokens = new RefreshTokenRepository(_context);
    }

    public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return await _context.SaveChangesAsync(cancellationToken);
    }
}