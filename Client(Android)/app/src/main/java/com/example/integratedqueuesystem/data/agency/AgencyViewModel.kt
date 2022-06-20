package com.example.integratedqueuesystem.data.agency

import android.app.Application
import androidx.lifecycle.AndroidViewModel
import androidx.lifecycle.LiveData
import androidx.lifecycle.viewModelScope
import kotlinx.coroutines.Dispatchers
import kotlinx.coroutines.launch

class AgencyViewModel(application: Application): AndroidViewModel(application) {

    val allAgencies: LiveData<List<AgencyData>>
    val allFavoriteAgencies: LiveData<List<AgencyData>>
    val allFavoriteAgencyIds: LiveData<List<String>>
    private val repository: AgencyRepository

    init {
        val agencyDao = AgencyDatabase.getDatabase(application).agencyDao()
        repository = AgencyRepository(agencyDao)
        allAgencies = repository.allAgencies
        allFavoriteAgencies = repository.allFavoriteAgencies
        allFavoriteAgencyIds = repository.allFavoriteAgencyIds
    }

    fun updateAgencies(agencies: List<AgencyData>) {
        viewModelScope.launch(Dispatchers.IO) {
            repository.updateAgencies(agencies)
        }
    }

    fun addFavoriteAgency(agency: FavoriteAgency) {
        viewModelScope.launch(Dispatchers.IO) {
            repository.addFavoriteAgency(agency)
        }
    }

    fun deleteFavoriteAgency(agency: FavoriteAgency) {
        viewModelScope.launch(Dispatchers.IO) {
            repository.deleteFavoriteAgency(agency)
        }
    }
}