package com.example.integratedqueuesystem.data.reservation

import androidx.lifecycle.LiveData
import androidx.room.Dao
import androidx.room.Insert
import androidx.room.OnConflictStrategy
import androidx.room.Query

@Dao
interface ReservationRecordDao {

    @Insert(onConflict = OnConflictStrategy.IGNORE)
    suspend fun addRecord(record: ReservationRecord): Long

    @Query("UPDATE reservation_record_table SET state = :state WHERE id = :id")
    suspend fun updateState(id: Long, state: Int)

    @Query("UPDATE reservation_record_table SET state = :state, pushKey = :pushKey, num = :num WHERE id = :id")
    suspend fun updateState(id: Long, state: Int, pushKey: String, num: String)

    @Query("SELECT * FROM reservation_record_table ORDER BY datetime(dateTime) DESC")
    fun readAllRecord(): LiveData<List<ReservationRecord>>

}