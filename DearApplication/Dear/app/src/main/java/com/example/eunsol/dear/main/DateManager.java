package com.example.eunsol.dear.main;

import java.text.ParseException;
import java.text.SimpleDateFormat;
import java.util.Date;

/**
 * Created by eunsol on 2017-08-15.
 */

public class DateManager  {

    private SimpleDateFormat formatter = new SimpleDateFormat("yy-MM-dd");

    public DateManager() {}
    public SimpleDateFormat getformatter(){return formatter;}

    // int 형 날짜들을 매개변수로 받아 원하는 타입의 날짜 string으로 바꿔준다.
    public String getDate(int Year,int Month,int Date){

        Date current =new Date();
        current.setYear(Year);
        current.setMonth(Month);
        current.setDate(Date);
        String selectedDate=formatter.format(current);

        return selectedDate;
    }



    // String을 매개변수로 받아 원하는 타입의 Date로 바꿔준다.
    public Date getDate(String s){

        Date selectDate = new Date();

        try {
            selectDate = formatter.parse(s);
        }catch (ParseException e){
            e.printStackTrace();
        }

        return selectDate;
    }


}
