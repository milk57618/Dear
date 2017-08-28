package com.example.eunsol.dear.main.Activity;

import android.app.ProgressDialog;
import android.content.Intent;
import android.os.AsyncTask;
import android.os.Build;
import android.os.Bundle;
import android.support.annotation.RequiresApi;
import android.support.v7.app.ActionBar;
import android.support.v7.app.AppCompatActivity;
import android.util.Log;
import android.view.View;
import android.widget.AdapterView;
import android.widget.ArrayAdapter;
import android.widget.Spinner;
import android.widget.TextView;
import java.text.SimpleDateFormat;
import java.util.ArrayList;
import com.example.eunsol.dear.R;
import com.example.eunsol.dear.main.Request.MemberRequest;
import com.example.eunsol.dear.main.jsonConvert.MemberDataConvert;


/**
 * Created by econo110 on 2017-05-31.
 */

public class MemberActivity extends CustomActivity {

    private int posNum;
    private String date;
    private final String url = "http://125.183.218.149/showMember.php";
    private Spinner staffSpinner;
    private TextView posiText;
    private TextView phoneText;
    private TextView wageText;
    private TextView payText;
    private TextView nameText;
    private MemberDataConvert memberDataConvert;
    private ArrayList<String> MemberName;  //스피너 어댑터에 연결할 데이터

    @Override
    protected void onCreate(@org.jetbrains.annotations.Nullable Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_member);
        System.setProperty("http.keepalive","false");

        //서버에 보낼 데이터 생성
        Intent intent = getIntent();
        posNum=intent.getIntExtra("posNum",-1);
        date = new SimpleDateFormat("yyyy-M").format(new java.util.Date());

        //UI 등록
        staffSpinner= (Spinner) findViewById(R.id.staff);
        posiText = (TextView) findViewById(R.id.posi);
        phoneText = (TextView) findViewById(R.id.phone);
        wageText = (TextView) findViewById(R.id.wage);
        payText = (TextView) findViewById(R.id.pay);
        nameText = (TextView) findViewById(R.id.memName);

        final ActionBar actionBar = getSupportActionBar();
        actionBar.setDisplayHomeAsUpEnabled(false);
        actionBar.setTitle("직원 조회");  //액션바 초기화



        staffSpinner.setOnItemSelectedListener(new AdapterView.OnItemSelectedListener() {
            @Override
            public void onItemSelected(AdapterView<?> parent, View view, int position, long id) {

                String phone =memberDataConvert.getMemberList().get(position).getPhone();

                posiText.setText(memberDataConvert.getMemberList().get(position).getPosi());
                phoneText.setText(phone.substring(0,3)+"-"+phone.substring(3,7)+"-"+phone.substring(7,11));
                wageText.setText(Integer.toString(memberDataConvert.getMemberList().get(position).getWage())+"원");
                payText.setText(Integer.toString(memberDataConvert.getMemberList().get(position).getPay())+"원");
                nameText.setText(memberDataConvert.getMemberList().get(position).getName());
            }

            @Override
            public void onNothingSelected(AdapterView<?> parent) {

            }
        });


        MemberActivity.getMemberList getdata = new MemberActivity.getMemberList(url,posNum,date);
        getdata.execute();

    }

    //직원 데이터 가져오는 스레드
    private class getMemberList extends AsyncTask<Void,Void,String> {

        private String url;
        private int posNum;
        private String date;
        private ProgressDialog nDialog;

        public getMemberList(String url,int posNum,String date){
            this.url=url;
            this.posNum=posNum;
            this.date =date;
        }

        @Override
        protected void onPreExecute() {
            super.onPreExecute();
            nDialog = new ProgressDialog(MemberActivity.this); //Here I get an error: The constructor ProgressDialog(PFragment) is undefined
            nDialog.setMessage("Loading..");
            nDialog.setTitle("정보를 받아오고 있습니다.");
            nDialog.setIndeterminate(false);
            nDialog.setCancelable(true);
            nDialog.show();
        }


        @Override
        protected String doInBackground(Void... voids) {

            String result;
            MemberRequest memberRequest =new MemberRequest();
            result = memberRequest.request(url,posNum,date);

            if (result!=null)
            result=result.trim();

            return result;

        }

        @Override
        protected void onPostExecute(String s) {
            //문자 스트림을 json 데이터로 변환
            memberDataConvert = new MemberDataConvert();
            memberDataConvert.getData(s);

            if (nDialog  != null && nDialog .isShowing()) {
                nDialog .dismiss();
            }

            //서버로부터 데이터 받아오는 과정이 완료되면 스피너에 직원이름을 넣는다
            MemberName= new ArrayList<String>();

            for(int i=0;i<memberDataConvert.getMemberList().size();i++){
                MemberName.add(memberDataConvert.getMemberList().get(i).getName());
            }

            ArrayAdapter<String> dataAdapter = new ArrayAdapter<String> (MemberActivity.this, R.layout.custom_spinner,MemberName);
            dataAdapter.setDropDownViewResource(R.layout.custom_spinner);
            staffSpinner.setPrompt("직원을 선택하세요");
            staffSpinner.setAdapter(dataAdapter);

            posiText.setText(memberDataConvert.getMemberList().get(0).getPosi());
            phoneText.setText(memberDataConvert.getMemberList().get(0).getPhone());
            wageText.setText(Integer.toString(memberDataConvert.getMemberList().get(0).getWage())+" 원");
            payText.setText(Integer.toString(memberDataConvert.getMemberList().get(0).getPay())+" 원");

        }
    }
}
