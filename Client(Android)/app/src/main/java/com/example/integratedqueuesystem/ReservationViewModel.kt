package com.example.integratedqueuesystem

import androidx.lifecycle.ViewModel

class ReservationViewModel: ViewModel() {

    /*
    //Global
    var location: LatLng? = null

    //HomePage
    var viewPagerPosition = 0
    val agencyStatus = MutableLiveData<HashMap<String, Agency>>(null)
    */

    //Map and Reservation
    var agencyId = ""
    //var agencyName = ""

    //Reservation
    var serviceType = 0
    //var serviceTypeList = emptyList<ServiceType>()
    var reservationData = emptyList<String>()
    var order: String? = null
}