using Microsoft.EntityFrameworkCore;
using School.Application.Interfaces.Common;
using School.Domain.Entities;
using School.Domain.Common;

namespace School.Infrastructure.Persistence.Context;

public class SchoolDbContext : DbContext
{

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfigurationsFromAssembly(typeof(SchoolDbContext).Assembly);
    }

   
        private readonly ICurrentUserService _currentUserService;

        public SchoolDbContext(
            DbContextOptions<SchoolDbContext> options,
            ICurrentUserService currentUserService)
            : base(options)
        {
            _currentUserService = currentUserService;
        }

    private void ApplyAuditInformation()
    {
        var currentUserId = _currentUserService.UserId;

        var entries = ChangeTracker
            .Entries<BaseEntity>()
            .Where(e => e.State == EntityState.Added || e.State == EntityState.Modified);

        foreach (var entry in entries)
        {
            if (entry.State == EntityState.Added)
            {
                entry.Entity.CreatedAt = DateTime.UtcNow;
                entry.Entity.CreatedBy = currentUserId;
            }

            if (entry.State == EntityState.Modified)
            {
                entry.Property(x => x.CreatedAt).IsModified = false;
                entry.Property(x => x.CreatedBy).IsModified = false;

                entry.Entity.UpdatedAt = DateTime.UtcNow;
                entry.Entity.UpdatedBy = currentUserId;
            }
        }
    }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        ApplyAuditInformation();
        return await base.SaveChangesAsync(cancellationToken);
    }


    public DbSet<User> Users => Set<User>();
    public DbSet<Role> Roles => Set<Role>();
    public DbSet<UserRole> UserRoles => Set<UserRole>();
    public DbSet<Teacher> Teachers => Set<Teacher>();
    public DbSet<Student> Students => Set<Student>();
    public DbSet<Subject> Subjects => Set<Subject>();
    public DbSet<AcademicYear> AcademicYears => Set<AcademicYear>();
    public DbSet<ClassRoom> ClassRooms => Set<ClassRoom>();
    public DbSet<Enrollment> Enrollments => Set<Enrollment>();
    public DbSet<ClassRoomSubjectTeacher> ClassRoomSubjectTeachers => Set<ClassRoomSubjectTeacher>();
}