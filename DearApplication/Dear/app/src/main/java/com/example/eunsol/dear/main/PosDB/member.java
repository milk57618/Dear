package com.example.eunsol.dear.main.PosDB;

/**
 * Created by eunsol on 2017-08-15.
 */

public class member {

    private String Name;
    private String Posi;
    private String Phone;
    private int Wage;
    private int Pay;

    public member(String Name,String Posi,String Phone,int Wage,int Pay){
        this.Name=Name;
        this.Posi=Posi;
        this.Phone=Phone;
        this.Wage=Wage;
        this.Pay=Pay;
    }

    public String getName() {
        return Name;
    }

    public String getPosi() {
        return Posi;
    }


    public String getPhone() {
        return Phone;
    }

    public int getWage() {
        return Wage;
    }

    public int getPay() {
        return Pay;
    }
}