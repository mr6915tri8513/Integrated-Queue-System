package com.example.integratedqueuesystem.data.agency

import androidx.room.Entity
import androidx.room.PrimaryKey
import com.example.integratedqueuesystem.data.reservation.ServiceType

@Entity(tableName = "agency_table")
data class AgencyData (
    @PrimaryKey
    val id: String,
    val name: String,
    val type: Int,
    val address: String,
    val latitude: Double,
    val longitude: Double,
) {
    constructor(agency: Agency) : this(
        agency.id,
        agency.name,
        agency.type,
        agency.address,
        agency.latitude,
        agency.longitude
    )
}

class Agency (
    val id: String = "00000",
    val name: String = "UnknownName",
    val type: Int = AgencyType.Unknown,
    val address: String = "UnknownAddress",
    val latitude: Double = 0.0,
    val longitude: Double = 0.0,
    //val next_num: List<String> = emptyList(),
    val pending: List<Int> = emptyList(),//null?
    val status: Int = 0,
    //val queue: HashMap<String, ReservationData> = HashMap(),
    val dataFormat: List<ServiceType> = emptyList()
) {
    constructor(agencyData: AgencyData): this(
        agencyData.id,
        agencyData.name,
        agencyData.type,
        agencyData.address,
        agencyData.latitude,
        agencyData.longitude,
    )
}

@Entity(tableName = "favorite_agency_table")
data class FavoriteAgency (
    @PrimaryKey
    val agencyId: String,
)