package com.example.integratedqueuesystem

import android.Manifest
import android.app.NotificationChannel
import android.app.NotificationManager
import android.content.Context
import android.content.pm.PackageManager
import android.location.LocationManager
import android.net.ConnectivityManager
import android.net.NetworkCapabilities
import android.os.Build
import androidx.appcompat.app.AppCompatActivity
import android.os.Bundle
import android.os.Handler
import android.os.Looper
import android.provider.Settings
import android.util.Log
import androidx.activity.result.contract.ActivityResultContracts
import androidx.core.app.ActivityCompat
import androidx.core.app.NotificationCompat
import androidx.core.app.NotificationManagerCompat
import androidx.core.view.get
import androidx.lifecycle.LiveData
import androidx.lifecycle.MutableLiveData
import androidx.lifecycle.ViewModelProvider
import androidx.navigation.findNavController
import androidx.navigation.ui.onNavDestinationSelected
import androidx.navigation.ui.setupWithNavController
import com.example.integratedqueuesystem.data.agency.Agency
import com.example.integratedqueuesystem.data.agency.AgencyData
import com.example.integratedqueuesystem.data.agency.AgencyViewModel
import com.example.integratedqueuesystem.data.agency.FavoriteAgency
import com.example.integratedqueuesystem.data.reservation.*
import com.example.integratedqueuesystem.databinding.ActivityMainBinding
import com.google.android.gms.maps.model.LatLng
import java.util.*
import kotlin.collections.HashMap
import kotlin.concurrent.schedule
import com.google.android.gms.location.*
import com.google.android.material.snackbar.Snackbar
import com.google.firebase.database.*


class MainActivity : AppCompatActivity(), Communicator {

    private lateinit var binding: ActivityMainBinding

    private lateinit var agencyViewModel: AgencyViewModel
    private lateinit var firebase: DatabaseReference
    private lateinit var fusedLocationClient: FusedLocationProviderClient
    private val root = FirebaseDatabase.getInstance().reference.root

    private val agencyStatus = MutableLiveData<HashMap<String, Agency>?>(null)
    private var location: LatLng? = null
    private var destinationId = ""
    private val updateTimer = MutableLiveData(0)

    private var viewPagerPosition = 0
    private var reservationPosition = 1
    private lateinit var reservationViewModel: ReservationViewModel
    //private var reservationId = ""
    //private var reservationDataServiceTypeList = emptyList<ServiceType>()
    //private var reservationData = emptyList<String>()

    private val notificationSettings = NotificationSettings()
    private var reminderAgencyId: String? = null
    private var reminderServiceType: Int? = null

    override fun onCreate(savedInstanceState: Bundle?) {
        super.onCreate(savedInstanceState)
        binding = ActivityMainBinding.inflate(layoutInflater)
        val view = binding.root
        setContentView(view)

        Handler(Looper.getMainLooper()).postDelayed({
            val navController = binding.fragmentContainerViewMain.findNavController()
            binding.bottomNavigationView.setupWithNavController(navController)
        }, 0)

        agencyViewModel = ViewModelProvider(this)[AgencyViewModel::class.java]
        reservationViewModel = ViewModelProvider(this)[ReservationViewModel::class.java]

        firebase = root.database.reference
        firebase.get().addOnSuccessListener { dataSnapshot ->
            val type = object: GenericTypeIndicator<HashMap<String, Agency>>(){}
            val data = dataSnapshot.getValue(type)
            if (data != null) {
                Log.e("agencyData", "update")
                agencyViewModel.updateAgencies(data.values.toList().map { AgencyData(it) })
                agencyStatus.value = data
            } else {
                Log.e("agencyData", "null")
                agencyStatus.value = null
            }
        }.addOnFailureListener {
            Log.e("agencyData", "failed")
            agencyStatus.value = null
        }

        fusedLocationClient = LocationServices.getFusedLocationProviderClient(this)
        requestLocationPermissions()

        createNotificationChannel()

        Timer().schedule(1000, 30000) {
            updateAgencyStatus()
        }

        Timer().schedule(1000, 1000) {
            runOnUiThread {
                updateTimer.value = updateTimer.value?.inc()
            }
        }

        if (!isOnline()) {
            Snackbar.make(
                view,
                getString(R.string.no_internet),
                Snackbar.LENGTH_LONG
            ).apply {
                setAction(getString(R.string.confirm)) {
                    dismiss()
                }
                show()
            }
        }
    }

    private fun createNotificationChannel() {
        // Create the NotificationChannel, but only on API 26+ because
        // the NotificationChannel class is new and not in the support library
        if (Build.VERSION.SDK_INT >= Build.VERSION_CODES.O) {
            val name = getString(R.string.channel_name)
            val descriptionText = getString(R.string.channel_description)
            val importance = NotificationManager.IMPORTANCE_DEFAULT
            val channel = NotificationChannel("reminder", name, importance).apply {
                description = descriptionText
            }
            // Register the channel with the system
            val notificationManager: NotificationManager =
                getSystemService(Context.NOTIFICATION_SERVICE) as NotificationManager
            notificationManager.createNotificationChannel(channel)
        }
    }

    private fun requestLocationPermissions() {
        val locationPermissionRequest = registerForActivityResult(
            ActivityResultContracts.RequestMultiplePermissions()
        ) { permissions ->
            //if (Build.VERSION.SDK_INT >= Build.VERSION_CODES.N) {
                when {
                    permissions.getOrDefault(
                        Manifest.permission.ACCESS_FINE_LOCATION,
                        false
                    ) -> {
                        /*
                        // Precise location access granted.
                        fusedLocationClient.locationAvailability.addOnSuccessListener { locationAvailability ->
                            if (locationAvailability.isLocationAvailable) {
                                fusedLocationClient.lastLocation.addOnSuccessListener { location2 ->
                                    this.location = LatLng(location2.latitude, location2.longitude)
                                    Log.e("position", location.toString())
                                }.addOnFailureListener {
                                    Log.e("position get", it.toString())
                                }
                            }
                        }*/
                    }
                    permissions.getOrDefault(
                        Manifest.permission.ACCESS_COARSE_LOCATION,
                        false
                    ) -> {
                        /*
                        fusedLocationClient.locationAvailability.addOnSuccessListener { locationAvailability ->
                            if (locationAvailability.isLocationAvailable) {
                                // Only approximate location access granted.
                                fusedLocationClient.lastLocation.addOnSuccessListener { location4 ->
                                    this.location = LatLng(location4.latitude, location4.longitude)
                                    Log.e("position", location.toString())
                                }.addOnFailureListener {
                                    Log.e("position get", it.toString())
                                }
                            }
                        }*/
                    }
                    else -> {
                        // No location access granted.
                        Log.e("position get", "permission denied")
                        location = null
                    }
                }
            //}
        }
        // Before you perform the actual permission request, check whether your app
        // already has the permissions, and whether your app needs to show a permission
        // rationale dialog. For more details, see Request permissions.
        locationPermissionRequest.launch(arrayOf(
            Manifest.permission.ACCESS_FINE_LOCATION,
            Manifest.permission.ACCESS_COARSE_LOCATION))
    }

    private fun isLocationEnabled(): Boolean {
        return if (Build.VERSION.SDK_INT >= Build.VERSION_CODES.P) {
            val lm = getSystemService(Context.LOCATION_SERVICE) as LocationManager
            lm.isLocationEnabled
        } else {
            val mode = Settings.Secure.getInt(
                contentResolver, Settings.Secure.LOCATION_MODE,
                Settings.Secure.LOCATION_MODE_OFF
            )
            mode != Settings.Secure.LOCATION_MODE_OFF
        }
    }

    private fun updateLocation() {
        /*
        if (ActivityCompat.checkSelfPermission(
                applicationContext,
                Manifest.permission.ACCESS_FINE_LOCATION
            ) == PackageManager.PERMISSION_GRANTED
        ) {
            fusedLocationClient.locationAvailability
                .addOnSuccessListener { locationAvailability ->
                    Log.d(
                        "locationAvailability",
                        "onSuccess: locationAvailability.isLocationAvailable " + locationAvailability.isLocationAvailable
                    )
                    if (locationAvailability.isLocationAvailable) {
                        if (ActivityCompat.checkSelfPermission(
                                applicationContext,
                                Manifest.permission.ACCESS_FINE_LOCATION
                            )
                            == PackageManager.PERMISSION_GRANTED
                        ) {
                            val locationTask: Task<Location> = fusedLocationClient.lastLocation
                            locationTask.addOnCompleteListener { task ->
                                val location: Location = task.result
                            }
                        } else {
                            requestLocationPermissions()
                        }
                    } else {
                        val locationCallback = object : LocationCallback() {
                            override fun onLocationResult(locationResult: LocationResult) {
                                Log.e("locationResult", locationResult.locations.toString())
                                if (locationResult.locations.isNotEmpty()) {
                                    /*val location = locationResult.lastLocation
                                    Log.e("location", location.toString())*/
                                    val addresses: List<Address>?
                                    val geoCoder = Geocoder(applicationContext, Locale.getDefault())
                                    addresses = geoCoder.getFromLocation(
                                        locationResult.lastLocation.latitude,
                                        locationResult.lastLocation.longitude,
                                        1
                                    )
                                    if (addresses != null && addresses.isNotEmpty()) {
                                        val address: String = addresses[0].getAddressLine(0)
                                        val city: String = addresses[0].locality
                                        val state: String = addresses[0].adminArea
                                        val country: String = addresses[0].countryName
                                        val postalCode: String = addresses[0].postalCode
                                        val knownName: String = addresses[0].featureName
                                        Log.e("location", "$address $city $state $postalCode $country $knownName")
                                    }
                                }
                            }
                        }
                        fusedLocationClient.requestLocationUpdates(
                            LocationRequest.create().apply {
                                interval = 100
                                fastestInterval = 50
                                priority = LocationRequest.PRIORITY_HIGH_ACCURACY
                                maxWaitTime= 100
                            },
                            locationCallback, Looper.getMainLooper()
                        )
                    }
                }
        } else {
            requestLocationPermissions()
        }*/

        if (ActivityCompat.checkSelfPermission(
                this,
                Manifest.permission.ACCESS_FINE_LOCATION
            ) == PackageManager.PERMISSION_GRANTED || ActivityCompat.checkSelfPermission(
                this,
                Manifest.permission.ACCESS_COARSE_LOCATION
            ) == PackageManager.PERMISSION_GRANTED
        ) {
            fusedLocationClient.locationAvailability.addOnSuccessListener { locationAvailability ->
                Log.e("locationAvailability", locationAvailability.isLocationAvailable.toString())
                if (locationAvailability.isLocationAvailable) {
                    Log.e("position", "getting position")
                    fusedLocationClient.lastLocation.addOnSuccessListener { location3 ->
                        location = LatLng(location3.latitude, location3.longitude)
                        Log.e("position", location.toString())
                    }.addOnFailureListener {
                        Log.e("position get failed ", it.toString())
                    }
                } else {
                    //requestLocationPermissions()
                    Log.e("location update", "requesting")
                    fusedLocationClient.requestLocationUpdates(
                        LocationRequest.create().apply {
                            priority = LocationRequest.PRIORITY_HIGH_ACCURACY
                            interval = 10000L
                            fastestInterval = 1000L
                            numUpdates = 2
                        },
                        object: LocationCallback() {
                            override fun onLocationResult(result: LocationResult) {
                                result.lastLocation.apply {
                                    location = LatLng(longitude, latitude)
                                    Log.e("location update", location.toString())
                                }
                                fusedLocationClient.removeLocationUpdates(this)
                            }

                            override fun onLocationAvailability(p0: LocationAvailability) {
                                super.onLocationAvailability(p0)
                                Log.e("location availability", p0.toString())
                            }
                        }, Looper.getMainLooper())
                }
            }
        } else {
            Log.e("position", "no permission")
            location = null
        }
    }

    override fun isOnline(): Boolean {
        val connectivityManager =
            getSystemService(Context.CONNECTIVITY_SERVICE) as ConnectivityManager
        val capabilities =
            connectivityManager.getNetworkCapabilities(connectivityManager.activeNetwork)
        if (capabilities != null) {
            when {
                capabilities.hasTransport(NetworkCapabilities.TRANSPORT_CELLULAR) -> {
                    Log.i("Internet", "NetworkCapabilities.TRANSPORT_CELLULAR")
                    return true
                }
                capabilities.hasTransport(NetworkCapabilities.TRANSPORT_WIFI) -> {
                    Log.i("Internet", "NetworkCapabilities.TRANSPORT_WIFI")
                    return true
                }
                capabilities.hasTransport(NetworkCapabilities.TRANSPORT_ETHERNET) -> {
                    Log.i("Internet", "NetworkCapabilities.TRANSPORT_ETHERNET")
                    return true
                }
            }
        }
        return false
    }

    override fun getUpdateTimer(): LiveData<Int> {
        return updateTimer
    }

    override fun setFavorite(agencyId: String, favorite: Boolean) {
        if (favorite) {
            agencyViewModel.addFavoriteAgency(FavoriteAgency(agencyId))
        } else {
            agencyViewModel.deleteFavoriteAgency(FavoriteAgency(agencyId))
        }
    }

    override fun getDestinationId(): String {
        return destinationId
    }

    override fun setDestinationId(id: String) {
        destinationId = id
        binding.bottomNavigationView.menu[1].onNavDestinationSelected(binding.fragmentContainerViewMain.findNavController())
    }

    override fun getLocation(): LatLng? {
        return location
    }

    override fun getViewPagerPosition(): Int {
        return viewPagerPosition
    }

    override fun setViewPagerPosition(position: Int) {
        viewPagerPosition = position
    }

    override fun getReservationPosition(): Int {
        return reservationPosition
    }

    override fun setReservationPosition(position: Int) {
        reservationPosition = position
    }

    override fun getAgencyStatus(): LiveData<HashMap<String, Agency>?> {
        return agencyStatus
    }

    override fun updateAgencyStatus() {
        if (isOnline()) {
            firebase.get().addOnSuccessListener { dataSnapshot ->
                val type = object : GenericTypeIndicator<HashMap<String, Agency>>() {}
                val data = dataSnapshot.getValue(type)
                agencyStatus.value = data
                updateTimer.value = 0
                if (data != null) {
                    Log.e("agencyStatus", "update")
                } else {
                    Log.e("agencyStatus", "null")
                }
            }.addOnFailureListener {
                agencyStatus.value = null
            }
        }

        if (isLocationEnabled()) {
            updateLocation()
        } else {
            Log.e("location", "location is not enabled")
        }
    }

    /*
    override fun getReservationId(): String {
        return reservationId
    }
    */

    override fun setReservationId(agencyId: String): Boolean {
        val agency = agencyStatus.value?.get(agencyId)
        return if (agency != null) {
            if (reservationViewModel.agencyId != agency.id) {
                reservationViewModel.agencyId = agencyId
                //reservationViewModel.agencyName = agency.name
                //reservationViewModel.serviceTypeList = agency.dataFormat
                reservationViewModel.serviceType = 0
                reservationViewModel.reservationData = if (agency.dataFormat.isEmpty()) mutableListOf()
                    else MutableList(agency.dataFormat[0].dataFormats.count()) { "" }
            }
            reservationPosition = 0
            binding.bottomNavigationView.menu[2].onNavDestinationSelected(binding.fragmentContainerViewMain.findNavController())
            true
        } else {
            false
        }
    }

    override fun sendReservation(agencyId: String, data: ReservationData) {
        val key = firebase.child("$agencyId/queue").push().key
        if (key != null) {
            data.pushKey = key
            val valueChangedListener = object: ValueEventListener {
                override fun onDataChange(snapshot: DataSnapshot) {
                    val responseData = snapshot.getValue(ResponseData::class.java)
                    if (responseData != null) {
                        if (responseData.response == "No") {
                            data.response = responseData.response
                            data.message = responseData.message
                            data.state.value = ReservationState.ReservationDenied
                            firebase.child("$agencyId/queue/$key/response")
                                .removeEventListener(this)
                            firebase.child("$agencyId/queue/$key").removeValue()
                        } else if (responseData.response != "0" && responseData.response != null) {
                            data.response = responseData.response
                            data.message = responseData.message
                            data.state.value = ReservationState.ReservationSuccess
                            firebase.child("$agencyId/queue/$key/response")
                                .removeEventListener(this)
                            firebase.child("$agencyId/queue/$key").removeValue()
                        }
                    }
                }
                override fun onCancelled(error: DatabaseError) {
                    Log.e("listener cancelled", error.toString())
                }
            }
            firebase.child("$agencyId/queue/$key").addValueEventListener (valueChangedListener)
            firebase.child("$agencyId/queue/$key").updateChildren(data.toMap())

        } else {
            data.state.value = ReservationState.KeyIsNull
        }
    }

    override fun getNotificationSettings(): NotificationSettings {
        return notificationSettings
    }

    override fun setReminder(agencyId: String, serviceType: Int, serviceTypeName: String, response: String) {
        if (reminderAgencyId != agencyId && reminderServiceType != serviceType) {
            reminderAgencyId = agencyId
            reminderServiceType = serviceType
            val notificationId = 1 //TODO what is this?
            val num = response.substring(1).toInt()
            val builder = NotificationCompat.Builder(this@MainActivity, "reminder")
                //.setSmallIcon(R.drawable.ic_round_notifications_48)
                .setContentTitle(serviceTypeName)
                //.setContentText("This is content")
                //.setPriority(NotificationCompat.PRIORITY_DEFAULT)
            val valueChangedListener = object : ValueEventListener {
                override fun onDataChange(snapshot: DataSnapshot) {
                    if (!notificationSettings.isEnabled || reminderAgencyId != agencyId || reminderServiceType != serviceType) {
                        firebase.child("${agencyId}/next_num/${serviceType}").removeEventListener(this)
                    }
                    snapshot.getValue(String::class.java)?.let { next_num ->
                        val next = next_num.substring(1).toInt()
                        Log.e("next_num", "$next_num $next $num ${notificationSettings.numberOfPeople}")
                        if (next <= num) {
                            if (num - next > notificationSettings.numberOfPeople) {
                                val content = getString(R.string.notification_content, response, num - next)
                                builder.setSmallIcon(R.drawable.ic_round_notifications_48)
                                    .setContentText(content)
                                    .setPriority(NotificationCompat.PRIORITY_LOW)
                                    .setOnlyAlertOnce(true)
                            } else {
                                val content = getString(R.string.notification_content, response, num - next)
                                builder.setSmallIcon(R.drawable.ic_round_notifications_active_48)
                                        .setContentText(content)
                                        .setPriority(NotificationCompat.PRIORITY_HIGH)
                                        .setOnlyAlertOnce(false)
                            }
                            with(NotificationManagerCompat.from(this@MainActivity)) {
                                notify(notificationId, builder.build())
                            }
                        }
                    }
                }

                override fun onCancelled(error: DatabaseError) {
                    Log.e("listener cancelled", error.toString())
                }

            }
            firebase.child("${agencyId}/next_num/${serviceType}")
                .addValueEventListener(valueChangedListener)
        }
    }
}