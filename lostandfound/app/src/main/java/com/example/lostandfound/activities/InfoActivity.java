package com.example.lostandfound.activities;

import static androidx.constraintlayout.helper.widget.MotionEffect.TAG;

import androidx.annotation.RequiresApi;
import androidx.appcompat.app.AlertDialog;
import androidx.appcompat.app.AppCompatActivity;
import androidx.recyclerview.widget.LinearLayoutManager;
import androidx.recyclerview.widget.RecyclerView;

import android.app.Activity;
import android.content.Context;
import android.content.DialogInterface;
import android.content.Intent;
import android.content.SharedPreferences;
import android.os.Build;
import android.os.Bundle;
import android.util.Log;
import android.view.View;
import android.view.inputmethod.InputMethodManager;
import android.widget.Button;
import android.widget.EditText;
import android.widget.ImageView;
import android.widget.LinearLayout;
import android.widget.ProgressBar;
import android.widget.TextView;
import android.widget.Toast;

import com.android.volley.Request;
import com.android.volley.RequestQueue;
import com.android.volley.Response;
import com.android.volley.VolleyError;
import com.android.volley.toolbox.JsonObjectRequest;
import com.bumptech.glide.Glide;
import com.example.lostandfound.MainActivity;
import com.example.lostandfound.R;
import com.example.lostandfound.adapters.CommentAdapter;
import com.example.lostandfound.models.AddBookmarkRequest;
import com.example.lostandfound.models.CloseItem;
import com.example.lostandfound.models.Comment;
import com.example.lostandfound.models.CommentList;
import com.example.lostandfound.settings.Settings;
import com.example.lostandfound.utils.VolleySingleton;
import com.fasterxml.jackson.core.JsonProcessingException;
import com.fasterxml.jackson.databind.ObjectMapper;
import com.google.android.material.bottomsheet.BottomSheetBehavior;

import org.json.JSONArray;
import org.json.JSONException;
import org.json.JSONObject;

import java.util.ArrayList;
import java.util.List;

public class InfoActivity extends AppCompatActivity {

    private ProgressBar actionLoader;
    private RequestQueue requestQueue;
    private int itemId, user_id;
    private ImageView bookmark;
    private EditText comment;
    private List<CommentList> commentArrayList;
    private RecyclerView recyclerView;
    private CommentAdapter commentAdapter;
    private TextView addComment;

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_info);

        ImageView imageBack = findViewById(R.id.imageBack);
        imageBack.setOnClickListener(v -> onBackPressed());

        recyclerView = findViewById(R.id.commentsRecyclerView);
        recyclerView.setHasFixedSize(true);
        recyclerView.setLayoutManager(new LinearLayoutManager(this));

        requestQueue = VolleySingleton.getmInstance(this).getRequestQueue();

        actionLoader = findViewById(R.id.actionLoader);
        ImageView image = findViewById(R.id.image);
        TextView name = findViewById(R.id.name);
        TextView note = findViewById(R.id.note);
        TextView description = findViewById(R.id.description);
        TextView lostLocationAddress = findViewById(R.id.lostLocationAddress);
        TextView createdAt = findViewById(R.id.createdAt);
        TextView color = findViewById(R.id.color);
        TextView contact = findViewById(R.id.contact);
        TextView retrievalLocationAddress = findViewById(R.id.retrievalLocationAddress);

        ImageView closeItem = findViewById(R.id.closeItem);
        closeItem.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View view) {
                String msg = "Are you sure you want to close this item ?";
                new AlertDialog.Builder(InfoActivity.this)
                        //.setIcon(android.R.drawable.star_on)
                        .setTitle("Closing Found Item")
                        .setMessage(msg)
                        .setPositiveButton("Yes", new DialogInterface.OnClickListener() {
                            @RequiresApi(api = Build.VERSION_CODES.LOLLIPOP)
                            @Override
                            public void onClick(DialogInterface dialog, int which) {
                                doCloseItem();
                            }
                        })
                        .setNegativeButton("No", null)
                        .show();
            }
        });

        bookmark = findViewById(R.id.bookmarkItem);
        bookmark.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View view) {
                doBookmark();
            }
        });

        Bundle bundle = getIntent().getExtras();
        SharedPreferences preferences = getSharedPreferences(Settings.APP_PREFERENCE_NAME, MODE_PRIVATE);
        user_id = preferences.getInt("user_id", 0);

        if(bundle.getInt("reportedByUserId") == user_id) {
            closeItem.setVisibility(View.VISIBLE);
        }

        name.setText("Name: "+ bundle.getString("name"));
        description.setText("Description: "+ bundle.getString("description"));
        Glide.with(this).load(bundle.getString("picture")).into(image);
        color.setText("Colour: "+ bundle.getString("color"));
        itemId = bundle.getInt("id");
        lostLocationAddress.setText("Lost location: "+ bundle.getString("lostLocationAddress"));
        retrievalLocationAddress.setText("Retrieval Location: "+ bundle.getString("retrievalLocationAddress"));
        note.setText("Note: "+ bundle.getString("note"));
        contact.setText("Contact: "+ bundle.getString("contact"));
        String date = bundle.getString("createdAt").split("T")[0];
        String[] time = bundle.getString("createdAt").split("T")[1].split(":");

        createdAt.setText("Created At: "+ date + " "+ time[0]+":"+time[1]);

        commentArrayList = new ArrayList<>();
        checkIfItemIsBookmarked();
        fetchComments();
        initAddComment();
    }

    private void doBookmark() {
        actionLoader.setVisibility(View.VISIBLE);
        AddBookmarkRequest request = new AddBookmarkRequest();
        request.setItemId(itemId);
        request.setUserId(user_id);

        //Converting request to json string
        ObjectMapper Obj = new ObjectMapper();
        String jsonStr = null;
        try {
            jsonStr = Obj.writeValueAsString(request);
        } catch (JsonProcessingException e) {
            e.printStackTrace();
        }

        //Making API call
        String url = Settings.BASE_URL.concat("userbookmarks");
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

                        actionLoader.setVisibility(View.GONE);
                        try {
                            if (response.getString("code").equals(Settings.SUCCESS_RESPONSE)) {
                                bookmark.setVisibility(View.GONE);
                                Toast.makeText(InfoActivity.this, "This item has been bookmarked", Toast.LENGTH_SHORT).show();
                            } else {
                                Toast.makeText(InfoActivity.this, "Failed to bookmark item", Toast.LENGTH_SHORT).show();
                            }
                        } catch (JSONException e) {
                            e.printStackTrace();
                        }
                    }
                }, new Response.ErrorListener() {
            @Override
            public void onErrorResponse(VolleyError error) {
                actionLoader.setVisibility(View.GONE);
                error.printStackTrace();
                Toast.makeText(InfoActivity.this, "Failed to bookmark item", Toast.LENGTH_SHORT).show();
            }
        });

        this.requestQueue.add(jsonObjectRequest);
    }

    private void doCloseItem() {
        actionLoader.setVisibility(View.VISIBLE);
        CloseItem request = new CloseItem();
        request.setItemId(itemId);
        request.setUserId(user_id);
        request.setClosingRemarks("Owner Identified the item");

        //Converting request to json string
        ObjectMapper Obj = new ObjectMapper();
        String jsonStr = null;
        try {
            jsonStr = Obj.writeValueAsString(request);
        } catch (JsonProcessingException e) {
            e.printStackTrace();
        }

        //Making API call
        String url = Settings.BASE_URL.concat("items/closelostitem");
        JSONObject jsonBody = null;
        try {
            jsonBody = new JSONObject(jsonStr);
        } catch (JSONException e) {
            e.printStackTrace();
        }

        JsonObjectRequest jsonObjectRequest = new JsonObjectRequest(Request.Method.PUT, url, jsonBody,
                new Response.Listener<JSONObject>() {
                    @Override
                    public void onResponse(JSONObject response) {

                        actionLoader.setVisibility(View.GONE);
                        try {
                            if (response.getString("code").equals(Settings.SUCCESS_RESPONSE)) {
                                Toast.makeText(InfoActivity.this, "Item has been closed successfully", Toast.LENGTH_SHORT).show();
                                startActivity(new Intent(InfoActivity.this, MainActivity.class));
                            } else {
                                Toast.makeText(InfoActivity.this, "Failed to close item", Toast.LENGTH_SHORT).show();
                            }
                        } catch (JSONException e) {
                            e.printStackTrace();
                        }
                    }
                }, new Response.ErrorListener() {
            @Override
            public void onErrorResponse(VolleyError error) {
                actionLoader.setVisibility(View.GONE);
                error.printStackTrace();
                Toast.makeText(InfoActivity.this, "Failed to close item", Toast.LENGTH_SHORT).show();
            }
        });

        this.requestQueue.add(jsonObjectRequest);
    }

    private void initAddComment() {
        final LinearLayout layoutAddComments = findViewById(R.id.layoutAddComments);
        final BottomSheetBehavior<LinearLayout> bottomSheetBehavior = BottomSheetBehavior.from(layoutAddComments);
        addComment = layoutAddComments.findViewById(R.id.textAddComment);
        addComment.setOnClickListener(v -> {
            if (bottomSheetBehavior.getState() != BottomSheetBehavior.STATE_EXPANDED) {
                bottomSheetBehavior.setState(BottomSheetBehavior.STATE_EXPANDED);
            } else {
                bottomSheetBehavior.setState(BottomSheetBehavior.STATE_COLLAPSED);
            }
        });
        comment = findViewById(R.id.comment);
        Button submitCommentBtn = findViewById(R.id.submitComment);
        submitCommentBtn.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View view) {
                addComment.performClick();
                submitComment(view);
            }
        });
    }

    private void submitComment(View view) {
        actionLoader.setVisibility(View.VISIBLE);
        SharedPreferences preferences = getSharedPreferences(Settings.APP_PREFERENCE_NAME, MODE_PRIVATE);
        String full_name = preferences.getString("full_name", "Dummy User");
        Comment request = new Comment();
        request.setItemId(itemId);
        request.setUserId(user_id);
        request.setComment(comment.getText().toString());

        //Converting request to json string
        ObjectMapper Obj = new ObjectMapper();
        String jsonStr = null;
        try {
            jsonStr = Obj.writeValueAsString(request);
        } catch (JsonProcessingException e) {
            e.printStackTrace();
        }

        //Making API call
        String url = Settings.BASE_URL.concat("items/addusercommenttoitem");
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

                        actionLoader.setVisibility(View.GONE);
                        try {
                            if (response.getString("code").equals(Settings.SUCCESS_RESPONSE)) {
                                Toast.makeText(InfoActivity.this, "Comment added successfully", Toast.LENGTH_SHORT).show();
                                CommentList commentList = new CommentList(full_name, comment.getText().toString());
                                commentArrayList.add(0, commentList);
                                commentAdapter.setFilteredList(commentArrayList);
                                comment.setText("");
                                hideSoftKeyboard(InfoActivity.this, view);
                            } else {
                                Toast.makeText(InfoActivity.this, "Failed to add comment", Toast.LENGTH_SHORT).show();
                            }
                        } catch (JSONException e) {
                            e.printStackTrace();
                        }
                    }
                }, new Response.ErrorListener() {
            @Override
            public void onErrorResponse(VolleyError error) {
                actionLoader.setVisibility(View.GONE);
                error.printStackTrace();
                Toast.makeText(InfoActivity.this, "Failed to add comment", Toast.LENGTH_SHORT).show();
            }
        });

        this.requestQueue.add(jsonObjectRequest);
    }

    private void fetchComments() {

        actionLoader.setVisibility(View.VISIBLE);

        String url = Settings.BASE_URL.concat("Items/"+itemId+"/getItemComments");

        Log.d(TAG, "URL: "+ url);

        JsonObjectRequest jsonObjectRequest = new JsonObjectRequest(Request.Method.GET, url, null,
                new Response.Listener<JSONObject>() {
                    @Override
                    public void onResponse(JSONObject response) {
                        actionLoader.setVisibility(View.GONE);
                        try {
                            if(response.getString("code").equals(Settings.SUCCESS_RESPONSE)) {
                                JSONArray data = response.getJSONArray("data");
                                for (int i = 0; i < data.length(); i++) {
                                    try {
                                        JSONObject jsonObject = data.getJSONObject(i);
                                        String name = jsonObject.getString("commentedBy");
                                        String description = jsonObject.getString("comment");

                                        CommentList item = new CommentList(name, description);
                                        commentArrayList.add(item);
                                    } catch (JSONException e) {
                                        e.printStackTrace();
                                    }
                                }
                            }
                            commentAdapter = new CommentAdapter(InfoActivity.this, commentArrayList);

                            recyclerView.setAdapter(commentAdapter);
                        } catch (JSONException e) {
                            e.printStackTrace();
                        }
                    }
                }, new Response.ErrorListener() {
            @Override
            public void onErrorResponse(VolleyError error) {
                actionLoader.setVisibility(View.GONE);
                error.printStackTrace();
                Toast.makeText(InfoActivity.this, "Failed to fetch comments", Toast.LENGTH_SHORT).show();
            }
        });
        requestQueue.add(jsonObjectRequest);
    }

    //This method runs in silence
    private void checkIfItemIsBookmarked() {
        String url = Settings.BASE_URL.concat("UserBookmarks/validateUserItemBookmark/"+user_id+"/"+itemId);

        JsonObjectRequest jsonObjectRequest = new JsonObjectRequest(Request.Method.GET, url, null,
                new Response.Listener<JSONObject>() {
                    @Override
                    public void onResponse(JSONObject response) {
                        try {
                            if(response.getString("code").equals(Settings.SUCCESS_RESPONSE)) {
                                JSONObject data = response.getJSONObject("data");
                                if(!data.getBoolean("itemIsBookmark")) bookmark.setVisibility(View.VISIBLE);
                            }
                            else {
                                Toast.makeText(InfoActivity.this, response.getString("message"), Toast.LENGTH_SHORT).show();
                            }
                        } catch (JSONException e) {
                            e.printStackTrace();
                        }
                    }
                }, new Response.ErrorListener() {
            @Override
            public void onErrorResponse(VolleyError error) {
                error.printStackTrace();
            }
        });
        requestQueue.add(jsonObjectRequest);
    }

    public static void hideSoftKeyboard (Activity activity, View view)
    {
        InputMethodManager imm = (InputMethodManager)activity.getSystemService(Context.INPUT_METHOD_SERVICE);
        imm.hideSoftInputFromWindow(view.getApplicationWindowToken(), 0);
    }
}