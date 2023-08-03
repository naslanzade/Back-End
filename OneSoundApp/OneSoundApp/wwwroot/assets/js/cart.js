$(function () {


    //add basket
    $(document).on("submit", "#basket-form", function (e) {
        console.log("ss");
        e.preventDefault();       
        let productId = $(this).attr("data-id");
        let data = { id: productId };

        $.ajax({
            url: "/albumcart/addbasket",
            type: "Post",
            data: data,
            success: function (res) {
                $("sup.cart-sup").text(res)
            }
        })
    })



    //delete from basket
    $(document).on("submit", "#basket-delete-form", function (e) {
        e.preventDefault();
        let productId = $(this).attr("data-id");

        $(this).parent().parent().remove();

        let data = { id: productId };

        $.ajax({
            url: "/albumcart/delete",
            type: "Post",
            data: data,
            success: function (res) {
                $("sup.cart-sup").text(res.count);
                if (res.count != 0) {
                    $(".total-price").text(res.total);
                } else {
                    $(".total-price").addClass("d-none");
                }

            }
        })




    })


 








































})