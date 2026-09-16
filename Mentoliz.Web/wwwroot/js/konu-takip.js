// konu takip haritasında bir durum değiştirilince sayfa tepeden yenilenip aşağı kaydırma pozisyonunu kaybetmesin diye fetch ile arka planda kaydediyoruz
(function () {
  var harita = document.getElementById("konu-takip-haritasi");
  if (!harita) {
    return;
  }

  // durum değerine göre kutunun arkaplan rengi, view'daki switch ile birebir aynı eşleme
  var ARKAPLAN_SINIFLARI = {
    Baslanmadi: "bg-secondary-subtle",
    DevamEdiyor: "bg-warning-subtle",
    Tamamlandi: "bg-success-subtle",
    TekrarGerekli: "bg-danger-subtle"
  };

  harita.addEventListener("change", function (olay) {
    var secim = olay.target;
    if (!secim.classList.contains("konu-durum-secim")) {
      return;
    }

    var form = secim.closest("form");
    var kutu = secim.closest(".konu-durum-kutusu");
    // token dahil formdaki bütün alanlar fetch gövdesine olduğu gibi taşınıyor, ayrıca elle eklemeye gerek kalmıyor
    var veri = new FormData(form);

    secim.disabled = true;

    fetch(form.action, {
      method: "POST",
      headers: { "X-Requested-With": "XMLHttpRequest" },
      body: veri
    })
      .then(function (yanit) {
        return yanit.json();
      })
      .then(function (sonuc) {
        if (!sonuc.basarili) {
          window.alert(sonuc.mesaj || "Konu durumu kaydedilemedi.");
          return;
        }

        if (kutu) {
          Object.keys(ARKAPLAN_SINIFLARI).forEach(function (anahtar) {
            kutu.classList.remove(ARKAPLAN_SINIFLARI[anahtar]);
          });
          kutu.classList.add(ARKAPLAN_SINIFLARI[secim.value]);
        }
      })
      .catch(function () {
        window.alert("Bağlantı hatası, konu durumu kaydedilemedi.");
      })
      .finally(function () {
        secim.disabled = false;
      });
  });
})();
