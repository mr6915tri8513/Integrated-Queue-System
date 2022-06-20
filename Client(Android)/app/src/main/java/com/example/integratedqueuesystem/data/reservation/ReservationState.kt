package com.example.integratedqueuesystem.data.reservation

interface ReservationState {
    companion object {
        const val WaitingForServer = 0
        const val ReservationCanceled = 1
        const val ReservationTimeout = 2
        const val KeyIsNull = 3
        const val ReservationFailed = 4
        const val ReservationDenied = 5
        const val ReservationSuccess = 6
    }
}