// ctrl+k veya arama düğmeleriyle açılan hızlı arama paleti, öğrenci arıyor ve sayfalar arasında hızlı geçiş yaptırıyor
(function () {
  var perde = document.getElementById("komut-paleti-perde");
  var girdi = document.getElementById("komut-paleti-girdi");
  var sonuclarEl = document.getElementById("komut-paleti-sonuclar");
  // hem üst çubuktaki büyüteç hem alt sekme çubuğundaki ortadaki düğme bu paleti açıyor
  var acButonlari = document.querySelectorAll(".rp-komut-ac");
  if (!perde || !girdi || !sonuclarEl) {
    return;
  }

  // sayfa kısayolları layout'taki gizli listeden kendiliğinden toplanıyor, ayrı bir liste elde tutmaya gerek kalmıyor
  var sayfaKisayollari = Array.prototype.map.call(document.querySelectorAll(".rp-komut-sayfa-listesi a"), function (a) {
    var ikonEl = a.querySelector("i");
    return {
      baslik: a.textContent.trim(),
      alt: "Sayfa",
      url: a.getAttribute("href"),
      ikon: ikonEl ? ikonEl.className : "bi bi-arrow-right"
    };
  });

  var aktifIndex = -1;
  var aramaZamanlayici = null;

  function ac() {
    // "diğer" paneli açık kalmışsa komut paletiyle üst üste binmesin diye önce onu kapatıyoruz
    var digerPaneli = document.getElementById("rp-diger-sayfalar");
    if (digerPaneli && window.bootstrap) {
      var digerOrnek = window.bootstrap.Offcanvas.getInstance(digerPaneli);
      if (digerOrnek) {
        digerOrnek.hide();
      }
    }

    perde.classList.remove("d-none");
    girdi.setAttribute("aria-expanded", "true");
    girdi.value = "";
    sonucGoster(sayfaKisayollari);
    window.setTimeout(function () {
      girdi.focus();
    }, 0);
  }

  function kapat() {
    perde.classList.add("d-none");
    girdi.setAttribute("aria-expanded", "false");
  }

  function sonucGoster(ogeler) {
    sonuclarEl.innerHTML = "";

    if (ogeler.length === 0) {
      var bosSatir = document.createElement("li");
      bosSatir.className = "rp-komut-paleti-bos";
      bosSatir.textContent = "Sonuç yok.";
      sonuclarEl.appendChild(bosSatir);
      aktifIndex = -1;
      return;
    }

    ogeler.forEach(function (oge, index) {
      var satir = document.createElement("li");
      satir.className = "rp-komut-paleti-satir";
      satir.id = "komut-paleti-satir-" + index;
      satir.setAttribute("role", "option");
      satir.setAttribute("aria-selected", "false");
      satir.innerHTML =
        '<i class="' + oge.ikon + '"></i>' +
        '<span class="rp-komut-paleti-satir-metin"><strong></strong><small></small></span>';
      satir.querySelector("strong").textContent = oge.baslik;
      satir.querySelector("small").textContent = oge.alt;
      satir.addEventListener("click", function () {
        window.location.href = oge.url;
      });
      satir.addEventListener("mouseenter", function () {
        seciliYap(index);
      });
      sonuclarEl.appendChild(satir);
    });

    seciliYap(0);
  }

  function seciliYap(index) {
    var satirlar = sonuclarEl.querySelectorAll(".rp-komut-paleti-satir");
    satirlar.forEach(function (satir) {
      satir.classList.remove("rp-komut-paleti-secili");
      satir.setAttribute("aria-selected", "false");
    });
    if (satirlar[index]) {
      satirlar[index].classList.add("rp-komut-paleti-secili");
      satirlar[index].setAttribute("aria-selected", "true");
      satirlar[index].scrollIntoView({ block: "nearest" });
      girdi.setAttribute("aria-activedescendant", satirlar[index].id);
      aktifIndex = index;
    }
  }

  // türkçe karakterlerde küçük harfe çevirmenin doğru çalışması için toLocaleLowerCase kullanıyoruz
  function aramaYap(sorgu) {
    var kisayolEslesenler = sayfaKisayollari.filter(function (k) {
      return k.baslik.toLocaleLowerCase("tr").indexOf(sorgu.toLocaleLowerCase("tr")) !== -1;
    });

    if (sorgu.trim().length < 2) {
      sonucGoster(kisayolEslesenler);
      return;
    }

    fetch("/Arama/Ogrenciler?q=" + encodeURIComponent(sorgu))
      .then(function (yanit) {
        return yanit.json();
      })
      .then(function (ogrenciSonuclari) {
        var ogrenciOgeleri = ogrenciSonuclari.map(function (o) {
          return { baslik: o.baslik, alt: o.alt, url: o.url, ikon: "bi bi-person" };
        });
        sonucGoster(ogrenciOgeleri.concat(kisayolEslesenler));
      });
  }

  girdi.addEventListener("input", function () {
    window.clearTimeout(aramaZamanlayici);
    var deger = girdi.value;
    aramaZamanlayici = window.setTimeout(function () {
      aramaYap(deger);
    }, 200);
  });

  girdi.addEventListener("keydown", function (olay) {
    var satirlar = sonuclarEl.querySelectorAll(".rp-komut-paleti-satir");
    if (olay.key === "ArrowDown") {
      olay.preventDefault();
      seciliYap(Math.min(aktifIndex + 1, satirlar.length - 1));
    } else if (olay.key === "ArrowUp") {
      olay.preventDefault();
      seciliYap(Math.max(aktifIndex - 1, 0));
    } else if (olay.key === "Enter") {
      olay.preventDefault();
      if (satirlar[aktifIndex]) {
        satirlar[aktifIndex].click();
      }
    } else if (olay.key === "Escape") {
      kapat();
    }
  });

  perde.addEventListener("click", function (olay) {
    if (olay.target === perde) {
      kapat();
    }
  });

  acButonlari.forEach(function (buton) {
    buton.addEventListener("click", ac);
  });

  // sayfanın herhangi bir yerinden ctrl+k (mac'te cmd+k) ile açılabiliyor
  document.addEventListener("keydown", function (olay) {
    var kIleAcma = (olay.ctrlKey || olay.metaKey) && olay.key.toLowerCase() === "k";
    if (kIleAcma) {
      olay.preventDefault();
      ac();
    } else if (olay.key === "Escape" && !perde.classList.contains("d-none")) {
      kapat();
    }
  });
})();
