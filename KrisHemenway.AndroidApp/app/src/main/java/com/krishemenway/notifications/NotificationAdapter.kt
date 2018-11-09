package com.krishemenway.notifications

import android.app.AlertDialog
import android.content.Context
import android.content.SharedPreferences
import android.preference.PreferenceManager
import android.util.Log
import android.view.LayoutInflater
import android.view.View
import android.view.ViewGroup
import android.widget.BaseAdapter
import android.widget.TextView
import com.android.volley.Response
import com.android.volley.toolbox.Volley
import com.google.gson.Gson

import java.text.SimpleDateFormat
import java.time.LocalDate
import java.time.format.DateTimeFormatter

class NotificationsAdapter(private val _context: Context, private val notifications: MutableList<Notification> = mutableListOf()) : BaseAdapter() {
    fun refresh() {
        this.setNotificationsFromResponse(Gson().fromJson(PreferenceManager.getDefaultSharedPreferences(this._context).getString("LastNotificationsResponse", "{\"Notifications\": []}"), NotificationResponse::class.java))
        makeRequest()
    }

    override fun getCount(): Int {
        return notifications.size
    }

    override fun getItem(position: Int): Notification {
        return notifications[position]
    }

    override fun getItemId(position: Int): Long {
        return 0
    }

    override fun getView(position: Int, convertView: View?, parent: ViewGroup): View {
        val view = convertView ?: LayoutInflater.from(this._context).inflate(R.layout.notification_item, parent, false)
        val item = getItem(position)

        val titleView = view.findViewById<TextView>(R.id.title)
        titleView.text = item.title

        val messageView = view.findViewById<TextView>(R.id.content)
        messageView.text = item.content

        val timestampView = view.findViewById<TextView>(R.id.timestamp)
        timestampView.text = NotificationDisplayFormat.format(item.sentTime)

        return view
    }

    private fun makeRequest() {
        val notificationsSinceDate = LocalDate.now().minusDays(24).format(NotificationRequestFormat)
        val notificationRequestUrl = "https://www.krishemenway.com/api/notifications/recent?fromTime=$notificationsSinceDate"

        Log.d("NotificationAdapter", "Requesting Notifications @ $notificationRequestUrl")
        val request = GsonRequest(notificationRequestUrl, NotificationResponse::class.java, HashMap(),
            Response.Listener { response ->
                PreferenceManager.getDefaultSharedPreferences(this._context).edit().putString("LastNotificationsResponse", Gson().toJson(response)).apply()
                this.setNotificationsFromResponse(response)
            },
            Response.ErrorListener { error ->
                AlertDialog.Builder(this._context)
                    .setTitle(R.string.notification_request_error_title)
                    .setMessage("Error: $error")
                    .setPositiveButton(R.string.notification_request_error_ok) { dialog, _ -> dialog.dismiss() }
                    .create().show()
            }
        )

        Volley.newRequestQueue(_context).add(request)
    }

    private fun setNotificationsFromResponse(response: NotificationResponse) {
        notifications.clear()
        notifications.addAll(response.notifications)
        this.notifyDataSetChanged()
    }

    companion object {
        private val NotificationRequestFormat = DateTimeFormatter.ofPattern("yyyy-MM-dd")
        private val NotificationDisplayFormat = SimpleDateFormat("yyyy | MM | dd")
    }
}
