InputCkMaker("comment_Comment").val("");

function Success() {
    var editor = CKEDITOR.instances['comment_Comment'];
    editor.setData('');
    Swal.fire({
        title: "موفقیت آمیز!",
        text: "کاربر گرامی نظر شما پس از تایید کارشناسان نمایش داده خواهد شد",
        icon: "success"
    });
}

function ReplyComment(id) {
    $("#ParentId").val(id);
    $("html, body").animate({ scrollTop: $("#myDiv").offset().top }, 1000);
}

$(function () {
    $("#listComment").load("/Course/ShowComments/" + @Model.CourseId);
});


function ShowPageComments(pageId) {
    $("#listComment").load("/Course/ShowComments/" + @Model.CourseId + "?pageId=" + pageId);
}

$(function () {
    $("#vote").load("/Course/CourseVote/" + @Model.CourseId);
});

function vote(vote) {
    $("#vote").fadeOut("slow");
    $("#vote").load("/Course/AddVote/" + @(Model.CourseId) + "?vote=" + vote);
    $("#vote").fadeIn("slow");

}