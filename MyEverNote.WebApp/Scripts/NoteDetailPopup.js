$(function () {
    $('#modal_notedetail').on('show.bs.modal', function (e) {
        var btn = $(e.relatedTarget); //butonu yakaladık. modalpopup da target onu veriyor.
        noteid = btn.data("note-id");
        $("#modal_notedetail_body").load("/Note/GetNoteText/" + noteid);
    })
});