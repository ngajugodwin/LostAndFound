package com.example.lostandfound.fragments;

import static android.content.Context.MODE_PRIVATE;

import android.content.DialogInterface;
import android.content.Intent;
import android.content.SharedPreferences;
import android.os.Build;
import android.os.Bundle;

import androidx.annotation.RequiresApi;
import androidx.appcompat.app.AlertDialog;
import androidx.fragment.app.Fragment;

import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;
import android.widget.Button;
import android.widget.ProgressBar;
import android.widget.Switch;
import android.widget.TextView;
import android.widget.Toast;

import com.android.volley.Request;
import com.android.volley.RequestQueue;
import com.android.volley.Response;
import com.android.volley.VolleyError;
import com.android.volley.toolbox.JsonObjectRequest;
import com.example.lostandfound.R;
import com.example.lostandfound.activities.LoginActivity;
import com.example.lostandfound.models.SettingsRequest;
import com.example.lostandfound.settings.Settings;
import com.example.lostandfound.utils.VolleySingleton;
import com.fasterxml.jackson.core.JsonProcessingException;
import com.fasterxml.jackson.databind.ObjectMapper;

import org.json.JSONException;
import org.json.JSONObject;

public class ProfileFragment extends Fragment {

    private TextView name, email, phoneNumber;
    private Button signout;
    private Switch switchAccount;
    private RequestQueue requestQueue;
    private ProgressBar loader;
    private int userId;

    @Override
    public View onCreateView(LayoutInflater inflater, ViewGroup container,
                             Bundle savedInstanceState) {
        // Inflate the layout for this fragment
        View view = inflater.inflate(R.layout.fragment_profile, container, false);
        TextView textView = (TextView) getActivity().findViewById(R.id.textMyRestaurants);
        textView.setText("Profile");
        name = (TextView) view.findViewById(R.id.name);
        email = (TextView) view.findViewById(R.id.email);
        phoneNumber = (TextView) view.findViewById(R.id.phoneNumber);
        signout = (Button) view.findViewById(R.id.signOutBtn);
        switchAccount = (Switch) view.findViewById(R.id.switchAccount);
        loader = (ProgressBar) view.findViewById(R.id.actionLoader);
        return view;
    }

    @Override
    public void onViewCreated(View view, Bundle savedInstanceState) {
        super.onViewCreated(view, savedInstanceState);

        SharedPreferences preferences = this.getActivity().getSharedPreferences(Settings.APP_PREFERENCE_NAME, MODE_PRIVATE);
        String full_name = preferences.getString("full_name", "Dummy User");
        userId = preferences.getInt("user_id", 0);
        String email_pref = preferences.getString("email", "dummy@email.com");
        String phone = preferences.getString("phoneNumber", "07764873673");

        requestQueue = VolleySingleton.getmInstance(getContext()).getRequestQueue();

        name.setText(full_name);
        email.setText(email_pref);
        phoneNumber.setText(phone);

        signout.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View view) {
                doLogout();
            }
        });

        switchAccount.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View view) {
                updateStatus();
            }
        });

        fetchUserStatusSettings();

    }

    private void fetchUserStatusSettings() {
        loader.setVisibility(View.VISIBLE);

        //Making API call
        String url = Settings.BASE_URL.concat("UserNotificationSettings/getUserSettings/"+userId);

        JsonObjectRequest jsonObjectRequest = new JsonObjectRequest(Request.Method.GET, url, null,
                new Response.Listener<JSONObject>() {
                    @Override
                    public void onResponse(JSONObject response) {

                        loader.setVisibility(View.GONE);
                        try {
                            if (response.getString("code").equals(Settings.SUCCESS_RESPONSE)) {
                                JSONObject data = response.getJSONObject("data");
                                boolean checked = data.getBoolean("isNotificationActive");
                                switchAccount.setChecked(checked);
                            } else {
                                Toast.makeText(getContext(), "Failed to fetch settings", Toast.LENGTH_SHORT).show();
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
                Toast.makeText(getContext(), "Failed to fetch settings", Toast.LENGTH_SHORT).show();
            }
        });

        this.requestQueue.add(jsonObjectRequest);
    }

    private void updateStatus() {
        loader.setVisibility(View.VISIBLE);
        SettingsRequest request = new SettingsRequest();
        request.setUserId(userId);
        request.setStatus(switchAccount.isChecked());

        //Converting request to json string
        ObjectMapper Obj = new ObjectMapper();
        String jsonStr = null;
        try {
            jsonStr = Obj.writeValueAsString(request);
        } catch (JsonProcessingException e) {
            e.printStackTrace();
        }

        //Making API call
        String url = Settings.BASE_URL.concat("UserNotificationSettings/activiateOrDisableNotification");
        JSONObject jsonBody = null;
        try {
            jsonBody = new JSONObject(jsonStr);
        } catch (JSONException e) {
            e.printStackTrace();
        }

        JsonObjectRequest jsonObjectRequest = new JsonObjectRequest(Request.Method.POST, url, jsonBody,
                new Response.Listener<JSONObject>() {
                    @Override
                    public void onResponse(JSONObject response) {

                        loader.setVisibility(View.GONE);
                        try {
                            if (response.getString("code").equals(Settings.SUCCESS_RESPONSE)) {
                                Toast.makeText(getContext(), "Settings saved successfully", Toast.LENGTH_SHORT).show();
                            } else {
                                Toast.makeText(getContext(), "Failed to save settings", Toast.LENGTH_SHORT).show();
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
                Toast.makeText(getContext(), "Failed to save settings", Toast.LENGTH_SHORT).show();
            }
        });

        this.requestQueue.add(jsonObjectRequest);
    }

    private void doLogout() {
        String msg = "Are you sure you want to Logout?";
        new AlertDialog.Builder(getContext())
                .setTitle("Logging Out")
                .setMessage(msg)
                .setPositiveButton("Yes", new DialogInterface.OnClickListener() {
                    @RequiresApi(api = Build.VERSION_CODES.LOLLIPOP)
                    @Override
                    public void onClick(DialogInterface dialog, int which) {
                        SharedPreferences preferences = getContext().getSharedPreferences(Settings.APP_PREFERENCE_NAME, MODE_PRIVATE);
                        preferences.edit().clear().commit();
                        startActivity(new Intent(getContext(), LoginActivity.class));
                    }
                })
                .setNegativeButton("No", null)
                .show();
    }
}