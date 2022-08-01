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


function removeComment(commentId,noteId) {

    var conf = confirm("Yorumunuzu silmek istediğinize emin misiniz?");


    if (conf) {

        $("#modal_notecomments .modal-body").empty();
        var loading = $(".template > .loading").html();

        $("#modal_notecomments .modal-body").append(loading);


        $.ajax({
            method: "post",
            url: "/Note/removeComment/" + commentId
        }).done(function (result) {
            if (result.hasError != undefined && result.hasError) {
                alert("Yorumunuz silinemedi.Bir hata oluştu");
            }
        }).fail(function () {
            alert("Yorumunuz silinemedi.Bir hata oluştu");
        }).always(function () {
            $("#modal_notecomments .modal-body").load("/Home/GetNoteComment/" + noteId);

        });

    }
}

function editComment(editlink) {

    var mBody = $(editlink).parent().parent().parent().parent();

    mBody.find(".edit-buttons").addClass("d-none");
    mBody.find(".edit-buttons").removeClass("d-none");

    mBody.find(".comment-text").attr("contenteditable", "true").focus();

}

function cancelEditComment(cancelEditLink,noteId) {

    //var mBody = $(cancelEditLink).parent().parent().parent();

    //mBody.find(".edit-buttons").removeClass("d-none");
    //mBody.find(".edit-buttons").addClass("d-none");

    //mBody.find(".comment-text").removeAttr("contenteditable");

    $("#modal_notecomments .modal-body").load("/Home/GetNoteComment/" + noteId);

} 

function updateComment(saveEditLink,commentId, noteId) {


    var mBody = $(saveEditLink).parent().parent().parent().parent();

    var parag = mBody.find(".comment-text");
    parag.removeAttr("contenteditable");

    var newCommentText = parag.text();


        $("#modal_notecomments .modal-body").empty();
        var loading = $(".template > .loading").html();

        $("#modal_notecomments .modal-body").append(loading);


        $.ajax({
            method: "post",
            url: "/Note/updateComment/" + commentId,
            data: { text: newCommentText }
        }).done(function (result) {
            if (result.hasError != undefined && result.hasError) {
                alert("Yorumunuz güncellemenemedi.Bir hata oluştu");
            }
        }).fail(function () {
            alert("Yorumunuz güncellemenemedi.Bir hata oluştu");
        }).always(function () {
            $("#modal_notecomments .modal-body").load("/Home/GetNoteComment/" + noteId);

        });

}

function likeNote(linkBtn, noteId) {
    $.ajax({
        method: "post",
        url: "/Note/likeNote/" + noteId
    }).done(function (result) {
        if (result.hasError != undefined && result.hasError) {
            alert("Yazı beğenilemedi.Bir hata oluştu");
        } else {
            var icon = $(linkBtn).find("i");
            icon.toggleClass("fas");
            icon.toggleClass("far");

            $(linkBtn).find("span").text(result.likeCount);
        }
    }).fail(function () {
        alert("Yazı beğenilemedi.Bir hata oluştu");
    });
}