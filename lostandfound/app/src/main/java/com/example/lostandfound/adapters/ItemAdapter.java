package com.example.lostandfound.adapters;

import static android.content.Context.MODE_PRIVATE;

import android.content.Context;
import android.content.DialogInterface;
import android.content.Intent;
import android.content.SharedPreferences;
import android.os.Build;
import android.os.Bundle;
import android.os.Handler;
import android.os.Looper;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;
import android.widget.ImageView;
import android.widget.TextView;
import android.widget.Toast;

import androidx.annotation.NonNull;
import androidx.annotation.RequiresApi;
import androidx.appcompat.app.AlertDialog;
import androidx.constraintlayout.widget.ConstraintLayout;
import androidx.recyclerview.widget.RecyclerView;

import com.android.volley.AuthFailureError;
import com.android.volley.Request;
import com.android.volley.RequestQueue;
import com.android.volley.Response;
import com.android.volley.VolleyError;
import com.android.volley.toolbox.JsonObjectRequest;
import com.bumptech.glide.Glide;
import com.example.lostandfound.R;
import com.example.lostandfound.activities.InfoActivity;
import com.example.lostandfound.models.AddBookmarkRequest;
import com.example.lostandfound.models.Item;
import com.example.lostandfound.settings.Settings;
import com.example.lostandfound.utils.VolleySingleton;
import com.fasterxml.jackson.core.JsonProcessingException;
import com.fasterxml.jackson.databind.ObjectMapper;

import org.json.JSONException;
import org.json.JSONObject;

import java.util.ArrayList;
import java.util.HashMap;
import java.util.List;
import java.util.Map;
import java.util.Timer;
import java.util.TimerTask;

public class ItemAdapter extends RecyclerView.Adapter<ItemAdapter.ItemHolder> {

    private Context context;
    private List<Item> itemList;
    private final List<Item> itemsSource;
    private boolean isBookmark;
    private RequestQueue requestQueue;

    private Timer timer;

    public ItemAdapter(Context context , List<Item> items, boolean isBookmark){
        this.context = context;
        itemList = items;
        itemsSource = items;
        this.isBookmark = isBookmark;
        requestQueue = VolleySingleton.getmInstance(context).getRequestQueue();
    }

    @NonNull
    @Override
    public ItemHolder onCreateViewHolder(@NonNull ViewGroup parent, int viewType) {
        View view = LayoutInflater.from(context).inflate(R.layout.item , parent , false);
        return new ItemHolder(view);
    }

    @Override
    public void onBindViewHolder(@NonNull ItemHolder holder, int position) {
        Item item = itemList.get(position);
        holder.color.setText("Color: "+ item.getColor());
        holder.name.setText(item.getName());
        holder.description.setText(item.getDescription());
        Glide.with(context).load(item.getPictureUrl()).into(holder.imageView);

        holder.constraintLayout.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View v) {
                Intent intent = new Intent(context , InfoActivity.class);

                Bundle bundle = new Bundle();
                bundle.putString("name" , item.getName());
                bundle.putString("description" , item.getDescription());
                bundle.putString("picture" , item.getPictureUrl());
                bundle.putString("color" , item.getColor());
                bundle.putInt("id", item.getId());
                bundle.putString("lostLocationAddress", item.getLostLocationAddress());
                bundle.putString("retrievalLocationAddress", item.getRetrievalLocationAddress());
                bundle.putString("note", item.getNote());
                bundle.putString("contact", item.getContact());
                bundle.putBoolean("active", item.isActive());
                bundle.putString("createdAt", item.getCreatedAt());
                bundle.putInt("reportedByUserId", item.getReportedByUserId());

                intent.putExtras(bundle);

                context.startActivity(intent);
            }
        });
        holder.removeBookmark.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View view) {
                String msg = "Sure to remove bookmark ?";
                new AlertDialog.Builder(context)
                        .setTitle("Remove Bookmark")
                        .setMessage(msg)
                        .setPositiveButton("Yes", new DialogInterface.OnClickListener() {
                            @RequiresApi(api = Build.VERSION_CODES.LOLLIPOP)
                            @Override
                            public void onClick(DialogInterface dialog, int which) {
                                SharedPreferences preferences = view.getContext().getSharedPreferences(Settings.APP_PREFERENCE_NAME, MODE_PRIVATE);
                                int user_id = preferences.getInt("user_id", 0);
                                AddBookmarkRequest request = new AddBookmarkRequest();
                                request.setItemId(item.getId());
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
                                String url = Settings.BASE_URL.concat("UserBookmarks/removeUserItemBookmark");
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
                                                try {
                                                    if (response.getString("code").equals(Settings.SUCCESS_RESPONSE)) {
                                                        itemList.remove(holder.getAdapterPosition());
                                                        notifyDataSetChanged();
                                                        Toast.makeText(context, "Bookmark removed", Toast.LENGTH_SHORT).show();
                                                    } else {
                                                        Toast.makeText(context, "Failed to remove bookmark", Toast.LENGTH_SHORT).show();
                                                    }
                                                } catch (JSONException e) {
                                                    e.printStackTrace();
                                                }
                                            }
                                        }, new Response.ErrorListener() {
                                    @Override
                                    public void onErrorResponse(VolleyError error) {
                                        error.printStackTrace();
                                        Toast.makeText(context, "Failed to remove bookmark", Toast.LENGTH_SHORT).show();
                                    }
                                }){ //no semicolon or coma
                                    @Override
                                    public Map<String, String> getHeaders() throws AuthFailureError {
                                        Map<String, String> params = new HashMap<String, String>();
                                        params.put("Content-Type", "application/json");
                                        params.put("accept", "*/*");
                                        return params;
                                    } };

                                requestQueue.add(jsonObjectRequest);
                            }
                        })
                        .setNegativeButton("No", null)
                        .show();
            }
        });
        if(isBookmark) {
            holder.removeBookmark.setVisibility(View.VISIBLE);
        }
    }

    @Override
    public int getItemCount() {
        return itemList.size();
    }

    public void searchItems(final String searchKeyword) {
        timer = new Timer();
        timer.schedule(new TimerTask() {
            @Override
            public void run() {
                if (searchKeyword.trim().isEmpty()) {
                    itemList = itemsSource;
                } else {
                    ArrayList<Item> temp = new ArrayList<>();
                    for (Item item : itemsSource) {
                        if (item.getName().toLowerCase().contains(searchKeyword.toLowerCase()) ||
                                item.getDescription().toLowerCase().contains(searchKeyword.toLowerCase())) {
                            temp.add(item);
                        }
                    }
                    itemList = temp;
                }

                new Handler(Looper.getMainLooper()).post(() -> notifyDataSetChanged());
            }
        }, 500);
    }

    public void cancelTimer() {
        if (timer != null) {
            timer.cancel();
        }
    }

    private void BookmarkRemoved(int position){

    }

    public class ItemHolder extends RecyclerView.ViewHolder{

        ImageView imageView, removeBookmark;
        TextView name , description , color;
        ConstraintLayout constraintLayout;

        public ItemHolder(@NonNull View itemView) {
            super(itemView);

            imageView = itemView.findViewById(R.id.imageview);
            name = itemView.findViewById(R.id.name_tv);
            description = itemView.findViewById(R.id.description_tv);
            color = itemView.findViewById(R.id.color_tv);
            constraintLayout = itemView.findViewById(R.id.main_layout);
            removeBookmark = itemView.findViewById(R.id.bookmarkremove);

        }
    }
}
