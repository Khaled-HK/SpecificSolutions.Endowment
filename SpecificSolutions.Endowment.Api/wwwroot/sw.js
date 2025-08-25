// Service Worker للـ Push Notifications
const CACHE_NAME = 'endowment-push-v1';
const urlsToCache = [
    '/',
    '/css/app.css',
    '/js/app.js',
    '/favicon.ico'
];

// تثبيت Service Worker
self.addEventListener('install', function(event) {
    event.waitUntil(
        caches.open(CACHE_NAME)
            .then(function(cache) {
                console.log('Opened cache');
                return cache.addAll(urlsToCache);
            })
    );
});

// تفعيل Service Worker
self.addEventListener('activate', function(event) {
    event.waitUntil(
        caches.keys().then(function(cacheNames) {
            return Promise.all(
                cacheNames.map(function(cacheName) {
                    if (cacheName !== CACHE_NAME) {
                        console.log('Deleting old cache:', cacheName);
                        return caches.delete(cacheName);
                    }
                })
            );
        })
    );
});

// استقبال Push Notifications
self.addEventListener('push', function(event) {
    console.log('Push event received:', event);
    
    let data = {};
    if (event.data) {
        try {
            data = event.data.json();
        } catch (e) {
            data = {
                title: 'نظام الأوقاف',
                body: event.data.text()
            };
        }
    }

    const options = {
        body: data.body || 'رسالة جديدة من نظام الأوقاف',
        icon: data.icon || '/favicon.ico',
        badge: data.badge || '/badge.png',
        data: data.data || {},
        actions: data.actions || [
            {
                action: 'verify',
                title: 'تحقق الآن'
            },
            {
                action: 'dismiss',
                title: 'تجاهل'
            }
        ],
        requireInteraction: true,
        silent: false,
        tag: 'endowment-verification',
        renotify: true
    };

    event.waitUntil(
        self.registration.showNotification(data.title || '🔐 رمز التحقق - نظام الأوقاف', options)
    );
});

// التعامل مع النقر على الإشعارات
self.addEventListener('notificationclick', function(event) {
    console.log('Notification clicked:', event);
    
    event.notification.close();

    if (event.action === 'verify') {
        // فتح صفحة التحقق
        event.waitUntil(
            clients.openWindow('/verify-code')
        );
    } else if (event.action === 'dismiss') {
        // تجاهل الإشعار
        console.log('Notification dismissed');
    } else {
        // النقر على الإشعار نفسه
        event.waitUntil(
            clients.openWindow('/')
        );
    }
});

// التعامل مع إغلاق الإشعارات
self.addEventListener('notificationclose', function(event) {
    console.log('Notification closed:', event);
});

// استقبال الرسائل من الصفحة الرئيسية
self.addEventListener('message', function(event) {
    console.log('Message received in service worker:', event.data);
    
    if (event.data && event.data.type === 'SKIP_WAITING') {
        self.skipWaiting();
    }
});

// التعامل مع الطلبات
self.addEventListener('fetch', function(event) {
    event.respondWith(
        caches.match(event.request)
            .then(function(response) {
                // إرجاع الملف من الكاش إذا وجد
                if (response) {
                    return response;
                }
                
                // إرجاع الطلب من الشبكة
                return fetch(event.request);
            }
        )
    );
});
