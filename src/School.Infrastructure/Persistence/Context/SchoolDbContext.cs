using Microsoft.EntityFrameworkCore;
using School.Domain.Entities;

namespace School.Infrastructure.Persistence.Context;

public class SchoolDbContext : DbContext
{

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfigurationsFromAssembly(typeof(SchoolDbContext).Assembly);
    }

    public SchoolDbContext(DbContextOptions<SchoolDbContext> options)
        : base(options)
    {
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