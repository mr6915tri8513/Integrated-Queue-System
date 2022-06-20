package com.example.integratedqueuesystem.data.agency

interface AgencyType {
    companion object {
        const val Unknown = 0
        const val Hospital = 1
        const val School = 2
        const val Clinic = 3
        const val PostOffice = 4
        const val Government = 5
        const val Restaurant = 6
        const val Store = 7
        const val Other = 8
    }
}