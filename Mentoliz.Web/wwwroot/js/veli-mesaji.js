// veli iletişim metnini panoya kopyalayan düğme, sadece bu buton varsa çalışıyor
(function () {
  var dugme = document.getElementById("veli-mesaji-kopyala");
  var metinKutusu = document.getElementById("veli-mesaji-metni");
  if (!dugme || !metinKutusu) {
    return;
  }

  dugme.addEventListener("click", function () {
    navigator.clipboard.writeText(metinKutusu.value).then(function () {
      var ikonHaric = dugme.getAttribute("data-basarili-metin");
      var eskiIcerik = dugme.innerHTML;
      dugme.innerHTML = '<i class="bi bi-check2"></i> ' + ikonHaric;

      window.setTimeout(function () {
        dugme.innerHTML = eskiIcerik;
      }, 1800);
    });
  });
})();
