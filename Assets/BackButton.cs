using UnityEngine;

// Handle tombol back ke menu utama

public class BackButton : MonoBehaviour
{
    public GameObject MainMenu;     // Panel menu utama yang bakal muncul
    public GameObject OptionsPanel; // Panel options yang bakal ditutup

    // Fungsi dipanggil pas tombol back diklik
    public void BackToMainMenu()
    {
        OptionsPanel.SetActive(false); // Tutup panel options
        MainMenu.SetActive(true);      // Munculin menu utama
    }
}
