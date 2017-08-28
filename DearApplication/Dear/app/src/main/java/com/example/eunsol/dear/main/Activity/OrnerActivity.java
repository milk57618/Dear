package com.example.eunsol.dear.main.Activity;

import android.app.Activity;
import android.content.Intent;
import android.os.Bundle;
import android.support.annotation.Nullable;
import android.view.View;
import android.widget.Button;
import android.widget.TextView;
import com.example.eunsol.dear.R;

/**
 * Created by TAEYONG on 2017-07-21.
 */

public class OrnerActivity extends Activity implements View.OnClickListener{

    Button saleBtn,employeeBtn;
    TextView storeNameText;
    String userID;
    String storeName;
    int posNum;

    @Override
    protected void onCreate(@Nullable Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);

        setContentView(R.layout.activity_orner);

        //앞의 엑티비티로부터 보낸 값을 얻는다.
        Intent intent = getIntent();
        posNum=intent.getIntExtra("posNum",-1);
        storeName=intent.getStringExtra("storeName");

        saleBtn = (Button) findViewById(R.id.salesButton);
        employeeBtn = (Button) findViewById(R.id.employeeButton);
        storeNameText= (TextView) findViewById(R.id.textStore);
        //리스너 연결
        saleBtn.setOnClickListener(this);
        employeeBtn.setOnClickListener(this);
        storeNameText.setText(storeName);
    }

    //각 버튼 클릭 이벤트
    @Override
    public void onClick(View view) {

        Intent intent;

        switch (view.getId()){
            //판매 조회
            case R.id.salesButton:

                intent = new Intent(OrnerActivity.this,SaleActivity.class);
                intent.putExtra("posNum",posNum);
                OrnerActivity.this.startActivity(intent);
                break;

            case R.id.employeeButton:

                intent = new Intent(OrnerActivity.this,MemberActivity.class);
                intent.putExtra("posNum",posNum);
                OrnerActivity.this.startActivity(intent);
                break;


        }

    }
}
