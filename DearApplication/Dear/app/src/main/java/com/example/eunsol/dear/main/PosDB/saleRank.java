package com.example.eunsol.dear.main.PosDB;

/**
 * Created by eunsol on 2017-08-15.
 */

public class saleRank {

    private int Price;
    private int Cash;
    private int Card;
    private String Month;

    public saleRank(int Price,int Cash,int Card, String Month){


        this.Price=Price;
        this.Cash=Cash;
        this.Card=Card;
        this.Month=Month;

    }


    public int getPrice() {return  Price;}
    public int getCash() {return  Cash;}
    public int getCard() {return Card;}
    public String getMonth(){ return Month; }

}
