// görüşme formundaki şablon doldurma ve öğrenci özeti yan panelini yönetiyor
(function () {
  var form = document.getElementById("gorusme-formu");
  if (!form) {
    return;
  }

  // sık tekrarlanan görüşme tipleri için hazır metin iskeletleri, sadece boş alanları dolduruyor
  var sablonlar = [
    {
      baslik: "Deneme değerlendirme görüşmesi",
      konu: "Deneme sonuçlarının değerlendirilmesi",
      notlar: "Son deneme sonuçları birlikte incelendi. Güçlü ve zayıf dersler konuşuldu, önümüzdeki döneme yönelik çalışma önceliği belirlendi."
    },
    {
      baslik: "Devamsızlık görüşmesi",
      konu: "Devamsızlık durumu",
      notlar: "Son dönemdeki devamsızlık durumu konuşuldu, sebepleri soruldu. Devam düzeninin toparlanması için birlikte plan yapıldı."
    },
    {
      baslik: "Motivasyon görüşmesi",
      konu: "Motivasyon ve çalışma düzeni",
      notlar: "Öğrencinin genel motivasyonu ve çalışma düzeni konuşuldu. Kısa vadeli, ulaşılabilir hedefler üzerinde duruldu."
    },
    {
      baslik: "Veli bilgilendirme görüşmesi",
      konu: "Veli bilgilendirmesi",
      notlar: "Veliyle öğrencinin genel durumu, deneme sonuçları ve ödev düzeni hakkında bilgi paylaşıldı."
    },
    {
      baslik: "Hedef güncelleme görüşmesi",
      konu: "Hedef gözden geçirme",
      notlar: "Mevcut hedef güncel durumla karşılaştırıldı, gerekiyorsa hedef üzerinde küçük revizyonlar yapıldı."
    }
  ];

  var sablonSecimi = document.getElementById("gorusme-sablon-secimi");
  var konuGirdisi = document.getElementById("gorusme-konu");
  var notlarGirdisi = document.getElementById("gorusme-notlar");

  if (sablonSecimi) {
    sablonlar.forEach(function (sablon, index) {
      var secenek = document.createElement("option");
      secenek.value = String(index);
      secenek.textContent = sablon.baslik;
      sablonSecimi.appendChild(secenek);
    });

    var sablonZamanlayici = null;

    // kapalı bir select'te ok tuşlarıyla gezinirken her adımda ayrı change olayı ateşleniyor, kısa bir bekleme ile sadece gezinme bitince doluyoruz
    sablonSecimi.addEventListener("change", function () {
      window.clearTimeout(sablonZamanlayici);
      sablonZamanlayici = window.setTimeout(function () {
        if (sablonSecimi.value === "") {
          return;
        }

        var sablon = sablonlar[parseInt(sablonSecimi.value, 10)];
        if (konuGirdisi && konuGirdisi.value.trim() === "") {
          konuGirdisi.value = sablon.konu;
        }
        if (notlarGirdisi && notlarGirdisi.value.trim() === "") {
          notlarGirdisi.value = sablon.notlar;
        }
      }, 200);
    });
  }

  // ---- öğrenci özeti yan paneli ----

  var ogrenciSecimi = document.getElementById("gorusme-ogrenci-secimi");
  var bosMesaj = document.getElementById("gorusme-ozet-bos-mesaji");
  var icerik = document.getElementById("gorusme-ozet-icerik");
  var netlerAlani = document.getElementById("gorusme-ozet-netler");
  var odevAlani = document.getElementById("gorusme-ozet-odev");
  var kararAlani = document.getElementById("gorusme-ozet-karar");

  function ozetiTemizle() {
    if (bosMesaj) {
      bosMesaj.classList.remove("d-none");
    }
    if (icerik) {
      icerik.classList.add("d-none");
    }
  }

  function ozetiGoster(ozet) {
    if (!bosMesaj || !icerik) {
      return;
    }

    bosMesaj.classList.add("d-none");
    icerik.classList.remove("d-none");

    netlerAlani.innerHTML = "";
    if (!ozet.sonDenemeNetleri || ozet.sonDenemeNetleri.length === 0) {
      netlerAlani.innerHTML = '<span class="text-body-secondary small">Henüz deneme sonucu yok.</span>';
    } else {
      ozet.sonDenemeNetleri.forEach(function (net) {
        var satir = document.createElement("div");
        satir.className = "d-flex justify-content-between small";
        satir.innerHTML = "<span></span><strong></strong>";
        satir.querySelector("span").textContent = net.denemeAdi;
        satir.querySelector("strong").textContent = net.toplamNet.toFixed(2).replace(".", ",");
        netlerAlani.appendChild(satir);
      });
    }

    odevAlani.textContent = ozet.acikOdevSayisi + " açık ödev";
    kararAlani.textContent = ozet.sonAlinanKarar ? ozet.sonAlinanKarar : "Kayıtlı karar yok.";
  }

  function ozetiYukle(ogrenciId) {
    if (!ogrenciId) {
      ozetiTemizle();
      return;
    }

    fetch("/Gorusmeler/HazirlikOzeti?ogrenciId=" + encodeURIComponent(ogrenciId))
      .then(function (yanit) {
        return yanit.json();
      })
      .then(ozetiGoster)
      .catch(ozetiTemizle);
  }

  if (ogrenciSecimi) {
    ogrenciSecimi.addEventListener("change", function () {
      ozetiYukle(ogrenciSecimi.value);
    });

    if (ogrenciSecimi.value) {
      ozetiYukle(ogrenciSecimi.value);
    }
  }
})();
