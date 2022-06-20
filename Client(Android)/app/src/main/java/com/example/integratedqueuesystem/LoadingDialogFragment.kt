package com.example.integratedqueuesystem

import android.app.AlertDialog
import android.app.Dialog
import android.os.Bundle
import androidx.fragment.app.DialogFragment
import android.content.DialogInterface

import android.app.Activity
import androidx.lifecycle.MutableLiveData
import com.example.integratedqueuesystem.data.reservation.ReservationState


class LoadingDialogFragment(private val message: String, private val state: MutableLiveData<Int>) : DialogFragment() {
    override fun onCreateDialog(savedInstanceState: Bundle?): Dialog {
        return activity?.let {
            // Use the Builder class for convenient dialog construction
            val builder = AlertDialog.Builder(it)//TODO set title?
            builder.setMessage(message)
                .setPositiveButton(getString(R.string.cancel)) { dialog, id ->
                    if (state.value == ReservationState.WaitingForServer) {
                        state.value = ReservationState.ReservationCanceled
                    }
                }
            // Create the AlertDialog object and return it
            /*
            val alertDialog = builder.create()
            alertDialog.setCanceledOnTouchOutside(false)
            alertDialog*/
            builder.create().apply {
                setCanceledOnTouchOutside(false)
            }

        } ?: throw IllegalStateException("Activity cannot be null")
    }
}