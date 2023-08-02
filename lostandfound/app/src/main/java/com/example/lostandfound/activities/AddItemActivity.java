package com.example.lostandfound.activities;

import static androidx.constraintlayout.helper.widget.MotionEffect.TAG;

import androidx.appcompat.app.AppCompatActivity;
import androidx.core.app.ActivityCompat;
import androidx.core.content.ContextCompat;

import android.Manifest;
import android.content.Intent;
import android.content.SharedPreferences;
import android.content.pm.PackageManager;
import android.database.Cursor;
import android.graphics.Bitmap;
import android.net.Uri;
import android.os.Bundle;
import android.provider.MediaStore;
import android.util.Log;
import android.view.View;
import android.widget.AdapterView;
import android.widget.ArrayAdapter;
import android.widget.Button;
import android.widget.EditText;
import android.widget.ImageView;
import android.widget.ProgressBar;
import android.widget.Spinner;
import android.widget.TextView;
import android.widget.Toast;

import com.android.volley.DefaultRetryPolicy;
import com.android.volley.NetworkResponse;
import com.android.volley.Request;
import com.android.volley.RequestQueue;
import com.android.volley.Response;
import com.android.volley.VolleyError;
import com.example.lostandfound.MainActivity;
import com.example.lostandfound.R;
import com.example.lostandfound.settings.Settings;
import com.example.lostandfound.utils.VolleyMultipartRequest;
import com.example.lostandfound.utils.VolleySingleton;

import java.io.ByteArrayOutputStream;
import java.io.IOException;
import java.util.HashMap;
import java.util.Map;

public class AddItemActivity extends AppCompatActivity implements AdapterView.OnItemSelectedListener{
    private TextView selectImage;
    private ProgressBar loader;
    private RequestQueue requestQueue;
    private static final int REQUEST_PERMISSIONS = 100;
    private static final int PICK_IMAGE_REQUEST =1 ;
    private Bitmap bitmap;
    private String filePath;
    private EditText name;
    private EditText color;
    private EditText lostLocationAddress;
    private EditText retrievalLocationAddress;
    private EditText description;
    private EditText note;
    private ImageView imageSelected;
    private Spinner spinner;
    private int modeOfContact = 0;
    private static final String[] paths = {"--- Mode of Contact ---", "Email", "Phone Number"};



    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_add_item);
        name = findViewById(R.id.inputItemName);
        color = findViewById(R.id.inputColor);
        lostLocationAddress = findViewById(R.id.inputLostlocation);
        retrievalLocationAddress = findViewById(R.id.inputRetrievalLocation);
        description = findViewById(R.id.inputDescription);
        imageSelected = findViewById(R.id.imageSelected);
        spinner = findViewById(R.id.spinner);
        ArrayAdapter<String> adapter = new ArrayAdapter<String>(AddItemActivity.this,
                android.R.layout.simple_spinner_item,paths);

        adapter.setDropDownViewResource(android.R.layout.simple_spinner_dropdown_item);
        spinner.setAdapter(adapter);
        spinner.setOnItemSelectedListener(this);
        note = findViewById(R.id.inputNote);

        requestQueue = VolleySingleton.getmInstance(this).getRequestQueue();

        loader = findViewById(R.id.submitLoader);
        ImageView imageBack = findViewById(R.id.imageBack);
        imageBack.setOnClickListener(v -> onBackPressed());

        Button submit = findViewById(R.id.submitBtn);
        submit.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View view) {
                doSubmit();
            }
        });

        selectImage = findViewById(R.id.SelectImage);
        selectImage.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View view) {
                //Check for permissions
                if ((ContextCompat.checkSelfPermission(getApplicationContext(),
                        Manifest.permission.WRITE_EXTERNAL_STORAGE) != PackageManager.PERMISSION_GRANTED) && (ContextCompat.checkSelfPermission(getApplicationContext(),
                        Manifest.permission.READ_EXTERNAL_STORAGE) != PackageManager.PERMISSION_GRANTED)) {
                    if ((ActivityCompat.shouldShowRequestPermissionRationale(AddItemActivity.this,
                            Manifest.permission.WRITE_EXTERNAL_STORAGE)) && (ActivityCompat.shouldShowRequestPermissionRationale(AddItemActivity.this,
                            Manifest.permission.READ_EXTERNAL_STORAGE))) {

                    } else {
                        ActivityCompat.requestPermissions(AddItemActivity.this,
                                new String[]{Manifest.permission.WRITE_EXTERNAL_STORAGE, Manifest.permission.READ_EXTERNAL_STORAGE},
                                REQUEST_PERMISSIONS);
                    }
                } else {
                    Log.e("Else", "Else");
                    showFileChooser();
                }
            }
        });
    }

    private void showFileChooser() {
        Intent intent = new Intent();
        intent.setType("image/*");
        intent.setAction(Intent.ACTION_GET_CONTENT);
        startActivityForResult(Intent.createChooser(intent, "Select Picture"), PICK_IMAGE_REQUEST);
    }
    protected void onActivityResult(int requestCode, int resultCode, Intent data) {
        super.onActivityResult(requestCode, resultCode, data);

        if (requestCode == PICK_IMAGE_REQUEST && resultCode == RESULT_OK && data != null && data.getData() != null) {
            Uri picUri = data.getData();
            filePath = getPath(picUri);
            if (filePath != null) {
                try {

                    //textView.setText("File Selected");
                    Log.d("filePath", String.valueOf(filePath));
                    bitmap = MediaStore.Images.Media.getBitmap(getContentResolver(), picUri);
                    imageSelected.setImageBitmap(bitmap);
                    Toast.makeText(this, "Image Selected", Toast.LENGTH_SHORT).show();
                } catch (IOException e) {
                    e.printStackTrace();
                }
            }
            else
            {
                Toast.makeText(this,"No image selected", Toast.LENGTH_LONG).show();
            }
        }

    }
    public String getPath(Uri uri) {
        Cursor cursor = getContentResolver().query(uri, null, null, null, null);
        cursor.moveToFirst();
        String document_id = cursor.getString(0);
        if(document_id != null) {
            document_id = document_id.substring(document_id.lastIndexOf(":") + 1);
        }
        else {
            Toast.makeText(this, "This Image is not supported", Toast.LENGTH_SHORT).show();
            return null;
        }
        cursor.close();

        cursor = getContentResolver().query(
                android.provider.MediaStore.Images.Media.EXTERNAL_CONTENT_URI,
                null, MediaStore.Images.Media._ID + " = ? ", new String[]{document_id}, null);
        cursor.moveToFirst();
        String path = cursor.getString(cursor.getColumnIndexOrThrow(MediaStore.Images.Media.DATA));
        cursor.close();

        return path;
    }


    public byte[] getFileDataFromDrawable(Bitmap bitmap) {
        ByteArrayOutputStream byteArrayOutputStream = new ByteArrayOutputStream();
        bitmap.compress(Bitmap.CompressFormat.PNG, 80, byteArrayOutputStream);
        return byteArrayOutputStream.toByteArray();
    }


    public void doSubmit() {

        if (name.getText().toString().isEmpty() || color.getText().toString().isEmpty() || lostLocationAddress.getText().toString().isEmpty()
                || retrievalLocationAddress.getText().toString().isEmpty() || description.getText().toString().isEmpty() || note.getText().toString().isEmpty() || modeOfContact == 0 || filePath == null) {
            Toast.makeText(this, "All credentials are required", Toast.LENGTH_SHORT).show();
        }
        else {
            loader.setVisibility(View.VISIBLE);

            String url = Settings.BASE_URL.concat("items");
            VolleyMultipartRequest multipartRequest = new VolleyMultipartRequest(Request.Method.POST, url, new Response.Listener<NetworkResponse>() {
                @Override
                public void onResponse(NetworkResponse response) {
                    loader.setVisibility(View.GONE);
                    String resultResponse = new String(response.data);
                    // parse success output
                    if (response.statusCode == 200) {
                        Toast.makeText(AddItemActivity.this, "Item created successfully", Toast.LENGTH_SHORT);
                        startActivity(new Intent(AddItemActivity.this, MainActivity.class));
                    }
                }
            }, new Response.ErrorListener() {
                @Override
                public void onErrorResponse(VolleyError error) {
                    loader.setVisibility(View.GONE);
                    error.printStackTrace();
                    Toast.makeText(AddItemActivity.this, "An error occurred", Toast.LENGTH_SHORT).show();
                }
            }) {
                @Override
                protected Map<String, String> getParams() {
                    Map<String, String> params = new HashMap<>();
                    SharedPreferences preferences = getSharedPreferences(Settings.APP_PREFERENCE_NAME, MODE_PRIVATE);
                    int user_id = preferences.getInt("user_id", 0);

                    params.put("Name", name.getText().toString());
                    params.put("Color", color.getText().toString());
                    params.put("Description", description.getText().toString());
                    params.put("LostLocationAddress", lostLocationAddress.getText().toString());
                    params.put("RetrievalLocationAddress", retrievalLocationAddress.getText().toString());
                    params.put("Note", note.getText().toString());
                    params.put("ModeOfContact", String.valueOf(modeOfContact));
                    params.put("ReportedByUserId", String.valueOf(user_id));

                    return params;
                }

                @Override
                protected Map<String, DataPart> getByteData() {
                    Map<String, DataPart> params = new HashMap<>();
                    long imagename = System.currentTimeMillis();
                    params.put("File", new DataPart(imagename + ".png", getFileDataFromDrawable(bitmap)));
                    return params;
                }
            };

            multipartRequest.setRetryPolicy(new DefaultRetryPolicy(
                    0,
                    DefaultRetryPolicy.DEFAULT_MAX_RETRIES,
                    DefaultRetryPolicy.DEFAULT_BACKOFF_MULT));
            this.requestQueue.add(multipartRequest);
        }
    }

    @Override
    public void onItemSelected(AdapterView<?> parent, View v, int position, long id) {

        switch (position) {
            case 0:
                //Do nothing
                break;
            case 1:
            case 2:
                modeOfContact = position;
                break;

        }
    }

    @Override
    public void onNothingSelected(AdapterView<?> parent) {
        // TODO Auto-generated method stub
    }
}