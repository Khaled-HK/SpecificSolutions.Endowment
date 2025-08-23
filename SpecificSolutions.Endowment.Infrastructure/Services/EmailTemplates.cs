namespace SpecificSolutions.Endowment.Infrastructure.Services
{
    /// <summary>
    /// قوالب البريد الإلكتروني
    /// </summary>
    public static class EmailTemplates
    {
        /// <summary>
        /// قالب تأكيد البريد الإلكتروني
        /// </summary>
        public static string GetEmailConfirmationTemplate(string userName, string confirmationLink)
        {
            return $@"
                <div style='direction: rtl; font-family: Arial, sans-serif; max-width: 600px; margin: 0 auto; padding: 20px; background-color: #f8f9fa;'>
                    <div style='background-color: white; padding: 30px; border-radius: 10px; box-shadow: 0 2px 10px rgba(0,0,0,0.1);'>
                        <div style='text-align: center; margin-bottom: 30px;'>
                            <h1 style='color: #2c3e50; margin: 0;'>نظام الأوقاف</h1>
                            <p style='color: #7f8c8d; margin: 10px 0 0 0;'>تأكيد البريد الإلكتروني</p>
                        </div>
                        
                        <div style='margin-bottom: 30px;'>
                            <h2 style='color: #2c3e50; margin-bottom: 20px;'>مرحباً {userName}</h2>
                            <p style='color: #34495e; line-height: 1.6; margin-bottom: 20px;'>
                                شكراً لك على التسجيل في نظام الأوقاف. يرجى النقر على الزر أدناه لتأكيد بريدك الإلكتروني:
                            </p>
                        </div>
                        
                        <div style='text-align: center; margin-bottom: 30px;'>
                            <a href='{confirmationLink}' 
                               style='background-color: #3498db; color: white; padding: 15px 30px; text-decoration: none; border-radius: 5px; display: inline-block; font-weight: bold;'>
                                تأكيد البريد الإلكتروني
                            </a>
                        </div>
                        
                        <div style='background-color: #ecf0f1; padding: 20px; border-radius: 5px; margin-bottom: 20px;'>
                            <p style='color: #7f8c8d; margin: 0; font-size: 14px;'>
                                <strong>ملاحظة:</strong> إذا لم يعمل الزر أعلاه، يمكنك نسخ الرابط التالي ولصقه في المتصفح:
                            </p>
                            <p style='color: #3498db; margin: 10px 0 0 0; font-size: 12px; word-break: break-all;'>
                                {confirmationLink}
                            </p>
                        </div>
                        
                        <div style='border-top: 1px solid #ecf0f1; padding-top: 20px;'>
                            <p style='color: #95a5a6; font-size: 12px; margin: 0;'>
                                هذا الرابط صالح لمدة 24 ساعة فقط. إذا لم تقم بتأكيد بريدك الإلكتروني خلال هذه المدة، 
                                ستحتاج إلى طلب رابط تأكيد جديد.
                            </p>
                        </div>
                    </div>
                </div>";
        }

        /// <summary>
        /// قالب إعادة تعيين كلمة المرور
        /// </summary>
        public static string GetPasswordResetTemplate(string userName, string resetLink)
        {
            return $@"
                <div style='direction: rtl; font-family: Arial, sans-serif; max-width: 600px; margin: 0 auto; padding: 20px; background-color: #f8f9fa;'>
                    <div style='background-color: white; padding: 30px; border-radius: 10px; box-shadow: 0 2px 10px rgba(0,0,0,0.1);'>
                        <div style='text-align: center; margin-bottom: 30px;'>
                            <h1 style='color: #2c3e50; margin: 0;'>نظام الأوقاف</h1>
                            <p style='color: #7f8c8d; margin: 10px 0 0 0;'>إعادة تعيين كلمة المرور</p>
                        </div>
                        
                        <div style='margin-bottom: 30px;'>
                            <h2 style='color: #2c3e50; margin-bottom: 20px;'>مرحباً {userName}</h2>
                            <p style='color: #34495e; line-height: 1.6; margin-bottom: 20px;'>
                                لقد تلقينا طلباً لإعادة تعيين كلمة المرور الخاصة بك. يرجى النقر على الزر أدناه لإعادة تعيين كلمة المرور:
                            </p>
                        </div>
                        
                        <div style='text-align: center; margin-bottom: 30px;'>
                            <a href='{resetLink}' 
                               style='background-color: #e74c3c; color: white; padding: 15px 30px; text-decoration: none; border-radius: 5px; display: inline-block; font-weight: bold;'>
                                إعادة تعيين كلمة المرور
                            </a>
                        </div>
                        
                        <div style='background-color: #ecf0f1; padding: 20px; border-radius: 5px; margin-bottom: 20px;'>
                            <p style='color: #7f8c8d; margin: 0; font-size: 14px;'>
                                <strong>ملاحظة:</strong> إذا لم يعمل الزر أعلاه، يمكنك نسخ الرابط التالي ولصقه في المتصفح:
                            </p>
                            <p style='color: #e74c3c; margin: 10px 0 0 0; font-size: 12px; word-break: break-all;'>
                                {resetLink}
                            </p>
                        </div>
                        
                        <div style='border-top: 1px solid #ecf0f1; padding-top: 20px;'>
                            <p style='color: #95a5a6; font-size: 12px; margin: 0;'>
                                هذا الرابط صالح لمدة ساعة واحدة فقط. إذا لم تقم بإعادة تعيين كلمة المرور خلال هذه المدة، 
                                ستحتاج إلى طلب رابط جديد.
                            </p>
                            <p style='color: #95a5a6; font-size: 12px; margin: 10px 0 0 0;'>
                                إذا لم تطلب إعادة تعيين كلمة المرور، يمكنك تجاهل هذا البريد الإلكتروني بأمان.
                            </p>
                        </div>
                    </div>
                </div>";
        }

        /// <summary>
        /// قالب ترحيب عام
        /// </summary>
        public static string GetWelcomeTemplate(string userName, string message)
        {
            return $@"
                <div style='direction: rtl; font-family: Arial, sans-serif; max-width: 600px; margin: 0 auto; padding: 20px; background-color: #f8f9fa;'>
                    <div style='background-color: white; padding: 30px; border-radius: 10px; box-shadow: 0 2px 10px rgba(0,0,0,0.1);'>
                        <div style='text-align: center; margin-bottom: 30px;'>
                            <h1 style='color: #2c3e50; margin: 0;'>نظام الأوقاف</h1>
                            <p style='color: #7f8c8d; margin: 10px 0 0 0;'>مرحباً بك</p>
                        </div>
                        
                        <div style='margin-bottom: 30px;'>
                            <h2 style='color: #2c3e50; margin-bottom: 20px;'>مرحباً {userName}</h2>
                            <p style='color: #34495e; line-height: 1.6; margin-bottom: 20px;'>
                                {message}
                            </p>
                        </div>
                        
                        <div style='border-top: 1px solid #ecf0f1; padding-top: 20px;'>
                            <p style='color: #95a5a6; font-size: 12px; margin: 0;'>
                                شكراً لك على استخدام نظام الأوقاف
                            </p>
                        </div>
                    </div>
                </div>";
        }
    }
}
