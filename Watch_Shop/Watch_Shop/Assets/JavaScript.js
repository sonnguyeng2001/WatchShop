//---------------------- Window scrolling ---------------------------------------------------------------

$(document).ready(function () {
    $(window).scroll(function () {
        if ($(window).scrollTop() > 800) {
            $(".header_nav").addClass("sticky");
            $(".header_nav_link").addClass("white");
            $(".header_nav_icon_js").addClass("white");
            $('.header_nav_img').css('padding', '15px 0px');
            $('.header_nav_img').css('position', 'relative');
            $('.header_nav_img').css('top', '2px');

        }
        else/* if ($(window).scrollTop() < 600)*/ {
            $(".header_nav").removeClass("sticky");
            $(".header_nav_link").removeClass("white");
            $(".header_nav_icon_js").removeClass("white");
            $('.header_nav_img').css('padding', '30px 0px');

        }
    })
});

//----------------------  Slick Slider -------------------------------------------------------------------

$(document).ready(function () {
    $('.slides').slick({
        infinite: true,
        slidesToShow: 1,
        slideToScroll: 1,
        arrows: true,
        prevArrow: "<button type='button' class='slick-prev pull-left'><i class='fas fa-chevron-left slick-arrow' aria-hidden='true'></i></button>",
        nextArrow: "<button type='button' class='slick-next pull-right'><i class='fas fa-chevron-right slick-arrow' aria-hidden='true'></i></button>",
        autoplay: true,
        autoplaySpeed: 1000,
        responsive: [
            {
                breakpoint: 1025,
                settings: {
                    slidesToShow: 1,
                    slidesToScroll: 1,
                    infinite: true,
                    dots: false,
                    arrows: false,

                }
            },
            {
                breakpoint: 767,
                settings: {
                    slidesToShow: 1,
                    slidesToScroll: 1,
                    infinite: true,
                    dots: false,
                    arrows: false,


                }
            },
            {
                breakpoint: 480,
                settings: {
                    slidesToShow: 1,
                    slidesToScroll: 1,
                    infinite: true,
                    dots: false,
                    arrows: false,
                }
            }

        ]
    })
})

$(document).ready(function () {
    $('.slide_info').slick({
        infinite: true,
        slidesToShow: 3,
        slideToScroll: 1,
        autoplay: true,
        autoplaySpeed: 1000,
        arrows: true,
        prevArrow: "<button type='button' class='slick-prev'><i class='fas fa-chevron-left slick_couple_prev' aria-hidden='true'></i></button>",
        nextArrow: "<button type='button' class='slick-next'><i class='fas fa-chevron-right slick_couple_next' aria-hidden='true'></i></button>",
        responsive: [
            {
                breakpoint: 1025,
                settings: {
                    slidesToShow: 2,
                    slidesToScroll: 1,
                    infinite: true,
                    dots: false,
                    arrows: false,
                }
            },
            {
                breakpoint: 767,
                settings: {
                    slidesToShow: 1,
                    slidesToScroll: 1,
                    infinite: true,
                    dots: false,
                    arrows: true,

                }
            },
            {
                breakpoint: 480,
                settings: {
                    slidesToShow: 1,
                    slidesToScroll: 1,
                    infinite: true,
                    dots: false,
                    arrows: true,
                }
            }

        ]
    })
});

$(document).ready(function () {
    $('#couples').slick({
        infinite: true,
        slidesToShow: 4,
        slideToScroll: 1,
        arrows: true,
        prevArrow: "<button type='button' class='slick-prev'><i class='fas fa-chevron-left slick_couple_prev' aria-hidden='true'></i></button>",
        nextArrow: "<button type='button' class='slick-next'><i class='fas fa-chevron-right slick_couple_next' aria-hidden='true'></i></button>",
        responsive: [
            {
                breakpoint: 1024,
                settings: {
                    slidesToShow: 2,
                    slidesToScroll: 1,
                    infinite: true,
                    arrows: false,
                    dots: true,
                }
            },
            {
                breakpoint: 600,
                settings: {
                    slidesToShow: 1,
                    slidesToScroll: 1,
                    infinite: true,
                    arrows: false,
                    dots: true,
                }
            },
            {
                breakpoint: 480,
                settings: {
                    slidesToShow: 1,
                    slidesToScroll: 1,
                    infinite: true,
                    arrows: false,
                    dots: true,

                }
            }
            // You can unslick at a given breakpoint now by adding:
            // settings: "unslick"
            // instead of a settings object
        ]
    })
});

//----------------------  Button: Come Back Top -----------------------------------------------------------

var OnTop = document.querySelector('.top')

window.onscroll = function () {
    scrollFunction();
}

function scrollFunction() { // Show button
    if (document.body.scrollTop > 800 || document.documentElement.scrollTop > 800) {
        OnTop.style.opacity = "1";
        OnTop.style.visibility = "visible";
        OnTop.style.bottom = 20 + "px";
    }
    else {
        OnTop.style.opacity = "0";
        OnTop.style.visibility = "hidden";
        OnTop.style.bottom = 0 + "px";

    }
}

function TopFunction() { // When the user clicks on the button, scroll to the top of the document
    document.body.scrollTop = 0;
    document.documentElement.scrollTop = 0;
}
OnTop.addEventListener("click", TopFunction);


//----------------------  Side bar modal -------------------------------------------------------------------

var sidebarModal = document.querySelector('.sidebar_modal');
var sidebarClose = document.querySelector('.js-sidebar-close');
var sidebarOpen = document.querySelector('.js-sidebar-open');
var sidebarContainer = document.querySelector('.sidebar_modal_container');

sidebarClose.addEventListener("click", function () {
    sidebarContainer.classList.remove("openSidebar");
    setTimeout(function () {
        sidebarModal.style.display = "none"
    }, 200);
});

sidebarOpen.addEventListener("click", function () {
    sidebarModal.style.display = "block"
    setTimeout(function () {
        sidebarContainer.classList.add("openSidebar");
    }, 200);
});

//----------------------  Open Modal Login ------------------------------------------------------------------
const modal = document.querySelector(".js-modal");
const modalClose = document.querySelector(".js-modal-close");
const modalContainer = document.querySelector(".js-modal-container");
const stopScroll = document.querySelector('#appBody');
const user = document.querySelector('.user');



// Hàm hiển thị modal mua vé (thêm class open vào modal)

function showBuyTicket() {
    modal.classList.add("open");
    stopScroll.classList.add('stop-scroll');
}

// Hàm ẩn modal mua vé (gỡ bỏ class open khỏi modal)

function closeBuyTicket() {
    modal.classList.remove("open");
    stopScroll.classList.remove('stop-scroll');

}

// Vì có một nút đóng nên không cần vòng for

modalClose.addEventListener("click", closeBuyTicket)

modal.addEventListener("click", closeBuyTicket);

if (user) {
    user.addEventListener('click', showBuyTicket, false);
}
modalContainer.addEventListener("click", function (event) {
    event.stopPropagation();
})

//----------------------  Update cart using Ajax ------------------------------------------------------------------

function UpdateCart() {
    var listProduct = $('.js-txt-quantity');
    var cartList = [];
    $.each(listProduct, function (i, item) {
        cartList.push({
            iProduct_ID: $(item).data('id'),
            iProduct_SoLuong: $(item).val(),
        });
    });

    $.ajax({
        url: '/Cart/Update',
        data: { cartModel: JSON.stringify(cartList) },
        dataType: 'json',
        type: 'POST',
        success: function (res) {
            if (res.status == true) {
                window.location.href = "/Payment/Payment";
            }
            else {
                alert("Có lỗi trong quá trình xử lý !!!");
                return;
            }
        },
        error: function (res) {
            alert(res.responseText)
        }
    })
}

//----------------------  Delete a product form cart --------------------------------------------------------------

function DeleleProduct(id) {
    $(document).ready(function () {
        swal({
            title: "Bạn có muốn xóa sản phẩm này?",
            icon: "warning",
            buttons: true,
            dangerMode: true,
        }).then((willDelete) => {
            if (willDelete) {
                $.ajax({
                    url: '/Cart/Delete_Product/' + id,
                    type: 'POST',
                    dataType: "json",
                    contentType: "application/json; charset=urf-8",
                    async: true,
                    success: function (result) {
                        swal("Thông báo!", "Xóa thành công!!!", "success").then(function () {
                            $(".cart_layout").load(location.href + ' .cart_layout');
                            $(".quantity_cart_number").load(window.location.href + " .quantity_cart_number");
                            if (document.querySelectorAll('.cartpage').length - 1 == 0) {
                                var cart_empty = "";
                                cart_empty += '<h2 class="Clock_Topic">';
                                cart_empty += 'Your Cart Is Empty<br /><br /><br />';
                                cart_empty += '<img src="/image/empty-cart.png" width="300" />';
                                cart_empty += '</h2>';
                                $('#cart_wrap').html(cart_empty);
                            }
                        });
                    },
                });
            };
        });

    })
}

//----------------------  Delete a product form List Product Favorite ----------------------------------------------

function DeleteProductFavorite(id) {
    var Customer_id = window.localStorage.getItem("USER_ID");
    var obj = {
        Customer_ID: Number(Customer_id),
        Product_ID: Number(id),
    };
    $.ajax({
        url: '/User/Delete_Favorite_Product',
        type: 'POST',
        dataType: "json",
        contentType: "application/json; charset=urf-8",
        async: true,
        data: JSON.stringify(obj),
        success: function (result) {
            if (result.status) {
                $("#Product_Item_" + id).fadeOut(300, function () {
                    $(this).remove();
                    $("#quantity_cart_number_favorite").load(window.location.href + " #quantity_cart_number_favorite");
                    if ($(".product_item").length <= 0) {
                        var html = '';
                        html += '<h2 style="text-transform:capitalize" class="Clock_Topic">Add something to make me happy :))<br /><br /><br />';
                        html += '<img src="/image/Product_Like_Empty.png" width="300" /> </h2>';
                        $(".Clock_Content").html(html);
                    }

                    create_Toast(" Xóa thành công!!!");
                });
                return;
            }
            else {
                alert("Xóa thất bại !!!");
                return;
            }

        },
        error: function (result) {
            alert("Status = Errorr")
        }
    })
}

//----------------------  Button change quantity in Cart -----------------------------------------------------------

$(document).ready(function () {
    
})

//----------------------  Clear cart -------------------------------------------------------------------------------

function ClearCart() {
    swal({
        title: "Bạn có muốn xóa hết sản phẩm trong giỏ hàng?",
        icon: "warning",
        buttons: true,
        dangerMode: true,
    }).then((willDelete) => {
        if (willDelete) {
            $.ajax({
                url: '/Cart/DeleteAll',
                type: 'POST',
                dataType: "json",
                contentType: "application/json; charset=urf-8",
                async: true,
                success: function (result) {
                    swal("Thông báo", "Xóa thành công!!", "success").then(function () {
                        $(".quantity_cart_number").load(window.location.href + " .quantity_cart_number");
                        var cart_empty = "";
                        cart_empty += '<h2 class="Clock_Topic">';
                        cart_empty += 'Your Cart Is Empty<br /><br /><br />';
                        cart_empty += '<img src="/image/empty-cart.png" width="300" />';
                        cart_empty += '</h2>';
                        $('#cart_wrap').html(cart_empty);
                    });
                }
            });
        };
    });

}

//----------------------  Add product to Cart ----------------------------------------------------------------------

function AddProduct(id) {
    $(document).ready(function () {
        $.ajax({
            url: '/Cart/Add_Product/' + id,
            type: 'POST',
            dataType: "json",
            contentType: "application/json; charset=urf-8",
            async: true,
            success: function (result) {
                $(".quantity_cart_number").load(location.href + " .quantity_cart_number");
                create_Toast("Thêm thành công");
            }
        });
    })
};

//----------------------  Toast message UI -------------------------------------------------------------------------

function create_Toast(message) {
    var html = `
                <div class="toast-content">
                    <i class="fas fa-solid fa-check check"></i>
                    <div class="message">
                        <p class="text text-1">Thông báo</p>
                        <p class="text text-2">${message}</p>
                    </div>
                </div>
                <i class="fa-solid fa-xmark close"></i>
                <div class="progress active"></div>
            `;

    var div_html = document.createElement('div');
    div_html.classList.add('toast');
    div_html.innerHTML = html;
    document.getElementById('toast_wrap').appendChild(div_html);

    // Auto remove Toast mesage after 2.5s
    const autoRemove = setTimeout(() => {
        div_html.classList.add('remove');
        setTimeout(() => {
            document.querySelector('#toast_wrap').removeChild(div_html);
        }, 1000)
    }, 2500);

    //  Click icon and remove Toast mesage
    div_html.onclick = function (e) {
        if (e.target.classList.contains('close')) {
            div_html.classList.add('remove');
            setTimeout(() => {
                document.querySelector('#toast_wrap').removeChild(div_html);
            }, 1000)
            clearTimeout(autoRemove);
        }
    };
}

//----------------------  Add a product to List Product Favorite -----------------------------------------------------------

function AddProductFavorite(Product_id, Customer_id) {
    var obj = {
        Customer_ID: Number(Customer_id),
        Product_ID: Number(Product_id),
    };
    $.ajax({
        url: '/User/Favorite_Product',
        type: 'POST',
        dataType: "json",
        contentType: "application/json; charset=urf-8",
        async: true,
        data: JSON.stringify(obj),
        success: function (result) {
            if (result.status) {
                create_Toast("Thêm thành công !!!")
                $(".icon_heart_" + Product_id + " i").addClass("red");
                $("#quantity_cart_number_favorite").load(window.location.href + " #quantity_cart_number_favorite");
                return;
            }
            else {
                swal("Thông báo", "Sản phẩm đã tồn tại !!!", "warning");
                $(".icon_heart_" + id + " i").addClass("red");
                return;
            }

        },
        error: function (result) {
            alert("Status = Errorr")
        }
    })
}

//----------------------  Clear localStorage -------------------------------------------------------------------------------
$(document.body).on("click", "#Remove_USER_ID", () => {
    window.localStorage.clear();
})



//----------------------  Function HightText when search -------------------------------------------------------------------------------

function Highlight(Row_tableName, Column_ClassName, searchText) {
    if (searchText) {
        for (let i = 0; i < ($(Row_tableName)).length; i++) {
            var content = $(Column_ClassName + i).text();
            var searchExp = new RegExp(searchText, "ig");
            var matches = content.match(searchExp);
            if (matches) {
                $(Column_ClassName + i).html(content.replace(searchExp, function (match) {
                    return `<span style="font-weight:bold">${match}</span>`;
                }));
            }
        }
    }
}

//----------------------  Render partial View List Product by search key in _Layout  -------------------------------------------------------------------------------

$(document.body).on("input", "input[name = 'keyword']", () => {
    $("#Area_Input").append("<button class='buttonload'><i class='fa fa-spinner fa-spin'></i></button>");
    var key = $("input[name = 'keyword']").val();
    if (key == "") {
        $("#ShowListFromKey table").remove();
        $("#ShowListFromKey").css("display", "none");
        $(".buttonload").remove();
        return;
    }

    $.ajax({
        url: '/Product/Find_Product',
        type: 'GET',
        dataType: "json",
        contentType: "application/json; charset=urf-8",
        async: true,
        data: { keyword: key },

        success: function (result) {
            var html = "";
            if (result.status) {
                if (result.count <= 0) {
                    html = "<p style='padding-top:12px; font-size:17px; color:rgb(226 186 72)'> No Result For ' " + key + " '</p>";
                    $("#ShowListFromKey").html(html);
                    $(".buttonload").remove();
                }
                else { // result.count > 0
                    html += "<p style='padding-top:12px; font-size:17px; color:rgb(226 186 72)'>Search Result (" + result.count + ")</p>"
                    html += '<table cellspacing="0" cellpadding="0">';
                    html += '<tbody>';
                    $.each(result.data, function (key, item) {
                        html += '<tr>';
                        html += '<td style="display:none" class = "ProductID_' + item.Product_ID + '">' + item.Product_ID + '</td>';
                        html += '<td width="10%"><img src = "/image/' + item.Image + '" width="50"/> </td>';
                        html += '<td class = "Search_Table_Product_' + key + '"  width="60%">' + item.Product_Name + '</td>';
                        html += '<td style="font-weight:700" width="20%">' + item.Price.toLocaleString() + '</td>';
                        html += '</tr>';
                    })
                    html += '</tbody>';
                    html += '</table>';
                    $("#ShowListFromKey").html(html);
                    $("#ShowListFromKey").css("display", "block")
                    $(".buttonload").remove();
                    Highlight("#ShowListFromKey table tbody tr", ".Search_Table_Product_", key);
                }
            }
            else // status = false
            {
                swal("Thông báo", "Có lỗi trong quá trình xử lý", "warning");
                return;
            }
        },

        error: function (result) {
            swal("Thông báo", result.responseText, "error");
        }

    })

})

//----------------------  Go to ShowProduct By ID -------------------------------------------------------------------------------
$(document.body).on("click", "#ShowListFromKey table tbody tr", function () {
    $('#ShowListFromKey table tbody tr td').on('click', function () {
        var row = $(this).closest('tr');
        var id = $(row).find('td').eq(0).html();
        window.location.href = "/Product/ShowProduct_Id/" + id;
    });
})
     

//----------------------  Tab - Click -------------------------------------------------------------------------------
const tabs = document.querySelectorAll(".tab-item");
const panes = document.querySelectorAll(".tab-pane");
const tabActive = document.querySelector(".tab-item.active");
const line = document.querySelector(".tabs .line");
tabs.forEach((tab, index) => {
    const pane = panes[index];
    tab.onclick = function () {
        document.querySelector(".tab-item.active").classList.remove("active");
        document.querySelector(".tab-pane.active").classList.remove("active");

        line.style.left = this.offsetLeft + "px";
        line.style.width = this.offsetWidth + "px";
        this.classList.add("active");
        pane.classList.add("active");
    };
});
line.style.left = tabActive.offsetLeft + "px";
line.style.width = tabActive.offsetWidth + "px";



