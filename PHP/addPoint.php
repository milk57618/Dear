<?
// 고객용 포인트 충전 창
session_start();
if(isset($_SESSION["id"]) && $_SESSION["type"] == "customer") {
?>
<!DOCTYPE html>
<html lang="ko">
<head>
<meta charset="UTF-8">
<title>포인트 충전</title>
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
		<form action="action_addPoint.php" method="POST">
			<table>
				<tr>
					<td colspan="2">
						<div style="padding: 0px 0px 0px 50px;">
							<h3>포인트 충전</h3>
						</div>
					</td>
				</tr>
				<tr>
					<td>금액</td>
					<td><input type="text" name="money" /></td>
				</tr>
				<tr>
					<td></td>
					<td style="padding: 0px 0px 0px 120px"><input type="submit"
						style="width: 45pt; height: 22pt;" value="충전" /></td>
				</tr>
				<tr></tr>
			</table>
		</form>
	</div>
</body>
</html>
<?} else{
echo "고객만 접근할 수 있는 페이지입니다.";
}?>