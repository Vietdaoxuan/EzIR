fadeout = function() {
    // $('#two').click(function() {
    //     //gets the source of the thumbnail
    //     var source = $(this).attr('style');
    //     $('#imgtwo').fadeOut(function() {
    //         $(this).attr("style", source);
    //         $(this).fadeIn();
    //     });
    // });


    $('.fpts-thumbnail_title').click(function() {
        var source = $(this).attr('style');
        $('.wrapper').fadeOut(function() {
            $(this).attr('style', source);
            $(this).fadeIn();
        });
    });




}