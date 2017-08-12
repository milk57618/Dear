<?
// 포스기에서 AS문의를 했을 때 처리해주는 창
if (mysqli_connect_errno ( $conn = mysqli_connect ( "localhost", "root", "root", "pos" ) )) {
	echo json_encode(-2);
} else {
	$json = json_decode(file_get_contents('php://input')); 

	$Time = $json->{"Time"};
	$PosId = $json->{"PosId"};
	$Text = $json->{"Text"};
		
	mysqli_query ( $conn, "insert into AfterService(Time, PosId, Text) values('$Time', $PosId, '$Text')" );
	mysqli_close ( $conn );

	header("Content-type: application/json"); 
	echo json_encode(1);
}
?>