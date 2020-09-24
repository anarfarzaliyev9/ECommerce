$(function () {
    $(document).on("click", ".btn-icon-wish", function (e) {
        e.preventDefault();
        var currentBtnWish = $(this);
        var productId_ = $(this).closest(".product-default").attr("data-id");
        var cartId_ = $(".cart-dropdown").attr("data-CartId");
        var isInCart= $(this).find(".fa-heart").hasClass("fas");
        if (isInCart) {
           
            var data_ = {
                CartId: cartId_,
                productId: productId_
            }
       
            $.ajax({
                url: "/Home/RemoveFromCart",
                type: "post",
                async: true,
                dataType: "json",
                data: data_,
                success: function (d) {
                    $(currentBtnWish).find(".fa-heart").removeClass("fas").addClass("far");
                    console.log($(".dropdown-menu").find(`.product[data-id="${productId_}"]`))
                    $(".dropdown-menu").find(`.product[data-id="${productId_}"]`).remove();
                },
                error: function (d) {
                    
                    console.log("error")
                }
            })
           
        }
        else {
      
        var productName = $(this).closest(".product-default").find(".product-title a").text();
        var productPrice = $(this).closest(".product-default").find(".product-price").text();
        var productImageUrl = $(this).closest(".product-default").find("figure a img").attr("src");


        var data_ = {
            CartId: cartId_,
            productId: productId_
        }
        $.ajax({
            url: "/Home/AddToCart",
            type: "post",
            async: true,
            dataType: "json",
            data: data_,
            success: function (d) {
                $(currentBtnWish).find(".fa-heart").removeClass("far").addClass("fas");
                var productDiv = $(`<div class="product" data-id="${productId_}"></div>`);
                var productDetailsDiv = $(`<div class="product-details"></div>`);

                var productTitleH4 = $(`<div class="product-title"></div>`);
                productDetailsDiv.append(productTitleH4);

                var productTitleATag = $(`<a class="product-title">${productName}</a>`);
                productTitleH4.append(productTitleATag);
                var cartProductInfoSpan = $(`<span class="cart-product-info">${productPrice}</span>`);
                productDetailsDiv.append(cartProductInfoSpan);
                var productFigureTag = $(`<figure class="product-image-container"></figure>`);

                var productFigureATag = $(`<a class="product-image"></a>`);
                productFigureTag.append(productFigureATag);
                var productFigureImgTag = $(`<img src="${productImageUrl}")' alt="product" width="80" height="80">`);
                productFigureATag.append(productFigureImgTag);
                var productRemoveBtn = $(`<a class="btn-remove icon-cancel" title="Remove Product" >  </a>`);
                productFigureTag.append(productRemoveBtn);
                productDiv.append(productDetailsDiv);
                productDiv.append(productFigureTag);
                $(".dropdown-cart-products").append(productDiv);
                


            },
            error: function (d) {
                alert(d.em)
                console.log("error")
            }
        })
        }

    })
})