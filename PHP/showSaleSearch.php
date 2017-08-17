<?
// 포스기 번호를 json으로 받아서 판매조회 정보를 json으로 반환해주는 창
class SaleSearch{
	var $SaleCount = 0;
	var $Price = 0;
	var $Cash = 0;
	var $Card = 0;
	var $Id = 0;
	var $SaleTime = "";
	var $Finished = 0;


	function __construct($a, $b, $c, $d, $e, $f, $g){
		$this -> SaleCount = $a;
		$this -> Price = $b;
		$this -> Cash = $c;
		$this -> Card = $d;
		$this -> Id = $e;
		$this -> SaleTime = $f;
		$this -> Finished = $g;
	}
}

$json = json_decode(file_get_contents('php://input')); 

if(!isset($json->{"PosId"})) {
	echo "비정상적인 접근입니다.";
} else {
	$PosId = $json->{"PosId"};
	if (mysqli_connect_errno ( $conn = mysqli_connect ( "localhost", "root", "root", "pos" ) )) {
		echo "DB 연결 실패!";
	} else {
		$saleSearch = array ();
		$ret = mysqli_query ( $conn, "select SaleCount, Price, Cash, Card, Id, SaleTime, Finished from SaleSearch where PosId = $PosId" );
		while ($rows = mysqli_fetch_array ( $ret )) {
			$saleSearch [] = new SaleSearch($rows[0], $rows[1], $rows[2], $rows[3], $rows[4], $rows[5], $rows[6]);
		}
	}
	echo "{\"SaleSearch\":" . json_encode($saleSearch, JSON_UNESCAPED_UNICODE) . "}";
}
?>