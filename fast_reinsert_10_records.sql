-- ملف إعادة إدخال 10 سجلات في كل جدول بسرعة
-- Fast Reinsert 10 Records in All Tables

SET QUOTED_IDENTIFIER ON;
SET ANSI_NULLS ON;

PRINT 'بدء حذف البيانات الموجودة...'

-- حذف البيانات الموجودة من الجداول (بترتيب عكسي لتجنب مشاكل Foreign Key)
BEGIN TRY
    DELETE FROM Decisions
    PRINT 'تم حذف بيانات القرارات!'
END TRY
BEGIN CATCH
    PRINT 'تحذير في حذف بيانات القرارات: ' + ERROR_MESSAGE()
END CATCH

BEGIN TRY
    DELETE FROM Banks
    PRINT 'تم حذف بيانات البنوك!'
END TRY
BEGIN CATCH
    PRINT 'تحذير في حذف بيانات البنوك: ' + ERROR_MESSAGE()
END CATCH

BEGIN TRY
    DELETE FROM Products
    PRINT 'تم حذف بيانات المنتجات!'
END TRY
BEGIN CATCH
    PRINT 'تحذير في حذف بيانات المنتجات: ' + ERROR_MESSAGE()
END CATCH

BEGIN TRY
    DELETE FROM Regions
    PRINT 'تم حذف بيانات المناطق!'
END TRY
BEGIN CATCH
    PRINT 'تحذير في حذف بيانات المناطق: ' + ERROR_MESSAGE()
END CATCH

BEGIN TRY
    DELETE FROM Cities
    PRINT 'تم حذف بيانات المدن!'
END TRY
BEGIN CATCH
    PRINT 'تحذير في حذف بيانات المدن: ' + ERROR_MESSAGE()
END CATCH

PRINT 'تم حذف جميع البيانات!'
PRINT 'بدء إدخال 10 سجلات في كل جدول...'

-- إدخال 10 مدن
PRINT 'إدخال 10 مدن...'
BEGIN TRY
    INSERT INTO Cities (Id, Name, Country) VALUES
    (NEWID(), N'طرابلس', N'ليبيا'),
    (NEWID(), N'بنغازي', N'ليبيا'),
    (NEWID(), N'مصراتة', N'ليبيا'),
    (NEWID(), N'الزاوية', N'ليبيا'),
    (NEWID(), N'البيضاء', N'ليبيا'),
    (NEWID(), N'سرت', N'ليبيا'),
    (NEWID(), N'طبرق', N'ليبيا'),
    (NEWID(), N'الخمس', N'ليبيا'),
    (NEWID(), N'زليتن', N'ليبيا'),
    (NEWID(), N'أجدابيا', N'ليبيا')
    
    PRINT 'تم إدخال 10 مدن!'
END TRY
BEGIN CATCH
    PRINT 'خطأ في إدخال المدن: ' + ERROR_MESSAGE()
END CATCH

-- إدخال 10 مناطق
PRINT 'إدخال 10 مناطق...'
BEGIN TRY
    DECLARE @TripoliId uniqueidentifier = (SELECT TOP 1 Id FROM Cities WHERE Name = N'طرابلس')
    DECLARE @BenghaziId uniqueidentifier = (SELECT TOP 1 Id FROM Cities WHERE Name = N'بنغازي')

    INSERT INTO Regions (Id, Name, Country, CityId) VALUES
    (NEWID(), N'وسط طرابلس', N'ليبيا', @TripoliId),
    (NEWID(), N'حي الأندلس', N'ليبيا', @TripoliId),
    (NEWID(), N'حي النصر', N'ليبيا', @TripoliId),
    (NEWID(), N'حي الهضبة', N'ليبيا', @TripoliId),
    (NEWID(), N'حي سيدي المصري', N'ليبيا', @TripoliId),
    (NEWID(), N'وسط بنغازي', N'ليبيا', @BenghaziId),
    (NEWID(), N'حي الصابري', N'ليبيا', @BenghaziId),
    (NEWID(), N'حي سيدي خريبيش', N'ليبيا', @BenghaziId),
    (NEWID(), N'حي الكويفية', N'ليبيا', @BenghaziId),
    (NEWID(), N'حي البركة', N'ليبيا', @BenghaziId)
    
    PRINT 'تم إدخال 10 مناطق!'
END TRY
BEGIN CATCH
    PRINT 'خطأ في إدخال المناطق: ' + ERROR_MESSAGE()
END CATCH

-- إدخال 10 منتجات
PRINT 'إدخال 10 منتجات...'
BEGIN TRY
    INSERT INTO Products (Id, Name, Description) VALUES
    (NEWID(), N'أسمنت بورتلاند', N'أسمنت عادي للبناء والإنشاءات'),
    (NEWID(), N'حديد تسليح', N'حديد تسليح للخرسانة المسلحة'),
    (NEWID(), N'طوب أحمر', N'طوب طيني أحمر للبناء'),
    (NEWID(), N'طوب إسمنتي', N'طوب إسمنتي خفيف الوزن'),
    (NEWID(), N'رمل ناعم', N'رمل ناعم للخرسانة واللياسة'),
    (NEWID(), N'كابلات كهربائية', N'كابلات نحاسية للتمديدات الكهربائية'),
    (NEWID(), N'مواسير PVC', N'مواسير بلاستيكية للمياه والصرف'),
    (NEWID(), N'طاولات مكتب', N'طاولات مكتب خشبية ومعدنية'),
    (NEWID(), N'أقفال أبواب', N'أقفال أبواب أمنية'),
    (NEWID(), N'منظفات أرضيات', N'منظفات أرضيات متنوعة')
    
    PRINT 'تم إدخال 10 منتجات!'
END TRY
BEGIN CATCH
    PRINT 'خطأ في إدخال المنتجات: ' + ERROR_MESSAGE()
END CATCH

-- إدخال 10 بنوك
PRINT 'إدخال 10 بنوك...'
BEGIN TRY
    INSERT INTO Banks (Id, Name, Address, ContactNumber) VALUES
    (NEWID(), N'البنك المركزي الليبي', N'طرابلس - ليبيا', '0218-91-1234567'),
    (NEWID(), N'بنك ليبيا المركزي', N'طرابلس - ليبيا', '0218-91-1234568'),
    (NEWID(), N'البنك التجاري الوطني', N'طرابلس - ليبيا', '0218-91-1234569'),
    (NEWID(), N'بنك الوحدة', N'طرابلس - ليبيا', '0218-91-1234570'),
    (NEWID(), N'بنك الجمهورية', N'طرابلس - ليبيا', '0218-91-1234571'),
    (NEWID(), N'البنك الأهلي السعودي', N'الرياض - السعودية', '00966-11-1234567'),
    (NEWID(), N'بنك الراجحي', N'الرياض - السعودية', '00966-11-1234568'),
    (NEWID(), N'بنك الإمارات دبي الوطني', N'دبي - الإمارات', '00971-4-1234567'),
    (NEWID(), N'البنك الأهلي المصري', N'القاهرة - مصر', '0020-2-1234567'),
    (NEWID(), N'بنك باركليز', N'لندن - بريطانيا', '0044-20-1234567')
    
    PRINT 'تم إدخال 10 بنوك!'
END TRY
BEGIN CATCH
    PRINT 'خطأ في إدخال البنوك: ' + ERROR_MESSAGE()
END CATCH

-- إدخال 10 قرارات
PRINT 'إدخال 10 قرارات...'
BEGIN TRY
    DECLARE @AdminUserId nvarchar(450) = (SELECT TOP 1 Id FROM AspNetUsers WHERE UserName = 'admin' OR Email = 'admin@example.com')

    IF @AdminUserId IS NULL
    BEGIN
        SET @AdminUserId = '00000000-0000-0000-0000-000000000000'
        PRINT 'تحذير: لم يتم العثور على مستخدم admin، سيتم استخدام ID افتراضي'
    END

    INSERT INTO Decisions (Id, Title, Description, CreatedDate, ReferenceNumber, UserId) VALUES
    (NEWID(), N'قرار تعيين مدير عام', N'قرار بتعيين السيد أحمد محمد علي مديراً عاماً للمؤسسة', DATEADD(day, -365, GETDATE()), 'DEC-2024-001', @AdminUserId),
    (NEWID(), N'قرار إنشاء فرع جديد', N'قرار بإنشاء فرع جديد في مدينة بنغازي', DATEADD(day, -350, GETDATE()), 'DEC-2024-002', @AdminUserId),
    (NEWID(), N'قرار شراء معدات', N'قرار بشراء معدات مكتبية جديدة', DATEADD(day, -340, GETDATE()), 'DEC-2024-003', @AdminUserId),
    (NEWID(), N'قرار تطوير النظام', N'قرار بتطوير النظام الإلكتروني للمؤسسة', DATEADD(day, -330, GETDATE()), 'DEC-2024-004', @AdminUserId),
    (NEWID(), N'قرار تدريب الموظفين', N'قرار بتنظيم دورات تدريبية للموظفين', DATEADD(day, -320, GETDATE()), 'DEC-2024-005', @AdminUserId),
    (NEWID(), N'قرار الميزانية السنوية', N'قرار بإنشاء الميزانية السنوية لعام 2024', DATEADD(day, -310, GETDATE()), 'DEC-2024-006', @AdminUserId),
    (NEWID(), N'قرار زيادة الرواتب', N'قرار بزيادة رواتب الموظفين بنسبة 10%', DATEADD(day, -300, GETDATE()), 'DEC-2024-007', @AdminUserId),
    (NEWID(), N'قرار شراء عقار', N'قرار بشراء عقار جديد للمؤسسة', DATEADD(day, -290, GETDATE()), 'DEC-2024-008', @AdminUserId),
    (NEWID(), N'قرار استثمار أموال', N'قرار باستثمار الأموال في مشاريع مربحة', DATEADD(day, -280, GETDATE()), 'DEC-2024-009', @AdminUserId),
    (NEWID(), N'قرار إعادة هيكلة مالية', N'قرار بإعادة هيكلة الوضع المالي للمؤسسة', DATEADD(day, -270, GETDATE()), 'DEC-2024-010', @AdminUserId)
    
    PRINT 'تم إدخال 10 قرارات!'
END TRY
BEGIN CATCH
    PRINT 'خطأ في إدخال القرارات: ' + ERROR_MESSAGE()
END CATCH

-- عرض إحصائيات البيانات المدخلة
PRINT '=== إحصائيات البيانات المدخلة ==='
DECLARE @CitiesCount int = (SELECT COUNT(*) FROM Cities)
DECLARE @RegionsCount int = (SELECT COUNT(*) FROM Regions)
DECLARE @ProductsCount int = (SELECT COUNT(*) FROM Products)
DECLARE @BanksCount int = (SELECT COUNT(*) FROM Banks)
DECLARE @DecisionsCount int = (SELECT COUNT(*) FROM Decisions)

PRINT 'عدد المدن: ' + CAST(@CitiesCount AS VARCHAR(10))
PRINT 'عدد المناطق: ' + CAST(@RegionsCount AS VARCHAR(10))
PRINT 'عدد المنتجات: ' + CAST(@ProductsCount AS VARCHAR(10))
PRINT 'عدد البنوك: ' + CAST(@BanksCount AS VARCHAR(10))
PRINT 'عدد القرارات: ' + CAST(@DecisionsCount AS VARCHAR(10))

PRINT 'تم إدخال 10 سجلات في كل جدول بنجاح! 🚀' 