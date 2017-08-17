<?
// 비콘 아이디 설정 창
session_start();
if(isset($_SESSION["id"]) && $_SESSION["id"] == "admin") {
if (mysqli_connect_errno($conn = mysqli_connect("localhost", "root", "root", "pos"))){
	echo "DB 연결 실패!";
} else{
	$column_posID = array();
	$column_userID = array();
	$column_storeName = array();
	$ret = mysqli_query($conn, "select PosID, UserID, storeName from seller order by PosID");
	while( $rows = mysqli_fetch_array($ret)){
		$column_posID[] = $rows[0];
		$column_userID[] = $rows[1];
		$column_storeName[] = $rows[2];
	}
}
?>
<!DOCTYPE html>
<html lang="ko">
<head>
<meta charset="UTF-8">
<title>비콘 ID 설정</title>
<style>
h, td, tr, input, textarea, select, FORM {
	font-family: 맑은 고딕;
	font-size: 1em;
	border-radius: 5px;
}

table {
	border: 1px solid rgba(36, 228, 172, 0.29);
	border-spacing: 10px;
}
</style>
</head>

<body>
	<div style="padding: 0px 0px 0px 10px;">
		<form action="action_setBeaconID.php" method="POST">
			<table>
				<tr>
					<td colspan="2">
						<div style="padding: 0px 0px 0px 80px;">
							<h3>비콘 ID 설정</h3>
						</div>
					</td>
				</tr>
				<tr>
					<td>포스기 번호</td>
					<td>
					<select name="posID" style="width:182px;">
							<option value="">포스기 선택</option>
							<?for($i=0; $i<count($column_posID); $i++){
							?><option value=<?echo $column_posID[$i]?>><?echo $column_posID[$i]," (",$column_userID[$i],", ",$column_storeName[$i],")"?></option>
							<?}?>
					</select>
					</td>
				</tr>
				<tr>
					<td>비콘 ID</td>
					<td><input type="text" name="beaconID"/></td>
				</tr>
				<tr>
					<td></td>
					<td style="padding: 0px 0px 0px 89px"><input type="submit"
						style="width: 70pt; height: 25pt;" value="입력" /></td>
				</tr>
				<tr></tr>
			</table>
		</form>
	</div>
</body>
</html>
<?} else{
echo "관리자만 접근할 수 있는 페이지입니다.";
}?>