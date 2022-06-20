package com.example.integratedqueuesystem.data.agency

import androidx.lifecycle.LiveData

class AgencyRepository(private val agencyDao: AgencyDao) {

    val allAgencies: LiveData<List<AgencyData>> = agencyDao.readAllAgencies()
    val allFavoriteAgencies: LiveData<List<AgencyData>> = agencyDao.readAllFavoriteAgencies()
    val allFavoriteAgencyIds: LiveData<List<String>> = agencyDao.readAllFavoriteAgencyIds()

    suspend fun updateAgencies(agencies: List<AgencyData>) {
        agencyDao.updateAgencies(agencies)
    }

    suspend fun addFavoriteAgency(agency: FavoriteAgency) {
        agencyDao.addFavoriteAgency(agency)
    }

    suspend fun deleteFavoriteAgency(agency: FavoriteAgency) {
        agencyDao.deleteFavoriteAgency(agency)
    }
}