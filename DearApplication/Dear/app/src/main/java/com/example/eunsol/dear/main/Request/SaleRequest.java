package com.example.eunsol.dear.main.Request;

import org.json.JSONObject;

import java.io.BufferedReader;
import java.io.IOException;
import java.io.InputStream;
import java.io.InputStreamReader;
import java.io.OutputStream;
import java.net.HttpURLConnection;
import java.net.URL;

/**
 * Created by eunsol on 2017-08-15.
 */
public class SaleRequest {

    //InputStream 을 String으로 변환
    public static String convertToString(InputStream is) throws Exception {

        BufferedReader reader = new BufferedReader(new InputStreamReader(is));
        StringBuilder sb = new StringBuilder();
        String line = null;

        while ((line = reader.readLine()) != null) {
            sb.append(line + "\n");
        }

        is.close();

        return sb.toString();
    }

    public String request (String url,int PosId){

        try {

            URL Url = new URL(url);
            HttpURLConnection httpCon = (HttpURLConnection)Url.openConnection();

            String result="";
            String json="";

            //서버에 전달할 json객체 생성
            JSONObject jsonObject = new JSONObject();
            jsonObject.accumulate("PosId",PosId);
            json = jsonObject.toString();

            httpCon.setRequestMethod("POST");
            httpCon.setRequestProperty("Content-type", "application/json");

            //post 데이터를 넘긴다는 옵션
            httpCon.setDoOutput(true);
            //서버로부터 응답을 받는다.
            httpCon.setDoInput(true);

            OutputStream output = httpCon.getOutputStream();
            output.write(json.getBytes());
            output.flush();
            output.close(); // 출력 스트림을 닫고 모든 시스템 자원을 해제.


            try{

                InputStream input = httpCon.getInputStream();

                if(input != null)
                    result = convertToString(input);

                else
                    result = "Did not work!";

                return result;

            }catch (IOException e){
                e.printStackTrace();
            }

        }catch (Exception e){
            e.printStackTrace();

        }

        return null;
    }

}
