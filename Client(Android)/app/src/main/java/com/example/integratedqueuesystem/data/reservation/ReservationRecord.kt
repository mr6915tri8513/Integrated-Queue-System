package com.example.integratedqueuesystem.data.reservation

import androidx.room.Embedded
import androidx.room.Entity
import androidx.room.PrimaryKey
import java.time.OffsetDateTime
import java.util.*

@Entity(tableName = "reservation_record_table")
data class ReservationRecord (
    @PrimaryKey(autoGenerate = true)
    val id: Long,
    val agencyId: String,
    val agencyName: String,
    val serviceType: Int,
    val serviceTypeName: String,
    val dateTime: OffsetDateTime,
    val state: Int,
    val pushKey: String?,
    val num: String?
) {
    constructor(agencyId: String, agencyName: String, serviceType: Int, serviceTypeName: String, dateTime: OffsetDateTime, state: Int): this (
        0, agencyId, agencyName, serviceType, serviceTypeName, dateTime, state, null, null
    )
}

class ReservationDateTime(
    val year: Int,
    val month: Int,
    val day: Int,
    val dayOfWeek: Int,
    val hour: Int,
    val minute: Int,
    val second: Int
) {
    constructor(calendar: Calendar) : this(
        calendar.get(Calendar.YEAR),
        calendar.get(Calendar.MONTH),
        calendar[Calendar.DAY_OF_MONTH],
        calendar[Calendar.DAY_OF_WEEK],
        calendar[Calendar.HOUR_OF_DAY],
        calendar[Calendar.MINUTE],
        calendar[Calendar.SECOND]
    )

    override fun toString(): String {
        return String.format("%04d/%02d/%02d Monday %02d:%02d:%02d", year, month + 1, day, hour, minute, second)
    }
}