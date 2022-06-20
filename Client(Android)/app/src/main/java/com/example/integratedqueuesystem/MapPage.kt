package com.example.integratedqueuesystem

import android.location.Location
import androidx.fragment.app.Fragment

import android.os.Bundle
import android.view.LayoutInflater
import android.view.View
import android.view.ViewGroup
import androidx.core.content.ContextCompat
import androidx.lifecycle.ViewModelProvider
import com.example.integratedqueuesystem.data.agency.AgencyViewModel
import com.example.integratedqueuesystem.databinding.FragmentMapPageBinding

import com.google.android.gms.maps.CameraUpdateFactory
import com.google.android.gms.maps.OnMapReadyCallback
import com.google.android.gms.maps.SupportMapFragment
import com.google.android.gms.maps.model.LatLng
import com.google.android.gms.maps.model.MarkerOptions

class MapPage : Fragment() {

    private var _binding: FragmentMapPageBinding? = null
    private val binding get() = _binding!!
    private lateinit var communicator: Communicator
    private lateinit var agencyViewModel: AgencyViewModel

    private val callback = OnMapReadyCallback { googleMap ->
        /**
         * Manipulates the map once available.
         * This callback is triggered when the map is ready to be used.
         * This is where we can add markers or lines, add listeners or move the camera.
         * In this case, we just add a marker near Sydney, Australia.
         * If Google Play services is not installed on the device, the user will be prompted to
         * install it inside the SupportMapFragment. This method will only be triggered once the
         * user has installed Google Play services and returned to the app.
         */

        var favoriteAgencyIdList = emptyList<String>()

        agencyViewModel.allAgencies.observe(viewLifecycleOwner) { agencies ->
            for (agency in agencies) {
                val marker = googleMap.addMarker(
                    MarkerOptions().position(
                        LatLng(
                            agency.latitude,
                            agency.longitude
                        )
                    )
                )
                if (marker != null) {
                    marker.tag = agency.id
                }
            }
        }

        agencyViewModel.allFavoriteAgencyIds.observe(viewLifecycleOwner) { ids ->
            favoriteAgencyIdList = ids
        }

        val agencyList = communicator.getAgencyStatus()
        val destinationId = communicator.getDestinationId()
        val destination = agencyList.value?.get(destinationId)
        if (destinationId.isNotEmpty() && destination != null) {
            googleMap.moveCamera(CameraUpdateFactory.newLatLngZoom(LatLng(destination.latitude, destination.longitude), 16F))
            binding.root.transitionToState(R.id.show)
        } else {
            val location = communicator.getLocation()
            if (location != null) {
                googleMap.moveCamera(CameraUpdateFactory.newLatLngZoom(location, 16F))
            } else {
                val tcivs = LatLng(24.114646027661692, 120.66148596877323)
                //googleMap.addMarker(MarkerOptions().position(tcivs).title("Marker in tcivs"))
                googleMap.moveCamera(CameraUpdateFactory.newLatLngZoom(tcivs, 16F))
            }
        }

        fun setDetail(): Boolean {
            val id = communicator.getDestinationId()
            val agency = communicator.getAgencyStatus().value?.get(id)
            var isFavorite = favoriteAgencyIdList.contains(id)

            return if (agency != null) {

                binding.infoNameTv.text = agency.name

                binding.infoAgencyTypeTv.text = getString(when(agency.type) {
                    0 -> R.string.unknown
                    1 -> R.string.hospital
                    2 -> R.string.school
                    3 -> R.string.clinic
                    4 -> R.string.post_office
                    5 -> R.string.government
                    6 -> R.string.restaurant
                    7 -> R.string.store
                    8 -> R.string.other
                    else -> R.string.unknown
                })

                val location = communicator.getLocation()
                if (location != null) {
                    val startPoint = Location("").apply {
                        latitude = location.latitude
                        longitude = location.longitude
                    }
                    val endPoint = Location("").apply {
                        latitude = agency.latitude
                        longitude = agency.longitude
                    }
                    val distance = startPoint.distanceTo(endPoint)
                    binding.infoDistanceTv.text = if (distance < 1000) {
                        getString(R.string.agency_distance_meter, distance.toInt())
                    } else {
                        getString(R.string.agency_distance_kilometer, distance / 1000)
                    }
                } else {
                    binding.infoDistanceTv.text = getString(R.string.unknown)
                }

                binding.infoAgencyStatusTv.text = getString(
                    when (agency.status) {
                        0 -> R.string.unknown
                        1 -> R.string.not_support
                        2 -> R.string.closed
                        3 -> R.string.open
                        4 -> R.string.paused
                        5 -> R.string.serving
                        else -> R.string.unknown
                    }
                )
                binding.infoAgencyStatusTv.setTextColor(
                    ContextCompat.getColor(
                        requireContext(),
                        when (agency.status) {
                            1, 2, 4 -> R.color.holo_red
                            3, 5 -> R.color.holo_green
                            else -> R.color.default_text_color
                        }
                    )
                )

                binding.favoriteBtn.setImageResource(
                    if (isFavorite) R.drawable.ic_round_star_48
                    else R.drawable.ic_round_star_border_48
                )

                binding.favoriteBtn.setOnClickListener {
                    isFavorite = !isFavorite
                    binding.favoriteBtn.setImageResource(
                        if (isFavorite) R.drawable.ic_round_star_48
                        else R.drawable.ic_round_star_border_48
                    )
                    communicator.setFavorite(id, isFavorite)
                }

                //TODO add info
                true
            } else {
                false
            }
        }

        setDetail()

        googleMap.setOnMarkerClickListener { marker ->
            val markerId = marker.tag as String
            if (binding.root.currentState == R.id.hide) {
                binding.root.transitionToState(R.id.show)
            }
            if (markerId != communicator.getDestinationId()) {
                communicator.setDestinationId(markerId)
                setDetail()
            }
            false
        }
    }

    override fun onCreateView(
        inflater: LayoutInflater,
        container: ViewGroup?,
        savedInstanceState: Bundle?
    ): View {
        _binding = FragmentMapPageBinding.inflate(inflater, container, false)
        val view = binding.root

        communicator = activity as Communicator
        agencyViewModel = ViewModelProvider(requireActivity())[AgencyViewModel::class.java]

        binding.infoReservationBtn.setOnClickListener {
            communicator.getDestinationId().let { destinationId ->
                if (destinationId.isNotEmpty()) {
                    communicator.setReservationId(destinationId)
                }
            }
        }

        return view
    }

    override fun onViewCreated(view: View, savedInstanceState: Bundle?) {
        super.onViewCreated(view, savedInstanceState)
        val mapFragment = childFragmentManager.findFragmentById(R.id.map) as SupportMapFragment?
        mapFragment?.getMapAsync(callback)
    }

    override fun onDestroyView() {
        super.onDestroyView()
        _binding = null
    }
}