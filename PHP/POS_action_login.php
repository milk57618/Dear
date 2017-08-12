<?
// 포스기 로그인 요청을 받아서 처리하는 창
if (mysqli_connect_errno ( $conn = mysqli_connect ( "localhost", "root", "root", "pos" ) )) {
	echo json_encode(-2);
} else {
	$json = json_decode(file_get_contents('php://input')); 

   	$id = $json->{"id"};
   	$pw = $json->{"pw"};

	$PosId = -1;
	$StoreName = "error";
	
	$ret = mysqli_query ( $conn, "select PosId, StoreName from seller where UserId = '$id' and UserPw = HEX(AES_ENCRYPT('$pw', 'secret'))" );
	if ( $rows = mysqli_fetch_array ( $ret ) ) {
		$PosId = $rows [0];
		$StoreName = $rows [1];
	}
	mysqli_close ( $conn );

	$data['PosId'] = (int)$PosId;
	$data['StoreName'] = $StoreName;

	header("Content-type: application/json"); 
	echo json_encode($data, JSON_UNESCAPED_UNICODE);
}
?>