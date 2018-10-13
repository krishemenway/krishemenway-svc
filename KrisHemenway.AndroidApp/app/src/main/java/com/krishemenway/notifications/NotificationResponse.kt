package com.krishemenway.notifications

import com.google.gson.annotations.SerializedName

data class NotificationResponse(
    @SerializedName("Notifications") val notifications: Array<Notification>
)
