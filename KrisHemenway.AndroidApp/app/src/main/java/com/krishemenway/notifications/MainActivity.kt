package com.krishemenway.notifications

import android.content.BroadcastReceiver
import android.content.Context
import android.content.IntentFilter
import android.support.v7.app.AppCompatActivity
import android.os.Bundle
import android.support.v4.content.LocalBroadcastManager
import android.widget.ListView
import com.google.firebase.messaging.FirebaseMessaging
import android.content.Intent
import android.widget.Toast

class MainActivity : AppCompatActivity() {
    override fun onCreate(savedInstanceState: Bundle?) {
        super.onCreate(savedInstanceState)

        setContentView(R.layout.activity_main)

        val messagesList = findViewById<ListView>(R.id.MessagesList)
        messagesList.adapter = notificationsAdapter

        refreshMessages(false)
        FirebaseMessaging.getInstance().subscribeToTopic("allDevices")
        broadcastManager().registerReceiver(notificationMessageReceiver, IntentFilter(NotificationReceivedEvent))
    }

    override fun onResume() {
        super.onResume()
        refreshMessages(false)
    }

    override fun onDestroy() {
        super.onDestroy()
        broadcastManager().unregisterReceiver(notificationMessageReceiver)
    }

    private fun broadcastManager() : LocalBroadcastManager {
        return LocalBroadcastManager.getInstance(this)
    }

    private val notificationMessageReceiver = object : BroadcastReceiver() {
        override fun onReceive(context: Context, intent: Intent) {
            refreshMessages(true)
        }
    }

    private fun refreshMessages(makeToast: Boolean) {
        if (makeToast) {
            Toast.makeText(this, R.string.notification_message_received_toast, Toast.LENGTH_LONG).show()
        }

        notificationsAdapter.refresh()
    }

    private var notificationsAdapter: NotificationsAdapter = NotificationsAdapter(this)
}
