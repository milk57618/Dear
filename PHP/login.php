<?
// 로그인 창
session_start()
?>
<!DOCTYPE html>
<html lang="ko">
<head>
<meta charset="UTF-8">
<title>로그인</title>
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
	<?if(isset($_SESSION['id'])){
		echo $_SESSION['id'] . " 님, 반갑습니다.";
	} else{ ?>
	<div style="padding: 0px 0px 0px 10px;">
		<form action="action_login.php" method="POST">
			<table>
				<tr>
					<td colspan="2">
						<div style="padding: 0px 0px 0px 90px;">
							<h3>로그인</h3>
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
					<td></td>
					<td style="padding: 0px 0px 0px 102px"><input type="submit"
						style="width: 50pt; height: 20pt;" value="로그인" /></td>
				</tr>
				<tr></tr>
			</table>
		</form>
	</div>
	<?}?>
</body>
</html>