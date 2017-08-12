<?
// 자신이 갖고 있는 포인트를 보여주는 창
session_start();
if(isset($_SESSION["id"]) && $_SESSION["type"] == "customer") {
if (mysqli_connect_errno ( $conn = mysqli_connect ( "localhost", "root", "root", "pos" ) )) {
	echo "DB 연결 실패!";
} else {
	$id = $_SESSION["id"];
	$column_data = array ();
	$ret = mysqli_query ( $conn, "select Money from customer where UserId = '$id'" );
	$point = mysqli_fetch_array ( $ret ) [0];
}
?>

<style>
body {
	font-family: 맑은 고딕;
	font-size: 1em;
	padding: 3px;
}
</style>

<body>
	<? echo "$id 님의 현재 포인트 : $point ";?>
</body>
<?} else{
echo "고객만 접근할 수 있는 페이지입니다.";
}?>