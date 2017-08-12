<?
// 메인 창
session_start();
?>
<!DOCTYPE HTML>
<html lang="kor">
<head>
<meta charset="UTF-8">
<title>Open POS Solution</title>
<link rel="stylesheet" type="text/css" href="style.css" media="all" />
<!--[if IE 7]>
<link rel="stylesheet" type="text/css" href="style/css/ie7.css" media="all" />
<![endif]-->
<!--[if IE 8]>
<link rel="stylesheet" type="text/css" href="style/css/ie8.css" media="all" />
<![endif]-->
<!--[if IE 9]>
<link rel="stylesheet" type="text/css" href="style/css/ie9.css" media="all" />
<![endif]-->
<script type="text/javascript" src="style/js/jquery-1.6.4.min.js"></script>
<script type="text/javascript" src="style/js/ddsmoothmenu.js"></script>
<script type="text/javascript" src="style/js/jquery.jcarousel.js"></script>
<script type="text/javascript" src="style/js/jquery.prettyPhoto.js"></script>
<script type="text/javascript" src="style/js/carousel.js"></script>
<script type="text/javascript" src="style/js/jquery.flexslider-min.js"></script>
<script type="text/javascript" src="style/js/jquery.masonry.min.js"></script>
<script type="text/javascript" src="style/js/jquery.slickforms.js"></script>
</head>
<body>
<!-- Begin Wrapper -->
<div id="wrapper"style="height:800px; border: 2px solid #bcbcbc;">
	<!-- Begin Sidebar -->
	<div id="sidebar">
		 <div id="logo"><a href="index.php"><img src="style/images/logo.png" alt="POS" /></a></div>
	<!-- Begin Menu -->
    <div id="menu" class="menu-v">
      <ul>
        <li><a href="showPoint.php" target="body_frame">고객 메뉴</a>
        	<ul>
        		<li><a href="showPoint.php" target="body_frame">포인트 확인</a></li>
        		<li><a href="addPoint.php" target="body_frame">포인트 충전</a></li>
        	</ul>
	</li>
	<li><a href="showMyDepartment.php" target="body_frame">사업자 메뉴</a>
        	<ul>
        		<li><a href="showMyDepartment.php" target="body_frame">가게 정보</a></li>
        		<li><a href="changeDepartmentInfo.php" target="body_frame">가게 정보 수정</a></li>
        	</ul>
	</li>
	<?if(isset($_SESSION["id"]) && $_SESSION["type"] == "admin") {?>
        <li><a href="showDepartment.php" target="body_frame" class="active">관리자 메뉴</a>
        	<ul>
        		<li><a href="showDepartment.php" target="body_frame">가게 조회</a></li>
        		<li><a href="showAS.php" target="body_frame">문의 내역</a></li>
        		<li><a href="setBeaconID.php" target="body_frame">비콘 ID 설정</a></li>
        	</ul>
        </li>
	<?}?>
      </ul>
    </div>
    <!-- End Menu -->
	</div>
	<!-- End Sidebar -->
	
	<!-- Begin Content -->
	<div id="content" style="height:600px;">
	<div style="text-align:right">
	<?if(!isset($_SESSION["id"])){?>
	<a href="login.php" target="body_frame">로그인</a>
	<?}else{?>
	<a href="action_logout.php" target="body_frame">로그아웃</a>
	<?}?>
	&nbsp;
	<a href="sign.php" target="body_frame">회원가입</a>
	&nbsp;&nbsp;<br><br>
	</div>
	<?//<h1 class="title">&nbsp;POS</h1>?>
	<div class="line"></div>
	<iframe width="100%" height="100%" name="body_frame" src="login.php"> </iframe>
</div>
<!-- End Wrapper -->
</div>
<div class="clear"></div>
<script type="text/javascript" src="style/js/scripts.js"></script>
<!--[if !IE]> -->
<script type="text/javascript" src="style/js/jquery.corner.js"></script>
<!-- <![endif]-->
</body>
</html>