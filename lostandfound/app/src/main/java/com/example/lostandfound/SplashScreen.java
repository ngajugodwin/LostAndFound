package com.example.lostandfound;

import android.content.Intent;
import android.content.SharedPreferences;
import android.os.Bundle;
import android.os.Handler;

import androidx.appcompat.app.AppCompatActivity;

import com.example.lostandfound.activities.LoginActivity;
import com.example.lostandfound.settings.Settings;

public class SplashScreen extends AppCompatActivity {
    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);

        new Handler().postDelayed(new Runnable() {
            @Override
            public void run() {
                SharedPreferences preferences = getSharedPreferences(Settings.APP_PREFERENCE_NAME, MODE_PRIVATE);
                boolean loggedIn = preferences.getBoolean("IS_LOGIN", false);
                if(loggedIn) {
                    startActivity(new Intent(SplashScreen.this, MainActivity.class));
                } else {
                    startActivity(new Intent(SplashScreen.this, LoginActivity.class));
                }
                finish();
            }
        }, 3000);
    }
}
