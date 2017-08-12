<?
// 포스기에서 보낸 물품, 판매조회, 직원 등의 정보를 받아서 mysql에 저장해주는 창
if (mysqli_connect_errno ( $conn = mysqli_connect ( "localhost", "root", "root", "pos" ) )) {
	echo json_encode(-2);
} else {
	$json = json_decode(file_get_contents('php://input')); 

	$PosId = $json->{"PosId"};

	// 일단 해당 포스기 번호로 저장된 모든 정보를 삭제함
	mysqli_query ( $conn, "delete from Member where PosId = $PosId" );
	mysqli_query ( $conn, "delete from MemberTime where PosId = $PosId" );
	mysqli_query ( $conn, "delete from Product where PosId = $PosId" );
	mysqli_query ( $conn, "delete from SaleSearch where PosId = $PosId" );

	// 직원 정보 추가
	for($i=0; $i<count($json->{"Member"}); $i++){
		$Id = ( int ) $json->{"Member"}[$i]->Id;
		$Price = ( int ) $json->{"Member"}[$i]->Price;
		$Name = $json->{"Member"}[$i]->Name;
		$PhoneNumber = $json->{"Member"}[$i]->PhoneNumber;
		$StartTime = $json->{"Member"}[$i]->StartTime;
		$FinishTime = $json->{"Member"}[$i]->FinishTime;
		$Position = $json->{"Member"}[$i]->Position;
		
		mysqli_query ( $conn, "insert into Member(posid, id, price, name, phonenumber, starttime, finishtime, position) values($PosId, $Id, $Price, '$Name', '$PhoneNumber', '$StartTime', '$FinishTime', '$Position')" );
	}

	// 직원 일했던 정보 추가
	for($i=0; $i<count($json->{"MemberTime"}); $i++){
		$Id = ( int ) $json->{"MemberTime"}[$i]->Id;
		$Date = $json->{"MemberTime"}[$i]->Date;
		$Name = $json->{"MemberTime"}[$i]->Name;
		$workStart = $json->{"MemberTime"}[$i]->workStart;
		$workFinish = $json->{"MemberTime"}[$i]->workFinish;
		$workTime = $json->{"MemberTime"}[$i]->workTime;
		
		mysqli_query ( $conn, "insert into MemberTime(posid, id, date, name, workstart, workfinish, worktime) values($PosId, $Id, '$Date', '$Name', '$workStart', '$workFinish', '$workTime')" );
	}
	
	// 물품 정보 추가
	for($i=0; $i<count($json->{"Product"}); $i++){
		$Id = ( int ) $json->{"Product"}[$i]->Id;
		$Category = $json->{"Product"}[$i]->Category;
		$Name = $json->{"Product"}[$i]->Name;
		$Price = ( int ) $json->{"Product"}[$i]->Price;
		
		mysqli_query ( $conn, "insert into Product(PosId, Id, Category, Name, Price) values($PosId, $Id, '$Category', '$Name', $Price)" );
	}

	// 판매조회 정보 추가
	for($i=0; $i<count($json->{"SaleSearch"}); $i++){
		$SaleCount = ( int ) $json->{"SaleSearch"}[$i]->SaleCount;
		$Price = ( int ) $json->{"SaleSearch"}[$i]->Price;
		$Cash = ( int ) $json->{"SaleSearch"}[$i]->Cash;
		$Card = ( int ) $json->{"SaleSearch"}[$i]->Card;
		$Id = ( int ) $json->{"SaleSearch"}[$i]->Id;
		$SaleTime = $json->{"SaleSearch"}[$i]->SaleTime;
		$Finished = ( int ) $json->{"SaleSearch"}[$i]->Finished;
		
		mysqli_query ( $conn, "insert into SaleSearch(PosId, SaleCount, Price, Cash, Card, Id, SaleTime, Finished) values($PosId, $SaleCount, $Price, $Cash, $Card, $Id, '$SaleTime', $Finished)" );
	}
	mysqli_close ( $conn );

	header("Content-type: application/json"); 
	echo json_encode(1);
}
?>