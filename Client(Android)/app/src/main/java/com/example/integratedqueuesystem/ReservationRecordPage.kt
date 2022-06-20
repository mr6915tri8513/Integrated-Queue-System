package com.example.integratedqueuesystem

import android.os.Bundle
import android.view.LayoutInflater
import android.view.View
import android.view.ViewGroup
import android.widget.TextView
import androidx.core.content.ContextCompat
import androidx.fragment.app.Fragment
import androidx.fragment.app.FragmentManager
import androidx.lifecycle.ViewModelProvider
import androidx.recyclerview.widget.LinearLayoutManager
import androidx.recyclerview.widget.RecyclerView
import com.example.integratedqueuesystem.data.reservation.ReservationRecord
import com.example.integratedqueuesystem.data.reservation.ReservationRecordViewModel
import com.example.integratedqueuesystem.data.reservation.ReservationState
import com.example.integratedqueuesystem.databinding.FragmentReservationRecordPageBinding
import java.time.LocalDateTime
import java.time.format.DateTimeFormatter
import java.util.*

class ReservationRecordPage: Fragment() {

    private var _binding: FragmentReservationRecordPageBinding? = null
    private val binding get() = _binding!!
    private lateinit var reservationRecordViewModel: ReservationRecordViewModel

    override fun onCreateView(
        inflater: LayoutInflater,
        container: ViewGroup?,
        savedInstanceState: Bundle?
    ): View {
        _binding = FragmentReservationRecordPageBinding.inflate(inflater, container, false)
        val view = binding.root

        reservationRecordViewModel = ViewModelProvider(requireActivity())[ReservationRecordViewModel::class.java]

        val adapter = ReservationRecordListAdapter(parentFragmentManager)
        binding.reservationRecordRv.adapter = adapter
        binding.reservationRecordRv.layoutManager = LinearLayoutManager(requireContext())

        reservationRecordViewModel.allRecords.observe(viewLifecycleOwner, { records ->
            if (records.isNullOrEmpty()) {
                binding.noReservationRecordTv.visibility = View.VISIBLE
            } else {
                binding.noReservationRecordTv.visibility = View.GONE
                adapter.setRecords(records)
            }
        })

        return view
    }

    class ReservationRecordListAdapter(private val fragmentManager: FragmentManager): RecyclerView.Adapter<ReservationRecordListAdapter.ReservationRecordViewHolder>() {

        private var recordList = emptyList<ReservationRecord>()
        private var now = LocalDateTime.now()

        class ReservationRecordViewHolder(itemView: View): RecyclerView.ViewHolder(itemView) {
            val nameTv: TextView = itemView.findViewById(R.id.agency_name_tv)
            val stateTv: TextView = itemView.findViewById(R.id.reservation_state_tv)
            val serviceTypeTv: TextView = itemView.findViewById(R.id.service_type_tv)
            val dateTimeTv: TextView = itemView.findViewById(R.id.date_time_tv)
        }

        override fun onCreateViewHolder(parent: ViewGroup, viewType: Int): ReservationRecordViewHolder {
            return ReservationRecordViewHolder(
                LayoutInflater.from(parent.context).inflate(R.layout.reservation_record_row, parent, false)
            )
        }

        override fun getItemCount(): Int {
            return recordList.count()
        }

        override fun onBindViewHolder(holder: ReservationRecordViewHolder, position: Int) {
            val currentItem = recordList[position]

            holder.nameTv.text = currentItem.agencyName

            holder.stateTv.text = holder.itemView.context.getString (
                when (currentItem.state) {
                    ReservationState.WaitingForServer -> R.string.waiting_for_server
                    ReservationState.ReservationCanceled -> R.string.reservation_record_canceled
                    ReservationState.ReservationTimeout -> R.string.reservation_record_timeout
                    ReservationState.KeyIsNull -> R.string.reservation_record_key_is_null
                    ReservationState.ReservationFailed -> R.string.reservation_record_failed
                    ReservationState.ReservationDenied -> R.string.reservation_record_denied
                    ReservationState.ReservationSuccess -> R.string.reservation_record_success
                    else -> R.string.reservation_state_default
                })
            holder.stateTv.setTextColor(
                ContextCompat.getColor(
                    holder.itemView.context,
                    when (currentItem.state) {
                        ReservationState.WaitingForServer -> R.color.default_text_color
                        ReservationState.ReservationCanceled, ReservationState.ReservationTimeout, ReservationState.KeyIsNull, ReservationState.ReservationFailed, ReservationState.ReservationDenied -> R.color.holo_red
                        ReservationState.ReservationSuccess -> R.color.holo_green
                        else -> R.color.default_text_color
                    }
                )
            )

            holder.serviceTypeTv.text = currentItem.serviceTypeName

            val formatter = currentItem.dateTime.let {
                if (it.year == now.year && it.month == now.month && it.dayOfMonth == now.dayOfMonth) {
                    DateTimeFormatter.ofPattern("a hh:mm", Locale.TAIWAN)
                } else if (it.year == now.year){
                    DateTimeFormatter.ofPattern("MM/dd", Locale.TAIWAN)
                } else {
                    DateTimeFormatter.ofPattern("yyyy/MM/dd", Locale.TAIWAN)
                }
            }
            holder.dateTimeTv.text = currentItem.dateTime.format(formatter)

            holder.itemView.setOnClickListener {
                val dialog = ReservationDetailDialogFragment(currentItem)
                dialog.show(fragmentManager, "reservation_detail")
            }
        }

        fun setRecords(records: List<ReservationRecord>) {
            recordList = records
            now = LocalDateTime.now()
            notifyItemRangeChanged(0, records.count())
        }
    }
}