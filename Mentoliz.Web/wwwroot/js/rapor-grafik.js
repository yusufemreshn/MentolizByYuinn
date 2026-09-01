// öğretmen ve veli raporlarındaki net gelişim grafiğini çiziyoruz
(function () {
  var veriEleman = document.getElementById("net-gelisimi-verisi");
  var canvas = document.getElementById("net-gelisimi-grafigi");
  if (!veriEleman || !canvas) {
    return;
  }

  var netGelisimi = JSON.parse(veriEleman.textContent);

  new Chart(canvas, {
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
      animation: false,
      // fareyi noktanın tam üstüne götürmeye gerek kalmasın diye o x konumundaki değeri dikey hizada gösteriyoruz
      interaction: { mode: "index", intersect: false },
      plugins: { legend: { display: false } },
      scales: { y: { beginAtZero: true } }
    }
  });
})();
