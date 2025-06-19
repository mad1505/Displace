using UnityEngine;

// Handle mekanik stealth di semak
// Script ngatur transparansi player saat di semak
public class BushStealth : MonoBehaviour
{
    private bool isHidden = false; // Status player lagi ngumpet atau nggak

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Cek kalo player masuk semak
        if (collision.CompareTag("Player"))
        {
            isHidden = true; // Set status ngumpet

            // Bikin player transparan
            collision.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 0.5f);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        // Cek kalo player keluar semak
        if (collision.CompareTag("Player"))
        {
            isHidden = false; // Reset status ngumpet

            // Balikin warna player ke normal
            collision.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 1f);
        }
    }

    // Get status ngumpet player
    public bool IsHidden()
    {
        return isHidden;
    }
}
