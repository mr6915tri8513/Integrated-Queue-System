package com.example.integratedqueuesystem

import android.os.Bundle
import androidx.fragment.app.Fragment
import android.view.LayoutInflater
import android.view.View
import android.view.ViewGroup
import androidx.viewpager2.adapter.FragmentStateAdapter
import androidx.viewpager2.widget.ViewPager2
import com.example.integratedqueuesystem.databinding.FragmentReservationPageBinding
import com.google.android.material.tabs.TabLayoutMediator

class ReservationPage : Fragment() {

    private var _binding: FragmentReservationPageBinding? = null
    private val binding get() = _binding!!
    private lateinit var communicator: Communicator

    override fun onCreateView(
        inflater: LayoutInflater, container: ViewGroup?,
        savedInstanceState: Bundle?
    ): View {
        _binding = FragmentReservationPageBinding.inflate(inflater, container, false)
        val view = binding.root

        communicator = activity as Communicator

        val adapter = ViewPagerAdapter(this)
        binding.viewPager.adapter = adapter
        binding.viewPager.setCurrentItem(communicator.getReservationPosition(), false)

        TabLayoutMediator(binding.tabLayout, binding.viewPager) { tab, position ->
            tab.text = getString(
                when (position) {
                    0 -> R.string.reserving
                    1 -> R.string.my_reservation
                    else -> R.string.unknown
                }
            )
        }.attach()

        binding.viewPager.registerOnPageChangeCallback(object: ViewPager2.OnPageChangeCallback() {
            override fun onPageSelected(position: Int) {
                super.onPageSelected(position)
                communicator.setReservationPosition(position)
            }
        })

        return view
    }

    class ViewPagerAdapter(fragment: Fragment): FragmentStateAdapter(fragment) {

        override fun getItemCount() = 2

        override fun createFragment(position: Int): Fragment {
            return when (position) {
                0 -> ReservingPage()
                1 -> ReservationRecordPage()
                else -> Fragment()
            }
        }
    }

    override fun onDestroyView() {
        super.onDestroyView()
        _binding = null
    }
}