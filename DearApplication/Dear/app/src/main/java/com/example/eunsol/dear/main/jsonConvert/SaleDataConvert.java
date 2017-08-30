package com.example.eunsol.dear.main.jsonConvert;

import com.example.eunsol.dear.main.DB_Pos.sale;
import com.example.eunsol.dear.main.DB_Pos.saleRank;
import org.json.JSONArray;
import org.json.JSONException;
import org.json.JSONObject;
import java.util.ArrayList;
import java.util.HashMap;

/**
 * Created by TAEYONG on 2017-08-07.
 */

public class SaleDataConvert {

    private static final String TAG_Response="SaleSearch";
    private static final String TAG_Price = "Price";
    private static final String TAG_Cash = "Cash";
    private static final String TAG_Card = "Card";
    private static final String TAG_SaleCount = "SaleCount";
    private static final String TAG_SaleTime = "SaleTime";
    private static final String TAG_PosId = "Id";
    private static final String TAG_Finished="Finished";
    private JSONArray sale;
    private HashMap<String,ArrayList<sale>> saleList;
    private HashMap<String,saleRank> saleMonthList;

    private ArrayList<saleRank> ranks;

    public HashMap<String,ArrayList<sale>> getSaleList() { return saleList; }

    public void getData(String s){

        saleList= new HashMap();
        saleMonthList = new HashMap();

        try {

            JSONObject jsonobject = new JSONObject(s);

            sale = jsonobject.getJSONArray(TAG_Response);
            ArrayList<sale> valueArray= new ArrayList<sale>();
            ArrayList<saleRank> monthValueArray = new ArrayList<saleRank>();
            String Key = sale.getJSONObject(0).getString(TAG_SaleTime).substring(0,8);


            //날짜별로 저장
            for(int i=0;i<sale.length();i++){
                // Log.d("날짜","!"+Key);
                JSONObject saleMember = sale.getJSONObject(i);
                String saleKey =  sale.getJSONObject(i).getString(TAG_SaleTime).substring(0,8);
                sale value=new sale(saleMember.getInt(TAG_PosId),saleMember.getInt(TAG_SaleCount),saleMember.getInt(TAG_Price),saleMember.getInt(TAG_Cash),saleMember.getInt(TAG_Card),saleKey,saleMember.getInt(TAG_Finished));

                if(value.getSaleTime().equals(Key)){
                    valueArray.add(value);
                } else{

                    ArrayList<sale> addArray= new ArrayList<sale>(valueArray);
                    saleList.put(Key,addArray);
                    Key = value.getSaleTime();
                    valueArray.clear();
                    valueArray.add(value);
                }
            }

            saleList.put(Key,valueArray);

            //달별로 저장
            Key = sale.getJSONObject(0).getString(TAG_SaleTime).substring(0,5);
            int Price=0;
            int Cash=0;
            int Card=0;

            for (int i=0;i<sale.length();i++){

                String tp =  sale.getJSONObject(i).getString(TAG_SaleTime).substring(0,5);

                if(tp.equals(Key)){
                    Price+=sale.getJSONObject(i).getInt(TAG_Price);
                    Cash+=sale.getJSONObject(i).getInt(TAG_Cash);
                    Card+=sale.getJSONObject(i).getInt(TAG_Card);
                }else{
                    saleMonthList.put(Key,new saleRank(Price,Cash,Card,Key));
                    Price=sale.getJSONObject(i).getInt(TAG_Price);
                    Cash=sale.getJSONObject(i).getInt(TAG_Cash);
                    Card=sale.getJSONObject(i).getInt(TAG_Card);
                    Key=tp;
                }

                saleMonthList.put(Key,new saleRank(Price,Cash,Card,Key));
            }



        }catch (JSONException e){
            e.printStackTrace();
        }
    }

    public String searchData(String selected){

        ArrayList<sale> searchList= new ArrayList<sale>();
        int total = 0;

        searchList = saleList.get(selected);


        if(searchList!=null){
            for(int i=0;i<searchList.size();i++) {
                total += searchList.get(i).getPrice();
            }
        }

        return Integer.toString(total);
    }

    //현재 날짜를 기준으로 앞의 4달의 매출을 조사하는 함수
    public ArrayList<saleRank> getRanks (String curday){

        ranks = new ArrayList<saleRank>();

        curday = curday.substring(0,5);

        for(int i=0;i<4;i++) {
            saleRank rank = saleMonthList.get(curday);

            if (rank == null) {
                ranks.add(new saleRank(0,0,0,curday));
            } else {
                ranks.add(rank);
            }

            int tp = Integer.parseInt(curday.substring(3,5));

            if(tp==1)
                tp = 12;
            else
                tp--;

            if(tp<10)
                curday=curday.substring(0,3)+"0"+tp;

            else
                curday=curday.substring(0,3)+tp;
        }

        return  ranks;
    }
}
