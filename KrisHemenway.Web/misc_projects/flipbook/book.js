(function($) {
	$.fn.book = function(params) {
	
		// Set Default Parameters
		params = $.extend({turn_speed: 500, page_width: 400, page_height: 441, add_arrows: true, start_open: false, open_in_modal: true}, params);
		
		this.each(function() {
			var book = $(this);
			
			if(params.open_in_modal) {
				book.dialog({
					dialogClass: "flipbook", 
					modal: true, 
					width: params.page_width * 2,
					height: params.page_height,
					draggable: false,
					resizable: false,
					position: "center",
					autoOpen: params.start_open
				});
			}
			
			var pages = $(this).children(".page").wrapInner('<div class="page-wrapper" />');
			
			var first_page = pages.first();
			first_page.css({'width': params.page_width, 'left': params.page_width});
			
			var max_z_index = pages.length;
			var min_z_index = 0;
			
			if(params.open_in_modal)
			{
				var dialog_z_index = parseInt(book.parent(".ui-dialog").css("z-index"));
				max_z_index += dialog_z_index;
				min_z_index = dialog_z_index + 1;
			}
			
			if(params.add_arrows)
			{
				var arrow_index = max_z_index + 2;
				
				var left_arrow = $('<button alt="Previous Page" title="Previous Page" class="left-arrow arrow">&lt;</a>').prependTo($(this)).hide();
				var right_arrow = $('<button alt="Next Page" title="Next Page" class="right-arrow arrow">&gt;</a>').appendTo($(this));
				
				left_arrow.css("z-index", arrow_index);
				right_arrow.css("z-index", arrow_index);
			}
			
			pages.each(function(index) {
				$(this).css({'z-index': max_z_index - index, 'left': params.page_width}).addClass("right-side");
			});
			
			if(params.open_in_modal) {
				$(window).resize(function() {
					book.dialog({position: "center"});
				});
			}
			
			function FlipLeft() {
				// global locking for pages that are flipping
				if($(".page-flipping").length == 0) {
					var left_side_pages = book.find(".left-side");
					var right_side_pages = book.find(".right-side");
					var flip_page = book.find(".left-side").last().addClass("page-flipping");
					var next_page = flip_page.prev(".page").addClass("page-flipping");
					
					var zindexes = right_side_pages.map(function() { return parseInt($(this).css("z-index")); }).toArray();
					var highest_index = (zindexes.length == 0) ? min_z_index : zindexes.max();
					var flip_page_z = ++highest_index;
					var next_page_z = ++highest_index;
					
					// If empty -- we will remove both of these left-side pages in a little bit.
					if(params.add_arrows && left_side_pages.length == 2) {
						left_arrow.hide();
					}

					flip_page.animate(
						{
							"width": 0, 
							"left": params.page_width,
							"background-position": params.page_width * -1
						},
						{
							duration: params.turn_speed / 2, 
							queue: false, 
							easing: "linear", 
							complete: function() {
								$(this).css({"z-index": flip_page_z, "background-position": 0}).addClass("right-side").removeClass("left-side");
								$(this).removeClass("page-flipping");
							}
						}
					);
					
					var flip_page_wrapper = flip_page.find(".page-wrapper");
					flip_page_wrapper.animate({"margin-left": -1 * params.page_width}, { duration: params.turn_speed / 2, queue: false, easing: "linear", complete: function() {
						$(this).css("margin-left", 0);
					}});
					
					var next_page_wrapper = next_page.find(".page-wrapper").css("margin-left", -1 * params.page_width);
					next_page_wrapper.animate({"margin-left": 0}, { duration: params.turn_speed, queue: false, easing: "linear"});
					
					next_page.css({"width": 0, "z-index": 10000});
					next_page.animate(
						{
							"left": params.page_width / 2, 
							"width": params.page_width / 2
						},
						{
							duration: params.turn_speed / 2, 
							queue: false, 
							easing: "linear", 
							complete: function() {
								right_arrow.show();
								$(this).addClass("right-side").removeClass("left-side");
						
								$(this).animate(
									{
										"left": params.page_width, 
										"width": params.page_width
									}, 
									{ 
										duration: params.turn_speed / 2, 
										queue: false, 
										easing: "linear", 
										complete: function() {
											$(this).css("z-index", next_page_z);
											right_side_pages.css("width", params.page_width);
											
											if(params.add_arrows && left_side_pages.length > 2) {
												// If we have arrows & we will end up with no pages left
												left_arrow.show();
											}
											
											$(this).removeClass("page-flipping");
										}
									}
								);
							}
						}
					);
				}
			}
			
			function FlipRight() {
				// global locking for pages that are flipping
				if($(".page-flipping").length == 0) {
					var left_side_pages = book.find(".left-side");
					var right_side_pages = book.find(".right-side");
					var flip_page = right_side_pages.first().addClass("page-flipping");
					var next_page = flip_page.next(".page").addClass("page-flipping").css("left", params.page_width * 2);

					var indexes = left_side_pages.map(function() { return parseInt($(this).css("z-index")); }).toArray();
					var highest_index = (indexes.length == 0) ? min_z_index : indexes.max();
					var flip_page_z = ++highest_index;
					var next_page_z = ++highest_index;

					if(params.add_arrows && right_side_pages.length == 2) {
						// If we have arrows & we will end up with no pages left
						right_arrow.hide();
					}

					flip_page.animate(
						{"width": 0}, 
						{
							duration: params.turn_speed / 2, 
							queue: false, 
							easing: "linear", 
							complete: function() {
								$(this).css({"left": 0, "z-index": flip_page_z}).addClass("left-side").removeClass("right-side");
								$(this).removeClass("page-flipping");
							}
						}
					);
					
					next_page.css({"width": 0});
					next_page.animate(
						{
							"left": params.page_width, 
							"width": params.page_width / 2 
						}, 
						{ 
							duration: params.turn_speed / 2, 
							queue: false, 
							easing: "linear", 
							complete: function() {
								$(this).css("z-index", max_z_index + 1).addClass("left-side").removeClass("right-side");
								left_arrow.show();
								
								$(this).animate(
									{
										"left": 0, 
										"width": params.page_width 
									}, 
									{
										duration: params.turn_speed / 2, 
										queue: false, 
										easing: "linear", 
										complete: function() {
											left_side_pages.css("width", params.page_width);
											$(this).css("z-index", next_page_z);
											
											if(params.add_arrows && right_side_pages.length > 2) {
												right_arrow.show();
											}
									
											$(this).removeClass("page-flipping");
										}
									}
								);
							}
						}
					);
				}
			}
			
			$("body, .ui-dialog").keyup(function(event) {
				switch(event.which) {
					case 39: /* right */
					case 32: /* space */
					case 38: /* up */
						FlipRight();
						break;
					case 40: /* down */
					case 37: /* left */
						FlipLeft();
						break;
				}
				
				return true;
			});
			
			if(params.add_arrows) {
				right_arrow.click( FlipRight );
				left_arrow.click( FlipLeft );
			}
			
			if(params.start_open) {
				FlipRight();
			}
		});
		
		return this;
	}
})(jQuery);