package com.example.integratedqueuesystem.data.reservation

import android.app.Application
import androidx.lifecycle.AndroidViewModel
import androidx.lifecycle.LiveData
import androidx.lifecycle.viewModelScope
import kotlinx.coroutines.Dispatchers
import kotlinx.coroutines.launch

class ReservationRecordViewModel(application: Application): AndroidViewModel(application) {

    val allRecords: LiveData<List<ReservationRecord>>
    private val repository: ReservationRecordRepository

    init {
        val reservationRecordDao = ReservationRecordDatabase.getDatabase(application).reservationRecordDao()
        repository = ReservationRecordRepository(reservationRecordDao)
        allRecords = repository.allRecords
    }

    fun addRecord(record: ReservationRecord, ret: (Long) -> Unit) {
        viewModelScope.launch(Dispatchers.Main) {
            ret(repository.addRecord(record))
        }
    }

    fun updateState(id: Long, state: Int) {
        viewModelScope.launch(Dispatchers.IO) {
            repository.updateState(id, state)
        }
    }

    fun updateState(id: Long, state: Int, pushKey: String, num: String) {
        viewModelScope.launch(Dispatchers.IO) {
            repository.updateState(id, state, pushKey, num)
        }
    }
}