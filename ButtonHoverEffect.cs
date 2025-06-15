using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;

// Handle efek hover & click di button
// Script ngatur animasi dan suara saat button di-hover/click
public class ButtonHoverEffect : MonoBehaviour
{
    // SFX untuk hover & click
    public static AudioClip hoverSound;
    public static AudioClip clickSound;

    // Setting animasi hover
    private static float hoverScaleMultiplier = 1.1f; // Scale button saat hover
    private static float hoverAnimationSpeed = 0.2f;  // Durasi animasi scale

    // Warna button normal & hover
    private static Color normalColor = Color.white;
    private static Color hoverColor = new Color(0.8f, 0.8f, 1f, 1f); // Warna kebiruan

    // Audio source untuk SFX
    private static AudioSource audioSource;

    // Setup efek hover ke button
    public static void SetupHover(Button button)
    {
        if (button == null) return;

        // Buat audio source sekali aja
        if (audioSource == null)
        {
            GameObject audioObj = new GameObject("ButtonHoverAudio");
            audioSource = audioObj.AddComponent<AudioSource>();
            Object.DontDestroyOnLoad(audioObj); // Tetap ada di semua scene
            audioSource.playOnAwake = false;
            audioSource.volume = 0.5f;
        }

        // Setup EventTrigger di button
        EventTrigger trigger = button.GetComponent<EventTrigger>();
        if (trigger == null)
            trigger = button.gameObject.AddComponent<EventTrigger>();

        trigger.triggers.Clear(); // Reset event lama

        // Tambah event hover/click
        AddTrigger(trigger, EventTriggerType.PointerEnter, () => OnPointerEnter(button));
        AddTrigger(trigger, EventTriggerType.PointerExit, () => OnPointerExit(button));
        AddTrigger(trigger, EventTriggerType.PointerDown, () => OnPointerDown(button));
        AddTrigger(trigger, EventTriggerType.PointerUp, () => OnPointerUp(button));
    }

    // Buat event trigger baru
    private static void AddTrigger(EventTrigger trigger, EventTriggerType type, UnityEngine.Events.UnityAction action)
    {
        var entry = new EventTrigger.Entry { eventID = type };
        entry.callback.AddListener((e) => action());
        trigger.triggers.Add(entry);
    }

    // Event saat pointer masuk (hover)
    private static void OnPointerEnter(Button button)
    {
        // Play SFX hover
        if (hoverSound != null)
        {
            audioSource.clip = hoverSound;
            audioSource.Play();
        }

        // Update warna button
        Image img = button.GetComponent<Image>();
        if (img != null) img.color = hoverColor;

        // Animasi scale
        button.StopAllCoroutines();
        button.StartCoroutine(ScaleButton(button.transform, button.transform.localScale, button.transform.localScale * hoverScaleMultiplier, hoverAnimationSpeed));
    }

    // Event saat pointer keluar
    private static void OnPointerExit(Button button)
    {
        // Reset warna button
        Image img = button.GetComponent<Image>();
        if (img != null) img.color = normalColor;

        // Animasi scale balik
        button.StopAllCoroutines();
        button.StartCoroutine(ScaleButton(button.transform, button.transform.localScale, Vector3.one, hoverAnimationSpeed));
    }

    // Event saat button ditekan
    private static void OnPointerDown(Button button)
    {
        // Play SFX click
        if (clickSound != null)
        {
            audioSource.clip = clickSound;
            audioSource.Play();
        }

        // Animasi scale kecil
        button.StopAllCoroutines();
        button.StartCoroutine(ScaleButton(button.transform, button.transform.localScale, Vector3.one * 0.95f, hoverAnimationSpeed / 2));
    }

    // Event saat button dilepas
    private static void OnPointerUp(Button button)
    {
        // Animasi scale balik
        button.StopAllCoroutines();
        button.StartCoroutine(ScaleButton(button.transform, button.transform.localScale, Vector3.one, hoverAnimationSpeed / 2));
    }

    // Animasi scale button
    private static IEnumerator ScaleButton(Transform buttonTransform, Vector3 start, Vector3 end, float duration)
    {
        float time = 0;
        while (time < duration)
        {
            buttonTransform.localScale = Vector3.Lerp(start, end, time / duration);
            time += Time.unscaledDeltaTime;
            yield return null;
        }
        buttonTransform.localScale = end;
    }
}
