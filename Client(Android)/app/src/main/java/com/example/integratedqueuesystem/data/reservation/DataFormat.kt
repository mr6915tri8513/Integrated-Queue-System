package com.example.integratedqueuesystem.data.reservation

import androidx.lifecycle.MutableLiveData

interface DataType {
    companion object {
        const val Unknown = 0
        const val SingleSelection = 1
        const val MultiSelection = 2
        const val Name = 3
        const val Number = 4
        const val Text = 5
        const val Address = 6
        const val Phone = 7
        const val EmailAddress = 8
        const val Password = 9
        const val DateTime = 10
        const val Date = 11
        const val Time = 12
    }
}

class SelectItem (
    val name: String,
    var isChecked: Boolean
)

class DataFormat (
    val dataName: String = "Unknown Data Name",
    val dataType: Int = DataType.Unknown,
    val dataSelectItems: List<String> = emptyList(),
    val dataRequired: Boolean = false
)

class ServiceType (
    val serviceTypeName: String = "Unknown Service Type",
    val dataFormats: List<DataFormat> = emptyList()
)

class ReservationData (
    private val serviceType: Int,
    private val data: Map<String, String>,
    var response: String = "0"
) {
    val state = MutableLiveData(ReservationState.WaitingForServer)
    var pushKey: String? = null
    var message: String? = null

    fun toMap(): Map<String, Any> {
        return mapOf (
            "data" to data,
            "serviceType" to serviceType,
            "response" to response
        )
    }
}

class ResponseData (
    val response: String? = null,
    val message: String? = null
)