package com.example.lostandfound;

import static androidx.constraintlayout.helper.widget.MotionEffect.TAG;

import androidx.annotation.RequiresApi;
import androidx.appcompat.app.AlertDialog;
import androidx.appcompat.app.AppCompatActivity;

import android.content.DialogInterface;
import android.content.Intent;
import android.os.Build;
import android.os.Bundle;
import android.util.Log;
import android.view.MenuItem;
import android.widget.ImageView;

import com.example.lostandfound.activities.AddItemActivity;
import com.example.lostandfound.fragments.BookmarkFragment;
import com.example.lostandfound.fragments.HomeFragment;
import com.example.lostandfound.fragments.ProfileFragment;
import com.example.lostandfound.R;
import com.google.android.material.bottomnavigation.BottomNavigationView;
import com.google.android.material.navigation.NavigationBarView;

public class MainActivity extends AppCompatActivity {

    BottomNavigationView bottomNavigationView;
    HomeFragment homeFragment = new HomeFragment();
    BookmarkFragment bookmarkFragment = new BookmarkFragment();
    ProfileFragment profileFragment = new ProfileFragment();

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_main);

        ImageView imageAddItem = findViewById(R.id.imageAddItem);
        imageAddItem.setOnClickListener(v -> startActivity(new Intent(MainActivity.this, AddItemActivity.class)));

        bottomNavigationView  = findViewById(R.id.bottom_navigation);

        getSupportFragmentManager().beginTransaction().replace(R.id.container,homeFragment).commit();

        bottomNavigationView.setOnItemSelectedListener(new NavigationBarView.OnItemSelectedListener() {
            @Override
            public boolean onNavigationItemSelected(MenuItem item) {
                switch (item.getItemId()){
                    case R.id.home:
                        getSupportFragmentManager().beginTransaction().replace(R.id.container,homeFragment).commit();
                        return true;
                    case R.id.bookmark:
                        getSupportFragmentManager().beginTransaction().replace(R.id.container,bookmarkFragment).commit();
                        return true;
                    case R.id.profile:
                        getSupportFragmentManager().beginTransaction().replace(R.id.container,profileFragment).commit();
                        return true;
                }

                return false;
            }
        });
    }

    @Override
    public void onBackPressed() {
        //super.onBackPressed(); Removed this line cause i didn't want to use the default action of back button
        displayClosingAlertBox();
    }

    private void displayClosingAlertBox(){
        String msg = "Are you sure you want to exit?";
        new AlertDialog.Builder(MainActivity.this)
                //.setIcon(android.R.drawable.star_on)
                .setTitle("Exiting the Application")
                .setMessage(msg)
                .setPositiveButton("Yes", new DialogInterface.OnClickListener() {
                    @RequiresApi(api = Build.VERSION_CODES.LOLLIPOP)
                    @Override
                    public void onClick(DialogInterface dialog, int which) {
                        Log.d(TAG, "ON STOP: YES");
                        finishAffinity();
                    }
                })
                .setNegativeButton("No", null)
                .show();
    }
}