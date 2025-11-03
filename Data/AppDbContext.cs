using Microsoft.EntityFrameworkCore;
using SaleemCare.Api.Domain.Entities;

namespace SaleemCare.Api.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> opt) : base(opt) { }

    public DbSet<Region> Regions => Set<Region>();
    public DbSet<Symptom> Symptoms => Set<Symptom>();
    public DbSet<RegionSymptom> RegionSymptoms => Set<RegionSymptom>();
    public DbSet<SymptomQuestion> SymptomQuestions => Set<SymptomQuestion>();
    public DbSet<UserProfile> UserProfiles => Set<UserProfile>();
    public DbSet<UserSymptomAnswer> UserSymptomAnswers => Set<UserSymptomAnswer>();
    public DbSet<User> Users => Set<User>();
    public DbSet<Encounter> Encounters => Set<Encounter>();
    public DbSet<Role> Roles => Set<Role>();
    public DbSet<UserRole> UserRoles => Set<UserRole>();



    protected override void OnModelCreating(ModelBuilder b)
    {

        b.Entity<User>().HasIndex(x => x.Email).IsUnique();
        b.Entity<Region>().HasIndex(x => x.Slug).IsUnique();
        b.Entity<Symptom>().HasIndex(x => x.Slug).IsUnique();
        b.Entity<RegionSymptom>()
           .HasIndex(x => new { x.RegionId, x.SymptomId })
              .IsUnique();

        b.Entity<Encounter>().HasIndex(x => new { x.UserId, x.StartedAt });

        b.Entity<UserSymptomAnswer>(e =>
        {
            e.HasIndex(x => new { x.UserId, x.SymptomId, x.SymptomQuestionId });

           
            e.HasOne(x => x.Encounter)
             .WithMany()
             .HasForeignKey(x => x.EncounterId)
             .OnDelete(DeleteBehavior.SetNull);

        });

        b.Entity<Role>().HasIndex(r => r.Name).IsUnique();

        b.Entity<UserRole>().HasKey(ur => new { ur.UserId, ur.RoleId });

        b.Entity<UserRole>()
            .HasOne(ur => ur.User).WithMany()
            .HasForeignKey(ur => ur.UserId);
        b.Entity<UserRole>()
            .HasOne(ur => ur.Role).WithMany()
            .HasForeignKey(ur => ur.RoleId);

        base.OnModelCreating(b);


    }

}