package com.example.integratedqueuesystem

import android.app.AlertDialog
import android.app.Dialog
import android.os.Bundle
import android.view.LayoutInflater
import android.view.View
import android.view.ViewGroup
import android.view.Window
import androidx.fragment.app.DialogFragment
import android.view.WindowManager

import android.view.Gravity
import android.widget.*
import androidx.core.content.ContextCompat
import androidx.core.view.marginLeft
import com.example.integratedqueuesystem.data.reservation.ReservationRecord
import com.google.android.material.snackbar.Snackbar
import com.google.android.material.textfield.TextInputLayout
import java.time.format.DateTimeFormatter
import java.util.*


class ReservationDetailDialogFragment(private val record: ReservationRecord) : DialogFragment() {

    private lateinit var communicator: Communicator

    override fun onCreateView (
        inflater: LayoutInflater,
        container: ViewGroup?,
        savedInstanceState: Bundle?
    ): View? {
        val view = inflater.inflate(R.layout.fragment_reservation_detail, container, false)

        communicator = activity as Communicator

        val agencyNameTv: TextView = view.findViewById(R.id.agency_name_tv)
        val serviceTypeTv: TextView = view.findViewById(R.id.service_type_tv)
        val reservationNumTv: TextView = view.findViewById(R.id.reservation_num_tv)
        val dateTimeTv: TextView = view.findViewById(R.id.date_time_tv)

        val setReminderCb: CheckBox = view.findViewById(R.id.set_reminder_cb)
        val advancePeopleDm: TextInputLayout = view.findViewById(R.id.advance_people_dm)
        val advancePeopleTv: AutoCompleteTextView = view.findViewById(R.id.advance_people_tv)

        val formatter = DateTimeFormatter.ofPattern("yyyy/MM/dd a hh:mm:ss", Locale.TAIWAN)
        val notificationSettings = communicator.getNotificationSettings()

        agencyNameTv.text = record.agencyName
        serviceTypeTv.text = record.serviceTypeName
        reservationNumTv.text = record.num ?: getString(R.string.reservation_record_failed)
        reservationNumTv.setTextColor(
            ContextCompat.getColor(
                requireContext(),
                if (record.num == null) R.color.holo_red else R.color.text_color
            )
        )
        dateTimeTv.text = record.dateTime.format(formatter)

        setReminderCb.isChecked = notificationSettings.isEnabled
        if (record.num != null) {
            setReminderCb.isEnabled = true

            if (setReminderCb.isChecked) {
                advancePeopleDm.isEnabled = true
                advancePeopleTv.isEnabled = true
                advancePeopleTv.setText(notificationSettings.numberOfPeople.toString())
            }


            val arrayAdapter = ArrayAdapter(
                requireContext(),
                R.layout.dropdown_item,
                arrayOf("2", "3", "4", "5", "6", "7", "8", "9", "10")
            )

            advancePeopleTv.setAdapter(arrayAdapter)
            advancePeopleTv.setOnItemClickListener { adapterView, v, pos, l ->
                notificationSettings.numberOfPeople = pos + 2
            }

            setReminderCb.setOnCheckedChangeListener { compoundButton, isChecked ->
                notificationSettings.isEnabled = if (isChecked) {
                    advancePeopleDm.isEnabled = true
                    advancePeopleTv.isEnabled = true
                    advancePeopleTv.setAdapter(arrayAdapter)
                    //advancePeopleTv.setText(notificationSettings.numberOfPeople.toString())
                    communicator.setReminder(record.agencyId, record.serviceType, record.serviceTypeName, record.num)
                    true
                } else {
                    advancePeopleDm.isEnabled = false
                    advancePeopleTv.isEnabled = false
                    false
                }
            }
        }

        view.setOnClickListener {
            dismiss()
        }

        return view
    }
}