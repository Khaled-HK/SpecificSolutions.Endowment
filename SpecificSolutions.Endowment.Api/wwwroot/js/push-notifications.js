// Push Notifications Service
class PushNotificationService {
    constructor() {
        this.vapidPublicKey = 'YOUR_VAPID_PUBLIC_KEY'; // سيتم تحديثه من الباك إند
        this.apiBaseUrl = '/api/verificationcode';
        this.subscription = null;
    }

    // تهيئة Push Notifications
    async initialize() {
        try {
            // التحقق من دعم Push Notifications
            if (!('serviceWorker' in navigator) || !('PushManager' in window)) {
                console.warn('Push notifications are not supported');
                return false;
            }

            // تسجيل Service Worker
            const registration = await navigator.serviceWorker.register('/sw.js');
            console.log('Service Worker registered:', registration);

            // التحقق من الإذن
            const permission = await this.requestPermission();
            if (permission !== 'granted') {
                console.warn('Push notification permission denied');
                return false;
            }

            // الاشتراك في Push Notifications
            await this.subscribe();
            return true;
        } catch (error) {
            console.error('Error initializing push notifications:', error);
            return false;
        }
    }

    // طلب إذن الإشعارات
    async requestPermission() {
        try {
            const permission = await Notification.requestPermission();
            return permission;
        } catch (error) {
            console.error('Error requesting notification permission:', error);
            return 'denied';
        }
    }

    // الاشتراك في Push Notifications
    async subscribe() {
        try {
            const registration = await navigator.serviceWorker.ready;
            
            // التحقق من الاشتراك الحالي
            let subscription = await registration.pushManager.getSubscription();
            
            if (!subscription) {
                // إنشاء اشتراك جديد
                subscription = await registration.pushManager.subscribe({
                    userVisibleOnly: true,
                    applicationServerKey: this.urlBase64ToUint8Array(this.vapidPublicKey)
                });
            }

            this.subscription = subscription;
            console.log('Push subscription:', subscription);

            // إرسال الاشتراك للباك إند
            await this.sendSubscriptionToServer(subscription);
            
            return subscription;
        } catch (error) {
            console.error('Error subscribing to push notifications:', error);
            throw error;
        }
    }

    // إلغاء الاشتراك
    async unsubscribe() {
        try {
            const registration = await navigator.serviceWorker.ready;
            const subscription = await registration.pushManager.getSubscription();
            
            if (subscription) {
                await subscription.unsubscribe();
                this.subscription = null;
                console.log('Unsubscribed from push notifications');
            }
        } catch (error) {
            console.error('Error unsubscribing from push notifications:', error);
        }
    }

    // إرسال الاشتراك للباك إند
    async sendSubscriptionToServer(subscription) {
        try {
            const response = await fetch(`${this.apiBaseUrl}/subscribe`, {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json',
                },
                body: JSON.stringify({
                    subscription: JSON.stringify(subscription),
                    userId: this.getCurrentUserId()
                })
            });

            if (response.ok) {
                console.log('Subscription sent to server successfully');
            } else {
                console.error('Failed to send subscription to server');
            }
        } catch (error) {
            console.error('Error sending subscription to server:', error);
        }
    }

    // إرسال رمز تحقق عبر Push Notification
    async sendVerificationCode(purpose) {
        try {
            if (!this.subscription) {
                throw new Error('No push subscription available');
            }

            const response = await fetch(`${this.apiBaseUrl}/send-push`, {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json',
                },
                body: JSON.stringify({
                    subscription: JSON.stringify(this.subscription),
                    purpose: purpose
                })
            });

            const result = await response.json();
            
            if (result.isSuccess) {
                console.log('Verification code sent via push notification');
                return result;
            } else {
                throw new Error(result.message || 'Failed to send verification code');
            }
        } catch (error) {
            console.error('Error sending verification code:', error);
            throw error;
        }
    }

    // التحقق من رمز التحقق
    async verifyCode(code, purpose) {
        try {
            if (!this.subscription) {
                throw new Error('No push subscription available');
            }

            const response = await fetch(`${this.apiBaseUrl}/verify-push`, {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json',
                },
                body: JSON.stringify({
                    subscription: JSON.stringify(this.subscription),
                    code: code,
                    purpose: purpose
                })
            });

            const result = await response.json();
            
            if (result.isSuccess) {
                console.log('Verification code verified successfully');
                return result;
            } else {
                throw new Error(result.message || 'Failed to verify code');
            }
        } catch (error) {
            console.error('Error verifying code:', error);
            throw error;
        }
    }

    // تحويل VAPID Public Key
    urlBase64ToUint8Array(base64String) {
        const padding = '='.repeat((4 - base64String.length % 4) % 4);
        const base64 = (base64String + padding)
            .replace(/-/g, '+')
            .replace(/_/g, '/');

        const rawData = window.atob(base64);
        const outputArray = new Uint8Array(rawData.length);

        for (let i = 0; i < rawData.length; ++i) {
            outputArray[i] = rawData.charCodeAt(i);
        }
        return outputArray;
    }

    // الحصول على معرف المستخدم الحالي
    getCurrentUserId() {
        // يمكن تعديل هذه الدالة حسب نظام المصادقة المستخدم
        return localStorage.getItem('userId') || 'anonymous';
    }

    // التحقق من حالة الإشعارات
    async checkNotificationStatus() {
        try {
            if (!('Notification' in window)) {
                return 'not-supported';
            }

            if (Notification.permission === 'default') {
                return 'not-requested';
            }

            if (Notification.permission === 'denied') {
                return 'denied';
            }

            if (Notification.permission === 'granted') {
                const registration = await navigator.serviceWorker.ready;
                const subscription = await registration.pushManager.getSubscription();
                
                if (subscription) {
                    return 'subscribed';
                } else {
                    return 'not-subscribed';
                }
            }
        } catch (error) {
            console.error('Error checking notification status:', error);
            return 'error';
        }
    }

    // إظهار رسالة للمستخدم
    showMessage(message, type = 'info') {
        // يمكن تعديل هذه الدالة حسب واجهة المستخدم المستخدمة
        const alertDiv = document.createElement('div');
        alertDiv.className = `alert alert-${type}`;
        alertDiv.textContent = message;
        
        document.body.appendChild(alertDiv);
        
        setTimeout(() => {
            alertDiv.remove();
        }, 5000);
    }
}

// تصدير الخدمة للاستخدام
window.PushNotificationService = PushNotificationService;

// تهيئة الخدمة عند تحميل الصفحة
document.addEventListener('DOMContentLoaded', async function() {
    const pushService = new PushNotificationService();
    
    // التحقق من حالة الإشعارات
    const status = await pushService.checkNotificationStatus();
    console.log('Push notification status:', status);
    
    // إضافة زر تفعيل الإشعارات إذا لم تكن مفعلة
    if (status === 'not-requested' || status === 'not-subscribed') {
        addNotificationButton(pushService);
    }
});

// إضافة زر تفعيل الإشعارات
function addNotificationButton(pushService) {
    const button = document.createElement('button');
    button.className = 'btn btn-primary';
    button.innerHTML = '🔔 تفعيل الإشعارات الفورية';
    button.onclick = async () => {
        try {
            const success = await pushService.initialize();
            if (success) {
                pushService.showMessage('تم تفعيل الإشعارات الفورية بنجاح!', 'success');
                button.remove();
            } else {
                pushService.showMessage('فشل في تفعيل الإشعارات الفورية', 'error');
            }
        } catch (error) {
            pushService.showMessage('حدث خطأ أثناء تفعيل الإشعارات', 'error');
        }
    };
    
    // إضافة الزر للصفحة
    const container = document.querySelector('.notification-container') || document.body;
    container.appendChild(button);
}
