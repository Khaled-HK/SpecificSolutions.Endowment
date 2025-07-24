-- Script آمن لحذف جميع البيانات من كافة الجداول
-- Database: Swagger_Endowment22
-- النسخة الآمنة مع إمكانية التراجع

USE [Swagger_Endowment22]
GO

-- =====================================================
-- الجزء الأول: التحقق من وجود البيانات
-- =====================================================

PRINT '=== تقرير حالة البيانات قبل الحذف ==='
PRINT 'Data Status Report Before Deletion'
PRINT '=================================='

SELECT 
    t.NAME AS TableName,
    i.rows AS RowCount
FROM 
    sys.tables t
INNER JOIN      
    sys.sysindexes i ON t.object_id = i.id 
WHERE 
    i.indid < 2  
    AND t.NAME NOT LIKE 'sys%'
    AND t.NAME NOT LIKE '__EFMigrations%'
    AND i.rows > 0
ORDER BY 
    i.rows DESC;

-- =====================================================
-- الجزء الثاني: إنشاء نسخة احتياطية من المعاملات (اختياري)
-- =====================================================

-- إذا كنت تريد إمكانية التراجع، قم بإلغاء التعليق عن الأسطر التالية
-- BEGIN TRANSACTION DeleteAllData;
-- PRINT 'Transaction started - يمكنك استخدام ROLLBACK للتراجع'

-- =====================================================
-- الجزء الثالث: حذف البيانات
-- =====================================================

BEGIN TRY
    PRINT 'بدء عملية حذف البيانات...'
    PRINT 'Starting data deletion process...'
    
    -- تعطيل فحص القيود مؤقتاً
    EXEC sp_msforeachtable "ALTER TABLE ? NOCHECK CONSTRAINT all"
    PRINT 'تم تعطيل فحص القيود مؤقتاً'
    
    -- حذف جداول Identity (AspNet) أولاً
    IF OBJECT_ID('AspNetUserRoles', 'U') IS NOT NULL
        DELETE FROM [AspNetUserRoles];
    IF OBJECT_ID('AspNetUserClaims', 'U') IS NOT NULL
        DELETE FROM [AspNetUserClaims];
    IF OBJECT_ID('AspNetUserLogins', 'U') IS NOT NULL
        DELETE FROM [AspNetUserLogins];
    IF OBJECT_ID('AspNetUserTokens', 'U') IS NOT NULL
        DELETE FROM [AspNetUserTokens];
    IF OBJECT_ID('AspNetRoleClaims', 'U') IS NOT NULL
        DELETE FROM [AspNetRoleClaims];
    IF OBJECT_ID('AspNetUsers', 'U') IS NOT NULL
        DELETE FROM [AspNetUsers];
    IF OBJECT_ID('AspNetRoles', 'U') IS NOT NULL
        DELETE FROM [AspNetRoles];
    PRINT 'تم حذف جداول الهوية (Identity Tables)'
    
    -- حذف جداول الطلبات
    IF OBJECT_ID('BuildingDetailRequests', 'U') IS NOT NULL
        DELETE FROM [BuildingDetailRequests];
    IF OBJECT_ID('ChangeOfPathRequests', 'U') IS NOT NULL
        DELETE FROM [ChangeOfPathRequests];
    IF OBJECT_ID('ConstructionRequests', 'U') IS NOT NULL
        DELETE FROM [ConstructionRequests];
    IF OBJECT_ID('DemolitionRequests', 'U') IS NOT NULL
        DELETE FROM [DemolitionRequests];
    IF OBJECT_ID('ExpenditureChangeRequests', 'U') IS NOT NULL
        DELETE FROM [ExpenditureChangeRequests];
    IF OBJECT_ID('MaintenanceRequests', 'U') IS NOT NULL
        DELETE FROM [MaintenanceRequests];
    IF OBJECT_ID('NameChangeRequests', 'U') IS NOT NULL
        DELETE FROM [NameChangeRequests];
    IF OBJECT_ID('NeedsRequests', 'U') IS NOT NULL
        DELETE FROM [NeedsRequests];
    IF OBJECT_ID('Requests', 'U') IS NOT NULL
        DELETE FROM [Requests];
    PRINT 'تم حذف جداول الطلبات'
    
    -- حذف جداول التفاصيل
    IF OBJECT_ID('AccountDetails', 'U') IS NOT NULL
        DELETE FROM [AccountDetails];
    IF OBJECT_ID('BuildingDetails', 'U') IS NOT NULL
        DELETE FROM [BuildingDetails];
    IF OBJECT_ID('FacilityDetails', 'U') IS NOT NULL
        DELETE FROM [FacilityDetails];
    PRINT 'تم حذف جداول التفاصيل'
    
    -- حذف جداول التدقيق
    IF OBJECT_ID('AuditLogs', 'U') IS NOT NULL
        DELETE FROM [AuditLogs];
    PRINT 'تم حذف جداول التدقيق'
    
    -- حذف الكيانات الرئيسية
    IF OBJECT_ID('Accounts', 'U') IS NOT NULL
        DELETE FROM [Accounts];
    IF OBJECT_ID('Buildings', 'U') IS NOT NULL
        DELETE FROM [Buildings];
    IF OBJECT_ID('Facilities', 'U') IS NOT NULL
        DELETE FROM [Facilities];
    IF OBJECT_ID('Mosques', 'U') IS NOT NULL
        DELETE FROM [Mosques];
    IF OBJECT_ID('QuranicSchools', 'U') IS NOT NULL
        DELETE FROM [QuranicSchools];
    IF OBJECT_ID('Offices', 'U') IS NOT NULL
        DELETE FROM [Offices];
    IF OBJECT_ID('Products', 'U') IS NOT NULL
        DELETE FROM [Products];
    IF OBJECT_ID('Decisions', 'U') IS NOT NULL
        DELETE FROM [Decisions];
    PRINT 'تم حذف الكيانات الرئيسية'
    
    -- حذف الجداول المرجعية
    IF OBJECT_ID('Branchs', 'U') IS NOT NULL
        DELETE FROM [Branchs];
    IF OBJECT_ID('Cities', 'U') IS NOT NULL
        DELETE FROM [Cities];
    IF OBJECT_ID('Regions', 'U') IS NOT NULL
        DELETE FROM [Regions];
    IF OBJECT_ID('Banks', 'U') IS NOT NULL
        DELETE FROM [Banks];
    PRINT 'تم حذف الجداول المرجعية'
    
    -- إعادة تفعيل فحص القيود
    EXEC sp_msforeachtable "ALTER TABLE ? WITH CHECK CHECK CONSTRAINT all"
    PRINT 'تم إعادة تفعيل فحص القيود'
    
    PRINT '=================================='
    PRINT 'تم حذف جميع البيانات بنجاح!'
    PRINT 'All data deleted successfully!'
    PRINT '=================================='
    
END TRY
BEGIN CATCH
    PRINT 'حدث خطأ أثناء حذف البيانات:'
    PRINT 'Error occurred during data deletion:'
    PRINT ERROR_MESSAGE()
    
    -- إعادة تفعيل القيود في حالة الخطأ
    EXEC sp_msforeachtable "ALTER TABLE ? WITH CHECK CHECK CONSTRAINT all"
    
    -- إذا كنت تستخدم Transaction، قم بإلغاء التعليق عن السطر التالي
    -- ROLLBACK TRANSACTION DeleteAllData;
    
END CATCH

-- =====================================================
-- الجزء الرابع: تقرير ما بعد الحذف
-- =====================================================

PRINT ''
PRINT '=== تقرير حالة البيانات بعد الحذف ==='
PRINT 'Data Status Report After Deletion'
PRINT '================================='

SELECT 
    t.NAME AS TableName,
    i.rows AS RowCount
FROM 
    sys.tables t
INNER JOIN      
    sys.sysindexes i ON t.object_id = i.id 
WHERE 
    i.indid < 2  
    AND t.NAME NOT LIKE 'sys%'
    AND t.NAME NOT LIKE '__EFMigrations%'
ORDER BY 
    i.rows DESC, t.NAME;

-- =====================================================
-- الجزء الخامس: إعادة تعيين Identity (اختياري)
-- =====================================================

PRINT ''
PRINT 'هل تريد إعادة تعيين Identity Seeds؟'
PRINT 'Do you want to reset Identity Seeds?'
PRINT 'قم بإلغاء التعليق عن الأسطر التالية إذا كنت تريد ذلك:'
PRINT 'Uncomment the following lines if you want to reset:'

/*
PRINT 'إعادة تعيين Identity Seeds...'
DBCC CHECKIDENT ('AccountDetails', RESEED, 0);
DBCC CHECKIDENT ('Accounts', RESEED, 0);
DBCC CHECKIDENT ('AuditLogs', RESEED, 0);
DBCC CHECKIDENT ('Banks', RESEED, 0);
DBCC CHECKIDENT ('Branchs', RESEED, 0);
DBCC CHECKIDENT ('BuildingDetailRequests', RESEED, 0);
DBCC CHECKIDENT ('BuildingDetails', RESEED, 0);
DBCC CHECKIDENT ('Buildings', RESEED, 0);
DBCC CHECKIDENT ('ChangeOfPathRequests', RESEED, 0);
DBCC CHECKIDENT ('Cities', RESEED, 0);
DBCC CHECKIDENT ('ConstructionRequests', RESEED, 0);
DBCC CHECKIDENT ('Decisions', RESEED, 0);
DBCC CHECKIDENT ('DemolitionRequests', RESEED, 0);
DBCC CHECKIDENT ('ExpenditureChangeRequests', RESEED, 0);
DBCC CHECKIDENT ('Facilities', RESEED, 0);
DBCC CHECKIDENT ('FacilityDetails', RESEED, 0);
DBCC CHECKIDENT ('MaintenanceRequests', RESEED, 0);
DBCC CHECKIDENT ('Mosques', RESEED, 0);
DBCC CHECKIDENT ('NameChangeRequests', RESEED, 0);
DBCC CHECKIDENT ('NeedsRequests', RESEED, 0);
DBCC CHECKIDENT ('Offices', RESEED, 0);
DBCC CHECKIDENT ('Products', RESEED, 0);
DBCC CHECKIDENT ('QuranicSchools', RESEED, 0);
DBCC CHECKIDENT ('Regions', RESEED, 0);
DBCC CHECKIDENT ('Requests', RESEED, 0);
PRINT 'تم إعادة تعيين Identity Seeds'
*/

-- إذا كنت تستخدم Transaction، قم بإلغاء التعليق عن السطر التالي لتأكيد العملية
-- COMMIT TRANSACTION DeleteAllData;

PRINT ''
PRINT 'انتهت العملية بنجاح!'
PRINT 'Operation completed successfully!'

GO