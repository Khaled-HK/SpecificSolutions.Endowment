#!/bin/bash

echo "================================================"
echo "        تشغيل البيانات الليبية لنظام الأوقاف"
echo "        Libyan Endowment System Data Import"
echo "================================================"
echo

# التحقق من وجود ملف SQL المطلوب
if [ ! -f "run_database_operations.sql" ]; then
    echo "خطأ: ملف run_database_operations.sql غير موجود!"
    echo "Error: run_database_operations.sql file not found!"
    exit 1
fi

echo "جاري الاتصال بقاعدة البيانات..."
echo "Connecting to database..."
echo

# تنفيذ عمليات قاعدة البيانات
# يمكنك تعديل server والمعاملات حسب إعداداتك
sqlcmd -S localhost -d Swagger_Endowment22 -i run_database_operations.sql -o output.log

# التحقق من نتيجة التنفيذ
if [ $? -eq 0 ]; then
    echo
    echo "================================================"
    echo "✅ تم بنجاح! تم إدراج البيانات الليبية"
    echo "✅ SUCCESS! Libyan data inserted successfully"
    echo "================================================"
    echo
    echo "يمكنك مراجعة التفاصيل في ملف output.log"
    echo "You can check details in output.log file"
    echo
    echo "البيانات المدرجة:"
    echo "- 10 بنوك ليبية"
    echo "- 5 مدن رئيسية"
    echo "- 5 مناطق"
    echo "- 5 منتجات"
    echo "- 5 مكاتب أوقاف"
    echo "- 5 مرافق"
    echo "- 5 مباني/مساجد"
    echo "- 5 حسابات مستفيدين"
    echo "- 5 قرارات"
    echo
    echo "جميع المبالغ بالدينار الليبي 💰"
    echo "All amounts in Libyan Dinar 💰"
else
    echo
    echo "================================================"
    echo "❌ فشل في التنفيذ! راجع ملف output.log"
    echo "❌ FAILED! Check output.log file"
    echo "================================================"
    echo
    echo "الأسباب المحتملة:"
    echo "- عدم وجود SQL Server"
    echo "- عدم وجود قاعدة البيانات Swagger_Endowment22"
    echo "- مشاكل في الصلاحيات"
    echo "- خطأ في بنية الجداول"
    echo
    echo "للاستخدام مع MySQL:"
    echo "mysql -u username -p Swagger_Endowment22 < run_database_operations.sql"
    echo
    echo "للاستخدام مع PostgreSQL:"
    echo "psql -d Swagger_Endowment22 -f run_database_operations.sql"
fi

echo
echo "اضغط Enter للخروج..."
read