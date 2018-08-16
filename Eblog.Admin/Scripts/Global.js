/* Kategori Ekle-Sil-Düzenle */

//function KategoriEkle() {
//    Kategori = new Object();
//    Kategori.KategoriAdi = $("#kategoriAdi").val();
//    Kategori.Onay = $("#kategoriOnay").is(":checked");



//    $.ajax({
//        url: "/Kategori/Ekle",
//        data: Kategori,
//        type: "POST",
//        datatype: 'json',
//        success: function (response) {
//            if (response.Success)
//            {
//                bootbox.alert(response.Message, function () {
//                    location.href = "/Kategori/Index";
//            }

//            else {
//                bootbox.alert(response.Message, function () {
//                    //geri döndüğünde bir şey yapılması isteniyorsa buraya yazılır.
//                });
//            }
//        }
//    })
//}

//$(document).on("click", "#KategoriDelete", function () {
//    var gelenID = $(this).attr("data-id");
//    var silTR = $(this).closest("tr");
//    $.ajax({
//        url: '/Kategori/Sil/' + gelenID,
//        type: "POST",
//        dataType: 'json',
//        success: function (response) {
//            if (response.Success) {
//                $.notify(response.Message, "success");
//                silTR.fadeOut(300, function () {
//                    silTR.remove();
//                })
//            }
//            else {
//                $.notify(response.Message, "error");
//            }


//        }

//    })
//})

//function KategoriDuzenle() {
//    Kategori = new Object();
//    Kategori.KategoriAdi = $("#kategoriAdi").val();
//    Kategori.Onay = $("#kategoriOnay").is(":checked");
//    Kategori.ID = $("#ID").val();


//    $.ajax({
//        url: "/Kategori/Duzenle",
//        data: Kategori,
//        type: "POST",
//        dataType: 'json',
//        success: function (response) {
//            if (response.Success) {
//                bootbox.alert(response.Message, function () {
//                    location.href = "/Kategori/Index";
//                });
//            }

//            else {
//                bootbox.alert(response.Message, function () {
//                    //geri döndüğünde bir şey yapıması isteniyorsa buraya yazılır.
//                });
//            }
//        }
//    })
//}