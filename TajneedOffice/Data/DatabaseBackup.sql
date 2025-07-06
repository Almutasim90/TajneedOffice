-- =============================================
-- TajneedOffice Database Backup Script
-- Royal Air Force of Oman Candidate Management System
-- Generated: $(Get-Date)
-- =============================================

USE master;
GO

-- Create database if it doesn't exist
IF NOT EXISTS (SELECT name FROM sys.databases WHERE name = 'TajneedOffice')
BEGIN
    CREATE DATABASE TajneedOffice;
END
GO

USE TajneedOffice;
GO

-- =============================================
-- Create Tables
-- =============================================

-- Candidate Categories Table
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[CandidateCategories]') AND type in (N'U'))
BEGIN
    CREATE TABLE [dbo].[CandidateCategories](
        [CategoryId] [int] IDENTITY(1,1) NOT NULL,
        [CategoryCode] [nvarchar](20) NOT NULL,
        [CategoryName] [nvarchar](100) NOT NULL,
        [Description] [nvarchar](500) NULL,
        [IsActive] [bit] NOT NULL DEFAULT 1,
        [CreatedDate] [datetime2](7) NOT NULL DEFAULT GETDATE(),
        CONSTRAINT [PK_CandidateCategories] PRIMARY KEY CLUSTERED ([CategoryId] ASC)
    );
END
GO

-- Ranks Table
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Ranks]') AND type in (N'U'))
BEGIN
    CREATE TABLE [dbo].[Ranks](
        [RankId] [int] IDENTITY(1,1) NOT NULL,
        [RankName] [nvarchar](50) NOT NULL,
        [RankCode] [nvarchar](20) NOT NULL,
        [RankOrder] [int] NOT NULL,
        [IsActive] [bit] NOT NULL DEFAULT 1,
        [CreatedDate] [datetime2](7) NOT NULL DEFAULT GETDATE(),
        CONSTRAINT [PK_Ranks] PRIMARY KEY CLUSTERED ([RankId] ASC)
    );
END
GO

-- Airbases Table
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Airbases]') AND type in (N'U'))
BEGIN
    CREATE TABLE [dbo].[Airbases](
        [AirbaseId] [int] IDENTITY(1,1) NOT NULL,
        [AirbaseName] [nvarchar](100) NOT NULL,
        [IsActive] [bit] NOT NULL DEFAULT 1,
        [CreatedDate] [datetime2](7) NOT NULL DEFAULT GETDATE(),
        CONSTRAINT [PK_Airbases] PRIMARY KEY CLUSTERED ([AirbaseId] ASC)
    );
END
GO

-- Test Types Table
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[TestTypes]') AND type in (N'U'))
BEGIN
    CREATE TABLE [dbo].[TestTypes](
        [TestTypeId] [int] IDENTITY(1,1) NOT NULL,
        [TestName] [nvarchar](100) NOT NULL,
        [TestCode] [nvarchar](10) NOT NULL,
        [Description] [nvarchar](500) NULL,
        [CriteriaType] [nvarchar](50) NOT NULL,
        [MinScore] [decimal](5,2) NULL,
        [MaxScore] [decimal](5,2) NULL,
        [PassingScore] [decimal](5,2) NULL,
        [IsActive] [bit] NOT NULL DEFAULT 1,
        [CreatedDate] [datetime2](7) NOT NULL DEFAULT GETDATE(),
        CONSTRAINT [PK_TestTypes] PRIMARY KEY CLUSTERED ([TestTypeId] ASC)
    );
END
GO

-- Category Test Paths Table
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[CategoryTestPaths]') AND type in (N'U'))
BEGIN
    CREATE TABLE [dbo].[CategoryTestPaths](
        [CategoryTestPathId] [int] IDENTITY(1,1) NOT NULL,
        [CategoryId] [int] NOT NULL,
        [TestTypeId] [int] NOT NULL,
        [Weight] [decimal](5,2) NOT NULL,
        [OrderIndex] [int] NOT NULL,
        [IsMandatory] [bit] NOT NULL DEFAULT 1,
        [IsActive] [bit] NOT NULL DEFAULT 1,
        [Notes] [nvarchar](500) NULL,
        [CreatedDate] [datetime2](7) NOT NULL DEFAULT GETDATE(),
        CONSTRAINT [PK_CategoryTestPaths] PRIMARY KEY CLUSTERED ([CategoryTestPathId] ASC)
    );
END
GO

-- Users Table
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Users]') AND type in (N'U'))
BEGIN
    CREATE TABLE [dbo].[Users](
        [UserId] [int] IDENTITY(1,1) NOT NULL,
        [Username] [nvarchar](50) NOT NULL,
        [Email] [nvarchar](100) NOT NULL,
        [PasswordHash] [nvarchar](255) NOT NULL,
        [FullName] [nvarchar](100) NOT NULL,
        [Role] [nvarchar](20) NOT NULL,
        [IsActive] [bit] NOT NULL DEFAULT 1,
        [LastLoginDate] [datetime2](7) NULL,
        [CreatedDate] [datetime2](7) NOT NULL DEFAULT GETDATE(),
        CONSTRAINT [PK_Users] PRIMARY KEY CLUSTERED ([UserId] ASC)
    );
END
GO

-- Candidates Table
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Candidates]') AND type in (N'U'))
BEGIN
    CREATE TABLE [dbo].[Candidates](
        [CandidateId] [int] IDENTITY(1,1) NOT NULL,
        [CategoryId] [int] NOT NULL,
        [FullName] [nvarchar](100) NOT NULL,
        [NationalIdNumber] [nvarchar](20) NOT NULL,
        [DateOfBirth] [date] NOT NULL,
        [Governorate] [nvarchar](50) NOT NULL,
        [Phone1] [nvarchar](20) NULL,
        [Phone2] [nvarchar](20) NULL,
        [CurrentRankId] [int] NULL,
        [CurrentAirbaseId] [int] NULL,
        [Major] [nvarchar](100) NULL,
        [University] [nvarchar](100) NULL,
        [GraduationYear] [int] NULL,
        [MarksGrade] [nvarchar](20) NULL,
        [Department] [nvarchar](100) NULL,
        [JobTitle] [nvarchar](100) NULL,
        [Address] [nvarchar](200) NULL,
        [IsActive] [bit] NOT NULL DEFAULT 1,
        [CurrentStatus] [nvarchar](50) NULL,
        [CreatedDate] [datetime2](7) NOT NULL DEFAULT GETDATE(),
        [ModifiedDate] [datetime2](7) NULL,
        CONSTRAINT [PK_Candidates] PRIMARY KEY CLUSTERED ([CandidateId] ASC)
    );
END
GO

-- Candidate Test Results Table
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[CandidateTestResults]') AND type in (N'U'))
BEGIN
    CREATE TABLE [dbo].[CandidateTestResults](
        [TestResultId] [int] IDENTITY(1,1) NOT NULL,
        [CandidateId] [int] NOT NULL,
        [TestTypeId] [int] NOT NULL,
        [Score] [decimal](5,2) NULL,
        [Result] [nvarchar](20) NULL,
        [TestDate] [date] NOT NULL,
        [Notes] [nvarchar](500) NULL,
        [CreatedDate] [datetime2](7) NOT NULL DEFAULT GETDATE(),
        CONSTRAINT [PK_CandidateTestResults] PRIMARY KEY CLUSTERED ([TestResultId] ASC)
    );
END
GO

-- Final Evaluations Table
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[FinalEvaluations]') AND type in (N'U'))
BEGIN
    CREATE TABLE [dbo].[FinalEvaluations](
        [EvaluationId] [int] IDENTITY(1,1) NOT NULL,
        [CandidateId] [int] NOT NULL,
        [TotalScore] [decimal](5,2) NOT NULL,
        [FinalResult] [nvarchar](20) NOT NULL,
        [EvaluationDate] [date] NOT NULL,
        [EvaluatorNotes] [nvarchar](1000) NULL,
        [CreatedDate] [datetime2](7) NOT NULL DEFAULT GETDATE(),
        CONSTRAINT [PK_FinalEvaluations] PRIMARY KEY CLUSTERED ([EvaluationId] ASC)
    );
END
GO

-- =============================================
-- Add Foreign Key Constraints
-- =============================================

-- Category Test Paths Foreign Keys
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_CategoryTestPaths_CandidateCategories]') AND parent_object_id = OBJECT_ID(N'[dbo].[CategoryTestPaths]'))
BEGIN
    ALTER TABLE [dbo].[CategoryTestPaths] ADD CONSTRAINT [FK_CategoryTestPaths_CandidateCategories] FOREIGN KEY([CategoryId]) REFERENCES [dbo].[CandidateCategories] ([CategoryId]);
END

IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_CategoryTestPaths_TestTypes]') AND parent_object_id = OBJECT_ID(N'[dbo].[CategoryTestPaths]'))
BEGIN
    ALTER TABLE [dbo].[CategoryTestPaths] ADD CONSTRAINT [FK_CategoryTestPaths_TestTypes] FOREIGN KEY([TestTypeId]) REFERENCES [dbo].[TestTypes] ([TestTypeId]);
END

-- Candidates Foreign Keys
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Candidates_CandidateCategories]') AND parent_object_id = OBJECT_ID(N'[dbo].[Candidates]'))
BEGIN
    ALTER TABLE [dbo].[Candidates] ADD CONSTRAINT [FK_Candidates_CandidateCategories] FOREIGN KEY([CategoryId]) REFERENCES [dbo].[CandidateCategories] ([CategoryId]);
END

IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Candidates_Ranks]') AND parent_object_id = OBJECT_ID(N'[dbo].[Candidates]'))
BEGIN
    ALTER TABLE [dbo].[Candidates] ADD CONSTRAINT [FK_Candidates_Ranks] FOREIGN KEY([CurrentRankId]) REFERENCES [dbo].[Ranks] ([RankId]);
END

IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Candidates_Airbases]') AND parent_object_id = OBJECT_ID(N'[dbo].[Candidates]'))
BEGIN
    ALTER TABLE [dbo].[Candidates] ADD CONSTRAINT [FK_Candidates_Airbases] FOREIGN KEY([CurrentAirbaseId]) REFERENCES [dbo].[Airbases] ([AirbaseId]);
END

-- Candidate Test Results Foreign Keys
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_CandidateTestResults_Candidates]') AND parent_object_id = OBJECT_ID(N'[dbo].[CandidateTestResults]'))
BEGIN
    ALTER TABLE [dbo].[CandidateTestResults] ADD CONSTRAINT [FK_CandidateTestResults_Candidates] FOREIGN KEY([CandidateId]) REFERENCES [dbo].[Candidates] ([CandidateId]);
END

IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_CandidateTestResults_TestTypes]') AND parent_object_id = OBJECT_ID(N'[dbo].[CandidateTestResults]'))
BEGIN
    ALTER TABLE [dbo].[CandidateTestResults] ADD CONSTRAINT [FK_CandidateTestResults_TestTypes] FOREIGN KEY([TestTypeId]) REFERENCES [dbo].[TestTypes] ([TestTypeId]);
END

-- Final Evaluations Foreign Keys
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_FinalEvaluations_Candidates]') AND parent_object_id = OBJECT_ID(N'[dbo].[FinalEvaluations]'))
BEGIN
    ALTER TABLE [dbo].[FinalEvaluations] ADD CONSTRAINT [FK_FinalEvaluations_Candidates] FOREIGN KEY([CandidateId]) REFERENCES [dbo].[Candidates] ([CandidateId]);
END

-- =============================================
-- Insert Seed Data
-- =============================================

-- Clear existing data (if any)
DELETE FROM [dbo].[FinalEvaluations];
DELETE FROM [dbo].[CandidateTestResults];
DELETE FROM [dbo].[Candidates];
DELETE FROM [dbo].[CategoryTestPaths];
DELETE FROM [dbo].[TestTypes];
DELETE FROM [dbo].[Airbases];
DELETE FROM [dbo].[Ranks];
DELETE FROM [dbo].[CandidateCategories];
DELETE FROM [dbo].[Users];

-- Reset identity columns
DBCC CHECKIDENT ('FinalEvaluations', RESEED, 0);
DBCC CHECKIDENT ('CandidateTestResults', RESEED, 0);
DBCC CHECKIDENT ('Candidates', RESEED, 0);
DBCC CHECKIDENT ('CategoryTestPaths', RESEED, 0);
DBCC CHECKIDENT ('TestTypes', RESEED, 0);
DBCC CHECKIDENT ('Airbases', RESEED, 0);
DBCC CHECKIDENT ('Ranks', RESEED, 0);
DBCC CHECKIDENT ('CandidateCategories', RESEED, 0);
DBCC CHECKIDENT ('Users', RESEED, 0);

-- Insert Candidate Categories
INSERT INTO [dbo].[CandidateCategories] ([CategoryCode], [CategoryName], [Description], [IsActive], [CreatedDate]) VALUES
('CAT-PLT', N'المرشحيين الطيارين', N'فئة المرشحين للالتحاق بسلاح الجو السلطاني العماني كمرشحين طيارين.', 1, GETDATE()),
('CAT-MUG', N'المرشحيين الجامعيين العسكريين', N'خريجوا الكليات والجامعات من حملة البكالوريوس ويتم تقيمهم بناءا على تخصصاتهم المهنية وينضموا للسلاح كضباط عسكريين جامعيين', 1, GETDATE()),
('CAT-CUG', N'المرشحيين الجامعيين المدنيين', N'خريجوا الكليات والجامعات من حملة البكالوريوس ويتم تقيمهم بناءا على تخصصاتهم المهنية وينضموا للسلاح كضباط مدنيين جامعيين', 1, GETDATE()),
('CAT-LSO', N'ضباط الخدمة المحدودة', N'ضباط الصف من ذوي الخدمة المحدودة وسسبق لهم العمل في السلاح وهم برتبة وكيل فأعلى', 1, GETDATE()),
('CAT-NCO', N'ضباط الصف ( رقباء / عرفاء)', N'ضباط صف برتبة رقيب أو عريف سبق لهم العمل بالسلاح', 1, GETDATE()),
('CAT-TCN', N'ضباط الصف الكلية التقنية العسكرية', N'ضباط صف خريجوا الكلية العسكرية التقنية', 1, GETDATE()),
('CAT-CNP', N'ضباط الصف المدنيين للترفيع', N'ضباط صف مدنيين مرشحين للترقية بالصفة المدنية', 1, GETDATE());

-- Insert Ranks
INSERT INTO [dbo].[Ranks] ([RankName], [RankCode], [RankOrder], [IsActive], [CreatedDate]) VALUES
(N'جندي', 'RECRUIT', 1, 1, GETDATE()),
(N'نائب عريف', 'LANCE_CPL', 2, 1, GETDATE()),
(N'عريف', 'CORPORAL', 3, 1, GETDATE()),
(N'رقيب', 'SERGEANT', 4, 1, GETDATE()),
(N'رقيب أول', 'STAFF_SGT', 5, 1, GETDATE()),
(N'وكيل', 'WARRANT', 6, 1, GETDATE()),
(N'وكيل أول', 'CHIEF_WARRANT', 7, 1, GETDATE()),
(N'ضابط مرشح', 'CADET', 8, 1, GETDATE()),
(N'ملازم ثاني', 'SECOND_LT', 9, 1, GETDATE()),
(N'ملازم أول', 'FIRST_LT', 10, 1, GETDATE()),
(N'نقيب', 'CAPTAIN', 11, 1, GETDATE()),
(N'رائد', 'MAJOR', 12, 1, GETDATE()),
(N'مقدم', 'LT_COLONEL', 13, 1, GETDATE()),
(N'عقيد', 'COLONEL', 14, 1, GETDATE()),
(N'عميد', 'BRIGADIER', 15, 1, GETDATE()),
(N'لواء', 'MAJ_GENERAL', 16, 1, GETDATE()),
(N'فريق', 'LT_GENERAL', 17, 1, GETDATE()),
(N'مدني درجة 16', 'CIV_G16', 18, 1, GETDATE()),
(N'مدني درجة 15', 'CIV_G15', 19, 1, GETDATE()),
(N'مدني درجة 14', 'CIV_G14', 20, 1, GETDATE()),
(N'مدني درجة 13', 'CIV_G13', 21, 1, GETDATE()),
(N'مدني درجة 12', 'CIV_G12', 22, 1, GETDATE()),
(N'مدني درجة 11', 'CIV_G11', 23, 1, GETDATE()),
(N'مدني درجة 10', 'CIV_G10', 24, 1, GETDATE()),
(N'مدني درجة 9', 'CIV_G9', 25, 1, GETDATE()),
(N'ضابط مدني د8', 'CIV_OFF_G8', 26, 1, GETDATE()),
(N'ضابط مدني د9', 'CIV_OFF_G9', 27, 1, GETDATE()),
(N'ضابط مدني د7', 'CIV_OFF_G7', 28, 1, GETDATE()),
(N'ضابط مدني د6', 'CIV_OFF_G6', 29, 1, GETDATE()),
(N'ضابط مدني د5', 'CIV_OFF_G5', 30, 1, GETDATE()),
(N'ضابط مدني د4', 'CIV_OFF_G4', 31, 1, GETDATE()),
(N'ضابط مدني د3', 'CIV_OFF_G3', 32, 1, GETDATE()),
(N'ضابط مدني د2', 'CIV_OFF_G2', 33, 1, GETDATE()),
(N'ضابط مدني د1', 'CIV_OFF_G1', 34, 1, GETDATE());

-- Insert Airbases
INSERT INTO [dbo].[Airbases] ([AirbaseName], [IsActive], [CreatedDate]) VALUES
(N'قيادة سلاح الجو السلطاني العماني', 1, GETDATE()),
(N'قاعدة غلا وأكاديمية السلطان قابوس الجوية', 1, GETDATE()),
(N'قاعدة السيب الجوية', 1, GETDATE()),
(N'قاعدة صلالة الجوية', 1, GETDATE()),
(N'قاعدة المصنعة الجوية', 1, GETDATE()),
(N'قاعدة مصيرة الجوية', 1, GETDATE()),
(N'قاعدة أدم الجوية', 1, GETDATE()),
(N'قاعدة ثمريت الجوية', 1, GETDATE()),
(N'قاعدة خصب الجوية', 1, GETDATE());

-- Insert Test Types
INSERT INTO [dbo].[TestTypes] ([TestName], [TestCode], [Description], [CriteriaType], [MinScore], [MaxScore], [PassingScore], [IsActive], [CreatedDate]) VALUES
(N'اختبار اللغة الإنجليزية', 'ENG', N'اختبار مهارات اللغة الإنجليزية الأساسية', N'درجات', 0.00, 100.00, 50.00, 1, GETDATE()),
(N'اختبار اللغة العربية', 'ARB', N'اختبار مهارات اللغة العربية والإملاء', N'درجات', 0.00, 100.00, 60.00, 1, GETDATE()),
(N'اختبار الرياضيات', 'MTH', N'اختبار المهارات الرياضية الأساسية', N'درجات', 0.00, 100.00, 50.00, 1, GETDATE()),
(N'اختبار اللياقة البدنية', 'FIT', N'تقييم اللياقة البدنية العامة', N'لائق_غير_لائق', NULL, NULL, NULL, 1, GETDATE()),
(N'الفحص الطبي', 'MED', N'الفحص الطبي الشامل', N'لائق_غير_لائق', NULL, NULL, NULL, 1, GETDATE()),
(N'اختبار الكفاءة الطيرانية', 'FLIGHT', N'اختبار مهارات الطيران الأساسية', N'درجات', 0.00, 100.00, 70.00, 1, GETDATE()),
(N'اختبار المقابلة الشخصية', 'INT', N'تقييم المقابلة الشخصية', N'ناجح_راسب', NULL, NULL, NULL, 1, GETDATE()),
(N'اختبار المعلومات العامة', 'GEN', N'اختبار المعلومات العامة والثقافة', N'درجات', 0.00, 100.00, 60.00, 1, GETDATE()),
(N'اختبار التخصص التقني', 'TECH', N'اختبار المهارات التقنية التخصصية', N'درجات', 0.00, 100.00, 65.00, 1, GETDATE()),
(N'تقييم مهارات القيادة', 'LEAD', N'تقييم المهارات القيادية والإدارية', N'درجات', 0.00, 100.00, 70.00, 1, GETDATE());

-- Insert Category Test Paths
INSERT INTO [dbo].[CategoryTestPaths] ([CategoryId], [TestTypeId], [Weight], [OrderIndex], [IsMandatory], [IsActive], [Notes], [CreatedDate]) VALUES
-- Pilots configuration
(1, 1, 15.00, 1, 1, 1, N'اختبار اللغة الإنجليزية إجباري للطيارين', GETDATE()),
(1, 3, 20.00, 2, 1, 1, N'اختبار الرياضيات ضروري للحسابات الطيرانية', GETDATE()),
(1, 6, 35.00, 3, 1, 1, N'اختبار الكفاءة الطيرانية - الوزن الأكبر', GETDATE()),
(1, 4, 10.00, 4, 1, 1, N'اللياقة البدنية للطيارين', GETDATE()),
(1, 5, 10.00, 5, 1, 1, N'الفحص الطبي الإجباري', GETDATE()),
(1, 7, 10.00, 6, 1, 1, N'المقابلة الشخصية للتقييم النهائي', GETDATE()),

-- Military University Graduates configuration
(2, 2, 20.00, 1, 1, 1, N'اختبار اللغة العربية للجامعيين العسكريين', GETDATE()),
(2, 1, 15.00, 2, 1, 1, N'اختبار اللغة الإنجليزية', GETDATE()),
(2, 8, 25.00, 3, 1, 1, N'اختبار المعلومات العامة والعسكرية', GETDATE()),
(2, 10, 20.00, 4, 1, 1, N'تقييم المهارات القيادية', GETDATE()),
(2, 4, 10.00, 5, 1, 1, N'اللياقة البدنية', GETDATE()),
(2, 7, 10.00, 6, 1, 1, N'المقابلة الشخصية', GETDATE()),

-- Civilian University Graduates configuration
(3, 2, 25.00, 1, 1, 1, N'اختبار اللغة العربية - الوزن الأكبر', GETDATE()),
(3, 1, 20.00, 2, 1, 1, N'اختبار اللغة الإنجليزية', GETDATE()),
(3, 8, 25.00, 3, 1, 1, N'اختبار المعلومات العامة', GETDATE()),
(3, 9, 15.00, 4, 0, 1, N'اختبار التخصص التقني (اختياري)', GETDATE()),
(3, 4, 10.00, 5, 1, 1, N'اللياقة البدنية', GETDATE()),
(3, 7, 5.00, 6, 1, 1, N'المقابلة الشخصية', GETDATE());

-- Insert Demo Users
INSERT INTO [dbo].[Users] ([Username], [Email], [PasswordHash], [FullName], [Role], [IsActive], [CreatedDate]) VALUES
('admin', 'admin@tajneed.om', '$2a$11$YourHashedPasswordHere', N'مدير النظام', 'Admin', 1, GETDATE()),
('coordinator', 'coordinator@tajneed.om', '$2a$11$YourHashedPasswordHere', N'منسق التجنيد', 'Coordinator', 1, GETDATE()),
('evaluator', 'evaluator@tajneed.om', '$2a$11$YourHashedPasswordHere', N'مقيم', 'Evaluator', 1, GETDATE());

-- Insert Demo Candidates
INSERT INTO [dbo].[Candidates] ([CategoryId], [FullName], [NationalIdNumber], [DateOfBirth], [Governorate], [Phone1], [CurrentRankId], [CurrentAirbaseId], [Major], [University], [GraduationYear], [MarksGrade], [Address], [IsActive], [CurrentStatus], [CreatedDate]) VALUES
(1, N'مرشح تجريبي 1', '10000001', '1995-05-15', N'مسقط', '91234567', 8, 1, N'هندسة طيران', N'جامعة السلطان قابوس', 2020, N'ممتاز', N'مسقط - حي رقم 1', 1, N'جديد', GETDATE()),
(2, N'مرشح تجريبي 2', '10000002', '1993-08-22', N'ظفار', '92345678', 9, 2, N'علوم عسكرية', N'الجامعة الألمانية للتكنولوجيا', 2019, N'جيد جدًا', N'ظفار - حي رقم 2', 1, N'مقبول مبدئيًا', GETDATE()),
(3, N'مرشح تجريبي 3', '10000003', '1994-12-10', N'الداخلية', '93456789', 10, 3, N'إدارة أعمال', N'جامعة نزوى', 2021, N'جيد', N'الداخلية - حي رقم 3', 1, N'قيد التقييم', GETDATE()),
(1, N'مرشح تجريبي 4', '10000004', '1996-03-28', N'الباطنة شمال', '94567890', 8, 4, N'علوم جوية', N'جامعة صحار', 2022, N'ممتاز', N'الباطنة شمال - حي رقم 4', 1, N'جديد', GETDATE()),
(2, N'مرشح تجريبي 5', '10000005', '1992-11-05', N'الباطنة جنوب', '95678901', 9, 5, N'قانون', N'جامعة التقنية والعلوم التطبيقية', 2018, N'جيد جدًا', N'الباطنة جنوب - حي رقم 5', 1, N'مقبول مبدئيًا', GETDATE());

-- =============================================
-- Create Indexes for Performance
-- =============================================

-- Create indexes on frequently queried columns
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[Candidates]') AND name = N'IX_Candidates_NationalIdNumber')
BEGIN
    CREATE UNIQUE INDEX [IX_Candidates_NationalIdNumber] ON [dbo].[Candidates] ([NationalIdNumber]);
END

IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[Candidates]') AND name = N'IX_Candidates_CategoryId')
BEGIN
    CREATE INDEX [IX_Candidates_CategoryId] ON [dbo].[Candidates] ([CategoryId]);
END

IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[CandidateTestResults]') AND name = N'IX_CandidateTestResults_CandidateId')
BEGIN
    CREATE INDEX [IX_CandidateTestResults_CandidateId] ON [dbo].[CandidateTestResults] ([CandidateId]);
END

IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[Users]') AND name = N'IX_Users_Username')
BEGIN
    CREATE UNIQUE INDEX [IX_Users_Username] ON [dbo].[Users] ([Username]);
END

IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[Users]') AND name = N'IX_Users_Email')
BEGIN
    CREATE UNIQUE INDEX [IX_Users_Email] ON [dbo].[Users] ([Email]);
END

-- =============================================
-- Database Backup Complete
-- =============================================

PRINT 'TajneedOffice Database Backup Completed Successfully!';
PRINT 'Database contains:';
PRINT '- 7 Candidate Categories';
PRINT '- 34 Ranks (Military and Civilian)';
PRINT '- 9 Airbases';
PRINT '- 10 Test Types';
PRINT '- 18 Category Test Paths';
PRINT '- 3 Demo Users';
PRINT '- 5 Demo Candidates';
PRINT '';
PRINT 'To restore this database:';
PRINT '1. Run this script on your SQL Server instance';
PRINT '2. Update connection string in appsettings.json';
PRINT '3. Run the application to test functionality'; 