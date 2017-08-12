<?
// 포인트 충전을 요청받아서 처리하는 창 -> 결제 모듈까진 만들기 힘들어서 안만듦
session_start();
if (mysqli_connect_errno ( $conn = mysqli_connect ( "localhost", "root", "root", "pos" ) )) {
	echo ("DB 연결 실패!");
} else {
	$Money = ( int ) $_POST ["money"];
	
	if ($Money == "" || ! is_int ( $Money ) || $Money < 1){
		echo "<script>alert('충전할 금액을 올바르게 입력해주세요.')</script>";
	} else {
		mysqli_query ( $conn, "update customer set Money = Money + " . $Money . " where UserId = '" . $_SESSION["id"] . "'");
		mysqli_close ( $conn );
		echo "<script>alert('충전 완료!')</script>";
		echo ("<script>location.replace('showPoint.php');</script>");
	}
}
echo ("<script>location.replace('addPoint.php');</script>");
?>