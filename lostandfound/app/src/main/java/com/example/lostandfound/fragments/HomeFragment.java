package com.example.lostandfound.fragments;

import static androidx.constraintlayout.helper.widget.MotionEffect.TAG;

import android.content.Context;
import android.os.Bundle;

import androidx.fragment.app.Fragment;
import androidx.recyclerview.widget.LinearLayoutManager;
import androidx.recyclerview.widget.RecyclerView;

import android.text.Editable;
import android.text.TextWatcher;
import android.util.Log;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;
import android.widget.EditText;
import android.widget.ProgressBar;
import android.widget.TextView;

import com.android.volley.Request;
import com.android.volley.RequestQueue;
import com.android.volley.Response;
import com.android.volley.VolleyError;
import com.android.volley.toolbox.JsonObjectRequest;
import com.example.lostandfound.R;
import com.example.lostandfound.adapters.ItemAdapter;
import com.example.lostandfound.models.Item;
import com.example.lostandfound.settings.Settings;
import com.example.lostandfound.utils.VolleySingleton;

import org.json.JSONArray;
import org.json.JSONException;
import org.json.JSONObject;

import java.util.ArrayList;
import java.util.List;

public class HomeFragment extends Fragment {
    private ProgressBar loader;
    private Context context;
    private RequestQueue requestQueue;
    private List<Item> itemList;
    private RecyclerView recyclerView;
    private ItemAdapter itemAdapter;
    private EditText search;

    @Override
    public View onCreateView(LayoutInflater inflater, ViewGroup container,
                             Bundle savedInstanceState) {
        // Inflate the layout for this fragment
        View view = inflater.inflate(R.layout.fragment_home, container, false);
        TextView textView = (TextView) getActivity().findViewById(R.id.textMyRestaurants);
        textView.setText("Items");
        loader = (ProgressBar) view.findViewById(R.id.loader);
        recyclerView = (RecyclerView) view.findViewById(R.id.itemsRecyclerView);
        search = (EditText) view.findViewById(R.id.inputSearch);
        context = view.getContext();
        return view;
    }

    @Override
    public void onViewCreated(View view, Bundle savedInstanceState) {
        super.onViewCreated(view, savedInstanceState);
        recyclerView.setHasFixedSize(true);
        recyclerView.setLayoutManager(new LinearLayoutManager(context));

        requestQueue = VolleySingleton.getmInstance(context).getRequestQueue();

        search.addTextChangedListener(new TextWatcher() {
            @Override
            public void beforeTextChanged(CharSequence s, int start, int count, int after) {

            }

            @Override
            public void onTextChanged(CharSequence s, int start, int before, int count) {
                itemAdapter.cancelTimer();
            }

            @Override
            public void afterTextChanged(Editable s) {
                if (itemList.size() != 0) {
                    itemAdapter.searchItems(s.toString());
                }
            }
        });

        itemList = new ArrayList<>();
        fetchItems();
    }

    private void fetchItems() {

        loader.setVisibility(View.VISIBLE);

        String url = Settings.BASE_URL.concat("items");

        Log.d(TAG, "URL: "+ url);

        JsonObjectRequest jsonObjectRequest = new JsonObjectRequest(Request.Method.GET, url, null,
                new Response.Listener<JSONObject>() {
                    @Override
                    public void onResponse(JSONObject response) {
                        loader.setVisibility(View.GONE);
                        try {
                            if(response.getString("code").equals(Settings.SUCCESS_RESPONSE)) {
                                JSONArray data = response.getJSONArray("data");
                                for (int i = 0; i < data.length(); i++) {
                                    try {
                                        JSONObject jsonObject = data.getJSONObject(i);
                                        String name = jsonObject.getString("name");
                                        String color = jsonObject.getString("color");
                                        String description = jsonObject.getString("description");
                                        String lostLocationAddress = jsonObject.getString("lostLocationAddress");
                                        String retrievalLocationAddress = jsonObject.getString("retrievalLocationAddress");
                                        String note = jsonObject.getString("note");
                                        String pictureUrl = jsonObject.getString("pictureUrl");
                                        int id = jsonObject.getInt("id");
                                        boolean isActive = jsonObject.getBoolean("isActive");
                                        String contact = jsonObject.getString("contact");
                                        String createdAt = jsonObject.getString("createdAt");
                                        int reportedByUserId = jsonObject.getInt("reportedByUserId");

                                        Item item = new Item(id, name, color, description, lostLocationAddress, retrievalLocationAddress, note, pictureUrl, contact, createdAt, isActive, reportedByUserId);
                                        itemList.add(item);
                                    } catch (JSONException e) {
                                        e.printStackTrace();
                                    }
                                }
                            }
                        } catch (JSONException e) {
                            e.printStackTrace();
                        }
                    }
                }, new Response.ErrorListener() {
            @Override
            public void onErrorResponse(VolleyError error) {
                loader.setVisibility(View.GONE);
                error.printStackTrace();
                //Toast.makeText(context, error.getMessage(), Toast.LENGTH_SHORT).show();
            }
        });                                    itemAdapter = new ItemAdapter(context, itemList, false);

        recyclerView.setAdapter(itemAdapter);
        //Put the request in volley queue
        requestQueue.add(jsonObjectRequest);
    }
}