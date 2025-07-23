-- إدخال بيانات القرارات (Decisions)
-- جدول Decisions يحتوي على: Id (uniqueidentifier), Title (nvarchar(200)), Description (nvarchar(1000)), CreatedDate (datetime2), ReferenceNumber (nvarchar(50)), UserId (nvarchar(450))
-- ملاحظة: UserId يجب أن يكون موجود في جدول AspNetUsers

-- أولاً نحتاج للحصول على UserId من جدول AspNetUsers
DECLARE @AdminUserId nvarchar(450) = (SELECT TOP 1 Id FROM AspNetUsers WHERE UserName = 'admin' OR Email = 'admin@example.com')

-- إذا لم يوجد مستخدم، نستخدم قيمة افتراضية (يجب إنشاء مستخدم أولاً)
IF @AdminUserId IS NULL
BEGIN
    SET @AdminUserId = '00000000-0000-0000-0000-000000000000'
END

INSERT INTO Decisions (Id, Title, Description, CreatedDate, ReferenceNumber, UserId) VALUES
-- قرارات إدارية
(NEWID(), 'قرار تعيين مدير عام', 'قرار بتعيين السيد أحمد محمد علي مديراً عاماً للمؤسسة', DATEADD(day, -365, GETDATE()), 'DEC-2024-001', @AdminUserId),
(NEWID(), 'قرار إنشاء فرع جديد', 'قرار بإنشاء فرع جديد في مدينة بنغازي', DATEADD(day, -350, GETDATE()), 'DEC-2024-002', @AdminUserId),
(NEWID(), 'قرار شراء معدات', 'قرار بشراء معدات مكتبية جديدة', DATEADD(day, -340, GETDATE()), 'DEC-2024-003', @AdminUserId),
(NEWID(), 'قرار تطوير النظام', 'قرار بتطوير النظام الإلكتروني للمؤسسة', DATEADD(day, -330, GETDATE()), 'DEC-2024-004', @AdminUserId),
(NEWID(), 'قرار تدريب الموظفين', 'قرار بتنظيم دورات تدريبية للموظفين', DATEADD(day, -320, GETDATE()), 'DEC-2024-005', @AdminUserId),

-- قرارات مالية
(NEWID(), 'قرار الميزانية السنوية', 'قرار بإنشاء الميزانية السنوية لعام 2024', DATEADD(day, -310, GETDATE()), 'DEC-2024-006', @AdminUserId),
(NEWID(), 'قرار زيادة الرواتب', 'قرار بزيادة رواتب الموظفين بنسبة 10%', DATEADD(day, -300, GETDATE()), 'DEC-2024-007', @AdminUserId),
(NEWID(), 'قرار شراء عقار', 'قرار بشراء عقار جديد للمؤسسة', DATEADD(day, -290, GETDATE()), 'DEC-2024-008', @AdminUserId),
(NEWID(), 'قرار استثمار أموال', 'قرار باستثمار الأموال في مشاريع مربحة', DATEADD(day, -280, GETDATE()), 'DEC-2024-009', @AdminUserId),
(NEWID(), 'قرار إعادة هيكلة مالية', 'قرار بإعادة هيكلة الوضع المالي للمؤسسة', DATEADD(day, -270, GETDATE()), 'DEC-2024-010', @AdminUserId),

-- قرارات تنظيمية
(NEWID(), 'قرار تعديل اللوائح', 'قرار بتعديل اللوائح الداخلية للمؤسسة', DATEADD(day, -260, GETDATE()), 'DEC-2024-011', @AdminUserId),
(NEWID(), 'قرار إنشاء أقسام جديدة', 'قرار بإنشاء أقسام جديدة في المؤسسة', DATEADD(day, -250, GETDATE()), 'DEC-2024-012', @AdminUserId),
(NEWID(), 'قرار إعادة تنظيم الهيكل', 'قرار بإعادة تنظيم الهيكل التنظيمي', DATEADD(day, -240, GETDATE()), 'DEC-2024-013', @AdminUserId),
(NEWID(), 'قرار إنشاء لجان', 'قرار بإنشاء لجان متخصصة', DATEADD(day, -230, GETDATE()), 'DEC-2024-014', @AdminUserId),
(NEWID(), 'قرار تطوير السياسات', 'قرار بتطوير السياسات الإدارية', DATEADD(day, -220, GETDATE()), 'DEC-2024-015', @AdminUserId),

-- قرارات مشاريع
(NEWID(), 'قرار بدء مشروع جديد', 'قرار ببدء مشروع تطوير البنية التحتية', DATEADD(day, -210, GETDATE()), 'DEC-2024-016', @AdminUserId),
(NEWID(), 'قرار توقيع عقد', 'قرار بتوقيع عقد مع شركة مقاولات', DATEADD(day, -200, GETDATE()), 'DEC-2024-017', @AdminUserId),
(NEWID(), 'قرار إيقاف مشروع', 'قرار بإيقاف مشروع غير مجدٍ', DATEADD(day, -190, GETDATE()), 'DEC-2024-018', @AdminUserId),
(NEWID(), 'قرار توسيع مشروع', 'قرار بتوسيع نطاق مشروع قائم', DATEADD(day, -180, GETDATE()), 'DEC-2024-019', @AdminUserId),
(NEWID(), 'قرار إنهاء مشروع', 'قرار بإنهاء مشروع بنجاح', DATEADD(day, -170, GETDATE()), 'DEC-2024-020', @AdminUserId),

-- قرارات موارد بشرية
(NEWID(), 'قرار تعيين موظفين', 'قرار بتعيين 10 موظفين جدد', DATEADD(day, -160, GETDATE()), 'DEC-2024-021', @AdminUserId),
(NEWID(), 'قرار ترقية موظفين', 'قرار بترقية 5 موظفين متميزين', DATEADD(day, -150, GETDATE()), 'DEC-2024-022', @AdminUserId),
(NEWID(), 'قرار إنهاء خدمات', 'قرار بإنهاء خدمات موظفين', DATEADD(day, -140, GETDATE()), 'DEC-2024-023', @AdminUserId),
(NEWID(), 'قرار إجازة إدارية', 'قرار بمنح إجازة إدارية لموظف', DATEADD(day, -130, GETDATE()), 'DEC-2024-024', @AdminUserId),
(NEWID(), 'قرار نقل موظف', 'قرار بنقل موظف إلى فرع آخر', DATEADD(day, -120, GETDATE()), 'DEC-2024-025', @AdminUserId),

-- قرارات تقنية
(NEWID(), 'قرار تحديث أنظمة', 'قرار بتحديث الأنظمة التقنية', DATEADD(day, -110, GETDATE()), 'DEC-2024-026', @AdminUserId),
(NEWID(), 'قرار شراء أجهزة', 'قرار بشراء أجهزة كمبيوتر جديدة', DATEADD(day, -100, GETDATE()), 'DEC-2024-027', @AdminUserId),
(NEWID(), 'قرار تطوير موقع', 'قرار بتطوير الموقع الإلكتروني', DATEADD(day, -90, GETDATE()), 'DEC-2024-028', @AdminUserId),
(NEWID(), 'قرار أمن معلومات', 'قرار بتحسين أمن المعلومات', DATEADD(day, -80, GETDATE()), 'DEC-2024-029', @AdminUserId),
(NEWID(), 'قرار نسخ احتياطي', 'قرار بإنشاء نظام نسخ احتياطي', DATEADD(day, -70, GETDATE()), 'DEC-2024-030', @AdminUserId),

-- قرارات خدمية
(NEWID(), 'قرار تحسين الخدمات', 'قرار بتحسين جودة الخدمات المقدمة', DATEADD(day, -60, GETDATE()), 'DEC-2024-031', @AdminUserId),
(NEWID(), 'قرار إنشاء خدمة جديدة', 'قرار بإنشاء خدمة إلكترونية جديدة', DATEADD(day, -50, GETDATE()), 'DEC-2024-032', @AdminUserId),
(NEWID(), 'قرار تطوير خدمة', 'قرار بتطوير الخدمات الحالية', DATEADD(day, -40, GETDATE()), 'DEC-2024-033', @AdminUserId),
(NEWID(), 'قرار إيقاف خدمة', 'قرار بإيقاف خدمة غير مستخدمة', DATEADD(day, -30, GETDATE()), 'DEC-2024-034', @AdminUserId),
(NEWID(), 'قرار توسيع خدمات', 'قرار بتوسيع نطاق الخدمات', DATEADD(day, -20, GETDATE()), 'DEC-2024-035', @AdminUserId),

-- قرارات أمنية
(NEWID(), 'قرار تحسين الأمن', 'قرار بتحسين الإجراءات الأمنية', DATEADD(day, -15, GETDATE()), 'DEC-2024-036', @AdminUserId),
(NEWID(), 'قرار إنشاء نظام مراقبة', 'قرار بإنشاء نظام مراقبة أمنية', DATEADD(day, -14, GETDATE()), 'DEC-2024-037', @AdminUserId),
(NEWID(), 'قرار تعيين حراس', 'قرار بتعيين حراس أمن جدد', DATEADD(day, -13, GETDATE()), 'DEC-2024-038', @AdminUserId),
(NEWID(), 'قرار تحديث أنظمة أمنية', 'قرار بتحديث الأنظمة الأمنية', DATEADD(day, -12, GETDATE()), 'DEC-2024-039', @AdminUserId),
(NEWID(), 'قرار تدريب أمني', 'قرار بتنظيم تدريبات أمنية', DATEADD(day, -11, GETDATE()), 'DEC-2024-040', @AdminUserId),

-- قرارات صيانة
(NEWID(), 'قرار صيانة دورية', 'قرار بإجراء صيانة دورية للمباني', DATEADD(day, -10, GETDATE()), 'DEC-2024-041', @AdminUserId),
(NEWID(), 'قرار إصلاح عاجل', 'قرار بإصلاح عاجل لبعض المرافق', DATEADD(day, -9, GETDATE()), 'DEC-2024-042', @AdminUserId),
(NEWID(), 'قرار تحديث مرافق', 'قرار بتحديث المرافق القديمة', DATEADD(day, -8, GETDATE()), 'DEC-2024-043', @AdminUserId),
(NEWID(), 'قرار شراء معدات صيانة', 'قرار بشراء معدات صيانة جديدة', DATEADD(day, -7, GETDATE()), 'DEC-2024-044', @AdminUserId),
(NEWID(), 'قرار تعيين فنيين', 'قرار بتعيين فنيي صيانة جدد', DATEADD(day, -6, GETDATE()), 'DEC-2024-045', @AdminUserId),

-- قرارات تعليمية
(NEWID(), 'قرار إنشاء مركز تدريب', 'قرار بإنشاء مركز تدريب متخصص', DATEADD(day, -5, GETDATE()), 'DEC-2024-046', @AdminUserId),
(NEWID(), 'قرار تنظيم ورش عمل', 'قرار بتنظيم ورش عمل للموظفين', DATEADD(day, -4, GETDATE()), 'DEC-2024-047', @AdminUserId),
(NEWID(), 'قرار إرسال بعثات', 'قرار بإرسال بعثات تدريبية', DATEADD(day, -3, GETDATE()), 'DEC-2024-048', @AdminUserId),
(NEWID(), 'قرار تطوير مهارات', 'قرار بتطوير مهارات الموظفين', DATEADD(day, -2, GETDATE()), 'DEC-2024-049', @AdminUserId),
(NEWID(), 'قرار إنشاء مكتبة', 'قرار بإنشاء مكتبة للمؤسسة', DATEADD(day, -1, GETDATE()), 'DEC-2024-050', @AdminUserId); 