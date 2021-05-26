$(function () {

    var noteids = [];
    //div elemtine ait bu attribute e sahip elementi bul. hepsinde dön.i:index e:elemti temsil ediyor
    $("div[data-note-id]").each(function (i, e) {

        noteids.push($(e).data("note-id")); //ekleme push ile yaptık.
    });

    $.ajax({

        method: "POST",
        url: "/Note/GetLiked",
        data: { ids: noteids }
    }).done(function (data) {

        if (data.result != null && data.result.length > 0) {
            //likeladıklarımız
            for (var i = 0; i < data.result.length; i++) {
                var id = data.result[i];
                var likedNote = $("div[data-note-id]=" + id); //yani bu idye ait olan divi yakaladık.
                var btn = $("button[data-liked]");
                var i_tag = btn.find("i.like star"); //butonun çocuklarından ilk elementin altındaki ilki

                btn.data("liked", true);
                i_tag.removeClass("far fa-star");
                i_tag.addClass("fas fa-star");
            }
        }

    }).fail(function () {

    });


    $("button[data-liked]").click(function () {

        var btn = $(this);
        var liked = btn.data("liked");
        var noteid = btn.data("note-id");
        var i_tag = btn.find("i.like star"); //classında like star olan i tagini find ile elde etmiş olduk. data ile ise veri okunmuş olur.
        var spancount = btn.find("span.like-count");// span. diyerek tagi span olan ve classı like-count olanı getir demiş olduk


        $.ajax({

            method: "POST",
            url: "/Note/SetLikeState",
            data: { "noteid": noteid, "liked": !liked } //likedın tersini aldık çünkü likelı ise kaldıracaz değilse likelıcaz ya

        }).done(function (data) {

            if (data.hasError) {
                alert(data.errorMessage);
            } else {

                liked = !liked; //artık false ise likelandı manasında
                btn.data("liked", liked);
                spancount.text(data.result);

                i_tag.removeClass("far fa-star");
                i_tag.removeClass("fas fa-star");

                if (liked) {
                    i_tag.addClass("fas fa-star");
                }
                else {
                    i_tag.addClass("far fa-star");
                }
            }

        }).fail(function () {
            alert("Beğenmek için sisteme kayıtlı olmanız gerekli");
        });
    });

});