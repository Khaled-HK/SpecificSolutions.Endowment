-- ملف SQL موحد لتنفيذ عمليات حذف البيانات وإضافة البيانات الليبية
-- Database: Swagger_Endowment22
-- يجب تنفيذ هذا الملف في SQL Server Management Studio

USE [Swagger_Endowment22]
GO

-- =====================================================
-- الجزء الأول: حذف جميع البيانات الموجودة
-- =====================================================

PRINT N'======================================='
PRINT N'بدء عملية تنظيف قاعدة البيانات'
PRINT N'Starting database cleanup process'
PRINT N'======================================='

BEGIN TRY
    -- تعطيل فحص القيود مؤقتاً
    EXEC sp_msforeachtable "ALTER TABLE ? NOCHECK CONSTRAINT all"
    PRINT N'تم تعطيل القيود مؤقتاً'
    
    -- حذف البيانات من جميع الجداول
    DELETE FROM [AspNetUserRoles];
    DELETE FROM [AspNetUserClaims];
    DELETE FROM [AspNetUserLogins];
    DELETE FROM [AspNetUserTokens];
    DELETE FROM [AspNetRoleClaims];
    DELETE FROM [AspNetUsers];
    DELETE FROM [AspNetRoles];
    
    DELETE FROM [BuildingDetailRequests];
    DELETE FROM [ChangeOfPathRequests];
    DELETE FROM [ConstructionRequests];
    DELETE FROM [DemolitionRequests];
    DELETE FROM [ExpenditureChangeRequests];
    DELETE FROM [MaintenanceRequests];
    DELETE FROM [NameChangeRequests];
    DELETE FROM [NeedsRequests];
    DELETE FROM [Requests];
    
    DELETE FROM [AccountDetails];
    DELETE FROM [BuildingDetails];
    DELETE FROM [FacilityDetails];
    DELETE FROM [AuditLogs];
    DELETE FROM [Accounts];
    DELETE FROM [Mosques];
    DELETE FROM [Buildings];
    DELETE FROM [Facilities];
    DELETE FROM [Decisions];
    DELETE FROM [Offices];
    DELETE FROM [Products];
    DELETE FROM [Regions];
    DELETE FROM [Cities];
    DELETE FROM [Banks];
    
    -- إعادة تفعيل القيود
    EXEC sp_msforeachtable "ALTER TABLE ? WITH CHECK CHECK CONSTRAINT all"
    
    PRINT N'تم حذف جميع البيانات بنجاح!'
    
END TRY
BEGIN CATCH
    PRINT N'حدث خطأ أثناء حذف البيانات:'
    PRINT ERROR_MESSAGE()
    EXEC sp_msforeachtable "ALTER TABLE ? WITH CHECK CHECK CONSTRAINT all"
END CATCH

-- =====================================================
-- الجزء الثاني: إضافة البيانات الليبية الجديدة
-- =====================================================

PRINT N''
PRINT N'======================================='
PRINT N'بدء إضافة البيانات الليبية الجديدة'
PRINT N'Starting Libyan data insertion'
PRINT N'======================================='

-- إضافة البنوك الليبية
PRINT N'إضافة البنوك الليبية...'
INSERT INTO [Banks] ([Id], [Name], [Address], [ContactNumber]) VALUES
(NEWID(), N'مصرف الجمهورية', N'شارع عمر المختار، طرابلس', N'021-4567890'),
(NEWID(), N'البنك التجاري الوطني', N'شارع الاستقلال، بنغازي', N'061-3456789'),
(NEWID(), N'مصرف الوحدة', N'شارع الجلاء، طرابلس', N'021-2345678'),
(NEWID(), N'البنك الأهلي الليبي', N'شارع جمال عبدالناصر، بنغازي', N'061-4567890'),
(NEWID(), N'مصرف الصحاري', N'طريق المطار، طرابلس', N'021-5678901'),
(NEWID(), N'بنك التنمية', N'شارع الفاتح، مصراتة', N'051-6789012'),
(NEWID(), N'المصرف المركزي الليبي', N'شارع الشط، طرابلس', N'021-7890123'),
(NEWID(), N'مصرف الخليج الليبي', N'شارع عمر بن الخطاب، بنغازي', N'061-8901234'),
(NEWID(), N'البنك الليبي للتجارة والتنمية', N'شارع الكورنيش، طرابلس', N'021-9012345'),
(NEWID(), N'مصرف الادخار والاستثمار', N'شارع النصر، سبها', N'071-0123456');

-- إضافة المدن الليبية
PRINT N'إضافة المدن الليبية...'
DECLARE @CityId1 UNIQUEIDENTIFIER = NEWID()
DECLARE @CityId2 UNIQUEIDENTIFIER = NEWID()
DECLARE @CityId3 UNIQUEIDENTIFIER = NEWID()
DECLARE @CityId4 UNIQUEIDENTIFIER = NEWID()
DECLARE @CityId5 UNIQUEIDENTIFIER = NEWID()

INSERT INTO [Cities] ([Id], [Name], [Country]) VALUES
(@CityId1, N'طرابلس', N'ليبيا'),
(@CityId2, N'بنغازي', N'ليبيا'),
(@CityId3, N'مصراتة', N'ليبيا'),
(@CityId4, N'الزاوية', N'ليبيا'),
(@CityId5, N'سبها', N'ليبيا');

-- إضافة المناطق الليبية
PRINT N'إضافة المناطق الليبية...'
DECLARE @RegionId1 UNIQUEIDENTIFIER = NEWID()
DECLARE @RegionId2 UNIQUEIDENTIFIER = NEWID()
DECLARE @RegionId3 UNIQUEIDENTIFIER = NEWID()
DECLARE @RegionId4 UNIQUEIDENTIFIER = NEWID()
DECLARE @RegionId5 UNIQUEIDENTIFIER = NEWID()

INSERT INTO [Regions] ([Id], [Name], [Country], [CityId]) VALUES
(@RegionId1, N'منطقة الدهماني', N'ليبيا', @CityId1),
(@RegionId2, N'منطقة الصابري', N'ليبيا', @CityId2),
(@RegionId3, N'منطقة قصر أحمد', N'ليبيا', @CityId3),
(@RegionId4, N'منطقة المدينة القديمة', N'ليبيا', @CityId4),
(@RegionId5, N'منطقة الصالحين', N'ليبيا', @CityId5);

-- إضافة المنتجات
PRINT N'إضافة المنتجات...'
DECLARE @ProductId1 UNIQUEIDENTIFIER = NEWID()
DECLARE @ProductId2 UNIQUEIDENTIFIER = NEWID()
DECLARE @ProductId3 UNIQUEIDENTIFIER = NEWID()
DECLARE @ProductId4 UNIQUEIDENTIFIER = NEWID()
DECLARE @ProductId5 UNIQUEIDENTIFIER = NEWID()

INSERT INTO [Products] ([Id], [Name], [Description]) VALUES
(@ProductId1, N'مصاحف القرآن الكريم', N'مصاحف عالية الجودة للمساجد والمراكز الإسلامية في ليبيا'),
(@ProductId2, N'كتب التفسير', N'مجموعة شاملة من كتب تفسير القرآن الكريم'),
(@ProductId3, N'كتب الحديث الشريف', N'مجموعات الحديث الشريف والسنة النبوية'),
(@ProductId4, N'كتب الفقه المالكي', N'كتب الفقه المالكي السائد في ليبيا'),
(@ProductId5, N'أجهزة الصوت والصوتيات', N'أنظمة الصوت للمساجد والمحاضرات');

-- إضافة المكاتب الليبية
PRINT N'إضافة المكاتب الليبية...'
DECLARE @OfficeId1 UNIQUEIDENTIFIER = NEWID()
DECLARE @OfficeId2 UNIQUEIDENTIFIER = NEWID()
DECLARE @OfficeId3 UNIQUEIDENTIFIER = NEWID()
DECLARE @OfficeId4 UNIQUEIDENTIFIER = NEWID()
DECLARE @OfficeId5 UNIQUEIDENTIFIER = NEWID()

INSERT INTO [Offices] ([Id], [Name], [Location], [PhoneNumber], [RegionId], [UserId]) VALUES
(@OfficeId1, N'مكتب أوقاف طرابلس المركزي', N'شارع عمر المختار، منطقة الدهماني', N'021-4567890', @RegionId1, NEWID()),
(@OfficeId2, N'مكتب أوقاف بنغازي الرئيسي', N'شارع الاستقلال، منطقة الصابري', N'061-3456789', @RegionId2, NEWID()),
(@OfficeId3, N'مكتب أوقاف مصراتة', N'شارع الشهداء، منطقة قصر أحمد', N'051-2345678', @RegionId3, NEWID()),
(@OfficeId4, N'مكتب أوقاف الزاوية', N'شارع الجمهورية، المدينة القديمة', N'023-5678901', @RegionId4, NEWID()),
(@OfficeId5, N'مكتب أوقاف سبها', N'شارع النصر، منطقة الصالحين', N'071-6789012', @RegionId5, NEWID());

-- إضافة المرافق الليبية
PRINT N'إضافة المرافق الليبية...'
INSERT INTO [Facilities] ([Id], [Name], [Location], [ContactInfo], [Capacity], [Status]) VALUES
(NEWID(), N'مركز تحفيظ القرآن الكريم - طرابلس', N'منطقة الدهماني، طرابلس', N'021-4567890', 200, N'نشط'),
(NEWID(), N'مكتبة الأوقاف الإسلامية - بنغازي', N'منطقة الصابري، بنغازي', N'061-3456789', 150, N'نشط'),
(NEWID(), N'مركز الدعوة والإرشاد - مصراتة', N'منطقة قصر أحمد، مصراتة', N'051-2345678', 300, N'نشط'),
(NEWID(), N'معهد العلوم الشرعية - الزاوية', N'المدينة القديمة، الزاوية', N'023-5678901', 250, N'نشط'),
(NEWID(), N'مركز التدريب الإسلامي - سبها', N'منطقة الصالحين، سبها', N'071-6789012', 180, N'نشط');

-- إضافة المباني الليبية
PRINT N'إضافة المباني الليبية...'
DECLARE @BuildingId1 UNIQUEIDENTIFIER = NEWID()
DECLARE @BuildingId2 UNIQUEIDENTIFIER = NEWID()
DECLARE @BuildingId3 UNIQUEIDENTIFIER = NEWID()
DECLARE @BuildingId4 UNIQUEIDENTIFIER = NEWID()
DECLARE @BuildingId5 UNIQUEIDENTIFIER = NEWID()

INSERT INTO [Buildings] ([Id], [Name], [FileNumber], [Definition], [Classification], [OfficeId], [Unit], [RegionId], [NearestLandmark], [ConstructionDate], [OpeningDate], [MapLocation], [TotalLandArea], [TotalCoveredArea], [NumberOfFloors], [ElectricityMeter], [AlternativeEnergySource], [WaterSource], [Sanitation], [BriefDescription], [LandDonorName], [SourceFunds], [PrayerCapacity], [UserId], [ServicesSpecialNeeds], [SpecialEntranceWomen], [PicturePath]) VALUES
(@BuildingId1, N'جامع طرابلس الكبير', N'TRP-001', N'مسجد جامع', N'مسجد رئيسي', @OfficeId1, N'الوحدة الأولى', @RegionId1, N'بجوار مستشفى طرابلس المركزي', '2020-01-15', '2020-06-01', N'32.8925,13.1802', 2500.0, 1800.0, 2, N'123456789', N'طاقة شمسية', N'شبكة المياه العامة', N'شبكة الصرف الصحي', N'مسجد حديث ومجهز بأحدث التقنيات', N'عائلة القذافي', 0, N'800 مصل', N'user1', 1, 1, N'/images/mosque1.jpg'),
(@BuildingId2, N'مسجد الصحابة', N'BNG-002', N'مسجد حي', N'مسجد فرعي', @OfficeId2, N'الوحدة الثانية', @RegionId2, N'قرب سوق الصابري', '2019-03-10', '2019-08-15', N'32.1181,20.0685', 1800.0, 1200.0, 1, N'987654321', N'لا يوجد', N'خزان علوي', N'خزان تجميع', N'مسجد متوسط الحجم يخدم الحي', N'محمد الكيلاني', 1, N'400 مصل', N'user2', 0, 1, N'/images/mosque2.jpg'),
(@BuildingId3, N'جامع الشهداء', N'MSR-003', N'مسجد جامع', N'مسجد رئيسي', @OfficeId3, N'الوحدة الثالثة', @RegionId3, N'شارع قصر أحمد الرئيسي', '2018-05-20', '2018-12-01', N'32.3740,15.0912', 3000.0, 2200.0, 3, N'555666777', N'طاقة شمسية', N'شبكة المياه العامة', N'شبكة الصرف الصحي', N'مسجد كبير مع مكتبة إسلامية', N'مؤسسة الجماهيرية الخيرية', 0, N'1200 مصل', N'user3', 1, 1, N'/images/mosque3.jpg'),
(@BuildingId4, N'مسجد عمر بن الخطاب', N'ZAW-004', N'مسجد حي', N'مسجد فرعي', @OfficeId4, N'الوحدة الرابعة', @RegionId4, N'بجوار سوق الزاوية القديم', '2021-02-01', '2021-07-10', N'32.7569,12.7276', 1500.0, 1000.0, 2, N'111222333', N'لا يوجد', N'بئر ارتوازي', N'شبكة الصرف الصحي', N'مسجد تاريخي تم تجديده', N'عبدالسلام الطرابلسي', 1, N'350 مصل', N'user4', 0, 0, N'/images/mosque4.jpg'),
(@BuildingId5, N'جامع الفاتح', N'SBH-005', N'مسجد جامع', N'مسجد رئيسي', @OfficeId5, N'الوحدة الخامسة', @RegionId5, N'وسط مدينة سبها', '2020-09-15', '2021-01-20', N'27.0377,14.4283', 2200.0, 1600.0, 2, N'444555666', N'طاقة شمسية', N'شبكة المياه العامة', N'شبكة الصرف الصحي', N'مسجد عصري مع قاعة محاضرات', N'أحمد الفزاني', 0, N'700 مصل', N'user5', 1, 1, N'/images/mosque5.jpg');

-- إضافة المساجد
PRINT N'إضافة المساجد...'
INSERT INTO [Mosques] ([Id], [BuildingId], [MosqueDefinition], [MosqueClassification]) VALUES
(NEWID(), @BuildingId1, 0, 0),
(NEWID(), @BuildingId2, 1, 1),
(NEWID(), @BuildingId3, 0, 0),
(NEWID(), @BuildingId4, 1, 1),
(NEWID(), @BuildingId5, 0, 0);

-- إضافة الحسابات الليبية
PRINT N'إضافة الحسابات الليبية...'
INSERT INTO [Accounts] ([Id], [Name], [MotherName], [BirthDate], [Gender], [Barcode], [Status], [LockerFileNumber], [SocialStatus], [BookNumber], [PaperNumber], [RegistrationNumber], [AccountNumber], [Type], [LookOver], [Note], [NID], [IsActive], [Balance], [UserId], [Address], [City], [Country], [ContactNumber], [Floors]) VALUES
(NEWID(), N'أحمد محمد الطرابلسي', N'فاطمة صالح', '1985-03-15', 1, N'ACC001', 1, 1001, 1, 101, 201, 301, N'AC-2024-001', 3, 0, N'حساب مستفيد رئيسي', 1234567890, 1, 1500.50, N'user1', N'منطقة الدهماني، شارع عمر المختار', N'طرابلس', N'ليبيا', N'091-234567', 2),
(NEWID(), N'فاطمة عبدالرحمن البنغازي', N'عائشة محمد', '1990-07-22', 2, N'ACC002', 1, 1002, 2, 102, 202, 302, N'AC-2024-002', 1, 1, N'حساب أرملة شهيد', 1234567891, 1, 2500.75, N'user2', N'منطقة الصابري، شارع الاستقلال', N'بنغازي', N'ليبيا', N'092-345678', 1),
(NEWID(), N'محمد سعد المصراتي', N'خديجة أحمد', '1978-12-10', 1, 1003, 1, 1003, 1, 103, 203, 303, N'AC-2024-003', 2, 0, N'حساب ذوي الاحتياجات الخاصة', 1234567892, 1, 1800.25, N'user3', N'منطقة قصر أحمد، شارع الشهداء', N'مصراتة', N'ليبيا', N'091-456789', 1),
(NEWID(), N'عائشة خالد الزاوي', N'زينب عبدالله', '1995-09-18', 2, N'ACC004', 1, 1004, 4, 104, 204, 304, N'AC-2024-004', 3, 0, N'حساب مستفيدة عزباء', 1234567893, 1, 1200.00, N'user4', N'المدينة القديمة، شارع الجمهورية', N'الزاوية', N'ليبيا', N'091-567890', 1),
(NEWID(), N'عبدالله فهد الفزاني', N'مريم صالح', '1982-05-30', 1, N'ACC005', 1, 1005, 1, 105, 205, 305, N'AC-2024-005', 3, 1, N'حساب مستفيد متزوج', 1234567894, 1, 2200.80, N'user5', N'منطقة الصالحين، شارع النصر', N'سبها', N'ليبيا', N'071-678901', 2);

-- إضافة بعض القرارات
PRINT N'إضافة القرارات...'
INSERT INTO [Decisions] ([Id], [Number], [Date], [Description], [Status]) VALUES
(NEWID(), N'QR-2024-001', '2024-01-10', N'قرار إنشاء مسجد جديد في منطقة الدهماني بطرابلس', N'معتمد'),
(NEWID(), N'QR-2024-002', '2024-01-15', N'قرار تخصيص مساعدات للأرامل والأيتام في بنغازي', N'معتمد'),
(NEWID(), N'QR-2024-003', '2024-02-01', N'قرار إنشاء مركز تحفيظ القرآن الكريم في مصراتة', N'قيد المراجعة'),
(NEWID(), N'QR-2024-004', '2024-02-10', N'قرار توزيع مصاحف على المساجد الجديدة', N'معتمد'),
(NEWID(), N'QR-2024-005', '2024-02-20', N'قرار إنشاء مكتبة إسلامية في سبها', N'معتمد');

PRINT N''
PRINT N'======================================='
PRINT N'تم بنجاح إدراج البيانات الليبية!'
PRINT N'Libyan data insertion completed successfully!'
PRINT N'======================================='

-- تقرير نهائي
SELECT 
    N'البنوك الليبية (Libyan Banks)' AS TableName,
    COUNT(*) AS RecordCount
FROM [Banks]
UNION ALL
SELECT 
    N'المدن الليبية (Libyan Cities)',
    COUNT(*)
FROM [Cities]
UNION ALL
SELECT 
    N'المناطق الليبية (Libyan Regions)',
    COUNT(*)
FROM [Regions]
UNION ALL
SELECT 
    N'المنتجات (Products)',
    COUNT(*)
FROM [Products]
UNION ALL
SELECT 
    N'المكاتب الليبية (Libyan Offices)',
    COUNT(*)
FROM [Offices]
UNION ALL
SELECT 
    N'المرافق الليبية (Libyan Facilities)',
    COUNT(*)
FROM [Facilities]
UNION ALL
SELECT 
    N'المباني الليبية (Libyan Buildings)',
    COUNT(*)
FROM [Buildings]
UNION ALL
SELECT 
    N'المساجد الليبية (Libyan Mosques)',
    COUNT(*)
FROM [Mosques]
UNION ALL
SELECT 
    N'الحسابات الليبية (Libyan Accounts)',
    COUNT(*)
FROM [Accounts]
UNION ALL
SELECT 
    N'القرارات (Decisions)',
    COUNT(*)
FROM [Decisions]
ORDER BY RecordCount DESC;

PRINT N''
PRINT N'العملية مكتملة! تم إدراج البيانات الليبية بنجاح'
PRINT N'Operation complete! Libyan data inserted successfully'
PRINT N'جميع المبالغ بالدينار الليبي'
PRINT N'All amounts in Libyan Dinar'

GO