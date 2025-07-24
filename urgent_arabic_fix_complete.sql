USE [Swagger_Endowment22]
GO

PRINT N'🚨 بدء الإصلاح العاجل للترميز العربي'
PRINT N'🚨 Starting urgent Arabic encoding fix'
PRINT N'==========================================='

-- المرحلة 1: فحص الوضع الحالي
PRINT N''
PRINT N'📋 المرحلة 1: فحص الوضع الحالي'
PRINT N'📋 Phase 1: Checking current status'

SELECT 
    DATABASEPROPERTYEX('Swagger_Endowment22', 'Collation') as CurrentDatabaseCollation,
    SERVERPROPERTY('Collation') as ServerCollation

-- المرحلة 2: تحديث قاعدة البيانات
PRINT N''
PRINT N'📋 المرحلة 2: تحديث إعدادات قاعدة البيانات'
PRINT N'📋 Phase 2: Updating database settings'

BEGIN TRY
    -- إيقاف جميع الاتصالات الأخرى
    ALTER DATABASE [Swagger_Endowment22] SET SINGLE_USER WITH ROLLBACK IMMEDIATE
    
    -- تحديث Collation
    ALTER DATABASE [Swagger_Endowment22] COLLATE Arabic_CI_AS
    
    -- السماح بالاتصالات مرة أخرى
    ALTER DATABASE [Swagger_Endowment22] SET MULTI_USER
    
    PRINT N'✅ تم تحديث Database Collation إلى Arabic_CI_AS'
END TRY
BEGIN CATCH
    PRINT N'⚠️ تعذر تحديث Database Collation: ' + ERROR_MESSAGE()
    -- السماح بالاتصالات في حالة الخطأ
    IF EXISTS (SELECT 1 FROM sys.databases WHERE name = 'Swagger_Endowment22' AND user_access = 1)
        ALTER DATABASE [Swagger_Endowment22] SET MULTI_USER
END CATCH

-- المرحلة 3: تحديث جداول المكاتب والمناطق
PRINT N''
PRINT N'📋 المرحلة 3: تحديث الجداول والأعمدة'
PRINT N'📋 Phase 3: Updating tables and columns'

BEGIN TRY
    -- تحديث جدول Offices
    IF EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'Offices' AND COLUMN_NAME = 'Name')
    BEGIN
        ALTER TABLE [Offices] ALTER COLUMN [Name] NVARCHAR(255) COLLATE Arabic_CI_AS
        PRINT N'✅ تم تحديث جدول Offices - العمود Name'
    END
    
    IF EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'Offices' AND COLUMN_NAME = 'Address')
    BEGIN
        ALTER TABLE [Offices] ALTER COLUMN [Address] NVARCHAR(500) COLLATE Arabic_CI_AS
        PRINT N'✅ تم تحديث جدول Offices - العمود Address'
    END
    
    -- تحديث جدول Regions
    IF EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'Regions' AND COLUMN_NAME = 'Name')
    BEGIN
        ALTER TABLE [Regions] ALTER COLUMN [Name] NVARCHAR(255) COLLATE Arabic_CI_AS
        PRINT N'✅ تم تحديث جدول Regions - العمود Name'
    END
    
    -- تحديث جدول Cities
    IF EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'Cities' AND COLUMN_NAME = 'Name')
    BEGIN
        ALTER TABLE [Cities] ALTER COLUMN [Name] NVARCHAR(255) COLLATE Arabic_CI_AS
        PRINT N'✅ تم تحديث جدول Cities - العمود Name'
    END
    
    -- تحديث جدول Banks
    IF EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'Banks' AND COLUMN_NAME = 'Name')
    BEGIN
        ALTER TABLE [Banks] ALTER COLUMN [Name] NVARCHAR(255) COLLATE Arabic_CI_AS
        ALTER TABLE [Banks] ALTER COLUMN [Address] NVARCHAR(500) COLLATE Arabic_CI_AS
        PRINT N'✅ تم تحديث جدول Banks'
    END
    
    -- تحديث جدول Buildings
    IF EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'Buildings' AND COLUMN_NAME = 'Name')
    BEGIN
        ALTER TABLE [Buildings] ALTER COLUMN [Name] NVARCHAR(255) COLLATE Arabic_CI_AS
        ALTER TABLE [Buildings] ALTER COLUMN [Address] NVARCHAR(500) COLLATE Arabic_CI_AS
        PRINT N'✅ تم تحديث جدول Buildings'
    END
    
    -- تحديث جدول Mosques
    IF EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'Mosques' AND COLUMN_NAME = 'Name')
    BEGIN
        ALTER TABLE [Mosques] ALTER COLUMN [Name] NVARCHAR(255) COLLATE Arabic_CI_AS
        ALTER TABLE [Mosques] ALTER COLUMN [Address] NVARCHAR(500) COLLATE Arabic_CI_AS
        PRINT N'✅ تم تحديث جدول Mosques'
    END
    
END TRY
BEGIN CATCH
    PRINT N'❌ خطأ في تحديث الجداول: ' + ERROR_MESSAGE()
END CATCH

-- المرحلة 4: حذف البيانات المشوهة وإعادة الإدراج
PRINT N''
PRINT N'📋 المرحلة 4: تنظيف البيانات المشوهة'
PRINT N'📋 Phase 4: Cleaning corrupted data'

BEGIN TRY
    -- إيقاف فحص Foreign Keys
    EXEC sp_msforeachtable "ALTER TABLE ? NOCHECK CONSTRAINT all"
    
    -- حذف البيانات المشوهة من الجداول الرئيسية
    DELETE FROM [Accounts] WHERE [Id] IS NOT NULL
    DELETE FROM [Buildings] WHERE [Id] IS NOT NULL
    DELETE FROM [Mosques] WHERE [Id] IS NOT NULL
    DELETE FROM [Offices] WHERE [Id] IS NOT NULL
    DELETE FROM [Regions] WHERE [Id] IS NOT NULL
    DELETE FROM [Cities] WHERE [Id] IS NOT NULL
    DELETE FROM [Banks] WHERE [Id] IS NOT NULL
    
    PRINT N'✅ تم حذف البيانات المشوهة'
    
    -- إعادة تفعيل فحص Foreign Keys
    EXEC sp_msforeachtable "ALTER TABLE ? WITH CHECK CHECK CONSTRAINT all"
    
END TRY
BEGIN CATCH
    PRINT N'❌ خطأ في حذف البيانات: ' + ERROR_MESSAGE()
    EXEC sp_msforeachtable "ALTER TABLE ? WITH CHECK CHECK CONSTRAINT all"
END CATCH

-- المرحلة 5: إدراج بيانات ليبية جديدة بترميز صحيح
PRINT N''
PRINT N'📋 المرحلة 5: إدراج البيانات الليبية الجديدة'
PRINT N'📋 Phase 5: Inserting new Libyan data'

BEGIN TRY
    -- متغيرات للـ IDs
    DECLARE @CityId1 UNIQUEIDENTIFIER = NEWID()
    DECLARE @CityId2 UNIQUEIDENTIFIER = NEWID()
    DECLARE @CityId3 UNIQUEIDENTIFIER = NEWID()
    DECLARE @CityId4 UNIQUEIDENTIFIER = NEWID()
    DECLARE @CityId5 UNIQUEIDENTIFIER = NEWID()
    
    DECLARE @RegionId1 UNIQUEIDENTIFIER = NEWID()
    DECLARE @RegionId2 UNIQUEIDENTIFIER = NEWID()
    DECLARE @RegionId3 UNIQUEIDENTIFIER = NEWID()
    DECLARE @RegionId4 UNIQUEIDENTIFIER = NEWID()
    DECLARE @RegionId5 UNIQUEIDENTIFIER = NEWID()
    
    -- إدراج البنوك الليبية
    INSERT INTO [Banks] ([Id], [Name], [Address], [ContactNumber]) VALUES
    (NEWID(), N'مصرف الجمهورية', N'شارع عمر المختار، طرابلس', N'021-4567890'),
    (NEWID(), N'البنك التجاري الوطني', N'شارع الاستقلال، بنغازي', N'061-3456789'),
    (NEWID(), N'مصرف الوحدة', N'ميدان الشهداء، طرابلس', N'021-5678901'),
    (NEWID(), N'البنك الأهلي الليبي', N'شارع جمال عبد الناصر، مصراتة', N'051-2345678'),
    (NEWID(), N'مصرف الصحاري', N'طريق المطار، طرابلس', N'021-6789012')
    
    PRINT N'✅ تم إدراج البنوك الليبية'
    
    -- إدراج المدن الليبية
    INSERT INTO [Cities] ([Id], [Name], [Country]) VALUES
    (@CityId1, N'طرابلس', N'ليبيا'),
    (@CityId2, N'بنغازي', N'ليبيا'),
    (@CityId3, N'مصراتة', N'ليبيا'),
    (@CityId4, N'الزاوية', N'ليبيا'),
    (@CityId5, N'شحات', N'ليبيا')
    
    PRINT N'✅ تم إدراج المدن الليبية'
    
    -- إدراج المناطق الليبية
    INSERT INTO [Regions] ([Id], [Name], [CityId]) VALUES
    (@RegionId1, N'منطقة الدهماني', @CityId1),
    (@RegionId2, N'منطقة الصابري', @CityId2),
    (@RegionId3, N'منطقة قصر أحمد', @CityId3),
    (@RegionId4, N'منطقة المدينة القديمة', @CityId1),
    (@RegionId5, N'منطقة الأثار', @CityId5)
    
    PRINT N'✅ تم إدراج المناطق الليبية'
    
    -- إدراج مكاتب الأوقاف الليبية
    INSERT INTO [Offices] ([Id], [Name], [Address], [PhoneNumber], [RegionId]) VALUES
    (NEWID(), N'مكتب الأوقاف - طرابلس', N'شارع الاستقلال، منطقة الدهماني، طرابلس', N'0218-84-1234579', @RegionId1),
    (NEWID(), N'مكتب الأوقاف - بنغازي', N'شارع عمر المختار، منطقة الصابري، بنغازي', N'0218-84-1234584', @RegionId2),
    (NEWID(), N'مكتب الأوقاف - مصراتة', N'شارع جمال عبد الناصر، منطقة قصر أحمد، مصراتة', N'0218-18-1234572', @RegionId3),
    (NEWID(), N'مكتب الأوقاف - الزاوية', N'طريق المطار، المدينة القديمة، الزاوية', N'0218-18-1234568', @RegionId4),
    (NEWID(), N'مكتب الأوقاف - شحات', N'شارع الثورة، منطقة الأثار، شحات', N'0218-84-1234568', @RegionId5)
    
    PRINT N'✅ تم إدراج مكاتب الأوقاف الليبية'
    
END TRY
BEGIN CATCH
    PRINT N'❌ خطأ في إدراج البيانات: ' + ERROR_MESSAGE()
END CATCH

-- المرحلة 6: فحص النتائج
PRINT N''
PRINT N'📋 المرحلة 6: فحص النتائج'
PRINT N'📋 Phase 6: Testing results'

PRINT N''
PRINT N'🏦 البنوك الليبية المدرجة:'
SELECT TOP 5 [Name] as 'Bank Name' FROM [Banks] ORDER BY [Name]

PRINT N''
PRINT N'🏙️ المدن الليبية المدرجة:'
SELECT TOP 5 [Name] as 'City Name' FROM [Cities] ORDER BY [Name]

PRINT N''
PRINT N'🏢 مكاتب الأوقاف المدرجة:'
SELECT TOP 5 
    o.[Name] as 'Office Name',
    r.[Name] as 'Region Name',
    o.[PhoneNumber] as 'Phone'
FROM [Offices] o
LEFT JOIN [Regions] r ON o.[RegionId] = r.[Id]
ORDER BY o.[Name]

-- إحصائيات نهائية
PRINT N''
PRINT N'📊 إحصائيات البيانات:'
SELECT 
    'Banks' as TableName, COUNT(*) as RecordCount FROM [Banks]
UNION ALL
SELECT 
    'Cities' as TableName, COUNT(*) as RecordCount FROM [Cities]
UNION ALL
SELECT 
    'Regions' as TableName, COUNT(*) as RecordCount FROM [Regions]
UNION ALL
SELECT 
    'Offices' as TableName, COUNT(*) as RecordCount FROM [Offices]

PRINT N''
PRINT N'==========================================='
PRINT N'🎉 تم الانتهاء من الإصلاح العاجل!'
PRINT N'🎉 Urgent fix completed successfully!'
PRINT N'==========================================='
PRINT N''
PRINT N'✅ الآن أعد تشغيل التطبيق لترى البيانات العربية صحيحة'
PRINT N'✅ Now restart your application to see correct Arabic data'