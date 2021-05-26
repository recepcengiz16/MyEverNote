var modalCommentBodyId = "#modal_comment_body";
var noteid = -1;
$(function () {
    $('#modal_comment').on('show.bs.modal', function (e) {
        var btn = $(e.relatedTarget); //butonu yakaladık. modalpopup da target onu veriyor.
        noteid = btn.data("note-id");
        $(modalCommentBodyId).load("/Comment/ShowNoteComments/" + noteid);
    })
});

//comment ile yapılacak işlemlerde
function doComment(btn, e, commentid, iconid) {

    var button = $(btn); //jquery butonuna çevirdik. Aşağıdaki metotlara erişebilmesi için bu tanımı yaptık. Çünkü bu metotlar jquerry ile gelen metotlar
    var mode = button.data("edit-mode");


    if (e === "edit_clicked") {
        if (!mode) {
            button.data("edit-mode", true); //eğer sadece edit mode olsa orada data attribute u okumuş oluruz. Değer atamak istersek de isimden sonra ikinci paramtere ile değeri veririz
            button.removeClass("btn-warning"); //classını sildik
            button.addClass("btn-success");
            var btnicon = button.find("i"); //butonun altında ara find ile(iconu değiştirmek için i tagini ara) ve
            btnicon.removeClass("fas fa-edit");
            btnicon.addClass("fas fa-check-circle");

            $(btnicon).addClass("editable");
            $(btnicon).attr("contenteditable", true); //bu attribute bul ve true yap
            $(btnicon).focus();
        }
        else {
            button.data("edit-mode", false); //eğer sadece edit mode olsa orada data attribute u okumuş oluruz. Değer atamak istersek de isimden sonra ikinci paramtere ile değeri veririz
            button.addClass("btn-warning"); //classını sildik
            button.removeClass("btn-success");
            var btnicon = button.find("i"); //butonun altında ara find ile(iconu değiştirmek için i tagini ara) ve
            btnicon.removeClass("fas fa-edit");
            btnicon.addClass("fas fa-check-circle");

            $(btnicon).removeClass("editable");
            $(btnicon).attr("contenteditable", false); //bu attribute bul ve false yap


            var txt = $(btnicon).text(); //i tagindeki içindeki texti al
            $.ajax({
                method: "POST",
                url: "/Comment/Edit/" + commentid,
                data: { "text": txt }
            }).done(function (data) {
                if (data.result) { //result comment controllerda json nesnesi olarak result geri döndürdük ya
                    //yorumlar partial tekrar yüklenir.
                    $(modalCommentBodyId).load("/Comment/ShowNoteComments/" + noteid);
                }
                else {
                    alert("Yorumlar güncellenemedi.");
                }

            }).fail(function () {
                alert("Sunucu ile bağlantı kurulamadı.");
            });
        }
    }
    else if (e === "delete_clicked") {
        var dialof_res = confirm("Yorum silinsin mi");

        if (!dialof_res) {
            return false;
        }

        $.ajax({
            method: "GET",
            url: "/Comment/Delete/" + commentid
        }).done(function (data) {
            if (data.result) {
                //yorumlar partiala tekrar yüklenir.
                $(modalCommentBodyId).load("/Comment/ShowNoteComments/" + noteid);
            }
            else {
                alert("yorum silinemedi");
            }
        }).fail(function () {
            alert("Sunucu ile bağlantı kesildi");
        });
    }
    else if (e === "new_clicked") {

        var txt = $("#new_comment_text").val();
        $.ajax({
            method: "POST",
            url: "/Comment/Create/" + commentid,
            data: { "text": txt, "noteid": noteid }
        }).done(function (data) {
            if (data.result) {
                //yorumlar partiala tekrar yüklenir.
                $(modalCommentBodyId).load("/Comment/ShowNoteComments/" + noteid);
            }
            else {
                alert("Yorum eklenemedi");
            }
        }).fail(function () {
            alert("Sunucu ile bağlantı kesildi");
        });
    }
}