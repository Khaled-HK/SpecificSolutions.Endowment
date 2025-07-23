-- ملف إضافة بيانات المساجد والبنايات والمواد المرتبطة
-- قم بتشغيل هذا الملف بعد تشغيل run_all_seed_data.sql

PRINT 'بدء إضافة بيانات المساجد والبنايات والمواد المرتبطة...'

-- التحقق من وجود البيانات الأساسية
IF NOT EXISTS (SELECT * FROM Cities WHERE Name = 'طرابلس')
BEGIN
    PRINT 'خطأ: يجب تشغيل run_all_seed_data.sql أولاً!'
    RETURN
END

-- إضافة بيانات البنايات (المساجد)
PRINT 'إضافة بيانات البنايات (المساجد)...'
BEGIN TRY
    -- الحصول على OfficeId و RegionId
    DECLARE @MainOfficeId uniqueidentifier = (SELECT TOP 1 Id FROM Offices WHERE Name = 'Main Office')
    DECLARE @TripoliRegionId uniqueidentifier = (SELECT TOP 1 Id FROM Regions WHERE Name = 'وسط طرابلس')
    DECLARE @AdminUserId nvarchar(450) = (SELECT TOP 1 Id FROM AspNetUsers WHERE UserName = 'admin@demo.com')

    -- إذا لم توجد البيانات الأساسية، نستخدم قيم افتراضية
    IF @MainOfficeId IS NULL SET @MainOfficeId = 'DDEC6E9E-7628-4623-9A94-4E4EFC02187C'
    IF @TripoliRegionId IS NULL SET @TripoliRegionId = 'DDEC6E9E-7698-4623-9A84-4E5EFC02187C'
    IF @AdminUserId IS NULL SET @AdminUserId = 'a3d890d8-01d1-444b-9f62-6336b937e5fc'

    INSERT INTO Buildings (Id, Name, FileNumber, Definition, Classification, OfficeId, Unit, RegionId, NearestLandmark, ConstructionDate, OpeningDate, MapLocation, TotalLandArea, TotalCoveredArea, NumberOfFloors, ElectricityMeter, AlternativeEnergySource, WaterSource, Sanitation, BriefDescription, LandDonorName, SourceFunds, PrayerCapacity, UserId, ServicesSpecialNeeds, SpecialEntranceWomen, PicturePath) VALUES
    -- مساجد طرابلس
    (NEWID(), 'مسجد النصر', 'MOS-001', 'مسجد جامع', 'مسجد حكومي', @MainOfficeId, 'وحدة إدارية', @TripoliRegionId, 'وسط المدينة', '1980-01-01', '1982-01-01', '32.8872,13.1913', 2500.0, 1800.0, 2, 'ELEC-001', 'طاقة شمسية', 'شبكة مياه', 'شبكة صرف', 'مسجد النصر التاريخي في وسط طرابلس', 'وقف النصر', 1, '500 مصلي', @AdminUserId, 1, 1, '/images/mosques/nasr.jpg'),
    (NEWID(), 'مسجد أحمد باشا', 'MOS-002', 'مسجد جامع', 'مسجد تراثي', @MainOfficeId, 'وحدة إدارية', @TripoliRegionId, 'القلعة القديمة', '1736-01-01', '1738-01-01', '32.8875,13.1918', 1200.0, 800.0, 1, 'ELEC-002', 'لا يوجد', 'بئر تقليدي', 'صرف صحي', 'مسجد أحمد باشا التراثي', 'أحمد باشا القرمانلي', 2, '200 مصلي', @AdminUserId, 0, 0, '/images/mosques/ahmed_pasha.jpg'),
    (NEWID(), 'مسجد درغوت باشا', 'MOS-003', 'مسجد جامع', 'مسجد تراثي', @MainOfficeId, 'وحدة إدارية', @TripoliRegionId, 'المدينة القديمة', '1560-01-01', '1560-01-01', '32.8870,13.1915', 1500.0, 1000.0, 1, 'ELEC-003', 'لا يوجد', 'بئر تقليدي', 'صرف صحي', 'مسجد درغوت باشا التاريخي', 'درغوت باشا', 2, '300 مصلي', @AdminUserId, 0, 0, '/images/mosques/draghut.jpg'),
    (NEWID(), 'مسجد سيدي عبد الواحد', 'MOS-004', 'مسجد جامع', 'مسجد تراثي', @MainOfficeId, 'وحدة إدارية', @TripoliRegionId, 'حي سيدي المصري', '1600-01-01', '1600-01-01', '32.8865,13.1920', 800.0, 600.0, 1, 'ELEC-004', 'لا يوجد', 'بئر تقليدي', 'صرف صحي', 'مسجد سيدي عبد الواحد', 'وقف إسلامي', 2, '150 مصلي', @AdminUserId, 0, 0, '/images/mosques/sidi_abdulwahid.jpg'),
    (NEWID(), 'مسجد سيدي سالم', 'MOS-005', 'مسجد جامع', 'مسجد تراثي', @MainOfficeId, 'وحدة إدارية', @TripoliRegionId, 'حي سيدي سالم', '1700-01-01', '1700-01-01', '32.8868,13.1910', 1000.0, 700.0, 1, 'ELEC-005', 'لا يوجد', 'بئر تقليدي', 'صرف صحي', 'مسجد سيدي سالم', 'وقف إسلامي', 2, '180 مصلي', @AdminUserId, 0, 0, '/images/mosques/sidi_salem.jpg'),
    (NEWID(), 'مسجد العتيق', 'MOS-006', 'مسجد جامع', 'مسجد تراثي', @MainOfficeId, 'وحدة إدارية', @TripoliRegionId, 'المدينة القديمة', '1400-01-01', '1400-01-01', '32.8873,13.1917', 600.0, 400.0, 1, 'ELEC-006', 'لا يوجد', 'بئر تقليدي', 'صرف صحي', 'مسجد العتيق التاريخي', 'وقف إسلامي', 2, '100 مصلي', @AdminUserId, 0, 0, '/images/mosques/ateeq.jpg'),
    (NEWID(), 'مسجد سيدي خريبيش', 'MOS-007', 'مسجد جامع', 'مسجد تراثي', @MainOfficeId, 'وحدة إدارية', @TripoliRegionId, 'حي سيدي خريبيش', '1800-01-01', '1800-01-01', '32.8860,13.1925', 900.0, 650.0, 1, 'ELEC-007', 'لا يوجد', 'بئر تقليدي', 'صرف صحي', 'مسجد سيدي خريبيش', 'وقف إسلامي', 2, '120 مصلي', @AdminUserId, 0, 0, '/images/mosques/sidi_khreibish.jpg'),
    (NEWID(), 'مسجد سيدي عبد السلام', 'MOS-008', 'مسجد جامع', 'مسجد تراثي', @MainOfficeId, 'وحدة إدارية', @TripoliRegionId, 'حي سيدي عبد السلام', '1750-01-01', '1750-01-01', '32.8862,13.1912', 1100.0, 750.0, 1, 'ELEC-008', 'لا يوجد', 'بئر تقليدي', 'صرف صحي', 'مسجد سيدي عبد السلام', 'وقف إسلامي', 2, '160 مصلي', @AdminUserId, 0, 0, '/images/mosques/sidi_abdulsalam.jpg'),
    (NEWID(), 'مسجد سيدي محمد', 'MOS-009', 'مسجد جامع', 'مسجد تراثي', @MainOfficeId, 'وحدة إدارية', @TripoliRegionId, 'حي سيدي محمد', '1650-01-01', '1650-01-01', '32.8867,13.1918', 950.0, 680.0, 1, 'ELEC-009', 'لا يوجد', 'بئر تقليدي', 'صرف صحي', 'مسجد سيدي محمد', 'وقف إسلامي', 2, '140 مصلي', @AdminUserId, 0, 0, '/images/mosques/sidi_mohammed.jpg'),
    (NEWID(), 'مسجد سيدي علي', 'MOS-010', 'مسجد جامع', 'مسجد تراثي', @MainOfficeId, 'وحدة إدارية', @TripoliRegionId, 'حي سيدي علي', '1700-01-01', '1700-01-01', '32.8864,13.1916', 850.0, 600.0, 1, 'ELEC-010', 'لا يوجد', 'بئر تقليدي', 'صرف صحي', 'مسجد سيدي علي', 'وقف إسلامي', 2, '110 مصلي', @AdminUserId, 0, 0, '/images/mosques/sidi_ali.jpg')

    PRINT 'تم إضافة بيانات البنايات (المساجد) بنجاح!'
END TRY
BEGIN CATCH
    PRINT 'خطأ في إضافة بيانات البنايات: ' + ERROR_MESSAGE()
END CATCH

-- إضافة بيانات المساجد
PRINT 'إضافة بيانات المساجد...'
BEGIN TRY
    -- الحصول على BuildingIds
    DECLARE @Mosque1Id uniqueidentifier = (SELECT TOP 1 Id FROM Buildings WHERE Name = 'مسجد النصر')
    DECLARE @Mosque2Id uniqueidentifier = (SELECT TOP 1 Id FROM Buildings WHERE Name = 'مسجد أحمد باشا')
    DECLARE @Mosque3Id uniqueidentifier = (SELECT TOP 1 Id FROM Buildings WHERE Name = 'مسجد درغوت باشا')
    DECLARE @Mosque4Id uniqueidentifier = (SELECT TOP 1 Id FROM Buildings WHERE Name = 'مسجد سيدي عبد الواحد')
    DECLARE @Mosque5Id uniqueidentifier = (SELECT TOP 1 Id FROM Buildings WHERE Name = 'مسجد سيدي سالم')

    INSERT INTO Mosques (Id, BuildingId, MosqueDefinition, MosqueClassification) VALUES
    (NEWID(), @Mosque1Id, 1, 1), -- مسجد جامع، مسجد حكومي
    (NEWID(), @Mosque2Id, 1, 2), -- مسجد جامع، مسجد تراثي
    (NEWID(), @Mosque3Id, 1, 2), -- مسجد جامع، مسجد تراثي
    (NEWID(), @Mosque4Id, 1, 2), -- مسجد جامع، مسجد تراثي
    (NEWID(), @Mosque5Id, 1, 2)  -- مسجد جامع، مسجد تراثي

    PRINT 'تم إضافة بيانات المساجد بنجاح!'
END TRY
BEGIN CATCH
    PRINT 'خطأ في إضافة بيانات المساجد: ' + ERROR_MESSAGE()
END CATCH

-- إضافة بيانات البنايات التفصيلية
PRINT 'إضافة بيانات البنايات التفصيلية...'
BEGIN TRY
    -- الحصول على BuildingIds
    DECLARE @Building1Id uniqueidentifier = (SELECT TOP 1 Id FROM Buildings WHERE Name = 'مسجد النصر')
    DECLARE @Building2Id uniqueidentifier = (SELECT TOP 1 Id FROM Buildings WHERE Name = 'مسجد أحمد باشا')
    DECLARE @Building3Id uniqueidentifier = (SELECT TOP 1 Id FROM Buildings WHERE Name = 'مسجد درغوت باشا')

    INSERT INTO BuildingDetails (Id, Name, WithinMosqueArea, Floors, BuildingCategory, BuildingId) VALUES
    -- مسجد النصر
    (NEWID(), 'المصلى الرئيسي', 1, 1, 1, @Building1Id), -- مصلى، داخل المسجد، طابق واحد، مصلى
    (NEWID(), 'المئذنة', 1, 1, 2, @Building1Id), -- مئذنة، داخل المسجد، طابق واحد، مئذنة
    (NEWID(), 'المكتبة', 1, 1, 3, @Building1Id), -- مكتبة، داخل المسجد، طابق واحد، مكتبة
    (NEWID(), 'غرف الإمام', 1, 1, 4, @Building1Id), -- غرف الإمام، داخل المسجد، طابق واحد، غرف إمام
    (NEWID(), 'دورات المياه', 1, 1, 5, @Building1Id), -- دورات المياه، داخل المسجد، طابق واحد، دورات مياه
    (NEWID(), 'المطبخ', 1, 1, 6, @Building1Id), -- مطبخ، داخل المسجد، طابق واحد، مطبخ
    (NEWID(), 'المخزن', 1, 1, 7, @Building1Id), -- مخزن، داخل المسجد، طابق واحد، مخزن
    (NEWID(), 'الساحة الخارجية', 0, 0, 8, @Building1Id), -- ساحة خارجية، خارج المسجد، لا طوابق، ساحة

    -- مسجد أحمد باشا
    (NEWID(), 'المصلى الرئيسي', 1, 1, 1, @Building2Id),
    (NEWID(), 'المئذنة', 1, 1, 2, @Building2Id),
    (NEWID(), 'المكتبة', 1, 1, 3, @Building2Id),
    (NEWID(), 'غرف الإمام', 1, 1, 4, @Building2Id),
    (NEWID(), 'دورات المياه', 1, 1, 5, @Building2Id),
    (NEWID(), 'الساحة الخارجية', 0, 0, 8, @Building2Id),

    -- مسجد درغوت باشا
    (NEWID(), 'المصلى الرئيسي', 1, 1, 1, @Building3Id),
    (NEWID(), 'المئذنة', 1, 1, 2, @Building3Id),
    (NEWID(), 'المكتبة', 1, 1, 3, @Building3Id),
    (NEWID(), 'غرف الإمام', 1, 1, 4, @Building3Id),
    (NEWID(), 'دورات المياه', 1, 1, 5, @Building3Id),
    (NEWID(), 'الساحة الخارجية', 0, 0, 8, @Building3Id)

    PRINT 'تم إضافة بيانات البنايات التفصيلية بنجاح!'
END TRY
BEGIN CATCH
    PRINT 'خطأ في إضافة بيانات البنايات التفصيلية: ' + ERROR_MESSAGE()
END CATCH

-- إضافة بيانات المرافق التفصيلية (المواد المرتبطة)
PRINT 'إضافة بيانات المرافق التفصيلية (المواد)...'
BEGIN TRY
    -- الحصول على BuildingDetailIds و ProductIds
    DECLARE @MainPrayerHallId uniqueidentifier = (SELECT TOP 1 Id FROM BuildingDetails WHERE Name = 'المصلى الرئيسي' AND BuildingId = @Building1Id)
    DECLARE @MinaretId uniqueidentifier = (SELECT TOP 1 Id FROM BuildingDetails WHERE Name = 'المئذنة' AND BuildingId = @Building1Id)
    DECLARE @LibraryId uniqueidentifier = (SELECT TOP 1 Id FROM BuildingDetails WHERE Name = 'المكتبة' AND BuildingId = @Building1Id)
    DECLARE @ImamRoomsId uniqueidentifier = (SELECT TOP 1 Id FROM BuildingDetails WHERE Name = 'غرف الإمام' AND BuildingId = @Building1Id)
    DECLARE @ToiletsId uniqueidentifier = (SELECT TOP 1 Id FROM BuildingDetails WHERE Name = 'دورات المياه' AND BuildingId = @Building1Id)

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

    -- إذا لم توجد بعض المنتجات، نستخدم منتجات موجودة
    IF @CarpetId IS NULL SET @CarpetId = @PaintId
    IF @BooksId IS NULL SET @BooksId = @FurnitureId
    IF @ToiletsProductId IS NULL SET @ToiletsProductId = @TapsId

    INSERT INTO FacilityDetails (Id, Quantity, ProductId, BuildingDetailId) VALUES
    -- المصلى الرئيسي - مسجد النصر
    (NEWID(), 50, @CementId, @MainPrayerHallId), -- 50 كيس أسمنت
    (NEWID(), 2000, @SteelId, @MainPrayerHallId), -- 2000 كجم حديد
    (NEWID(), 10000, @BricksId, @MainPrayerHallId), -- 10000 طوبة
    (NEWID(), 100, @PaintId, @MainPrayerHallId), -- 100 لتر دهان
    (NEWID(), 500, @CarpetId, @MainPrayerHallId), -- 500 متر مربع سجاد
    (NEWID(), 50, @LightingId, @MainPrayerHallId), -- 50 مصباح LED

    -- المئذنة - مسجد النصر
    (NEWID(), 20, @CementId, @MinaretId), -- 20 كيس أسمنت
    (NEWID(), 500, @SteelId, @MinaretId), -- 500 كجم حديد
    (NEWID(), 2000, @BricksId, @MinaretId), -- 2000 طوبة
    (NEWID(), 20, @PaintId, @MinaretId), -- 20 لتر دهان

    -- المكتبة - مسجد النصر
    (NEWID(), 10, @FurnitureId, @LibraryId), -- 10 طاولات
    (NEWID(), 1000, @BooksId, @LibraryId), -- 1000 كتاب
    (NEWID(), 20, @LightingId, @LibraryId), -- 20 مصباح LED

    -- غرف الإمام - مسجد النصر
    (NEWID(), 5, @FurnitureId, @ImamRoomsId), -- 5 طاولات
    (NEWID(), 10, @LightingId, @ImamRoomsId), -- 10 مصابيح LED
    (NEWID(), 50, @PaintId, @ImamRoomsId), -- 50 لتر دهان

    -- دورات المياه - مسجد النصر
    (NEWID(), 10, @ToiletsProductId, @ToiletsId), -- 10 مرحاض
    (NEWID(), 15, @TapsId, @ToiletsId), -- 15 حنفية
    (NEWID(), 30, @PaintId, @ToiletsId) -- 30 لتر دهان

    PRINT 'تم إضافة بيانات المرافق التفصيلية بنجاح!'
END TRY
BEGIN CATCH
    PRINT 'خطأ في إضافة بيانات المرافق التفصيلية: ' + ERROR_MESSAGE()
END CATCH

-- عرض إحصائيات البيانات المضافة
PRINT '=== إحصائيات البيانات المضافة ==='
PRINT 'عدد البنايات (المساجد): ' + CAST((SELECT COUNT(*) FROM Buildings WHERE Name LIKE '%مسجد%') AS VARCHAR(10))
PRINT 'عدد المساجد: ' + CAST((SELECT COUNT(*) FROM Mosques) AS VARCHAR(10))
PRINT 'عدد البنايات التفصيلية: ' + CAST((SELECT COUNT(*) FROM BuildingDetails) AS VARCHAR(10))
PRINT 'عدد المرافق التفصيلية: ' + CAST((SELECT COUNT(*) FROM FacilityDetails) AS VARCHAR(10))

PRINT 'تم إضافة جميع بيانات المساجد والبنايات والمواد بنجاح! 🕌' 