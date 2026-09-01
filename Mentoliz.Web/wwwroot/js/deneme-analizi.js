// deneme analizi sayfasındaki sekiz sınıf ortalama net grafiğini çiziyoruz, sayısı sabit yazılmasın diye canvas'lar data-veri-id ile taranıyor
(function () {
  var g = window.MentolizGrafik;
  var anaRenk = g.temaRengiOku("--rp-ana-renk", "#1e3a5f");
  var kenarlikRenk = g.temaRengiOku("--rp-kenarlik", "#e0e0e0");

  document.querySelectorAll("canvas[data-veri-id]").forEach(function (canvas) {
    var veri = g.veriOku(canvas.getAttribute("data-veri-id"));
    if (!veri) {
      return;
    }

    g.barGrafigiCiz(
      canvas,
      veri.map(function (s) { return s.etiket; }),
      veri.map(function (s) { return s.net; }),
      anaRenk,
      kenarlikRenk
    );
  });
})();
