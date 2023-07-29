$(function () {

    //Change photo in Detail
    $(document).on("click", "#detail .left-img .down-img .item", function () {
        let photo = $(this).children().eq(0).attr("src")
        $(".mainImg").attr("src", photo)

    })











})