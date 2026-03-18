using School.Application.Interfaces.Repositories;
using School.Domain.Entities;
using School.Infrastructure.Persistence.Context;

namespace School.Infrastructure.Persistence.Repositories;

public class UserRepository : GenericRepository<User>, IUserRepository
{
    public UserRepository(SchoolDbContext context) : base(context)
    {
    }
}