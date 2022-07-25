const { post } = require("jquery");

//id:noteid
function loadNoteDetail(id) {
    $("#modal_notedetail .modal-body").empty();
    var loading = $(".template > .loading").html();
    $("#modal_notedetail .modal-body").append(loading);
    $("#modal_notedetail .modal-body").load("/Home/GetNoteDetail/" + id);
}

//id:noteid
function loadNoteComments(id) {
    $("#modal_notecomments .modal-body").empty();
    var loading = $(".template > .loading").html();
    $("#modal_notecomments .modal-body").append(loading);
    $("#modal_notecomments .modal-body").load("/Home/GetNoteComment/" + id);
}


//id:noteid
function sendComment(id) {

    var txt = $("#txtCommentText").val();

    $("#modal_notecomments .modal-body").empty();
    var loading = $(".template > .loading").html();

    $("#modal_notecomments .modal-body").append(loading);


    $.ajax({
        method: "post",
        url: "/Note/AddCommentToNote/"+id,
        data: { text: txt  }
    }).done(function (result) {
        if (result.hasError != undefined && result.hasError) {
            alert("Yorumunuz eklenemedi.Bir hata oluştu");
        }
    }).fail(function () {
        alert("Yorumunuz eklenemedi.Bir hata oluştu");
    }).always(function () {
        $("#modal_notecomments .modal-body").load("/Home/GetNoteComment/" + id);

    });

}