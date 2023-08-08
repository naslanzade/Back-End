$(document).ready(function () {



    $(document).on("click", ".images button", function () {

        let productImageId = $(this).attr("data-id");
        let removeElem = $(this).parent();
        let data = { id: productImageId };


        $.ajax({
            url: "/admin/podcast/DeleteProductImage",
            type: "Post",
            data: data,
            success: function (res) {
                $(removeElem).remove();
            }
        })
    })








})