using System.Collections; 
using UnityEngine;
using TMPro;

public class ModeManager : MonoBehaviour
{
    [Header("Pengaturan Objek")]
    public GameObject[] npcCars;    
    public GameObject timerUI;      
    public TextMeshProUGUI timerText; 

    [Header("Pengaturan Hitung Mundur")]
    public TextMeshProUGUI countdownText; 

    [Header("Pengaturan Garis Finish (UI)")]
    public GameObject panelWin;         // Panel saat menang VS NPC
    public GameObject panelLose;        // Panel saat kalah VS NPC
    public GameObject panelTimeRecord;  // Panel hasil waktu (Time Attack)
    public TextMeshProUGUI teksWaktuAkhir; // Teks untuk menampilkan hasil waktu di panel Time Attack

    private int modePilihan;
    private float waktuBerjalan = 0f;
    private bool hitungWaktu = false;
    
    // Variabel penanda finish
    private bool balapanSelesai = false;
    private bool npcFinishDuluan = false;

    void Start()
    {
        modePilihan = PlayerPrefs.GetInt("GameMode", 0);

        // 1. Munculkan/Sembunyikan NPC dan Timer sesuai mode
        if (modePilihan == 0) // VS NPC
        {
            foreach (GameObject npc in npcCars)
            {
                if (npc != null) npc.SetActive(true);
            }
            if (timerUI != null) timerUI.SetActive(false); 
        }
        else if (modePilihan == 1) // Time Attack
        {
            foreach (GameObject npc in npcCars)
            {
                if (npc != null) npc.SetActive(false);
            }
            if (timerUI != null) timerUI.SetActive(true); 
        }

        // 2. Mulai proses hitung mundur
        StartCoroutine(MulaiHitungMundur());
    }

    IEnumerator MulaiHitungMundur()
    {
        Time.timeScale = 0f; 

        if (countdownText != null)
        {
            countdownText.gameObject.SetActive(true);
            
            countdownText.text = "3";
            yield return new WaitForSecondsRealtime(1f); 
            
            countdownText.text = "2";
            yield return new WaitForSecondsRealtime(1f);
            
            countdownText.text = "1";
            yield return new WaitForSecondsRealtime(1f);
            
            countdownText.text = "GO!";
            
            Time.timeScale = 1f; 

            if (modePilihan == 1)
            {
                hitungWaktu = true; 
            }

            yield return new WaitForSeconds(1f); 
            countdownText.gameObject.SetActive(false);
        }
        else
        {
            yield return new WaitForSecondsRealtime(3f);
            Time.timeScale = 1f;
            if (modePilihan == 1) hitungWaktu = true;
        }
    }

    void Update()
    {
        // Tambahkan pengecekan !balapanSelesai agar waktu berhenti saat sudah sampai finish
        if (modePilihan == 1 && hitungWaktu == true && !balapanSelesai)
        {
            waktuBerjalan += Time.deltaTime; 
            UpdateUIWaktu();
        }
    }

    void UpdateUIWaktu()
    {
        if (timerText == null) return;

        int menit = Mathf.FloorToInt(waktuBerjalan / 60F);
        int detik = Mathf.FloorToInt(waktuBerjalan - menit * 60);
        int milidetik = Mathf.FloorToInt((waktuBerjalan - Mathf.Floor(waktuBerjalan)) * 100);

        timerText.text = string.Format("{0:00}:{1:00}.{2:00}", menit, detik, milidetik);
    }

    // ==============================================================
    // FUNGSI FINISH (Dipanggil oleh script FinishLine di Garis Finish)
    // ==============================================================
    
    public void PlayerMenyentuhFinish()
    {
        if (balapanSelesai) return; // Mencegah terpanggil dua kali
        
        balapanSelesai = true;
        Time.timeScale = 0f; // Bekukan game saat finish

        if (modePilihan == 0) // Mode VS NPC
        {
            if (npcFinishDuluan)
            {
                // Player finish tapi NPC sudah sampai duluan
                if (panelLose != null) panelLose.SetActive(true);
            }
            else
            {
                // Player finish dan NPC belum sampai
                if (panelWin != null) panelWin.SetActive(true);
            }
        }
        else if (modePilihan == 1) // Mode Time Attack
        {
            if (panelTimeRecord != null) panelTimeRecord.SetActive(true);
            
            // Pindahkan angka timer UI ke teks hasil
            if (teksWaktuAkhir != null) 
            {
                teksWaktuAkhir.text = "Waktu Tempuh: " + timerText.text; 
            }
        }
    }

    public void NpcMenyentuhFinish()
    {
        if (modePilihan == 0 && !balapanSelesai)
        {
            npcFinishDuluan = true;
            balapanSelesai = true;
            Time.timeScale = 0f; // Hentikan game langsung saat NPC menang
            
            // Munculkan panel kalah karena NPC sampai duluan
            if (panelLose != null) panelLose.SetActive(true);
        }
    }
}