package com.krishemenway.notifications

import android.support.v7.app.AppCompatActivity
import android.os.Bundle
import android.widget.ListView
import com.google.firebase.messaging.FirebaseMessaging

class MainActivity : AppCompatActivity() {
    override fun onCreate(savedInstanceState: Bundle?) {
        super.onCreate(savedInstanceState)
        notificationsAdapter = NotificationsAdapter(this)

        setContentView(R.layout.activity_main)

        val messagesList = findViewById<ListView>(R.id.MessagesList)
        messagesList.adapter = notificationsAdapter

        notificationsAdapter!!.refresh()
        FirebaseMessaging.getInstance().subscribeToTopic("allDevices")
    }

    override fun onResume() {
        super.onResume()
        notificationsAdapter!!.refresh()
    }

    private var notificationsAdapter: NotificationsAdapter? = null
}
