$(function () {

    $(document).on('click', '.category span a', function (e) {
        e.preventDefault();
        let category = $(this).attr('data-id');
        let products = $('.each-cards');

        products.each(function () {
            if (category == $(this).attr('data-id')) {
                $(this).removeClass('d-none');
            }
            else {
                $(this).addClass('d-none');
            }
        })     

    })



    $(document).on('click', '.category span a', function (e) {
        e.preventDefault();
        let category = $(this).attr('data-id');
        let page = $('.pagination');

        page.each(function () {
            if (category == $(this).attr('data-id')) {
                $(this).removeClass('d-none');
            }
            else {
                $(this).addClass('d-none');
            }
        })

    })











})