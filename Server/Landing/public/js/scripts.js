(function($) {
    "use strict";

    $('body').scrollspy({
        target: '.navbar-fixed-top',
        offset: 60
    });

    $('#topNav').affix({
        offset: {
            top: 200
        }
    });
    
    new WOW().init();
    
    $('a.page-scroll').bind('click', function(event) {
        var $ele = $(this);
        $('html, body').stop().animate({
            scrollTop: ($($ele.attr('href')).offset().top - 60)
        }, 1450, 'easeInOutExpo');
        event.preventDefault();
    });
    
    $('.navbar-collapse ul li a').click(function() {
        /* always close responsive nav after click */
        $('.navbar-toggle:visible').click();
    });

    $('#galleryModal').on('show.bs.modal', function (e) {
       $('#galleryImage').attr("src",$(e.relatedTarget).data("src"));
    });
	
	$("#toggleVideo").click(function(){
		var video = document.getElementById("video-background");
		video.play();
		$(video).fadeIn();
		$(".header-content").fadeOut();
	});
	
	$("#video-background").click(function(){
		var video = document.getElementById("video-background");
		video.pause();
		$(video).fadeOut();
		$(".header-content").fadeIn();
	});

})(jQuery);