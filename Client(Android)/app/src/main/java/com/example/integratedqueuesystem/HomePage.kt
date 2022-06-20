package com.example.integratedqueuesystem

import android.os.Bundle
import android.view.LayoutInflater
import android.view.View
import android.view.ViewGroup
import androidx.fragment.app.Fragment
import androidx.fragment.app.activityViewModels
import androidx.viewpager2.adapter.FragmentStateAdapter
import androidx.viewpager2.widget.ViewPager2
import com.example.integratedqueuesystem.databinding.FragmentHomePageBinding
import com.google.android.material.tabs.TabLayoutMediator

class HomePage : Fragment() {

    private var _binding: FragmentHomePageBinding? = null
    private val binding get() = _binding!!
    private lateinit var communicator: Communicator
    //private val userInterfaceViewModel by activityViewModels<UserInterfaceViewModel>()

    override fun onCreateView(
        inflater: LayoutInflater, container: ViewGroup?,
        savedInstanceState: Bundle?
    ): View {
        _binding = FragmentHomePageBinding.inflate(inflater, container, false)
        val view = binding.root

        communicator = activity as Communicator

        val adapter = ViewPagerAdapter(this)
        binding.viewPager.adapter = adapter
        binding.viewPager.setCurrentItem(communicator.getViewPagerPosition(), false)

        TabLayoutMediator(binding.tabLayout, binding.viewPager) { tab, position ->
            tab.text = getString(
                when (position) {
                0 -> R.string.favorite
                1 -> R.string.hospital
                2 -> R.string.school
                3 -> R.string.clinic
                4 -> R.string.post_office
                5 -> R.string.government
                6 -> R.string.restaurant
                7 -> R.string.store
                8 -> R.string.other
                else -> R.string.unknown
            })
        }.attach()

        binding.viewPager.registerOnPageChangeCallback(object: ViewPager2.OnPageChangeCallback() {
            override fun onPageSelected(position: Int) {
                super.onPageSelected(position)
                communicator.setViewPagerPosition(position)
            }
        })

        communicator.getUpdateTimer().observe(viewLifecycleOwner) { timer ->
            binding.updateCounterTv.text = getString(R.string.update_timer, timer)
        }

        /*
        val valueChangedListener = object: ValueEventListener {
            override fun onDataChange(snapshot: DataSnapshot) {
                val data = snapshot.getValue(HashMap::class.java)
                if (data != null) {
                    Log.e("listener", data.toString())
                    Log.e("queue", data.queue.toString())
                    Log.d("id", data.id.toString())
                }
            }

            override fun onCancelled(error: DatabaseError) {
                Log.e("listener", error.toString())
            }
        }
        firebase.addValueEventListener(valueChangedListener)*/

        return view
    }

    class ViewPagerAdapter(fragment: Fragment): FragmentStateAdapter(fragment) {

        override fun getItemCount() = 9

        override fun createFragment(position: Int): Fragment {
            return when (position) {
                0 -> FavoriteAgencyFragment()
                in 1..8 -> AgencyFragment(position)
                else -> Fragment()
            }
        }
    }

    override fun onDestroyView() {
        super.onDestroyView()
        _binding = null
    }
}