package com.example.eunsol.dear.main.jsonConvert;

import org.json.JSONException;
import org.json.JSONObject;

/**
 * Created by eunsol on 2017-08-15.
 */

public class LoginDataConvert {

    private int PosId=-1;
    private String StoreName;
    private static final String TAG_PosId="PosId";
    private static final String TAG_StoreName="StoreName";


    public int getPosId() {return  PosId; }
    public String getStoreName()  {return  StoreName; }

    public void getData(String s){

        try{

            JSONObject jsonObject = new JSONObject(s);

            PosId = jsonObject.getInt(TAG_PosId);
            StoreName=jsonObject.getString(TAG_StoreName);

        }catch (JSONException e){
            e.printStackTrace();
        }
    }

}
