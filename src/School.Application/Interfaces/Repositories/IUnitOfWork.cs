namespace School.Application.Interfaces.Repositories;

public interface IUnitOfWork
{
    IStudentRepository Students { get; }
    IUserRepository Users { get; }
    IRefreshTokenRepository RefreshTokens { get; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}