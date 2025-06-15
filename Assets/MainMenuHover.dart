using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;

public class MainMenuHover : MonoBehaviour
{
    [Header("Hover Settings")]
    [Range(1f, 1.5f)] public float hoverScaleMultiplier = 1.1f;
    [Range(0.1f, 1f)] public float hoverAnimationSpeed = 0.2f;

    [Header("Buttons")]
    public Button newGameButton;
    public Button continueButton;
    public Button optionButton;
    public Button exitButton;

    [Header("Sounds")]
    public AudioClip hoverSound;
    public AudioClip clickSound;

    private AudioSource hoverAudioSource;

    void Start()
    {
        SetupHoverAudioSource();
        AddHoverEffects();
    }

    void SetupHoverAudioSource()
    {
        hoverAudioSource = gameObject.AddComponent<AudioSource>();
        hoverAudioSource.playOnAwake = false;
        hoverAudioSource.volume = 0.5f;
    }

    void AddHoverEffects()
    {
        AddHoverEffect(newGameButton, new Vector3(2.4407f, 2.4407f, 2.4407f));
        AddHoverEffect(continueButton, new Vector3(2.4407f, 2.4407f, 2.4407f));
        AddHoverEffect(optionButton, new Vector3(2.4407f, 2.4407f, 2.4407f));
        AddHoverEffect(exitButton, new Vector3(1.815075f, 1.815075f, 1.815075f));
    }

    void AddHoverEffect(Button button, Vector3 normalScale)
    {
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
        var entry = new EventTrigger.Entry { eventID = type };
        entry.callback.AddListener((e) => action());
        trigger.triggers.Add(entry);
    }

    void OnPointerEnter(Button button, Vector3 normalScale)
    {
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
        StopAllCoroutines();
        Outline outline = button.GetComponent<Outline>();
        if (outline != null) outline.effectColor = new Color(0.9f, 0.9f, 1f, 0f);

        Image img = button.GetComponent<Image>();
        if (img != null) img.color = Color.white;

        StartCoroutine(ScaleButton(button.transform, button.transform.localScale, normalScale, hoverAnimationSpeed));
    }

    void OnPointerDown(Button button)
    {
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
        StopAllCoroutines();
        Vector3 baseScale = button.transform.localScale;
        StartCoroutine(ScaleButton(button.transform, baseScale, baseScale / 0.95f, hoverAnimationSpeed / 2));
    }

    IEnumerator ScaleButton(Transform buttonTransform, Vector3 start, Vector3 end, float duration)
    {
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
