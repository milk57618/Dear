<?
// 로그아웃 했을 때 존재하는 모든 세션을 없애주는 창
session_start();
session_destroy();
if(isset($_SESSION['id'])){
	echo "<script>alert('로그아웃 완료!')</script>";
	echo "<script>top.parent.document.location.reload();</script>";
} else
	echo "<script>alert('로그인 되어있지 않은 상태입니다!')</script>";
echo ("<script>location.replace('login.php');</script>");
?>