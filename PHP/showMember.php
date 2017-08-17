<?
// 포스기 번호를 json으로 받아서 직원들 정보를 json으로 반환해주는 창
class Member{
   var $Name = "";
   var $Position = "";
   var $PhoneNumber = "";
   var $WageH = 0; // 시급
   var $Pay = 0; // 임금

   function __construct($a, $b, $c, $d, $e){
      $this -> Name = $a;
      $this -> Position = $b;
      $this -> PhoneNumber = $c;
      $this -> WageH = $d;
      $this -> Pay = $e;
   }
}

$json = json_decode(file_get_contents('php://input')); 

if(!isset($json->{"PosId"})) {
	echo "비정상적인 접근입니다.";
} else {
	$PosID = $json->{"PosId"};
	$Date = $json->{"Date"};
	if (mysqli_connect_errno ( $conn = mysqli_connect ( "localhost", "root", "root", "pos" ) )) {
	   echo "DB 연결 실패!";
	} else {
	   $member= array ();
	   $ret = mysqli_query ( $conn, "select t.name as name, m.position as position, m.phonenumber as phoneNumber, m.price as price, m.price*sum(t.worktime)/60 as wageH from membertime t, member m where m.posid = $PosID and m.name = t.name and Date='$Date' group by t.name" );
	   for ($i = 0; $rows = mysqli_fetch_array ( $ret ); $i++) {
	      $member [] = new Member($rows[0], $rows[1], $rows[2], $rows[3], $rows[4]);
	   }
	}
	echo "{\"Member\":" . json_encode($member, JSON_UNESCAPED_UNICODE) . "}";
}
?>