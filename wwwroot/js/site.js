function loadNoteDetail(id) {
    $("#modal_notedetail .modal-body").empty();
    var loading = $(".template > .loading").html();
    $("#modal_notedetail .modal-body").append(loading);
    $("#modal_notedetail .modal-body").load("/Home/GetNoteDetail/" + id);
} 