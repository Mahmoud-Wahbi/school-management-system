using Microsoft.EntityFrameworkCore;
using School.Domain.Entities;
using School.Infrastructure.Persistence.Context;
using School.Infrastructure.Security;

namespace School.Infrastructure.Persistence.Seed;

public static class DbInitializer
{
    public static async Task SeedAsync(SchoolDbContext context)
    {
        // Apply pending migrations (create/update database schema)
        await context.Database.MigrateAsync();

        // Ensure roles exist (Admin + Teacher)
        if (!await context.Roles.AnyAsync(r => r.Name == "Admin"))
        {
            await context.Roles.AddAsync(new Role
            {
                Id = Guid.NewGuid(),
                Name = "Admin",
                Description = "System Administrator"
            });
        }

        if (!await context.Roles.AnyAsync(r => r.Name == "Teacher"))
        {
            await context.Roles.AddAsync(new Role
            {
                Id = Guid.NewGuid(),
                Name = "Teacher",
                Description = "Teacher Role"
            });
        }

        // Save roles before using them
        await context.SaveChangesAsync();

        var passwordHasher = new PasswordHasher();

        // Seed Admin user if not exists
        if (!await context.Users.AnyAsync(u => u.Email == "admin@school.com"))
        {
            var adminUser = new User
            {
                Id = Guid.NewGuid(),
                FullName = "System Admin",
                Email = "admin@school.com",
                PasswordHash = passwordHasher.Hash("Admin@123"), // store hashed password
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            };

            await context.Users.AddAsync(adminUser);
            await context.SaveChangesAsync();

            var adminRole = await context.Roles.FirstAsync(r => r.Name == "Admin");

            // Link Admin user to Admin role
            await context.UserRoles.AddAsync(new UserRole
            {
                UserId = adminUser.Id,
                RoleId = adminRole.Id
            });

            await context.SaveChangesAsync();
        }

        // Seed Teacher user if not exists
        if (!await context.Users.AnyAsync(u => u.Email == "teacher@school.com"))
        {
            var teacherUser = new User
            {
                Id = Guid.NewGuid(),
                FullName = "Teacher User",
                Email = "teacher@school.com",
                PasswordHash = passwordHasher.Hash("Teacher@123"),
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            };

            await context.Users.AddAsync(teacherUser);
            await context.SaveChangesAsync();

            var teacherRole = await context.Roles.FirstAsync(r => r.Name == "Teacher");

            // Link Teacher user to Teacher role
            await context.UserRoles.AddAsync(new UserRole
            {
                UserId = teacherUser.Id,
                RoleId = teacherRole.Id
            });

            await context.SaveChangesAsync();
        }
    }
}