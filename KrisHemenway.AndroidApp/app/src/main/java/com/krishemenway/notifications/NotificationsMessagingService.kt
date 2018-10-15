package com.krishemenway.notifications

import android.content.Intent
import android.support.v4.content.LocalBroadcastManager
import com.google.firebase.messaging.FirebaseMessagingService
import com.google.firebase.messaging.RemoteMessage

class NotificationsMessagingService : FirebaseMessagingService() {
    override fun onMessageReceived(message: RemoteMessage?) {
        super.onMessageReceived(message)
        LocalBroadcastManager.getInstance(this).sendBroadcast(Intent(NotificationReceivedEvent))
    }
}

const val NotificationReceivedEvent: String = "com.krishemenway.refresh"