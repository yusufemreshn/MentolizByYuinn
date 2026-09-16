// devamsızlık listesinde bir öğrencinin durumu değiştirilince sayfa yenilenmeden fetch ile arka planda kaydediyoruz
(function () {
  var liste = document.getElementById("devamsizlik-listesi");
  if (!liste) {
    return;
  }

  var sinifId = liste.getAttribute("data-sinif-id");
  var tarih = liste.getAttribute("data-tarih");

  var tokenGirdisi = document.querySelector('#devamsizlik-token-formu input[name="__RequestVerificationToken"]');
  var token = tokenGirdisi ? tokenGirdisi.value : "";

  liste.addEventListener("change", function (olay) {
    var secim = olay.target;
    if (!secim.classList.contains("devamsizlik-durum-secim")) {
      return;
    }

    var satir = secim.closest(".devamsizlik-satiri");
    var ogrenciId = satir.getAttribute("data-ogrenci-id");

    var veri = new FormData();
    veri.append("OgrenciId", ogrenciId);
    veri.append("Tarih", tarih);
    veri.append("Durum", secim.value);
    veri.append("sinifId", sinifId);
    veri.append("__RequestVerificationToken", token);

    secim.disabled = true;

    fetch("/Devamsizlik/DurumKaydet", {
      method: "POST",
      headers: { "X-Requested-With": "XMLHttpRequest" },
      body: veri
    })
      .then(function (yanit) {
        return yanit.json();
      })
      .then(function (sonuc) {
        if (!sonuc.basarili) {
          window.alert(sonuc.mesaj || "Devamsızlık durumu kaydedilemedi.");
        }
      })
      .catch(function () {
        window.alert("Bağlantı hatası, devamsızlık durumu kaydedilemedi.");
      })
      .finally(function () {
        secim.disabled = false;
      });
  });
})();
