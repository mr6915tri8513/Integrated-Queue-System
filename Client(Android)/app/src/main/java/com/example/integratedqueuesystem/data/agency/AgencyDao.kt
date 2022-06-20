package com.example.integratedqueuesystem.data.agency

import androidx.lifecycle.LiveData
import androidx.room.*

@Dao
interface AgencyDao {

    @Insert(onConflict = OnConflictStrategy.REPLACE)
    suspend fun updateAgencies(agencies: List<AgencyData>)

    @Insert(onConflict = OnConflictStrategy.IGNORE)
    suspend fun addFavoriteAgency(agency: FavoriteAgency)

    @Delete
    suspend fun deleteFavoriteAgency(agency: FavoriteAgency)

    @Query("SELECT * FROM agency_table")
    fun readAllAgencies(): LiveData<List<AgencyData>>

    @Query("SELECT * FROM agency_table INNER JOIN favorite_agency_table ON agency_table.id = favorite_agency_table.agencyId")
    fun readAllFavoriteAgencies(): LiveData<List<AgencyData>>

    @Query("SELECT agencyId FROM favorite_agency_table")
    fun readAllFavoriteAgencyIds(): LiveData<List<String>>
}