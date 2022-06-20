package com.example.integratedqueuesystem

import android.os.Bundle
import android.os.Handler
import android.os.Looper
import android.text.Editable
import android.text.InputType
import android.text.TextWatcher
import android.util.Log
import androidx.fragment.app.Fragment
import android.view.LayoutInflater
import android.view.View
import android.view.ViewGroup
import android.widget.*
import androidx.core.content.ContextCompat.getColor
import androidx.lifecycle.ViewModelProvider
import androidx.recyclerview.widget.LinearLayoutManager
import androidx.recyclerview.widget.RecyclerView
import com.example.integratedqueuesystem.data.agency.AgencyStatus
import com.example.integratedqueuesystem.data.reservation.*
import com.example.integratedqueuesystem.databinding.FragmentReservingPageBinding
import com.google.android.material.snackbar.Snackbar
import com.google.android.material.textfield.TextInputLayout
import java.time.OffsetDateTime
import java.util.*

class ReservingPage : Fragment() {

    private var _binding: FragmentReservingPageBinding? = null
    private val binding get() = _binding!!
    private lateinit var communicator: Communicator
    private lateinit var reservationViewModel: ReservationViewModel
    private lateinit var reservationRecordViewModel: ReservationRecordViewModel

    private val adapter = ReservationDataAdapter()

    override fun onCreateView(
        inflater: LayoutInflater, container: ViewGroup?,
        savedInstanceState: Bundle?
    ): View {
        _binding = FragmentReservingPageBinding.inflate(inflater, container, false)
        val view = binding.root

        communicator = activity as Communicator
        reservationViewModel = ViewModelProvider(requireActivity())[ReservationViewModel::class.java]
        reservationRecordViewModel = ViewModelProvider(requireActivity())[ReservationRecordViewModel::class.java]

        fun noAgency() {
            binding.noAgencyTv.visibility = View.VISIBLE
            binding.agencyNameTv.visibility = View.GONE
            binding.agencyStatusTv.visibility = View.GONE
            binding.updateCounterTv.visibility = View.GONE
            binding.agencyPendingTv.visibility = View.GONE
            binding.serviceTypeDm.visibility = View.GONE
            binding.reservationDataRv.visibility = View.GONE
            binding.reservationBtn.visibility = View.GONE
        }

        fun showSnackBar(message: String) {
            Snackbar.make(
                requireView(),
                message,
                Snackbar.LENGTH_LONG
            ).apply {
                setAction(getString(R.string.confirm)) {
                    dismiss()
                }
                show()
            }
        }

        var isDisplay = false
        val agencyId = reservationViewModel.agencyId

        if (agencyId.isNotEmpty()) {
            communicator.getAgencyStatus().observe(viewLifecycleOwner) { agencies ->

                val agency = if (agencies?.get(agencyId) != null) agencies[agencyId] else null
                if (agency != null) {
                    val position = reservationViewModel.serviceType
                    if (!isDisplay) {
                        isDisplay = true

                        //agencyName
                        binding.agencyNameTv.text =
                            agency.name.ifEmpty { getString(R.string.agency_name_default) }

                        //reservationDataList
                        binding.reservationDataRv.adapter = adapter
                        binding.reservationDataRv.layoutManager = LinearLayoutManager(requireContext())//TODO
                        if (agency.dataFormat.isNotEmpty() && agency.status == AgencyStatus.Serving) {
                            adapter.setDataFormats(
                                agency.dataFormat[position].dataFormats,
                                reservationViewModel.reservationData
                            )

                            //serviceTypeSelector
                            val serviceTypeNameList: List<String> =
                                if (agency.dataFormat.isEmpty()) emptyList()
                                else agency.dataFormat.map { serviceType ->
                                    serviceType.serviceTypeName
                                }
                            val arrayAdapter = ArrayAdapter(
                                requireContext(),
                                R.layout.dropdown_item,
                                serviceTypeNameList
                            )
                            binding.serviceTypeDm.isEnabled = true
                            binding.serviceTypeTv.isEnabled = true
                            binding.serviceTypeTv.setText(agency.dataFormat[position].serviceTypeName)
                            binding.serviceTypeTv.setAdapter(arrayAdapter)
                            binding.serviceTypeTv.setOnItemClickListener { adapterView, v, pos, l ->
                                reservationViewModel.serviceType = pos
                                binding.agencyPendingTv.text =
                                    getString(R.string.agency_pending, agency.pending[pos])
                                adapter.setDataFormats(pos, agency.dataFormat[pos].dataFormats)
                            }

                            //reservationButton
                            binding.reservationBtn.isEnabled = true
                            binding.reservationBtn.setOnClickListener {
                                if (adapter.checkData()) {
                                    if (communicator.isOnline()) {
                                        val reservationData = ReservationData(
                                            reservationViewModel.serviceType,
                                            adapter.getDataMap()
                                        )
                                        reservationRecordViewModel.addRecord(
                                            ReservationRecord(
                                                agency.id,
                                                agency.name,
                                                reservationViewModel.serviceType,
                                                agency.dataFormat[reservationViewModel.serviceType].serviceTypeName,
                                                OffsetDateTime.now(),
                                                ReservationState.WaitingForServer
                                            )
                                        ) { reservationId ->
                                            val dialog = LoadingDialogFragment(
                                                getString(R.string.wait_for_server),
                                                reservationData.state
                                            )
                                            dialog.show(parentFragmentManager, "wait_for_server")

                                            reservationData.state.observe(
                                                viewLifecycleOwner
                                            ) { state ->
                                                when (state) {
                                                    ReservationState.WaitingForServer -> {}
                                                    ReservationState.ReservationCanceled -> {
                                                        reservationRecordViewModel.updateState(
                                                            reservationId,
                                                            ReservationState.ReservationCanceled
                                                        )
                                                        dialog.dismiss()
                                                        showSnackBar(getString(R.string.reservation_canceled))
                                                    }
                                                    ReservationState.ReservationTimeout -> {
                                                        reservationRecordViewModel.updateState(
                                                            reservationId,
                                                            ReservationState.ReservationTimeout
                                                        )
                                                        dialog.dismiss()
                                                        showSnackBar(getString(R.string.reservation_timeout))
                                                    }
                                                    ReservationState.KeyIsNull -> {
                                                        reservationRecordViewModel.updateState(
                                                            reservationId,
                                                            ReservationState.KeyIsNull
                                                        )
                                                        dialog.dismiss()
                                                        showSnackBar(getString(R.string.unknown_error))
                                                    }
                                                    ReservationState.ReservationDenied -> {
                                                        reservationRecordViewModel.updateState(
                                                            reservationId,
                                                            ReservationState.ReservationDenied
                                                        )
                                                        dialog.dismiss()
                                                        if (reservationData.message != null) {
                                                            showSnackBar(
                                                                getString(
                                                                    R.string.reservation_denied_with_message,
                                                                    reservationData.message
                                                                )
                                                            )
                                                        } else {
                                                            showSnackBar(getString(R.string.reservation_denied))
                                                        }
                                                    }
                                                    ReservationState.ReservationSuccess -> {
                                                        reservationRecordViewModel.updateState(
                                                            reservationId,
                                                            ReservationState.ReservationSuccess,
                                                            reservationData.pushKey ?: "",
                                                            reservationData.response
                                                        )
                                                        adapter.clearData()
                                                        dialog.dismiss()
                                                        showSnackBar(
                                                            getString(
                                                                R.string.reservation_success,
                                                                reservationData.response
                                                            )
                                                        )
                                                        communicator.setReminder(
                                                            agency.id,
                                                            reservationViewModel.serviceType,
                                                            agency.dataFormat[reservationViewModel.serviceType].serviceTypeName,
                                                            reservationData.response
                                                        )
                                                    }
                                                    else -> {}
                                                }
                                            }

                                            Handler(Looper.getMainLooper()).postDelayed({
                                                if (reservationData.state.value == ReservationState.WaitingForServer) {
                                                    reservationData.state.value =
                                                        ReservationState.ReservationTimeout
                                                }
                                            }, 8000)
                                            communicator.sendReservation(
                                                agencyId,
                                                reservationData
                                            )
                                        }
                                    } else {
                                        showSnackBar(getString(R.string.please_check_network_state))
                                    }
                                } else {
                                    showSnackBar(getString(R.string.please_check_data))
                                }
                            }
                        }
                    }

                    //agencyStatus
                    binding.agencyStatusTv.text = getString(
                        when (agency.status) {
                            0 -> R.string.unknown
                            1 -> R.string.not_support
                            2 -> R.string.closed
                            3 -> R.string.open
                            4 -> R.string.paused
                            5 -> R.string.serving
                            else -> R.string.unknown
                        }
                    )
                    binding.agencyStatusTv.setTextColor(
                        getColor(
                            requireContext(),
                            when (agency.status) {
                                1, 2, 4 -> R.color.holo_red
                                3, 5 -> R.color.holo_green
                                else -> R.color.default_text_color
                            }
                        )
                    )

                    //pending
                    binding.agencyPendingTv.text =
                        getString(R.string.agency_pending, agency.pending[position])

                } else {
                    noAgency()
                }
            }

            communicator.getUpdateTimer().observe(viewLifecycleOwner) { timer ->
                binding.updateCounterTv.text = getString(R.string.update_timer, timer)
            }

        } else {
            Log.e("reservationId", "is empty")
            noAgency()
        }
        return view
    }

    override fun onDestroyView() {
        Log.e("reserving page", "view destroy")
        reservationViewModel.reservationData = adapter.getDataList()
        super.onDestroyView()
    }

    class ReservationDataAdapter: RecyclerView.Adapter<RecyclerView.ViewHolder>() {

        private var serviceType = 0
        private var dataFormatList = emptyList<DataFormat>()
        private var dataList = mutableListOf<String>()
        private var strict = false

        class ReservationDataViewHolder(itemView: View): RecyclerView.ViewHolder(itemView) {
            val layout: TextInputLayout = itemView.findViewById(R.id.reservation_data_tl)
            val editText: EditText = itemView.findViewById(R.id.reservation_data_te)
        }

        inner class ReservationDataSingleSelectionViewHolder(itemView: View): RecyclerView.ViewHolder(itemView) {

            val selectItemNameTv: TextView = itemView.findViewById(R.id.select_item_name_tv)
            val selectItemListRv: RecyclerView = itemView.findViewById(R.id.select_item_list_rv)

            inner class SelectItemAdapter(private val itemList: List<String>, private val pos: Int): RecyclerView.Adapter<SelectItemAdapter.SelectItemViewHolder>() {

                private var selectedIndex: Int? = if (dataList[pos].isEmpty()) null else dataList[pos].toInt()

                inner class SelectItemViewHolder(itemView: View): RecyclerView.ViewHolder(itemView) {
                    val item: RadioButton = itemView.findViewById(R.id.select_item_btn)
                }

                override fun onCreateViewHolder(
                    parent: ViewGroup,
                    viewType: Int
                ): SelectItemViewHolder {
                    return SelectItemViewHolder(
                        LayoutInflater.from(parent.context).inflate(R.layout.reservation_data_single_selection_item_row, parent, false)
                    )
                }

                override fun getItemCount(): Int {
                    return itemList.count()
                }

                override fun onBindViewHolder(holder: SelectItemViewHolder, position: Int) {
                    val currentItem = itemList[position]
                    holder.item.text = currentItem
                    holder.item.isChecked = position == selectedIndex
                    holder.item.setOnClickListener {
                        if (selectedIndex != holder.adapterPosition) {
                            val tmp = selectedIndex
                            selectedIndex = holder.adapterPosition
                            dataList[pos] = holder.adapterPosition.toString()
                            tmp?.let { notifyItemChanged(it) }
                        }
                    }
                }
            }
        }

        inner class ReservationDataMultiSelectionViewHolder(itemView: View): RecyclerView.ViewHolder(itemView) {

            val selectItemNameTv: TextView = itemView.findViewById(R.id.select_item_name_tv)
            val selectItemListRv: RecyclerView = itemView.findViewById(R.id.select_item_list_rv)

            inner class SelectItemAdapter(private val itemList: List<String>, private val pos: Int): RecyclerView.Adapter<SelectItemAdapter.SelectItemViewHolder>() {

                private val selectedList: MutableList<Boolean> =
                    if (dataList[pos].isBlank()) MutableList(itemList.count()) { false } else dataList[pos].split(
                        ','
                    ).map { it.toBoolean() }.toMutableList()

                inner class SelectItemViewHolder(itemView: View): RecyclerView.ViewHolder(itemView) {
                    val item: CheckBox = itemView.findViewById(R.id.select_item_btn)
                }

                override fun onCreateViewHolder(
                    parent: ViewGroup,
                    viewType: Int
                ): SelectItemViewHolder {
                    return SelectItemViewHolder(
                        LayoutInflater.from(parent.context).inflate(R.layout.reservation_data_multi_selection_item_row, parent, false)
                    )
                }

                override fun getItemCount(): Int {
                    return itemList.count()
                }

                override fun onBindViewHolder(holder: SelectItemViewHolder, position: Int) {
                    val currentItem = itemList[position]
                    holder.item.text = currentItem
                    holder.item.isChecked = selectedList[position]
                    holder.item.setOnClickListener {
                        selectedList[holder.adapterPosition] = !selectedList[holder.adapterPosition]
                        dataList[pos] = selectedList.joinToString(",")
                    }
                }
            }
        }

        override fun getItemViewType(position: Int): Int {
            return when (dataFormatList[position].dataType) {
                DataType.SingleSelection -> 0
                DataType.MultiSelection -> 1
                else -> 2
            }
        }

        override fun onCreateViewHolder(parent: ViewGroup, viewType: Int): RecyclerView.ViewHolder {
            return when (viewType) {
                0 -> ReservationDataSingleSelectionViewHolder(
                    LayoutInflater.from(parent.context).inflate(R.layout.reservation_data_selection_row, parent, false)
                )
                1 -> ReservationDataMultiSelectionViewHolder(
                    LayoutInflater.from(parent.context).inflate(R.layout.reservation_data_selection_row, parent, false)
                )
                else -> ReservationDataViewHolder(
                    LayoutInflater.from(parent.context)
                        .inflate(R.layout.reservation_data_row, parent, false)
                )
            }
        }

        override fun getItemCount(): Int {
            return dataFormatList.count()
        }

        override fun onBindViewHolder(holder: RecyclerView.ViewHolder, position: Int) {
            when (dataFormatList[position].dataType) {
                DataType.SingleSelection -> {
                    val currentItem = dataFormatList[position]
                    val holder1 = holder as ReservationDataSingleSelectionViewHolder

                    holder1.selectItemNameTv.text = currentItem.dataName

                    val adapter = holder1.SelectItemAdapter(currentItem.dataSelectItems, position)
                    holder1.selectItemListRv.adapter = adapter
                    holder1.selectItemListRv.layoutManager = LinearLayoutManager(holder1.itemView.context)
                }
                DataType.MultiSelection -> {
                    val currentItem = dataFormatList[position]
                    val holder1 = holder as ReservationDataMultiSelectionViewHolder

                    holder1.selectItemNameTv.text = currentItem.dataName

                    val adapter = holder1.SelectItemAdapter(currentItem.dataSelectItems, position)
                    holder1.selectItemListRv.adapter = adapter
                    holder1.selectItemListRv.layoutManager = LinearLayoutManager(holder1.itemView.context)
                }
                else -> {
                    val holder1 = holder as ReservationDataViewHolder
                    val currentItem = dataFormatList[holder.adapterPosition]
                    //Log.e(currentItem.first, currentItem.second.toString())

                    holder1.layout.hint = currentItem.dataName

                    holder1.editText.inputType = when (currentItem.dataType) {
                        DataType.Unknown -> InputType.TYPE_CLASS_TEXT
                        DataType.Name -> InputType.TYPE_TEXT_VARIATION_PERSON_NAME
                        DataType.Number -> InputType.TYPE_CLASS_NUMBER
                        DataType.Text -> InputType.TYPE_TEXT_VARIATION_SHORT_MESSAGE
                        DataType.Address -> InputType.TYPE_TEXT_VARIATION_POSTAL_ADDRESS
                        DataType.Phone -> InputType.TYPE_CLASS_PHONE
                        DataType.EmailAddress -> InputType.TYPE_TEXT_VARIATION_EMAIL_ADDRESS
                        DataType.Password -> InputType.TYPE_TEXT_VARIATION_PASSWORD
                        DataType.DateTime -> InputType.TYPE_CLASS_DATETIME
                        DataType.Date -> InputType.TYPE_DATETIME_VARIATION_DATE
                        DataType.Time -> InputType.TYPE_DATETIME_VARIATION_TIME
                        else -> InputType.TYPE_NULL
                    }

                    val textWatcher = object : TextWatcher {
                        override fun beforeTextChanged(s: CharSequence, start: Int, count: Int, after: Int) {}

                        override fun onTextChanged(s: CharSequence, start: Int, before: Int, count: Int) {}
                        override fun afterTextChanged(s: Editable) {
                            dataList[holder1.adapterPosition] = s.toString()
                            holder1.editText.error = if (s.toString().isNotBlank()) null
                            else holder1.itemView.context.getString(R.string.data_required)
                        }
                    }
                    holder1.editText.error =
                        if (strict && dataList[holder1.adapterPosition].isBlank()) holder1.itemView.context.getString(
                            R.string.data_required
                        )
                        else null
                    holder1.editText.setText(dataList[holder1.adapterPosition])
                    holder1.editText.setOnFocusChangeListener { view, focus ->
                        if (focus) {
                            holder1.editText.addTextChangedListener(textWatcher)
                        } else {
                            holder1.editText.removeTextChangedListener(textWatcher)
                        }
                    }
                }
            }
        }

        fun setDataFormats(serviceType: Int, dataFormats: List<DataFormat>) {
            if (this.serviceType != serviceType) {
                strict = false
                this.serviceType = serviceType
                dataFormatList = dataFormats
                dataList = MutableList(dataFormats.count()) { "" }
                notifyDataSetChanged()
            }
        }

        fun setDataFormats(dataFormats: List<DataFormat>, data: List<String>){
            serviceType = 0
            dataFormatList = dataFormats
            dataList = data.toMutableList()
            notifyItemRangeChanged(0, data.count())
        }

        fun checkData(): Boolean {

            fun checkData(data: String, dataType: Int): Boolean {
                return when (dataType) {
                    DataType.SingleSelection -> data.isNotBlank()
                    DataType.MultiSelection -> data.contains("true")
                    else -> data.isNotBlank()
                }
            }

            strict = true
            var pass  = true
            for (i in dataFormatList.indices) {
                if (dataFormatList[i].dataRequired && !checkData(dataList[i], dataFormatList[i].dataType)) {
                    notifyItemChanged(i)
                    pass = false
                }
            }
            return pass
        }

        fun getDataList(): List<String> {
            return dataList
        }

        fun getDataMap(): Map<String, String> {
            val data = mutableMapOf<String, String>()
            for (i in dataFormatList.indices) {
                data[dataFormatList[i].dataName] = dataList[i]
            }
            return data
        }

        fun clearData() {
            strict = false
            dataList = MutableList(dataFormatList.count()) { "" }
            notifyItemRangeChanged(0, dataFormatList.count())
        }
    }
}