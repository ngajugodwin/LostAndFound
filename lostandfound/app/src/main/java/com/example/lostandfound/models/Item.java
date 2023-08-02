package com.example.lostandfound.models;

public class Item {

    private int id, reportedByUserId;
    private String name, color, description, lostLocationAddress, retrievalLocationAddress, note, pictureUrl, contact, createdAt;
    private boolean active;

    public Item(int id, String name, String color, String description, String lostLocationAddress, String retrievalLocationAddress, String note, String pictureUrl, String contact, String createdAt, boolean active, int reportedByUserId) {
        this.id = id;
        this.name = name;
        this.color = color;
        this.description = description;
        this.lostLocationAddress = lostLocationAddress;
        this.retrievalLocationAddress = retrievalLocationAddress;
        this.note = note;
        this.pictureUrl = pictureUrl;
        this.contact = contact;
        this.createdAt = createdAt;
        this.active = active;
        this.reportedByUserId = reportedByUserId;
    }

    public int getId() {
        return id;
    }

    public String getName() {
        return name;
    }

    public String getColor() {
        return color;
    }

    public String getDescription() {
        return description;
    }

    public String getLostLocationAddress() {
        return lostLocationAddress;
    }

    public String getRetrievalLocationAddress() {
        return retrievalLocationAddress;
    }

    public String getNote() {
        return note;
    }

    public String getPictureUrl() {
        return pictureUrl;
    }

    public String getContact() {
        return contact;
    }

    public String getCreatedAt() {
        return createdAt;
    }

    public boolean isActive() {
        return active;
    }

    public int getReportedByUserId() {
        return reportedByUserId;
    }
}
