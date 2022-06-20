package com.example.integratedqueuesystem.data.agency

interface AgencyStatus {
    companion object {
        const val Unknown = 0       //未知
        const val NotSupport = 1    //停止支援
        const val Closed = 2        //已打烊
        const val Open = 3          //營業中
        const val Paused = 4        //暫停服務
        const val Serving = 5       //服務中
    }
}