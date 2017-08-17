<?
// 포스기 번호를 json으로 받아서 물품들 정보를 json으로 반환해주는 창
class Product{
   var $Id = 0;
   var $Category = "";
   var $Name = "";
   var $Price = 0;

   function __construct($a, $b, $c, $d){
      $this -> Id = $a;
      $this -> Category = $b;
      $this -> Name = $c;
      $this -> Price = $d;
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
	   $product = array ();
	   $ret = mysqli_query ( $conn, "select Id, Category, Name, Price from Product where PosId = $PosId" );
	   for ($i = 0; $rows = mysqli_fetch_array ( $ret ); $i++) {
	      $product [] = new Product($rows[0], $rows[1], $rows[2], $rows[3]);
	   }
	}
	echo "{\"Product\":" . json_encode($product, JSON_UNESCAPED_UNICODE) . "}";

}
?>