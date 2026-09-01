// birden fazla sayfada kullanılan chart.js kurulum yardımcıları burada duruyor, aynı grafik kodu her sayfada yeniden yazılmasın diye
window.MentolizGrafik = (function () {
  function veriOku(elemanId) {
    var eleman = document.getElementById(elemanId);
    if (!eleman) {
      return null;
    }
    return JSON.parse(eleman.textContent);
  }

  // grafik renkleri sabit yazılmasın diye aktif temanın css değişkenlerinden okunuyor, tema değişince grafik de uyum sağlasın diye
  function temaRengiOku(degiskenAdi, yedekRenk) {
    var deger = getComputedStyle(document.documentElement).getPropertyValue(degiskenAdi).trim();
    return deger || yedekRenk;
  }

  // chart.js kendi varsayılan gri rengini ve sistem fontunu kullanıyordu, gece modunda eksen yazıları hiç okunmuyordu, burada bir kere ayarlayınca bütün grafikler kapsanıyor
  if (window.Chart) {
    Chart.defaults.font.family = "Inter, system-ui, -apple-system, 'Segoe UI', sans-serif";
    Chart.defaults.font.size = 12;
    Chart.defaults.color = temaRengiOku("--rp-ikincil-metin", "#5b6b82");
    Chart.defaults.borderColor = temaRengiOku("--rp-kenarlik", "#e3e8ef");
  }

  // sınıf ortalama net gibi tek serili bar grafiklerin hepsi aynı ayarlarla çiziliyor
  function barGrafigiCiz(canvas, etiketler, degerler, renk, kenarlikRenk) {
    if (!canvas) {
      return null;
    }

    return new Chart(canvas, {
      type: "bar",
      data: {
        labels: etiketler,
        datasets: [{
          label: "Ortalama Net",
          data: degerler,
          backgroundColor: renk,
          borderRadius: 6,
          maxBarThickness: 48
        }]
      },
      options: {
        // sabit yükseklikli kutu içinde çalıştığı için oranı kutuya değil kutunun kendisine bırakıyoruz, yoksa geniş sütunlarda grafik orantısızca büyüyordu
        maintainAspectRatio: false,
        interaction: { mode: "index", intersect: false },
        plugins: { legend: { display: false } },
        scales: {
          y: { beginAtZero: true, grid: { color: kenarlikRenk } },
          x: { grid: { display: false } }
        }
      }
    });
  }

  return {
    veriOku: veriOku,
    temaRengiOku: temaRengiOku,
    barGrafigiCiz: barGrafigiCiz
  };
})();
