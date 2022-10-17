$(document).ready(function () {
    var selectedUrl = "";

    $("#deleteModal").on("shown.bs.modal", function (event) {
        selectedUrl = $(event.relatedTarget).attr('href');
    });

    $("#deleteModal #btn-submit").on("click", function (e) {
        e.preventDefault();
        var button = $('a[href="' + selectedUrl + '"]');
        var token = $("input[name='__RequestVerificationToken']").val();

        $.ajax({
            method: "POST",
            url: selectedUrl,
            data: {
                __RequestVerificationToken: token
            },
            success: function () {
                $(button).closest("tr").remove();
            },
            complete: function () {
                $("#deleteModal").modal('hide');
                selectedUrl = "";
            }
        });
    });

    $(".btn-show-details").on("click", function () {
        var id = $(this).data("id");
        var isRead = $(this).attr("data-is-read") == "True";

        $.ajax({
            method: "GET",
            url: `/Admin/Reservations/Details/${id}`,
            success: function (result) {
                $(".modal-wrapper").html(result);
                $("#reservationDetailsModal").modal('show');

                if (!isRead) {
                    markAsRead(id);
                }
            }
        });
    });

    function markAsRead(id) {
        var token = $("input[name='__RequestVerificationToken']").val();

        $.ajax({
            method: "POST",
            url: `/Admin/Reservations/${id}/MarkAsRead`,
            data: {
                __RequestVerificationToken: token
            },
            success: function () {
                $(`.btn-show-details[data-id="${id}"]`).attr("data-is-read", "True");
                //$(`.badge[data-id="${id}"]`).remove();
            }
        });
    }

    $("#searchForm").on("submit", function () {
        var value = $('input[name="searchString"]').val();
        if (value == "") {
            return false;
        }
    });

    $(document).on("change", ".currency-field", function () {
        var value = parseFloat($(this).val()).toFixed(2);
        $(this).val(value);
    });
});

function showToast(content, delay = 5000) {
    var toastElement = $(".toast");
    $(toastElement).find(".toast-content").html(content);
    var toast = bootstrap.Toast.getOrCreateInstance(toastElement, { delay: delay });
    toast.show();
}