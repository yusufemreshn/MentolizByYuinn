// öğrenci formundaki veli satırlarını ekleme, silme ve yeniden numaralandırma burada
(function () {
  var satirlarAlani = document.getElementById("veli-satirlari-alani");
  var ekleButonu = document.getElementById("veli-ekle-butonu");
  var sablon = document.getElementById("veli-satiri-sablonu");

  if (!satirlarAlani || !ekleButonu || !sablon) {
    return;
  }

  // formdaki her input'un name'inde bulunan index numarasını satırın dom sırasına göre yeniden yazıyor
  function satirlariYenidenNumarala() {
    var satirlar = satirlarAlani.querySelectorAll(".veli-satiri");
    satirlar.forEach(function (satir, sira) {
      satir.querySelectorAll("[name]").forEach(function (alan) {
        alan.name = alan.name.replace(/Veliler\[\d+\]/, "Veliler[" + sira + "]");
      });
    });
  }

  ekleButonu.addEventListener("click", function () {
    var yeniSatir = sablon.content.cloneNode(true);
    // şablondaki __index__ yerine geçici bir değer yazıyoruz, ekleme bitince zaten yeniden numaralanacak
    yeniSatir.querySelectorAll("[name]").forEach(function (alan) {
      alan.name = alan.name.replace("__index__", satirlarAlani.children.length);
    });
    satirlarAlani.appendChild(yeniSatir);
    satirlariYenidenNumarala();
  });

  satirlarAlani.addEventListener("click", function (olay) {
    var silButonu = olay.target.closest(".veli-sil-butonu");
    if (!silButonu) {
      return;
    }
    silButonu.closest(".veli-satiri").remove();
    satirlariYenidenNumarala();
  });
})();
