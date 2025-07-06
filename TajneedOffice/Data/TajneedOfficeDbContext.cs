using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using TajneedOffice.Models;

namespace TajneedOffice.Data
{
    /// <summary>
    /// Main database context for the Tajneed Office system
    /// </summary>
    public class TajneedOfficeDbContext : IdentityDbContext<User>
    {
        public TajneedOfficeDbContext(DbContextOptions<TajneedOfficeDbContext> options)
            : base(options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.ConfigureWarnings(warnings => 
                warnings.Ignore(Microsoft.EntityFrameworkCore.Diagnostics.RelationalEventId.PendingModelChangesWarning));
        }

        // Core lookup tables
        public DbSet<Rank> Ranks { get; set; }
        public DbSet<Airbase> Airbases { get; set; }
        public DbSet<CandidateCategory> CandidateCategories { get; set; }

        // Main entities
        public DbSet<Candidate> Candidates { get; set; }
        
        // TEMPORARY - will be removed after migration
        public DbSet<CandidateTestScores> CandidateTestScores { get; set; }

        // New flexible test system
        public DbSet<TestType> TestTypes { get; set; }
        public DbSet<CategoryTestPath> CategoryTestPaths { get; set; }
        public DbSet<CandidateTestResult> CandidateTestResults { get; set; }
        public DbSet<FinalEvaluation> FinalEvaluations { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // Configure decimal precision for new test system
            builder.Entity<CandidateTestResult>(entity =>
            {
                entity.Property(e => e.NumericScore).HasPrecision(5, 2);
                entity.Property(e => e.WeightedScore).HasPrecision(5, 2);
            });

            builder.Entity<CategoryTestPath>(entity =>
            {
                entity.Property(e => e.Weight).HasPrecision(5, 2);
            });

            builder.Entity<TestType>(entity =>
            {
                entity.Property(e => e.MinScore).HasPrecision(5, 2);
                entity.Property(e => e.MaxScore).HasPrecision(5, 2);
                entity.Property(e => e.PassingScore).HasPrecision(5, 2);
            });

            builder.Entity<FinalEvaluation>(entity =>
            {
                entity.Property(e => e.RecruitmentTestsScore).HasPrecision(5, 2);
                entity.Property(e => e.RecruitmentTestsPercentage).HasPrecision(5, 2);
                entity.Property(e => e.MainCommitteeScore).HasPrecision(5, 2);
                entity.Property(e => e.MainCommitteePercentage).HasPrecision(5, 2);
                entity.Property(e => e.ProfessionalTestScore).HasPrecision(5, 2);
                entity.Property(e => e.ProfessionalTestPercentage).HasPrecision(5, 2);
                entity.Property(e => e.FinalPercentage).HasPrecision(5, 2);
            });

            // Configure decimal precision for legacy test scores
            builder.Entity<CandidateTestScores>(entity =>
            {
                entity.Property(e => e.EnglishScore).HasPrecision(18, 2);
                entity.Property(e => e.ArabicScore).HasPrecision(18, 2);
                entity.Property(e => e.MathematicsScore).HasPrecision(18, 2);
                entity.Property(e => e.GeneralCultureScore).HasPrecision(18, 2);
                entity.Property(e => e.FlightAptitudeScore).HasPrecision(18, 2);
                entity.Property(e => e.ProfessionalEvaluationScore).HasPrecision(18, 2);
                entity.Property(e => e.CommunicationSkillScore).HasPrecision(18, 2);
                entity.Property(e => e.DrivingSkillScore).HasPrecision(18, 2);
            });

            // Configure entity relationships and constraints

            // Rank relationships
            builder.Entity<Rank>()
                .HasIndex(r => r.RankName)
                .IsUnique();

            // Airbase relationships
            builder.Entity<Airbase>()
                .HasIndex(a => a.AirbaseName)
                .IsUnique();

            // CandidateCategory relationships
            builder.Entity<CandidateCategory>()
                .HasIndex(c => c.CategoryName)
                .IsUnique();

            builder.Entity<CandidateCategory>()
                .HasIndex(c => c.CategoryCode)
                .IsUnique();

            // User relationships
            builder.Entity<User>(entity =>
            {
                entity.HasIndex(u => u.ServiceNumber)
                    .IsUnique();
            });

            builder.Entity<User>()
                .HasOne(u => u.Rank)
                .WithMany(r => r.Users)
                .HasForeignKey(u => u.RankId)
                .OnDelete(DeleteBehavior.Restrict);

            // Candidate relationships
            builder.Entity<Candidate>()
                .HasIndex(c => c.NationalIdNumber)
                .IsUnique();

            builder.Entity<Candidate>()
                .HasOne(c => c.Category)
                .WithMany(cat => cat.Candidates)
                .HasForeignKey(c => c.CategoryId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Candidate>()
                .HasOne(c => c.CurrentRank)
                .WithMany(r => r.Candidates)
                .HasForeignKey(c => c.CurrentRankId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Candidate>()
                .HasOne(c => c.CurrentAirbase)
                .WithMany(a => a.Candidates)
                .HasForeignKey(c => c.CurrentAirbaseId)
                .OnDelete(DeleteBehavior.Restrict);

            // New test system relationships
            
            // TestType relationships
            builder.Entity<TestType>()
                .HasIndex(t => t.TestName)
                .IsUnique();

            builder.Entity<TestType>()
                .HasIndex(t => t.TestCode)
                .IsUnique();

            // CategoryTestPath relationships
            builder.Entity<CategoryTestPath>()
                .HasOne(ctp => ctp.Category)
                .WithMany()
                .HasForeignKey(ctp => ctp.CategoryId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<CategoryTestPath>()
                .HasOne(ctp => ctp.TestType)
                .WithMany(t => t.CategoryTestPaths)
                .HasForeignKey(ctp => ctp.TestTypeId)
                .OnDelete(DeleteBehavior.Restrict);

            // Composite index for CategoryTestPath
            builder.Entity<CategoryTestPath>()
                .HasIndex(ctp => new { ctp.CategoryId, ctp.TestTypeId })
                .IsUnique();

            // CandidateTestResult relationships
            builder.Entity<CandidateTestResult>()
                .HasOne(ctr => ctr.Candidate)
                .WithMany(c => c.TestResults)
                .HasForeignKey(ctr => ctr.CandidateId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<CandidateTestResult>()
                .HasOne(ctr => ctr.TestType)
                .WithMany(t => t.CandidateTestResults)
                .HasForeignKey(ctr => ctr.TestTypeId)
                .OnDelete(DeleteBehavior.Restrict);

            // Composite index for CandidateTestResult
            builder.Entity<CandidateTestResult>()
                .HasIndex(ctr => new { ctr.CandidateId, ctr.TestTypeId })
                .IsUnique();

            // FinalEvaluation relationships
            builder.Entity<FinalEvaluation>()
                .HasOne(fe => fe.Candidate)
                .WithMany()
                .HasForeignKey(fe => fe.CandidateId)
                .OnDelete(DeleteBehavior.Cascade);

            // Seed initial data
            SeedInitialData(builder);
        }

        private void SeedInitialData(ModelBuilder builder)
        {
            // Seed Ranks
            builder.Entity<Rank>().HasData(
                new Rank { RankId = 1, RankName = "جندي", RankCode = "RECRUIT", RankOrder = 1 },
                new Rank { RankId = 2, RankName = "نائب عريف", RankCode = "LANCE_CPL", RankOrder = 2 },
                new Rank { RankId = 3, RankName = "عريف", RankCode = "CORPORAL", RankOrder = 3 },
                new Rank { RankId = 4, RankName = "رقيب", RankCode = "SERGEANT", RankOrder = 4 },
                new Rank { RankId = 5, RankName = "رقيب أول", RankCode = "STAFF_SGT", RankOrder = 5 },
                new Rank { RankId = 6, RankName = "وكيل", RankCode = "WARRANT", RankOrder = 6 },
                new Rank { RankId = 7, RankName = "وكيل أول", RankCode = "CHIEF_WARRANT", RankOrder = 7 },
                new Rank { RankId = 8, RankName = "ضابط مرشح", RankCode = "CADET", RankOrder = 8 },
                new Rank { RankId = 9, RankName = "ملازم ثاني", RankCode = "SECOND_LT", RankOrder = 9 },
                new Rank { RankId = 10, RankName = "ملازم أول", RankCode = "FIRST_LT", RankOrder = 10 },
                new Rank { RankId = 11, RankName = "نقيب", RankCode = "CAPTAIN", RankOrder = 11 },
                new Rank { RankId = 12, RankName = "رائد", RankCode = "MAJOR", RankOrder = 12 },
                new Rank { RankId = 13, RankName = "مقدم", RankCode = "LT_COLONEL", RankOrder = 13 },
                new Rank { RankId = 14, RankName = "عقيد", RankCode = "COLONEL", RankOrder = 14 },
                new Rank { RankId = 15, RankName = "عميد", RankCode = "BRIGADIER", RankOrder = 15 },
                new Rank { RankId = 16, RankName = "لواء", RankCode = "MAJ_GENERAL", RankOrder = 16 },
                new Rank { RankId = 17, RankName = "فريق", RankCode = "LT_GENERAL", RankOrder = 17 },
                new Rank { RankId = 18, RankName = "مدني درجة 16", RankCode = "CIV_G16", RankOrder = 18 },
                new Rank { RankId = 19, RankName = "مدني درجة 15", RankCode = "CIV_G15", RankOrder = 19 },
                new Rank { RankId = 20, RankName = "مدني درجة 14", RankCode = "CIV_G14", RankOrder = 20 },
                new Rank { RankId = 21, RankName = "مدني درجة 13", RankCode = "CIV_G13", RankOrder = 21 },
                new Rank { RankId = 22, RankName = "مدني درجة 12", RankCode = "CIV_G12", RankOrder = 22 },
                new Rank { RankId = 23, RankName = "مدني درجة 11", RankCode = "CIV_G11", RankOrder = 23 },
                new Rank { RankId = 24, RankName = "مدني درجة 10", RankCode = "CIV_G10", RankOrder = 24 },
                new Rank { RankId = 25, RankName = "مدني درجة 9", RankCode = "CIV_G9", RankOrder = 25 },
                new Rank { RankId = 26, RankName = "ضابط مدني د8", RankCode = "CIV_OFF_G8", RankOrder = 26 },
                new Rank { RankId = 27, RankName = "ضابط مدني د9", RankCode = "CIV_OFF_G9", RankOrder = 27 },
                new Rank { RankId = 28, RankName = "ضابط مدني د7", RankCode = "CIV_OFF_G7", RankOrder = 28 },
                new Rank { RankId = 29, RankName = "ضابط مدني د6", RankCode = "CIV_OFF_G6", RankOrder = 29 },
                new Rank { RankId = 30, RankName = "ضابط مدني د5", RankCode = "CIV_OFF_G5", RankOrder = 30 },
                new Rank { RankId = 31, RankName = "ضابط مدني د4", RankCode = "CIV_OFF_G4", RankOrder = 31 },
                new Rank { RankId = 32, RankName = "ضابط مدني د3", RankCode = "CIV_OFF_G3", RankOrder = 32 },
                new Rank { RankId = 33, RankName = "ضابط مدني د2", RankCode = "CIV_OFF_G2", RankOrder = 33 },
                new Rank { RankId = 34, RankName = "ضابط مدني د1", RankCode = "CIV_OFF_G1", RankOrder = 34 }
            );

            // Seed Airbases
            builder.Entity<Airbase>().HasData(
                new Airbase { AirbaseId = 1, AirbaseName = "قيادة سلاح الجو السلطاني العماني", IsActive = true },
                new Airbase { AirbaseId = 2, AirbaseName = "قاعدة غلا وأكاديمية السلطان قابوس الجوية", IsActive = true },
                new Airbase { AirbaseId = 3, AirbaseName = "قاعدة السيب الجوية", IsActive = true },
                new Airbase { AirbaseId = 4, AirbaseName = "قاعدة صلالة الجوية", IsActive = true },
                new Airbase { AirbaseId = 5, AirbaseName = "قاعدة المصنعة الجوية", IsActive = true },
                new Airbase { AirbaseId = 6, AirbaseName = "قاعدة مصيرة الجوية", IsActive = true },
                new Airbase { AirbaseId = 7, AirbaseName = "قاعدة أدم الجوية", IsActive = true },
                new Airbase { AirbaseId = 8, AirbaseName = "قاعدة ثمريت الجوية", IsActive = true },
                new Airbase { AirbaseId = 9, AirbaseName = "قاعدة خصب الجوية", IsActive = true }
            );

            // Seed Candidate Categories
            builder.Entity<CandidateCategory>().HasData(
                new CandidateCategory { CategoryId = 1, CategoryCode = "CAT-PLT", CategoryName = "المرشحيين الطيارين", Description = "فئة المرشحين للالتحاق بسلاح الجو السلطاني العماني كمرشحين طيارين." },
                new CandidateCategory { CategoryId = 2, CategoryCode = "CAT-MUG", CategoryName = "المرشحيين الجامعيين العسكريين", Description = "خريجوا الكليات والجامعات من حملة البكالوريوس ويتم تقيمهم بناءا على تخصصاتهم المهنية وينضموا للسلاح كضباط عسكريين جامعيين" },
                new CandidateCategory { CategoryId = 3, CategoryCode = "CAT-CUG", CategoryName = "المرشحيين الجامعيين المدنيين", Description = "خريجوا الكليات والجامعات من حملة البكالوريوس ويتم تقيمهم بناءا على تخصصاتهم المهنية وينضموا للسلاح كضباط مدنيين جامعيين" },
                new CandidateCategory { CategoryId = 4, CategoryCode = "CAT-LSO", CategoryName = "ضباط الخدمة المحدودة", Description = "ضباط الصف من ذوي الخدمة المحدودة وسسبق لهم العمل في السلاح وهم برتبة وكيل فأعلى" },
                new CandidateCategory { CategoryId = 5, CategoryCode = "CAT-NCO", CategoryName = "ضباط الصف ( رقباء / عرفاء)", Description = "ضباط صف برتبة رقيب أو عريف سبق لهم العمل بالسلاح" },
                new CandidateCategory { CategoryId = 6, CategoryCode = "CAT-TCN", CategoryName = "ضباط الصف الكلية التقنية العسكرية", Description = "ضباط صف خريجوا الكلية العسكرية التقنية" },
                new CandidateCategory { CategoryId = 7, CategoryCode = "CAT-CNP", CategoryName = "ضباط الصف المدنيين للترفيع", Description = "ضباط صف مدنيين مرشحين للترقية بالصفة المدنية" }
            );
        }
    }
} 