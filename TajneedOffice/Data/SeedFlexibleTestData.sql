-- إضافة أنواع الاختبارات الأساسية
INSERT INTO TestTypes (TestName, TestCode, Description, CriteriaType, MinScore, MaxScore, PassingScore, IsActive, CreatedDate)
VALUES 
    ('اختبار اللغة الإنجليزية', 'ENG', 'اختبار مهارات اللغة الإنجليزية الأساسية', 'درجات', 0, 100, 50, 1, GETDATE()),
    ('اختبار اللغة العربية', 'ARB', 'اختبار مهارات اللغة العربية والإملاء', 'درجات', 0, 100, 60, 1, GETDATE()),
    ('اختبار الرياضيات', 'MTH', 'اختبار المهارات الرياضية الأساسية', 'درجات', 0, 100, 50, 1, GETDATE()),
    ('اختبار اللياقة البدنية', 'FIT', 'تقييم اللياقة البدنية العامة', 'لائق_غير_لائق', NULL, NULL, NULL, 1, GETDATE()),
    ('الفحص الطبي', 'MED', 'الفحص الطبي الشامل', 'لائق_غير_لائق', NULL, NULL, NULL, 1, GETDATE()),
    ('اختبار الكفاءة الطيرانية', 'FLIGHT', 'اختبار مهارات الطيران الأساسية', 'درجات', 0, 100, 70, 1, GETDATE()),
    ('اختبار المقابلة الشخصية', 'INT', 'تقييم المقابلة الشخصية', 'ناجح_راسب', NULL, NULL, NULL, 1, GETDATE()),
    ('اختبار المعلومات العامة', 'GEN', 'اختبار المعلومات العامة والثقافة', 'درجات', 0, 100, 60, 1, GETDATE()),
    ('اختبار التخصص التقني', 'TECH', 'اختبار المهارات التقنية التخصصية', 'درجات', 0, 100, 65, 1, GETDATE()),
    ('تقييم مهارات القيادة', 'LEAD', 'تقييم المهارات القيادية والإدارية', 'درجات', 0, 100, 70, 1, GETDATE());

-- إضافة مسارات الاختبارات للمرشحين الطيارين (CategoryId = 1)
INSERT INTO CategoryTestPaths (CategoryId, TestTypeId, Weight, OrderIndex, IsMandatory, IsActive, Notes, CreatedDate)
VALUES 
    (1, (SELECT TestTypeId FROM TestTypes WHERE TestCode = 'ENG'), 15, 1, 1, 1, 'اختبار اللغة الإنجليزية إجباري للطيارين', GETDATE()),
    (1, (SELECT TestTypeId FROM TestTypes WHERE TestCode = 'MTH'), 20, 2, 1, 1, 'اختبار الرياضيات ضروري للحسابات الطيرانية', GETDATE()),
    (1, (SELECT TestTypeId FROM TestTypes WHERE TestCode = 'FLIGHT'), 35, 3, 1, 1, 'اختبار الكفاءة الطيرانية - الوزن الأكبر', GETDATE()),
    (1, (SELECT TestTypeId FROM TestTypes WHERE TestCode = 'FIT'), 10, 4, 1, 1, 'اللياقة البدنية للطيارين', GETDATE()),
    (1, (SELECT TestTypeId FROM TestTypes WHERE TestCode = 'MED'), 10, 5, 1, 1, 'الفحص الطبي الإجباري', GETDATE()),
    (1, (SELECT TestTypeId FROM TestTypes WHERE TestCode = 'INT'), 10, 6, 1, 1, 'المقابلة الشخصية للتقييم النهائي', GETDATE());

-- إضافة مسارات الاختبارات للجامعيين العسكريين (CategoryId = 2)
INSERT INTO CategoryTestPaths (CategoryId, TestTypeId, Weight, OrderIndex, IsMandatory, IsActive, Notes, CreatedDate)
VALUES 
    (2, (SELECT TestTypeId FROM TestTypes WHERE TestCode = 'ARB'), 20, 1, 1, 1, 'اختبار اللغة العربية للجامعيين العسكريين', GETDATE()),
    (2, (SELECT TestTypeId FROM TestTypes WHERE TestCode = 'ENG'), 15, 2, 1, 1, 'اختبار اللغة الإنجليزية', GETDATE()),
    (2, (SELECT TestTypeId FROM TestTypes WHERE TestCode = 'GEN'), 25, 3, 1, 1, 'اختبار المعلومات العامة والعسكرية', GETDATE()),
    (2, (SELECT TestTypeId FROM TestTypes WHERE TestCode = 'LEAD'), 20, 4, 1, 1, 'تقييم المهارات القيادية', GETDATE()),
    (2, (SELECT TestTypeId FROM TestTypes WHERE TestCode = 'FIT'), 10, 5, 1, 1, 'اللياقة البدنية', GETDATE()),
    (2, (SELECT TestTypeId FROM TestTypes WHERE TestCode = 'INT'), 10, 6, 1, 1, 'المقابلة الشخصية', GETDATE());

-- إضافة مسارات الاختبارات للجامعيين المدنيين (CategoryId = 3)
INSERT INTO CategoryTestPaths (CategoryId, TestTypeId, Weight, OrderIndex, IsMandatory, IsActive, Notes, CreatedDate)
VALUES 
    (3, (SELECT TestTypeId FROM TestTypes WHERE TestCode = 'ARB'), 25, 1, 1, 1, 'اختبار اللغة العربية - الوزن الأكبر', GETDATE()),
    (3, (SELECT TestTypeId FROM TestTypes WHERE TestCode = 'ENG'), 20, 2, 1, 1, 'اختبار اللغة الإنجليزية', GETDATE()),
    (3, (SELECT TestTypeId FROM TestTypes WHERE TestCode = 'GEN'), 25, 3, 1, 1, 'اختبار المعلومات العامة', GETDATE()),
    (3, (SELECT TestTypeId FROM TestTypes WHERE TestCode = 'TECH'), 15, 4, 0, 1, 'اختبار التخصص التقني (اختياري)', GETDATE()),
    (3, (SELECT TestTypeId FROM TestTypes WHERE TestCode = 'FIT'), 10, 5, 1, 1, 'اللياقة البدنية', GETDATE()),
    (3, (SELECT TestTypeId FROM TestTypes WHERE TestCode = 'INT'), 5, 6, 1, 1, 'المقابلة الشخصية', GETDATE());
    
-- تحديث مجموع أوزان الجامعيين المدنيين لتصل إلى 100%
UPDATE CategoryTestPaths 
SET Weight = 20
WHERE CategoryId = 3 AND TestTypeId = (SELECT TestTypeId FROM TestTypes WHERE TestCode = 'ENG');

UPDATE CategoryTestPaths 
SET Weight = 20
WHERE CategoryId = 3 AND TestTypeId = (SELECT TestTypeId FROM TestTypes WHERE TestCode = 'TECH');

-- التحقق من أن مجموع الأوزان = 100% لكل فئة
SELECT 
    cc.CategoryName,
    SUM(ctp.Weight) as TotalWeight,
    COUNT(ctp.PathId) as TestCount
FROM CategoryTestPaths ctp
INNER JOIN CandidateCategories cc ON ctp.CategoryId = cc.CategoryId
WHERE ctp.IsActive = 1
GROUP BY cc.CategoryId, cc.CategoryName
ORDER BY cc.CategoryId; 