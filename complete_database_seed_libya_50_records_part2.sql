-- الجزء الثاني من ملف إدراج البيانات الليبية
-- استكمال باقي الجداول بـ 50 سجل لكل جدول
-- يجب تشغيل الجزء الأول أولاً

SET QUOTED_IDENTIFIER ON;
SET ANSI_NULLS ON;
SET NOCOUNT ON;

PRINT N'=== بدء الجزء الثاني من إدراج البيانات ==='

-- ===== 7. إنشاء مستخدمين (Users) للمكاتب والبنايات =====
PRINT N'إنشاء بيانات المستخدمين...'
BEGIN TRY
    -- إدراج مستخدمين في جدول AspNetUsers إذا لم يكونوا موجودين
    IF NOT EXISTS (SELECT * FROM AspNetUsers WHERE Email = N'admin@libya-endowment.ly')
    BEGIN
        INSERT INTO AspNetUsers (Id, UserName, Email, EmailConfirmed, PhoneNumberConfirmed, TwoFactorEnabled, LockoutEnabled, AccessFailedCount)
        VALUES 
        (N'admin-libya-001', N'admin@libya-endowment.ly', N'admin@libya-endowment.ly', 1, 0, 0, 1, 0),
        (N'manager-tripoli-001', N'manager.tripoli@libya-endowment.ly', N'manager.tripoli@libya-endowment.ly', 1, 0, 0, 1, 0),
        (N'manager-benghazi-001', N'manager.benghazi@libya-endowment.ly', N'manager.benghazi@libya-endowment.ly', 1, 0, 0, 1, 0),
        (N'manager-misrata-001', N'manager.misrata@libya-endowment.ly', N'manager.misrata@libya-endowment.ly', 1, 0, 0, 1, 0),
        (N'manager-zawiya-001', N'manager.zawiya@libya-endowment.ly', N'manager.zawiya@libya-endowment.ly', 1, 0, 0, 1, 0),
        (N'supervisor-001', N'supervisor1@libya-endowment.ly', N'supervisor1@libya-endowment.ly', 1, 0, 0, 1, 0),
        (N'supervisor-002', N'supervisor2@libya-endowment.ly', N'supervisor2@libya-endowment.ly', 1, 0, 0, 1, 0),
        (N'supervisor-003', N'supervisor3@libya-endowment.ly', N'supervisor3@libya-endowment.ly', 1, 0, 0, 1, 0),
        (N'officer-001', N'officer1@libya-endowment.ly', N'officer1@libya-endowment.ly', 1, 0, 0, 1, 0),
        (N'officer-002', N'officer2@libya-endowment.ly', N'officer2@libya-endowment.ly', 1, 0, 0, 1, 0);
        
        PRINT N'تم إنشاء 10 مستخدمين في AspNetUsers';
    END
    ELSE
    BEGIN
        PRINT N'المستخدمون موجودون مسبقاً في AspNetUsers';
    END
END TRY
BEGIN CATCH
    PRINT N'خطأ في إنشاء المستخدمين: ' + ERROR_MESSAGE();
END CATCH

-- ===== 8. جدول المكاتب (Offices) - 50 مكتب =====
PRINT N'إدراج بيانات المكاتب...'
BEGIN TRY
    DELETE FROM Offices;

    -- الحصول على معرفات المناطق والمستخدمين
    DECLARE @AdminUserId NVARCHAR(450) = N'admin-libya-001';
    DECLARE @ManagerTripoliId NVARCHAR(450) = N'manager-tripoli-001';
    DECLARE @ManagerBenghaziId NVARCHAR(450) = N'manager-benghazi-001';
    DECLARE @ManagerMisrataId NVARCHAR(450) = N'manager-misrata-001';
    DECLARE @ManagerZawiyaId NVARCHAR(450) = N'manager-zawiya-001';

    -- معرفات المناطق
    DECLARE @TripoliRegionId UNIQUEIDENTIFIER = (SELECT TOP 1 Id FROM Regions WHERE Name = N'وسط طرابلس');
    DECLARE @BenghaziRegionId UNIQUEIDENTIFIER = (SELECT TOP 1 Id FROM Regions WHERE Name = N'وسط بنغازي');
    DECLARE @MisrataRegionId UNIQUEIDENTIFIER = (SELECT TOP 1 Id FROM Regions WHERE Name = N'وسط مصراتة');
    DECLARE @ZawiyaRegionId UNIQUEIDENTIFIER = (SELECT TOP 1 Id FROM Regions WHERE Name = N'وسط الزاوية');
    DECLARE @AlbaydaRegionId UNIQUEIDENTIFIER = (SELECT TOP 1 Id FROM Regions WHERE Name = N'وسط البيضاء');

    -- إذا لم نجد المناطق نستخدم أي منطقة متاحة
    IF @TripoliRegionId IS NULL SET @TripoliRegionId = (SELECT TOP 1 Id FROM Regions);
    IF @BenghaziRegionId IS NULL SET @BenghaziRegionId = @TripoliRegionId;
    IF @MisrataRegionId IS NULL SET @MisrataRegionId = @TripoliRegionId;
    IF @ZawiyaRegionId IS NULL SET @ZawiyaRegionId = @TripoliRegionId;
    IF @AlbaydaRegionId IS NULL SET @AlbaydaRegionId = @TripoliRegionId;

    INSERT INTO Offices (Id, Name, Location, PhoneNumber, RegionId, UserId) VALUES
    -- مكاتب طرابلس (20 مكتب)
    (NEWID(), N'المكتب الرئيسي طرابلس', N'وسط طرابلس - شارع عمر المختار', N'0218-91-2000001', @TripoliRegionId, @AdminUserId),
    (NEWID(), N'مكتب حي الأندلس', N'حي الأندلس - طرابلس', N'0218-91-2000002', @TripoliRegionId, @ManagerTripoliId),
    (NEWID(), N'مكتب حي النصر', N'حي النصر - طرابلس', N'0218-91-2000003', @TripoliRegionId, @ManagerTripoliId),
    (NEWID(), N'مكتب حي الهضبة', N'حي الهضبة - طرابلس', N'0218-91-2000004', @TripoliRegionId, @ManagerTripoliId),
    (NEWID(), N'مكتب سيدي المصري', N'حي سيدي المصري - طرابلس', N'0218-91-2000005', @TripoliRegionId, @ManagerTripoliId),
    (NEWID(), N'مكتب باب العزيزية', N'حي باب العزيزية - طرابلس', N'0218-91-2000006', @TripoliRegionId, @ManagerTripoliId),
    (NEWID(), N'مكتب تاجوراء', N'حي تاجوراء - طرابلس', N'0218-91-2000007', @TripoliRegionId, @ManagerTripoliId),
    (NEWID(), N'مكتب أبو سليم', N'حي أبو سليم - طرابلس', N'0218-91-2000008', @TripoliRegionId, @ManagerTripoliId),
    (NEWID(), N'مكتب عين زارة', N'حي عين زارة - طرابلس', N'0218-91-2000009', @TripoliRegionId, @ManagerTripoliId),
    (NEWID(), N'مكتب الدريبي', N'منطقة الدريبي - طرابلس', N'0218-91-2000010', @TripoliRegionId, @ManagerTripoliId),
    (NEWID(), N'مكتب الكريمية', N'منطقة الكريمية - طرابلس', N'0218-91-2000011', @TripoliRegionId, @ManagerTripoliId),
    (NEWID(), N'مكتب الغيران', N'منطقة الغيران - طرابلس', N'0218-91-2000012', @TripoliRegionId, @ManagerTripoliId),
    (NEWID(), N'مكتب القربولي', N'منطقة القربولي - طرابلس', N'0218-91-2000013', @TripoliRegionId, @ManagerTripoliId),
    (NEWID(), N'مكتب الحدائق', N'منطقة الحدائق - طرابلس', N'0218-91-2000014', @TripoliRegionId, @ManagerTripoliId),
    (NEWID(), N'مكتب الصياد', N'منطقة الصياد - طرابلس', N'0218-91-2000015', @TripoliRegionId, @ManagerTripoliId),
    (NEWID(), N'مكتب الزهراء', N'منطقة الزهراء - طرابلس', N'0218-91-2000016', @TripoliRegionId, @ManagerTripoliId),
    (NEWID(), N'مكتب الجفارة', N'منطقة الجفارة - طرابلس', N'0218-91-2000017', @TripoliRegionId, @ManagerTripoliId),
    (NEWID(), N'مكتب القادسية', N'منطقة القادسية - طرابلس', N'0218-91-2000018', @TripoliRegionId, @ManagerTripoliId),
    (NEWID(), N'مكتب شارع الجمهورية', N'شارع الجمهورية - طرابلس', N'0218-91-2000019', @TripoliRegionId, @ManagerTripoliId),
    (NEWID(), N'مكتب شارع الفتح', N'شارع الفتح - طرابلس', N'0218-91-2000020', @TripoliRegionId, @ManagerTripoliId),

    -- مكاتب بنغازي (15 مكتب)
    (NEWID(), N'المكتب الرئيسي بنغازي', N'وسط بنغازي - شارع جمال عبد الناصر', N'0218-61-3000001', @BenghaziRegionId, @ManagerBenghaziId),
    (NEWID(), N'مكتب الصابري', N'حي الصابري - بنغازي', N'0218-61-3000002', @BenghaziRegionId, @ManagerBenghaziId),
    (NEWID(), N'مكتب سيدي خريبيش', N'حي سيدي خريبيش - بنغازي', N'0218-61-3000003', @BenghaziRegionId, @ManagerBenghaziId),
    (NEWID(), N'مكتب الكويفية', N'حي الكويفية - بنغازي', N'0218-61-3000004', @BenghaziRegionId, @ManagerBenghaziId),
    (NEWID(), N'مكتب البركة', N'حي البركة - بنغازي', N'0218-61-3000005', @BenghaziRegionId, @ManagerBenghaziId),
    (NEWID(), N'مكتب سيدي حسين', N'حي سيدي حسين - بنغازي', N'0218-61-3000006', @BenghaziRegionId, @ManagerBenghaziId),
    (NEWID(), N'مكتب القوارشة', N'حي القوارشة - بنغازي', N'0218-61-3000007', @BenghaziRegionId, @ManagerBenghaziId),
    (NEWID(), N'مكتب الهواري', N'حي الهواري - بنغازي', N'0218-61-3000008', @BenghaziRegionId, @ManagerBenghaziId),
    (NEWID(), N'مكتب الجواري', N'حي الجواري - بنغازي', N'0218-61-3000009', @BenghaziRegionId, @ManagerBenghaziId),
    (NEWID(), N'مكتب المقرون', N'حي المقرون - بنغازي', N'0218-61-3000010', @BenghaziRegionId, @ManagerBenghaziId),
    (NEWID(), N'مكتب شارع الثقافة', N'شارع الثقافة - بنغازي', N'0218-61-3000011', @BenghaziRegionId, @ManagerBenghaziId),
    (NEWID(), N'مكتب طريق الشاطئ', N'طريق الشاطئ - بنغازي', N'0218-61-3000012', @BenghaziRegionId, @ManagerBenghaziId),
    (NEWID(), N'مكتب بوعطني', N'منطقة بوعطني - بنغازي', N'0218-61-3000013', @BenghaziRegionId, @ManagerBenghaziId),
    (NEWID(), N'مكتب الليثي', N'منطقة الليثي - بنغازي', N'0218-61-3000014', @BenghaziRegionId, @ManagerBenghaziId),
    (NEWID(), N'مكتب القواريشة', N'منطقة القواريشة - بنغازي', N'0218-61-3000015', @BenghaziRegionId, @ManagerBenghaziId),

    -- مكاتب المدن الأخرى (15 مكتب)
    (NEWID(), N'المكتب الرئيسي مصراتة', N'وسط مصراتة', N'0218-51-4000001', @MisrataRegionId, @ManagerMisrataId),
    (NEWID(), N'مكتب شرق مصراتة', N'شرق مصراتة', N'0218-51-4000002', @MisrataRegionId, @ManagerMisrataId),
    (NEWID(), N'مكتب غرب مصراتة', N'غرب مصراتة', N'0218-51-4000003', @MisrataRegionId, @ManagerMisrataId),
    (NEWID(), N'المكتب الرئيسي الزاوية', N'وسط الزاوية', N'0218-23-5000001', @ZawiyaRegionId, @ManagerZawiyaId),
    (NEWID(), N'مكتب شرق الزاوية', N'شرق الزاوية', N'0218-23-5000002', @ZawiyaRegionId, @ManagerZawiyaId),
    (NEWID(), N'مكتب غرب الزاوية', N'غرب الزاوية', N'0218-23-5000003', @ZawiyaRegionId, @ManagerZawiyaId),
    (NEWID(), N'المكتب الرئيسي البيضاء', N'وسط البيضاء', N'0218-84-6000001', @AlbaydaRegionId, @AdminUserId),
    (NEWID(), N'مكتب شمال البيضاء', N'شمال البيضاء', N'0218-84-6000002', @AlbaydaRegionId, @AdminUserId),
    (NEWID(), N'مكتب سرت', N'وسط سرت', N'0218-54-7000001', @TripoliRegionId, @AdminUserId),
    (NEWID(), N'مكتب طبرق', N'وسط طبرق', N'0218-87-8000001', @BenghaziRegionId, @AdminUserId),
    (NEWID(), N'مكتب الخمس', N'وسط الخمس', N'0218-31-9000001', @TripoliRegionId, @AdminUserId),
    (NEWID(), N'مكتب زليتن', N'وسط زليتن', N'0218-41-1000001', @MisrataRegionId, @AdminUserId),
    (NEWID(), N'مكتب أجدابيا', N'وسط أجدابيا', N'0218-64-1100001', @BenghaziRegionId, @AdminUserId),
    (NEWID(), N'مكتب غريان', N'وسط غريان', N'0218-25-1200001', @TripoliRegionId, @AdminUserId),
    (NEWID(), N'مكتب سبها', N'وسط سبها', N'0218-71-1300001', @TripoliRegionId, @AdminUserId);

    DECLARE @OfficesCount INT = (SELECT COUNT(*) FROM Offices);
    PRINT N'تم إدراج ' + CAST(@OfficesCount AS NVARCHAR(10)) + N' مكتب بنجاح!';
END TRY
BEGIN CATCH
    PRINT N'خطأ في إدراج المكاتب: ' + ERROR_MESSAGE();
END CATCH

-- ===== 9. جدول البنايات (Buildings) - 50 بناية =====
PRINT N'إدراج بيانات البنايات...'
BEGIN TRY
    DELETE FROM Buildings;

    -- الحصول على معرفات المكاتب والمناطق
    DECLARE @FirstOfficeId UNIQUEIDENTIFIER = (SELECT TOP 1 Id FROM Offices ORDER BY Name);
    DECLARE @SecondOfficeId UNIQUEIDENTIFIER = (SELECT Id FROM Offices ORDER BY Name OFFSET 1 ROWS FETCH NEXT 1 ROWS ONLY);
    DECLARE @ThirdOfficeId UNIQUEIDENTIFIER = (SELECT Id FROM Offices ORDER BY Name OFFSET 2 ROWS FETCH NEXT 1 ROWS ONLY);

    -- الحصول على معرفات المناطق
    DECLARE @FirstRegionId UNIQUEIDENTIFIER = (SELECT TOP 1 Id FROM Regions ORDER BY Name);
    DECLARE @SecondRegionId UNIQUEIDENTIFIER = (SELECT Id FROM Regions ORDER BY Name OFFSET 1 ROWS FETCH NEXT 1 ROWS ONLY);
    DECLARE @ThirdRegionId UNIQUEIDENTIFIER = (SELECT Id FROM Regions ORDER BY Name OFFSET 2 ROWS FETCH NEXT 1 ROWS ONLY);

    -- قيم افتراضية إذا لم توجد البيانات
    IF @FirstOfficeId IS NULL SET @FirstOfficeId = NEWID();
    IF @SecondOfficeId IS NULL SET @SecondOfficeId = @FirstOfficeId;
    IF @ThirdOfficeId IS NULL SET @ThirdOfficeId = @FirstOfficeId;
    IF @FirstRegionId IS NULL SET @FirstRegionId = NEWID();
    IF @SecondRegionId IS NULL SET @SecondRegionId = @FirstRegionId;
    IF @ThirdRegionId IS NULL SET @ThirdRegionId = @FirstRegionId;

    INSERT INTO Buildings (Id, Name, FileNumber, Definition, Classification, OfficeId, Unit, RegionId, NearestLandmark, ConstructionDate, OpeningDate, MapLocation, TotalLandArea, TotalCoveredArea, NumberOfFloors, ElectricityMeter, AlternativeEnergySource, WaterSource, Sanitation, BriefDescription, LandDonorName, SourceFunds, PrayerCapacity, UserId, ServicesSpecialNeeds, SpecialEntranceWomen, PicturePath) VALUES
    -- مساجد طرابلس (20 مسجد)
    (NEWID(), N'مسجد النصر الكبير', N'MOS-TR-001', N'مسجد جامع', N'مسجد حكومي', @FirstOfficeId, N'وحدة إدارية', @FirstRegionId, N'وسط طرابلس', '1980-01-01', '1982-01-01', N'32.8872,13.1913', 2500.0, 1800.0, 2, N'ELEC-001', N'طاقة شمسية', N'شبكة مياه', N'شبكة صرف', N'مسجد النصر الكبير في وسط طرابلس', N'وقف النصر', 0, N'500 مصلي', @AdminUserId, 1, 1, N'/images/mosques/nasr_tripoli.jpg'),
    (NEWID(), N'مسجد أحمد باشا القرمانلي', N'MOS-TR-002', N'مسجد جامع', N'مسجد تراثي', @FirstOfficeId, N'وحدة تراثية', @FirstRegionId, N'المدينة القديمة', '1736-01-01', '1738-01-01', N'32.8875,13.1918', 1200.0, 800.0, 1, N'ELEC-002', N'لا يوجد', N'بئر تقليدي', N'صرف صحي', N'مسجد أحمد باشا القرمانلي التراثي', N'أحمد باشا القرمانلي', 1, N'200 مصلي', @AdminUserId, 0, 0, N'/images/mosques/ahmed_pasha.jpg'),
    (NEWID(), N'مسجد درغوت باشا', N'MOS-TR-003', N'مسجد جامع', N'مسجد تراثي', @FirstOfficeId, N'وحدة تراثية', @FirstRegionId, N'ساحة الشهداء', '1560-01-01', '1560-01-01', N'32.8870,13.1915', 1500.0, 1000.0, 1, N'ELEC-003', N'لا يوجد', N'بئر تقليدي', N'صرف صحي', N'مسجد درغوت باشا التاريخي', N'درغوت باشا', 1, N'300 مصلي', @AdminUserId, 0, 0, N'/images/mosques/draghut.jpg'),
    (NEWID(), N'مسجد سيدي عبد الواحد', N'MOS-TR-004', N'مسجد جامع', N'مسجد تراثي', @FirstOfficeId, N'وحدة تراثية', @FirstRegionId, N'حي سيدي المصري', '1600-01-01', '1600-01-01', N'32.8865,13.1920', 800.0, 600.0, 1, N'ELEC-004', N'لا يوجد', N'بئر تقليدي', N'صرف صحي', N'مسجد سيدي عبد الواحد', N'وقف إسلامي', 1, N'150 مصلي', @AdminUserId, 0, 0, N'/images/mosques/sidi_abdulwahid.jpg'),
    (NEWID(), N'مسجد الفتح الجديد', N'MOS-TR-005', N'مسجد جامع', N'مسجد حكومي', @FirstOfficeId, N'وحدة إدارية', @FirstRegionId, N'حي الفتح', '2010-01-01', '2012-01-01', N'32.8864,13.1916', 3000.0, 2200.0, 2, N'ELEC-005', N'طاقة شمسية', N'شبكة مياه', N'شبكة صرف', N'مسجد الفتح الجديد الحديث', N'وزارة الأوقاف', 0, N'800 مصلي', @AdminUserId, 1, 1, N'/images/mosques/fatah_new.jpg'),
    (NEWID(), N'مسجد الأندلس', N'MOS-TR-006', N'مسجد جامع', N'مسجد حكومي', @FirstOfficeId, N'وحدة إدارية', @FirstRegionId, N'حي الأندلس', '1995-01-01', '1997-01-01', N'32.8860,13.1912', 2000.0, 1500.0, 2, N'ELEC-006', N'طاقة شمسية', N'شبكة مياه', N'شبكة صرف', N'مسجد الأندلس في حي الأندلس', N'أهل الحي', 0, N'400 مصلي', @AdminUserId, 1, 1, N'/images/mosques/andalus.jpg'),
    (NEWID(), N'مسجد الهدى', N'MOS-TR-007', N'مسجد حي', N'مسجد أهلي', @FirstOfficeId, N'وحدة أهلية', @FirstRegionId, N'حي النصر', '2000-01-01', '2002-01-01', N'32.8858,13.1914', 1800.0, 1200.0, 2, N'ELEC-007', N'لا يوجد', N'شبكة مياه', N'شبكة صرف', N'مسجد الهدى في حي النصر', N'تبرعات أهلية', 1, N'350 مصلي', @AdminUserId, 1, 1, N'/images/mosques/huda.jpg'),
    (NEWID(), N'مسجد الرحمة', N'MOS-TR-008', N'مسجد حي', N'مسجد أهلي', @FirstOfficeId, N'وحدة أهلية', @FirstRegionId, N'حي الهضبة', '2005-01-01', '2007-01-01', N'32.8856,13.1916', 1600.0, 1100.0, 2, N'ELEC-008', N'لا يوجد', N'شبكة مياه', N'شبكة صرف', N'مسجد الرحمة في حي الهضبة', N'تبرعات أهلية', 1, N'300 مصلي', @AdminUserId, 1, 0, N'/images/mosques/rahma.jpg'),
    (NEWID(), N'مسجد التوحيد', N'MOS-TR-009', N'مسجد حي', N'مسجد أهلي', @FirstOfficeId, N'وحدة أهلية', @FirstRegionId, N'باب العزيزية', '1990-01-01', '1992-01-01', N'32.8854,13.1918', 1400.0, 1000.0, 1, N'ELEC-009', N'لا يوجد', N'شبكة مياه', N'شبكة صرف', N'مسجد التوحيد في باب العزيزية', N'تبرعات أهلية', 1, N'250 مصلي', @AdminUserId, 0, 0, N'/images/mosques/tawheed.jpg'),
    (NEWID(), N'مسجد الإيمان', N'MOS-TR-010', N'مسجد حي', N'مسجد أهلي', @FirstOfficeId, N'وحدة أهلية', @FirstRegionId, N'تاجوراء', '2008-01-01', '2010-01-01', N'32.8852,13.1920', 1700.0, 1300.0, 2, N'ELEC-010', N'ألواح شمسية', N'شبكة مياه', N'شبكة صرف', N'مسجد الإيمان في تاجوراء', N'تبرعات أهلية', 1, N'400 مصلي', @AdminUserId, 1, 1, N'/images/mosques/iman.jpg'),
    (NEWID(), N'مسجد الصبر', N'MOS-TR-011', N'مسجد حي', N'مسجد أهلي', @SecondOfficeId, N'وحدة أهلية', @SecondRegionId, N'أبو سليم', '1985-01-01', '1987-01-01', N'32.8850,13.1922', 1500.0, 1100.0, 2, N'ELEC-011', N'لا يوجد', N'شبكة مياه', N'شبكة صرف', N'مسجد الصبر في أبو سليم', N'تبرعات أهلية', 1, N'320 مصلي', @AdminUserId, 1, 0, N'/images/mosques/sabr.jpg'),
    (NEWID(), N'مسجد النور', N'MOS-TR-012', N'مسجد حي', N'مسجد أهلي', @SecondOfficeId, N'وحدة أهلية', @SecondRegionId, N'عين زارة', '2003-01-01', '2005-01-01', N'32.8848,13.1924', 1900.0, 1400.0, 2, N'ELEC-012', N'ألواح شمسية', N'شبكة مياه', N'شبكة صرف', N'مسجد النور في عين زارة', N'تبرعات أهلية', 1, N'450 مصلي', @AdminUserId, 1, 1, N'/images/mosques/noor.jpg'),
    (NEWID(), N'مسجد الصفا', N'MOS-TR-013', N'مسجد حي', N'مسجد أهلي', @SecondOfficeId, N'وحدة أهلية', @SecondRegionId, N'الدريبي', '1998-01-01', '2000-01-01', N'32.8846,13.1926', 1600.0, 1200.0, 2, N'ELEC-013', N'لا يوجد', N'شبكة مياه', N'شبكة صرف', N'مسجد الصفا في الدريبي', N'تبرعات أهلية', 1, N'380 مصلي', @AdminUserId, 1, 1, N'/images/mosques/safa.jpg'),
    (NEWID(), N'مسجد المروة', N'MOS-TR-014', N'مسجد حي', N'مسجد أهلي', @SecondOfficeId, N'وحدة أهلية', @SecondRegionId, N'الكريمية', '2001-01-01', '2003-01-01', N'32.8844,13.1928', 1750.0, 1250.0, 2, N'ELEC-014', N'ألواح شمسية', N'شبكة مياه', N'شبكة صرف', N'مسجد المروة في الكريمية', N'تبرعات أهلية', 1, N'400 مصلي', @AdminUserId, 1, 1, N'/images/mosques/marwa.jpg'),
    (NEWID(), N'مسجد الفرقان', N'MOS-TR-015', N'مسجد حي', N'مسجد أهلي', @SecondOfficeId, N'وحدة أهلية', @SecondRegionId, N'الغيران', '1993-01-01', '1995-01-01', N'32.8842,13.1930', 1400.0, 1000.0, 1, N'ELEC-015', N'لا يوجد', N'شبكة مياه', N'شبكة صرف', N'مسجد الفرقان في الغيران', N'تبرعات أهلية', 1, N'280 مصلي', @AdminUserId, 0, 0, N'/images/mosques/furqan.jpg'),
    (NEWID(), N'مسجد البيان', N'MOS-TR-016', N'مسجد حي', N'مسجد أهلي', @ThirdOfficeId, N'وحدة أهلية', @ThirdRegionId, N'القربولي', '2006-01-01', '2008-01-01', N'32.8840,13.1932', 1650.0, 1200.0, 2, N'ELEC-016', N'ألواح شمسية', N'شبكة مياه', N'شبكة صرف', N'مسجد البيان في القربولي', N'تبرعات أهلية', 1, N'350 مصلي', @AdminUserId, 1, 1, N'/images/mosques/bayan.jpg'),
    (NEWID(), N'مسجد الشكر', N'MOS-TR-017', N'مسجد حي', N'مسجد أهلي', @ThirdOfficeId, N'وحدة أهلية', @ThirdRegionId, N'الحدائق', '1996-01-01', '1998-01-01', N'32.8838,13.1934', 1550.0, 1150.0, 2, N'ELEC-017', N'لا يوجد', N'شبكة مياه', N'شبكة صرف', N'مسجد الشكر في الحدائق', N'تبرعات أهلية', 1, N'330 مصلي', @AdminUserId, 1, 0, N'/images/mosques/shukr.jpg'),
    (NEWID(), N'مسجد الصدق', N'MOS-TR-018', N'مسجد حي', N'مسجد أهلي', @ThirdOfficeId, N'وحدة أهلية', @ThirdRegionId, N'الصياد', '2004-01-01', '2006-01-01', N'32.8836,13.1936', 1700.0, 1300.0, 2, N'ELEC-018', N'ألواح شمسية', N'شبكة مياه', N'شبكة صرف', N'مسجد الصدق في الصياد', N'تبرعات أهلية', 1, N'390 مصلي', @AdminUserId, 1, 1, N'/images/mosques/sidq.jpg'),
    (NEWID(), N'مسجد العدل', N'MOS-TR-019', N'مسجد حي', N'مسجد أهلي', @ThirdOfficeId, N'وحدة أهلية', @ThirdRegionId, N'الزهراء', '1999-01-01', '2001-01-01', N'32.8834,13.1938', 1600.0, 1200.0, 2, N'ELEC-019', N'لا يوجد', N'شبكة مياه', N'شبكة صرف', N'مسجد العدل في الزهراء', N'تبرعات أهلية', 1, N'360 مصلي', @AdminUserId, 1, 1, N'/images/mosques/adl.jpg'),
    (NEWID(), N'مسجد الحكمة', N'MOS-TR-020', N'مسجد حي', N'مسجد أهلي', @ThirdOfficeId, N'وحدة أهلية', @ThirdRegionId, N'الجفارة', '2007-01-01', '2009-01-01', N'32.8832,13.1940', 1800.0, 1350.0, 2, N'ELEC-020', N'ألواح شمسية', N'شبكة مياه', N'شبكة صرف', N'مسجد الحكمة في الجفارة', N'تبرعات أهلية', 1, N'420 مصلي', @AdminUserId, 1, 1, N'/images/mosques/hikma.jpg'),

    -- مساجد بنغازي (15 مسجد)
    (NEWID(), N'مسجد العتيق بنغازي', N'MOS-BG-001', N'مسجد جامع', N'مسجد تراثي', @SecondOfficeId, N'وحدة تراثية', @SecondRegionId, N'وسط بنغازي', '1400-01-01', '1400-01-01', N'32.1147,20.0686', 600.0, 400.0, 1, N'ELEC-021', N'لا يوجد', N'بئر تقليدي', N'صرف صحي', N'مسجد العتيق التاريخي في بنغازي', N'وقف إسلامي', 1, N'100 مصلي', @AdminUserId, 0, 0, N'/images/mosques/ateeq_benghazi.jpg'),
    (NEWID(), N'مسجد سيدي خريبيش بنغازي', N'MOS-BG-002', N'مسجد جامع', N'مسجد تراثي', @SecondOfficeId, N'وحدة تراثية', @SecondRegionId, N'حي سيدي خريبيش', '1800-01-01', '1800-01-01', N'32.1140,20.0695', 900.0, 650.0, 1, N'ELEC-022', N'لا يوجد', N'بئر تقليدي', N'صرف صحي', N'مسجد سيدي خريبيش في بنغازي', N'وقف إسلامي', 1, N'120 مصلي', @AdminUserId, 0, 0, N'/images/mosques/sidi_khreibish_benghazi.jpg'),
    (NEWID(), N'مسجد البحر', N'MOS-BG-003', N'مسجد جامع', N'مسجد حكومي', @SecondOfficeId, N'وحدة إدارية', @SecondRegionId, N'طريق الشاطئ', '1990-01-01', '1992-01-01', N'32.1145,20.0690', 2200.0, 1600.0, 2, N'ELEC-023', N'ألواح شمسية', N'شبكة مياه', N'شبكة صرف', N'مسجد البحر على طريق الشاطئ', N'وزارة الأوقاف', 0, N'500 مصلي', @AdminUserId, 1, 1, N'/images/mosques/bahr.jpg'),
    (NEWID(), N'مسجد الصابري', N'MOS-BG-004', N'مسجد حي', N'مسجد أهلي', @SecondOfficeId, N'وحدة أهلية', @SecondRegionId, N'حي الصابري', '1985-01-01', '1987-01-01', N'32.1142,20.0692', 1500.0, 1100.0, 2, N'ELEC-024', N'لا يوجد', N'شبكة مياه', N'شبكة صرف', N'مسجد الصابري في حي الصابري', N'تبرعات أهلية', 1, N'350 مصلي', @AdminUserId, 1, 1, N'/images/mosques/sabri.jpg'),
    (NEWID(), N'مسجد الكويفية', N'MOS-BG-005', N'مسجد حي', N'مسجد أهلي', @SecondOfficeId, N'وحدة أهلية', @SecondRegionId, N'حي الكويفية', '1995-01-01', '1997-01-01', N'32.1138,20.0698', 1400.0, 1000.0, 2, N'ELEC-025', N'لا يوجد', N'شبكة مياه', N'شبكة صرف', N'مسجد الكويفية في حي الكويفية', N'تبرعات أهلية', 1, N'300 مصلي', @AdminUserId, 1, 0, N'/images/mosques/kuwaifiya.jpg'),
    (NEWID(), N'مسجد البركة', N'MOS-BG-006', N'مسجد حي', N'مسجد أهلي', @SecondOfficeId, N'وحدة أهلية', @SecondRegionId, N'حي البركة', '2000-01-01', '2002-01-01', N'32.1135,20.0700', 1600.0, 1200.0, 2, N'ELEC-026', N'ألواح شمسية', N'شبكة مياه', N'شبكة صرف', N'مسجد البركة في حي البركة', N'تبرعات أهلية', 1, N'380 مصلي', @AdminUserId, 1, 1, N'/images/mosques/baraka.jpg'),
    (NEWID(), N'مسجد سيدي حسين', N'MOS-BG-007', N'مسجد حي', N'مسجد تراثي', @SecondOfficeId, N'وحدة تراثية', @SecondRegionId, N'حي سيدي حسين', '1750-01-01', '1750-01-01', N'32.1132,20.0702', 800.0, 600.0, 1, N'ELEC-027', N'لا يوجد', N'بئر تقليدي', N'صرف صحي', N'مسجد سيدي حسين التراثي', N'وقف إسلامي', 1, N'140 مصلي', @AdminUserId, 0, 0, N'/images/mosques/sidi_hussein.jpg'),
    (NEWID(), N'مسجد القوارشة', N'MOS-BG-008', N'مسجد حي', N'مسجد أهلي', @SecondOfficeId, N'وحدة أهلية', @SecondRegionId, N'حي القوارشة', '1998-01-01', '2000-01-01', N'32.1130,20.0704', 1550.0, 1150.0, 2, N'ELEC-028', N'لا يوجد', N'شبكة مياه', N'شبكة صرف', N'مسجد القوارشة في حي القوارشة', N'تبرعات أهلية', 1, N'340 مصلي', @AdminUserId, 1, 1, N'/images/mosques/qawarsha.jpg'),
    (NEWID(), N'مسجد الهواري', N'MOS-BG-009', N'مسجد حي', N'مسجد أهلي', @SecondOfficeId, N'وحدة أهلية', @SecondRegionId, N'حي الهواري', '2003-01-01', '2005-01-01', N'32.1128,20.0706', 1700.0, 1300.0, 2, N'ELEC-029', N'ألواح شمسية', N'شبكة مياه', N'شبكة صرف', N'مسجد الهواري في حي الهواري', N'تبرعات أهلية', 1, N'400 مصلي', @AdminUserId, 1, 1, N'/images/mosques/hawari.jpg'),
    (NEWID(), N'مسجد الجواري', N'MOS-BG-010', N'مسجد حي', N'مسجد أهلي', @SecondOfficeId, N'وحدة أهلية', @SecondRegionId, N'حي الجواري', '1988-01-01', '1990-01-01', N'32.1125,20.0708', 1450.0, 1050.0, 2, N'ELEC-030', N'لا يوجد', N'شبكة مياه', N'شبكة صرف', N'مسجد الجواري في حي الجواري', N'تبرعات أهلية', 1, N'320 مصلي', @AdminUserId, 1, 0, N'/images/mosques/jawari.jpg'),
    (NEWID(), N'مسجد المقرون', N'MOS-BG-011', N'مسجد حي', N'مسجد أهلي', @SecondOfficeId, N'وحدة أهلية', @SecondRegionId, N'حي المقرون', '2001-01-01', '2003-01-01', N'32.1122,20.0710', 1650.0, 1250.0, 2, N'ELEC-031', N'ألواح شمسية', N'شبكة مياه', N'شبكة صرف', N'مسجد المقرون في حي المقرون', N'تبرعات أهلية', 1, N'370 مصلي', @AdminUserId, 1, 1, N'/images/mosques/maqroon.jpg'),
    (NEWID(), N'مسجد الثقافة', N'MOS-BG-012', N'مسجد حي', N'مسجد أهلي', @SecondOfficeId, N'وحدة أهلية', @SecondRegionId, N'شارع الثقافة', '2005-01-01', '2007-01-01', N'32.1120,20.0712', 1800.0, 1400.0, 2, N'ELEC-032', N'ألواح شمسية', N'شبكة مياه', N'شبكة صرف', N'مسجد الثقافة على شارع الثقافة', N'تبرعات أهلية', 1, N'430 مصلي', @AdminUserId, 1, 1, N'/images/mosques/thaqafa.jpg'),
    (NEWID(), N'مسجد بوعطني', N'MOS-BG-013', N'مسجد حي', N'مسجد أهلي', @SecondOfficeId, N'وحدة أهلية', @SecondRegionId, N'منطقة بوعطني', '1992-01-01', '1994-01-01', N'32.1118,20.0714', 1500.0, 1100.0, 2, N'ELEC-033', N'لا يوجد', N'شبكة مياه', N'شبكة صرف', N'مسجد بوعطني في منطقة بوعطني', N'تبرعات أهلية', 1, N'330 مصلي', @AdminUserId, 1, 1, N'/images/mosques/buatni.jpg'),
    (NEWID(), N'مسجد الليثي', N'MOS-BG-014', N'مسجد حي', N'مسجد أهلي', @SecondOfficeId, N'وحدة أهلية', @SecondRegionId, N'منطقة الليثي', '1996-01-01', '1998-01-01', N'32.1115,20.0716', 1400.0, 1000.0, 2, N'ELEC-034', N'لا يوجد', N'شبكة مياه', N'شبكة صرف', N'مسجد الليثي في منطقة الليثي', N'تبرعات أهلية', 1, N'300 مصلي', @AdminUserId, 1, 0, N'/images/mosques/laithi.jpg'),
    (NEWID(), N'مسجد القواريشة', N'MOS-BG-015', N'مسجد حي', N'مسجد أهلي', @SecondOfficeId, N'وحدة أهلية', @SecondRegionId, N'منطقة القواريشة', '2004-01-01', '2006-01-01', N'32.1112,20.0718', 1600.0, 1200.0, 2, N'ELEC-035', N'ألواح شمسية', N'شبكة مياه', N'شبكة صرف', N'مسجد القواريشة في منطقة القواريشة', N'تبرعات أهلية', 1, N'360 مصلي', @AdminUserId, 1, 1, N'/images/mosques/qawarisha.jpg'),

    -- مساجد مصراتة والمدن الأخرى (15 مسجد)
    (NEWID(), N'مسجد سيدي عبد السلام مصراتة', N'MOS-MS-001', N'مسجد جامع', N'مسجد تراثي', @ThirdOfficeId, N'وحدة تراثية', @ThirdRegionId, N'وسط مصراتة', '1750-01-01', '1750-01-01', N'32.3783,15.0906', 1100.0, 750.0, 1, N'ELEC-036', N'لا يوجد', N'بئر تقليدي', N'صرف صحي', N'مسجد سيدي عبد السلام في مصراتة', N'وقف إسلامي', 1, N'160 مصلي', @AdminUserId, 0, 0, N'/images/mosques/sidi_abdulsalam_misrata.jpg'),
    (NEWID(), N'مسجد الجمعة مصراتة', N'MOS-MS-002', N'مسجد جامع', N'مسجد حكومي', @ThirdOfficeId, N'وحدة إدارية', @ThirdRegionId, N'سوق الجمعة', '1980-01-01', '1982-01-01', N'32.3785,15.0908', 2000.0, 1500.0, 2, N'ELEC-037', N'ألواح شمسية', N'شبكة مياه', N'شبكة صرف', N'مسجد الجمعة في سوق الجمعة', N'وزارة الأوقاف', 0, N'450 مصلي', @AdminUserId, 1, 1, N'/images/mosques/jumaa_misrata.jpg'),
    (NEWID(), N'مسجد التوبة الزاوية', N'MOS-ZW-001', N'مسجد جامع', N'مسجد حكومي', @FirstOfficeId, N'وحدة إدارية', @FirstRegionId, N'وسط الزاوية', '1985-01-01', '1987-01-01', N'32.7569,12.7283', 1800.0, 1300.0, 2, N'ELEC-038', N'لا يوجد', N'شبكة مياه', N'شبكة صرف', N'مسجد التوبة في وسط الزاوية', N'وزارة الأوقاف', 0, N'400 مصلي', @AdminUserId, 1, 1, N'/images/mosques/tawba_zawiya.jpg'),
    (NEWID(), N'مسجد الشهداء الزاوية', N'MOS-ZW-002', N'مسجد حي', N'مسجد أهلي', @FirstOfficeId, N'وحدة أهلية', @FirstRegionId, N'حي الشهداء', '1995-01-01', '1997-01-01', N'32.7571,12.7285', 1600.0, 1200.0, 2, N'ELEC-039', N'ألواح شمسية', N'شبكة مياه', N'شبكة صرف', N'مسجد الشهداء في حي الشهداء', N'تبرعات أهلية', 1, N'370 مصلي', @AdminUserId, 1, 1, N'/images/mosques/shuhada_zawiya.jpg'),
    (NEWID(), N'مسجد الحرية الزاوية', N'MOS-ZW-003', N'مسجد حي', N'مسجد أهلي', @FirstOfficeId, N'وحدة أهلية', @FirstRegionId, N'حي الحرية', '2000-01-01', '2002-01-01', N'32.7573,12.7287', 1500.0, 1100.0, 2, N'ELEC-040', N'لا يوجد', N'شبكة مياه', N'شبكة صرف', N'مسجد الحرية في حي الحرية', N'تبرعات أهلية', 1, N'330 مصلي', @AdminUserId, 1, 0, N'/images/mosques/huriya_zawiya.jpg'),
    (NEWID(), N'مسجد المجد البيضاء', N'MOS-BY-001', N'مسجد جامع', N'مسجد حكومي', @SecondOfficeId, N'وحدة إدارية', @SecondRegionId, N'وسط البيضاء', '1990-01-01', '1992-01-01', N'32.7617,21.7581', 1700.0, 1250.0, 2, N'ELEC-041', N'ألواح شمسية', N'شبكة مياه', N'شبكة صرف', N'مسجد المجد في وسط البيضاء', N'وزارة الأوقاف', 0, N'380 مصلي', @AdminUserId, 1, 1, N'/images/mosques/majd_bayda.jpg'),
    (NEWID(), N'مسجد العروبة سرت', N'MOS-SR-001', N'مسجد جامع', N'مسجد حكومي', @ThirdOfficeId, N'وحدة إدارية', @ThirdRegionId, N'وسط سرت', '1988-01-01', '1990-01-01', N'31.2089,16.5887', 1900.0, 1400.0, 2, N'ELEC-042', N'لا يوجد', N'شبكة مياه', N'شبكة صرف', N'مسجد العروبة في وسط سرت', N'وزارة الأوقاف', 0, N'420 مصلي', @AdminUserId, 1, 1, N'/images/mosques/uruba_sirt.jpg'),
    (NEWID(), N'مسجد الفاتح طبرق', N'MOS-TB-001', N'مسجد جامع', N'مسجد حكومي', @SecondOfficeId, N'وحدة إدارية', @SecondRegionId, N'وسط طبرق', '1992-01-01', '1994-01-01', N'32.0840,23.9580', 1600.0, 1200.0, 2, N'ELEC-043', N'ألواح شمسية', N'شبكة مياه', N'شبكة صرف', N'مسجد الفاتح في وسط طبرق', N'وزارة الأوقاف', 0, N'360 مصلي', @AdminUserId, 1, 1, N'/images/mosques/fatih_tobruk.jpg'),
    (NEWID(), N'مسجد السلام الخمس', N'MOS-KH-001', N'مسجد جامع', N'مسجد حكومي', @FirstOfficeId, N'وحدة إدارية', @FirstRegionId, N'وسط الخمس', '1985-01-01', '1987-01-01', N'32.6487,14.2618', 1550.0, 1150.0, 2, N'ELEC-044', N'لا يوجد', N'شبكة مياه', N'شبكة صرف', N'مسجد السلام في وسط الخمس', N'وزارة الأوقاف', 0, N'340 مصلي', @AdminUserId, 1, 1, N'/images/mosques/salam_khums.jpg'),
    (NEWID(), N'مسجد الأمان زليتن', N'MOS-ZL-001', N'مسجد جامع', N'مسجد حكومي', @ThirdOfficeId, N'وحدة إدارية', @ThirdRegionId, N'وسط زليتن', '1987-01-01', '1989-01-01', N'32.4677,14.5687', 1650.0, 1250.0, 2, N'ELEC-045', N'ألواح شمسية', N'شبكة مياه', N'شبكة صرف', N'مسجد الأمان في وسط زليتن', N'وزارة الأوقاف', 0, N'370 مصلي', @AdminUserId, 1, 1, N'/images/mosques/aman_zliten.jpg'),
    (NEWID(), N'مسجد الوفاء أجدابيا', N'MOS-AJ-001', N'مسجد جامع', N'مسجد حكومي', @SecondOfficeId, N'وحدة إدارية', @SecondRegionId, N'وسط أجدابيا', '1990-01-01', '1992-01-01', N'30.7554,20.2264', 1750.0, 1300.0, 2, N'ELEC-046', N'لا يوجد', N'شبكة مياه', N'شبكة صرف', N'مسجد الوفاء في وسط أجدابيا', N'وزارة الأوقاف', 0, N'390 مصلي', @AdminUserId, 1, 1, N'/images/mosques/wafa_ajdabiya.jpg'),
    (NEWID(), N'مسجد الأمل غريان', N'MOS-GH-001', N'مسجد جامع', N'مسجد حكومي', @FirstOfficeId, N'وحدة إدارية', @FirstRegionId, N'وسط غريان', '1983-01-01', '1985-01-01', N'32.1718,13.0219', 1450.0, 1050.0, 2, N'ELEC-047', N'ألواح شمسية', N'شبكة مياه', N'شبكة صرف', N'مسجد الأمل في وسط غريان', N'وزارة الأوقاف', 0, N'310 مصلي', @AdminUserId, 1, 1, N'/images/mosques/amal_gharyan.jpg'),
    (NEWID(), N'مسجد الفجر سبها', N'MOS-SB-001', N'مسجد جامع', N'مسجد حكومي', @FirstOfficeId, N'وحدة إدارية', @FirstRegionId, N'وسط سبها', '1989-01-01', '1991-01-01', N'27.0377,14.4283', 1800.0, 1350.0, 2, N'ELEC-048', N'لا يوجد', N'شبكة مياه', N'شبكة صرف', N'مسجد الفجر في وسط سبها', N'وزارة الأوقاف', 0, N'410 مصلي', @AdminUserId, 1, 1, N'/images/mosques/fajr_sebha.jpg'),
    (NEWID(), N'مسجد النهضة درنة', N'MOS-DR-001', N'مسجد جامع', N'مسجد حكومي', @SecondOfficeId, N'وحدة إدارية', @SecondRegionId, N'وسط درنة', '1986-01-01', '1988-01-01', N'32.7649,22.6374', 1650.0, 1200.0, 2, N'ELEC-049', N'ألواح شمسية', N'شبكة مياه', N'شبكة صرف', N'مسجد النهضة في وسط درنة', N'وزارة الأوقاف', 0, N'360 مصلي', @AdminUserId, 1, 1, N'/images/mosques/nahda_derna.jpg'),
    (NEWID(), N'مسجد السكينة المرج', N'MOS-MR-001', N'مسجد جامع', N'مسجد حكومي', @SecondOfficeId, N'وحدة إدارية', @SecondRegionId, N'وسط المرج', '1991-01-01', '1993-01-01', N'32.4918,20.8306', 1550.0, 1150.0, 2, N'ELEC-050', N'لا يوجد', N'شبكة مياه', N'شبكة صرف', N'مسجد السكينة في وسط المرج', N'وزارة الأوقاف', 0, N'340 مصلي', @AdminUserId, 1, 1, N'/images/mosques/sakina_marj.jpg');

    DECLARE @BuildingsCount INT = (SELECT COUNT(*) FROM Buildings);
    PRINT N'تم إدراج ' + CAST(@BuildingsCount AS NVARCHAR(10)) + N' بناية بنجاح!';
END TRY
BEGIN CATCH
    PRINT N'خطأ في إدراج البنايات: ' + ERROR_MESSAGE();
END CATCH

PRINT N'=== انتهاء الجزء الثاني من إدراج البيانات ==='
PRINT N'للمتابعة، قم بتشغيل الجزء الثالث للجداول المرتبطة بالبنايات'