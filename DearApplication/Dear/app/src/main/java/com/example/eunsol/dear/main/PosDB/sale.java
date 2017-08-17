package com.example.eunsol.dear.main.PosDB;

/**
 * Created by eunsol on 2017-08-15.
 */

public class sale {

    private int posNum;
    private int SaleCount;
    private int Price;
    private int Cash;
    private int Card;
    private String SaleTime;
    private int finished;


    public sale(int posNum,int saleCount,int Price,int Cash,int Card, String SaleTime,int finished){

        this.posNum=posNum;
        this.SaleCount=saleCount;
        this.Price=Price;
        this.Cash=Cash;
        this.Card=Card;
        this.SaleTime = SaleTime;
        this.finished=finished;
    }

    public int getPosNum() {return posNum;}
    public int getSaleCount() {return SaleCount;}
    public int getPrice() {return  Price;}
    public int getCash() {return  Cash;}
    public int getCard() {return Card;}
    public String getSaleTime(){ return SaleTime; }
    public int getFinished() {return finished; }

}
