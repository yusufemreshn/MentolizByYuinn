// data-onay özelliği taşıyan formlar artık tarayıcının çirkin confirm() kutusu yerine kendi onay modalımızı açıyor
// kullanıcı "evet" deyince aynı form data-onay-gecildi işaretiyle tekrar gönderiliyor, bu dinleyici o sefer araya girmiyor
(function () {
  var bekleyenForm = null;

  document.addEventListener("submit", function (olay) {
    var form = olay.target;
    var onayMesaji = form.getAttribute("data-onay");
    if (!onayMesaji || form.dataset.onayGecildi === "evet") {
      return;
    }

    olay.preventDefault();

    var modalEleman = document.getElementById("rp-onay-modali");
    if (!modalEleman || !window.bootstrap) {
      // modal her nasılsa sayfada yoksa form hiç gönderilmeden kalmasın diye eski yönteme düşüyoruz
      if (window.confirm(onayMesaji)) {
        form.submit();
      }
      return;
    }

    modalEleman.querySelector(".rp-onay-mesaj").textContent = onayMesaji;
    bekleyenForm = form;
    window.bootstrap.Modal.getOrCreateInstance(modalEleman).show();
  });

  document.addEventListener("DOMContentLoaded", function () {
    var onaylaDugmesi = document.getElementById("rp-onay-onayla");
    if (!onaylaDugmesi) {
      return;
    }

    onaylaDugmesi.addEventListener("click", function () {
      if (!bekleyenForm) {
        return;
      }

      window.bootstrap.Modal.getOrCreateInstance(document.getElementById("rp-onay-modali")).hide();

      var form = bekleyenForm;
      bekleyenForm = null;
      form.dataset.onayGecildi = "evet";
      form.requestSubmit();
    });
  });
})();

// tema seçeneklerinden biri işaretlenince form kendiliğinden gönderiliyor, ayrı bir uygula düğmesine gerek kalmıyor
document.addEventListener("change", function (olay) {
  if (olay.target.classList.contains("rp-tema-radyo")) {
    olay.target.form.submit();
  }
});

// adres çubuğunda bir sekme kimliği varsa, örneğin bir filtreden sonra sayfa yenilendiğinde o sekmeyi açık gösteriyoruz
document.addEventListener("DOMContentLoaded", function () {
  var hedefId = window.location.hash.replace("#", "");
  if (!hedefId) {
    return;
  }

  var sekmeDugmesi = document.querySelector('[data-bs-target="#' + hedefId + '"]');
  if (sekmeDugmesi && window.bootstrap) {
    new window.bootstrap.Tab(sekmeDugmesi).show();
  }
});

// controller'ın tempdata ile bıraktığı bildirim verisini okuyup sağ üstte bir toast olarak gösteriyoruz
var RP_TOAST_IKONLARI = {
  basarili: "bi-check-circle-fill",
  hata: "bi-x-circle-fill",
  uyari: "bi-exclamation-triangle-fill"
};

function rpToastOlustur(tur, mesaj) {
  var alan = document.getElementById("rp-toast-alani");
  if (!alan) {
    return;
  }

  var toast = document.createElement("div");
  toast.className = "rp-toast rp-toast-" + tur;
  toast.setAttribute("role", "status");

  var ikon = document.createElement("i");
  ikon.className = "bi " + (RP_TOAST_IKONLARI[tur] || "bi-info-circle-fill");

  var metin = document.createElement("span");
  metin.className = "rp-toast-metin";
  metin.textContent = mesaj;

  var kapatDugmesi = document.createElement("button");
  kapatDugmesi.type = "button";
  kapatDugmesi.className = "rp-toast-kapat";
  kapatDugmesi.setAttribute("aria-label", "Kapat");
  kapatDugmesi.innerHTML = '<i class="bi bi-x"></i>';

  toast.append(ikon, metin, kapatDugmesi);
  alan.appendChild(toast);

  // sınıf bir sonraki karede eklenmezse css transition tetiklenmiyor, tarayıcı ilk hali görmeden geçişi başlatamıyor
  requestAnimationFrame(function () {
    toast.classList.add("rp-toast-goster");
  });

  var kapat = function () {
    toast.classList.remove("rp-toast-goster");
    toast.addEventListener("transitionend", function () {
      toast.remove();
    }, { once: true });
  };

  kapatDugmesi.addEventListener("click", kapat);

  // hata mesajları kullanıcı bilerek kapatana kadar ekranda kalıyor, başarı ve uyarı kendiliğinden kayboluyor
  if (tur !== "hata") {
    setTimeout(kapat, 4000);
  }
}

document.addEventListener("DOMContentLoaded", function () {
  document.querySelectorAll(".rp-bildirim-verisi").forEach(function (veri) {
    rpToastOlustur(veri.getAttribute("data-tur"), veri.getAttribute("data-mesaj"));
    veri.remove();
  });
});

// üst menüde ve alt sekme çubuğunda sayfalar arası geçiş, tarayıcının view transitions api'si sayfalar arası yükseklik farkında pencereyi küçültüp büyütüyordu, burada kendi elimizle kontrol ediyoruz
(function () {
  var ONCEKI_LINK_ANAHTARI = "mentoliz-onceki-menu-href";
  var hareketAzaltilsinMi = window.matchMedia("(prefers-reduced-motion: reduce)").matches;
  var icerik = document.querySelector(".rp-sayfa-icerigi");
  // kayan aktif çizgi sadece üst menüde var, alt sekme çubuğu kendi renk değişimiyle yetiniyor
  var aktifLink = document.querySelector(".rp-ustmenu a.active");
  var aktifCizgi = aktifLink ? aktifLink.querySelector(".rp-aktif-cizgi") : null;

  if (hareketAzaltilsinMi) {
    return;
  }

  // bu sayfaya gelmeden önce hangi link aktifti, o bilgi bir önceki sayfadan session storage ile taşınıyor
  var oncekiHref = sessionStorage.getItem(ONCEKI_LINK_ANAHTARI);
  sessionStorage.removeItem(ONCEKI_LINK_ANAHTARI);

  // aktif çizgi eski linkle yeni link arasındaki yatay farkı css değişkenine yazıp animasyonu tetikliyor, animation transition'dan farklı olarak bir önceki kareyi beklemeden direkt o farktan başlayıp kayıyor
  if (aktifCizgi && oncekiHref) {
    var oncekiLink = document.querySelector('.rp-ustmenu a[href="' + oncekiHref + '"]');
    if (oncekiLink && oncekiLink !== aktifLink) {
      var fark = oncekiLink.offsetLeft - aktifLink.offsetLeft;
      aktifCizgi.style.setProperty("--rp-cizgi-eski-x", fark + "px");
      aktifCizgi.classList.add("rp-cizgi-tasin");
    }
  }

  // içeriğin aşağıdan gelme animasyonu artık site.css'te kendiliğinden çalışıyor, burada ayrıca bir şey yapmaya gerek yok
  // ama içerik aşağıdan gelirken transform yüzünden bir anlığına viewport'un altına taşıyor, sağda kaydırma çubuğu belirip kaybolmasın diye o kısa süre boyunca kaydırmayı kapatıyoruz
  var eskiTasmaDegeri = document.documentElement.style.overflowY;
  document.documentElement.style.overflowY = "hidden";
  setTimeout(function () {
    document.documentElement.style.overflowY = eskiTasmaDegeri;
  }, 340);

  // üst menüden, alt sekme çubuğundan veya diğer panelinden başka bir sayfaya geçerken önce içerik solup gitsin, hemen ardından gerçek sayfa değişimi tetiklensin
  document.querySelectorAll(".rp-nav-link, .rp-diger-liste a").forEach(function (link) {
    link.addEventListener("click", function (olay) {
      // yeni sekmede açma, ctrl/cmd ile tıklama gibi durumlara karışmıyoruz, tarayıcı kendi işini yapsın
      if (olay.defaultPrevented || olay.button !== 0 || olay.ctrlKey || olay.metaKey || olay.shiftKey || olay.altKey) {
        return;
      }
      if (link.classList.contains("active") || !icerik) {
        return;
      }

      olay.preventDefault();
      // yeni sayfada aktif çizginin nereden kayacağını bilmesi için şu an aktif olan linki saklıyoruz, tıklanan linki değil
      if (aktifLink) {
        sessionStorage.setItem(ONCEKI_LINK_ANAHTARI, aktifLink.getAttribute("href"));
      }
      icerik.classList.add("rp-icerik-cikis");
      setTimeout(function () {
        window.location.href = link.href;
      }, 150);
    });
  });
})();
