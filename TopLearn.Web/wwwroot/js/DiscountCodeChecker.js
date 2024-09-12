$("#Discount_DiscountCode").blur(function () {

    $.ajax({
        type: "get",
        url: "/Admin/Discount/Create/CheckdiscountCode?discountCode=" + $("#Discount_DiscountCode").val(),
    }).done(function (res) {
        if (res === "True") {
            $("#AlertId").show();
        }
        else {
            $("#AlertId").hide();
        }
    });

});