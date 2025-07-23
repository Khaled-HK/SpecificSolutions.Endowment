-- إدخال بيانات المناطق (Regions)
-- جدول Regions يحتوي على: Id (uniqueidentifier), Name (nvarchar(200)), Country (nvarchar(100)), CityId (uniqueidentifier)
-- ملاحظة: يجب أن يكون CityId موجود في جدول Cities

-- أولاً نحتاج للحصول على CityId من جدول Cities
DECLARE @TripoliId uniqueidentifier = (SELECT TOP 1 Id FROM Cities WHERE Name = 'طرابلس')
DECLARE @BenghaziId uniqueidentifier = (SELECT TOP 1 Id FROM Cities WHERE Name = 'بنغازي')
DECLARE @MisrataId uniqueidentifier = (SELECT TOP 1 Id FROM Cities WHERE Name = 'مصراتة')
DECLARE @ZawiyaId uniqueidentifier = (SELECT TOP 1 Id FROM Cities WHERE Name = 'الزاوية')
DECLARE @AlBaydaId uniqueidentifier = (SELECT TOP 1 Id FROM Cities WHERE Name = 'البيضاء')

INSERT INTO Regions (Id, Name, Country, CityId) VALUES
-- مناطق طرابلس
(NEWID(), 'وسط طرابلس', 'ليبيا', @TripoliId),
(NEWID(), 'حي الأندلس', 'ليبيا', @TripoliId),
(NEWID(), 'حي النصر', 'ليبيا', @TripoliId),
(NEWID(), 'حي الهضبة', 'ليبيا', @TripoliId),
(NEWID(), 'حي سيدي المصري', 'ليبيا', @TripoliId),
(NEWID(), 'حي باب العزيزية', 'ليبيا', @TripoliId),
(NEWID(), 'حي باب تاجوراء', 'ليبيا', @TripoliId),
(NEWID(), 'حي أبو سليم', 'ليبيا', @TripoliId),
(NEWID(), 'حي عين زارة', 'ليبيا', @TripoliId),
(NEWID(), 'حي تاجوراء', 'ليبيا', @TripoliId),

-- مناطق بنغازي
(NEWID(), 'وسط بنغازي', 'ليبيا', @BenghaziId),
(NEWID(), 'حي الصابري', 'ليبيا', @BenghaziId),
(NEWID(), 'حي سيدي خريبيش', 'ليبيا', @BenghaziId),
(NEWID(), 'حي الكويفية', 'ليبيا', @BenghaziId),
(NEWID(), 'حي البركة', 'ليبيا', @BenghaziId),
(NEWID(), 'حي سيدي حسين', 'ليبيا', @BenghaziId),
(NEWID(), 'حي القوارشة', 'ليبيا', @BenghaziId),
(NEWID(), 'حي الهواري', 'ليبيا', @BenghaziId),
(NEWID(), 'حي الجواري', 'ليبيا', @BenghaziId),
(NEWID(), 'حي المقرون', 'ليبيا', @BenghaziId),

-- مناطق مصراتة
(NEWID(), 'وسط مصراتة', 'ليبيا', @MisrataId),
(NEWID(), 'حي الزاوية', 'ليبيا', @MisrataId),
(NEWID(), 'حي سوق الثلاثاء', 'ليبيا', @MisrataId),
(NEWID(), 'حي سوق الجمعة', 'ليبيا', @MisrataId),
(NEWID(), 'حي القره بولي', 'ليبيا', @MisrataId),
(NEWID(), 'حي تاوورغا', 'ليبيا', @MisrataId),
(NEWID(), 'حي زليتن', 'ليبيا', @MisrataId),
(NEWID(), 'حي الخمس', 'ليبيا', @MisrataId),
(NEWID(), 'حي تاجوراء', 'ليبيا', @MisrataId),
(NEWID(), 'حي أبو سليم', 'ليبيا', @MisrataId),

-- مناطق الزاوية
(NEWID(), 'وسط الزاوية', 'ليبيا', @ZawiyaId),
(NEWID(), 'حي النصر', 'ليبيا', @ZawiyaId),
(NEWID(), 'حي الوحدة', 'ليبيا', @ZawiyaId),
(NEWID(), 'حي الشهداء', 'ليبيا', @ZawiyaId),
(NEWID(), 'حي الفتح', 'ليبيا', @ZawiyaId),
(NEWID(), 'حي الثورة', 'ليبيا', @ZawiyaId),
(NEWID(), 'حي الحرية', 'ليبيا', @ZawiyaId),
(NEWID(), 'حي الاستقلال', 'ليبيا', @ZawiyaId),
(NEWID(), 'حي الوادي', 'ليبيا', @ZawiyaId),
(NEWID(), 'حي الساحل', 'ليبيا', @ZawiyaId),

-- مناطق البيضاء
(NEWID(), 'وسط البيضاء', 'ليبيا', @AlBaydaId),
(NEWID(), 'حي الجبل الأخضر', 'ليبيا', @AlBaydaId),
(NEWID(), 'حي المرج', 'ليبيا', @AlBaydaId),
(NEWID(), 'حي درنة', 'ليبيا', @AlBaydaId),
(NEWID(), 'حي شحات', 'ليبيا', @AlBaydaId),
(NEWID(), 'حي طبرق', 'ليبيا', @AlBaydaId),
(NEWID(), 'حي سرت', 'ليبيا', @AlBaydaId),
(NEWID(), 'حي أجدابيا', 'ليبيا', @AlBaydaId),
(NEWID(), 'حي سبها', 'ليبيا', @AlBaydaId),
(NEWID(), 'حي غات', 'ليبيا', @AlBaydaId); 