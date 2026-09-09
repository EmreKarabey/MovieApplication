importScripts('https://www.gstatic.com/firebasejs/10.7.1/firebase-app-compat.js');
importScripts('https://www.gstatic.com/firebasejs/10.7.1/firebase-messaging-compat.js');


const firebaseConfig = {
    apiKey: "AIzaSyBUDQXXFq9l2Str8u-KD29dJ5pEN_QO_uE",
    projectId: "movieapp-f2a76",
    messagingSenderId: "273423657332",
    appId: "1:273423657332:web:65fae0e00f35169941a421"
};

firebase.initializeApp(firebaseConfig);
const messaging = firebase.messaging();


messaging.onBackgroundMessage(function (payload) {
    const notificationTitle = payload.notification.title;
    const notificationOptions = {
        body: payload.notification.body,
        icon: '/images/bell-icon.png'
    };

    self.registration.showNotification(notificationTitle, notificationOptions);


});
