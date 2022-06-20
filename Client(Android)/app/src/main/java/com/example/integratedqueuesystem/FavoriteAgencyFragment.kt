package com.example.integratedqueuesystem

import android.location.Location
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
import com.example.integratedqueuesystem.databinding.FragmentFavoriteAgencyBinding
import com.google.android.gms.maps.model.LatLng

class FavoriteAgencyFragment: Fragment() {

    private var _binding: FragmentFavoriteAgencyBinding? = null
    private val binding get() = _binding!!
    private lateinit var communicator: Communicator
    private lateinit var agencyViewModel: AgencyViewModel

    override fun onCreateView(
        inflater: LayoutInflater,
        container: ViewGroup?,
        savedInstanceState: Bundle?
    ): View {
        _binding = FragmentFavoriteAgencyBinding.inflate(inflater, container, false)
        val view = binding.root

        communicator = activity as Communicator
        val agencyStatus = communicator.getAgencyStatus()
        var favoriteAgencyIdList = emptyList<String>()
        var agencyHashMap = hashMapOf<String, Agency>()

        val adapter = AgencyAdapter(communicator)
        binding.favoriteAgencyListRv.adapter = adapter
        binding.favoriteAgencyListRv.layoutManager = LinearLayoutManager(requireContext())

        agencyViewModel = ViewModelProvider(requireActivity())[AgencyViewModel::class.java]

        fun refresh() {
            val data = agencyHashMap.filterKeys { favoriteAgencyIdList.contains(it) }
            adapter.setAgencies(communicator.getLocation(), data.values.toList())
            if (data.isNotEmpty()) {
                binding.noFavoriteAgencyTv.visibility = View.GONE
            } else {
                binding.noFavoriteAgencyTv.visibility = View.VISIBLE
            }
        }

        agencyViewModel.allFavoriteAgencyIds.observe(viewLifecycleOwner, { ids ->
            favoriteAgencyIdList = ids ?: emptyList()
            refresh()
        })

        agencyStatus.observe(viewLifecycleOwner, { agencies ->
            if (agencies != null) {
                agencyHashMap = agencies
                refresh()
            }
        })

        agencyViewModel.allFavoriteAgencies.observe(viewLifecycleOwner, { agencies ->
            if (agencies != null) {
                if (adapter.agencyIsEmpty()) {
                    Log.e("favorite", agencies.count().toString())
                    val data = agencies.map { Agency(it) }
                    adapter.setAgencies(communicator.getLocation(), data)
                    binding.noFavoriteAgencyTv.visibility =
                        if (data.isNotEmpty()) View.GONE else View.VISIBLE
                }
            } else if (adapter.agencyIsEmpty()) {
                binding.noFavoriteAgencyTv.visibility = View.VISIBLE
            }
        })

        return view
    }

    class AgencyAdapter(private val communicator: Communicator): RecyclerView.Adapter<AgencyAdapter.AgencyViewHolder>() {

        private var agencyList = emptyList<Agency>()
        private var location: LatLng? = null

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
            val currentItem = agencyList[position]

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
                val startPoint = Location("").apply {
                    latitude = location!!.latitude
                    longitude = location!!.longitude
                }
                val endPoint = Location("").apply {
                    latitude = currentItem.latitude
                    longitude = currentItem.longitude
                }
                val distance = startPoint.distanceTo(endPoint)
                holder.distanceTv.text = if (distance < 1000F) {
                    holder.itemView.context.getString(R.string.agency_distance_meter, distance.toInt())
                } else {
                    holder.itemView.context.getString(R.string.agency_distance_kilometer, distance / 1000F)
                }
            } else {
                holder.distanceTv.text = holder.itemView.context.getString(R.string.unknown)
            }

            holder.favoriteBtn.setImageResource(R.drawable.ic_round_star_48)
            holder.favoriteBtn.setOnClickListener {
                holder.favoriteBtn.setImageResource(R.drawable.ic_round_star_border_48)
                communicator.setFavorite(currentItem.id, false)
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
            //notifyItemRangeChanged(0, agencies.count()) TODO error Inconsistency detected.
            notifyDataSetChanged()
        }

        fun agencyIsEmpty() = agencyList.isEmpty()
    }

    override fun onDestroyView() {
        super.onDestroyView()
        _binding = null
    }
}