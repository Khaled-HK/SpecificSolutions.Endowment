-- إصلاح مشاكل الترميز العربي في قاعدة البيانات
-- Fix Arabic Encoding Issues

USE [Swagger_Endowment22]
GO

PRINT N'بدء إصلاح مشاكل الترميز العربي...'
PRINT 'Starting Arabic encoding fix...'

-- ======================================
-- الجزء الأول: فحص إعدادات قاعدة البيانات
-- ======================================

PRINT N'فحص إعدادات Collation الحالية...'
SELECT 
    name as DatabaseName,
    collation_name as CurrentCollation
FROM sys.databases 
WHERE name = 'Swagger_Endowment22'

-- ======================================
-- الجزء الثاني: تغيير Collation لدعم العربية
-- ======================================

PRINT N'تحديث إعدادات قاعدة البيانات لدعم العربية...'

-- تغيير collation قاعدة البيانات إلى Arabic_CI_AS
ALTER DATABASE [Swagger_Endowment22] 
COLLATE Arabic_CI_AS

PRINT N'تم تحديث Collation إلى Arabic_CI_AS'

-- ======================================
-- الجزء الثالث: إصلاح الجداول الموجودة
-- ======================================

PRINT N'تحديث إعدادات الأعمدة النصية...'

-- تحديث جدول Banks
ALTER TABLE [Banks] ALTER COLUMN [Name] NVARCHAR(255) COLLATE Arabic_CI_AS
ALTER TABLE [Banks] ALTER COLUMN [Address] NVARCHAR(500) COLLATE Arabic_CI_AS
ALTER TABLE [Banks] ALTER COLUMN [ContactNumber] NVARCHAR(50) COLLATE Arabic_CI_AS

-- تحديث جدول Cities
ALTER TABLE [Cities] ALTER COLUMN [Name] NVARCHAR(255) COLLATE Arabic_CI_AS
ALTER TABLE [Cities] ALTER COLUMN [Country] NVARCHAR(100) COLLATE Arabic_CI_AS

-- تحديث جدول Regions
ALTER TABLE [Regions] ALTER COLUMN [Name] NVARCHAR(255) COLLATE Arabic_CI_AS
ALTER TABLE [Regions] ALTER COLUMN [Country] NVARCHAR(100) COLLATE Arabic_CI_AS

-- تحديث جدول Products
ALTER TABLE [Products] ALTER COLUMN [Name] NVARCHAR(255) COLLATE Arabic_CI_AS
ALTER TABLE [Products] ALTER COLUMN [Description] NVARCHAR(1000) COLLATE Arabic_CI_AS

-- تحديث جدول Offices
ALTER TABLE [Offices] ALTER COLUMN [Name] NVARCHAR(255) COLLATE Arabic_CI_AS
ALTER TABLE [Offices] ALTER COLUMN [Location] NVARCHAR(500) COLLATE Arabic_CI_AS
ALTER TABLE [Offices] ALTER COLUMN [PhoneNumber] NVARCHAR(50) COLLATE Arabic_CI_AS

-- تحديث جدول Facilities
ALTER TABLE [Facilities] ALTER COLUMN [Name] NVARCHAR(255) COLLATE Arabic_CI_AS
ALTER TABLE [Facilities] ALTER COLUMN [Location] NVARCHAR(500) COLLATE Arabic_CI_AS
ALTER TABLE [Facilities] ALTER COLUMN [ContactInfo] NVARCHAR(255) COLLATE Arabic_CI_AS
ALTER TABLE [Facilities] ALTER COLUMN [Status] NVARCHAR(50) COLLATE Arabic_CI_AS

-- تحديث جدول Buildings
ALTER TABLE [Buildings] ALTER COLUMN [Name] NVARCHAR(255) COLLATE Arabic_CI_AS
ALTER TABLE [Buildings] ALTER COLUMN [Definition] NVARCHAR(255) COLLATE Arabic_CI_AS
ALTER TABLE [Buildings] ALTER COLUMN [Classification] NVARCHAR(255) COLLATE Arabic_CI_AS
ALTER TABLE [Buildings] ALTER COLUMN [Unit] NVARCHAR(255) COLLATE Arabic_CI_AS
ALTER TABLE [Buildings] ALTER COLUMN [NearestLandmark] NVARCHAR(500) COLLATE Arabic_CI_AS
ALTER TABLE [Buildings] ALTER COLUMN [BriefDescription] NVARCHAR(1000) COLLATE Arabic_CI_AS
ALTER TABLE [Buildings] ALTER COLUMN [LandDonorName] NVARCHAR(255) COLLATE Arabic_CI_AS
ALTER TABLE [Buildings] ALTER COLUMN [PrayerCapacity] NVARCHAR(100) COLLATE Arabic_CI_AS

-- تحديث جدول Accounts
ALTER TABLE [Accounts] ALTER COLUMN [Name] NVARCHAR(255) COLLATE Arabic_CI_AS
ALTER TABLE [Accounts] ALTER COLUMN [MotherName] NVARCHAR(255) COLLATE Arabic_CI_AS
ALTER TABLE [Accounts] ALTER COLUMN [Note] NVARCHAR(1000) COLLATE Arabic_CI_AS
ALTER TABLE [Accounts] ALTER COLUMN [Address] NVARCHAR(500) COLLATE Arabic_CI_AS
ALTER TABLE [Accounts] ALTER COLUMN [City] NVARCHAR(100) COLLATE Arabic_CI_AS
ALTER TABLE [Accounts] ALTER COLUMN [Country] NVARCHAR(100) COLLATE Arabic_CI_AS

PRINT N'تم تحديث إعدادات الجداول بنجاح'

-- ======================================
-- الجزء الرابع: إعادة إدراج البيانات الليبية بترميز صحيح
-- ======================================

PRINT N'إعادة إدراج البيانات الليبية بترميز صحيح...'

-- حذف البيانات الموجودة
DELETE FROM [Accounts]
DELETE FROM [Mosques]
DELETE FROM [Buildings]
DELETE FROM [Facilities]
DELETE FROM [Decisions]
DELETE FROM [Offices]
DELETE FROM [Products]
DELETE FROM [Regions]
DELETE FROM [Cities]
DELETE FROM [Banks]

-- إعادة إدراج البنوك الليبية
INSERT INTO [Banks] ([Id], [Name], [Address], [ContactNumber]) VALUES
(NEWID(), N'مصرف الجمهورية', N'شارع عمر المختار، طرابلس', N'021-4567890'),
(NEWID(), N'البنك التجاري الوطني', N'شارع الاستقلال، بنغازي', N'061-3456789'),
(NEWID(), N'مصرف الوحدة', N'شارع الجلاء، طرابلس', N'021-2345678'),
(NEWID(), N'البنك الأهلي الليبي', N'شارع جمال عبد الناصر، بنغازي', N'061-4567890'),
(NEWID(), N'مصرف الصحاري', N'طريق المطار، طرابلس', N'021-5678901');

-- إعادة إدراج المدن الليبية
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
(@CityId5, N'شحات', N'ليبيا');

-- إعادة إدراج المناطق الليبية
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
(@RegionId5, N'منطقة الأثار', N'ليبيا', @CityId5);

-- إعادة إدراج مكاتب الأوقاف
DECLARE @OfficeId1 UNIQUEIDENTIFIER = NEWID()
DECLARE @OfficeId2 UNIQUEIDENTIFIER = NEWID()
DECLARE @OfficeId3 UNIQUEIDENTIFIER = NEWID()
DECLARE @OfficeId4 UNIQUEIDENTIFIER = NEWID()
DECLARE @OfficeId5 UNIQUEIDENTIFIER = NEWID()

INSERT INTO [Offices] ([Id], [Name], [Location], [PhoneNumber], [RegionId], [UserId]) VALUES
(@OfficeId1, N'مكتب الأوقاف - طرابلس', N'شارع عمر المختار، الدهماني', N'021-4567890', @RegionId1, NEWID()),
(@OfficeId2, N'مكتب الأوقاف - بنغازي', N'شارع الاستقلال، الصابري', N'061-3456789', @RegionId2, NEWID()),
(@OfficeId3, N'مكتب الأوقاف - مصراتة', N'شارع الشهداء، قصر أحمد', N'051-2345678', @RegionId3, NEWID()),
(@OfficeId4, N'مكتب الأوقاف - الزاوية', N'شارع الجمهورية، المدينة القديمة', N'023-5678901', @RegionId4, NEWID()),
(@OfficeId5, N'مكتب الأوقاف - شحات', N'منطقة الأثار، شحات', N'081-7890123', @RegionId5, NEWID());

PRINT N'تم إعادة إدراج البيانات بترميز صحيح'

-- ======================================
-- الجزء الخامس: اختبار النتائج
-- ======================================

PRINT N'اختبار عرض البيانات العربية...'

SELECT TOP 5 
    N'البنوك الليبية' as TableType,
    Name as ArabicName,
    Address as ArabicAddress
FROM [Banks]

UNION ALL

SELECT TOP 5
    N'المدن الليبية' as TableType,
    Name as ArabicName,
    Country as ArabicAddress
FROM [Cities]

UNION ALL

SELECT TOP 5
    N'مكاتب الأوقاف' as TableType,
    Name as ArabicName,
    Location as ArabicAddress
FROM [Offices]

PRINT N''
PRINT N'=================================================='
PRINT N'تم إصلاح مشاكل الترميز العربي بنجاح!'
PRINT N'Arabic encoding issues fixed successfully!'
PRINT N'=================================================='
PRINT N''
PRINT N'الآن يجب أن تظهر النصوص العربية بشكل صحيح'
PRINT N'Arabic text should now display correctly'

GO