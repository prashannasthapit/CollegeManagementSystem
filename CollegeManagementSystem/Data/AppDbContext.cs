using CollegeManagementSystem.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace CollegeManagementSystem.Data;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<Student> Students { get; set; } = null!;
    public DbSet<Course> Courses { get; set; } = null!;
    public DbSet<Module> Modules { get; set; } = null!;
    public DbSet<Instructor> Instructors { get; set; } = null!;
    public DbSet<Enrollment> Enrollments { get; set; } = null!;
    public DbSet<ModuleInstructor> ModuleInstructors { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Module>()
            .HasOne(m => m.Course)
            .WithMany(c => c.Modules)
            .HasForeignKey(m => m.CourseId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Enrollment>()
            .HasKey(e => new { e.StudentId, e.CourseId });

        modelBuilder.Entity<Enrollment>()
            .HasOne(e => e.Student)
            .WithMany(s => s.Enrollments)
            .HasForeignKey(e => e.StudentId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Enrollment>()
            .HasOne(e => e.Course)
            .WithMany(c => c.Enrollments)
            .HasForeignKey(e => e.CourseId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<ModuleInstructor>()
            .HasKey(mi => new { mi.ModuleId, mi.InstructorId });

        modelBuilder.Entity<ModuleInstructor>()
            .HasOne(mi => mi.Module)
            .WithMany(m => m.ModuleInstructors)
            .HasForeignKey(mi => mi.ModuleId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<ModuleInstructor>()
            .HasOne(mi => mi.Instructor)
            .WithMany(i => i.ModuleInstructors)
            .HasForeignKey(mi => mi.InstructorId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}