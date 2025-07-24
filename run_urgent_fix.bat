@echo off
chcp 65001 >nul
echo ================================================
echo 🚨 إصلاح عاجل للترميز العربي في التطبيق
echo 🚨 Urgent Arabic Encoding Fix for Application
echo ================================================
echo.
echo 📋 المشكلة الحالية:
echo ❌ ط…ظƒطھط¨ ط§ظ„ط£ظˆظ‚ط§ظپ - ط·ط±ط§ط¨ظ„ط³
echo.
echo 📋 النتيجة المتوقعة:
echo ✅ مكتب الأوقاف - طرابلس
echo.
echo 🔧 بدء الإصلاح...
echo.

if not exist "urgent_arabic_fix_complete.sql" (
    echo ❌ خطأ: ملف urgent_arabic_fix_complete.sql غير موجود!
    echo Please make sure the SQL file exists in the same directory.
    pause
    exit /b 1
)

echo 🚀 تشغيل إصلاح قاعدة البيانات...
sqlcmd -S . -d Swagger_Endowment22 -i urgent_arabic_fix_complete.sql -o urgent_fix_output.log

if %ERRORLEVEL% EQU 0 (
    echo.
    echo ✅ تم إصلاح قاعدة البيانات بنجاح!
    echo ✅ Database fix completed successfully!
    echo.
    echo 📋 الخطوات التالية:
    echo 1. أعد تشغيل التطبيق (Application)
    echo 2. أعد تشغيل الخادم (IIS/Server)
    echo 3. امسح cache المتصفح
    echo 4. تحقق من النتائج
    echo.
    echo 🎯 النتيجة المتوقعة الآن:
    echo ✅ مكتب الأوقاف - طرابلس
    echo ✅ مكتب الأوقاف - بنغازي
    echo ✅ مكتب الأوقاف - مصراتة
    echo.
) else (
    echo.
    echo ❌ فشل في إصلاح قاعدة البيانات!
    echo ❌ Database fix failed!
    echo.
    echo 🔍 تحقق من ملف السجل: urgent_fix_output.log
    echo 🔍 Check log file: urgent_fix_output.log
    echo.
    echo 💡 حلول بديلة:
    echo 1. تشغيل SQL Server Management Studio كـ Administrator
    echo 2. فتح urgent_arabic_fix_complete.sql وتشغيله يدوياً
    echo 3. التأكد من أن قاعدة البيانات Swagger_Endowment22 متاحة
)

echo.
echo ================================================
echo للحصول على مساعدة إضافية، راجع:
echo urgent_fix_output.log
echo ================================================
pause >nul