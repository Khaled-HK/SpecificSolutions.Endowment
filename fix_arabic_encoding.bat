@echo off
echo ================================================
echo         إصلاح مشاكل الترميز العربي
echo         Arabic Encoding Issues Fix
echo ================================================
echo.

echo 🔧 المشكلة المكتشفة:
echo البيانات تظهر كـ: ظ…ظƒطھط¨ ط§ظ„ط£ظˆظ‚ط§ظپ
echo يجب أن تظهر كـ: مكتب الأوقاف
echo.

echo 🚀 بدء إصلاح مشاكل الترميز...
echo.

REM التحقق من وجود ملف الإصلاح
if not exist "fix_arabic_encoding.sql" (
    echo ❌ خطأ: ملف fix_arabic_encoding.sql غير موجود!
    echo ❌ Error: fix_arabic_encoding.sql file not found!
    pause
    exit /b 1
)

echo 🔄 تنفيذ إصلاحات الترميز...
sqlcmd -S . -d Swagger_Endowment22 -i fix_arabic_encoding.sql -o encoding_fix_output.log

if %ERRORLEVEL% EQU 0 (
    echo.
    echo ================================================
    echo ✅ تم إصلاح مشاكل الترميز بنجاح!
    echo ✅ Arabic encoding issues fixed successfully!
    echo ================================================
    echo.
    echo الآن يجب أن تظهر البيانات كالتالي:
    echo ✅ مكتب الأوقاف - طرابلس
    echo ✅ مكتب الأوقاف - بنغازي  
    echo ✅ مكتب الأوقاف - مصراتة
    echo ✅ مكتب الأوقاف - الزاوية
    echo ✅ مكتب الأوقاف - شحات
    echo.
    echo 📋 التحديثات المطبقة:
    echo • تغيير Collation إلى Arabic_CI_AS
    echo • تحديث جميع الأعمدة النصية إلى NVARCHAR
    echo • إعادة إدراج البيانات بترميز N'' صحيح
    echo • فحص وإصلاح جميع الجداول
    echo.
    echo 💡 نصائح مهمة:
    echo • تأكد من استخدام Unicode (UTF-8) في المتصفح
    echo • في SSMS: Tools ^> Options ^> Query Results ^> SQL Server ^> Results to Grid ^> Include column headers
    echo • تأكد من ضبط Font في SSMS على Arial Unicode MS
) else (
    echo.
    echo ================================================
    echo ❌ فشل في إصلاح الترميز!
    echo ❌ Encoding fix failed!
    echo ================================================
    echo.
    echo الأسباب المحتملة:
    echo • عدم وجود صلاحيات كافية
    echo • عدم وجود قاعدة البيانات
    echo • مشكلة في الاتصال بـ SQL Server
    echo.
    echo الحلول:
    echo • تشغيل Command Prompt as Administrator
    echo • التأكد من تشغيل SQL Server
    echo • فحص ملف encoding_fix_output.log للتفاصيل
)

echo.
echo اضغط أي مفتاح للخروج...
pause > nul