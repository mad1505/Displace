using UnityEngine;
using System.Collections;

// Handle player blink effect
// Script ngatur efek kedip player
public class PlayerBlink : MonoBehaviour
{
    public SpriteRenderer spriteRenderer;           // Renderer sprite player
    public float blinkDuration = 0.1f;              // Durasi kedip
    public int blinkCount = 5;                      // Jumlah kedip

    public void TakeDamage()
    {
        // Start blink effect
        StartCoroutine(BlinkCoroutine());
    }

    private IEnumerator BlinkCoroutine()
    {
        // Blink loop
        for (int i = 0; i < blinkCount; i++)
        {
            // Fade out
            SetSpriteAlpha(0f);
            yield return new WaitForSeconds(blinkDuration);

            // Fade in
            SetSpriteAlpha(1f);
            yield return new WaitForSeconds(blinkDuration);
        }
    }

    private void SetSpriteAlpha(float alpha)
    {
        // Set sprite alpha
        if (spriteRenderer != null)
        {
            Color color = spriteRenderer.color;
            color.a = alpha;
            spriteRenderer.color = color;
        }
    }
}