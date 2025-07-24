-- Script لإضافة 10 سجلات تجريبية لكل جدول بالعربية
-- Database: Swagger_Endowment22
-- تم إنشاؤه مع مراعاة الترميز العربي N''

USE [Swagger_Endowment22]
GO

-- =====================================================
-- الجزء الأول: إضافة البيانات المرجعية الأساسية
-- =====================================================

PRINT N'بدء إضافة البيانات التجريبية بالعربية...'
PRINT 'Starting to insert Arabic sample data...'

-- إضافة البنوك (Banks)
PRINT N'إضافة البنوك...'
INSERT INTO [Banks] ([Id], [Name], [Address], [ContactNumber]) VALUES
(NEWID(), N'البنك الأهلي السعودي', N'شارع الملك فهد، الرياض', N'011-4567890'),
(NEWID(), N'بنك الراجحي', N'طريق الملك عبدالعزيز، جدة', N'012-3456789'),
(NEWID(), N'البنك السعودي الفرنسي', N'حي العليا، الرياض', N'011-2345678'),
(NEWID(), N'بنك ساب', N'شارع التحلية، جدة', N'012-4567890'),
(NEWID(), N'البنك العربي الوطني', N'طريق الخرج، الرياض', N'011-5678901'),
(NEWID(), N'بنك الإنماء', N'حي السلامة، جدة', N'012-6789012'),
(NEWID(), N'البنك الأول', N'شارع الأمير محمد بن عبدالعزيز، الرياض', N'011-7890123'),
(NEWID(), N'بنك الجزيرة', N'طريق المدينة، جدة', N'012-8901234'),
(NEWID(), N'البنك السعودي البريطاني', N'حي الملز، الرياض', N'011-9012345'),
(NEWID(), N'البنك السعودي للاستثمار', N'شارع فلسطين، جدة', N'012-0123456');

-- إضافة المدن (Cities)
PRINT N'إضافة المدن...'
DECLARE @CityId1 UNIQUEIDENTIFIER = NEWID()
DECLARE @CityId2 UNIQUEIDENTIFIER = NEWID()
DECLARE @CityId3 UNIQUEIDENTIFIER = NEWID()
DECLARE @CityId4 UNIQUEIDENTIFIER = NEWID()
DECLARE @CityId5 UNIQUEIDENTIFIER = NEWID()
DECLARE @CityId6 UNIQUEIDENTIFIER = NEWID()
DECLARE @CityId7 UNIQUEIDENTIFIER = NEWID()
DECLARE @CityId8 UNIQUEIDENTIFIER = NEWID()
DECLARE @CityId9 UNIQUEIDENTIFIER = NEWID()
DECLARE @CityId10 UNIQUEIDENTIFIER = NEWID()

INSERT INTO [Cities] ([Id], [Name], [Country]) VALUES
(@CityId1, N'الرياض', N'المملكة العربية السعودية'),
(@CityId2, N'جدة', N'المملكة العربية السعودية'),
(@CityId3, N'مكة المكرمة', N'المملكة العربية السعودية'),
(@CityId4, N'المدينة المنورة', N'المملكة العربية السعودية'),
(@CityId5, N'الدمام', N'المملكة العربية السعودية'),
(@CityId6, N'تبوك', N'المملكة العربية السعودية'),
(@CityId7, N'أبها', N'المملكة العربية السعودية'),
(@CityId8, N'الطائف', N'المملكة العربية السعودية'),
(@CityId9, N'بريدة', N'المملكة العربية السعودية'),
(@CityId10, N'خميس مشيط', N'المملكة العربية السعودية');

-- إضافة المناطق (Regions)
PRINT N'إضافة المناطق...'
DECLARE @RegionId1 UNIQUEIDENTIFIER = NEWID()
DECLARE @RegionId2 UNIQUEIDENTIFIER = NEWID()
DECLARE @RegionId3 UNIQUEIDENTIFIER = NEWID()
DECLARE @RegionId4 UNIQUEIDENTIFIER = NEWID()
DECLARE @RegionId5 UNIQUEIDENTIFIER = NEWID()
DECLARE @RegionId6 UNIQUEIDENTIFIER = NEWID()
DECLARE @RegionId7 UNIQUEIDENTIFIER = NEWID()
DECLARE @RegionId8 UNIQUEIDENTIFIER = NEWID()
DECLARE @RegionId9 UNIQUEIDENTIFIER = NEWID()
DECLARE @RegionId10 UNIQUEIDENTIFIER = NEWID()

INSERT INTO [Regions] ([Id], [Name], [Country], [CityId]) VALUES
(@RegionId1, N'حي العليا', N'المملكة العربية السعودية', @CityId1),
(@RegionId2, N'حي الروضة', N'المملكة العربية السعودية', @CityId2),
(@RegionId3, N'حي العزيزية', N'المملكة العربية السعودية', @CityId3),
(@RegionId4, N'حي قباء', N'المملكة العربية السعودية', @CityId4),
(@RegionId5, N'حي الفردوس', N'المملكة العربية السعودية', @CityId5),
(@RegionId6, N'حي السلامة', N'المملكة العربية السعودية', @CityId6),
(@RegionId7, N'حي الخالدية', N'المملكة العربية السعودية', @CityId7),
(@RegionId8, N'حي الشفا', N'المملكة العربية السعودية', @CityId8),
(@RegionId9, N'حي الصفراء', N'المملكة العربية السعودية', @CityId9),
(@RegionId10, N'حي النهضة', N'المملكة العربية السعودية', @CityId10);

-- إضافة المنتجات (Products)
PRINT N'إضافة المنتجات...'
DECLARE @ProductId1 UNIQUEIDENTIFIER = NEWID()
DECLARE @ProductId2 UNIQUEIDENTIFIER = NEWID()
DECLARE @ProductId3 UNIQUEIDENTIFIER = NEWID()
DECLARE @ProductId4 UNIQUEIDENTIFIER = NEWID()
DECLARE @ProductId5 UNIQUEIDENTIFIER = NEWID()
DECLARE @ProductId6 UNIQUEIDENTIFIER = NEWID()
DECLARE @ProductId7 UNIQUEIDENTIFIER = NEWID()
DECLARE @ProductId8 UNIQUEIDENTIFIER = NEWID()
DECLARE @ProductId9 UNIQUEIDENTIFIER = NEWID()
DECLARE @ProductId10 UNIQUEIDENTIFIER = NEWID()

INSERT INTO [Products] ([Id], [Name], [Description]) VALUES
(@ProductId1, N'مصاحف القرآن الكريم', N'مصاحف عالية الجودة للمساجد والمراكز الإسلامية'),
(@ProductId2, N'كتب التفسير', N'مجموعة شاملة من كتب تفسير القرآن الكريم'),
(@ProductId3, N'كتب الحديث الشريف', N'مجموعات الحديث الشريف والسنة النبوية'),
(@ProductId4, N'كتب الفقه الإسلامي', N'كتب الفقه والأحكام الشرعية'),
(@ProductId5, N'الكتب التعليمية للأطفال', N'كتب تعليم القرآن والسيرة للأطفال'),
(@ProductId6, N'أجهزة الصوت والصوتيات', N'أنظمة الصوت للمساجد والمحاضرات'),
(@ProductId7, N'السجاد والمفروشات', N'سجاد وفرش للصلاة والمساجد'),
(@ProductId8, N'أدوات الوضوء والطهارة', N'مستلزمات الوضوء والطهارة'),
(@ProductId9, N'اللوحات والزخارف الإسلامية', N'لوحات فنية وزخارف إسلامية للمساجد'),
(@ProductId10, N'مستلزمات المكتبة', N'رفوف ومعدات تنظيم المكتبات الإسلامية');

-- إضافة المكاتب (Offices)
PRINT N'إضافة المكاتب...'
DECLARE @OfficeId1 UNIQUEIDENTIFIER = NEWID()
DECLARE @OfficeId2 UNIQUEIDENTIFIER = NEWID()
DECLARE @OfficeId3 UNIQUEIDENTIFIER = NEWID()
DECLARE @OfficeId4 UNIQUEIDENTIFIER = NEWID()
DECLARE @OfficeId5 UNIQUEIDENTIFIER = NEWID()
DECLARE @OfficeId6 UNIQUEIDENTIFIER = NEWID()
DECLARE @OfficeId7 UNIQUEIDENTIFIER = NEWID()
DECLARE @OfficeId8 UNIQUEIDENTIFIER = NEWID()
DECLARE @OfficeId9 UNIQUEIDENTIFIER = NEWID()
DECLARE @OfficeId10 UNIQUEIDENTIFIER = NEWID()

INSERT INTO [Offices] ([Id], [Name], [Location], [PhoneNumber], [RegionId], [UserId]) VALUES
(@OfficeId1, N'مكتب أوقاف الرياض الرئيسي', N'شارع الملك فيصل، حي العليا', N'011-4567890', @RegionId1, NEWID()),
(@OfficeId2, N'مكتب أوقاف جدة المركزي', N'طريق الملك عبدالعزيز، حي الروضة', N'012-3456789', @RegionId2, NEWID()),
(@OfficeId3, N'مكتب أوقاف مكة المكرمة', N'شارع الحرم، حي العزيزية', N'012-2345678', @RegionId3, NEWID()),
(@OfficeId4, N'مكتب أوقاف المدينة المنورة', N'طريق قباء، حي قباء', N'014-5678901', @RegionId4, NEWID()),
(@OfficeId5, N'مكتب أوقاف الدمام', N'شارع الملك فهد، حي الفردوس', N'013-6789012', @RegionId5, NEWID()),
(@OfficeId6, N'مكتب أوقاف تبوك', N'طريق الأمير فهد، حي السلامة', N'014-7890123', @RegionId6, NEWID()),
(@OfficeId7, N'مكتب أوقاف أبها', N'شارع الملك عبدالله، حي الخالدية', N'017-8901234', @RegionId7, NEWID()),
(@OfficeId8, N'مكتب أوقاف الطائف', N'طريق الهدا، حي الشفا', N'012-9012345', @RegionId8, NEWID()),
(@OfficeId9, N'مكتب أوقاف بريدة', N'شارع الملك سعود، حي الصفراء', N'016-0123456', @RegionId9, NEWID()),
(@OfficeId10, N'مكتب أوقاف خميس مشيط', N'طريق الملك خالد، حي النهضة', N'017-1234567', @RegionId10, NEWID());

-- =====================================================
-- الجزء الثاني: إضافة الكيانات الرئيسية
-- =====================================================

-- إضافة المرافق (Facilities)
PRINT N'إضافة المرافق...'
INSERT INTO [Facilities] ([Id], [Name], [Location], [ContactInfo], [Capacity], [Status]) VALUES
(NEWID(), N'مركز تحفيظ القرآن الكريم - العليا', N'حي العليا، الرياض', N'011-4567890', 200, N'نشط'),
(NEWID(), N'مكتبة الأوقاف الإسلامية - جدة', N'حي الروضة، جدة', N'012-3456789', 150, N'نشط'),
(NEWID(), N'مركز الدعوة والإرشاد - مكة', N'حي العزيزية، مكة المكرمة', N'012-2345678', 300, N'نشط'),
(NEWID(), N'معهد العلوم الشرعية - المدينة', N'حي قباء، المدينة المنورة', N'014-5678901', 250, N'نشط'),
(NEWID(), N'مركز التدريب الإسلامي - الدمام', N'حي الفردوس، الدمام', N'013-6789012', 180, N'نشط'),
(NEWID(), N'دار القرآن الكريم - تبوك', N'حي السلامة، تبوك', N'014-7890123', 120, N'نشط'),
(NEWID(), N'مركز العلوم الإسلامية - أبها', N'حي الخالدية، أبها', N'017-8901234', 160, N'نشط'),
(NEWID(), N'معهد الدراسات القرآنية - الطائف', N'حي الشفا، الطائف', N'012-9012345', 140, N'نشط'),
(NEWID(), N'مركز الأنشطة الإسلامية - بريدة', N'حي الصفراء، بريدة', N'016-0123456', 190, N'نشط'),
(NEWID(), N'دار الحديث الشريف - خميس مشيط', N'حي النهضة، خميس مشيط', N'017-1234567', 130, N'نشط');

-- إضافة المباني (Buildings)
PRINT N'إضافة المباني...'
DECLARE @BuildingId1 UNIQUEIDENTIFIER = NEWID()
DECLARE @BuildingId2 UNIQUEIDENTIFIER = NEWID()
DECLARE @BuildingId3 UNIQUEIDENTIFIER = NEWID()
DECLARE @BuildingId4 UNIQUEIDENTIFIER = NEWID()
DECLARE @BuildingId5 UNIQUEIDENTIFIER = NEWID()
DECLARE @BuildingId6 UNIQUEIDENTIFIER = NEWID()
DECLARE @BuildingId7 UNIQUEIDENTIFIER = NEWID()
DECLARE @BuildingId8 UNIQUEIDENTIFIER = NEWID()
DECLARE @BuildingId9 UNIQUEIDENTIFIER = NEWID()
DECLARE @BuildingId10 UNIQUEIDENTIFIER = NEWID()

INSERT INTO [Buildings] ([Id], [Name], [FileNumber], [Definition], [Classification], [OfficeId], [Unit], [RegionId], [NearestLandmark], [ConstructionDate], [OpeningDate], [MapLocation], [TotalLandArea], [TotalCoveredArea], [NumberOfFloors], [ElectricityMeter], [AlternativeEnergySource], [WaterSource], [Sanitation], [BriefDescription], [LandDonorName], [SourceFunds], [PrayerCapacity], [UserId], [ServicesSpecialNeeds], [SpecialEntranceWomen], [PicturePath]) VALUES
(@BuildingId1, N'مسجد الملك فيصل الكبير', N'RYD-001', N'مسجد جامع', N'مسجد رئيسي', @OfficeId1, N'الوحدة الأولى', @RegionId1, N'بجوار مستشفى الملك فيصل', '2020-01-15', '2020-06-01', N'24.7136,46.6753', 2500.0, 1800.0, 2, N'123456789', N'طاقة شمسية', N'شبكة المياه العامة', N'شبكة الصرف الصحي', N'مسجد حديث ومجهز بأحدث التقنيات', N'الأمير فيصل بن عبدالعزيز', 0, N'800 مصل', N'user1', 1, 1, N'/images/mosque1.jpg'),
(@BuildingId2, N'مسجد النور الإسلامي', N'JED-002', N'مسجد حي', N'مسجد فرعي', @OfficeId2, N'الوحدة الثانية', @RegionId2, N'قرب مجمع الروضة التجاري', '2019-03-10', '2019-08-15', N'21.4858,39.1925', 1800.0, 1200.0, 1, N'987654321', N'لا يوجد', N'خزان علوي', N'خزان تجميع', N'مسجد متوسط الحجم يخدم الحي', N'عبدالله محمد الأحمد', 1, N'400 مصل', N'user2', 0, 1, N'/images/mosque2.jpg'),
(@BuildingId3, N'مسجد الحرمين الشريفين', N'MKK-003', N'مسجد جامع', N'مسجد رئيسي', @OfficeId3, N'الوحدة الثالثة', @RegionId3, N'شارع العزيزية الرئيسي', '2018-05-20', '2018-12-01', N'21.3891,39.8579', 3000.0, 2200.0, 3, N'555666777', N'طاقة شمسية', N'شبكة المياه العامة', N'شبكة الصرف الصحي', N'مسجد كبير مع مكتبة إسلامية', N'مؤسسة الحرمين الخيرية', 0, N'1200 مصل', N'user3', 1, 1, N'/images/mosque3.jpg'),
(@BuildingId4, N'مسجد المصطفى', N'MED-004', N'مسجد حي', N'مسجد فرعي', @OfficeId4, N'الوحدة الرابعة', @RegionId4, N'بجوار سوق قباء', '2021-02-01', '2021-07-10', N'24.4539,39.6084', 1500.0, 1000.0, 2, N'111222333', N'لا يوجد', N'بئر ارتوازي', N'شبكة الصرف الصحي', N'مسجد تاريخي تم تجديده', N'محمد صالح القحطاني', 1, N'350 مصل', N'user4', 0, 0, N'/images/mosque4.jpg'),
(@BuildingId5, N'مسجد الإمام الشافعي', N'DMM-005', N'مسجد جامع', N'مسجد رئيسي', @OfficeId5, N'الوحدة الخامسة', @RegionId5, N'طريق الملك فهد', '2020-09-15', '2021-01-20', N'26.4207,50.0888', 2200.0, 1600.0, 2, N'444555666', N'طاقة شمسية', N'شبكة المياه العامة', N'شبكة الصرف الصحي', N'مسجد عصري مع قاعة محاضرات', N'سعد عبدالعزيز الدوسري', 0, N'700 مصل', N'user5', 1, 1, N'/images/mosque5.jpg'),
(@BuildingId6, N'مسجد التوحيد', N'TBK-006', N'مسجد حي', N'مسجد فرعي', @OfficeId6, N'الوحدة السادسة', @RegionId6, N'حي السلامة المركزي', '2019-11-05', '2020-04-01', N'28.3838,36.5551', 1300.0, 900.0, 1, N'777888999', N'لا يوجد', N'خزان علوي', N'خزان تجميع', N'مسجد صغير يخدم الحي', N'خالد فهد الشمري', 1, N'250 مصل', N'user6', 0, 0, N'/images/mosque6.jpg'),
(@BuildingId7, N'مسجد الهدى', N'ABH-007', N'مسجد حي', N'مسجد فرعي', @OfficeId7, N'الوحدة السابعة', @RegionId7, N'قرب مطار أبها', '2021-04-12', '2021-09-25', N'18.2156,42.6565', 1600.0, 1100.0, 2, N'222333444', N'لا يوجد', N'شبكة المياه العامة', N'شبكة الصرف الصحي', N'مسجد حديث مع مركز تعليمي', N'أحمد عبدالله عسيري', 0, N'450 مصل', N'user7', 1, 1, N'/images/mosque7.jpg'),
(@BuildingId8, N'مسجد الرحمة', N'TAF-008', N'مسجد جامع', N'مسجد رئيسي', @OfficeId8, N'الوحدة الثامنة', @RegionId8, N'طريق الهدا السياحي', '2018-08-30', '2019-02-15', N'21.2633,40.4127', 2000.0, 1400.0, 2, N'666777888', N'طاقة شمسية', N'شبكة المياه العامة', N'شبكة الصرف الصحي', N'مسجد سياحي مع مرافق للزوار', N'عبدالرحمن محمد الغامدي', 1, N'600 مصل', N'user8', 1, 1, N'/images/mosque8.jpg'),
(@BuildingId9, N'مسجد الفاروق', N'BRD-009', N'مسجد حي', N'مسجد فرعي', @OfficeId9, N'الوحدة التاسعة', @RegionId9, N'سوق بريدة المركزي', '2020-06-20', '2020-11-10', N'26.3260,43.9750', 1400.0, 1000.0, 1, N'999000111', N'لا يوجد', N'خزان علوي', N'خزان تجميع', N'مسجد تجاري يخدم السوق', N'فهد سعد المطيري', 0, N'300 مصل', N'user9', 0, 0, N'/images/mosque9.jpg'),
(@BuildingId10, N'مسجد الأنصار', N'KMS-010', N'مسجد حي', N'مسجد فرعي', @OfficeId10, N'الوحدة العاشرة', @RegionId10, N'مجمع النهضة السكني', '2021-01-08', '2021-06-20', N'18.3000,42.7500', 1700.0, 1200.0, 2, N'333444555', N'لا يوجد', N'شبكة المياه العامة', N'شبكة الصرف الصحي', N'مسجد حديث مع مواقف سيارات', N'ناصر علي الشهراني', 1, N'500 مصل', N'user10', 1, 1, N'/images/mosque10.jpg');

-- إضافة المساجد (Mosques)
PRINT N'إضافة المساجد...'
INSERT INTO [Mosques] ([Id], [BuildingId], [MosqueDefinition], [MosqueClassification]) VALUES
(NEWID(), @BuildingId1, 0, 0),  -- جامع، تصنيف أساسي
(NEWID(), @BuildingId2, 1, 1),  -- مسجد حي، تصنيف فرعي  
(NEWID(), @BuildingId3, 0, 0),  -- جامع، تصنيف أساسي
(NEWID(), @BuildingId4, 1, 1),  -- مسجد حي، تصنيف فرعي
(NEWID(), @BuildingId5, 0, 0),  -- جامع، تصنيف أساسي
(NEWID(), @BuildingId6, 1, 1),  -- مسجد حي، تصنيف فرعي
(NEWID(), @BuildingId7, 1, 1),  -- مسجد حي، تصنيف فرعي
(NEWID(), @BuildingId8, 0, 0),  -- جامع، تصنيف أساسي
(NEWID(), @BuildingId9, 1, 1),  -- مسجد حي، تصنيف فرعي
(NEWID(), @BuildingId10, 1, 1); -- مسجد حي، تصنيف فرعي

-- إضافة الحسابات (Accounts)
PRINT N'إضافة الحسابات...'
INSERT INTO [Accounts] ([Id], [Name], [MotherName], [BirthDate], [Gender], [Barcode], [Status], [LockerFileNumber], [SocialStatus], [BookNumber], [PaperNumber], [RegistrationNumber], [AccountNumber], [Type], [LookOver], [Note], [NID], [IsActive], [Balance], [UserId], [Address], [City], [Country], [ContactNumber], [Floors]) VALUES
(NEWID(), N'أحمد محمد العبدالله', N'فاطمة صالح', '1985-03-15', 1, N'ACC001', 1, 1001, 1, 101, 201, 301, N'AC-2024-001', 3, 0, N'حساب مستفيد رئيسي', 1234567890, 1, 15000.50, N'user1', N'حي العليا، شارع الملك فهد', N'الرياض', N'السعودية', N'0551234567', 2),
(NEWID(), N'فاطمة عبدالرحمن القحطاني', N'عائشة محمد', '1990-07-22', 2, N'ACC002', 1, 1002, 2, 102, 202, 302, N'AC-2024-002', 1, 1, N'حساب أرملة شهيد', 1234567891, 1, 25000.75, N'user2', N'حي الروضة، طريق الملك عبدالعزيز', N'جدة', N'السعودية', N'0552345678', 1),
(NEWID(), N'محمد سعد الدوسري', N'خديجة أحمد', '1978-12-10', 1, 1003, 1, 1003, 1, 103, 203, 303, N'AC-2024-003', 2, 0, N'حساب ذوي الاحتياجات الخاصة', 1234567892, 1, 18000.25, N'user3', N'حي العزيزية، شارع الحرم', N'مكة المكرمة', N'السعودية', N'0553456789', 1),
(NEWID(), N'عائشة خالد الأحمد', N'زينب عبدالله', '1995-09-18', 2, N'ACC004', 1, 1004, 4, 104, 204, 304, N'AC-2024-004', 3, 0, N'حساب مستفيدة عزباء', 1234567893, 1, 12000.00, N'user4', N'حي قباء، طريق قباء', N'المدينة المنورة', N'السعودية', N'0554567890', 1),
(NEWID(), N'عبدالله فهد الشمري', N'مريم صالح', '1982-05-30', 1, N'ACC005', 1, 1005, 1, 105, 205, 305, N'AC-2024-005', 3, 1, N'حساب مستفيد متزوج', 1234567894, 1, 22000.80, N'user5', N'حي الفردوس، شارع الملك فهد', N'الدمام', N'السعودية', N'0555678901', 2),
(NEWID(), N'نورا عبدالعزيز المطيري', N'هند محمد', '1988-11-25', 2, N'ACC006', 1, 1006, 3, 106, 206, 306, N'AC-2024-006', 3, 0, N'حساب مطلقة مع أطفال', 1234567895, 1, 16500.40, N'user6', N'حي السلامة، طريق الأمير فهد', N'تبوك', N'السعودية', N'0556789012', 1),
(NEWID(), N'سالم عبدالله عسيري', N'صفية أحمد', '1975-02-14', 1, N'ACC007', 1, 1007, 1, 107, 207, 307, N'AC-2024-007', 1, 1, N'حساب عائلة شهيد', 1234567896, 1, 30000.90, N'user7', N'حي الخالدية، شارع الملك عبدالله', N'أبها', N'السعودية', N'0557890123', 2),
(NEWID(), N'هدى محمد الغامدي', N'رقية سعد', '1992-08-08', 2, N'ACC008', 1, 1008, 4, 108, 208, 308, N'AC-2024-008', 3, 0, N'حساب خريجة جامعية', 1234567897, 1, 14000.60, N'user8', N'حي الشفا، طريق الهدا', N'الطائف', N'السعودية', N'0558901234', 1),
(NEWID(), N'ماجد فيصل الحربي', N'لطيفة عبدالله', '1980-04-12', 1, N'ACC009', 1, 1009, 1, 109, 209, 309, N'AC-2024-009', 2, 0, N'حساب ذوي إعاقة حركية', 1234567898, 1, 19500.35, N'user9', N'حي الصفراء، شارع الملك سعود', N'بريدة', N'السعودية', N'0559012345', 1),
(NEWID(), N'سارة ناصر الشهراني', N'وداد محمد', '1987-06-05', 2, N'ACC010', 1, 1010, 1, 110, 210, 310, N'AC-2024-010', 3, 1, N'حساب أم عاملة', 1234567899, 1, 17800.45, N'user10', N'حي النهضة، طريق الملك خالد', N'خميس مشيط', N'السعودية', N'0550123456', 2);

-- =====================================================
-- الجزء الثالث: إضافة الجداول التابعة والتفاصيل
-- =====================================================

-- إضافة تفاصيل المباني (BuildingDetails)
PRINT N'إضافة تفاصيل المباني...'
INSERT INTO [BuildingDetails] ([Id], [BuildingId], [ProductId], [Quantity], [Price]) VALUES
(NEWID(), @BuildingId1, @ProductId1, 50, 120.00),  -- مصاحف
(NEWID(), @BuildingId1, @ProductId6, 2, 15000.00), -- أجهزة صوت
(NEWID(), @BuildingId2, @ProductId1, 30, 120.00),  -- مصاحف
(NEWID(), @BuildingId2, @ProductId7, 1, 8000.00),  -- سجاد
(NEWID(), @BuildingId3, @ProductId1, 80, 120.00),  -- مصاحف
(NEWID(), @BuildingId3, @ProductId2, 25, 250.00),  -- كتب تفسير
(NEWID(), @BuildingId4, @ProductId1, 25, 120.00),  -- مصاحف
(NEWID(), @BuildingId4, @ProductId8, 10, 150.00),  -- أدوات وضوء
(NEWID(), @BuildingId5, @ProductId1, 45, 120.00),  -- مصاحف
(NEWID(), @BuildingId5, @ProductId6, 1, 12000.00); -- أجهزة صوت

-- إضافة تفاصيل المرافق (FacilityDetails)
PRINT N'إضافة تفاصيل المرافق...'
DECLARE @FacilityId1 UNIQUEIDENTIFIER = (SELECT TOP 1 Id FROM [Facilities])
INSERT INTO [FacilityDetails] ([Id], [FacilityId], [ProductId], [Quantity], [Price]) VALUES
(NEWID(), @FacilityId1, @ProductId2, 100, 250.00), -- كتب تفسير
(NEWID(), @FacilityId1, @ProductId3, 75, 180.00),  -- كتب حديث
(NEWID(), @FacilityId1, @ProductId4, 50, 300.00),  -- كتب فقه
(NEWID(), @FacilityId1, @ProductId5, 200, 85.00),  -- كتب أطفال
(NEWID(), @FacilityId1, @ProductId9, 15, 450.00),  -- لوحات إسلامية
(NEWID(), @FacilityId1, @ProductId10, 10, 800.00), -- مستلزمات مكتبة
(NEWID(), @FacilityId1, @ProductId1, 150, 120.00), -- مصاحف
(NEWID(), @FacilityId1, @ProductId6, 3, 15000.00), -- أجهزة صوت
(NEWID(), @FacilityId1, @ProductId7, 5, 8000.00),  -- سجاد
(NEWID(), @FacilityId1, @ProductId8, 25, 150.00);  -- أدوات وضوء

-- إضافة تفاصيل الحسابات (AccountDetails)
PRINT N'إضافة تفاصيل الحسابات...'
DECLARE @AccountId1 UNIQUEIDENTIFIER = (SELECT TOP 1 Id FROM [Accounts])
INSERT INTO [AccountDetails] ([Id], [AccountId], [TransactionDate], [Amount], [Description], [TransactionType]) VALUES
(NEWID(), @AccountId1, '2024-01-15', 5000.00, N'مساعدة شهرية', N'إيداع'),
(NEWID(), @AccountId1, '2024-01-20', -500.00, N'مساعدة طبية', N'سحب'),
(NEWID(), @AccountId1, '2024-02-15', 5000.00, N'مساعدة شهرية', N'إيداع'),
(NEWID(), @AccountId1, '2024-02-25', -1200.00, N'مساعدة تعليمية', N'سحب'),
(NEWID(), @AccountId1, '2024-03-15', 5000.00, N'مساعدة شهرية', N'إيداع'),
(NEWID(), @AccountId1, '2024-03-30', -800.00, N'مساعدة غذائية', N'سحب'),
(NEWID(), @AccountId1, '2024-04-15', 5000.00, N'مساعدة شهرية', N'إيداع'),
(NEWID(), @AccountId1, '2024-04-28', -300.00, N'مساعدة مواصلات', N'سحب'),
(NEWID(), @AccountId1, '2024-05-15', 5000.00, N'مساعدة شهرية', N'إيداع'),
(NEWID(), @AccountId1, '2024-05-22', -1000.00, N'مساعدة سكن', N'سحب');

-- إضافة القرارات (Decisions)
PRINT N'إضافة القرارات...'
INSERT INTO [Decisions] ([Id], [Number], [Date], [Description], [Status]) VALUES
(NEWID(), N'QR-2024-001', '2024-01-10', N'قرار إنشاء مسجد جديد في حي العليا بمدينة الرياض', N'معتمد'),
(NEWID(), N'QR-2024-002', '2024-01-15', N'قرار تخصيص مساعدات للأرامل والأيتام في منطقة جدة', N'معتمد'),
(NEWID(), N'QR-2024-003', '2024-02-01', N'قرار إنشاء مركز تحفيظ القرآن الكريم في مكة المكرمة', N'قيد المراجعة'),
(NEWID(), N'QR-2024-004', '2024-02-10', N'قرار توزيع مصاحف على المساجد الجديدة', N'معتمد'),
(NEWID(), N'QR-2024-005', '2024-02-20', N'قرار إنشاء مكتبة إسلامية في المدينة المنورة', N'معتمد'),
(NEWID(), N'QR-2024-006', '2024-03-01', N'قرار تجديد مسجد قديم في منطقة الدمام', N'قيد التنفيذ'),
(NEWID(), N'QR-2024-007', '2024-03-15', N'قرار توفير أجهزة صوت للمساجد في منطقة تبوك', N'معتمد'),
(NEWID(), N'QR-2024-008', '2024-04-01', N'قرار إنشاء معهد للعلوم الشرعية في أبها', N'قيد المراجعة'),
(NEWID(), N'QR-2024-009', '2024-04-20', N'قرار تخصيص مساعدات لذوي الاحتياجات الخاصة', N'معتمد'),
(NEWID(), N'QR-2024-010', '2024-05-01', N'قرار إنشاء دار للقرآن الكريم في الطائف', N'قيد الدراسة');

-- إضافة طلبات البناء (ConstructionRequests)
PRINT N'إضافة طلبات البناء...'
INSERT INTO [ConstructionRequests] ([Id], [RequestNumber], [RequestDate], [ProjectName], [Location], [EstimatedCost], [Description], [Status], [UserId]) VALUES
(NEWID(), N'CR-2024-001', '2024-01-15', N'مسجد النور الجديد', N'حي الياسمين، الرياض', 2500000.00, N'بناء مسجد جديد يتسع لـ 500 مصل مع مرافق متكاملة', N'قيد المراجعة', N'user1'),
(NEWID(), N'CR-2024-002', '2024-02-01', N'مركز الدعوة والإرشاد', N'حي الفيصلية، جدة', 1800000.00, N'إنشاء مركز دعوي مع قاعة محاضرات ومكتبة', N'معتمد', N'user2'),
(NEWID(), N'CR-2024-003', '2024-02-20', N'مسجد الهداية', N'حي الشرفية، مكة المكرمة', 3200000.00, N'بناء مسجد كبير يتسع لـ 800 مصل بالقرب من الحرم', N'قيد التنفيذ', N'user3'),
(NEWID(), N'CR-2024-004', '2024-03-10', N'معهد العلوم القرآنية', N'حي العوالي، المدينة المنورة', 4500000.00, N'إنشاء معهد متخصص لتعليم القرآن والعلوم الشرعية', N'قيد المراجعة', N'user4'),
(NEWID(), N'CR-2024-005', '2024-03-25', N'مسجد التوحيد', N'حي الواحة، الدمام', 2200000.00, N'بناء مسجد عصري مع مواقف سيارات ومرافق للمعاقين', N'معتمد', N'user5'),
(NEWID(), N'CR-2024-006', '2024-04-05', N'مركز الأنشطة الإسلامية', N'حي القادسية، تبوك', 1500000.00, N'إنشاء مركز للأنشطة الإسلامية والثقافية للشباب', N'قيد الدراسة', N'user6'),
(NEWID(), N'CR-2024-007', '2024-04-20', N'مسجد الرحمة', N'حي المنصورة، أبها', 2800000.00, N'بناء مسجد مع مركز تحفيظ ومكتبة للأطفال', N'قيد المراجعة', N'user7'),
(NEWID(), N'CR-2024-008', '2024-05-01', N'دار القرآن الكريم', N'حي السلامة، الطائف', 3500000.00, N'إنشاء دار متخصصة لتحفيظ القرآن مع سكن للطلاب', N'معتمد', N'user8'),
(NEWID(), N'CR-2024-009', '2024-05-15', N'مسجد الفاروق', N'حي الخزامى، بريدة', 1900000.00, N'بناء مسجد متوسط مع قاعة للمناسبات', N'قيد التنفيذ', N'user9'),
(NEWID(), N'CR-2024-010', '2024-06-01', N'مركز العلوم الشرعية', N'حي الموسى، خميس مشيط', 2600000.00, N'إنشاء مركز تعليمي للعلوم الشرعية والدعوة', N'قيد الدراسة', N'user10');

-- إضافة طلبات الصيانة (MaintenanceRequests)
PRINT N'إضافة طلبات الصيانة...'
INSERT INTO [MaintenanceRequests] ([Id], [RequestNumber], [RequestDate], [BuildingId], [MaintenanceType], [Description], [Priority], [EstimatedCost], [Status], [UserId]) VALUES
(NEWID(), N'MR-2024-001', '2024-01-20', @BuildingId1, N'صيانة كهربائية', N'استبدال لوحة الكهرباء الرئيسية وإصلاح الإضاءة', N'عالية', 25000.00, N'مكتملة', N'user1'),
(NEWID(), N'MR-2024-002', '2024-02-05', @BuildingId2, N'صيانة سباكة', N'إصلاح تسريب في نظام المياه وتنظيف الخزانات', N'متوسطة', 15000.00, N'قيد التنفيذ', N'user2'),
(NEWID(), N'MR-2024-003', '2024-02-15', @BuildingId3, N'صيانة تكييف', N'فحص وصيانة أجهزة التكييف وتنظيف الفلاتر', N'عالية', 35000.00, N'معتمدة', N'user3'),
(NEWID(), N'MR-2024-004', '2024-03-01', @BuildingId4, N'صيانة عامة', N'دهان الجدران وإصلاح الأرضيات المتضررة', N'منخفضة', 20000.00, N'قيد المراجعة', N'user4'),
(NEWID(), N'MR-2024-005', '2024-03-20', @BuildingId5, N'صيانة أنظمة الصوت', N'إصلاح نظام الصوت وتركيب مكبرات صوت جديدة', N'متوسطة', 18000.00, N'مكتملة', N'user5'),
(NEWID(), N'MR-2024-006', '2024-04-10', @BuildingId6, N'صيانة الأبواب والنوافذ', N'إصلاح الأبواب واستبدال النوافذ المكسورة', N'متوسطة', 12000.00, N'قيد التنفيذ', N'user6'),
(NEWID(), N'MR-2024-007', '2024-04-25', @BuildingId7, N'صيانة السقف', N'إصلاح تسريب في السقف وعزل مائي جديد', N'عالية', 40000.00, N'معتمدة', N'user7'),
(NEWID(), N'MR-2024-008', '2024-05-05', @BuildingId8, N'صيانة أرضيات', N'تجديد السجاد واستبدال البلاط المتضرر', N'منخفضة', 22000.00, N'قيد المراجعة', N'user8'),
(NEWID(), N'MR-2024-009', '2024-05-20', @BuildingId9, N'صيانة كهربائية', N'تحديث الأسلاك الكهربائية وتركيب قواطع جديدة', N'عالية', 28000.00, N'قيد التنفيذ', N'user9'),
(NEWID(), N'MR-2024-010', '2024-06-01', @BuildingId10, N'صيانة عامة شاملة', N'صيانة شاملة للمبنى تشمل الكهرباء والسباكة والدهان', N'عالية', 55000.00, N'قيد الدراسة', N'user10');

-- =====================================================
-- الجزء الرابع: إضافة الطلبات المتنوعة
-- =====================================================

-- إضافة طلبات الهدم (DemolitionRequests)
PRINT N'إضافة طلبات الهدم...'
INSERT INTO [DemolitionRequests] ([Id], [RequestNumber], [RequestDate], [BuildingId], [Reason], [Description], [EstimatedCost], [Status], [UserId]) VALUES
(NEWID(), N'DR-2024-001', '2024-01-25', @BuildingId1, N'تطوير المنطقة', N'هدم مبنى قديم لإنشاء مسجد أكبر وأحدث', 150000.00, N'قيد المراجعة', N'user1'),
(NEWID(), N'DR-2024-002', '2024-02-10', @BuildingId2, N'أضرار هيكلية', N'هدم مبنى متضرر من الأمطار والسيول', 120000.00, N'معتمدة', N'user2'),
(NEWID(), N'DR-2024-003', '2024-02-28', @BuildingId3, N'إعادة تخطيط', N'هدم جزئي لإعادة تصميم المساحات الداخلية', 80000.00, N'قيد التنفيذ', N'user3'),
(NEWID(), N'DR-2024-004', '2024-03-15', @BuildingId4, N'مخالفات بناء', N'هدم إضافات غير مرخصة في المبنى', 60000.00, N'مكتملة', N'user4'),
(NEWID(), N'DR-2024-005', '2024-04-01', @BuildingId5, N'تحديث المرافق', N'هدم مرافق قديمة لإنشاء مرافق عصرية', 200000.00, N'قيد المراجعة', N'user5'),
(NEWID(), N'DR-2024-006', '2024-04-18', @BuildingId6, N'توسعة الطريق', N'هدم جزء من المبنى لتوسعة الطريق المجاور', 90000.00, N'معتمدة', N'user6'),
(NEWID(), N'DR-2024-007', '2024-05-02', @BuildingId7, N'أضرار حريق', N'هدم أجزاء متضررة من حريق قديم', 110000.00, N'قيد التنفيذ', N'user7'),
(NEWID(), N'DR-2024-008', '2024-05-20', @BuildingId8, N'تطوير عمراني', N'هدم مباني قديمة ضمن مشروع تطوير شامل', 300000.00, N'قيد الدراسة', N'user8'),
(NEWID(), N'DR-2024-009', '2024-06-05', @BuildingId9, N'أضرار زلزال', N'هدم مبنى متضرر من النشاط الزلزالي', 180000.00, N'قيد المراجعة', N'user9'),
(NEWID(), N'DR-2024-010', '2024-06-15', @BuildingId10, N'انتهاء العمر الافتراضي', N'هدم مبنى وصل لنهاية عمره الافتراضي', 250000.00, N'قيد الدراسة', N'user10');

-- إضافة طلبات تغيير الاسم (NameChangeRequests)
PRINT N'إضافة طلبات تغيير الاسم...'
INSERT INTO [NameChangeRequests] ([Id], [RequestNumber], [RequestDate], [CurrentName], [ProposedName], [Reason], [Status], [UserId]) VALUES
(NEWID(), N'NCR-2024-001', '2024-01-30', N'مسجد النور', N'مسجد الملك سلمان', N'تكريماً لخادم الحرمين الشريفين', N'قيد المراجعة', N'user1'),
(NEWID(), N'NCR-2024-002', '2024-02-12', N'مركز التعليم', N'مركز الأمير محمد بن سلمان للتعليم', N'تكريماً لولي العهد', N'معتمدة', N'user2'),
(NEWID(), N'NCR-2024-003', '2024-02-25', N'مسجد الهداية', N'مسجد الشيخ عبدالعزيز بن باز', N'تكريماً للمفتي الراحل', N'قيد التنفيذ', N'user3'),
(NEWID(), N'NCR-2024-004', '2024-03-10', N'دار القرآن', N'دار الملك عبدالعزيز للقرآن الكريم', N'تكريماً للملك المؤسس', N'مكتملة', N'user4'),
(NEWID(), N'NCR-2024-005', '2024-03-28', N'مسجد السلام', N'مسجد الأمير عبدالله الفيصل', N'تكريماً للأمير الراحل', N'قيد المراجعة', N'user5'),
(NEWID(), N'NCR-2024-006', '2024-04-15', N'مركز الدعوة', N'مركز الشيخ ابن عثيمين للدعوة', N'تكريماً للشيخ الراحل', N'معتمدة', N'user6'),
(NEWID(), N'NCR-2024-007', '2024-05-01', N'مسجد التوحيد', N'مسجد الإمام محمد بن سعود', N'تكريماً للإمام المؤسس', N'قيد التنفيذ', N'user7'),
(NEWID(), N'NCR-2024-008', '2024-05-18', N'معهد العلوم', N'معهد الملك فهد للعلوم الإسلامية', N'تكريماً للملك الراحل', N'قيد المراجعة', N'user8'),
(NEWID(), N'NCR-2024-009', '2024-06-02', N'مسجد الفجر', N'مسجد الأمير نايف', N'تكريماً للأمير الراحل', N'معتمدة', N'user9'),
(NEWID(), N'NCR-2024-010', '2024-06-20', N'دار الحديث', N'دار الشيخ الألباني للحديث', N'تكريماً للمحدث الراحل', N'قيد الدراسة', N'user10');

-- إضافة طلبات تغيير النفقات (ExpenditureChangeRequests)
PRINT N'إضافة طلبات تغيير النفقات...'
INSERT INTO [ExpenditureChangeRequests] ([Id], [RequestNumber], [RequestDate], [CurrentAmount], [ProposedAmount], [Reason], [Description], [Status], [UserId]) VALUES
(NEWID(), N'ECR-2024-001', '2024-02-01', 50000.00, 75000.00, N'زيادة تكاليف المواد', N'ارتفاع أسعار مواد البناء والتشطيبات', N'معتمدة', N'user1'),
(NEWID(), N'ECR-2024-002', '2024-02-15', 120000.00, 100000.00, N'توفير في التكاليف', N'الحصول على خصم من المقاول الرئيسي', N'قيد المراجعة', N'user2'),
(NEWID(), N'ECR-2024-003', '2024-03-01', 200000.00, 250000.00, N'إضافة مرافق جديدة', N'طلب إضافة قاعة محاضرات ومكتبة', N'قيد التنفيذ', N'user3'),
(NEWID(), N'ECR-2024-004', '2024-03-20', 80000.00, 65000.00, N'تقليل المواصفات', N'تبسيط بعض التشطيبات لخفض التكلفة', N'مرفوضة', N'user4'),
(NEWID(), N'ECR-2024-005', '2024-04-05', 150000.00, 180000.00, N'تحديث التقنيات', N'إضافة أنظمة ذكية وتقنيات حديثة', N'معتمدة', N'user5'),
(NEWID(), N'ECR-2024-006', '2024-04-25', 90000.00, 110000.00, N'زيادة مساحة البناء', N'توسيع المساحة المخصصة للصلاة', N'قيد المراجعة', N'user6'),
(NEWID(), N'ECR-2024-007', '2024-05-10', 300000.00, 270000.00, N'إعادة تفاوض العقود', N'الحصول على أسعار أفضل من الموردين', N'قيد التنفيذ', N'user7'),
(NEWID(), N'ECR-2024-008', '2024-05-28', 175000.00, 200000.00, N'متطلبات إضافية', N'إضافة مرافق لذوي الاحتياجات الخاصة', N'معتمدة', N'user8'),
(NEWID(), N'ECR-2024-009', '2024-06-12', 140000.00, 125000.00, N'تبسيط التصميم', N'تعديل التصميم لتقليل التعقيدات', N'قيد المراجعة', N'user9'),
(NEWID(), N'ECR-2024-010', '2024-06-25', 220000.00, 280000.00, N'تحسين جودة المواد', N'استخدام مواد عالية الجودة ومقاومة للعوامل الجوية', N'قيد الدراسة', N'user10');

-- =====================================================
-- الجزء الخامس: رسائل التأكيد والتقرير النهائي
-- =====================================================

PRINT N'=================================='
PRINT N'تم إضافة جميع البيانات التجريبية بنجاح!'
PRINT N'All sample data inserted successfully!'
PRINT N'=================================='

-- تقرير إحصائي بعدد السجلات في كل جدول
PRINT N''
PRINT N'=== تقرير إحصائي للبيانات المدرجة ==='
PRINT N'Statistical Report of Inserted Data'
PRINT N'=================================='

SELECT 
    N'البنوك (Banks)' AS TableName,
    COUNT(*) AS RecordCount
FROM [Banks]
UNION ALL
SELECT 
    N'المدن (Cities)' AS TableName,
    COUNT(*) AS RecordCount
FROM [Cities]
UNION ALL
SELECT 
    N'المناطق (Regions)' AS TableName,
    COUNT(*) AS RecordCount
FROM [Regions]
UNION ALL
SELECT 
    N'المنتجات (Products)' AS TableName,
    COUNT(*) AS RecordCount
FROM [Products]
UNION ALL
SELECT 
    N'المكاتب (Offices)' AS TableName,
    COUNT(*) AS RecordCount
FROM [Offices]
UNION ALL
SELECT 
    N'المرافق (Facilities)' AS TableName,
    COUNT(*) AS RecordCount
FROM [Facilities]
UNION ALL
SELECT 
    N'المباني (Buildings)' AS TableName,
    COUNT(*) AS RecordCount
FROM [Buildings]
UNION ALL
SELECT 
    N'المساجد (Mosques)' AS TableName,
    COUNT(*) AS RecordCount
FROM [Mosques]
UNION ALL
SELECT 
    N'الحسابات (Accounts)' AS TableName,
    COUNT(*) AS RecordCount
FROM [Accounts]
UNION ALL
SELECT 
    N'القرارات (Decisions)' AS TableName,
    COUNT(*) AS RecordCount
FROM [Decisions]
ORDER BY RecordCount DESC;

PRINT N''
PRINT N'تم الانتهاء من العملية بنجاح!'
PRINT N'Operation completed successfully!'
PRINT N'جميع النصوص تم إدراجها بالترميز العربي الصحيح N'''''
PRINT N'All Arabic text has been inserted with proper encoding N'''''

GO