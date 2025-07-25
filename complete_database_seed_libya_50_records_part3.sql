-- الجزء الثالث من ملف إدراج البيانات الليبية
-- استكمال الجداول المرتبطة بـ 50 سجل لكل جدول
-- يجب تشغيل الجزأين الأول والثاني أولاً

SET QUOTED_IDENTIFIER ON;
SET ANSI_NULLS ON;
SET NOCOUNT ON;

PRINT N'=== بدء الجزء الثالث من إدراج البيانات ==='

-- ===== 10. جدول المساجد (Mosques) - 50 مسجد =====
PRINT N'إدراج بيانات المساجد...'
BEGIN TRY
    DELETE FROM Mosques;

    -- الحصول على معرفات البنايات
    DECLARE @Buildings TABLE (Id UNIQUEIDENTIFIER, RowNum INT);
    INSERT INTO @Buildings (Id, RowNum)
    SELECT Id, ROW_NUMBER() OVER (ORDER BY Name) as RowNum 
    FROM Buildings 
    WHERE Name LIKE N'%مسجد%';

    INSERT INTO Mosques (Id, BuildingId, MosqueDefinition, MosqueClassification) 
    SELECT 
        NEWID(),
        b.Id,
        CASE 
            WHEN b.RowNum <= 25 THEN 1  -- مسجد جامع
            WHEN b.RowNum <= 40 THEN 2  -- مسجد حي
            ELSE 3                      -- مسجد مصلى
        END as MosqueDefinition,
        CASE 
            WHEN b.RowNum <= 15 THEN 1  -- مسجد حكومي
            WHEN b.RowNum <= 35 THEN 2  -- مسجد تراثي
            ELSE 3                      -- مسجد أهلي
        END as MosqueClassification
    FROM @Buildings b
    WHERE b.RowNum <= 50;

    DECLARE @MosquesCount INT = (SELECT COUNT(*) FROM Mosques);
    PRINT N'تم إدراج ' + CAST(@MosquesCount AS NVARCHAR(10)) + N' مسجد بنجاح!';
END TRY
BEGIN CATCH
    PRINT N'خطأ في إدراج المساجد: ' + ERROR_MESSAGE();
END CATCH

-- ===== 11. جدول تفاصيل البنايات (BuildingDetails) - 50 تفصيل =====
PRINT N'إدراج تفاصيل البنايات...'
BEGIN TRY
    DELETE FROM BuildingDetails;

    -- الحصول على معرفات البنايات
    DECLARE @BuildingsList TABLE (Id UNIQUEIDENTIFIER, RowNum INT);
    INSERT INTO @BuildingsList (Id, RowNum)
    SELECT Id, ROW_NUMBER() OVER (ORDER BY Name) as RowNum 
    FROM Buildings;

    INSERT INTO BuildingDetails (Id, Name, WithinMosqueArea, Floors, BuildingCategory, BuildingId) 
    SELECT 
        NEWID(),
        CASE (b.RowNum % 10)
            WHEN 1 THEN N'المصلى الرئيسي'
            WHEN 2 THEN N'المئذنة'
            WHEN 3 THEN N'المكتبة'
            WHEN 4 THEN N'غرف الإمام'
            WHEN 5 THEN N'دورات المياه'
            WHEN 6 THEN N'مرافق الوضوء'
            WHEN 7 THEN N'المطبخ'
            WHEN 8 THEN N'المخزن'
            WHEN 9 THEN N'قاعة اجتماعات'
            ELSE N'الساحة الخارجية'
        END as Name,
        CASE 
            WHEN (b.RowNum % 10) = 0 THEN 0  -- الساحة خارج المسجد
            ELSE 1                           -- باقي المرافق داخل المسجد
        END as WithinMosqueArea,
        CASE 
            WHEN (b.RowNum % 10) = 0 THEN 0  -- الساحة بلا طوابق
            WHEN (b.RowNum % 3) = 0 THEN 2   -- بعض المرافق طابقين
            ELSE 1                           -- معظم المرافق طابق واحد
        END as Floors,
        (b.RowNum % 5) as BuildingCategory,  -- توزيع الفئات من 0 إلى 4
        b.Id
    FROM @BuildingsList b
    WHERE b.RowNum <= 50;

    DECLARE @BuildingDetailsCount INT = (SELECT COUNT(*) FROM BuildingDetails);
    PRINT N'تم إدراج ' + CAST(@BuildingDetailsCount AS NVARCHAR(10)) + N' تفصيل بناية بنجاح!';
END TRY
BEGIN CATCH
    PRINT N'خطأ في إدراج تفاصيل البنايات: ' + ERROR_MESSAGE();
END CATCH

-- ===== 12. جدول تفاصيل المرافق (FacilityDetails) - 50 تفصيل مرفق =====
PRINT N'إدراج تفاصيل المرافق...'
BEGIN TRY
    DELETE FROM FacilityDetails;

    -- الحصول على معرفات تفاصيل البنايات والمنتجات
    DECLARE @BuildingDetailsList TABLE (Id UNIQUEIDENTIFIER, RowNum INT);
    INSERT INTO @BuildingDetailsList (Id, RowNum)
    SELECT Id, ROW_NUMBER() OVER (ORDER BY Name) as RowNum 
    FROM BuildingDetails;

    DECLARE @ProductsList TABLE (Id UNIQUEIDENTIFIER, RowNum INT);
    INSERT INTO @ProductsList (Id, RowNum)
    SELECT Id, ROW_NUMBER() OVER (ORDER BY Name) as RowNum 
    FROM Products;

    INSERT INTO FacilityDetails (Id, Quantity, ProductId, BuildingDetailId)
    SELECT 
        NEWID(),
        (CAST(RAND(CHECKSUM(NEWID())) * 100 AS INT) + 1) as Quantity,  -- كمية عشوائية من 1 إلى 100
        p.Id,
        bd.Id
    FROM @BuildingDetailsList bd
    CROSS JOIN @ProductsList p
    WHERE bd.RowNum <= 50 AND p.RowNum <= 50 AND (bd.RowNum + p.RowNum) <= 50;

    DECLARE @FacilityDetailsCount INT = (SELECT COUNT(*) FROM FacilityDetails);
    PRINT N'تم إدراج ' + CAST(@FacilityDetailsCount AS NVARCHAR(10)) + N' تفصيل مرفق بنجاح!';
END TRY
BEGIN CATCH
    PRINT N'خطأ في إدراج تفاصيل المرافق: ' + ERROR_MESSAGE();
END CATCH

-- ===== 13. جدول الحسابات (Accounts) - 50 حساب =====
PRINT N'إدراج بيانات الحسابات...'
BEGIN TRY
    DELETE FROM AccountDetails;
    DELETE FROM Accounts;

    DECLARE @LibyanNames TABLE (FirstName NVARCHAR(50), LastName NVARCHAR(50), MotherName NVARCHAR(50));
    INSERT INTO @LibyanNames VALUES
    (N'أحمد', N'محمد علي', N'فاطمة أحمد'),
    (N'محمد', N'عبد الله حسن', N'عائشة محمد'),
    (N'علي', N'أحمد إبراهيم', N'خديجة علي'),
    (N'حسن', N'محمود عبد الله', N'زينب حسن'),
    (N'عبد الله', N'علي محمد', N'أم كلثوم عبد الله'),
    (N'إبراهيم', N'حسن أحمد', N'سعاد إبراهيم'),
    (N'محمود', N'عبد الرحمن علي', N'نجاة محمود'),
    (N'عبد الرحمن', N'يوسف إبراهيم', N'هدى عبد الرحمن'),
    (N'يوسف', N'عبد الله محمد', N'سليمة يوسف'),
    (N'خالد', N'أحمد حسن', N'أمينة خالد'),
    (N'سالم', N'محمد عبد الله', N'صفية سالم'),
    (N'عمر', N'علي إبراهيم', N'رقية عمر'),
    (N'عثمان', N'حسن محمد', N'حليمة عثمان'),
    (N'بلال', N'عبد الله أحمد', N'سمية بلال'),
    (N'زياد', N'محمود علي', N'ليلى زياد'),
    (N'طارق', N'إبراهيم حسن', N'وردة طارق'),
    (N'ماجد', N'أحمد عبد الله', N'نورا ماجد'),
    (N'سامر', N'علي محمد', N'سارة سامر'),
    (N'ناصر', N'حسن إبراهيم', N'مريم ناصر'),
    (N'فيصل', N'عبد الرحمن أحمد', N'جميلة فيصل'),
    (N'وليد', N'يوسف علي', N'كريمة وليد'),
    (N'راشد', N'محمد حسن', N'عزيزة راشد'),
    (N'سعد', N'عبد الله إبراهيم', N'منى سعد'),
    (N'فهد', N'أحمد محمد', N'سلمى فهد'),
    (N'نايف', N'علي عبد الله', N'هناء نايف');

    INSERT INTO Accounts (Id, Name, MotherName, BirthDate, Gender, Barcode, Status, LockerFileNumber, SocialStatus, BookNumber, PaperNumber, RegistrationNumber, AccountNumber, Type, LookOver, Note, NID, IsActive, Balance, UserId, Address, City, Country, ContactNumber, Floors)
    SELECT 
        NEWID(),
        ln.FirstName + N' ' + ln.LastName,
        ln.MotherName,
        DATEADD(YEAR, -((ROW_NUMBER() OVER (ORDER BY ln.FirstName) % 60) + 20), GETDATE()) as BirthDate,  -- أعمار من 20 إلى 80 سنة
        ((ROW_NUMBER() OVER (ORDER BY ln.FirstName) % 2) + 1) as Gender,  -- 1 ذكر، 2 أنثى
        N'BAR' + RIGHT(N'000000' + CAST(ROW_NUMBER() OVER (ORDER BY ln.FirstName) AS NVARCHAR(6)), 6) as Barcode,
        ((ROW_NUMBER() OVER (ORDER BY ln.FirstName) % 3) + 1) as Status,  -- حالات مختلفة
        (ROW_NUMBER() OVER (ORDER BY ln.FirstName) + 1000) as LockerFileNumber,
        ((ROW_NUMBER() OVER (ORDER BY ln.FirstName) % 4) + 1) as SocialStatus,  -- حالات اجتماعية
        (ROW_NUMBER() OVER (ORDER BY ln.FirstName) + 5000) as BookNumber,
        (ROW_NUMBER() OVER (ORDER BY ln.FirstName) + 7000) as PaperNumber,
        (ROW_NUMBER() OVER (ORDER BY ln.FirstName) + 2000) as RegistrationNumber,
        N'ACC' + RIGHT(N'000000' + CAST(ROW_NUMBER() OVER (ORDER BY ln.FirstName) AS NVARCHAR(6)), 6) as AccountNumber,
        ((ROW_NUMBER() OVER (ORDER BY ln.FirstName) % 14) + 1) as Type,  -- أنواع الحسابات المختلفة
        ((ROW_NUMBER() OVER (ORDER BY ln.FirstName) % 2)) as LookOver,  -- 0 أو 1
        N'ملاحظات خاصة بـ ' + ln.FirstName + N' ' + ln.LastName as Note,
        (ROW_NUMBER() OVER (ORDER BY ln.FirstName) + 1200000000) as NID,  -- رقم هوية ليبي
        1 as IsActive,
        (CAST(RAND(CHECKSUM(NEWID())) * 50000 AS DECIMAL(18,2))) as Balance,  -- رصيد عشوائي
        N'admin-libya-001' as UserId,
        N'شارع ' + ln.FirstName + N' - حي ' + ln.LastName as Address,
        CASE ((ROW_NUMBER() OVER (ORDER BY ln.FirstName) % 5) + 1)
            WHEN 1 THEN N'طرابلس'
            WHEN 2 THEN N'بنغازي'
            WHEN 3 THEN N'مصراتة'
            WHEN 4 THEN N'الزاوية'
            ELSE N'البيضاء'
        END as City,
        N'ليبيا' as Country,
        N'0218-' + CAST((90 + (ROW_NUMBER() OVER (ORDER BY ln.FirstName) % 10)) AS NVARCHAR(2)) + N'-' + 
        RIGHT(N'0000000' + CAST(ROW_NUMBER() OVER (ORDER BY ln.FirstName) AS NVARCHAR(7)), 7) as ContactNumber,
        ((ROW_NUMBER() OVER (ORDER BY ln.FirstName) % 3) + 1) as Floors
    FROM @LibyanNames ln
    WHERE ROW_NUMBER() OVER (ORDER BY ln.FirstName) <= 50;

    DECLARE @AccountsCount INT = (SELECT COUNT(*) FROM Accounts);
    PRINT N'تم إدراج ' + CAST(@AccountsCount AS NVARCHAR(10)) + N' حساب بنجاح!';
END TRY
BEGIN CATCH
    PRINT N'خطأ في إدراج الحسابات: ' + ERROR_MESSAGE();
END CATCH

-- ===== 14. جدول تفاصيل الحسابات (AccountDetails) - 50 تفصيل حساب =====
PRINT N'إدراج تفاصيل الحسابات...'
BEGIN TRY
    -- الحصول على معرفات الحسابات
    DECLARE @AccountsList TABLE (Id UNIQUEIDENTIFIER, RowNum INT);
    INSERT INTO @AccountsList (Id, RowNum)
    SELECT Id, ROW_NUMBER() OVER (ORDER BY Name) as RowNum 
    FROM Accounts;

    INSERT INTO AccountDetails (Id, Debtor, Creditor, Note, OperationType, OperationNumber, Balance, AccountId, CreatedDate)
    SELECT 
        NEWID(),
        CASE 
            WHEN (a.RowNum % 2) = 1 THEN CAST(RAND(CHECKSUM(NEWID())) * 10000 AS DECIMAL(18,2))
            ELSE 0.00
        END as Debtor,
        CASE 
            WHEN (a.RowNum % 2) = 0 THEN CAST(RAND(CHECKSUM(NEWID())) * 8000 AS DECIMAL(18,2))
            ELSE 0.00
        END as Creditor,
        N'عملية ' + 
        CASE (a.RowNum % 5)
            WHEN 0 THEN N'إيداع نقدي'
            WHEN 1 THEN N'سحب نقدي'
            WHEN 2 THEN N'تحويل مصرفي'
            WHEN 3 THEN N'رسوم خدمة'
            ELSE N'فوائد'
        END as Note,
        (a.RowNum % 2) as OperationType,  -- 0 Credit, 1 Debit
        (a.RowNum + 10000) as OperationNumber,
        CAST(RAND(CHECKSUM(NEWID())) * 25000 AS DECIMAL(18,2)) as Balance,
        a.Id,
        DATEADD(DAY, -(a.RowNum * 3), GETDATE()) as CreatedDate
    FROM @AccountsList a
    WHERE a.RowNum <= 50;

    DECLARE @AccountDetailsCount INT = (SELECT COUNT(*) FROM AccountDetails);
    PRINT N'تم إدراج ' + CAST(@AccountDetailsCount AS NVARCHAR(10)) + N' تفصيل حساب بنجاح!';
END TRY
BEGIN CATCH
    PRINT N'خطأ في إدراج تفاصيل الحسابات: ' + ERROR_MESSAGE();
END CATCH

-- ===== 15. جدول القرارات (Decisions) - 50 قرار =====
PRINT N'إدراج بيانات القرارات...'
BEGIN TRY
    DELETE FROM Decisions;

    INSERT INTO Decisions (Id, Title, Description, CreatedDate, ReferenceNumber, UserId) VALUES
    (NEWID(), N'قرار تعيين مدير عام', N'قرار بتعيين السيد أحمد محمد علي مديراً عاماً للمؤسسة', DATEADD(day, -365, GETDATE()), N'DEC-2024-001', N'admin-libya-001'),
    (NEWID(), N'قرار إنشاء فرع جديد بنغازي', N'قرار بإنشاء فرع جديد في مدينة بنغازي', DATEADD(day, -350, GETDATE()), N'DEC-2024-002', N'admin-libya-001'),
    (NEWID(), N'قرار شراء معدات مكتبية', N'قرار بشراء معدات مكتبية جديدة للمكاتب', DATEADD(day, -340, GETDATE()), N'DEC-2024-003', N'admin-libya-001'),
    (NEWID(), N'قرار تطوير النظام الإلكتروني', N'قرار بتطوير النظام الإلكتروني للمؤسسة', DATEADD(day, -330, GETDATE()), N'DEC-2024-004', N'admin-libya-001'),
    (NEWID(), N'قرار تدريب الموظفين', N'قرار بتنظيم دورات تدريبية للموظفين', DATEADD(day, -320, GETDATE()), N'DEC-2024-005', N'admin-libya-001'),
    (NEWID(), N'قرار الميزانية السنوية', N'قرار بإنشاء الميزانية السنوية لعام 2024', DATEADD(day, -310, GETDATE()), N'DEC-2024-006', N'admin-libya-001'),
    (NEWID(), N'قرار زيادة الرواتب', N'قرار بزيادة رواتب الموظفين بنسبة 10%', DATEADD(day, -300, GETDATE()), N'DEC-2024-007', N'admin-libya-001'),
    (NEWID(), N'قرار شراء عقار جديد', N'قرار بشراء عقار جديد للمؤسسة في طرابلس', DATEADD(day, -290, GETDATE()), N'DEC-2024-008', N'admin-libya-001'),
    (NEWID(), N'قرار استثمار الأموال', N'قرار باستثمار الأموال في مشاريع مربحة', DATEADD(day, -280, GETDATE()), N'DEC-2024-009', N'admin-libya-001'),
    (NEWID(), N'قرار إعادة هيكلة مالية', N'قرار بإعادة هيكلة الوضع المالي للمؤسسة', DATEADD(day, -270, GETDATE()), N'DEC-2024-010', N'admin-libya-001'),
    (NEWID(), N'قرار تعديل اللوائح الداخلية', N'قرار بتعديل اللوائح الداخلية للمؤسسة', DATEADD(day, -260, GETDATE()), N'DEC-2024-011', N'admin-libya-001'),
    (NEWID(), N'قرار إنشاء أقسام جديدة', N'قرار بإنشاء أقسام جديدة في المؤسسة', DATEADD(day, -250, GETDATE()), N'DEC-2024-012', N'admin-libya-001'),
    (NEWID(), N'قرار إعادة تنظيم الهيكل', N'قرار بإعادة تنظيم الهيكل التنظيمي', DATEADD(day, -240, GETDATE()), N'DEC-2024-013', N'admin-libya-001'),
    (NEWID(), N'قرار إنشاء لجان متخصصة', N'قرار بإنشاء لجان متخصصة في المؤسسة', DATEADD(day, -230, GETDATE()), N'DEC-2024-014', N'admin-libya-001'),
    (NEWID(), N'قرار تطوير السياسات', N'قرار بتطوير السياسات الإدارية للمؤسسة', DATEADD(day, -220, GETDATE()), N'DEC-2024-015', N'admin-libya-001'),
    (NEWID(), N'قرار بدء مشروع جديد', N'قرار ببدء مشروع تطوير البنية التحتية', DATEADD(day, -210, GETDATE()), N'DEC-2024-016', N'admin-libya-001'),
    (NEWID(), N'قرار توقيع عقد مقاولات', N'قرار بتوقيع عقد مع شركة مقاولات ليبية', DATEADD(day, -200, GETDATE()), N'DEC-2024-017', N'admin-libya-001'),
    (NEWID(), N'قرار إيقاف مشروع', N'قرار بإيقاف مشروع غير مجدٍ اقتصادياً', DATEADD(day, -190, GETDATE()), N'DEC-2024-018', N'admin-libya-001'),
    (NEWID(), N'قرار توسيع مشروع قائم', N'قرار بتوسيع نطاق مشروع قائم في مصراتة', DATEADD(day, -180, GETDATE()), N'DEC-2024-019', N'admin-libya-001'),
    (NEWID(), N'قرار إنهاء مشروع بنجاح', N'قرار بإنهاء مشروع بنجاح في الزاوية', DATEADD(day, -170, GETDATE()), N'DEC-2024-020', N'admin-libya-001'),
    (NEWID(), N'قرار تعيين موظفين جدد', N'قرار بتعيين 10 موظفين جدد', DATEADD(day, -160, GETDATE()), N'DEC-2024-021', N'admin-libya-001'),
    (NEWID(), N'قرار ترقية موظفين', N'قرار بترقية 5 موظفين متميزين', DATEADD(day, -150, GETDATE()), N'DEC-2024-022', N'admin-libya-001'),
    (NEWID(), N'قرار إنهاء خدمات موظفين', N'قرار بإنهاء خدمات موظفين مخالفين', DATEADD(day, -140, GETDATE()), N'DEC-2024-023', N'admin-libya-001'),
    (NEWID(), N'قرار إجازة إدارية', N'قرار بمنح إجازة إدارية لموظف', DATEADD(day, -130, GETDATE()), N'DEC-2024-024', N'admin-libya-001'),
    (NEWID(), N'قرار نقل موظف', N'قرار بنقل موظف إلى فرع آخر', DATEADD(day, -120, GETDATE()), N'DEC-2024-025', N'admin-libya-001'),
    (NEWID(), N'قرار تحديث الأنظمة التقنية', N'قرار بتحديث الأنظمة التقنية للمؤسسة', DATEADD(day, -110, GETDATE()), N'DEC-2024-026', N'admin-libya-001'),
    (NEWID(), N'قرار شراء أجهزة كمبيوتر', N'قرار بشراء أجهزة كمبيوتر جديدة', DATEADD(day, -100, GETDATE()), N'DEC-2024-027', N'admin-libya-001'),
    (NEWID(), N'قرار تطوير الموقع الإلكتروني', N'قرار بتطوير الموقع الإلكتروني للمؤسسة', DATEADD(day, -90, GETDATE()), N'DEC-2024-028', N'admin-libya-001'),
    (NEWID(), N'قرار تحسين أمن المعلومات', N'قرار بتحسين أمن المعلومات والبيانات', DATEADD(day, -80, GETDATE()), N'DEC-2024-029', N'admin-libya-001'),
    (NEWID(), N'قرار نظام نسخ احتياطي', N'قرار بإنشاء نظام نسخ احتياطي', DATEADD(day, -70, GETDATE()), N'DEC-2024-030', N'admin-libya-001'),
    (NEWID(), N'قرار تحسين جودة الخدمات', N'قرار بتحسين جودة الخدمات المقدمة', DATEADD(day, -60, GETDATE()), N'DEC-2024-031', N'admin-libya-001'),
    (NEWID(), N'قرار خدمة إلكترونية جديدة', N'قرار بإنشاء خدمة إلكترونية جديدة', DATEADD(day, -50, GETDATE()), N'DEC-2024-032', N'admin-libya-001'),
    (NEWID(), N'قرار تطوير الخدمات الحالية', N'قرار بتطوير الخدمات الحالية للمواطنين', DATEADD(day, -40, GETDATE()), N'DEC-2024-033', N'admin-libya-001'),
    (NEWID(), N'قرار إيقاف خدمة', N'قرار بإيقاف خدمة غير مستخدمة', DATEADD(day, -30, GETDATE()), N'DEC-2024-034', N'admin-libya-001'),
    (NEWID(), N'قرار توسيع نطاق الخدمات', N'قرار بتوسيع نطاق الخدمات المقدمة', DATEADD(day, -20, GETDATE()), N'DEC-2024-035', N'admin-libya-001'),
    (NEWID(), N'قرار تحسين الأمن', N'قرار بتحسين الإجراءات الأمنية', DATEADD(day, -15, GETDATE()), N'DEC-2024-036', N'admin-libya-001'),
    (NEWID(), N'قرار نظام مراقبة أمنية', N'قرار بإنشاء نظام مراقبة أمنية', DATEADD(day, -14, GETDATE()), N'DEC-2024-037', N'admin-libya-001'),
    (NEWID(), N'قرار تعيين حراس أمن', N'قرار بتعيين حراس أمن جدد', DATEADD(day, -13, GETDATE()), N'DEC-2024-038', N'admin-libya-001'),
    (NEWID(), N'قرار تحديث الأنظمة الأمنية', N'قرار بتحديث الأنظمة الأمنية للمباني', DATEADD(day, -12, GETDATE()), N'DEC-2024-039', N'admin-libya-001'),
    (NEWID(), N'قرار تدريبات أمنية', N'قرار بتنظيم تدريبات أمنية للموظفين', DATEADD(day, -11, GETDATE()), N'DEC-2024-040', N'admin-libya-001'),
    (NEWID(), N'قرار صيانة دورية', N'قرار بإجراء صيانة دورية للمباني', DATEADD(day, -10, GETDATE()), N'DEC-2024-041', N'admin-libya-001'),
    (NEWID(), N'قرار إصلاح عاجل', N'قرار بإصلاح عاجل لبعض المرافق', DATEADD(day, -9, GETDATE()), N'DEC-2024-042', N'admin-libya-001'),
    (NEWID(), N'قرار تحديث المرافق', N'قرار بتحديث المرافق القديمة', DATEADD(day, -8, GETDATE()), N'DEC-2024-043', N'admin-libya-001'),
    (NEWID(), N'قرار شراء معدات صيانة', N'قرار بشراء معدات صيانة جديدة', DATEADD(day, -7, GETDATE()), N'DEC-2024-044', N'admin-libya-001'),
    (NEWID(), N'قرار تعيين فنيي صيانة', N'قرار بتعيين فنيي صيانة جدد', DATEADD(day, -6, GETDATE()), N'DEC-2024-045', N'admin-libya-001'),
    (NEWID(), N'قرار مركز تدريب', N'قرار بإنشاء مركز تدريب متخصص', DATEADD(day, -5, GETDATE()), N'DEC-2024-046', N'admin-libya-001'),
    (NEWID(), N'قرار ورش عمل', N'قرار بتنظيم ورش عمل للموظفين', DATEADD(day, -4, GETDATE()), N'DEC-2024-047', N'admin-libya-001'),
    (NEWID(), N'قرار بعثات تدريبية', N'قرار بإرسال بعثات تدريبية للخارج', DATEADD(day, -3, GETDATE()), N'DEC-2024-048', N'admin-libya-001'),
    (NEWID(), N'قرار تطوير المهارات', N'قرار بتطوير مهارات الموظفين', DATEADD(day, -2, GETDATE()), N'DEC-2024-049', N'admin-libya-001'),
    (NEWID(), N'قرار إنشاء مكتبة', N'قرار بإنشاء مكتبة للمؤسسة', DATEADD(day, -1, GETDATE()), N'DEC-2024-050', N'admin-libya-001');

    DECLARE @DecisionsCount INT = (SELECT COUNT(*) FROM Decisions);
    PRINT N'تم إدراج ' + CAST(@DecisionsCount AS NVARCHAR(10)) + N' قرار بنجاح!';
END TRY
BEGIN CATCH
    PRINT N'خطأ في إدراج القرارات: ' + ERROR_MESSAGE();
END CATCH

-- ===== 16. جدول المدارس القرآنية (QuranicSchools) - 50 مدرسة =====
PRINT N'إدراج بيانات المدارس القرآنية...'
BEGIN TRY
    DELETE FROM QuranicSchools;

    -- الحصول على معرفات البنايات التي يمكن أن تكون مدارس قرآنية
    DECLARE @QuranicBuildings TABLE (Id UNIQUEIDENTIFIER, RowNum INT);
    INSERT INTO @QuranicBuildings (Id, RowNum)
    SELECT Id, ROW_NUMBER() OVER (ORDER BY Name) as RowNum 
    FROM Buildings
    WHERE Name LIKE N'%مسجد%';

    INSERT INTO QuranicSchools (Id, BuildingId)
    SELECT 
        NEWID(),
        qb.Id
    FROM @QuranicBuildings qb
    WHERE qb.RowNum <= 50;

    DECLARE @QuranicSchoolsCount INT = (SELECT COUNT(*) FROM QuranicSchools);
    PRINT N'تم إدراج ' + CAST(@QuranicSchoolsCount AS NVARCHAR(10)) + N' مدرسة قرآنية بنجاح!';
END TRY
BEGIN CATCH
    PRINT N'خطأ في إدراج المدارس القرآنية: ' + ERROR_MESSAGE();
END CATCH

-- ===== عرض إحصائيات شاملة نهائية =====
PRINT N''
PRINT N'=== إحصائيات شاملة للبيانات المدرجة ==='
DECLARE @TotalCities INT = (SELECT COUNT(*) FROM Cities);
DECLARE @TotalRegions INT = (SELECT COUNT(*) FROM Regions);
DECLARE @TotalBanks INT = (SELECT COUNT(*) FROM Banks);
DECLARE @TotalBranches INT = (SELECT COUNT(*) FROM Branches);
DECLARE @TotalProducts INT = (SELECT COUNT(*) FROM Products);
DECLARE @TotalFacilities INT = (SELECT COUNT(*) FROM Facilities);
DECLARE @TotalOffices INT = (SELECT COUNT(*) FROM Offices);
DECLARE @TotalBuildings INT = (SELECT COUNT(*) FROM Buildings);
DECLARE @TotalMosques INT = (SELECT COUNT(*) FROM Mosques);
DECLARE @TotalBuildingDetails INT = (SELECT COUNT(*) FROM BuildingDetails);
DECLARE @TotalFacilityDetails INT = (SELECT COUNT(*) FROM FacilityDetails);
DECLARE @TotalAccounts INT = (SELECT COUNT(*) FROM Accounts);
DECLARE @TotalAccountDetails INT = (SELECT COUNT(*) FROM AccountDetails);
DECLARE @TotalDecisions INT = (SELECT COUNT(*) FROM Decisions);
DECLARE @TotalQuranicSchools INT = (SELECT COUNT(*) FROM QuranicSchools);

PRINT N'عدد المدن: ' + CAST(@TotalCities AS NVARCHAR(10));
PRINT N'عدد المناطق: ' + CAST(@TotalRegions AS NVARCHAR(10));
PRINT N'عدد البنوك: ' + CAST(@TotalBanks AS NVARCHAR(10));
PRINT N'عدد الفروع: ' + CAST(@TotalBranches AS NVARCHAR(10));
PRINT N'عدد المنتجات: ' + CAST(@TotalProducts AS NVARCHAR(10));
PRINT N'عدد المرافق: ' + CAST(@TotalFacilities AS NVARCHAR(10));
PRINT N'عدد المكاتب: ' + CAST(@TotalOffices AS NVARCHAR(10));
PRINT N'عدد البنايات: ' + CAST(@TotalBuildings AS NVARCHAR(10));
PRINT N'عدد المساجد: ' + CAST(@TotalMosques AS NVARCHAR(10));
PRINT N'عدد تفاصيل البنايات: ' + CAST(@TotalBuildingDetails AS NVARCHAR(10));
PRINT N'عدد تفاصيل المرافق: ' + CAST(@TotalFacilityDetails AS NVARCHAR(10));
PRINT N'عدد الحسابات: ' + CAST(@TotalAccounts AS NVARCHAR(10));
PRINT N'عدد تفاصيل الحسابات: ' + CAST(@TotalAccountDetails AS NVARCHAR(10));
PRINT N'عدد القرارات: ' + CAST(@TotalDecisions AS NVARCHAR(10));
PRINT N'عدد المدارس القرآنية: ' + CAST(@TotalQuranicSchools AS NVARCHAR(10));

PRINT N''
PRINT N'=== تم إدراج جميع البيانات الليبية بنجاح! 🇱🇾 ==='
PRINT N'=== يحتوي النظام الآن على بيانات شاملة من ليبيا بالترميز العربي ==='
PRINT N'=== يمكنك الآن استخدام النظام لإدارة الأوقاف والمساجد والبنايات ==='