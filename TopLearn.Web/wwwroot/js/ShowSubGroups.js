$("#Course_GroupId").change(function () {

    $("#Course_SubGroupId").empty();

    $.getJSON("/ShowSubGroup/index/" + $("#Course_GroupId :selected").val(),
        function (data) {

            $.each(data,
                function () {

                    $("#Course_SubGroupId").append('<option value =' + this.value + '>' + this.text + '</option>');
                }
            );
        }
    );
});