namespace School.Application.Interfaces.Repositories;

public interface IUnitOfWork
{
    IStudentRepository Students { get; }
    IUserRepository Users { get; }


    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}