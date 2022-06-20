package com.example.integratedqueuesystem

import android.location.Location
import android.location.LocationManager
import android.os.Bundle
import android.util.Log
import android.view.LayoutInflater
import android.view.View
import android.view.ViewGroup
import android.widget.Button
import android.widget.ImageView
import android.widget.TextView
import androidx.constraintlayout.motion.widget.MotionLayout
import androidx.core.content.ContextCompat
import androidx.fragment.app.Fragment
import androidx.lifecycle.ViewModelProvider
import androidx.recyclerview.widget.LinearLayoutManager
import androidx.recyclerview.widget.RecyclerView
import com.example.integratedqueuesystem.data.agency.Agency
import com.example.integratedqueuesystem.data.agency.AgencyViewModel
import com.example.integratedqueuesystem.databinding.FragmentAgencyBinding
import com.google.android.gms.maps.model.LatLng

class AgencyFragment(private val agencyType: Int): Fragment() {

    private var _binding: FragmentAgencyBinding? = null
    private val binding get() = _binding!!
    private lateinit var communicator: Communicator
    private lateinit var agencyViewModel: AgencyViewModel

    override fun onCreateView (
        inflater: LayoutInflater,
        container: ViewGroup?,
        savedInstanceState: Bundle?
    ): View {
        _binding = FragmentAgencyBinding.inflate(inflater, container, false)
        val view = binding.root

        communicator = activity as Communicator
        val agencyList = communicator.getAgencyStatus()
        var favoriteAgencyIdList = emptyList<String>()

        binding.noAgencyTv.text = getString(
            when (agencyType) {
                0 -> R.string.no_favorite_agency
                1 -> R.string.no_hospital
                2 -> R.string.no_school
                3 -> R.string.no_clinic
                4 -> R.string.no_post_office
                5 -> R.string.no_government
                6 -> R.string.no_restaurant
                7 -> R.string.no_store
                8 -> R.string.no_other
                else -> R.string.no_agency_default
            }
        )
        
        val adapter = AgencyAdapter(communicator)
        binding.agencyListRv.adapter = adapter
        binding.agencyListRv.layoutManager = LinearLayoutManager(requireContext())

        agencyViewModel = ViewModelProvider(requireActivity())[AgencyViewModel::class.java]

        agencyViewModel.allFavoriteAgencyIds.observe(viewLifecycleOwner) { ids ->
            adapter.setFavoriteList(ids)
        }

        agencyList.observe(viewLifecycleOwner) { agencies ->
            if (agencies != null) {
                val data = agencies.filterValues { it.type == agencyType }
                if (data.isNotEmpty()) {
                    adapter.setAgencies(communicator.getLocation(), data.values.toList())
                    binding.noAgencyTv.visibility = View.GONE
                } else {
                    binding.noAgencyTv.visibility = View.VISIBLE
                }
            }
        }

        agencyViewModel.allAgencies.observe(viewLifecycleOwner) { agencies ->
            if (agencies != null) {
                if (adapter.agencyIsEmpty()) {
                    val data = agencies.filter { it.type == agencyType }.map { Agency(it) }
                    adapter.setAgencies(communicator.getLocation(), data)
                    binding.noAgencyTv.visibility =
                        if (data.isNotEmpty()) View.GONE else View.VISIBLE
                }
            } else if (adapter.agencyIsEmpty()) {
                binding.noAgencyTv.visibility = View.VISIBLE
            }
        }

        return view
    }

    class AgencyAdapter(private val communicator: Communicator): RecyclerView.Adapter<AgencyAdapter.AgencyViewHolder>() {

        private var agencyList = emptyList<Agency>()
        private var location: LatLng? = null
        private var favoriteAgencyIdList = emptyList<String>()

        class AgencyViewHolder(itemView: View): RecyclerView.ViewHolder(itemView) {
            val motionLayout: MotionLayout = itemView.findViewById(R.id.motion_layout)
            val nameTv: TextView = itemView.findViewById(R.id.agency_name_tv)
            val statusTv: TextView = itemView.findViewById(R.id.agency_status_tv)
            val pendingTv: TextView = itemView.findViewById(R.id.agency_pending_tv)
            val distanceTv: TextView = itemView.findViewById(R.id.agency_distance_tv)
            val favoriteBtn: ImageView = itemView.findViewById(R.id.favorite_btn)
            val locationBtn: Button = itemView.findViewById(R.id.location_btn)
            val reservationBtn: Button = itemView.findViewById(R.id.reservation_btn)
        }

        override fun onCreateViewHolder(parent: ViewGroup, viewType: Int): AgencyViewHolder {
            return AgencyViewHolder(
                LayoutInflater.from(parent.context).inflate(R.layout.agency_row, parent, false)
            )
        }

        override fun getItemCount() = agencyList.count()

        override fun onBindViewHolder(holder: AgencyViewHolder, position: Int) {
            val currentItem = agencyList[holder.adapterPosition]

            holder.nameTv.text = currentItem.name

            holder.statusTv.text = holder.itemView.context.getString(
                when (currentItem.status) {
                    0 -> R.string.unknown
                    1 -> R.string.not_support
                    2 -> R.string.closed
                    3 -> R.string.open
                    4 -> R.string.paused
                    5 -> R.string.serving
                    else -> R.string.unknown
                }
            )
            holder.statusTv.setTextColor(
                ContextCompat.getColor(
                    holder.itemView.context,
                    when (currentItem.status) {
                        1, 2, 4 -> R.color.holo_red
                        3, 5 -> R.color.holo_green
                        else -> R.color.default_text_color
                    }
                )
            )

            holder.pendingTv.text = if (currentItem.pending.isNotEmpty()) {
                holder.itemView.context.getString(R.string.agency_pending, currentItem.pending.sum())
            } else {
                holder.itemView.context.getString(R.string.unknown)
            }

            if (location != null) {
                val startPoint = Location(LocationManager.PASSIVE_PROVIDER).apply {
                    latitude = location!!.latitude
                    longitude = location!!.longitude
                }
                val endPoint = Location(LocationManager.PASSIVE_PROVIDER).apply {
                    latitude = currentItem.latitude
                    longitude = currentItem.longitude
                }
                val distance = startPoint.distanceTo(endPoint)
                holder.distanceTv.text = if (distance < 1000) {
                    holder.itemView.context.getString(R.string.agency_distance_meter, distance.toInt())
                } else {
                    holder.itemView.context.getString(R.string.agency_distance_kilometer, distance / 1000)
                }
            } else {
                holder.distanceTv.text = holder.itemView.context.getString(R.string.unknown)
            }

            holder.favoriteBtn.setImageResource(
                if (favoriteAgencyIdList.contains(currentItem.id)) R.drawable.ic_round_star_48
                else R.drawable.ic_round_star_border_48)
            holder.favoriteBtn.setOnClickListener {
                if (favoriteAgencyIdList.contains(currentItem.id)) {
                    holder.favoriteBtn.setImageResource(R.drawable.ic_round_star_border_48)
                    communicator.setFavorite(currentItem.id, false)
                } else {
                    holder.favoriteBtn.setImageResource(R.drawable.ic_round_star_48)
                    communicator.setFavorite(currentItem.id, true)
                }
            }

            holder.locationBtn.setOnClickListener {
                communicator.setDestinationId(currentItem.id)
            }

            holder.reservationBtn.setOnClickListener {
                communicator.setReservationId(currentItem.id)
            }

            holder.motionLayout.setTransitionListener(object: MotionLayout.TransitionListener {
                override fun onTransitionStarted(
                    motionLayout: MotionLayout?,
                    startId: Int,
                    endId: Int
                ) {
                    if (startId == R.id.start) {
                        holder.locationBtn.isEnabled = true
                        holder.reservationBtn.isEnabled = true
                    }
                }

                override fun onTransitionChange(
                    motionLayout: MotionLayout?,
                    startId: Int,
                    endId: Int,
                    progress: Float
                ) {

                }

                override fun onTransitionCompleted(motionLayout: MotionLayout?, currentId: Int) {
                    if (currentId == R.id.start) {
                        holder.locationBtn.isEnabled = false
                        holder.reservationBtn.isEnabled = false
                    }
                }

                override fun onTransitionTrigger(
                    motionLayout: MotionLayout?,
                    triggerId: Int,
                    positive: Boolean,
                    progress: Float
                ) {

                }

            })
        }

        fun setAgencies(position: LatLng?, agencies: List<Agency>) {
            location = position
            agencyList = agencies
            notifyItemRangeChanged(0, agencies.count())
        }

        fun setFavoriteList(favoriteAgencyIds: List<String>) {
            favoriteAgencyIdList = favoriteAgencyIds
            if (!agencyIsEmpty()) {
                notifyItemRangeChanged(0, agencyList.count())
            }
        }

        fun agencyIsEmpty() = agencyList.isEmpty()
    }

    override fun onDestroyView() {
        super.onDestroyView()
        _binding = null
    }
}