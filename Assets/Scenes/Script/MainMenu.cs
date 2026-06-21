using UnityEngine;
using UnityEngine.SceneManagement; // Wajib ditambahkan untuk memanggil Scene

public class MainMenu : MonoBehaviour
{
    public GameObject Option_panel; 
    public AudioSource musicSource;
    // Fungsi ini akan dipanggil saat tombol Start ditekan
    void Start()
    {
        if (musicSource != null)
        {
            musicSource.volume = 1.0f; // Kembalikan volume ke 100% saat di Menu
        }
    }

    public void PlayGame()
    {
        if (musicSource != null)
        {
            musicSource.volume = 0.3f; // Mengecilkan volume jadi 30% saat main
        }
        SceneManager.LoadScene("Main Scene"); 
    }

    // Fungsi ini untuk tombol Quit
    public void QuitGame()
    {
        Debug.Log("Game Keluar!"); // Hanya terlihat di editor
        Application.Quit(); // Berfungsi saat game sudah di-build ke HP/PC
    }
    // Fungsi untuk membuka Option
    public void OpenOption()
    {
        Option_panel.SetActive(true); 
    }

    // Fungsi untuk menutup Option
    public void CloseOption()
    {
        Option_panel.SetActive(false); 
    }

    // Fungsi BARU untuk mematikan/menyalakan musik
    public void ToggleMusic()
    {
        // Mengecek apakah sumber suara sudah dimasukkan
        if (musicSource != null)
        {
            // Jika sedang nyala, maka matikan (mute). Jika mati, maka nyalakan.
            musicSource.mute = !musicSource.mute; 
        }
    }
}