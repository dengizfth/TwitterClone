$('.btn-send-comment').click(function () {
    var val = $(this).siblings('.form-comment-text').val();
    if (val.length == 0) {
        alert('Yorum boş geçilemez !')
        return false;
    }
});

$('.show-comment-area').click(function () {
    var parentTweet = $(this).closest('.media-body');
    parentTweet.find('.comment-area').toggleClass('hide');
});