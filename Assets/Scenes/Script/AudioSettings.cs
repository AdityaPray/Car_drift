using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class AudioSettings : MonoBehaviour
{
    [Header("Audio Mixer & Sliders")]
    public AudioMixer myMixer;
    public Slider volumeSlider; 
    public Slider musicSlider;  

    void Start()
    {
        // 1. Membaca memori saat panel setting dibuka (Default 1 jika belum pernah diatur)
        float savedVolume = PlayerPrefs.GetFloat("SavedMasterVolume", 1f);
        float savedMusic = PlayerPrefs.GetFloat("SavedMusicVolume", 1f);

        // 2. Mengatur posisi tuas slider agar sesuai dengan memori
        if (volumeSlider != null) volumeSlider.value = savedVolume;
        if (musicSlider != null) musicSlider.value = savedMusic;

        // 3. Menerapkan volume ke Audio Mixer
        myMixer.SetFloat("MasterVolume", Mathf.Log10(savedVolume) * 20);
        myMixer.SetFloat("MusicVolume", Mathf.Log10(savedMusic) * 20);
    }

    public void SetVolume(float sliderValue)
    {
        // Mengubah suara di Mixer
        myMixer.SetFloat("MasterVolume", Mathf.Log10(sliderValue) * 20);
        
        // Menyimpan nilai baru ke memori HP/PC
        PlayerPrefs.SetFloat("SavedMasterVolume", sliderValue);
        PlayerPrefs.Save();
    }

    public void SetMusicVolume(float sliderValue)
    {
        // Mengubah suara musik di Mixer
        myMixer.SetFloat("MusicVolume", Mathf.Log10(sliderValue) * 20);
        
        // Menyimpan nilai baru ke memori HP/PC
        PlayerPrefs.SetFloat("SavedMusicVolume", sliderValue);
        PlayerPrefs.Save();
    }
}