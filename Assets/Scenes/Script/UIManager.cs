using UnityEngine;
using UnityEngine.SceneManagement; // Wajib ditambahkan untuk fungsi Restart/Pindah Scene

public class UIManager : MonoBehaviour
{
    [Header("UI Panels")]
    public GameObject panelPause;
    public GameObject panelSettings;

    // 1. Fungsi untuk tombol Pause (Garis tiga di pojok)
    public void PauseGame()
    {
        panelPause.SetActive(true);
        panelSettings.SetActive(false); // Pastikan setting tertutup
        Time.timeScale = 0f;            // Hentikan waktu/gameplay
    }

    // 2. Fungsi untuk tombol Resume
    public void ResumeGame()
    {
        panelPause.SetActive(false);
        Time.timeScale = 1f;            // Lanjutkan waktu/gameplay
    }

    // 3. Fungsi untuk tombol Settings (Buka Setting dari Pause)
    public void OpenSettings()
    {
        panelPause.SetActive(false);
        panelSettings.SetActive(true);
    }

    // 4. Fungsi untuk tombol Back (Tutup Setting, kembali ke Pause)
    public void CloseSettings()
    {
        panelSettings.SetActive(false);
        panelPause.SetActive(true);
    }

    // 5. Fungsi untuk tombol Restart
    public void RestartGame()
    {
        Time.timeScale = 1f; // Wajib dinormalkan sebelum pindah scene
        // Memuat ulang scene yang sedang aktif saat ini
        SceneManager.LoadScene(SceneManager.GetActiveScene().name); 
    }

    // 6. Fungsi untuk tombol Exit To Main Menu
    public void ExitToMenu()
    {
        Time.timeScale = 1f; // Wajib dinormalkan
        // Ganti "MainMenu" dengan nama scene menu utama kamu
        SceneManager.LoadScene("Main Menu"); 
    }
}