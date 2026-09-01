// ana sayfadaki sınıf ortalama net ve haftalık ödev tamamlama grafiklerini çiziyoruz
(function () {
  var g = window.MentolizGrafik;

  var anaRenk = g.temaRengiOku("--rp-ana-renk", "#1e3a5f");
  var basariRenk = g.temaRengiOku("--rp-basari", "#2e7d32");
  var kenarlikRenk = g.temaRengiOku("--rp-kenarlik", "#e0e0e0");
  var yuzeyRenk = g.temaRengiOku("--rp-yuzey", "#ffffff");

  var sinifOrtalamalari = g.veriOku("sinif-ortalama-net-verisi");
  var sinifOrtalamaCanvas = document.getElementById("sinif-ortalama-net-grafigi");
  if (sinifOrtalamalari && sinifOrtalamaCanvas) {
    g.barGrafigiCiz(
      sinifOrtalamaCanvas,
      sinifOrtalamalari.map(function (s) { return s.etiket; }),
      sinifOrtalamalari.map(function (s) { return s.net; }),
      anaRenk,
      kenarlikRenk
    );
  }

  var tamamlamaOraniCanvas = document.getElementById("odev-tamamlama-grafigi");
  var tamamlamaOraniVerisi = g.veriOku("odev-tamamlama-orani-verisi");
  if (tamamlamaOraniCanvas && tamamlamaOraniVerisi !== null) {
    new Chart(tamamlamaOraniCanvas, {
      type: "doughnut",
      data: {
        labels: ["Tamamlanan", "Kalan"],
        datasets: [{
          data: [tamamlamaOraniVerisi, 100 - tamamlamaOraniVerisi],
          backgroundColor: [basariRenk, kenarlikRenk],
          borderColor: yuzeyRenk,
          borderWidth: 2
        }]
      },
      options: {
        maintainAspectRatio: false,
        // ortasındaki yüzde yazısını zaten html tarafında bindirdiğimiz için grafiğin kendi lejantına gerek yok
        cutout: "74%",
        plugins: { legend: { display: false }, tooltip: { enabled: false } }
      }
    });
  }
})();
