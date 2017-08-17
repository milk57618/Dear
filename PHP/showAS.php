<?
// AS 문의 내용들을 관리자에게 보여주는 창
session_start();
if(isset($_SESSION["id"]) && $_SESSION["id"] == "admin") {
if (mysqli_connect_errno ( $conn = mysqli_connect ( "localhost", "root", "root", "pos" ) )) {
	echo "DB 연결 실패!";
} else {
	$column_data = array ();
	$ret = mysqli_query ( $conn, "select Time, POSId, Text from AfterService" );
	while ( $rows = mysqli_fetch_array ( $ret ) ) {
		$column_data [] = $rows [0];
		$column_data [] = $rows [1];
		$column_data [] = $rows [2];
	}
}
?>

<style>
table {
	border: 1px solid black;
	border-collapse: collapse;
}

h, th, td, tr, input, textarea, select, FORM {
	font-family: 맑은 고딕;
	font-size: 1em;
	border: 1px solid  black;
	padding: 5px;
}

thead {
	background-color: #dedeee;
	text-algin: center;
}
</style>

<body>
	<table border="1">
		<thead>
			<tr>
				<td align=center width='23%'>문의 시간</td>
				<td align=center width='15%'>포스기 번호</td>
				<td align=center width='62%'>내용</td>
			</tr>
		</thead>
		<tbody>
			<tr>
		<?
		for($j = 1; $j <= count ( $column_data ); $j ++) {
			?>
			<td align=center><?echo $column_data[$j-1] ?></td>
			<?
			if ($j % 3 == 0) {
				?>
			</tr>
			<tr>
			<?}}?>
			</tr>
		</tbody>
	</table>
</body>
<?} else{
echo "관리자만 접근할 수 있는 페이지입니다.";
}?>