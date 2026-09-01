// sonuç girişi ızgarasında yazarken toplam neti anlık gösteriyoruz, klavyeyle hücreler arası atlıyoruz, satır bittiğinde otomatik kaydediyoruz ve önceki denemeden doldurma düğmesini burada yönetiyoruz
(function () {
  var form = document.getElementById("sonuc-girisi-formu");
  var tablo = document.getElementById("sonuc-girisi-tablosu");
  if (!form || !tablo) {
    return;
  }

  var denemeId = form.getAttribute("data-deneme-id");
  var sinifId = form.getAttribute("data-sinif-id");

  // form tag helper post formuna doğrulama alanını kendiliğinden ekliyor, fetch isteklerinde aynı değeri başlıkla gönderiyoruz
  var tokenGirdisi = form.querySelector('input[name="__RequestVerificationToken"]');
  var token = tokenGirdisi ? tokenGirdisi.value : "";

  // ---- anlık net hesabı ----

  function satiriHesapla(satirEl) {
    var girdiler = satirEl.querySelectorAll(".net-girdi");
    var toplamNet = 0;

    // her ders için sırayla doğru, yanlış, boş üçlüsü geliyor, dördü bir doğruyu götürüyor
    for (var k = 0; k < girdiler.length; k += 3) {
      var dogru = parseInt(girdiler[k].value, 10) || 0;
      var yanlis = parseInt(girdiler[k + 1].value, 10) || 0;
      toplamNet += dogru - yanlis / 4;
    }

    var satirNo = satirEl.getAttribute("data-satir");
    var hedef = document.getElementById("net-toplam-" + satirNo);
    if (hedef) {
      hedef.textContent = (Math.round(toplamNet * 100) / 100).toString();
    }
  }

  tablo.addEventListener("input", function (olay) {
    if (!olay.target.classList.contains("net-girdi")) {
      return;
    }
    satiriHesapla(olay.target.closest("tr"));
  });

  tablo.querySelectorAll("tbody tr").forEach(function (satirEl) {
    satiriHesapla(satirEl);
  });

  // ---- klavye ile hücre atlama ----

  function tumGirdiler() {
    return Array.prototype.slice.call(tablo.querySelectorAll(".net-girdi"));
  }

  // enter tuşu formu erken göndermesin diye burada yakalanıp tab gibi bir sonraki hücreye geçiriyoruz
  function odakSonrakineTasi(hedefGirdi) {
    var girdiler = tumGirdiler();
    var index = girdiler.indexOf(hedefGirdi);
    if (index >= 0 && index < girdiler.length - 1) {
      girdiler[index + 1].focus();
      girdiler[index + 1].select();
    }
  }

  // yukarı aşağı ok tuşları aynı sütunda bir üst veya alt satıra geçiyor, tek bir dersin bütün sınıf sonuçlarını art arda girerken işe yarıyor
  function odakDikeyTasi(hedefGirdi, yon) {
    var satirEl = hedefGirdi.closest("tr");
    var satirGirdileri = Array.prototype.slice.call(satirEl.querySelectorAll(".net-girdi"));
    var sutunIndex = satirGirdileri.indexOf(hedefGirdi);

    var hedefSatirNo = parseInt(satirEl.getAttribute("data-satir"), 10) + yon;
    var hedefSatirEl = tablo.querySelector('tbody tr[data-satir="' + hedefSatirNo + '"]');
    if (!hedefSatirEl) {
      return;
    }

    var hedefGirdiler = Array.prototype.slice.call(hedefSatirEl.querySelectorAll(".net-girdi"));
    if (hedefGirdiler[sutunIndex]) {
      hedefGirdiler[sutunIndex].focus();
      hedefGirdiler[sutunIndex].select();
    }
  }

  tablo.addEventListener("keydown", function (olay) {
    if (!olay.target.classList.contains("net-girdi")) {
      return;
    }

    if (olay.key === "Enter") {
      olay.preventDefault();
      odakSonrakineTasi(olay.target);
    } else if (olay.key === "ArrowDown") {
      olay.preventDefault();
      odakDikeyTasi(olay.target, 1);
    } else if (olay.key === "ArrowUp") {
      olay.preventDefault();
      odakDikeyTasi(olay.target, -1);
    }
  });

  // ---- satır bazlı otomatik kayıt ----

  var bekleyenZamanlayicilar = {};

  function satiriDtoyaCevir(satirEl) {
    var idGirdisi = satirEl.querySelector(".satir-sonuc-id");
    var katilmadiGirdisi = satirEl.querySelector('input[type="checkbox"]');
    var detaylar = [];

    satirEl.querySelectorAll(".satir-detay-id").forEach(function (detayIdGirdisi) {
      var testId = detayIdGirdisi.getAttribute("data-test-id");
      var dogruGirdi = satirEl.querySelector('.net-girdi[data-test-id="' + testId + '"][data-alan="dogru"]');
      var yanlisGirdi = satirEl.querySelector('.net-girdi[data-test-id="' + testId + '"][data-alan="yanlis"]');
      var bosGirdi = satirEl.querySelector('.net-girdi[data-test-id="' + testId + '"][data-alan="bos"]');

      detaylar.push({
        id: parseInt(detayIdGirdisi.value, 10) || 0,
        sinavTuruTestId: parseInt(testId, 10),
        dogru: parseInt(dogruGirdi.value, 10) || 0,
        yanlis: parseInt(yanlisGirdi.value, 10) || 0,
        bos: parseInt(bosGirdi.value, 10) || 0
      });
    });

    return {
      id: parseInt(idGirdisi.value, 10) || 0,
      denemeId: parseInt(denemeId, 10),
      ogrenciId: parseInt(satirEl.getAttribute("data-ogrenci-id"), 10),
      katilmadiMi: katilmadiGirdisi ? katilmadiGirdisi.checked : false,
      detaylar: detaylar
    };
  }

  function durumGoster(satirNo, metin, hataMi) {
    var durumEl = document.getElementById("satir-durum-" + satirNo);
    if (!durumEl) {
      return;
    }

    durumEl.textContent = metin;
    durumEl.classList.toggle("rp-satir-durum-hata", hataMi);
    durumEl.classList.add("rp-satir-durum-goster");

    window.clearTimeout(durumEl._gizlemeZamanlayicisi);
    durumEl._gizlemeZamanlayicisi = window.setTimeout(function () {
      durumEl.classList.remove("rp-satir-durum-goster");
    }, 2000);
  }

  function satiriKaydet(satirEl) {
    var satirNo = satirEl.getAttribute("data-satir");

    fetch("/Denemeler/SatirKaydet", {
      method: "POST",
      headers: {
        "Content-Type": "application/json",
        "X-CSRF-TOKEN": token
      },
      body: JSON.stringify(satiriDtoyaCevir(satirEl))
    })
      .then(function (yanit) {
        return yanit.json();
      })
      .then(function (sonuc) {
        if (!sonuc.basarili) {
          durumGoster(satirNo, "Kaydedilemedi", true);
          return;
        }

        if (sonuc.atlandi) {
          return;
        }

        // yeni satırın id'sini forma yazmazsak bir sonraki otomatik kayıtta aynı sonuç ikinci kez eklenmeye çalışılır
        var idGirdisi = satirEl.querySelector(".satir-sonuc-id");
        idGirdisi.value = sonuc.id;

        (sonuc.detaylar || []).forEach(function (detay) {
          var detayIdGirdisi = satirEl.querySelector('.satir-detay-id[data-test-id="' + detay.sinavTuruTestId + '"]');
          if (detayIdGirdisi) {
            detayIdGirdisi.value = detay.id;
          }
        });

        durumGoster(satirNo, "Kaydedildi", false);
      })
      .catch(function () {
        durumGoster(satirNo, "Bağlantı hatası", true);
      });
  }

  // satır bitince hemen değil, kısa bir bekleme sonunda kaydediyoruz, yoksa d y b girilirken üç ayrı istek atılır
  function satirKaydiniZamanla(satirEl) {
    var satirNo = satirEl.getAttribute("data-satir");
    window.clearTimeout(bekleyenZamanlayicilar[satirNo]);
    bekleyenZamanlayicilar[satirNo] = window.setTimeout(function () {
      satiriKaydet(satirEl);
    }, 600);
  }

  tablo.addEventListener("change", function (olay) {
    var hedef = olay.target;
    if (!hedef.classList.contains("net-girdi") && hedef.type !== "checkbox") {
      return;
    }
    satirKaydiniZamanla(hedef.closest("tr"));
  });

  // ---- önceki denemeden doldurma ----

  var oncekiDoldurDugmesi = document.getElementById("onceki-denemeden-doldur");
  if (!oncekiDoldurDugmesi) {
    return;
  }

  oncekiDoldurDugmesi.addEventListener("click", function () {
    oncekiDoldurDugmesi.disabled = true;

    fetch("/Denemeler/OncekiSonuclar?denemeId=" + denemeId + "&sinifId=" + sinifId)
      .then(function (yanit) {
        return yanit.json();
      })
      .then(function (oncekiListe) {
        var etkilenenSatirlar = [];

        oncekiListe.forEach(function (oncekiOgrenci) {
          var satirEl = tablo.querySelector('tbody tr[data-ogrenci-id="' + oncekiOgrenci.ogrenciId + '"]');
          if (!satirEl) {
            return;
          }

          // öğrencinin bu deneme için zaten kayıtlı bir sonucu varsa veya satıra elle bir şey girilmişse dokunmuyoruz
          var idGirdisi = satirEl.querySelector(".satir-sonuc-id");
          var sonucKayitliMi = (parseInt(idGirdisi.value, 10) || 0) !== 0;
          var satiraElleGirilmisMi = Array.prototype.some.call(satirEl.querySelectorAll(".net-girdi"), function (girdi) {
            return (parseInt(girdi.value, 10) || 0) !== 0;
          });

          if (sonucKayitliMi || satiraElleGirilmisMi) {
            return;
          }

          (oncekiOgrenci.detaylar || []).forEach(function (detay) {
            ["dogru", "yanlis", "bos"].forEach(function (alan) {
              var girdi = satirEl.querySelector('.net-girdi[data-test-id="' + detay.sinavTuruTestId + '"][data-alan="' + alan + '"]');
              if (girdi) {
                girdi.value = detay[alan];
              }
            });
          });

          satiriHesapla(satirEl);
          etkilenenSatirlar.push(satirEl);
        });

        etkilenenSatirlar.forEach(function (satirEl) {
          satirKaydiniZamanla(satirEl);
        });

        oncekiDoldurDugmesi.disabled = false;
      })
      .catch(function () {
        oncekiDoldurDugmesi.disabled = false;
      });
  });
})();
