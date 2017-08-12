<?
// 회원가입 창
?>
<!DOCTYPE html>
<html lang="ko">
<head>
<meta charset="UTF-8">
<title>회원가입</title>
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
		<form action="action_sign.php" method="POST">
			<table>
				<tr>
					<td colspan="2">
						<div style="padding: 0px 0px 0px 35px;">
							<h3>회원가입</h3>
						</div>
					</td>
				</tr>
				<tr>
					<td style="padding: 0px 0px 0px 10px"><input type="radio" name="accountType" value="customer" checked/></td>
					<td style="padding: 0px 0px 0px 5px">손님용 계정</td>
				</tr>	
				<tr>
					<td style="padding: 0px 0px 0px 10px"><input type="radio" name="accountType" value="seller" /></td>
					<td style="padding: 0px 0px 0px 5px">사업자용 계정</td>
				</tr>
				<tr>
					<td></td>
					<td style="padding: 0px 0px 0px 50px"><input type="submit"
						style="width: 45pt; height: 22pt;" value="다음" /></td>
				</tr>
				<tr></tr>
			</table>
		</form>
	</div>
</body>
</html>