<?
// 비콘 아이디 수정을 요청받아서 처리해주는 창
if (mysqli_connect_errno ( $conn = mysqli_connect ( "localhost", "root", "root", "pos" ) )) {
	echo ("DB 연결 실패!");
} else {
	$posID = ($_POST ['posID']);
	$beaconID = ($_POST ['beaconID']);
	
	if ($posID == "") {
		echo "<script>alert('포스기 번호를 선택해주세요.')</script>";
	} else if ($beaconID == "") {
		echo "<script>alert('비콘 ID를 입력해주세요.')</script>";
	} else {
		$ret = mysqli_query ( $conn, "select posID from seller where beaconID = '$beaconID'" );
		if ( mysqli_fetch_array ( $ret ) ) {
			echo "<script>alert('동일한 비콘 ID를 가진 사업자가 존재합니다.')</script>";
		}
		else {
			mysqli_query ( $conn, "update seller set beaconID = '$beaconID' where posID = $posID" );
			mysqli_close ( $conn );
			echo "<script>alert('입력 완료!')</script>";
		}
	}
	echo ("<script>location.replace('setBeaconID.php');</script>");
}
?>