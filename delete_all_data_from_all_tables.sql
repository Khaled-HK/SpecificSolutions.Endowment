-- Script لحذف جميع البيانات من كافة الجداول في قاعدة البيانات
-- Database: Swagger_Endowment22
-- تم إنشاؤه بناءً على كيانات المشروع

USE [Swagger_Endowment22]
GO

-- تعطيل فحص القيود (Foreign Key Constraints) مؤقتاً
EXEC sp_msforeachtable "ALTER TABLE ? NOCHECK CONSTRAINT all"
GO

-- حذف البيانات من الجداول الفرعية أولاً (التي تحتوي على Foreign Keys)

-- جداول الطلبات والتفاصيل
DELETE FROM [BuildingDetailRequests];
DELETE FROM [ChangeOfPathRequests];
DELETE FROM [ConstructionRequests];
DELETE FROM [DemolitionRequests];
DELETE FROM [ExpenditureChangeRequests];
DELETE FROM [MaintenanceRequests];
DELETE FROM [NameChangeRequests];
DELETE FROM [NeedsRequests];
DELETE FROM [Requests];

-- جداول التفاصيل
DELETE FROM [AccountDetails];
DELETE FROM [BuildingDetails];
DELETE FROM [FacilityDetails];

-- جداول الأدوار والمستخدمين
DELETE FROM [AspNetUserRoles];
DELETE FROM [AspNetUserClaims];
DELETE FROM [AspNetUserLogins];
DELETE FROM [AspNetUserTokens];
DELETE FROM [AspNetRoleClaims];
DELETE FROM [AspNetUsers];
DELETE FROM [AspNetRoles];

-- جداول التدقيق
DELETE FROM [AuditLogs];

-- جداول الكيانات الرئيسية
DELETE FROM [Accounts];
DELETE FROM [Buildings];
DELETE FROM [Facilities];
DELETE FROM [Mosques];
DELETE FROM [QuranicSchools];
DELETE FROM [Offices];
DELETE FROM [Products];

-- جداول القرارات
DELETE FROM [Decisions];

-- جداول المراجع الأساسية
DELETE FROM [Branchs];
DELETE FROM [Cities];
DELETE FROM [Regions];
DELETE FROM [Banks];

-- إعادة تعيين Identity Seeds للجداول (اختياري)
-- إذا كنت تريد إعادة تعيين الـ Identity إلى 1

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

-- إعادة تفعيل فحص القيود
EXEC sp_msforeachtable "ALTER TABLE ? WITH CHECK CHECK CONSTRAINT all"
GO

-- عرض رسالة تأكيد
PRINT 'تم حذف جميع البيانات من كافة الجداول بنجاح!'
PRINT 'Data deletion completed successfully for all tables!'

-- (اختياري) عرض عدد الصفوف في كل جدول للتأكد من الحذف
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

GO