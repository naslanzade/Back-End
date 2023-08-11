$(document).ready(function () {



    $(document).on("click", ".delete-image", function (e) {
        e.preventDefault();
        let Id = $(this).parent().parent().attr("data-id");
        let deletedElem = $(this).parent().parent();
        let data = { id: Id }
        $.ajax({
            url: "/Admin/Album/DeleteProductImage",
            type: "Post",
            data: data,
            success: function (res) {
                $(deletedElem).remove();
                let imagesId = $(".images").children().eq(1).attr("data-id");
                let data = $(".images").children().eq(1);
                let changeElem = $(data).children().eq(1).children().eq(1);

                if (res.id == imagesId) {
                    if ($(changeElem).children().hasClass("de-active")) {
                        $(changeElem).children().eq(0).addClass("active-status");
                        $(changeElem).children().eq(0).removeClass("de-active");
                    }
                }
            }

        })
    })


    $(document).on("click", ".image-status", function (e) {
        e.preventDefault();
        let imageId = $(this).parent().parent().attr("data-id");
        let elems = $(".image-status")
        let changeElem = $(this);
        let data = { id: imageId }
        $.ajax({
            url: "/Admin/Album/SetMainImage",
            type: "Post",
            data: data,
            success: function (res) {
                if (res) {
                    for (var elem of elems) {
                        if ($(elem).hasClass("active-status")) {
                            $(elem).removeClass("active-status")
                            $(elem).addClass("de-active")
                        }
                    }
                    if ($(changeElem).hasClass("de-active")) {
                        $(changeElem).removeClass("de-active");
                        $(changeElem).addClass("active-status");
                    }
                }
            }
        })
    })








})