using UnityEngine;
using UnityEngine.SceneManagement; // Wajib ditambahkan untuk memanggil Scene

public class MainMenu : MonoBehaviour
{
    // Fungsi ini akan dipanggil saat tombol Start ditekan
    public void PlayGame()
    {
        // Ganti "SampleScene" dengan nama scene balapanmu yang sebenarnya!
        SceneManager.LoadScene("Main Scene"); 
    }

    // Fungsi ini untuk tombol Quit
    public void QuitGame()
    {
        Debug.Log("Game Keluar!"); // Hanya terlihat di editor
        Application.Quit(); // Berfungsi saat game sudah di-build ke HP/PC
    }
}