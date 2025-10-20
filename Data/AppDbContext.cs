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



    protected override void OnModelCreating(ModelBuilder b)
    {

        b.Entity<User>().HasIndex(x => x.Email).IsUnique();
        b.Entity<Region>().HasIndex(x => x.Slug).IsUnique();
        b.Entity<Symptom>().HasIndex(x => x.Slug).IsUnique();
        b.Entity<RegionSymptom>()
           .HasIndex(x => new { x.RegionId, x.SymptomId })
              .IsUnique();

        base.OnModelCreating(b);

    }

}