package com.example.eunsol.dear.main.Activity;

import android.app.ProgressDialog;
import android.content.Context;
import android.content.Intent;
import android.graphics.Color;
import android.os.AsyncTask;
import android.os.Bundle;
import android.support.v7.app.ActionBar;
import android.support.v7.app.AppCompatActivity;
import android.view.View;
import android.widget.Button;
import android.widget.TextView;

import com.example.eunsol.dear.R;
import com.example.eunsol.dear.main.DateManager;
import com.example.eunsol.dear.main.DB_Pos.saleRank;
import com.example.eunsol.dear.main.Request.SaleRequest;
import com.example.eunsol.dear.main.jsonConvert.SaleDataConvert;
import com.github.sundeepk.compactcalendarview.CompactCalendarView;
import com.github.sundeepk.compactcalendarview.domain.Event;

import java.util.ArrayList;
import java.util.Calendar;
import java.util.Date;

/**
 * Created by eunsol on 2017-08-15.
 */

public class SaleActivity extends AppCompatActivity implements CompactCalendarView.CompactCalendarViewListener{

    private CompactCalendarView compactCalendarView; //캘린더 뷰
    private Calendar currentday = Calendar.getInstance();  // 현재 날짜/시간 등의 각종 정보 얻기
    private Button analysis;
    private TextView tv_profit;
    private String selectedDay;  //선택한 날짜
    private String curDay; //현재 날짜
    private DateManager dateManager = new DateManager();
    private ActionBar actionBar;
    private int posNum;
    private SaleDataConvert saleDataConvert; //판매 내역을 변환해서 sale 형태로 저장
    private final String url = "http://125.183.218.149/showSaleSearch.php";
    public static ArrayList<saleRank> rank; //순위

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_sale);
        System.setProperty("http.keepalive","false");

        //앞의 엑티비티로부터 보낸 값을 얻는다.
        Intent intent = getIntent();
        posNum=intent.getIntExtra("posNum",-1);

        actionBar = getSupportActionBar();
        actionBar.setDisplayHomeAsUpEnabled(false);
        actionBar.setTitle(null);  //액션바 초기화

        tv_profit = (TextView) findViewById(R.id.Profit);
        analysis= (Button) findViewById(R.id.Analysis);    //판매 물품 분석 버튼
        analysis.setOnClickListener(clickListener);

        //날짜를 표시할 달력객체 생성,리스너 생성
        compactCalendarView = (CompactCalendarView) findViewById(R.id.compactcalendar_view);
        compactCalendarView.setUseThreeLetterAbbreviation(true);
        Event ev1 = new Event(Color.BLUE,1477054800000L);
        compactCalendarView.addEvent(ev1);
        compactCalendarView.setListener(this);

        //액션바 날짜 조정
        actionBar.setTitle(currentday.get(Calendar.YEAR)+"년" + (currentday.get(Calendar.MONTH)+1)+"월");

        //현재날짜 지정
        selectedDay =dateManager.getDate(currentday.get(Calendar.YEAR),currentday.get(Calendar.MONTH),currentday.get(Calendar.DATE));
        curDay = selectedDay;
        //서버에서 판매 데이터 받아오기
        getSaleList getdata = new getSaleList(url,posNum);
        getdata.execute();

    }



    //버튼 클릭시 발생 이벤트트
    View.OnClickListener clickListener = new View.OnClickListener() { //버튼의 리스너
        @Override
        public void onClick(View v) {
            rank = saleDataConvert.getRanks(curDay);
            Intent intent = new Intent(SaleActivity.this, AnalysisActivity.class);
            startActivity(intent);
        }
    };


    @Override
    public void onDayClick(Date dateClicked) {  //달력에서 날짜가 눌려졌을 경우 (선택한 날짜의 매출 총액을 보여준다.)

        Context conext= getApplicationContext();
        selectedDay=dateManager.getformatter().format(dateClicked); //선택된 날짜 저장
        tv_profit.setText(saleDataConvert.searchData(selectedDay));

    }


    @Override
    public void onMonthScroll(Date firstDayOfNewMonth) {  //달력을 넘길때 발생하는 이벤트

        String Year=firstDayOfNewMonth.toString();
        Year= Year.substring(Year.length()-4,Year.length());
        actionBar.setTitle(Year+"년" + (firstDayOfNewMonth.getMonth()+1)+"월");
        selectedDay =dateManager.getDate(currentday.get(Calendar.YEAR),(firstDayOfNewMonth.getMonth()),1);

    }

    //판매 데이터 가져오는 스레드
    private class getSaleList extends AsyncTask<Void,Void,String> {

        private String url;
        private int posNum;
        private ProgressDialog nDialog;

        public getSaleList(String url,int posNum){
            this.url=url;
            this.posNum=posNum;
        }

        @Override
        protected void onPreExecute() {
            super.onPreExecute();
            nDialog = new ProgressDialog(SaleActivity.this); //Here I get an error: The constructor ProgressDialog(PFragment) is undefined
            nDialog.setMessage("Loading..");
            nDialog.setTitle("정보를 받아오고 있습니다.");
            nDialog.setIndeterminate(false);
            nDialog.setCancelable(true);
            nDialog.show();
        }


        @Override
        protected String doInBackground(Void... voids) {

            String result;
            SaleRequest saleRequest =new SaleRequest();
            result = saleRequest.request(url,posNum);
            result=result.trim();
            return result;

        }

        @Override
        protected void onPostExecute(String s) {
            //문자 스트림을 json 데이터로 변환
            saleDataConvert = new SaleDataConvert();
            saleDataConvert.getData(s);

            if (nDialog  != null && nDialog .isShowing()) {
                nDialog .dismiss();
            }

            tv_profit.setText(saleDataConvert.searchData(selectedDay));

        }
    }
}
