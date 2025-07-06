using Microsoft.EntityFrameworkCore;
using TajneedOffice.Models;

namespace TajneedOffice.Data
{
    public static class DbInitializer
    {
        public static void Initialize(TajneedOfficeDbContext context)
        {
            // Ensure database is created
            context.Database.EnsureCreated();

            // Check if database already has data
            if (context.CandidateCategories.Any())
            {
                // Add flexible test system data if not exists
                InitializeFlexibleTestSystem(context);
                return; // DB has been seeded
            }

            // Seed Categories
            var categories = new CandidateCategory[]
            {
                new CandidateCategory { CategoryCode = "CAT-PLT", CategoryName = "المرشحيين الطيارين", Description = "فئة المرشحين للالتحاق بسلاح الجو السلطاني العماني كمرشحين طيارين." },
                new CandidateCategory { CategoryCode = "CAT-MUG", CategoryName = "المرشحيين الجامعيين العسكريين", Description = "خريجوا الكليات والجامعات من حملة البكالوريوس ويتم تقيمهم بناءا على تخصصاتهم المهنية وينضموا للسلاح كضباط عسكريين جامعيين" },
                new CandidateCategory { CategoryCode = "CAT-CUG", CategoryName = "المرشحيين الجامعيين المدنيين", Description = "خريجوا الكليات والجامعات من حملة البكالوريوس ويتم تقيمهم بناءا على تخصصاتهم المهنية وينضموا للسلاح كضباط مدنيين جامعيين" },
                new CandidateCategory { CategoryCode = "CAT-LSO", CategoryName = "ضباط الخدمة المحدودة", Description = "ضباط الصف من ذوي الخدمة المحدودة وسسبق لهم العمل في السلاح وهم برتبة وكيل فأعلى" },
                new CandidateCategory { CategoryCode = "CAT-NCO", CategoryName = "ضباط الصف ( رقباء / عرفاء)", Description = "ضباط صف برتبة رقيب أو عريف سبق لهم العمل بالسلاح" },
                new CandidateCategory { CategoryCode = "CAT-TCN", CategoryName = "ضباط الصف الكلية التقنية العسكرية", Description = "ضباط صف خريجوا الكلية العسكرية التقنية" },
                new CandidateCategory { CategoryCode = "CAT-CNP", CategoryName = "ضباط الصف المدنيين للترفيع", Description = "ضباط صف مدنيين مرشحين للترقية بالصفة المدنية" }
            };

            foreach (var category in categories)
            {
                context.CandidateCategories.Add(category);
            }
            context.SaveChanges();

            // Seed Ranks
            var ranks = new Rank[]
            {
                new Rank { RankName = "جندي", RankCode = "RECRUIT", RankOrder = 1 },
                new Rank { RankName = "نائب عريف", RankCode = "LANCE_CPL", RankOrder = 2 },
                new Rank { RankName = "عريف", RankCode = "CORPORAL", RankOrder = 3 },
                new Rank { RankName = "رقيب", RankCode = "SERGEANT", RankOrder = 4 },
                new Rank { RankName = "رقيب أول", RankCode = "STAFF_SGT", RankOrder = 5 },
                new Rank { RankName = "وكيل", RankCode = "WARRANT", RankOrder = 6 },
                new Rank { RankName = "وكيل أول", RankCode = "CHIEF_WARRANT", RankOrder = 7 },
                new Rank { RankName = "ضابط مرشح", RankCode = "CADET", RankOrder = 8 },
                new Rank { RankName = "ملازم ثاني", RankCode = "SECOND_LT", RankOrder = 9 },
                new Rank { RankName = "ملازم أول", RankCode = "FIRST_LT", RankOrder = 10 },
                new Rank { RankName = "نقيب", RankCode = "CAPTAIN", RankOrder = 11 },
                new Rank { RankName = "رائد", RankCode = "MAJOR", RankOrder = 12 },
                new Rank { RankName = "مقدم", RankCode = "LT_COLONEL", RankOrder = 13 },
                new Rank { RankName = "عقيد", RankCode = "COLONEL", RankOrder = 14 },
                new Rank { RankName = "عميد", RankCode = "BRIGADIER", RankOrder = 15 },
                new Rank { RankName = "لواء", RankCode = "MAJ_GENERAL", RankOrder = 16 },
                new Rank { RankName = "فريق", RankCode = "LT_GENERAL", RankOrder = 17 },
                new Rank { RankName = "مدني درجة 16", RankCode = "CIV_G16", RankOrder = 18 },
                new Rank { RankName = "مدني درجة 15", RankCode = "CIV_G15", RankOrder = 19 },
                new Rank { RankName = "مدني درجة 14", RankCode = "CIV_G14", RankOrder = 20 },
                new Rank { RankName = "مدني درجة 13", RankCode = "CIV_G13", RankOrder = 21 },
                new Rank { RankName = "مدني درجة 12", RankCode = "CIV_G12", RankOrder = 22 },
                new Rank { RankName = "مدني درجة 11", RankCode = "CIV_G11", RankOrder = 23 },
                new Rank { RankName = "مدني درجة 10", RankCode = "CIV_G10", RankOrder = 24 },
                new Rank { RankName = "مدني درجة 9", RankCode = "CIV_G9", RankOrder = 25 },
                new Rank { RankName = "ضابط مدني د8", RankCode = "CIV_OFF_G8", RankOrder = 26 },
                new Rank { RankName = "ضابط مدني د9", RankCode = "CIV_OFF_G9", RankOrder = 27 },
                new Rank { RankName = "ضابط مدني د7", RankCode = "CIV_OFF_G7", RankOrder = 28 },
                new Rank { RankName = "ضابط مدني د6", RankCode = "CIV_OFF_G6", RankOrder = 29 },
                new Rank { RankName = "ضابط مدني د5", RankCode = "CIV_OFF_G5", RankOrder = 30 },
                new Rank { RankName = "ضابط مدني د4", RankCode = "CIV_OFF_G4", RankOrder = 31 },
                new Rank { RankName = "ضابط مدني د3", RankCode = "CIV_OFF_G3", RankOrder = 32 },
                new Rank { RankName = "ضابط مدني د2", RankCode = "CIV_OFF_G2", RankOrder = 33 },
                new Rank { RankName = "ضابط مدني د1", RankCode = "CIV_OFF_G1", RankOrder = 34 }
            };

            foreach (var rank in ranks)
            {
                context.Ranks.Add(rank);
            }
            context.SaveChanges();

            // Seed Airbases
            var airbases = new Airbase[]
            {
                new Airbase { AirbaseName = "قيادة سلاح الجو السلطاني العماني", IsActive = true },
                new Airbase { AirbaseName = "قاعدة غلا وأكاديمية السلطان قابوس الجوية", IsActive = true },
                new Airbase { AirbaseName = "قاعدة السيب الجوية", IsActive = true },
                new Airbase { AirbaseName = "قاعدة صلالة الجوية", IsActive = true },
                new Airbase { AirbaseName = "قاعدة المصنعة الجوية", IsActive = true },
                new Airbase { AirbaseName = "قاعدة مصيرة الجوية", IsActive = true },
                new Airbase { AirbaseName = "قاعدة أدم الجوية", IsActive = true },
                new Airbase { AirbaseName = "قاعدة ثمريت الجوية", IsActive = true },
                new Airbase { AirbaseName = "قاعدة خصب الجوية", IsActive = true }
            };

            foreach (var airbase in airbases)
            {
                context.Airbases.Add(airbase);
            }
            context.SaveChanges();

            // Initialize flexible test system
            InitializeFlexibleTestSystem(context);

            // Seed demo candidates if none exist
            if (!context.Candidates.Any())
            {
                SeedDemoCandidates(context);
            }
        }

        private static void InitializeFlexibleTestSystem(TajneedOfficeDbContext context)
        {
            // Check if test types already exist
            if (context.TestTypes.Any())
            {
                return; // Already seeded
            }

            // Add Test Types
            var testTypes = new TestType[]
            {
                new TestType 
                { 
                    TestName = "اختبار اللغة الإنجليزية", 
                    TestCode = "ENG", 
                    Description = "اختبار مهارات اللغة الإنجليزية الأساسية", 
                    CriteriaType = "درجات", 
                    MinScore = 0, 
                    MaxScore = 100, 
                    PassingScore = 50, 
                    IsActive = true, 
                    CreatedDate = DateTime.Now 
                },
                new TestType 
                { 
                    TestName = "اختبار اللغة العربية", 
                    TestCode = "ARB", 
                    Description = "اختبار مهارات اللغة العربية والإملاء", 
                    CriteriaType = "درجات", 
                    MinScore = 0, 
                    MaxScore = 100, 
                    PassingScore = 60, 
                    IsActive = true, 
                    CreatedDate = DateTime.Now 
                },
                new TestType 
                { 
                    TestName = "اختبار الرياضيات", 
                    TestCode = "MTH", 
                    Description = "اختبار المهارات الرياضية الأساسية", 
                    CriteriaType = "درجات", 
                    MinScore = 0, 
                    MaxScore = 100, 
                    PassingScore = 50, 
                    IsActive = true, 
                    CreatedDate = DateTime.Now 
                },
                new TestType 
                { 
                    TestName = "اختبار اللياقة البدنية", 
                    TestCode = "FIT", 
                    Description = "تقييم اللياقة البدنية العامة", 
                    CriteriaType = "لائق_غير_لائق", 
                    IsActive = true, 
                    CreatedDate = DateTime.Now 
                },
                new TestType 
                { 
                    TestName = "الفحص الطبي", 
                    TestCode = "MED", 
                    Description = "الفحص الطبي الشامل", 
                    CriteriaType = "لائق_غير_لائق", 
                    IsActive = true, 
                    CreatedDate = DateTime.Now 
                },
                new TestType 
                { 
                    TestName = "اختبار الكفاءة الطيرانية", 
                    TestCode = "FLIGHT", 
                    Description = "اختبار مهارات الطيران الأساسية", 
                    CriteriaType = "درجات", 
                    MinScore = 0, 
                    MaxScore = 100, 
                    PassingScore = 70, 
                    IsActive = true, 
                    CreatedDate = DateTime.Now 
                },
                new TestType 
                { 
                    TestName = "اختبار المقابلة الشخصية", 
                    TestCode = "INT", 
                    Description = "تقييم المقابلة الشخصية", 
                    CriteriaType = "ناجح_راسب", 
                    IsActive = true, 
                    CreatedDate = DateTime.Now 
                },
                new TestType 
                { 
                    TestName = "اختبار المعلومات العامة", 
                    TestCode = "GEN", 
                    Description = "اختبار المعلومات العامة والثقافة", 
                    CriteriaType = "درجات", 
                    MinScore = 0, 
                    MaxScore = 100, 
                    PassingScore = 60, 
                    IsActive = true, 
                    CreatedDate = DateTime.Now 
                },
                new TestType 
                { 
                    TestName = "اختبار التخصص التقني", 
                    TestCode = "TECH", 
                    Description = "اختبار المهارات التقنية التخصصية", 
                    CriteriaType = "درجات", 
                    MinScore = 0, 
                    MaxScore = 100, 
                    PassingScore = 65, 
                    IsActive = true, 
                    CreatedDate = DateTime.Now 
                },
                new TestType 
                { 
                    TestName = "تقييم مهارات القيادة", 
                    TestCode = "LEAD", 
                    Description = "تقييم المهارات القيادية والإدارية", 
                    CriteriaType = "درجات", 
                    MinScore = 0, 
                    MaxScore = 100, 
                    PassingScore = 70, 
                    IsActive = true, 
                    CreatedDate = DateTime.Now 
                }
            };

            context.TestTypes.AddRange(testTypes);
            context.SaveChanges();

            // Add Category Test Paths after TestTypes are saved
            CreateCategoryTestPaths(context);
        }

        private static void CreateCategoryTestPaths(TajneedOfficeDbContext context)
        {
            // Get categories and test types from database
            var pilotCategory = context.CandidateCategories.FirstOrDefault(c => c.CategoryCode == "CAT-PLT");
            var militaryUniCategory = context.CandidateCategories.FirstOrDefault(c => c.CategoryCode == "CAT-MUG");
            var civilianUniCategory = context.CandidateCategories.FirstOrDefault(c => c.CategoryCode == "CAT-CUG");

            // Get test types
            var engTest = context.TestTypes.FirstOrDefault(t => t.TestCode == "ENG");
            var arbTest = context.TestTypes.FirstOrDefault(t => t.TestCode == "ARB");
            var mthTest = context.TestTypes.FirstOrDefault(t => t.TestCode == "MTH");
            var fitTest = context.TestTypes.FirstOrDefault(t => t.TestCode == "FIT");
            var medTest = context.TestTypes.FirstOrDefault(t => t.TestCode == "MED");
            var flightTest = context.TestTypes.FirstOrDefault(t => t.TestCode == "FLIGHT");
            var intTest = context.TestTypes.FirstOrDefault(t => t.TestCode == "INT");
            var genTest = context.TestTypes.FirstOrDefault(t => t.TestCode == "GEN");
            var techTest = context.TestTypes.FirstOrDefault(t => t.TestCode == "TECH");
            var leadTest = context.TestTypes.FirstOrDefault(t => t.TestCode == "LEAD");

            var categoryTestPaths = new List<CategoryTestPath>();

            // Pilots configuration
            if (pilotCategory != null && engTest != null && mthTest != null && flightTest != null && fitTest != null && medTest != null && intTest != null)
            {
                categoryTestPaths.AddRange(new[]
                {
                    new CategoryTestPath { CategoryId = pilotCategory.CategoryId, TestTypeId = engTest.TestTypeId, Weight = 15, OrderIndex = 1, IsMandatory = true, IsActive = true, Notes = "اختبار اللغة الإنجليزية إجباري للطيارين", CreatedDate = DateTime.Now },
                    new CategoryTestPath { CategoryId = pilotCategory.CategoryId, TestTypeId = mthTest.TestTypeId, Weight = 20, OrderIndex = 2, IsMandatory = true, IsActive = true, Notes = "اختبار الرياضيات ضروري للحسابات الطيرانية", CreatedDate = DateTime.Now },
                    new CategoryTestPath { CategoryId = pilotCategory.CategoryId, TestTypeId = flightTest.TestTypeId, Weight = 35, OrderIndex = 3, IsMandatory = true, IsActive = true, Notes = "اختبار الكفاءة الطيرانية - الوزن الأكبر", CreatedDate = DateTime.Now },
                    new CategoryTestPath { CategoryId = pilotCategory.CategoryId, TestTypeId = fitTest.TestTypeId, Weight = 10, OrderIndex = 4, IsMandatory = true, IsActive = true, Notes = "اللياقة البدنية للطيارين", CreatedDate = DateTime.Now },
                    new CategoryTestPath { CategoryId = pilotCategory.CategoryId, TestTypeId = medTest.TestTypeId, Weight = 10, OrderIndex = 5, IsMandatory = true, IsActive = true, Notes = "الفحص الطبي الإجباري", CreatedDate = DateTime.Now },
                    new CategoryTestPath { CategoryId = pilotCategory.CategoryId, TestTypeId = intTest.TestTypeId, Weight = 10, OrderIndex = 6, IsMandatory = true, IsActive = true, Notes = "المقابلة الشخصية للتقييم النهائي", CreatedDate = DateTime.Now }
                });
            }

            // Military University Graduates configuration
            if (militaryUniCategory != null && arbTest != null && engTest != null && genTest != null && leadTest != null && fitTest != null && intTest != null)
            {
                categoryTestPaths.AddRange(new[]
                {
                    new CategoryTestPath { CategoryId = militaryUniCategory.CategoryId, TestTypeId = arbTest.TestTypeId, Weight = 20, OrderIndex = 1, IsMandatory = true, IsActive = true, Notes = "اختبار اللغة العربية للجامعيين العسكريين", CreatedDate = DateTime.Now },
                    new CategoryTestPath { CategoryId = militaryUniCategory.CategoryId, TestTypeId = engTest.TestTypeId, Weight = 15, OrderIndex = 2, IsMandatory = true, IsActive = true, Notes = "اختبار اللغة الإنجليزية", CreatedDate = DateTime.Now },
                    new CategoryTestPath { CategoryId = militaryUniCategory.CategoryId, TestTypeId = genTest.TestTypeId, Weight = 25, OrderIndex = 3, IsMandatory = true, IsActive = true, Notes = "اختبار المعلومات العامة والعسكرية", CreatedDate = DateTime.Now },
                    new CategoryTestPath { CategoryId = militaryUniCategory.CategoryId, TestTypeId = leadTest.TestTypeId, Weight = 20, OrderIndex = 4, IsMandatory = true, IsActive = true, Notes = "تقييم المهارات القيادية", CreatedDate = DateTime.Now },
                    new CategoryTestPath { CategoryId = militaryUniCategory.CategoryId, TestTypeId = fitTest.TestTypeId, Weight = 10, OrderIndex = 5, IsMandatory = true, IsActive = true, Notes = "اللياقة البدنية", CreatedDate = DateTime.Now },
                    new CategoryTestPath { CategoryId = militaryUniCategory.CategoryId, TestTypeId = intTest.TestTypeId, Weight = 10, OrderIndex = 6, IsMandatory = true, IsActive = true, Notes = "المقابلة الشخصية", CreatedDate = DateTime.Now }
                });
            }

            // Civilian University Graduates configuration
            if (civilianUniCategory != null && arbTest != null && engTest != null && genTest != null && techTest != null && fitTest != null && intTest != null)
            {
                categoryTestPaths.AddRange(new[]
                {
                    new CategoryTestPath { CategoryId = civilianUniCategory.CategoryId, TestTypeId = arbTest.TestTypeId, Weight = 25, OrderIndex = 1, IsMandatory = true, IsActive = true, Notes = "اختبار اللغة العربية - الوزن الأكبر", CreatedDate = DateTime.Now },
                    new CategoryTestPath { CategoryId = civilianUniCategory.CategoryId, TestTypeId = engTest.TestTypeId, Weight = 20, OrderIndex = 2, IsMandatory = true, IsActive = true, Notes = "اختبار اللغة الإنجليزية", CreatedDate = DateTime.Now },
                    new CategoryTestPath { CategoryId = civilianUniCategory.CategoryId, TestTypeId = genTest.TestTypeId, Weight = 25, OrderIndex = 3, IsMandatory = true, IsActive = true, Notes = "اختبار المعلومات العامة", CreatedDate = DateTime.Now },
                    new CategoryTestPath { CategoryId = civilianUniCategory.CategoryId, TestTypeId = techTest.TestTypeId, Weight = 15, OrderIndex = 4, IsMandatory = false, IsActive = true, Notes = "اختبار التخصص التقني (اختياري)", CreatedDate = DateTime.Now },
                    new CategoryTestPath { CategoryId = civilianUniCategory.CategoryId, TestTypeId = fitTest.TestTypeId, Weight = 10, OrderIndex = 5, IsMandatory = true, IsActive = true, Notes = "اللياقة البدنية", CreatedDate = DateTime.Now },
                    new CategoryTestPath { CategoryId = civilianUniCategory.CategoryId, TestTypeId = intTest.TestTypeId, Weight = 5, OrderIndex = 6, IsMandatory = true, IsActive = true, Notes = "المقابلة الشخصية", CreatedDate = DateTime.Now }
                });
            }

            if (categoryTestPaths.Any())
            {
                context.CategoryTestPaths.AddRange(categoryTestPaths);
                context.SaveChanges();
            }
        }

        private static void SeedDemoCandidates(TajneedOfficeDbContext context)
        {
            var categories = context.CandidateCategories.ToList();
            var ranks = context.Ranks.ToList();
            var airbases = context.Airbases.ToList();
            var governorates = new[] { "مسقط", "ظفار", "الداخلية", "الباطنة شمال", "الباطنة جنوب", "الشرقية شمال", "الشرقية جنوب", "الظاهرة", "البريمي", "الوسطى", "مسندم" };
            var universities = new[] { "جامعة السلطان قابوس", "الجامعة الألمانية للتكنولوجيا", "جامعة نزوى", "جامعة صحار", "جامعة التقنية والعلوم التطبيقية", "جامعة ظفار", "جامعة البريمي", "جامعة الشرقية", "جامعة مسقط", "كلية عمان البحرية الدولية", "كلية الشرق الأوسط" };
            var majors = new[] { "هندسة طيران", "علوم عسكرية", "إدارة أعمال", "قانون", "علوم الحاسب", "هندسة كهربائية", "إلكترونيات", "ميكانيكا", "إدارة عامة", "علوم جوية", "تقنية معلومات", "علوم سياسية", "تربية رياضية", "طب طيران" };
            var grades = new[] { "ممتاز", "جيد جدًا", "جيد", "مقبول" };
            var statuses = new[] { "جديد", "مقبول مبدئيًا", "قيد التقييم", "مرفوض" };

            var random = new Random();
            int candidateCount = 25;
            int nationalIdSeed = 10000000;
            var candidates = new List<Candidate>();

            for (int i = 0; i < candidateCount; i++)
            {
                var category = categories[i % categories.Count];
                var rank = ranks[random.Next(ranks.Count)];
                var airbase = airbases[random.Next(airbases.Count)];
                var governorate = governorates[random.Next(governorates.Length)];
                var fullName = $"مرشح تجريبي {i + 1}";
                var nationalId = (nationalIdSeed + i).ToString().PadLeft(8, '0');
                var phone = $"9{random.Next(1, 9)}{random.Next(10000000, 99999999)}";
                var dob = DateTime.Now.AddYears(-random.Next(20, 35)).AddDays(-random.Next(0, 365));
                var status = statuses[random.Next(statuses.Length)];
                var isActive = random.NextDouble() > 0.1;

                // بيانات تعليمية حسب الفئة
                string? major = null, university = null, marksGrade = null;
                int? graduationYear = null;
                if (category.CategoryCode.StartsWith("CAT-PLT") || category.CategoryCode.StartsWith("CAT-MUG") || category.CategoryCode.StartsWith("CAT-CUG") || category.CategoryCode.StartsWith("CAT-TCN"))
                {
                    major = majors[random.Next(majors.Length)];
                    university = universities[random.Next(universities.Length)];
                    graduationYear = DateTime.Now.Year - random.Next(0, 10);
                    marksGrade = grades[random.Next(grades.Length)];
                }

                var candidate = new Candidate
                {
                    CategoryId = category.CategoryId,
                    FullName = fullName,
                    NationalIdNumber = nationalId,
                    DateOfBirth = dob,
                    Governorate = governorate,
                    Phone1 = phone,
                    CurrentRankId = rank.RankId,
                    CurrentAirbaseId = airbase.AirbaseId,
                    Major = major,
                    University = university,
                    GraduationYear = graduationYear,
                    MarksGrade = marksGrade,
                    Department = null,
                    JobTitle = null,
                    Address = $"{governorate} - حي رقم {random.Next(1, 20)}",
                    IsActive = isActive,
                    CurrentStatus = status
                };
                candidates.Add(candidate);
            }
            context.Candidates.AddRange(candidates);
            context.SaveChanges();
        }
    }
} 