package com.example.lostandfound.activities;

import androidx.appcompat.app.AppCompatActivity;

import android.content.Intent;
import android.os.Bundle;
import android.view.View;
import android.widget.Button;
import android.widget.EditText;
import android.widget.ImageView;
import android.widget.ProgressBar;
import android.widget.TextView;
import android.widget.Toast;

import com.android.volley.Request;
import com.android.volley.RequestQueue;
import com.android.volley.Response;
import com.android.volley.VolleyError;
import com.android.volley.toolbox.JsonObjectRequest;
import com.example.lostandfound.R;
import com.example.lostandfound.models.Register;
import com.example.lostandfound.settings.Settings;
import com.example.lostandfound.utils.VolleySingleton;
import com.fasterxml.jackson.core.JsonProcessingException;
import com.fasterxml.jackson.databind.ObjectMapper;

import org.json.JSONException;
import org.json.JSONObject;

public class SignupActivity extends AppCompatActivity {

    private ProgressBar progressBar;
    private EditText fullName;
    private EditText phoneNumber;
    private EditText email;
    private EditText password;
    private RequestQueue requestQueue;

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_signup);

        ImageView imageBack = findViewById(R.id.imageBack);
        imageBack.setOnClickListener(v -> onBackPressed());

        TextView loginLink = findViewById(R.id.loginLink);
        loginLink.setOnClickListener(v -> onBackPressed());

        fullName = findViewById(R.id.inputFullname);
        phoneNumber = findViewById(R.id.inputphonenumber);
        email = findViewById(R.id.inputEmail);
        password = findViewById(R.id.inputPassword);

        requestQueue = VolleySingleton.getmInstance(this).getRequestQueue();

        progressBar = findViewById(R.id.signupProgressBar);
        Button signupButton = findViewById(R.id.signupBtn);
        signupButton.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View view) {
                signUp();
            }
        });


    }

    private void signUp() {

        if(fullName.getText().toString().isEmpty() || email.getText().toString().isEmpty() || phoneNumber.getText().toString().isEmpty() || password.getText().toString().isEmpty()) {
            Toast.makeText(this, "All fields are required", Toast.LENGTH_SHORT).show();
        }
        else {
            progressBar.setVisibility(View.VISIBLE);

            //Building request
            Register register = new Register();
            register.setFullName(fullName.getText().toString());
            register.setEmail(email.getText().toString());
            register.setPhoneNumber(phoneNumber.getText().toString());
            register.setPassword(password.getText().toString());

            //Converting request to json string
            ObjectMapper Obj = new ObjectMapper();
            String jsonStr = null;
            try {
                jsonStr = Obj.writeValueAsString(register);
            } catch (JsonProcessingException e) {
                e.printStackTrace();
            }

            //Making API call
            String url = Settings.BASE_URL.concat("Accounts");
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

                            progressBar.setVisibility(View.GONE);
                            try {
                                if (response.getString("code").equals(Settings.SUCCESS_RESPONSE)) {
                                    Toast.makeText(SignupActivity.this, Settings.SIGNUP_SUCCESS_MESSAGE, Toast.LENGTH_LONG).show();
                                    startActivity(new Intent(SignupActivity.this, LoginActivity.class));

                                } else {
                                    Toast.makeText(SignupActivity.this, "Cannot create your account, kindly try again", Toast.LENGTH_LONG).show();
                                }
                            } catch (JSONException e) {
                                e.printStackTrace();
                            }
                        }
                    }, new Response.ErrorListener() {
                @Override
                public void onErrorResponse(VolleyError error) {
                    progressBar.setVisibility(View.GONE);
                    error.printStackTrace();
                    Toast.makeText(SignupActivity.this, "An error occurred", Toast.LENGTH_SHORT).show();
                }
            });

            this.requestQueue.add(jsonObjectRequest);
        }

    }
}