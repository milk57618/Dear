package com.example.eunsol.dear.main.Activity;

import android.content.Intent;
import android.os.AsyncTask;
import android.support.annotation.IdRes;
import android.support.v7.app.ActionBar;
import android.support.v7.app.AlertDialog;
import android.support.v7.app.AppCompatActivity;
import android.os.Bundle;
import android.util.Log;
import android.view.View;
import android.widget.Button;
import android.widget.EditText;
import android.widget.RadioButton;
import android.widget.RadioGroup;
import com.example.eunsol.dear.R;
import com.example.eunsol.dear.main.Request.ClientLoginRequest;
import com.example.eunsol.dear.main.Request.OrnerLoginRequest;
import com.example.eunsol.dear.main.jsonConvert.ClientLoginConvert;
import com.example.eunsol.dear.main.jsonConvert.OrnerLoginDataConvert;

import java.util.regex.Pattern;


//폰트 설정
/*Copyright (c) 2015 Hien Ngo

        Licensed under the Apache License, Version 2.0 (the "License");
        you may not use this file except in compliance with the License.
        You may obtain a copy of the License at

        http://www.apache.org/licenses/LICENSE-2.0

        Unless required by applicable law or agreed to in writing, software
        distributed under the License is distributed on an "AS IS" BASIS,
        WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
        See the License for the specific language governing permissions and
        limitations under the License.  */

public class LoginActivity extends AppCompatActivity implements View.OnClickListener {

    private Button loginBtn;
    private EditText idText,pwText;
    private String userId;
    private String userPw;
    private String userGroup;
    private int userGroupID;
    private int radiobtn = 1;
    private final String ornerurl = "http://125.183.218.149/POS_action_login.php";
    private final String clienturl = "http://125.183.218.149/POS_action_login_c.php";

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_main);
        System.setProperty("http.keepalive","false");

        //레이아웃 객체 등록
        loginBtn = (Button) findViewById(R.id.loginButton);
        idText = (EditText) findViewById(R.id.idText);
        pwText = (EditText) findViewById(R.id.passwordText);
        loginBtn.setOnClickListener(this);

        RadioGroup rg = (RadioGroup) findViewById(R.id.rg);
        userGroupID = rg.getCheckedRadioButtonId();
        userGroup = ((RadioButton) findViewById(userGroupID)).getText().toString();

        RadioButton rd1 = (RadioButton) findViewById(R.id.memrber);
        final RadioButton rd2 = (RadioButton) findViewById(R.id.storemember);

        rg.setOnCheckedChangeListener(new RadioGroup.OnCheckedChangeListener() {

            @Override
            public void onCheckedChanged(RadioGroup group, @IdRes int checkedId) {
                RadioButton groupButton = (RadioButton) findViewById(checkedId);
                userGroup = groupButton.getText().toString();
            }
        });

        //액션바 숨기기기
        hideActionBar();
    }

    //로그인 버튼 클릭시 발생 이벤트
    @Override
    public void onClick(View view) {

        //입력한 아이디 패스워드 값 받아오기
        userId = idText.getText().toString();
        userPw = pwText.getText().toString();

        switch (userGroup) {

            case "일반고객" :

                           radiobtn=1;
                            //서버에 로그인 정보를 요청한다.
                            if (!userId.equals("") && !userPw.equals("")&& radiobtn == 1) {
                                Network network = new Network(clienturl, userId, userPw);
                                network.execute();
                             }
                            break;

            case "사업자"  :

                           radiobtn=2;
                            if (!userId.equals("") && !userPw.equals("")&& radiobtn == 2) {
                                Network network = new Network(ornerurl, userId, userPw);
                                network.execute();
                            }

                            break;

        }

    }

    //서버에 정보를 요청하는 스레드

    private class Network extends AsyncTask<Void,Void,String>{

        private String url;
        private String userId;
        private String userPw;
        private String storeName="";  //가게주인 로그인일 경우
        private int posNum=-1;   //가게 주인 로그인일 경우
        private int userMoney=-1; //회원 로그인일 경우

        public Network(String url,String Id,String Pw){
            this.url=url;
            this.userId=Id;
            this.userPw=Pw;
        }

        @Override
        protected String doInBackground(Void... voids) {

            String result;

            if(radiobtn==2) {
                OrnerLoginRequest ornerLoginRequest = new OrnerLoginRequest();
                result = ornerLoginRequest.request(url, userId, userPw);
            }
            else {
                ClientLoginRequest clientLoginRequest =new ClientLoginRequest();
                result = clientLoginRequest.request(url,userId,userPw);
            }

            if (result!=null)
               result=result.trim();

            //네트워크 연결 안됨
            else
             return  null;


            return result;

        }

        @Override
        protected void onPostExecute(String s) {
            super.onPostExecute(s);

            if (s != null) {

                if (radiobtn == 1) {

                    ClientLoginConvert clientLoginConvert =new ClientLoginConvert();
                    clientLoginConvert.getData(s);
                    userMoney=clientLoginConvert.getClientMoney();

                    if (userMoney==-1) {
                        //로그인 실패 시
                        AlertDialog.Builder builder = new AlertDialog.Builder(LoginActivity.this);
                        builder.setMessage("로그인에 실패하였습니다.")
                                .setNegativeButton("다시시도", null)
                                .create()
                                .show();
                    } else {
                        //로그인 성공시 다음 화면으로 넘김
                        Intent intent = new Intent(LoginActivity.this, ClientActivity.class);
                        intent.putExtra("userId", userId);
                        intent.putExtra("userPw", userPw);
                        intent.putExtra("userMoney",Integer.toString(userMoney));
                        LoginActivity.this.startActivity(intent);
                    }

                } else if (radiobtn == 2) {
                    OrnerLoginDataConvert ornerLoginDataConvert = new OrnerLoginDataConvert();
                    ornerLoginDataConvert.getData(s);
                    //json 데이터로부터 value 값을 얻어온다.
                    posNum = ornerLoginDataConvert.getPosId();
                    storeName = ornerLoginDataConvert.getStoreName();

                    if (posNum == -1) {
                        //로그인 실패 시
                        AlertDialog.Builder builder = new AlertDialog.Builder(LoginActivity.this);
                        builder.setMessage("로그인에 실패하였습니다.")
                                .setNegativeButton("다시시도", null)
                                .create()
                                .show();
                    } else {

                        //로그인 성공시 다음 화면으로 넘김
                        Intent intent = new Intent(LoginActivity.this, OrnerActivity.class);
                        intent.putExtra("userId", userId);
                        intent.putExtra("userPw", userPw);
                        intent.putExtra("posNum", posNum);
                        intent.putExtra("storeName", storeName);
                        LoginActivity.this.startActivity(intent);
                    }
                }


            }else{

                AlertDialog.Builder builder = new AlertDialog.Builder(LoginActivity.this);
                builder.setMessage("네트워크에 연결할 수 없습니다.")
                        .setNegativeButton("확인", null)
                        .create()
                        .show();
            }
        }
    }

    //액션바 숨기기
    private void hideActionBar() {
        ActionBar actionBar = getSupportActionBar();

        if (actionBar != null) {
            actionBar.hide();
        }
    }

}


