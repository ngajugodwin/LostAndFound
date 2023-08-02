package com.example.lostandfound.activities;

import androidx.annotation.RequiresApi;
import androidx.appcompat.app.AlertDialog;
import androidx.appcompat.app.AppCompatActivity;

import android.content.DialogInterface;
import android.content.Intent;
import android.content.SharedPreferences;
import android.os.Build;
import android.os.Bundle;
import android.view.View;
import android.widget.Button;
import android.widget.EditText;
import android.widget.ProgressBar;
import android.widget.TextView;
import android.widget.Toast;

import com.android.volley.Request;
import com.android.volley.RequestQueue;
import com.android.volley.Response;
import com.android.volley.VolleyError;
import com.android.volley.toolbox.JsonObjectRequest;
import com.example.lostandfound.MainActivity;
import com.example.lostandfound.R;
import com.example.lostandfound.models.Login;
import com.example.lostandfound.settings.Settings;
import com.example.lostandfound.utils.VolleySingleton;
import com.fasterxml.jackson.core.JsonProcessingException;
import com.fasterxml.jackson.databind.ObjectMapper;

import org.json.JSONException;
import org.json.JSONObject;

public class LoginActivity extends AppCompatActivity {

    private Button loginButton;
    private ProgressBar loginProgressBar;
    private EditText email;
    private EditText password;
    private TextView createAccount;
    private RequestQueue requestQueue;

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_login);

        loginButton = findViewById(R.id.loginBtn);
        loginProgressBar = findViewById(R.id.loginProgressBar);
        email = findViewById(R.id.inputEmail);
        password = findViewById(R.id.inputPassword);
        createAccount = findViewById(R.id.createAccountLink);

        requestQueue = VolleySingleton.getmInstance(this).getRequestQueue();

        loginButton.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View view) {
                login();
            }
        });

        createAccount.setOnClickListener(new View.OnClickListener() {
            public void onClick(View v) {
                startActivity(new Intent(LoginActivity.this, SignupActivity.class));
            }
        });
    }

    private void login() {
        if(email.getText().toString().isEmpty() || password.getText().toString().isEmpty()) {
            Toast.makeText(LoginActivity.this, "Email and password fields are mandatory", Toast.LENGTH_SHORT).show();
        }
        else {
            loginProgressBar.setVisibility(View.VISIBLE);
            Login loginRequest = new Login();
            loginRequest.setEmail(email.getText().toString());
            loginRequest.setPassword(password.getText().toString());

            //Converting request to json string
            ObjectMapper Obj = new ObjectMapper();
            String jsonStr = null;
            try {
                jsonStr = Obj.writeValueAsString(loginRequest);
            } catch (JsonProcessingException e) {
                e.printStackTrace();
            }

            //Making API call
            String url = Settings.BASE_URL.concat("Auth/login");
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

                            loginProgressBar.setVisibility(View.GONE);
                            try {
                                if (response.getString("code").equals(Settings.SUCCESS_RESPONSE)) {
                                    SharedPreferences preferences = getSharedPreferences(Settings.APP_PREFERENCE_NAME, MODE_PRIVATE);
                                    SharedPreferences.Editor editor = preferences.edit();
                                    JSONObject user = response.getJSONObject("data");
                                    editor.putBoolean("IS_LOGIN", true);
                                    editor.putInt("user_id", user.getInt("id"));
                                    editor.putString("full_name", user.getString("fullName"));
                                    editor.putString("email", user.getString("email"));
                                    editor.putString("phoneNumber", user.getString("phoneNumber"));
                                    editor.commit();
                                    email.setText("");
                                    password.setText("");
                                    startActivity(new Intent(LoginActivity.this, MainActivity.class));
                                } else {
                                    Toast.makeText(LoginActivity.this, Settings.FAILED_LOGIN_MESSAGE, Toast.LENGTH_SHORT).show();
                                }
                            } catch (JSONException e) {
                                e.printStackTrace();
                            }
                        }
                    }, new Response.ErrorListener() {
                @Override
                public void onErrorResponse(VolleyError error) {
                    loginProgressBar.setVisibility(View.GONE);
                    error.printStackTrace();
                    Toast.makeText(LoginActivity.this, Settings.FAILED_LOGIN_MESSAGE, Toast.LENGTH_SHORT).show();
                }
            });

            this.requestQueue.add(jsonObjectRequest);
        }
    }

    @Override
    public void onBackPressed() {
        //super.onBackPressed(); Removed this line cause i didn't want to use the default action of back button
        displayClosingAlertBox();
    }

    private void displayClosingAlertBox(){
        String msg = "Are you sure you want to exit?";
        new AlertDialog.Builder(LoginActivity.this)
                //.setIcon(android.R.drawable.star_on)
                .setTitle("Exiting the Application")
                .setMessage(msg)
                .setPositiveButton("Yes", new DialogInterface.OnClickListener() {
                    @RequiresApi(api = Build.VERSION_CODES.LOLLIPOP)
                    @Override
                    public void onClick(DialogInterface dialog, int which) {
                        finishAffinity();
                    }
                })
                .setNegativeButton("No", null)
                .show();
    }
}