using Microsoft.EntityFrameworkCore;
using SaleemCare.Api.Domain.Entities;
using System.Threading.Tasks;

namespace SaleemCare.Api.Data.Seed;

public static class DbInitializer
{
    public static async Task SeedAsync(AppDbContext db)

    {

        // Regions
        await EnsureRegion(db, "head", "الرأس");
        await EnsureRegion(db, "chest", "الصدر");
        await EnsureRegion(db, "abdomen", "البطن");

        // Symptoms
        await EnsureSymptom(db, "headache", "صداع", "ألم بالرأس قد يكون نابضًا أو مستمرًا");
        await EnsureSymptom(db, "cough", "سعال", "سعال جاف أو ببلغم.");
        await EnsureSymptom(db, "nausea", "غثيان", "إحساس بعدم الراحة في المعدة مع رغبة بالتقيؤ.");

        await db.SaveChangesAsync();


        // Region-Symptom Association (map)
        await LinkRegionSymptom(db, "head", "headache");
        await LinkRegionSymptom(db, "chest", "cough");
        await LinkRegionSymptom(db, "abdomen", "nausea");


        // ---------- Symptom questions (examples) ----------

        await EnsureQuestion(db, "headache", order: 1, type: "single", questionAr: "منذ متى بدأ الصداع؟",
            optionsAr: new[] { "أقل من يوم", "1-3 أيام", "أكثر من أسبوع" });

        await EnsureQuestion(db, "cough", order: 2, type: "bool", questionAr: "هل يزداد الصداع مع الضوء أو الصوت؟",
            optionsAr: null);

        await EnsureQuestion(db, "cough", order: 1, type: "single",
            questionAr: "هل السعال مصحوب ببلغم؟",
            optionsAr: new[] { "نعم", "لا" });

        await EnsureQuestion(db, "nausea", order: 1, type: "single",
            questionAr: "هل الغثيان مرتبط بتناول الطعام؟",
            optionsAr: new[] { "قبل الأكل", "بعد الأكل", "لا علاقة" });


        if (!await db.Roles.AnyAsync())
        {
            db.Roles.AddRange(
                new Role { Name = "Admin" },
                new Role { Name = "Clinician" },
                new Role { Name = "Patient" }

                );
        }

        await db.SaveChangesAsync();

    }

    // helpers

    private static async Task EnsureRegion(AppDbContext db, string slug, string nameAr)
    {
        var r = await db.Regions.FirstOrDefaultAsync(x => x.Slug == slug);

        if (r is null)
        {
            db.Regions.Add(new Region { Slug = slug, NameAr = nameAr });
        }
    }

    private static async Task EnsureSymptom(AppDbContext db, string slug, string nameAr, string? descriptionAr)
    {
        var s = await db.Symptoms.FirstOrDefaultAsync(x => x.Slug == slug);

        if (s is null)
        {
            db.Symptoms.Add(new Symptom { Slug = slug, NameAr = nameAr, DescriptionAr = descriptionAr });
        }
    }

    private static async Task LinkRegionSymptom(AppDbContext db, string regionSlug, string symptomSlug)
    {
        var region = await db.Regions.FirstOrDefaultAsync(r => r.Slug == regionSlug);
        var symptom = await db.Symptoms.FirstOrDefaultAsync(s => s.Slug == symptomSlug);

        var exists = await db.RegionSymptoms
            .AnyAsync(x => x.RegionId == region.Id && x.SymptomId == symptom.Id);

        if (!exists)
        {
            db.RegionSymptoms.Add(new RegionSymptom { RegionId = region.Id, SymptomId = symptom.Id });
            await db.SaveChangesAsync();
        }
    }

    private static async Task EnsureQuestion(AppDbContext db, string symptomSlug, byte order, string type, string questionAr, string[]? optionsAr)
    {
        var symptomId = await db.Symptoms
            .Where(s => s.Slug == symptomSlug)
            .Select(s => s.Id)
            .FirstAsync();

        var exists = await db.SymptomQuestions
            .AnyAsync(q => q.SymptomId == symptomId && q.Order == order);

        if (!exists)
        {
            db.SymptomQuestions.Add(new SymptomQuestion
            {
                SymptomId = symptomId,
                Order = order,
                Type = type,
                QuestionAr = questionAr,
                OptionsArJson = optionsAr is null ? null : System.Text.Json.JsonSerializer.Serialize(optionsAr)
            });
        }

    }

  
}