
//Personel listesinde Sisteme Girişin Kontrolünü Yapar
function toggleCheckbox(element) {
    var id = element.value;  //id değerini alıyoruz

    //Checkbox işaretlimi değilmi kontrol ediliyoru.
    if (element.checked) {
        var durum = "1";
    }
    else {
        var durum = "0";
    }

    $.ajax({
        type: 'POST',
        url: '/Modul/Komut.aspx/sistemegiris',  //işlem yaptığımız sayfayı belirtiyoruz
        data: '{ id:' + id + ',durum:' + durum + '}', //datamızı yolluyoruz
        contentType: 'application/json; charset=utf-8',
        dataType: 'json',
        success: function (result) {
            if (result.d != "Hata") {
                Toastinette.show('success', 2000 + Math.random() * 5000, result.d);
            }
            else {
                Toastinette.show('error', 2000 + Math.random() * 5000, "İşleminiz sırasında bir hata meydana geldi.");
            }
           
        },
        error: function () {
            Toastinette.show('error', 2000 + Math.random() * 5000, result.d);
        }
    });
};

//Roller ve Yetkiler Sayfasındaki Rollerin Yetkilerini Belirlerken Kullanılan Komut.
function yetki(element) {
    var id = element.value;// Tıklanan Ögenen id değerini alıyoruz

    //Tıklanan Ögenen Çoklu Değer Gönderiyoruz. ve Parçalayarak Alıyoruz.
    var fields = id.split('-');
    var birinci = fields[0];
    var ikinci = fields[1];

    //Checkbox işaretlimi değilmi kontrol ediliyoru.
    if (element.checked) {
        var durum = "1";
    }
    else {
        var durum = "0";
    }
   
    $.ajax({
        type: 'POST',
        url: '/Modul/Komut.aspx/yetki_yap',  //işlem yaptığımız sayfa ve içindeki modülü belirtiyoruz
        data: '{ Rolid:' + birinci + ', yetkiid:' + ikinci + ',durum:' + durum + '}', //datamızı yolluyoruz
        contentType: 'application/json; charset=utf-8',
        dataType: 'json',
        success: function (result) {
            if (result.d != "Hata") {
                Toastinette.show('success', 2000 + Math.random() * 5000, result.d);
            }
            else {
                Toastinette.show('error', 2000 + Math.random() * 5000, "İşleminiz sırasında bir hata meydana geldi.");
            }

        },
        error: function () {
            Toastinette.show('error', 2000 + Math.random() * 5000, result.d);
        }
    });
};



function peryetki(element) {
    var id = element.value;  //id değerini alıyoruz

    var fields = id.split('-');
    var birinci = fields[0];
    var ikinci = fields[1];

    //Checkbox işaretlimi değilmi kontrol ediliyoru.
    if (element.checked) {
        var durum = "1";
    }
    else {
        var durum = "0";
    }

    $.ajax({
        type: 'POST',
        url: '/Modul/Komut.aspx/peryetki',  //işlem yaptığımız sayfayı belirtiyoruz
        data: '{ perid:' + birinci + ', rolid:' + ikinci + ',durum:' + durum + '}', //datamızı yolluyoruz
        contentType: 'application/json; charset=utf-8',
        dataType: 'json',
        success: function (result) {
            if (result.d != "Hata") {
                Toastinette.show('success', 2000 + Math.random() * 5000, result.d);
            }
            else {
                Toastinette.show('error', 2000 + Math.random() * 5000, "İşleminiz sırasında bir hata meydana geldi.");
            }

        },
        error: function () {
            Toastinette.show('error', 2000 + Math.random() * 5000, result.d);
        }
    });
};





function yetkipersonelist(element) {
    var id = element.value;  //id değerini alıyoruz

    var fields = id.split('-');
    var birinci = fields[0];
    var ikinci = fields[1];

    //Checkbox işaretlimi değilmi kontrol ediliyoru.
    if (element.checked) {
        var durum = "1";
    }
    else {
        var durum = "0";
    }

    $.ajax({
        type: 'POST',
        url: '/Modul/Komut.aspx/yetkipersonelist',  //işlem yaptığımız sayfayı belirtiyoruz
        data: '{ perid:' + birinci + ', yetkiid:' + ikinci + ',durum:' + durum + '}', //datamızı yolluyoruz
        contentType: 'application/json; charset=utf-8',
        dataType: 'json',
        success: function (result) {
            if (result.d != "Hata") {
                Toastinette.show('success', 2000 + Math.random() * 5000, result.d);
            }
            else {
                Toastinette.show('error', 2000 + Math.random() * 5000, "İşleminiz sırasında bir hata meydana geldi.");
            }

        },
        error: function () {
            Toastinette.show('error', 2000 + Math.random() * 5000, result.d);
        }
    });
};


function yetkipersonelistsil(element) {
    var id = element.value;  //id değerini alıyoruz

    var fields = id.split('-');
    var birinci = fields[0];
    var ikinci = fields[1];

    //Checkbox işaretlimi değilmi kontrol ediliyoru.
    if (element.checked) {
        var durum = "1";
    }
    else {
        var durum = "0";
    }

    $.ajax({
        type: 'POST',
        url: '/Modul/Komut.aspx/yetkipersonelistsil',  //işlem yaptığımız sayfayı belirtiyoruz
        data: '{ perid:' + birinci + ', yetkiid:' + ikinci + ',durum:' + durum + '}', //datamızı yolluyoruz
        contentType: 'application/json; charset=utf-8',
        dataType: 'json',
        success: function (result) {
            if (result.d != "Hata") {
                Toastinette.show('success', 2000 + Math.random() * 5000, result.d);
            }
            else {
                Toastinette.show('error', 2000 + Math.random() * 5000, "İşleminiz sırasında bir hata meydana geldi.");
            }

        },
        error: function () {
            Toastinette.show('error', 2000 + Math.random() * 5000, result.d);
        }
    });
};




//Personel listesinde Sisteme Girişin Kontrolünü Yapar
function cinsiyet(element) {
    var id = element.value;  //id değerini alıyoruz

    //Checkbox işaretlimi değilmi kontrol ediliyoru.
    durum = "0";
    
    


    $.ajax({
        type: 'POST',
        url: '/Modul/Komut.aspx/cinsiyet',  //işlem yaptığımız sayfayı belirtiyoruz
        data: '{ id:' + id + ',durum:' + durum + '}', //datamızı yolluyoruz
        contentType: 'application/json; charset=utf-8',
        dataType: 'json',
        success: function (result) {
            if (result.d != "Hata") {
                Toastinette.show('success', 2000 + Math.random() * 5000, result.d);
            }
            else {
                Toastinette.show('error', 2000 + Math.random() * 5000, "İşleminiz sırasında bir hata meydana geldi.");
            }

        },
        error: function () {
            Toastinette.show('error', 2000 + Math.random() * 5000, result.d);
        }
    });
};





//Personel listesinde Sisteme Girişin Kontrolünü Yapar
function cinsiyet2(element) {
    var id = element.value;  //id değerini alıyoruz

    //Checkbox işaretlimi değilmi kontrol ediliyoru.
    durum = "1";    

    $.ajax({
        type: 'POST',
        url: '/Modul/Komut.aspx/cinsiyet',  //işlem yaptığımız sayfayı belirtiyoruz
        data: '{ id:' + id + ',durum:' + durum + '}', //datamızı yolluyoruz
        contentType: 'application/json; charset=utf-8',
        dataType: 'json',
        success: function (result) {
            if (result.d != "Hata") {
                Toastinette.show('success', 2000 + Math.random() * 5000, result.d);
            }
            else {
                Toastinette.show('error', 2000 + Math.random() * 5000, "İşleminiz sırasında bir hata meydana geldi.");
            }

        },
        error: function () {
            Toastinette.show('error', 2000 + Math.random() * 5000, result.d);
        }
    });
};