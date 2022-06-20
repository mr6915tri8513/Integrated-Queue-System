package com.example.integratedqueuesystem.data.agency

import android.content.Context
import androidx.room.Database
import androidx.room.Room
import androidx.room.RoomDatabase

@Database(entities = [AgencyData::class, FavoriteAgency::class], version = 1, exportSchema = true)
abstract class AgencyDatabase: RoomDatabase() {

    abstract fun agencyDao(): AgencyDao

    companion object {
        @Volatile
        private var INSTANCE: AgencyDatabase? = null

        fun getDatabase(context: Context): AgencyDatabase {
            val tempInstance = INSTANCE
            if (tempInstance != null) {
                return tempInstance
            }
            synchronized(this) {
                val instance = Room.databaseBuilder(
                    context.applicationContext,
                    AgencyDatabase::class.java,
                    "agency_database"
                ).build()
                INSTANCE = instance
                return instance
            }
        }
    }
}