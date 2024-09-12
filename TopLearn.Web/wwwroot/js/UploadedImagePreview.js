function ReadURL(input) {
    if (input.files && input.files[0]) {
        var reader = new FileReader();
        reader.onload = function (e) {
            $("#Imgreview").attr("src", e.target.result);
        }
        reader.readAsDataURL(input.files[0]);
    }
}
$("#Image").change(function () {
    ReadURL(this);
});