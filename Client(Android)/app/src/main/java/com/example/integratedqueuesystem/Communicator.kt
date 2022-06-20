package com.example.integratedqueuesystem

import androidx.lifecycle.LiveData
import com.example.integratedqueuesystem.data.agency.Agency
import com.example.integratedqueuesystem.data.reservation.NotificationSettings
import com.example.integratedqueuesystem.data.reservation.ReservationData
import com.example.integratedqueuesystem.data.reservation.ReservationRecord
import com.google.android.gms.maps.model.LatLng

interface Communicator {

    fun isOnline(): Boolean
    fun getUpdateTimer(): LiveData<Int>
    fun getLocation(): LatLng?

    fun getAgencyStatus(): LiveData<HashMap<String, Agency>?>
    fun updateAgencyStatus()

    fun setFavorite(agencyId: String, favorite: Boolean)

    fun getDestinationId(): String
    fun setDestinationId(id: String)

    fun getViewPagerPosition(): Int
    fun setViewPagerPosition(position: Int)

    fun getReservationPosition(): Int
    fun setReservationPosition(position: Int)

    fun setReservationId(agencyId: String): Boolean

    fun sendReservation(agencyId: String, data: ReservationData)

    fun getNotificationSettings(): NotificationSettings
    fun setReminder(agencyId: String, serviceType: Int, serviceTypeName: String, response: String)
}