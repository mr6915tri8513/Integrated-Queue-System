package com.example.integratedqueuesystem.data.reservation

import androidx.lifecycle.LiveData

class ReservationRecordRepository(private val reservationRecordDao: ReservationRecordDao) {

    val allRecords: LiveData<List<ReservationRecord>> = reservationRecordDao.readAllRecord()

    suspend fun addRecord(record: ReservationRecord): Long {
        return reservationRecordDao.addRecord(record)
    }

    suspend fun updateState(id: Long, state: Int) {
        reservationRecordDao.updateState(id, state)
    }

    suspend fun updateState(id: Long, state: Int, pushKey: String, num: String) {
        reservationRecordDao.updateState(id, state, pushKey, num)
    }
}