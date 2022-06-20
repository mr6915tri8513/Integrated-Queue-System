package com.example.integratedqueuesystem.data.reservation

import android.content.Context
import androidx.room.*
import java.time.LocalDate
import java.time.OffsetDateTime
import java.time.format.DateTimeFormatter

class DateTimeConverter {
    private val formatter = DateTimeFormatter.ISO_OFFSET_DATE_TIME

    @TypeConverter
    fun fromString(value: String): OffsetDateTime {
        return formatter.parse(value, OffsetDateTime::from)
    }

    @TypeConverter
    fun toString(date: OffsetDateTime): String {
        return date.format(formatter)
    }
}

@Database(entities = [ReservationRecord::class], version = 1, exportSchema = true)
@TypeConverters(DateTimeConverter::class)
abstract class ReservationRecordDatabase: RoomDatabase() {

    abstract fun reservationRecordDao(): ReservationRecordDao

    companion object {
        @Volatile
        private var INSTANCE: ReservationRecordDatabase? = null

        fun getDatabase(context: Context): ReservationRecordDatabase {
            val tempInstance = INSTANCE
            if (tempInstance != null) {
                return tempInstance
            }
            synchronized(this) {
                val instance = Room.databaseBuilder(
                    context.applicationContext,
                    ReservationRecordDatabase::class.java,
                    "reservation_record_database"
                ).build()
                INSTANCE = instance
                return instance
            }
        }
    }
}