package com.krishemenway.notifications

import com.google.gson.annotations.SerializedName
import java.util.*

data class Notification(
    @SerializedName("NotificationId") val notificationId: String,
    @SerializedName("Title") val title: String,
    @SerializedName("Content") val content: String,
    @SerializedName("TypeName") val typeName: String,
    @SerializedName("SentTime") val sentTime: Date
)