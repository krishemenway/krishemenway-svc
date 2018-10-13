package com.krishemenway.notifications

import android.util.Log
import com.android.volley.*
import com.android.volley.toolbox.HttpHeaderParser
import com.google.gson.Gson
import com.google.gson.JsonSyntaxException

import java.io.UnsupportedEncodingException

/**
 * Make a GET request and return a parsed object from JSON.
 *
 * @param url URL of the request to make
 * @param headers Map of request headers
 * @param clazz Relevant class object, for Gson's reflection
 */
class GsonRequest<T> ( url: String, private val clazz: Class<T>, private val headers: Map<String, String>?, private val listener: Response.Listener<T>, errorListener: Response.ErrorListener )
    : Request<T>(Method.GET, url, errorListener) {
    private val gson = Gson()

    @Throws(AuthFailureError::class)
    override fun getHeaders(): Map<String, String> {
        return headers ?: super.getHeaders()
    }

    override fun deliverResponse(response: T) {
        listener.onResponse(response)
    }

    override fun parseNetworkResponse(response: NetworkResponse): Response<T> {
        Log.d("GsonRequest", "Handling response from $this.url")
        return try {
            Response.success(
                gson.fromJson(String(response.data), clazz),
                HttpHeaderParser.parseCacheHeaders(response)
            )
        } catch (e: UnsupportedEncodingException) {
            Response.error(ParseError(e))
        } catch (e: JsonSyntaxException) {
            Response.error(ParseError(e))
        }
    }
}