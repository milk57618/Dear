<?
// 가게 정보 수정 창
session_start();
if(isset($_SESSION["id"]) && ($_SESSION["type"] == "seller" || $_SESSION["type"] == "admin")) {
$id = $_SESSION["id"];
if (mysqli_connect_errno ( $conn = mysqli_connect ( "localhost", "root", "root", "pos" ) )) {
	echo "DB 연결 실패!";
} else {
	$column_data = array ();
	$ret = mysqli_query ( $conn, "select POSId, UserId, StoreName, StoreAddress from seller where UserId = '$id'" );
	while ( $rows = mysqli_fetch_array ( $ret ) ) {
		$column_data [] = $rows [0];
		$column_data [] = $rows [1];
		$column_data [] = $rows [2];
		$column_data [] = $rows [3];
	}
}
?>
<!DOCTYPE html>
<html lang="ko">
<head>
<meta charset="UTF-8">
<title>가게 정보 수정</title>
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
		<form action="action_changeDepartmentInfo.php" method="POST">
			<table>
				<tr>
					<td colspan="2">
						<div style="padding: 0px 0px 0px 154px;">
							<h3>가게 정보 수정</h3>
						</div>
					</td>
				</tr>
				<tr>
					<td>포스기 번호 </td>
					<td><input type="text" name="posID" value="<?echo $column_data[0]?>" readonly="" size="39" style="padding-left: 5px;"/></td>
				</tr>
				<tr>
					<td>사용자 ID </td>
					<td><input type="text" value="<?echo $column_data[1]?>" readonly="" size="39" style="padding-left: 5px;"/></td>
				</tr>
				<tr>
					<td>가게 이름 </td>
					<td><input type="text" name="storeName" value="<?echo $column_data[2]?>" size="39" style="padding-left: 5px;"/></td>
				</tr>
				<tr>
					<td>가게 주소 </td>
					<td><input type="text" name="storeAddress" value="<?echo $column_data[3]?>" size="39" style="padding-left: 5px;"/></td>
				</tr>
				<tr>
					<td></td>
					<td style="padding: 0px 0px 0px 234px"><input type="submit"
						style="width: 70pt; height: 25pt;" value="정보 수정" /></td>
				</tr>
				<tr></tr>
			</table>
		</form>
	</div>
</body>
</html>
<?} else{
echo "사업자만 접근할 수 있는 페이지입니다.";
}?>