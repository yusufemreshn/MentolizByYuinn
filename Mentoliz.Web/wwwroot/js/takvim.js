// bir günde dörtten fazla etkinlik varsa "+N daha" düğmesi tıklanınca gizli kalanları aynı hücrede açıyor
document.addEventListener("click", function (olay) {
  var dugme = olay.target.closest(".rp-takvim-fazla");
  if (!dugme) {
    return;
  }

  var liste = dugme.closest(".rp-takvim-oge-listesi");
  liste.querySelectorAll(".rp-takvim-oge-gizli").forEach(function (oge) {
    oge.classList.remove("d-none");
  });

  dugme.remove();
});
