// ===== AppleShop — JS phía Khách hàng =====
// Cập nhật giỏ hàng bằng AJAX: đổi số lượng / xóa dòng không reload trang.
$(function () {

    // Đổi số lượng trong trang giỏ hàng
    $(document).on("change", ".qty-input", function () {
        var $input = $(this);
        var variantId = $input.data("variant-id");
        var quantity = parseInt($input.val(), 10);
        if (isNaN(quantity) || quantity < 1) {
            quantity = 1;
            $input.val(1);
        }

        $.post("/Cart/UpdateQuantity", { variantId: variantId, quantity: quantity }, function (data) {
            if (!data.success) {
                alert(data.message || "Không cập nhật được giỏ hàng.");
                return;
            }
            $input.closest("tr").find(".line-total").text(data.lineTotal + " ₫");
            $("#cart-total").text(data.cartTotal + " ₫");
            updateNavCartCount(data.cartCount);
        });
    });

    // Xóa một dòng khỏi giỏ hàng
    $(document).on("click", ".btn-remove-item", function () {
        var $btn = $(this);
        var variantId = $btn.data("variant-id");
        if (!confirm("Xóa sản phẩm này khỏi giỏ hàng?")) return;

        $.post("/Cart/Remove", { variantId: variantId }, function (data) {
            if (!data.success) return;
            $btn.closest("tr").remove();
            $("#cart-total").text(data.cartTotal + " ₫");
            updateNavCartCount(data.cartCount);
            if (data.isEmpty) {
                $("#cart-table-wrap").html(
                    '<div class="alert alert-info">Giỏ hàng của bạn đang trống. ' +
                    '<a href="/Product">Tiếp tục mua sắm</a></div>');
            }
        });
    });

    function updateNavCartCount(count) {
        var $badge = $("#nav-cart-count");
        $badge.text(count);
        $badge.toggleClass("d-none", count === 0);
    }
});
