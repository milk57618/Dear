<?
// 로그인 창에서 id, pw를 받아서 처리한 후 세션을 부여하는 창
session_start();
if (mysqli_connect_errno ( $conn = mysqli_connect ( "localhost", "root", "root", "pos" ) )) {
	echo ("DB 연결 실패!");
} else {
	$id = ($_POST ['id']);
	$pw = ($_POST ['pw']);
	
	$ret_seller = mysqli_query ( $conn, "select UserId from seller where UserId = '$id' and UserPw = HEX(AES_ENCRYPT('$pw', 'secret'))" );
	$ret_customer = mysqli_query ( $conn, "select UserId from customer where UserId = '$id' and UserPw = HEX(AES_ENCRYPT('$pw', 'secret'))" );
	mysqli_close ( $conn );
	if ( $rows = mysqli_fetch_array ( $ret_seller ) ) { // 사업자용 로그인
		$_SESSION["id"] = $id;
		if($id=="admin") 
			$_SESSION["type"] = "admin";
		else
			$_SESSION["type"] = "seller";
		echo "<script>alert('로그인 성공!')</script>";
		echo "<script>top.parent.document.location.reload();</script>";
		echo "<script>location.replace('login.php');</script>";
	} else if ( $rows = mysqli_fetch_array ( $ret_customer ) ) { // 고객용 로그인
		$_SESSION["id"] = $id;
		if($id=="admin") 
			$_SESSION["type"] = "admin";
		else
			$_SESSION["type"] = "customer";
		echo "<script>alert('로그인 성공!')</script>";
		echo "<script>top.parent.document.location.reload();</script>";
		echo "<script>location.replace('login.php');</script>";
	}
	else { // 로그인 실패
		echo "<script>alert('아이디 또는 비밀번호가 일치하지 않습니다.')</script>";
	}
}
echo "<script>location.replace('login.php');</script>";
?>