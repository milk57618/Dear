<?
// 가게 정보 수정을 요청받아서 처리하는 창
if (mysqli_connect_errno ( $conn = mysqli_connect ( "localhost", "root", "root", "pos" ) )) {
	echo ("DB 연결 실패!");
} else {
	$posID = ($_POST ['posID']);
	$storeName = ($_POST ['storeName']);
	$storeAddress = ($_POST ['storeAddress']);
	
	if ($storeName == "") {
		echo "<script>alert('가게 이름을 입력해주세요.')</script>";
	} else if ($storeAddress == "") {
		echo "<script>alert('가게 주소를 입력해주세요.')</script>";
	} else {
		
		mysqli_query ( $conn, "update seller set storeName='$storeName', storeAddress='$storeAddress' where posID = $posID" );
		mysqli_close ( $conn );
		echo "<script>alert('수정 완료!!')</script>";
	}
	echo ("<script>location.replace('changeDepartmentInfo.php');</script>");
}
?>