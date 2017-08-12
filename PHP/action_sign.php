<?
// 회원가입 버튼 눌렀을 때 계정 종류에 따라 고객용 또는 사업자용 회원가입 페이지로 넘어가주는 창
$type = ($_POST ['accountType']);

switch($type){
	case "customer":
		echo ("<script>location.replace('sign_customer.php');</script>");
		break;
	case "seller":
		echo ("<script>location.replace('sign_seller.php');</script>");
		break;
}
?>