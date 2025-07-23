-- ملف إضافة مساجد إضافية في مدن أخرى
-- قم بتشغيل هذا الملف بعد تشغيل seed_complete_mosque_data.sql

PRINT 'إضافة مساجد إضافية في مدن أخرى...'

-- التحقق من وجود البيانات الأساسية
IF NOT EXISTS (SELECT * FROM Mosques)
BEGIN
    PRINT 'خطأ: يجب تشغيل seed_complete_mosque_data.sql أولاً!'
    RETURN
END

-- إضافة مساجد إضافية
BEGIN TRY
    -- الحصول على OfficeId و RegionIds
    DECLARE @MainOfficeId uniqueidentifier = (SELECT TOP 1 Id FROM Offices WHERE Name = 'Main Office')
    DECLARE @ZawiyaRegionId uniqueidentifier = (SELECT TOP 1 Id FROM Regions WHERE Name = 'وسط الزاوية')
    DECLARE @AlBaydaRegionId uniqueidentifier = (SELECT TOP 1 Id FROM Regions WHERE Name = 'وسط البيضاء')
    DECLARE @SirtRegionId uniqueidentifier = (SELECT TOP 1 Id FROM Regions WHERE Name = 'وسط سرت')
    DECLARE @TobrukRegionId uniqueidentifier = (SELECT TOP 1 Id FROM Regions WHERE Name = 'وسط طبرق')
    DECLARE @AdminUserId nvarchar(450) = (SELECT TOP 1 Id FROM AspNetUsers WHERE UserName = 'admin@demo.com')

    -- إذا لم توجد البيانات الأساسية، نستخدم قيم افتراضية
    IF @MainOfficeId IS NULL SET @MainOfficeId = 'DDEC6E9E-7628-4623-9A94-4E4EFC02187C'
    IF @ZawiyaRegionId IS NULL SET @ZawiyaRegionId = (SELECT TOP 1 Id FROM Regions WHERE Name LIKE '%الزاوية%')
    IF @AlBaydaRegionId IS NULL SET @AlBaydaRegionId = (SELECT TOP 1 Id FROM Regions WHERE Name LIKE '%البيضاء%')
    IF @SirtRegionId IS NULL SET @SirtRegionId = (SELECT TOP 1 Id FROM Regions WHERE Name LIKE '%سرت%')
    IF @TobrukRegionId IS NULL SET @TobrukRegionId = (SELECT TOP 1 Id FROM Regions WHERE Name LIKE '%طبرق%')
    IF @AdminUserId IS NULL SET @AdminUserId = 'a3d890d8-01d1-444b-9f62-6336b937e5fc'

    -- إذا لم توجد المناطق، نستخدم منطقة طرابلس
    IF @ZawiyaRegionId IS NULL SET @ZawiyaRegionId = (SELECT TOP 1 Id FROM Regions WHERE Name = 'وسط طرابلس')
    IF @AlBaydaRegionId IS NULL SET @AlBaydaRegionId = (SELECT TOP 1 Id FROM Regions WHERE Name = 'وسط طرابلس')
    IF @SirtRegionId IS NULL SET @SirtRegionId = (SELECT TOP 1 Id FROM Regions WHERE Name = 'وسط طرابلس')
    IF @TobrukRegionId IS NULL SET @TobrukRegionId = (SELECT TOP 1 Id FROM Regions WHERE Name = 'وسط طرابلس')

    INSERT INTO Buildings (Id, Name, FileNumber, Definition, Classification, OfficeId, Unit, RegionId, NearestLandmark, ConstructionDate, OpeningDate, MapLocation, TotalLandArea, TotalCoveredArea, NumberOfFloors, ElectricityMeter, AlternativeEnergySource, WaterSource, Sanitation, BriefDescription, LandDonorName, SourceFunds, PrayerCapacity, UserId, ServicesSpecialNeeds, SpecialEntranceWomen, PicturePath) VALUES
    -- مساجد الزاوية
    (NEWID(), 'مسجد سيدي أحمد الزاوي', 'MOS-011', 'مسجد جامع', 'مسجد تراثي', @MainOfficeId, 'وحدة إدارية', @ZawiyaRegionId, 'وسط الزاوية', '1600-01-01', '1600-01-01', '32.7522,12.7277', 900.0, 650.0, 1, 'ELEC-011', 'لا يوجد', 'بئر تقليدي', 'صرف صحي', 'مسجد سيدي أحمد الزاوي التاريخي', 'وقف إسلامي', 2, '120 مصلي', @AdminUserId, 0, 0, '/images/mosques/sidi_ahmed_zawiya.jpg'),
    (NEWID(), 'مسجد سيدي محمد الزاوي', 'MOS-012', 'مسجد جامع', 'مسجد تراثي', @MainOfficeId, 'وحدة إدارية', @ZawiyaRegionId, 'حي النصر', '1700-01-01', '1700-01-01', '32.7525,12.7280', 1100.0, 750.0, 1, 'ELEC-012', 'لا يوجد', 'بئر تقليدي', 'صرف صحي', 'مسجد سيدي محمد الزاوي', 'وقف إسلامي', 2, '160 مصلي', @AdminUserId, 0, 0, '/images/mosques/sidi_mohammed_zawiya.jpg'),
    
    -- مساجد البيضاء
    (NEWID(), 'مسجد العتيق البيضاء', 'MOS-013', 'مسجد جامع', 'مسجد تراثي', @MainOfficeId, 'وحدة إدارية', @AlBaydaRegionId, 'وسط البيضاء', '1400-01-01', '1400-01-01', '32.7628,21.7551', 800.0, 600.0, 1, 'ELEC-013', 'لا يوجد', 'بئر تقليدي', 'صرف صحي', 'مسجد العتيق التاريخي في البيضاء', 'وقف إسلامي', 2, '100 مصلي', @AdminUserId, 0, 0, '/images/mosques/ateeq_albayda.jpg'),
    (NEWID(), 'مسجد سيدي عبد الله البيضاء', 'MOS-014', 'مسجد جامع', 'مسجد تراثي', @MainOfficeId, 'وحدة إدارية', @AlBaydaRegionId, 'حي الجبل الأخضر', '1650-01-01', '1650-01-01', '32.7630,21.7555', 1200.0, 850.0, 1, 'ELEC-014', 'لا يوجد', 'بئر تقليدي', 'صرف صحي', 'مسجد سيدي عبد الله في البيضاء', 'وقف إسلامي', 2, '180 مصلي', @AdminUserId, 0, 0, '/images/mosques/sidi_abdullah_albayda.jpg'),
    
    -- مساجد سرت
    (NEWID(), 'مسجد سيدي علي سرت', 'MOS-015', 'مسجد جامع', 'مسجد تراثي', @MainOfficeId, 'وحدة إدارية', @SirtRegionId, 'وسط سرت', '1700-01-01', '1700-01-01', '31.2089,16.5887', 1000.0, 700.0, 1, 'ELEC-015', 'لا يوجد', 'بئر تقليدي', 'صرف صحي', 'مسجد سيدي علي في سرت', 'وقف إسلامي', 2, '150 مصلي', @AdminUserId, 0, 0, '/images/mosques/sidi_ali_sirt.jpg'),
    (NEWID(), 'مسجد سيدي حسن سرت', 'MOS-016', 'مسجد جامع', 'مسجد تراثي', @MainOfficeId, 'وحدة إدارية', @SirtRegionId, 'حي الوحدة', '1750-01-01', '1750-01-01', '31.2092,16.5890', 950.0, 680.0, 1, 'ELEC-016', 'لا يوجد', 'بئر تقليدي', 'صرف صحي', 'مسجد سيدي حسن في سرت', 'وقف إسلامي', 2, '140 مصلي', @AdminUserId, 0, 0, '/images/mosques/sidi_hassan_sirt.jpg'),
    
    -- مساجد طبرق
    (NEWID(), 'مسجد سيدي عمر طبرق', 'MOS-017', 'مسجد جامع', 'مسجد تراثي', @MainOfficeId, 'وحدة إدارية', @TobrukRegionId, 'وسط طبرق', '1600-01-01', '1600-01-01', '32.0835,23.9765', 850.0, 600.0, 1, 'ELEC-017', 'لا يوجد', 'بئر تقليدي', 'صرف صحي', 'مسجد سيدي عمر في طبرق', 'وقف إسلامي', 2, '110 مصلي', @AdminUserId, 0, 0, '/images/mosques/sidi_omar_tobruk.jpg'),
    (NEWID(), 'مسجد سيدي يوسف طبرق', 'MOS-018', 'مسجد جامع', 'مسجد تراثي', @MainOfficeId, 'وحدة إدارية', @TobrukRegionId, 'حي الشهداء', '1650-01-01', '1650-01-01', '32.0838,23.9768', 1100.0, 750.0, 1, 'ELEC-018', 'لا يوجد', 'بئر تقليدي', 'صرف صحي', 'مسجد سيدي يوسف في طبرق', 'وقف إسلامي', 2, '160 مصلي', @AdminUserId, 0, 0, '/images/mosques/sidi_youssef_tobruk.jpg'),
    
    -- مساجد حديثة إضافية
    (NEWID(), 'مسجد النور الجديد', 'MOS-019', 'مسجد جامع', 'مسجد حكومي', @MainOfficeId, 'وحدة إدارية', @ZawiyaRegionId, 'حي الفتح', '2015-01-01', '2017-01-01', '32.7520,12.7285', 2800.0, 2000.0, 2, 'ELEC-019', 'طاقة شمسية', 'شبكة مياه', 'شبكة صرف', 'مسجد النور الجديد في الزاوية', 'وزارة الأوقاف', 1, '600 مصلي', @AdminUserId, 1, 1, '/images/mosques/nour_new.jpg'),
    (NEWID(), 'مسجد الرحمة الجديد', 'MOS-020', 'مسجد جامع', 'مسجد حكومي', @MainOfficeId, 'وحدة إدارية', @AlBaydaRegionId, 'حي النصر', '2018-01-01', '2020-01-01', '32.7635,21.7560', 3200.0, 2400.0, 2, 'ELEC-020', 'طاقة شمسية', 'شبكة مياه', 'شبكة صرف', 'مسجد الرحمة الجديد في البيضاء', 'وزارة الأوقاف', 1, '700 مصلي', @AdminUserId, 1, 1, '/images/mosques/rahma_new.jpg')

    PRINT 'تم إضافة المساجد الإضافية بنجاح!'
END TRY
BEGIN CATCH
    PRINT 'خطأ في إضافة المساجد الإضافية: ' + ERROR_MESSAGE()
END CATCH

-- إضافة بيانات المساجد
BEGIN TRY
    -- الحصول على BuildingIds
    DECLARE @Mosque11Id uniqueidentifier = (SELECT TOP 1 Id FROM Buildings WHERE Name = 'مسجد سيدي أحمد الزاوي')
    DECLARE @Mosque12Id uniqueidentifier = (SELECT TOP 1 Id FROM Buildings WHERE Name = 'مسجد سيدي محمد الزاوي')
    DECLARE @Mosque13Id uniqueidentifier = (SELECT TOP 1 Id FROM Buildings WHERE Name = 'مسجد العتيق البيضاء')
    DECLARE @Mosque14Id uniqueidentifier = (SELECT TOP 1 Id FROM Buildings WHERE Name = 'مسجد سيدي عبد الله البيضاء')
    DECLARE @Mosque15Id uniqueidentifier = (SELECT TOP 1 Id FROM Buildings WHERE Name = 'مسجد سيدي علي سرت')
    DECLARE @Mosque16Id uniqueidentifier = (SELECT TOP 1 Id FROM Buildings WHERE Name = 'مسجد سيدي حسن سرت')
    DECLARE @Mosque17Id uniqueidentifier = (SELECT TOP 1 Id FROM Buildings WHERE Name = 'مسجد سيدي عمر طبرق')
    DECLARE @Mosque18Id uniqueidentifier = (SELECT TOP 1 Id FROM Buildings WHERE Name = 'مسجد سيدي يوسف طبرق')
    DECLARE @Mosque19Id uniqueidentifier = (SELECT TOP 1 Id FROM Buildings WHERE Name = 'مسجد النور الجديد')
    DECLARE @Mosque20Id uniqueidentifier = (SELECT TOP 1 Id FROM Buildings WHERE Name = 'مسجد الرحمة الجديد')

    INSERT INTO Mosques (Id, BuildingId, MosqueDefinition, MosqueClassification) VALUES
    (NEWID(), @Mosque11Id, 1, 2), -- مسجد جامع، مسجد تراثي
    (NEWID(), @Mosque12Id, 1, 2), -- مسجد جامع، مسجد تراثي
    (NEWID(), @Mosque13Id, 1, 2), -- مسجد جامع، مسجد تراثي
    (NEWID(), @Mosque14Id, 1, 2), -- مسجد جامع، مسجد تراثي
    (NEWID(), @Mosque15Id, 1, 2), -- مسجد جامع، مسجد تراثي
    (NEWID(), @Mosque16Id, 1, 2), -- مسجد جامع، مسجد تراثي
    (NEWID(), @Mosque17Id, 1, 2), -- مسجد جامع، مسجد تراثي
    (NEWID(), @Mosque18Id, 1, 2), -- مسجد جامع، مسجد تراثي
    (NEWID(), @Mosque19Id, 1, 1), -- مسجد جامع، مسجد حكومي
    (NEWID(), @Mosque20Id, 1, 1)  -- مسجد جامع، مسجد حكومي

    PRINT 'تم إضافة بيانات المساجد الإضافية بنجاح!'
END TRY
BEGIN CATCH
    PRINT 'خطأ في إضافة بيانات المساجد الإضافية: ' + ERROR_MESSAGE()
END CATCH

-- إضافة بيانات البنايات التفصيلية للمساجد الجديدة
BEGIN TRY
    -- الحصول على BuildingIds للمساجد الجديدة
    DECLARE @Building11Id uniqueidentifier = (SELECT TOP 1 Id FROM Buildings WHERE Name = 'مسجد سيدي أحمد الزاوي')
    DECLARE @Building12Id uniqueidentifier = (SELECT TOP 1 Id FROM Buildings WHERE Name = 'مسجد سيدي محمد الزاوي')
    DECLARE @Building13Id uniqueidentifier = (SELECT TOP 1 Id FROM Buildings WHERE Name = 'مسجد العتيق البيضاء')
    DECLARE @Building14Id uniqueidentifier = (SELECT TOP 1 Id FROM Buildings WHERE Name = 'مسجد سيدي عبد الله البيضاء')
    DECLARE @Building15Id uniqueidentifier = (SELECT TOP 1 Id FROM Buildings WHERE Name = 'مسجد سيدي علي سرت')
    DECLARE @Building16Id uniqueidentifier = (SELECT TOP 1 Id FROM Buildings WHERE Name = 'مسجد سيدي حسن سرت')
    DECLARE @Building17Id uniqueidentifier = (SELECT TOP 1 Id FROM Buildings WHERE Name = 'مسجد سيدي عمر طبرق')
    DECLARE @Building18Id uniqueidentifier = (SELECT TOP 1 Id FROM Buildings WHERE Name = 'مسجد سيدي يوسف طبرق')
    DECLARE @Building19Id uniqueidentifier = (SELECT TOP 1 Id FROM Buildings WHERE Name = 'مسجد النور الجديد')
    DECLARE @Building20Id uniqueidentifier = (SELECT TOP 1 Id FROM Buildings WHERE Name = 'مسجد الرحمة الجديد')

    INSERT INTO BuildingDetails (Id, Name, WithinMosqueArea, Floors, BuildingCategory, BuildingId) VALUES
    -- مسجد سيدي أحمد الزاوي
    (NEWID(), 'المصلى الرئيسي', 1, 1, 1, @Building11Id),
    (NEWID(), 'المئذنة', 1, 1, 2, @Building11Id),
    (NEWID(), 'غرف الإمام', 1, 1, 4, @Building11Id),
    (NEWID(), 'دورات المياه', 1, 1, 5, @Building11Id),
    (NEWID(), 'مرافق الوضوء', 1, 1, 9, @Building11Id),
    (NEWID(), 'الساحة الخارجية', 0, 0, 8, @Building11Id),

    -- مسجد سيدي محمد الزاوي
    (NEWID(), 'المصلى الرئيسي', 1, 1, 1, @Building12Id),
    (NEWID(), 'المئذنة', 1, 1, 2, @Building12Id),
    (NEWID(), 'المكتبة', 1, 1, 3, @Building12Id),
    (NEWID(), 'غرف الإمام', 1, 1, 4, @Building12Id),
    (NEWID(), 'دورات المياه', 1, 1, 5, @Building12Id),
    (NEWID(), 'مرافق الوضوء', 1, 1, 9, @Building12Id),
    (NEWID(), 'الساحة الخارجية', 0, 0, 8, @Building12Id),

    -- مسجد العتيق البيضاء
    (NEWID(), 'المصلى الرئيسي', 1, 1, 1, @Building13Id),
    (NEWID(), 'المئذنة', 1, 1, 2, @Building13Id),
    (NEWID(), 'غرف الإمام', 1, 1, 4, @Building13Id),
    (NEWID(), 'دورات المياه', 1, 1, 5, @Building13Id),
    (NEWID(), 'مرافق الوضوء', 1, 1, 9, @Building13Id),
    (NEWID(), 'الساحة الخارجية', 0, 0, 8, @Building13Id),

    -- مسجد سيدي عبد الله البيضاء
    (NEWID(), 'المصلى الرئيسي', 1, 1, 1, @Building14Id),
    (NEWID(), 'المئذنة', 1, 1, 2, @Building14Id),
    (NEWID(), 'المكتبة', 1, 1, 3, @Building14Id),
    (NEWID(), 'غرف الإمام', 1, 1, 4, @Building14Id),
    (NEWID(), 'دورات المياه', 1, 1, 5, @Building14Id),
    (NEWID(), 'مرافق الوضوء', 1, 1, 9, @Building14Id),
    (NEWID(), 'الساحة الخارجية', 0, 0, 8, @Building14Id),

    -- مسجد سيدي علي سرت
    (NEWID(), 'المصلى الرئيسي', 1, 1, 1, @Building15Id),
    (NEWID(), 'المئذنة', 1, 1, 2, @Building15Id),
    (NEWID(), 'غرف الإمام', 1, 1, 4, @Building15Id),
    (NEWID(), 'دورات المياه', 1, 1, 5, @Building15Id),
    (NEWID(), 'مرافق الوضوء', 1, 1, 9, @Building15Id),
    (NEWID(), 'الساحة الخارجية', 0, 0, 8, @Building15Id),

    -- مسجد سيدي حسن سرت
    (NEWID(), 'المصلى الرئيسي', 1, 1, 1, @Building16Id),
    (NEWID(), 'المئذنة', 1, 1, 2, @Building16Id),
    (NEWID(), 'المكتبة', 1, 1, 3, @Building16Id),
    (NEWID(), 'غرف الإمام', 1, 1, 4, @Building16Id),
    (NEWID(), 'دورات المياه', 1, 1, 5, @Building16Id),
    (NEWID(), 'مرافق الوضوء', 1, 1, 9, @Building16Id),
    (NEWID(), 'الساحة الخارجية', 0, 0, 8, @Building16Id),

    -- مسجد سيدي عمر طبرق
    (NEWID(), 'المصلى الرئيسي', 1, 1, 1, @Building17Id),
    (NEWID(), 'المئذنة', 1, 1, 2, @Building17Id),
    (NEWID(), 'غرف الإمام', 1, 1, 4, @Building17Id),
    (NEWID(), 'دورات المياه', 1, 1, 5, @Building17Id),
    (NEWID(), 'مرافق الوضوء', 1, 1, 9, @Building17Id),
    (NEWID(), 'الساحة الخارجية', 0, 0, 8, @Building17Id),

    -- مسجد سيدي يوسف طبرق
    (NEWID(), 'المصلى الرئيسي', 1, 1, 1, @Building18Id),
    (NEWID(), 'المئذنة', 1, 1, 2, @Building18Id),
    (NEWID(), 'المكتبة', 1, 1, 3, @Building18Id),
    (NEWID(), 'غرف الإمام', 1, 1, 4, @Building18Id),
    (NEWID(), 'دورات المياه', 1, 1, 5, @Building18Id),
    (NEWID(), 'مرافق الوضوء', 1, 1, 9, @Building18Id),
    (NEWID(), 'الساحة الخارجية', 0, 0, 8, @Building18Id),

    -- مسجد النور الجديد (مسجد حديث)
    (NEWID(), 'المصلى الرئيسي', 1, 2, 1, @Building19Id),
    (NEWID(), 'المئذنة', 1, 1, 2, @Building19Id),
    (NEWID(), 'المكتبة', 1, 1, 3, @Building19Id),
    (NEWID(), 'غرف الإمام', 1, 1, 4, @Building19Id),
    (NEWID(), 'دورات المياه', 1, 1, 5, @Building19Id),
    (NEWID(), 'مرافق الوضوء', 1, 1, 9, @Building19Id),
    (NEWID(), 'المطبخ', 1, 1, 6, @Building19Id),
    (NEWID(), 'المخزن', 1, 1, 7, @Building19Id),
    (NEWID(), 'قاعة اجتماعات', 1, 1, 10, @Building19Id),
    (NEWID(), 'غرف إدارية', 1, 1, 11, @Building19Id),
    (NEWID(), 'الساحة الخارجية', 0, 0, 8, @Building19Id),

    -- مسجد الرحمة الجديد (مسجد حديث)
    (NEWID(), 'المصلى الرئيسي', 1, 2, 1, @Building20Id),
    (NEWID(), 'المئذنة', 1, 1, 2, @Building20Id),
    (NEWID(), 'المكتبة', 1, 1, 3, @Building20Id),
    (NEWID(), 'غرف الإمام', 1, 1, 4, @Building20Id),
    (NEWID(), 'دورات المياه', 1, 1, 5, @Building20Id),
    (NEWID(), 'مرافق الوضوء', 1, 1, 9, @Building20Id),
    (NEWID(), 'المطبخ', 1, 1, 6, @Building20Id),
    (NEWID(), 'المخزن', 1, 1, 7, @Building20Id),
    (NEWID(), 'قاعة اجتماعات', 1, 1, 10, @Building20Id),
    (NEWID(), 'غرف إدارية', 1, 1, 11, @Building20Id),
    (NEWID(), 'الساحة الخارجية', 0, 0, 8, @Building20Id)

    PRINT 'تم إضافة بيانات البنايات التفصيلية للمساجد الجديدة بنجاح!'
END TRY
BEGIN CATCH
    PRINT 'خطأ في إضافة بيانات البنايات التفصيلية: ' + ERROR_MESSAGE()
END CATCH

-- عرض إحصائيات شاملة
PRINT '=== إحصائيات شاملة للبيانات المضافة ==='
PRINT 'إجمالي عدد البنايات (المساجد): ' + CAST((SELECT COUNT(*) FROM Buildings WHERE Name LIKE '%مسجد%') AS VARCHAR(10))
PRINT 'إجمالي عدد المساجد: ' + CAST((SELECT COUNT(*) FROM Mosques) AS VARCHAR(10))
PRINT 'إجمالي عدد البنايات التفصيلية: ' + CAST((SELECT COUNT(*) FROM BuildingDetails) AS VARCHAR(10))

PRINT ''
PRINT '=== تفاصيل المساجد المضافة ==='
SELECT 
    m.Id as MosqueId,
    b.Name as MosqueName,
    b.FileNumber,
    b.PrayerCapacity,
    r.Name as RegionName,
    c.Name as CityName,
    CASE m.MosqueDefinition 
        WHEN 1 THEN 'مسجد جامع'
        WHEN 2 THEN 'مسجد حي'
        WHEN 3 THEN 'مسجد مصلى'
        ELSE 'غير محدد'
    END as MosqueType,
    CASE m.MosqueClassification
        WHEN 1 THEN 'مسجد حكومي'
        WHEN 2 THEN 'مسجد تراثي'
        WHEN 3 THEN 'مسجد خاص'
        ELSE 'غير محدد'
    END as MosqueCategory
FROM Mosques m
INNER JOIN Buildings b ON m.BuildingId = b.Id
INNER JOIN Regions r ON b.RegionId = r.Id
INNER JOIN Cities c ON r.CityId = c.Id
ORDER BY c.Name, b.Name

PRINT ''
PRINT 'تم إضافة جميع المساجد الإضافية بنجاح! 🕌✨'
PRINT 'إجمالي عدد المساجد في النظام: ' + CAST((SELECT COUNT(*) FROM Mosques) AS VARCHAR(10)) 