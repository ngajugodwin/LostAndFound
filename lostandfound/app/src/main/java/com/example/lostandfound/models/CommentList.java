package com.example.lostandfound.models;

public class CommentList {

    private String name, comment;

    public String getName() {
        return name;
    }

    public String getComment() {
        return comment;
    }

    public CommentList(String name, String comment) {
        this.name = name;
        this.comment = comment;
    }
}
