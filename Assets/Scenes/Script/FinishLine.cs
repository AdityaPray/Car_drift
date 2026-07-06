using UnityEngine;

public class FinishLine : MonoBehaviour
{
    public ModeManager modeManager;
    private bool sudahDisentuh = false;

    void OnTriggerEnter(Collider other)
    {
        // KODE BARU: Akan mencetak nama dan tag objek apa pun yang menabrak garis finish ke tab Console
        Debug.Log("Garis Finish disentuh oleh: " + other.gameObject.name + " | Tag-nya adalah: " + other.tag);

        if (sudahDisentuh) return; 

        if (other.CompareTag("Player"))
        {
            modeManager.PlayerMenyentuhFinish();
            sudahDisentuh = true;
        }
        else if (other.CompareTag("NPC")) 
        {
            modeManager.NpcMenyentuhFinish();
            sudahDisentuh = true;
        }
    }
}