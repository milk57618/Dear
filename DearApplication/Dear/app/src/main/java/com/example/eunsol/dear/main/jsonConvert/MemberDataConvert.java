package com.example.eunsol.dear.main.jsonConvert;

import android.util.Log;

import com.example.eunsol.dear.main.PosDB.member;

import org.json.JSONArray;
import org.json.JSONException;
import org.json.JSONObject;

import java.util.ArrayList;

/**
 * Created by eunsol on 2017-08-15.
 */

public class MemberDataConvert {

    private static final String TAG_Response="Member";
    private static final String TAG_Name = "Name";
    private static final String TAG_Posi = "Position";
    private static final String TAG_Phone = "PhoneNumber";
    private static final String TAG_Wage = "WageH";
    private static final String TAG_Pay = "Pay";
    private JSONArray Member;
    private ArrayList<member> MemberList; //직원 목록 저장

    public void getData(String s){

        MemberList = new ArrayList<>();


        try {

            JSONObject jsonobject = new JSONObject(s);
            Member = jsonobject.getJSONArray(TAG_Response);


            //날짜별로 저장
            for(int i = 0; i< Member.length(); i++){
                member value = new member(Member.getJSONObject(i).getString(TAG_Name),Member.getJSONObject(i).getString(TAG_Posi),Member.getJSONObject(i).getString(TAG_Phone)
                        ,Member.getJSONObject(i).getInt(TAG_Wage),(int)Member.getJSONObject(i).getDouble(TAG_Pay));
                MemberList.add(value);
            }


            for(int i=0;i<MemberList.size();i++){
                Log.d("직원","!"+MemberList.get(i).getName()+MemberList.get(i).getPay());
            }

        }catch (JSONException e){
            e.printStackTrace();
        }

    }

    public ArrayList<member> getMemberList() {
        return MemberList;
    }

}
