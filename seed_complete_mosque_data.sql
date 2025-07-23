-- ملف شامل لإضافة بيانات المساجد والبنايات والمواد المرتبطة
-- قم بتشغيل هذا الملف بعد تشغيل run_all_seed_data.sql

-- إعداد الترميز للعربية
SET QUOTED_IDENTIFIER ON;
SET ANSI_NULLS ON;

PRINT '=== بدء إضافة بيانات المساجد والبنايات والمواد المرتبطة ==='

-- التحقق من وجود البيانات الأساسية
IF NOT EXISTS (SELECT * FROM Cities WHERE Name = 'طرابلس')
BEGIN
    PRINT 'خطأ: يجب تشغيل run_all_seed_data.sql أولاً!'
    RETURN
END

-- ===== إضافة منتجات خاصة بالمساجد =====
PRINT 'إضافة منتجات خاصة بالمساجد...'
BEGIN TRY
    INSERT INTO Products (Id, Name, Description) VALUES
    -- منتجات السجاد والفرش
    (NEWID(), 'سجاد مصلى', 'سجاد خاص بالمصليات والمساجد'),
    (NEWID(), 'سجاد تركي', 'سجاد تركي عالي الجودة للمساجد'),
    (NEWID(), 'سجاد إيراني', 'سجاد إيراني فاخر للمساجد'),
    (NEWID(), 'ثريات مساجد', 'ثريات إضاءة فاخرة للمساجد'),
    (NEWID(), 'مصابيح مئذنة', 'مصابيح خاصة بالمآذن'),
    (NEWID(), 'مكبرات صوت مساجد', 'مكبرات صوت عالية الجودة للمساجد'),
    (NEWID(), 'ميكروفونات إمام', 'ميكروفونات خاصة بالإمام'),
    (NEWID(), 'أحواض وضوء', 'أحواض الوضوء للمساجد'),
    (NEWID(), 'دش وضوء', 'دش للوضوء'),
    (NEWID(), 'كتب دينية', 'كتب دينية متنوعة'),
    (NEWID(), 'مصاحف شريفة', 'مصاحف شريفة بخطوط مختلفة'),
    (NEWID(), 'ساعات مساجد', 'ساعات خاصة بالمساجد'),
    (NEWID(), 'أنظمة توقيت صلاة', 'أنظمة توقيت الصلاة'),
    (NEWID(), 'طفايات حريق مساجد', 'طفايات حريق للمساجد'),
    (NEWID(), 'مبردات مياه مساجد', 'مبردات مياه للمساجد'),
    (NEWID(), 'زخارف إسلامية', 'زخارف إسلامية للمساجد'),
    (NEWID(), 'آيات قرآنية', 'آيات قرآنية للديكور'),
    (NEWID(), 'أنظمة ذكية للمساجد', 'أنظمة ذكية متكاملة للمساجد'),
    (NEWID(), 'ألواح شمسية مساجد', 'ألواح طاقة شمسية للمساجد'),
    (NEWID(), 'كاميرات مراقبة مساجد', 'كاميرات مراقبة للمساجد')

    PRINT 'تم إضافة منتجات المساجد بنجاح!'
END TRY
BEGIN CATCH
    PRINT 'خطأ في إضافة منتجات المساجد: ' + ERROR_MESSAGE()
END CATCH

-- ===== إضافة بيانات البنايات (المساجد) =====
PRINT 'إضافة بيانات البنايات (المساجد)...'
BEGIN TRY
    -- الحصول على OfficeId و RegionId
    DECLARE @MainOfficeId uniqueidentifier = (SELECT TOP 1 Id FROM Offices WHERE Name = 'Main Office')
    DECLARE @TripoliRegionId uniqueidentifier = (SELECT TOP 1 Id FROM Regions WHERE Name = 'وسط طرابلس')
    DECLARE @BenghaziRegionId uniqueidentifier = (SELECT TOP 1 Id FROM Regions WHERE Name = 'وسط بنغازي')
    DECLARE @MisrataRegionId uniqueidentifier = (SELECT TOP 1 Id FROM Regions WHERE Name = 'وسط مصراتة')
    DECLARE @AdminUserId nvarchar(450) = (SELECT TOP 1 Id FROM AspNetUsers WHERE UserName = 'admin@demo.com')

    -- إذا لم توجد البيانات الأساسية، نستخدم قيم افتراضية
    IF @MainOfficeId IS NULL SET @MainOfficeId = 'DDEC6E9E-7628-4623-9A94-4E4EFC02187C'
    IF @TripoliRegionId IS NULL SET @TripoliRegionId = 'DDEC6E9E-7698-4623-9A84-4E5EFC02187C'
    IF @BenghaziRegionId IS NULL SET @BenghaziRegionId = @TripoliRegionId
    IF @MisrataRegionId IS NULL SET @MisrataRegionId = @TripoliRegionId
    IF @AdminUserId IS NULL SET @AdminUserId = 'a3d890d8-01d1-444b-9f62-6336b937e5fc'

    INSERT INTO Buildings (Id, Name, FileNumber, Definition, Classification, OfficeId, Unit, RegionId, NearestLandmark, ConstructionDate, OpeningDate, MapLocation, TotalLandArea, TotalCoveredArea, NumberOfFloors, ElectricityMeter, AlternativeEnergySource, WaterSource, Sanitation, BriefDescription, LandDonorName, SourceFunds, PrayerCapacity, UserId, ServicesSpecialNeeds, SpecialEntranceWomen, PicturePath) VALUES
    -- مساجد طرابلس
    (NEWID(), 'مسجد النصر', 'MOS-001', 'مسجد جامع', 'مسجد حكومي', @MainOfficeId, 'وحدة إدارية', @TripoliRegionId, 'وسط المدينة', '1980-01-01', '1982-01-01', '32.8872,13.1913', 2500.0, 1800.0, 2, 'ELEC-001', 'طاقة شمسية', 'شبكة مياه', 'شبكة صرف', 'مسجد النصر التاريخي في وسط طرابلس', 'وقف النصر', 1, '500 مصلي', @AdminUserId, 1, 1, '/images/mosques/nasr.jpg'),
    (NEWID(), 'مسجد أحمد باشا', 'MOS-002', 'مسجد جامع', 'مسجد تراثي', @MainOfficeId, 'وحدة إدارية', @TripoliRegionId, 'القلعة القديمة', '1736-01-01', '1738-01-01', '32.8875,13.1918', 1200.0, 800.0, 1, 'ELEC-002', 'لا يوجد', 'بئر تقليدي', 'صرف صحي', 'مسجد أحمد باشا التراثي', 'أحمد باشا القرمانلي', 2, '200 مصلي', @AdminUserId, 0, 0, '/images/mosques/ahmed_pasha.jpg'),
    (NEWID(), 'مسجد درغوت باشا', 'MOS-003', 'مسجد جامع', 'مسجد تراثي', @MainOfficeId, 'وحدة إدارية', @TripoliRegionId, 'المدينة القديمة', '1560-01-01', '1560-01-01', '32.8870,13.1915', 1500.0, 1000.0, 1, 'ELEC-003', 'لا يوجد', 'بئر تقليدي', 'صرف صحي', 'مسجد درغوت باشا التاريخي', 'درغوت باشا', 2, '300 مصلي', @AdminUserId, 0, 0, '/images/mosques/draghut.jpg'),
    (NEWID(), 'مسجد سيدي عبد الواحد', 'MOS-004', 'مسجد جامع', 'مسجد تراثي', @MainOfficeId, 'وحدة إدارية', @TripoliRegionId, 'حي سيدي المصري', '1600-01-01', '1600-01-01', '32.8865,13.1920', 800.0, 600.0, 1, 'ELEC-004', 'لا يوجد', 'بئر تقليدي', 'صرف صحي', 'مسجد سيدي عبد الواحد', 'وقف إسلامي', 2, '150 مصلي', @AdminUserId, 0, 0, '/images/mosques/sidi_abdulwahid.jpg'),
    (NEWID(), 'مسجد سيدي سالم', 'MOS-005', 'مسجد جامع', 'مسجد تراثي', @MainOfficeId, 'وحدة إدارية', @TripoliRegionId, 'حي سيدي سالم', '1700-01-01', '1700-01-01', '32.8868,13.1910', 1000.0, 700.0, 1, 'ELEC-005', 'لا يوجد', 'بئر تقليدي', 'صرف صحي', 'مسجد سيدي سالم', 'وقف إسلامي', 2, '180 مصلي', @AdminUserId, 0, 0, '/images/mosques/sidi_salem.jpg'),
    
    -- مساجد بنغازي
    (NEWID(), 'مسجد العتيق بنغازي', 'MOS-006', 'مسجد جامع', 'مسجد تراثي', @MainOfficeId, 'وحدة إدارية', @BenghaziRegionId, 'وسط بنغازي', '1400-01-01', '1400-01-01', '32.1147,20.0686', 600.0, 400.0, 1, 'ELEC-006', 'لا يوجد', 'بئر تقليدي', 'صرف صحي', 'مسجد العتيق التاريخي في بنغازي', 'وقف إسلامي', 2, '100 مصلي', @AdminUserId, 0, 0, '/images/mosques/ateeq_benghazi.jpg'),
    (NEWID(), 'مسجد سيدي خريبيش بنغازي', 'MOS-007', 'مسجد جامع', 'مسجد تراثي', @MainOfficeId, 'وحدة إدارية', @BenghaziRegionId, 'حي سيدي خريبيش', '1800-01-01', '1800-01-01', '32.1140,20.0695', 900.0, 650.0, 1, 'ELEC-007', 'لا يوجد', 'بئر تقليدي', 'صرف صحي', 'مسجد سيدي خريبيش في بنغازي', 'وقف إسلامي', 2, '120 مصلي', @AdminUserId, 0, 0, '/images/mosques/sidi_khreibish_benghazi.jpg'),
    
    -- مساجد مصراتة
    (NEWID(), 'مسجد سيدي عبد السلام مصراتة', 'MOS-008', 'مسجد جامع', 'مسجد تراثي', @MainOfficeId, 'وحدة إدارية', @MisrataRegionId, 'وسط مصراتة', '1750-01-01', '1750-01-01', '32.3783,15.0906', 1100.0, 750.0, 1, 'ELEC-008', 'لا يوجد', 'بئر تقليدي', 'صرف صحي', 'مسجد سيدي عبد السلام في مصراتة', 'وقف إسلامي', 2, '160 مصلي', @AdminUserId, 0, 0, '/images/mosques/sidi_abdulsalam_misrata.jpg'),
    (NEWID(), 'مسجد سيدي محمد مصراتة', 'MOS-009', 'مسجد جامع', 'مسجد تراثي', @MainOfficeId, 'وحدة إدارية', @MisrataRegionId, 'حي سيدي محمد', '1650-01-01', '1650-01-01', '32.3787,20.0698', 950.0, 680.0, 1, 'ELEC-009', 'لا يوجد', 'بئر تقليدي', 'صرف صحي', 'مسجد سيدي محمد في مصراتة', 'وقف إسلامي', 2, '140 مصلي', @AdminUserId, 0, 0, '/images/mosques/sidi_mohammed_misrata.jpg'),
    
    -- مساجد حديثة
    (NEWID(), 'مسجد الفتح الجديد', 'MOS-010', 'مسجد جامع', 'مسجد حكومي', @MainOfficeId, 'وحدة إدارية', @TripoliRegionId, 'حي الفتح', '2010-01-01', '2012-01-01', '32.8864,13.1916', 3000.0, 2200.0, 2, 'ELEC-010', 'طاقة شمسية', 'شبكة مياه', 'شبكة صرف', 'مسجد الفتح الجديد', 'وزارة الأوقاف', 1, '800 مصلي', @AdminUserId, 1, 1, '/images/mosques/fatah_new.jpg')

    PRINT 'تم إضافة بيانات البنايات (المساجد) بنجاح!'
END TRY
BEGIN CATCH
    PRINT 'خطأ في إضافة بيانات البنايات: ' + ERROR_MESSAGE()
END CATCH

-- ===== إضافة بيانات المساجد =====
PRINT 'إضافة بيانات المساجد...'
BEGIN TRY
    -- الحصول على BuildingIds
    DECLARE @Mosque1Id uniqueidentifier = (SELECT TOP 1 Id FROM Buildings WHERE Name = 'مسجد النصر')
    DECLARE @Mosque2Id uniqueidentifier = (SELECT TOP 1 Id FROM Buildings WHERE Name = 'مسجد أحمد باشا')
    DECLARE @Mosque3Id uniqueidentifier = (SELECT TOP 1 Id FROM Buildings WHERE Name = 'مسجد درغوت باشا')
    DECLARE @Mosque4Id uniqueidentifier = (SELECT TOP 1 Id FROM Buildings WHERE Name = 'مسجد سيدي عبد الواحد')
    DECLARE @Mosque5Id uniqueidentifier = (SELECT TOP 1 Id FROM Buildings WHERE Name = 'مسجد سيدي سالم')
    DECLARE @Mosque6Id uniqueidentifier = (SELECT TOP 1 Id FROM Buildings WHERE Name = 'مسجد العتيق بنغازي')
    DECLARE @Mosque7Id uniqueidentifier = (SELECT TOP 1 Id FROM Buildings WHERE Name = 'مسجد سيدي خريبيش بنغازي')
    DECLARE @Mosque8Id uniqueidentifier = (SELECT TOP 1 Id FROM Buildings WHERE Name = 'مسجد سيدي عبد السلام مصراتة')
    DECLARE @Mosque9Id uniqueidentifier = (SELECT TOP 1 Id FROM Buildings WHERE Name = 'مسجد سيدي محمد مصراتة')
    DECLARE @Mosque10Id uniqueidentifier = (SELECT TOP 1 Id FROM Buildings WHERE Name = 'مسجد الفتح الجديد')

    INSERT INTO Mosques (Id, BuildingId, MosqueDefinition, MosqueClassification) VALUES
    (NEWID(), @Mosque1Id, 1, 1), -- مسجد جامع، مسجد حكومي
    (NEWID(), @Mosque2Id, 1, 2), -- مسجد جامع، مسجد تراثي
    (NEWID(), @Mosque3Id, 1, 2), -- مسجد جامع، مسجد تراثي
    (NEWID(), @Mosque4Id, 1, 2), -- مسجد جامع، مسجد تراثي
    (NEWID(), @Mosque5Id, 1, 2), -- مسجد جامع، مسجد تراثي
    (NEWID(), @Mosque6Id, 1, 2), -- مسجد جامع، مسجد تراثي
    (NEWID(), @Mosque7Id, 1, 2), -- مسجد جامع، مسجد تراثي
    (NEWID(), @Mosque8Id, 1, 2), -- مسجد جامع، مسجد تراثي
    (NEWID(), @Mosque9Id, 1, 2), -- مسجد جامع، مسجد تراثي
    (NEWID(), @Mosque10Id, 1, 1) -- مسجد جامع، مسجد حكومي

    PRINT 'تم إضافة بيانات المساجد بنجاح!'
END TRY
BEGIN CATCH
    PRINT 'خطأ في إضافة بيانات المساجد: ' + ERROR_MESSAGE()
END CATCH

-- ===== إضافة بيانات البنايات التفصيلية =====
PRINT 'إضافة بيانات البنايات التفصيلية...'
BEGIN TRY
    -- الحصول على BuildingIds
    DECLARE @Building1Id uniqueidentifier = (SELECT TOP 1 Id FROM Buildings WHERE Name = 'مسجد النصر')
    DECLARE @Building2Id uniqueidentifier = (SELECT TOP 1 Id FROM Buildings WHERE Name = 'مسجد أحمد باشا')
    DECLARE @Building3Id uniqueidentifier = (SELECT TOP 1 Id FROM Buildings WHERE Name = 'مسجد درغوت باشا')
    DECLARE @Building4Id uniqueidentifier = (SELECT TOP 1 Id FROM Buildings WHERE Name = 'مسجد سيدي عبد الواحد')
    DECLARE @Building5Id uniqueidentifier = (SELECT TOP 1 Id FROM Buildings WHERE Name = 'مسجد سيدي سالم')
    DECLARE @Building10Id uniqueidentifier = (SELECT TOP 1 Id FROM Buildings WHERE Name = 'مسجد الفتح الجديد')

    INSERT INTO BuildingDetails (Id, Name, WithinMosqueArea, Floors, BuildingCategory, BuildingId) VALUES
    -- مسجد النصر
    (NEWID(), 'المصلى الرئيسي', 1, 1, 1, @Building1Id), -- مصلى، داخل المسجد، طابق واحد، مصلى
    (NEWID(), 'المئذنة', 1, 1, 2, @Building1Id), -- مئذنة، داخل المسجد، طابق واحد، مئذنة
    (NEWID(), 'المكتبة', 1, 1, 3, @Building1Id), -- مكتبة، داخل المسجد، طابق واحد، مكتبة
    (NEWID(), 'غرف الإمام', 1, 1, 4, @Building1Id), -- غرف الإمام، داخل المسجد، طابق واحد، غرف إمام
    (NEWID(), 'دورات المياه', 1, 1, 5, @Building1Id), -- دورات المياه، داخل المسجد، طابق واحد، دورات مياه
    (NEWID(), 'مرافق الوضوء', 1, 1, 9, @Building1Id), -- مرافق الوضوء، داخل المسجد، طابق واحد، مرافق وضوء
    (NEWID(), 'المطبخ', 1, 1, 6, @Building1Id), -- مطبخ، داخل المسجد، طابق واحد، مطبخ
    (NEWID(), 'المخزن', 1, 1, 7, @Building1Id), -- مخزن، داخل المسجد، طابق واحد، مخزن
    (NEWID(), 'الساحة الخارجية', 0, 0, 8, @Building1Id), -- ساحة خارجية، خارج المسجد، لا طوابق، ساحة

    -- مسجد أحمد باشا
    (NEWID(), 'المصلى الرئيسي', 1, 1, 1, @Building2Id),
    (NEWID(), 'المئذنة', 1, 1, 2, @Building2Id),
    (NEWID(), 'المكتبة', 1, 1, 3, @Building2Id),
    (NEWID(), 'غرف الإمام', 1, 1, 4, @Building2Id),
    (NEWID(), 'دورات المياه', 1, 1, 5, @Building2Id),
    (NEWID(), 'مرافق الوضوء', 1, 1, 9, @Building2Id),
    (NEWID(), 'الساحة الخارجية', 0, 0, 8, @Building2Id),

    -- مسجد درغوت باشا
    (NEWID(), 'المصلى الرئيسي', 1, 1, 1, @Building3Id),
    (NEWID(), 'المئذنة', 1, 1, 2, @Building3Id),
    (NEWID(), 'المكتبة', 1, 1, 3, @Building3Id),
    (NEWID(), 'غرف الإمام', 1, 1, 4, @Building3Id),
    (NEWID(), 'دورات المياه', 1, 1, 5, @Building3Id),
    (NEWID(), 'مرافق الوضوء', 1, 1, 9, @Building3Id),
    (NEWID(), 'الساحة الخارجية', 0, 0, 8, @Building3Id),

    -- مسجد سيدي عبد الواحد
    (NEWID(), 'المصلى الرئيسي', 1, 1, 1, @Building4Id),
    (NEWID(), 'المئذنة', 1, 1, 2, @Building4Id),
    (NEWID(), 'غرف الإمام', 1, 1, 4, @Building4Id),
    (NEWID(), 'دورات المياه', 1, 1, 5, @Building4Id),
    (NEWID(), 'مرافق الوضوء', 1, 1, 9, @Building4Id),
    (NEWID(), 'الساحة الخارجية', 0, 0, 8, @Building4Id),

    -- مسجد سيدي سالم
    (NEWID(), 'المصلى الرئيسي', 1, 1, 1, @Building5Id),
    (NEWID(), 'المئذنة', 1, 1, 2, @Building5Id),
    (NEWID(), 'غرف الإمام', 1, 1, 4, @Building5Id),
    (NEWID(), 'دورات المياه', 1, 1, 5, @Building5Id),
    (NEWID(), 'مرافق الوضوء', 1, 1, 9, @Building5Id),
    (NEWID(), 'الساحة الخارجية', 0, 0, 8, @Building5Id),

    -- مسجد الفتح الجديد (مسجد حديث)
    (NEWID(), 'المصلى الرئيسي', 1, 2, 1, @Building10Id),
    (NEWID(), 'المئذنة', 1, 1, 2, @Building10Id),
    (NEWID(), 'المكتبة', 1, 1, 3, @Building10Id),
    (NEWID(), 'غرف الإمام', 1, 1, 4, @Building10Id),
    (NEWID(), 'دورات المياه', 1, 1, 5, @Building10Id),
    (NEWID(), 'مرافق الوضوء', 1, 1, 9, @Building10Id),
    (NEWID(), 'المطبخ', 1, 1, 6, @Building10Id),
    (NEWID(), 'المخزن', 1, 1, 7, @Building10Id),
    (NEWID(), 'قاعة اجتماعات', 1, 1, 10, @Building10Id), -- قاعة اجتماعات، داخل المسجد، طابق واحد، قاعة اجتماعات
    (NEWID(), 'غرف إدارية', 1, 1, 11, @Building10Id), -- غرف إدارية، داخل المسجد، طابق واحد، غرف إدارية
    (NEWID(), 'الساحة الخارجية', 0, 0, 8, @Building10Id)

    PRINT 'تم إضافة بيانات البنايات التفصيلية بنجاح!'
END TRY
BEGIN CATCH
    PRINT 'خطأ في إضافة بيانات البنايات التفصيلية: ' + ERROR_MESSAGE()
END CATCH

-- ===== إضافة بيانات المرافق التفصيلية (المواد المرتبطة) =====
PRINT 'إضافة بيانات المرافق التفصيلية (المواد)...'
BEGIN TRY
    -- الحصول على BuildingDetailIds
    DECLARE @MainPrayerHall1Id uniqueidentifier = (SELECT TOP 1 Id FROM BuildingDetails WHERE Name = 'المصلى الرئيسي' AND BuildingId = @Building1Id)
    DECLARE @Minaret1Id uniqueidentifier = (SELECT TOP 1 Id FROM BuildingDetails WHERE Name = 'المئذنة' AND BuildingId = @Building1Id)
    DECLARE @Library1Id uniqueidentifier = (SELECT TOP 1 Id FROM BuildingDetails WHERE Name = 'المكتبة' AND BuildingId = @Building1Id)
    DECLARE @ImamRooms1Id uniqueidentifier = (SELECT TOP 1 Id FROM BuildingDetails WHERE Name = 'غرف الإمام' AND BuildingId = @Building1Id)
    DECLARE @Toilets1Id uniqueidentifier = (SELECT TOP 1 Id FROM BuildingDetails WHERE Name = 'دورات المياه' AND BuildingId = @Building1Id)
    DECLARE @WuduFacilities1Id uniqueidentifier = (SELECT TOP 1 Id FROM BuildingDetails WHERE Name = 'مرافق الوضوء' AND BuildingId = @Building1Id)

    -- الحصول على ProductIds
    DECLARE @CementId uniqueidentifier = (SELECT TOP 1 Id FROM Products WHERE Name = 'أسمنت بورتلاند')
    DECLARE @SteelId uniqueidentifier = (SELECT TOP 1 Id FROM Products WHERE Name = 'حديد تسليح')
    DECLARE @BricksId uniqueidentifier = (SELECT TOP 1 Id FROM Products WHERE Name = 'طوب أحمر')
    DECLARE @PaintId uniqueidentifier = (SELECT TOP 1 Id FROM Products WHERE Name = 'ألوان دهان')
    DECLARE @CarpetId uniqueidentifier = (SELECT TOP 1 Id FROM Products WHERE Name = 'سجاد مصلى')
    DECLARE @LightingId uniqueidentifier = (SELECT TOP 1 Id FROM Products WHERE Name = 'مصابيح LED')
    DECLARE @FurnitureId uniqueidentifier = (SELECT TOP 1 Id FROM Products WHERE Name = 'طاولات مكتب')
    DECLARE @BooksId uniqueidentifier = (SELECT TOP 1 Id FROM Products WHERE Name = 'كتب دينية')
    DECLARE @TapsId uniqueidentifier = (SELECT TOP 1 Id FROM Products WHERE Name = 'حنفيات مياه')
    DECLARE @ToiletsProductId uniqueidentifier = (SELECT TOP 1 Id FROM Products WHERE Name = 'مراحيض')
    DECLARE @WuduBasinsId uniqueidentifier = (SELECT TOP 1 Id FROM Products WHERE Name = 'أحواض وضوء')
    DECLARE @ChandeliersId uniqueidentifier = (SELECT TOP 1 Id FROM Products WHERE Name = 'ثريات مساجد')
    DECLARE @MinaretLightsId uniqueidentifier = (SELECT TOP 1 Id FROM Products WHERE Name = 'مصابيح مئذنة')
    DECLARE @SpeakersId uniqueidentifier = (SELECT TOP 1 Id FROM Products WHERE Name = 'مكبرات صوت مساجد')
    DECLARE @MicrophonesId uniqueidentifier = (SELECT TOP 1 Id FROM Products WHERE Name = 'ميكروفونات إمام')
    DECLARE @QuranBooksId uniqueidentifier = (SELECT TOP 1 Id FROM Products WHERE Name = 'مصاحف شريفة')
    DECLARE @MosqueClocksId uniqueidentifier = (SELECT TOP 1 Id FROM Products WHERE Name = 'ساعات مساجد')
    DECLARE @PrayerTimingId uniqueidentifier = (SELECT TOP 1 Id FROM Products WHERE Name = 'أنظمة توقيت صلاة')
    DECLARE @FireExtinguishersId uniqueidentifier = (SELECT TOP 1 Id FROM Products WHERE Name = 'طفايات حريق مساجد')
    DECLARE @WaterCoolersId uniqueidentifier = (SELECT TOP 1 Id FROM Products WHERE Name = 'مبردات مياه مساجد')
    DECLARE @IslamicDecorId uniqueidentifier = (SELECT TOP 1 Id FROM Products WHERE Name = 'زخارف إسلامية')
    DECLARE @QuranVersesId uniqueidentifier = (SELECT TOP 1 Id FROM Products WHERE Name = 'آيات قرآنية')

    -- إذا لم توجد بعض المنتجات، نستخدم منتجات موجودة
    IF @CarpetId IS NULL SET @CarpetId = @PaintId
    IF @BooksId IS NULL SET @BooksId = @FurnitureId
    IF @ToiletsProductId IS NULL SET @ToiletsProductId = @TapsId
    IF @WuduBasinsId IS NULL SET @WuduBasinsId = @TapsId
    IF @ChandeliersId IS NULL SET @ChandeliersId = @LightingId
    IF @MinaretLightsId IS NULL SET @MinaretLightsId = @LightingId
    IF @SpeakersId IS NULL SET @SpeakersId = @FurnitureId
    IF @MicrophonesId IS NULL SET @MicrophonesId = @FurnitureId
    IF @QuranBooksId IS NULL SET @QuranBooksId = @BooksId
    IF @MosqueClocksId IS NULL SET @MosqueClocksId = @FurnitureId
    IF @PrayerTimingId IS NULL SET @PrayerTimingId = @FurnitureId
    IF @FireExtinguishersId IS NULL SET @FireExtinguishersId = @FurnitureId
    IF @WaterCoolersId IS NULL SET @WaterCoolersId = @TapsId
    IF @IslamicDecorId IS NULL SET @IslamicDecorId = @PaintId
    IF @QuranVersesId IS NULL SET @QuranVersesId = @BooksId

    INSERT INTO FacilityDetails (Id, Quantity, ProductId, BuildingDetailId) VALUES
    -- المصلى الرئيسي - مسجد النصر
    (NEWID(), 50, @CementId, @MainPrayerHall1Id), -- 50 كيس أسمنت
    (NEWID(), 2000, @SteelId, @MainPrayerHall1Id), -- 2000 كجم حديد
    (NEWID(), 10000, @BricksId, @MainPrayerHall1Id), -- 10000 طوبة
    (NEWID(), 100, @PaintId, @MainPrayerHall1Id), -- 100 لتر دهان
    (NEWID(), 500, @CarpetId, @MainPrayerHall1Id), -- 500 متر مربع سجاد
    (NEWID(), 50, @LightingId, @MainPrayerHall1Id), -- 50 مصباح LED
    (NEWID(), 5, @ChandeliersId, @MainPrayerHall1Id), -- 5 ثريات
    (NEWID(), 10, @SpeakersId, @MainPrayerHall1Id), -- 10 مكبرات صوت
    (NEWID(), 2, @MicrophonesId, @MainPrayerHall1Id), -- 2 ميكروفون
    (NEWID(), 20, @IslamicDecorId, @MainPrayerHall1Id), -- 20 زخرفة إسلامية
    (NEWID(), 10, @QuranVersesId, @MainPrayerHall1Id), -- 10 آية قرآنية

    -- المئذنة - مسجد النصر
    (NEWID(), 20, @CementId, @Minaret1Id), -- 20 كيس أسمنت
    (NEWID(), 500, @SteelId, @Minaret1Id), -- 500 كجم حديد
    (NEWID(), 2000, @BricksId, @Minaret1Id), -- 2000 طوبة
    (NEWID(), 20, @PaintId, @Minaret1Id), -- 20 لتر دهان
    (NEWID(), 10, @MinaretLightsId, @Minaret1Id), -- 10 مصباح مئذنة

    -- المكتبة - مسجد النصر
    (NEWID(), 10, @FurnitureId, @Library1Id), -- 10 طاولات
    (NEWID(), 1000, @BooksId, @Library1Id), -- 1000 كتاب
    (NEWID(), 20, @LightingId, @Library1Id), -- 20 مصباح LED
    (NEWID(), 50, @QuranBooksId, @Library1Id), -- 50 مصحف

    -- غرف الإمام - مسجد النصر
    (NEWID(), 5, @FurnitureId, @ImamRooms1Id), -- 5 طاولات
    (NEWID(), 10, @LightingId, @ImamRooms1Id), -- 10 مصابيح LED
    (NEWID(), 50, @PaintId, @ImamRooms1Id), -- 50 لتر دهان
    (NEWID(), 1, @MosqueClocksId, @ImamRooms1Id), -- 1 ساعة مسجد
    (NEWID(), 1, @PrayerTimingId, @ImamRooms1Id), -- 1 نظام توقيت صلاة

    -- دورات المياه - مسجد النصر
    (NEWID(), 10, @ToiletsProductId, @Toilets1Id), -- 10 مرحاض
    (NEWID(), 15, @TapsId, @Toilets1Id), -- 15 حنفية
    (NEWID(), 30, @PaintId, @Toilets1Id), -- 30 لتر دهان
    (NEWID(), 5, @FireExtinguishersId, @Toilets1Id), -- 5 طفاية حريق

    -- مرافق الوضوء - مسجد النصر
    (NEWID(), 20, @WuduBasinsId, @WuduFacilities1Id), -- 20 حوض وضوء
    (NEWID(), 25, @TapsId, @WuduFacilities1Id), -- 25 حنفية
    (NEWID(), 40, @PaintId, @WuduFacilities1Id), -- 40 لتر دهان
    (NEWID(), 2, @WaterCoolersId, @WuduFacilities1Id) -- 2 مبرد مياه

    PRINT 'تم إضافة بيانات المرافق التفصيلية بنجاح!'
END TRY
BEGIN CATCH
    PRINT 'خطأ في إضافة بيانات المرافق التفصيلية: ' + ERROR_MESSAGE()
END CATCH

-- ===== عرض إحصائيات شاملة =====
PRINT '=== إحصائيات شاملة للبيانات المضافة ==='
DECLARE @MosqueBuildingsCount int = (SELECT COUNT(*) FROM Buildings WHERE Name LIKE '%مسجد%')
DECLARE @MosquesCount int = (SELECT COUNT(*) FROM Mosques)
DECLARE @BuildingDetailsCount int = (SELECT COUNT(*) FROM BuildingDetails)
DECLARE @FacilityDetailsCount int = (SELECT COUNT(*) FROM FacilityDetails)
DECLARE @ProductsCount int = (SELECT COUNT(*) FROM Products)
DECLARE @MosqueProductsCount int = (SELECT COUNT(*) FROM Products WHERE Name LIKE '%مسجد%' OR Name LIKE '%مصلى%' OR Name LIKE '%إمام%' OR Name LIKE '%وضوء%' OR Name LIKE '%سجاد%' OR Name LIKE '%ثريات%' OR Name LIKE '%مئذنة%')

PRINT 'عدد البنايات (المساجد): ' + CAST(@MosqueBuildingsCount AS VARCHAR(10))
PRINT 'عدد المساجد: ' + CAST(@MosquesCount AS VARCHAR(10))
PRINT 'عدد البنايات التفصيلية: ' + CAST(@BuildingDetailsCount AS VARCHAR(10))
PRINT 'عدد المرافق التفصيلية: ' + CAST(@FacilityDetailsCount AS VARCHAR(10))
PRINT 'إجمالي عدد المنتجات: ' + CAST(@ProductsCount AS VARCHAR(10))
PRINT 'عدد منتجات المساجد: ' + CAST(@MosqueProductsCount AS VARCHAR(10))

PRINT ''
PRINT '=== تفاصيل المساجد المضافة ==='
SELECT 
    m.Id as MosqueId,
    b.Name as MosqueName,
    b.FileNumber,
    b.PrayerCapacity,
    b.TotalLandArea,
    b.TotalCoveredArea,
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
ORDER BY b.Name

PRINT ''
PRINT '=== تفاصيل البنايات التفصيلية ==='
SELECT 
    bd.Id as BuildingDetailId,
    b.Name as MosqueName,
    bd.Name as BuildingDetailName,
    CASE bd.BuildingCategory
        WHEN 1 THEN 'مصلى'
        WHEN 2 THEN 'مئذنة'
        WHEN 3 THEN 'مكتبة'
        WHEN 4 THEN 'غرف إمام'
        WHEN 5 THEN 'دورات مياه'
        WHEN 6 THEN 'مطبخ'
        WHEN 7 THEN 'مخزن'
        WHEN 8 THEN 'ساحة'
        WHEN 9 THEN 'مرافق وضوء'
        WHEN 10 THEN 'قاعة اجتماعات'
        WHEN 11 THEN 'غرف إدارية'
        ELSE 'غير محدد'
    END as Category,
    bd.Floors as NumberOfFloors,
    CASE bd.WithinMosqueArea
        WHEN 1 THEN 'داخل المسجد'
        WHEN 0 THEN 'خارج المسجد'
    END as Location
FROM BuildingDetails bd
INNER JOIN Buildings b ON bd.BuildingId = b.Id
ORDER BY b.Name, bd.Name

PRINT ''
PRINT '=== تفاصيل المرافق والمواد ==='
SELECT 
    fd.Id as FacilityDetailId,
    b.Name as MosqueName,
    bd.Name as BuildingDetailName,
    p.Name as ProductName,
    fd.Quantity,
    p.Description as ProductDescription
FROM FacilityDetails fd
INNER JOIN BuildingDetails bd ON fd.BuildingDetailId = bd.Id
INNER JOIN Buildings b ON bd.BuildingId = b.Id
INNER JOIN Products p ON fd.ProductId = p.Id
ORDER BY b.Name, bd.Name, p.Name

PRINT ''
PRINT 'تم إضافة جميع بيانات المساجد والبنايات والمواد بنجاح! 🕌✨'
PRINT 'يمكنك الآن استخدام النظام لإدارة المساجد والبنايات والمواد المرتبطة بها.' 