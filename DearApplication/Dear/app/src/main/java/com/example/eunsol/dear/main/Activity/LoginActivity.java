package com.example.eunsol.dear.main.Activity;
import android.content.Intent;
import android.os.AsyncTask;
import android.support.v7.app.AlertDialog;
import android.support.v7.app.AppCompatActivity;
import android.os.Bundle;
import android.view.View;
import android.widget.Button;
import android.widget.EditText;
import com.example.eunsol.dear.R;
import com.example.eunsol.dear.main.Request.LoginRequest;
import com.example.eunsol.dear.main.jsonConvert.LoginDataConvert;

public class LoginActivity extends AppCompatActivity implements View.OnClickListener {

    private Button loginBtn;
    private EditText idText,pwText;
    private String userId;
    private String userPw;
    private String storeName="";
    private int posNum=-1;
    private final String url = "http://125.183.218.149/POS_action_login.php";


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


    }

    //로그인 버튼 클릭시 발생 이벤트
    @Override
    public void onClick(View view) {

        //입력한 아이디 패스워드 값 받아오기
        userId = idText.getText().toString();
        userPw = pwText.getText().toString();

        //서버에 로그인 정보를 요청한다.
        if(!userId.equals("") && !userPw.equals("")) {
            Network network = new Network(url,userId,userPw);
            network.execute();
        }

    }

    //서버에 정보를 요청하는 스레드

    private class Network extends AsyncTask<Void,Void,String>{

        private String url;
        private String userId;
        private String userPw;

        public Network(String url,String Id,String Pw){
            this.url=url;
            this.userId=Id;
            this.userPw=Pw;
        }

        @Override
        protected String doInBackground(Void... voids) {

            String result;
            LoginRequest loginRequest = new LoginRequest();
            result = loginRequest.request(url,userId,userPw);

            if (result!=null)
            result=result.trim();

            return result;

        }

        @Override
        protected void onPostExecute(String s) {
            super.onPostExecute(s);

            LoginDataConvert loginDataConvert= new LoginDataConvert();
            loginDataConvert.getData(s);
            //json 데이터로부터 value 값을 얻어온다.
            posNum=loginDataConvert.getPosId();
            storeName=loginDataConvert.getStoreName();

            if(posNum==-1){
                //로그인 실패 시
                AlertDialog.Builder builder = new AlertDialog.Builder(LoginActivity.this);
                builder.setMessage("로그인에 실패하였습니다.")
                        .setNegativeButton("다시시도", null)
                        .create()
                        .show();
            }else{
                //로그인 성공시 다음 화면으로 넘김
                Intent intent = new Intent(LoginActivity.this, OrnerActivity.class);
                intent.putExtra("userId", userId);
                intent.putExtra("userPw", userPw);
                intent.putExtra("posNum",posNum);
                intent.putExtra("storeName",storeName);
                LoginActivity.this.startActivity(intent);
            }

        }
    }
}
