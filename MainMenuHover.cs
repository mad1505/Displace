using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;

// Handle hover effect di menu utama
// Script ngatur efek hover di menu
public class MainMenuHover : MonoBehaviour
{
    [Header("Pengaturan Hover")]
    [Range(1f, 1.5f)] public float hoverScaleMultiplier = 1.1f;    // Skala saat hover
    [Range(0.1f, 1f)] public float hoverAnimationSpeed = 0.2f;     // Kecepatan animasi

    [Header("Tombol")]
    public Button newGameButton;                   // Tombol new game
    public Button continueButton;                  // Tombol continue
    public Button optionButton;                    // Tombol options
    public Button exitButton;                      // Tombol exit

    [Header("Suara")]
    public AudioClip hoverSound;                   // Suara hover
    public AudioClip clickSound;                   // Suara klik

    private AudioSource hoverAudioSource;          // Source suara hover

    void Start()
    {
        // Setup audio dan efek hover
        SetupHoverAudioSource();
        AddHoverEffects();
    }

    void SetupHoverAudioSource()
    {
        // Setup audio source
        hoverAudioSource = gameObject.AddComponent<AudioSource>();
        hoverAudioSource.playOnAwake = false;
        hoverAudioSource.volume = 0.5f;
    }

    void AddHoverEffects()
    {
        // Tambah efek hover ke tombol
        Vector3 defaultScale = new Vector3(2.4407f, 2.4407f, 2.4407f);
        Vector3 exitScale = new Vector3(2.2f, 2.2f, 2.2f);

        AddHoverEffect(newGameButton, defaultScale);
        AddHoverEffect(continueButton, defaultScale);
        AddHoverEffect(optionButton, defaultScale);
        AddHoverEffect(exitButton, exitScale);
    }

    void AddHoverEffect(Button button, Vector3 normalScale)
    {
        // Setup efek hover per tombol
        if (button == null) return;

        EventTrigger trigger = button.GetComponent<EventTrigger>();
        if (trigger == null)
            trigger = button.gameObject.AddComponent<EventTrigger>();

        trigger.triggers.Clear();

        AddTrigger(trigger, EventTriggerType.PointerEnter, () => OnPointerEnter(button, normalScale));
        AddTrigger(trigger, EventTriggerType.PointerExit, () => OnPointerExit(button, normalScale));
        AddTrigger(trigger, EventTriggerType.PointerDown, () => OnPointerDown(button));
        AddTrigger(trigger, EventTriggerType.PointerUp, () => OnPointerUp(button));
    }

    void AddTrigger(EventTrigger trigger, EventTriggerType type, UnityEngine.Events.UnityAction action)
    {
        // Tambah event trigger
        var entry = new EventTrigger.Entry { eventID = type };
        entry.callback.AddListener((e) => action());
        trigger.triggers.Add(entry);
    }

    void OnPointerEnter(Button button, Vector3 normalScale)
    {
        // Handle saat pointer masuk
        StopAllCoroutines();
        if (hoverSound != null)
        {
            hoverAudioSource.clip = hoverSound;
            hoverAudioSource.Play();
        }

        Outline outline = button.GetComponent<Outline>();
        if (outline != null) outline.effectColor = new Color(0.9f, 0.9f, 1f, 0.8f);

        Image img = button.GetComponent<Image>();
        if (img != null) img.color = new Color(0.8f, 0.8f, 1f, 1f);

        Vector3 scaleTo = normalScale * hoverScaleMultiplier;
        StartCoroutine(ScaleButton(button.transform, normalScale, scaleTo, hoverAnimationSpeed));
    }

    void OnPointerExit(Button button, Vector3 normalScale)
    {
        // Handle saat pointer keluar
        StopAllCoroutines();
        Outline outline = button.GetComponent<Outline>();
        if (outline != null) outline.effectColor = new Color(0.9f, 0.9f, 1f, 0f);

        Image img = button.GetComponent<Image>();
        if (img != null) img.color = Color.white;

        StartCoroutine(ScaleButton(button.transform, button.transform.localScale, normalScale, hoverAnimationSpeed));
    }

    void OnPointerDown(Button button)
    {
        // Handle saat tombol ditekan
        StopAllCoroutines();
        if (clickSound != null)
        {
            hoverAudioSource.clip = clickSound;
            hoverAudioSource.Play();
        }

        Vector3 baseScale = button.transform.localScale;
        StartCoroutine(ScaleButton(button.transform, baseScale, baseScale * 0.95f, hoverAnimationSpeed / 2));
    }

    void OnPointerUp(Button button)
    {
        // Handle saat tombol dilepas
        StopAllCoroutines();
        Vector3 baseScale = button.transform.localScale;
        StartCoroutine(ScaleButton(button.transform, baseScale, baseScale / 0.95f, hoverAnimationSpeed / 2));
    }

    IEnumerator ScaleButton(Transform buttonTransform, Vector3 start, Vector3 end, float duration)
    {
        // Animasi scale tombol
        float startTime = Time.time;
        float endTime = startTime + duration;

        while (Time.time < endTime)
        {
            float t = (Time.time - startTime) / duration;
            buttonTransform.localScale = Vector3.Lerp(start, end, Mathf.SmoothStep(0, 1, t));
            yield return null;
        }

        buttonTransform.localScale = end;
    }
}