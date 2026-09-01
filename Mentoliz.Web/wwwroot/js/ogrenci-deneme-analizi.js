// öğrenci detayındaki deneme sonuçları sekmesinde net gelişimi ve ders dağılımı grafiklerini çiziyoruz
(function () {
  function veriOku(elemanId) {
    var eleman = document.getElementById(elemanId);
    if (!eleman) {
      return null;
    }
    return JSON.parse(eleman.textContent);
  }

  var netGelisimi = veriOku("net-gelisimi-verisi");
  var netGelisimiCanvas = document.getElementById("net-gelisimi-grafigi");
  if (netGelisimi && netGelisimiCanvas) {
    new Chart(netGelisimiCanvas, {
      type: "line",
      data: {
        labels: netGelisimi.map(function (n) { return n.etiket; }),
        datasets: [{
          label: "Toplam Net",
          data: netGelisimi.map(function (n) { return n.net; }),
          borderColor: "#1e3a5f",
          backgroundColor: "rgba(30, 58, 95, 0.15)",
          tension: 0.25,
          fill: true,
          pointHoverRadius: 5
        }]
      },
      options: {
        // fareyi noktanın tam üstüne götürmeye gerek kalmasın diye o x konumundaki değeri dikey hizada gösteriyoruz
        interaction: { mode: "index", intersect: false },
        plugins: { legend: { display: false } },
        scales: { y: { beginAtZero: true } }
      }
    });
  }

  var dersNetleri = veriOku("ders-netleri-verisi");
  var dersNetleriCanvas = document.getElementById("ders-netleri-grafigi");
  if (dersNetleri && dersNetleriCanvas) {
    // radar grafiğinde çok sayıda ders olunca etiketler üst üste biniyor ve hiç okunmuyordu, yatay çubuk grafiğe çevirdik
    new Chart(dersNetleriCanvas, {
      type: "bar",
      data: {
        labels: dersNetleri.map(function (d) { return d.etiket; }),
        datasets: [{
          label: "Net",
          data: dersNetleri.map(function (d) { return d.net; }),
          backgroundColor: "#1e3a5f",
          borderRadius: 4,
          maxBarThickness: 28
        }]
      },
      options: {
        indexAxis: "y",
        maintainAspectRatio: false,
        interaction: { mode: "index", intersect: false },
        plugins: { legend: { display: false } },
        scales: { x: { beginAtZero: true } }
      }
    });
  }
})();
