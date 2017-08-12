<?
// 고객용 회원가입 요청을 처리해주는 창
if (mysqli_connect_errno ( $conn = mysqli_connect ( "localhost", "root", "root", "pos" ) )) {
	echo ("DB 연결 실패!");
} else {
	$id = ($_POST ['id']);
	$pw = ($_POST ['pw']);
	$pwCheck = ($_POST ['pwCheck']);
	$storeName = ($_POST ['storeName']);
	$storeAddress = ($_POST ['storeAddress']);
	
	if ($id == "") {
		echo "<script>alert('아이디를 입력해주세요.')</script>";
	} else if ($pw == "") {
		echo "<script>alert('비밀번호를 입력해주세요.')</script>";
	} else if ($pwCheck == "") {
		echo "<script>alert('비밀번호 확인 칸에도 비밀번호를 입력해주세요.')</script>";
	} else if ($pw != $pwCheck) {
		echo "<script>alert('비밀번호가 일치하지 않습니다.')</script>";
	} else if ($storeName == "") {
		echo "<script>alert('가게 이름을 입력해주세요.')</script>";
	} else if ($storeAddress == "") {
		echo "<script>alert('가게 주소를 입력해주세요.')</script>";
	} else {
		$ret = mysqli_query ( $conn, "select * from seller where UserId = '$id'" );
		$ret2 = mysqli_query ( $conn, "select * from customer where UserId = '$id'" );
		if ( mysqli_fetch_array ( $ret ) || mysqli_fetch_array ( $ret2 ) ) {
			echo "<script>alert('동일한 아이디를 가진 회원이 존재합니다.')</script>";
		}
		else {
			mysqli_query ( $conn, "insert into seller(UserId, UserPw, StoreName, StoreAddress) values('$id', HEX(AES_ENCRYPT('$pw', 'secret')), '$storeName', '$storeAddress')" );
			mysqli_close ( $conn );
			echo "<script>alert('가입 완료!')</script>";
		}
		echo ("<script>location.replace('sign.php');</script>");
	}
	echo ("<script>location.replace('sign_seller.php');</script>");
}
?>