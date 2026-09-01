using Mentoliz.Business.Dto.Deneme;

namespace Mentoliz.Business.Abstract;

// sınav türü ve test yapıları adım 3'te tohumlanan referans veri, buradan sadece okunuyor
public interface ISinavTuruServisi
{
    Task<List<SinavTuruDTO>> ListeleAsync();

    Task<SinavTuruDTO?> TekGetirAsync(int id);
}
