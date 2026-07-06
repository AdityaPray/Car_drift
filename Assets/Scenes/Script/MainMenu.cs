using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public GameObject Option_panel; 
    public AudioSource musicSource;

    void Start()
    {
        if (musicSource != null)
        {
            musicSource.volume = 1.0f; // Kembalikan volume ke 100% saat di Menu
        }
    }

    // Fungsi ini untuk tombol "VS NPC" di dalam Panel Mode
    public void PlayVersusMode()
    {
        // Menyimpan pilihan mode (0 = Versus NPC)
        PlayerPrefs.SetInt("GameMode", 0);
        PlayerPrefs.Save();
        
        // Panggil fungsi pindah scene
        MuatSceneBalapan();
    }

    // Fungsi ini untuk tombol "Time Attack" di dalam Panel Mode
    public void PlayTimeAttackMode()
    {
        // Menyimpan pilihan mode (1 = Time Attack)
        PlayerPrefs.SetInt("GameMode", 1);
        PlayerPrefs.Save();
        
        // Panggil fungsi pindah scene
        MuatSceneBalapan();
    }

    // Fungsi internal untuk mengecilkan suara dan pindah scene
    private void MuatSceneBalapan()
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
        Debug.Log("Game Keluar!"); 
        Application.Quit(); 
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

    // Fungsi untuk mematikan/menyalakan musik
    public void ToggleMusic()
    {
        if (musicSource != null)
        {
            musicSource.mute = !musicSource.mute; 
        }
    }
}