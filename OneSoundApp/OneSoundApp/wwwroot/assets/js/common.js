$(function () {

    //Change photo in Detail
    $(document).on("click", "#detail .left-img .down-img .item", function () {
        let photo = $(this).children().eq(0).attr("src")
        $(".mainImg").attr("src", photo)

    })


    //add wishlist
    $(document).on("submit", "#wishlist-form", function (e) {
        e.preventDefault();
        let productId = $(this).attr("data-id");
        let data = { id: productId };

        $.ajax({
            url: "/albumwishlist/addwishlist",
            type: "Post",
            data: data,
            success: function (res) {
                $("sup.wishlist-sup").text(res)
            }
        })

    })

    //delete from basket
    $(document).on("submit", "#wishlist-delete-form", function (e) {
        e.preventDefault();
        let productId = $(this).attr("data-id");

        $(this).parent().parent().remove();

        let data = { id: productId };

        $.ajax({
            url: "/albumwishlist/delete",
            type: "Post",
            data: data,
            success: function (res) {
                $("sup.wishlist-sup").text(res.count);
                if (res.count != 0) {
                    $("#total-price").text(res.total);
                } else {
                    $("#total-price").addClass("d-none");
                }

            }
        })
    })




})