<?
// 사업자용 회원가입 창
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
		<form action="action_sign_seller.php" method="POST">
			<table>
				<tr>
					<td colspan="2">
						<div style="padding: 0px 0px 0px 100px;">
							<h3>회원가입</h3>
						</div>
					</td>
				</tr>
				<tr>
					<td>아이디</td>
					<td><input type="text" name="id" /></td>
				</tr>
				<tr>
					<td>비밀번호</td>
					<td><input type="password" name="pw" /></td>
				</tr>
				<tr>
					<td>비밀번호 확인</td>
					<td><input type="password" name="pwCheck" /></td>
				</tr>
				<tr>
					<td>가게이름</td>
					<td><input type="text" name="storeName" /></td>
				</tr>
				<tr>
					<td>가게주소</td>
					<td><input type="text" name="storeAddress" /></td>
				</tr>
				<tr>
					<td></td>
					<td style="padding: 0px 0px 0px 89px"><input type="submit"
						style="width: 70pt; height: 25pt;" value="회원가입" /></td>
				</tr>
				<tr></tr>
			</table>
		</form>
	</div>
</body>
</html>