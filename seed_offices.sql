-- ملف إضافة بيانات المكاتب في مختلف المدن الليبية
-- قم بتشغيل هذا الملف بعد تشغيل run_all_seed_data.sql

-- إعداد الترميز للعربية
SET QUOTED_IDENTIFIER ON;
SET ANSI_NULLS ON;

PRINT '=== بدء إضافة بيانات المكاتب ==='

-- التحقق من وجود البيانات الأساسية
IF NOT EXISTS (SELECT * FROM Cities WHERE Name = 'طرابلس')
BEGIN
    PRINT 'خطأ: يجب تشغيل run_all_seed_data.sql أولاً!'
    RETURN
END

-- إضافة بيانات المكاتب
BEGIN TRY
    -- الحصول على RegionIds
    DECLARE @TripoliRegionId uniqueidentifier = (SELECT TOP 1 Id FROM Regions WHERE Name = 'وسط طرابلس')
    DECLARE @BenghaziRegionId uniqueidentifier = (SELECT TOP 1 Id FROM Regions WHERE Name = 'وسط بنغازي')
    DECLARE @MisrataRegionId uniqueidentifier = (SELECT TOP 1 Id FROM Regions WHERE Name = 'وسط مصراتة')
    DECLARE @ZawiyaRegionId uniqueidentifier = (SELECT TOP 1 Id FROM Regions WHERE Name = 'وسط الزاوية')
    DECLARE @AlBaydaRegionId uniqueidentifier = (SELECT TOP 1 Id FROM Regions WHERE Name = 'وسط البيضاء')
    DECLARE @SirtRegionId uniqueidentifier = (SELECT TOP 1 Id FROM Regions WHERE Name = 'وسط سرت')
    DECLARE @TobrukRegionId uniqueidentifier = (SELECT TOP 1 Id FROM Regions WHERE Name = 'وسط طبرق')
    DECLARE @AdminUserId uniqueidentifier = (SELECT TOP 1 Id FROM AspNetUsers WHERE UserName = 'admin@demo.com')

    -- إذا لم توجد البيانات الأساسية، نستخدم قيم افتراضية
    IF @TripoliRegionId IS NULL SET @TripoliRegionId = 'DDEC6E9E-7698-4623-9A84-4E5EFC02187C'
    IF @BenghaziRegionId IS NULL SET @BenghaziRegionId = @TripoliRegionId
    IF @MisrataRegionId IS NULL SET @MisrataRegionId = @TripoliRegionId
    IF @ZawiyaRegionId IS NULL SET @ZawiyaRegionId = @TripoliRegionId
    IF @AlBaydaRegionId IS NULL SET @AlBaydaRegionId = @TripoliRegionId
    IF @SirtRegionId IS NULL SET @SirtRegionId = @TripoliRegionId
    IF @TobrukRegionId IS NULL SET @TobrukRegionId = @TripoliRegionId
    IF @AdminUserId IS NULL SET @AdminUserId = 'a3d890d8-01d1-494b-9f62-6336b937e6fc'

    INSERT INTO Offices (Id, Name, Location, PhoneNumber, RegionId, UserId) VALUES
    -- مكاتب طرابلس
    (NEWID(), 'المكتب الرئيسي للأوقاف - طرابلس', 'وسط طرابلس - شارع طرابلس', '0218-91-1234567', @TripoliRegionId, @AdminUserId),
    (NEWID(), 'مكتب الأوقاف - حي الأندلس', 'حي الأندلس - طرابلس', '0218-91-1234568', @TripoliRegionId, @AdminUserId),
    (NEWID(), 'مكتب الأوقاف - حي النصر', 'حي النصر - طرابلس', '0218-91-1234569', @TripoliRegionId, @AdminUserId),
    (NEWID(), 'مكتب الأوقاف - حي الهضبة', 'حي الهضبة - طرابلس', '0218-91-1234570', @TripoliRegionId, @AdminUserId),
    (NEWID(), 'مكتب الأوقاف - حي سيدي المصري', 'حي سيدي المصري - طرابلس', '0218-91-1234571', @TripoliRegionId, @AdminUserId),
    (NEWID(), 'مكتب الأوقاف - حي باب العزيزية', 'حي باب العزيزية - طرابلس', '0218-91-1234572', @TripoliRegionId, @AdminUserId),
    (NEWID(), 'مكتب الأوقاف - حي أبو سليم', 'حي أبو سليم - طرابلس', '0218-91-1234573', @TripoliRegionId, @AdminUserId),
    (NEWID(), 'مكتب الأوقاف - حي عين زارة', 'حي عين زارة - طرابلس', '0218-91-1234574', @TripoliRegionId, @AdminUserId),
    (NEWID(), 'مكتب الأوقاف - حي تاجوراء', 'حي تاجوراء - طرابلس', '0218-91-1234575', @TripoliRegionId, @AdminUserId),

    -- مكاتب بنغازي
    (NEWID(), 'المكتب الرئيسي للأوقاف - بنغازي', 'وسط بنغازي - شارع بنغازي', '0218-61-1234567', @BenghaziRegionId, @AdminUserId),
    (NEWID(), 'مكتب الأوقاف - حي الصابري', 'حي الصابري - بنغازي', '0218-61-1234568', @BenghaziRegionId, @AdminUserId),
    (NEWID(), 'مكتب الأوقاف - حي سيدي خريبيش', 'حي سيدي خريبيش - بنغازي', '0218-61-1234569', @BenghaziRegionId, @AdminUserId),
    (NEWID(), 'مكتب الأوقاف - حي الكويفية', 'حي الكويفية - بنغازي', '0218-61-1234570', @BenghaziRegionId, @AdminUserId),
    (NEWID(), 'مكتب الأوقاف - حي البركة', 'حي البركة - بنغازي', '0218-61-1234571', @BenghaziRegionId, @AdminUserId),
    (NEWID(), 'مكتب الأوقاف - حي سيدي حسين', 'حي سيدي حسين - بنغازي', '0218-61-1234572', @BenghaziRegionId, @AdminUserId),
    (NEWID(), 'مكتب الأوقاف - حي القوارشة', 'حي القوارشة - بنغازي', '0218-61-1234573', @BenghaziRegionId, @AdminUserId),
    (NEWID(), 'مكتب الأوقاف - حي الهواري', 'حي الهواري - بنغازي', '0218-61-1234574', @BenghaziRegionId, @AdminUserId),
    (NEWID(), 'مكتب الأوقاف - حي الجواري', 'حي الجواري - بنغازي', '0218-61-1234575', @BenghaziRegionId, @AdminUserId),
    (NEWID(), 'مكتب الأوقاف - حي المقرون', 'حي المقرون - بنغازي', '0218-61-1234576', @BenghaziRegionId, @AdminUserId),

    -- مكاتب مصراتة
    (NEWID(), 'المكتب الرئيسي للأوقاف - مصراتة', 'وسط مصراتة - شارع مصراتة', '0218-51-1234567', @MisrataRegionId, @AdminUserId),
    (NEWID(), 'مكتب الأوقاف - حي الزاوية', 'حي الزاوية - مصراتة', '0218-51-1234568', @MisrataRegionId, @AdminUserId),
    (NEWID(), 'مكتب الأوقاف - حي سوق الثلاثاء', 'حي سوق الثلاثاء - مصراتة', '0218-51-1234569', @MisrataRegionId, @AdminUserId),
    (NEWID(), 'مكتب الأوقاف - حي سوق الجمعة', 'حي سوق الجمعة - مصراتة', '0218-51-1234570', @MisrataRegionId, @AdminUserId),
    (NEWID(), 'مكتب الأوقاف - حي القره بولي', 'حي القره بولي - مصراتة', '0218-51-1234571', @MisrataRegionId, @AdminUserId),
    (NEWID(), 'مكتب الأوقاف - حي تاوورغا', 'حي تاوورغا - مصراتة', '0218-51-1234572', @MisrataRegionId, @AdminUserId),
    (NEWID(), 'مكتب الأوقاف - حي زليتن', 'حي زليتن - مصراتة', '0218-51-1234573', @MisrataRegionId, @AdminUserId),
    (NEWID(), 'مكتب الأوقاف - حي الخمس', 'حي الخمس - مصراتة', '0218-51-1234574', @MisrataRegionId, @AdminUserId),

    -- مكاتب الزاوية
    (NEWID(), 'المكتب الرئيسي للأوقاف - الزاوية', 'وسط الزاوية - شارع الزاوية', '0218-23-1234567', @ZawiyaRegionId, @AdminUserId),
    (NEWID(), 'مكتب الأوقاف - حي النصر', 'حي النصر - الزاوية', '0218-23-1234568', @ZawiyaRegionId, @AdminUserId),
    (NEWID(), 'مكتب الأوقاف - حي الوحدة', 'حي الوحدة - الزاوية', '0218-23-1234569', @ZawiyaRegionId, @AdminUserId),
    (NEWID(), 'مكتب الأوقاف - حي الشهداء', 'حي الشهداء - الزاوية', '0218-23-1234570', @ZawiyaRegionId, @AdminUserId),
    (NEWID(), 'مكتب الأوقاف - حي الفتح', 'حي الفتح - الزاوية', '0218-23-1234571', @ZawiyaRegionId, @AdminUserId),
    (NEWID(), 'مكتب الأوقاف - حي الثورة', 'حي الثورة - الزاوية', '0218-23-1234572', @ZawiyaRegionId, @AdminUserId),
    (NEWID(), 'مكتب الأوقاف - حي الحرية', 'حي الحرية - الزاوية', '0218-23-1234573', @ZawiyaRegionId, @AdminUserId),
    (NEWID(), 'مكتب الأوقاف - حي الاستقلال', 'حي الاستقلال - الزاوية', '0218-23-1234574', @ZawiyaRegionId, @AdminUserId),
    (NEWID(), 'مكتب الأوقاف - حي الوادي', 'حي الوادي - الزاوية', '0218-23-1234575', @ZawiyaRegionId, @AdminUserId),
    (NEWID(), 'مكتب الأوقاف - حي الساحل', 'حي الساحل - الزاوية', '0218-23-1234576', @ZawiyaRegionId, @AdminUserId),

    -- مكاتب البيضاء
    (NEWID(), 'المكتب الرئيسي للأوقاف - البيضاء', 'وسط البيضاء - شارع البيضاء', '0218-84-1234567', @AlBaydaRegionId, @AdminUserId),
    (NEWID(), 'مكتب الأوقاف - حي الجبل الأخضر', 'حي الجبل الأخضر - البيضاء', '0218-84-1234568', @AlBaydaRegionId, @AdminUserId),
    (NEWID(), 'مكتب الأوقاف - حي المرج', 'حي المرج - البيضاء', '0218-84-1234569', @AlBaydaRegionId, @AdminUserId),
    (NEWID(), 'مكتب الأوقاف - حي درنة', 'حي درنة - البيضاء', '0218-84-1234570', @AlBaydaRegionId, @AdminUserId),
    (NEWID(), 'مكتب الأوقاف - حي شحات', 'حي شحات - البيضاء', '0218-84-1234571', @AlBaydaRegionId, @AdminUserId),
    (NEWID(), 'مكتب الأوقاف - حي طبرق', 'حي طبرق - البيضاء', '0218-84-1234572', @AlBaydaRegionId, @AdminUserId),
    (NEWID(), 'مكتب الأوقاف - حي سرت', 'حي سرت - البيضاء', '0218-84-1234573', @AlBaydaRegionId, @AdminUserId),
    (NEWID(), 'مكتب الأوقاف - حي أجدابيا', 'حي أجدابيا - البيضاء', '0218-84-1234574', @AlBaydaRegionId, @AdminUserId),
    (NEWID(), 'مكتب الأوقاف - حي سبها', 'حي سبها - البيضاء', '0218-84-1234575', @AlBaydaRegionId, @AdminUserId),
    (NEWID(), 'مكتب الأوقاف - حي غات', 'حي غات - البيضاء', '0218-84-1234576', @AlBaydaRegionId, @AdminUserId),

    -- مكاتب سرت
    (NEWID(), 'المكتب الرئيسي للأوقاف - سرت', 'وسط سرت - شارع سرت', '0218-54-1234567', @SirtRegionId, @AdminUserId),
    (NEWID(), 'مكتب الأوقاف - حي الوحدة', 'حي الوحدة - سرت', '0218-54-1234568', @SirtRegionId, @AdminUserId),
    (NEWID(), 'مكتب الأوقاف - حي النصر', 'حي النصر - سرت', '0218-54-1234569', @SirtRegionId, @AdminUserId),
    (NEWID(), 'مكتب الأوقاف - حي الشهداء', 'حي الشهداء - سرت', '0218-54-1234570', @SirtRegionId, @AdminUserId),
    (NEWID(), 'مكتب الأوقاف - حي الفتح', 'حي الفتح - سرت', '0218-54-1234571', @SirtRegionId, @AdminUserId),
    (NEWID(), 'مكتب الأوقاف - حي الثورة', 'حي الثورة - سرت', '0218-54-1234572', @SirtRegionId, @AdminUserId),
    (NEWID(), 'مكتب الأوقاف - حي الحرية', 'حي الحرية - سرت', '0218-54-1234573', @SirtRegionId, @AdminUserId),
    (NEWID(), 'مكتب الأوقاف - حي الاستقلال', 'حي الاستقلال - سرت', '0218-54-1234574', @SirtRegionId, @AdminUserId),

    -- مكاتب طبرق
    (NEWID(), 'المكتب الرئيسي للأوقاف - طبرق', 'وسط طبرق - شارع طبرق', '0218-18-1234567', @TobrukRegionId, @AdminUserId),
    (NEWID(), 'مكتب الأوقاف - حي الشهداء', 'حي الشهداء - طبرق', '0218-18-1234568', @TobrukRegionId, @AdminUserId),
    (NEWID(), 'مكتب الأوقاف - حي النصر', 'حي النصر - طبرق', '0218-18-1234569', @TobrukRegionId, @AdminUserId),
    (NEWID(), 'مكتب الأوقاف - حي الوحدة', 'حي الوحدة - طبرق', '0218-18-1234570', @TobrukRegionId, @AdminUserId),
    (NEWID(), 'مكتب الأوقاف - حي الفتح', 'حي الفتح - طبرق', '0218-18-1234571', @TobrukRegionId, @AdminUserId),
    (NEWID(), 'مكتب الأوقاف - حي الثورة', 'حي الثورة - طبرق', '0218-18-1234572', @TobrukRegionId, @AdminUserId),
    (NEWID(), 'مكتب الأوقاف - حي الحرية', 'حي الحرية - طبرق', '0218-18-1234573', @TobrukRegionId, @AdminUserId),
    (NEWID(), 'مكتب الأوقاف - حي الاستقلال', 'حي الاستقلال - طبرق', '0218-18-1234574', @TobrukRegionId, @AdminUserId),

    -- مكاتب إضافية في مدن أخرى
    (NEWID(), 'مكتب الأوقاف - الخمس', 'وسط الخمس - شارع الخمس', '0218-31-1234567', @TripoliRegionId, @AdminUserId),
    (NEWID(), 'مكتب الأوقاف - زليتن', 'وسط زليتن - شارع زليتن', '0218-31-1234568', @TripoliRegionId, @AdminUserId),
    (NEWID(), 'مكتب الأوقاف - أجدابيا', 'وسط أجدابيا - شارع أجدابيا', '0218-47-1234567', @BenghaziRegionId, @AdminUserId),
    (NEWID(), 'مكتب الأوقاف - سبها', 'وسط سبها - شارع سبها', '0218-71-1234567', @AlBaydaRegionId, @AdminUserId),
    (NEWID(), 'مكتب الأوقاف - غات', 'وسط غات - شارع غات', '0218-71-1234568', @AlBaydaRegionId, @AdminUserId),
    (NEWID(), 'مكتب الأوقاف - أوباري', 'وسط أوباري - شارع أوباري', '0218-71-1234569', @AlBaydaRegionId, @AdminUserId),
    (NEWID(), 'مكتب الأوقاف - المرج', 'وسط المرج - شارع المرج', '0218-84-1234577', @AlBaydaRegionId, @AdminUserId),
    (NEWID(), 'مكتب الأوقاف - درنة', 'وسط درنة - شارع درنة', '0218-84-1234578', @AlBaydaRegionId, @AdminUserId),
    (NEWID(), 'مكتب الأوقاف - شحات', 'وسط شحات - شارع شحات', '0218-84-1234579', @AlBaydaRegionId, @AdminUserId),
    (NEWID(), 'مكتب الأوقاف - جالو', 'وسط جالو - شارع جالو', '0218-84-1234580', @AlBaydaRegionId, @AdminUserId),
    (NEWID(), 'مكتب الأوقاف - أوجلة', 'وسط أوجلة - شارع أوجلة', '0218-84-1234581', @AlBaydaRegionId, @AdminUserId),
    (NEWID(), 'مكتب الأوقاف - جغبوب', 'وسط جغبوب - شارع جغبوب', '0218-84-1234582', @AlBaydaRegionId, @AdminUserId),
    (NEWID(), 'مكتب الأوقاف - الواحات', 'وسط الواحات - شارع الواحات', '0218-84-1234583', @AlBaydaRegionId, @AdminUserId),
    (NEWID(), 'مكتب الأوقاف - السرير', 'وسط السرير - شارع السرير', '0218-84-1234584', @AlBaydaRegionId, @AdminUserId)

    PRINT 'تم إضافة بيانات المكاتب بنجاح!'
END TRY
BEGIN CATCH
    PRINT 'خطأ في إضافة بيانات المكاتب: ' + ERROR_MESSAGE()
END CATCH

-- عرض إحصائيات شاملة
PRINT '=== إحصائيات شاملة للبيانات المضافة ==='
DECLARE @OfficesCount int = (SELECT COUNT(*) FROM Offices)
PRINT 'إجمالي عدد المكاتب: ' + CAST(@OfficesCount AS VARCHAR(10))

PRINT ''
PRINT '=== تفاصيل المكاتب المضافة ==='
SELECT 
    o.Id as OfficeId,
    o.Name as OfficeName,
    o.Location,
    o.PhoneNumber,
    r.Name as RegionName,
    c.Name as CityName
FROM Offices o
INNER JOIN Regions r ON o.RegionId = r.Id
INNER JOIN Cities c ON r.CityId = c.Id
ORDER BY c.Name, o.Name

PRINT ''
PRINT '=== إحصائيات المكاتب حسب المدينة ==='
SELECT 
    c.Name as CityName,
    COUNT(o.Id) as OfficeCount
FROM Offices o
INNER JOIN Regions r ON o.RegionId = r.Id
INNER JOIN Cities c ON r.CityId = c.Id
GROUP BY c.Name
ORDER BY OfficeCount DESC, c.Name

PRINT ''
PRINT 'تم إضافة جميع بيانات المكاتب بنجاح! 🏢✨'
PRINT 'إجمالي عدد المكاتب في النظام: ' + CAST(@OfficesCount AS VARCHAR(10)) 