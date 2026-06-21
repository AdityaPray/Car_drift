using UnityEngine;

public class BGMPlayer : MonoBehaviour
{
    private static BGMPlayer instance;

    void Awake()
    {
        // Sistem ini mencegah lagunya terputus, dan mencegah lagu 
        // bertumpuk menjadi dua saat pemain kembali ke Main Menu.
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}