$(document).ready(function(){
	let flag=false;
	$('#cat').click(function() {
		if (!flag) {
 		$("#cats_hdn").fadeIn(300);
 		flag=true;
 		$('#arrowDown').toggleClass('hdn');
 		$('#arrowUp').toggleClass('hdn');
 		}
 		else{
 			$("#cats_hdn").fadeOut(300);
 			flag=false;
 			$('#arrowDown').toggleClass('hdn');
 			$('#arrowUp').toggleClass('hdn');
 		}
		});
})