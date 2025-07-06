using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace TajneedOffice.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Airbases",
                columns: table => new
                {
                    AirbaseId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AirbaseName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Airbases", x => x.AirbaseId);
                });

            migrationBuilder.CreateTable(
                name: "AspNetRoles",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CandidateCategories",
                columns: table => new
                {
                    CategoryId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CategoryName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    CategoryCode = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CandidateCategories", x => x.CategoryId);
                });

            migrationBuilder.CreateTable(
                name: "Ranks",
                columns: table => new
                {
                    RankId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RankName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    RankCode = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    RankOrder = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Ranks", x => x.RankId);
                });

            migrationBuilder.CreateTable(
                name: "TestTypes",
                columns: table => new
                {
                    TestTypeId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TestName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    CriteriaType = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    MinScore = table.Column<decimal>(type: "decimal(5,2)", precision: 5, scale: 2, nullable: true),
                    MaxScore = table.Column<decimal>(type: "decimal(5,2)", precision: 5, scale: 2, nullable: true),
                    PassingScore = table.Column<decimal>(type: "decimal(5,2)", precision: 5, scale: 2, nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    TestCode = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TestTypes", x => x.TestTypeId);
                });

            migrationBuilder.CreateTable(
                name: "AspNetRoleClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoleId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoleClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetRoleClaims_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUsers",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ServiceNumber = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    FullName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    RankId = table.Column<int>(type: "int", nullable: false),
                    Position = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    UserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedUserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    Email = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedEmail = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    EmailConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SecurityStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    TwoFactorEnabled = table.Column<bool>(type: "bit", nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    LockoutEnabled = table.Column<bool>(type: "bit", nullable: false),
                    AccessFailedCount = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUsers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetUsers_Ranks_RankId",
                        column: x => x.RankId,
                        principalTable: "Ranks",
                        principalColumn: "RankId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Candidates",
                columns: table => new
                {
                    CandidateId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CategoryId = table.Column<int>(type: "int", nullable: true),
                    FullName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    NationalIdNumber = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    ServiceNumber = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    DateOfBirth = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Governorate = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Phone1 = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    Phone2 = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    Phone3 = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    CurrentRankId = table.Column<int>(type: "int", nullable: true),
                    CurrentAirbaseId = table.Column<int>(type: "int", nullable: true),
                    Department = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    JobTitle = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    Major = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    University = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    GraduationYear = table.Column<int>(type: "int", nullable: true),
                    MarksGrade = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Address = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    CurrentStatus = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Candidates", x => x.CandidateId);
                    table.ForeignKey(
                        name: "FK_Candidates_Airbases_CurrentAirbaseId",
                        column: x => x.CurrentAirbaseId,
                        principalTable: "Airbases",
                        principalColumn: "AirbaseId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Candidates_CandidateCategories_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "CandidateCategories",
                        principalColumn: "CategoryId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Candidates_Ranks_CurrentRankId",
                        column: x => x.CurrentRankId,
                        principalTable: "Ranks",
                        principalColumn: "RankId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "CategoryTestPaths",
                columns: table => new
                {
                    PathId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CategoryId = table.Column<int>(type: "int", nullable: false),
                    TestTypeId = table.Column<int>(type: "int", nullable: false),
                    Weight = table.Column<decimal>(type: "decimal(5,2)", precision: 5, scale: 2, nullable: false),
                    OrderIndex = table.Column<int>(type: "int", nullable: false),
                    IsMandatory = table.Column<bool>(type: "bit", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    Notes = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CategoryTestPaths", x => x.PathId);
                    table.ForeignKey(
                        name: "FK_CategoryTestPaths_CandidateCategories_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "CandidateCategories",
                        principalColumn: "CategoryId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CategoryTestPaths_TestTypes_TestTypeId",
                        column: x => x.TestTypeId,
                        principalTable: "TestTypes",
                        principalColumn: "TestTypeId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetUserClaims_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserLogins",
                columns: table => new
                {
                    LoginProvider = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProviderKey = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProviderDisplayName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserLogins", x => new { x.LoginProvider, x.ProviderKey });
                    table.ForeignKey(
                        name: "FK_AspNetUserLogins_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserRoles",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    RoleId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserRoles", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserTokens",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    LoginProvider = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Value = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserTokens", x => new { x.UserId, x.LoginProvider, x.Name });
                    table.ForeignKey(
                        name: "FK_AspNetUserTokens_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CandidateTestResults",
                columns: table => new
                {
                    TestResultId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CandidateId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TestTypeId = table.Column<int>(type: "int", nullable: false),
                    NumericScore = table.Column<decimal>(type: "decimal(5,2)", precision: 5, scale: 2, nullable: true),
                    TextResult = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    IsPassed = table.Column<bool>(type: "bit", nullable: true),
                    TestDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EvaluatorName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Notes = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    WeightedScore = table.Column<decimal>(type: "decimal(5,2)", precision: 5, scale: 2, nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CandidateTestResults", x => x.TestResultId);
                    table.ForeignKey(
                        name: "FK_CandidateTestResults_Candidates_CandidateId",
                        column: x => x.CandidateId,
                        principalTable: "Candidates",
                        principalColumn: "CandidateId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CandidateTestResults_TestTypes_TestTypeId",
                        column: x => x.TestTypeId,
                        principalTable: "TestTypes",
                        principalColumn: "TestTypeId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "CandidateTestScores",
                columns: table => new
                {
                    TestScoreId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CandidateId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    EnglishScore = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: true),
                    MathematicsScore = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: true),
                    GeneralCultureScore = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: true),
                    ArabicScore = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: true),
                    FlightAptitudeScore = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: true),
                    DrivingSkillScore = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: true),
                    CommunicationSkillScore = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: true),
                    ProfessionalEvaluationScore = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CandidateTestScores", x => x.TestScoreId);
                    table.ForeignKey(
                        name: "FK_CandidateTestScores_Candidates_CandidateId",
                        column: x => x.CandidateId,
                        principalTable: "Candidates",
                        principalColumn: "CandidateId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "FinalEvaluations",
                columns: table => new
                {
                    FinalEvaluationId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CandidateId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AppearanceScore = table.Column<int>(type: "int", nullable: false),
                    AppearanceStrength = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AppearanceWeakness = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PersonalityScore = table.Column<int>(type: "int", nullable: false),
                    PersonalityStrength = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PersonalityWeakness = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IntelligenceScore = table.Column<int>(type: "int", nullable: false),
                    IntelligenceStrength = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IntelligenceWeakness = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SocialSkillsScore = table.Column<int>(type: "int", nullable: false),
                    SocialSkillsStrength = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SocialSkillsWeakness = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ResponsibilityScore = table.Column<int>(type: "int", nullable: false),
                    ResponsibilityStrength = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ResponsibilityWeakness = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CommunicationScore = table.Column<int>(type: "int", nullable: false),
                    CommunicationStrength = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CommunicationWeakness = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AwarenessScore = table.Column<int>(type: "int", nullable: false),
                    AwarenessStrength = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AwarenessWeakness = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AmbitionScore = table.Column<int>(type: "int", nullable: false),
                    AmbitionStrength = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AmbitionWeakness = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TotalScore = table.Column<int>(type: "int", nullable: false),
                    FinalGrade = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    Notes = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    EvaluationDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    RecruitmentTestsScore = table.Column<decimal>(type: "decimal(5,2)", precision: 5, scale: 2, nullable: true),
                    RecruitmentTestsPercentage = table.Column<decimal>(type: "decimal(5,2)", precision: 5, scale: 2, nullable: true),
                    MainCommitteeScore = table.Column<decimal>(type: "decimal(5,2)", precision: 5, scale: 2, nullable: true),
                    MainCommitteePercentage = table.Column<decimal>(type: "decimal(5,2)", precision: 5, scale: 2, nullable: true),
                    ProfessionalTestScore = table.Column<decimal>(type: "decimal(5,2)", precision: 5, scale: 2, nullable: true),
                    ProfessionalTestPercentage = table.Column<decimal>(type: "decimal(5,2)", precision: 5, scale: 2, nullable: true),
                    FinalPercentage = table.Column<decimal>(type: "decimal(5,2)", precision: 5, scale: 2, nullable: true),
                    FinalDecision = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    DecisionReason = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    GeneralNotes = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    EvaluationStatus = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ApprovalDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ApprovedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CandidateId1 = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FinalEvaluations", x => x.FinalEvaluationId);
                    table.ForeignKey(
                        name: "FK_FinalEvaluations_Candidates_CandidateId",
                        column: x => x.CandidateId,
                        principalTable: "Candidates",
                        principalColumn: "CandidateId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FinalEvaluations_Candidates_CandidateId1",
                        column: x => x.CandidateId1,
                        principalTable: "Candidates",
                        principalColumn: "CandidateId");
                });

            migrationBuilder.InsertData(
                table: "Airbases",
                columns: new[] { "AirbaseId", "AirbaseName", "IsActive" },
                values: new object[,]
                {
                    { 1, "قيادة سلاح الجو السلطاني العماني", true },
                    { 2, "قاعدة غلا وأكاديمية السلطان قابوس الجوية", true },
                    { 3, "قاعدة السيب الجوية", true },
                    { 4, "قاعدة صلالة الجوية", true },
                    { 5, "قاعدة المصنعة الجوية", true },
                    { 6, "قاعدة مصيرة الجوية", true },
                    { 7, "قاعدة أدم الجوية", true },
                    { 8, "قاعدة ثمريت الجوية", true },
                    { 9, "قاعدة خصب الجوية", true }
                });

            migrationBuilder.InsertData(
                table: "CandidateCategories",
                columns: new[] { "CategoryId", "CategoryCode", "CategoryName", "Description" },
                values: new object[,]
                {
                    { 1, "CAT-PLT", "المرشحيين الطيارين", "فئة المرشحين للالتحاق بسلاح الجو السلطاني العماني كمرشحين طيارين." },
                    { 2, "CAT-MUG", "المرشحيين الجامعيين العسكريين", "خريجوا الكليات والجامعات من حملة البكالوريوس ويتم تقيمهم بناءا على تخصصاتهم المهنية وينضموا للسلاح كضباط عسكريين جامعيين" },
                    { 3, "CAT-CUG", "المرشحيين الجامعيين المدنيين", "خريجوا الكليات والجامعات من حملة البكالوريوس ويتم تقيمهم بناءا على تخصصاتهم المهنية وينضموا للسلاح كضباط مدنيين جامعيين" },
                    { 4, "CAT-LSO", "ضباط الخدمة المحدودة", "ضباط الصف من ذوي الخدمة المحدودة وسسبق لهم العمل في السلاح وهم برتبة وكيل فأعلى" },
                    { 5, "CAT-NCO", "ضباط الصف ( رقباء / عرفاء)", "ضباط صف برتبة رقيب أو عريف سبق لهم العمل بالسلاح" },
                    { 6, "CAT-TCN", "ضباط الصف الكلية التقنية العسكرية", "ضباط صف خريجوا الكلية العسكرية التقنية" },
                    { 7, "CAT-CNP", "ضباط الصف المدنيين للترفيع", "ضباط صف مدنيين مرشحين للترقية بالصفة المدنية" }
                });

            migrationBuilder.InsertData(
                table: "Ranks",
                columns: new[] { "RankId", "RankCode", "RankName", "RankOrder" },
                values: new object[,]
                {
                    { 1, "RECRUIT", "جندي", 1 },
                    { 2, "LANCE_CPL", "نائب عريف", 2 },
                    { 3, "CORPORAL", "عريف", 3 },
                    { 4, "SERGEANT", "رقيب", 4 },
                    { 5, "STAFF_SGT", "رقيب أول", 5 },
                    { 6, "WARRANT", "وكيل", 6 },
                    { 7, "CHIEF_WARRANT", "وكيل أول", 7 },
                    { 8, "CADET", "ضابط مرشح", 8 },
                    { 9, "SECOND_LT", "ملازم ثاني", 9 },
                    { 10, "FIRST_LT", "ملازم أول", 10 },
                    { 11, "CAPTAIN", "نقيب", 11 },
                    { 12, "MAJOR", "رائد", 12 },
                    { 13, "LT_COLONEL", "مقدم", 13 },
                    { 14, "COLONEL", "عقيد", 14 },
                    { 15, "BRIGADIER", "عميد", 15 },
                    { 16, "MAJ_GENERAL", "لواء", 16 },
                    { 17, "LT_GENERAL", "فريق", 17 },
                    { 18, "CIV_G16", "مدني درجة 16", 18 },
                    { 19, "CIV_G15", "مدني درجة 15", 19 },
                    { 20, "CIV_G14", "مدني درجة 14", 20 },
                    { 21, "CIV_G13", "مدني درجة 13", 21 },
                    { 22, "CIV_G12", "مدني درجة 12", 22 },
                    { 23, "CIV_G11", "مدني درجة 11", 23 },
                    { 24, "CIV_G10", "مدني درجة 10", 24 },
                    { 25, "CIV_G9", "مدني درجة 9", 25 },
                    { 26, "CIV_OFF_G8", "ضابط مدني د8", 26 },
                    { 27, "CIV_OFF_G9", "ضابط مدني د9", 27 },
                    { 28, "CIV_OFF_G7", "ضابط مدني د7", 28 },
                    { 29, "CIV_OFF_G6", "ضابط مدني د6", 29 },
                    { 30, "CIV_OFF_G5", "ضابط مدني د5", 30 },
                    { 31, "CIV_OFF_G4", "ضابط مدني د4", 31 },
                    { 32, "CIV_OFF_G3", "ضابط مدني د3", 32 },
                    { 33, "CIV_OFF_G2", "ضابط مدني د2", 33 },
                    { 34, "CIV_OFF_G1", "ضابط مدني د1", 34 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Airbases_AirbaseName",
                table: "Airbases",
                column: "AirbaseName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AspNetRoleClaims_RoleId",
                table: "AspNetRoleClaims",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                table: "AspNetRoles",
                column: "NormalizedName",
                unique: true,
                filter: "[NormalizedName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserClaims_UserId",
                table: "AspNetUserClaims",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserLogins_UserId",
                table: "AspNetUserLogins",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserRoles_RoleId",
                table: "AspNetUserRoles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                table: "AspNetUsers",
                column: "NormalizedEmail");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_RankId",
                table: "AspNetUsers",
                column: "RankId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_ServiceNumber",
                table: "AspNetUsers",
                column: "ServiceNumber",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                table: "AspNetUsers",
                column: "NormalizedUserName",
                unique: true,
                filter: "[NormalizedUserName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_CandidateCategories_CategoryCode",
                table: "CandidateCategories",
                column: "CategoryCode",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_CandidateCategories_CategoryName",
                table: "CandidateCategories",
                column: "CategoryName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Candidates_CategoryId",
                table: "Candidates",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Candidates_CurrentAirbaseId",
                table: "Candidates",
                column: "CurrentAirbaseId");

            migrationBuilder.CreateIndex(
                name: "IX_Candidates_CurrentRankId",
                table: "Candidates",
                column: "CurrentRankId");

            migrationBuilder.CreateIndex(
                name: "IX_Candidates_NationalIdNumber",
                table: "Candidates",
                column: "NationalIdNumber",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_CandidateTestResults_CandidateId_TestTypeId",
                table: "CandidateTestResults",
                columns: new[] { "CandidateId", "TestTypeId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_CandidateTestResults_TestTypeId",
                table: "CandidateTestResults",
                column: "TestTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_CandidateTestScores_CandidateId",
                table: "CandidateTestScores",
                column: "CandidateId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_CategoryTestPaths_CategoryId_TestTypeId",
                table: "CategoryTestPaths",
                columns: new[] { "CategoryId", "TestTypeId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_CategoryTestPaths_TestTypeId",
                table: "CategoryTestPaths",
                column: "TestTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_FinalEvaluations_CandidateId",
                table: "FinalEvaluations",
                column: "CandidateId");

            migrationBuilder.CreateIndex(
                name: "IX_FinalEvaluations_CandidateId1",
                table: "FinalEvaluations",
                column: "CandidateId1",
                unique: true,
                filter: "[CandidateId1] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Ranks_RankName",
                table: "Ranks",
                column: "RankName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_TestTypes_TestCode",
                table: "TestTypes",
                column: "TestCode",
                unique: true,
                filter: "[TestCode] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_TestTypes_TestName",
                table: "TestTypes",
                column: "TestName",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AspNetRoleClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserLogins");

            migrationBuilder.DropTable(
                name: "AspNetUserRoles");

            migrationBuilder.DropTable(
                name: "AspNetUserTokens");

            migrationBuilder.DropTable(
                name: "CandidateTestResults");

            migrationBuilder.DropTable(
                name: "CandidateTestScores");

            migrationBuilder.DropTable(
                name: "CategoryTestPaths");

            migrationBuilder.DropTable(
                name: "FinalEvaluations");

            migrationBuilder.DropTable(
                name: "AspNetRoles");

            migrationBuilder.DropTable(
                name: "AspNetUsers");

            migrationBuilder.DropTable(
                name: "TestTypes");

            migrationBuilder.DropTable(
                name: "Candidates");

            migrationBuilder.DropTable(
                name: "Airbases");

            migrationBuilder.DropTable(
                name: "CandidateCategories");

            migrationBuilder.DropTable(
                name: "Ranks");
        }
    }
}
