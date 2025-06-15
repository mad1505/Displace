using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using System.Collections;

// Handle menu options
// Script ngatur menu pengaturan
public class OptionsMenu : MonoBehaviour
{
    [Header("Pengaturan Menu Opsi")]
    public Button backButton;                      // Tombol back
    public Button creditButton;                    // Tombol credit
    public Slider masterVolumeSlider;              // Slider volume master
    public Slider musicVolumeSlider;               // Slider volume musik
    public Slider sfxVolumeSlider;                 // Slider volume SFX
    public AudioMixer audioMixer;                  // Mixer audio
    public Sprite normalSprite;                    // Sprite normal
    public Sprite hoverSprite;                     // Sprite hover

    [Header("Efek Hover")]
    [Range(1f, 1.5f)] public float hoverScaleMultiplier = 1.1f;    // Skala hover
    [Range(0.1f, 1f)] public float hoverAnimationSpeed = 0.2f;     // Kecepatan hover
    public Color normalColor = Color.white;                         // Warna normal
    public Color hoverColor = new Color(0.8f, 0.8f, 1f, 1f);       // Warna hover

    [Header("Suara Tombol")]
    public AudioClip hoverSound;                   // Suara hover
    public AudioClip clickSound;                   // Suara klik

    [Header("Panel Menu")]
    public GameObject mainMenuPanel;               // Panel menu utama
    public GameObject optionPanel;                 // Panel options

    private AudioSource clickAudioSource;          // Source suara klik
    private AudioSource hoverAudioSource;          // Source suara hover
    private bool settingsChanged = false;          // Flag perubahan setting

    private float initialMasterVolume;             // Volume master awal
    private float initialMusicVolume;              // Volume musik awal
    private float initialSFXVolume;                // Volume SFX awal

    private GameManager gameManager;               // Referensi game manager
    private Vector3 defaultButtonScale = new Vector3(2.4407f, 2.4407f, 2.4407f);  // Skala default

    void Start()
    {
        // Setup game manager
        gameManager = GameManager.instance;
        if (gameManager == null)
        {
            GameObject gmObject = new GameObject("GameManager");
            gameManager = gmObject.AddComponent<GameManager>();
        }

        // Setup komponen
        SetupAudioSources();
        SetupButtonVisuals();
        SetupButtonListeners();
        AddHoverEffects();
        SetupOptionsMenu();
    }

    void SetupAudioSources()
    {
        // Setup audio source
        clickAudioSource = gameObject.AddComponent<AudioSource>();
        clickAudioSource.playOnAwake = false;
        clickAudioSource.volume = 0.5f;

        hoverAudioSource = gameObject.AddComponent<AudioSource>();
        hoverAudioSource.playOnAwake = false;
        hoverAudioSource.volume = 0.5f;
    }

    void SetupButtonVisuals()
    {
        // Setup tampilan tombol
        SetupButton(backButton, normalSprite);
        SetupButton(creditButton, normalSprite);
    }

    void SetupButton(Button button, Sprite sprite)
    {
        // Setup tombol individual
        if (button != null && sprite != null)
        {
            Image img = button.GetComponent<Image>();
            if (img != null)
            {
                img.sprite = sprite;
                img.color = normalColor;
            }

            Outline outline = button.GetComponent<Outline>();
            if (outline == null)
            {
                outline = button.gameObject.AddComponent<Outline>();
                outline.effectColor = new Color(0.9f, 0.9f, 1f, 0f);
                outline.effectDistance = new Vector2(3, 3);
            }

            button.transform.localScale = defaultButtonScale;

            Text txt = button.GetComponentInChildren<Text>();
            if (txt != null)
                txt.text = "";
        }
    }

    void SetupButtonListeners()
    {
        // Setup listener tombol
        if (backButton != null)
        {
            backButton.onClick.RemoveAllListeners();
            backButton.onClick.AddListener(SaveAndBackToMainMenu);
        }

        if (creditButton != null)
        {
            creditButton.onClick.RemoveAllListeners();
            creditButton.onClick.AddListener(GoToCreditScene);
        }
    }

    void SetupOptionsMenu()
    {
        // Setup menu options
        if (masterVolumeSlider != null)
        {
            initialMasterVolume = PlayerPrefs.GetFloat("MasterVolume", 0.75f);
            masterVolumeSlider.value = initialMasterVolume;
            masterVolumeSlider.onValueChanged.AddListener(OnMasterVolumeChanged);
        }

        if (musicVolumeSlider != null)
        {
            initialMusicVolume = PlayerPrefs.GetFloat("MusicVolume", 0.75f);
            musicVolumeSlider.value = initialMusicVolume;
            musicVolumeSlider.onValueChanged.AddListener(OnMusicVolumeChanged);
        }

        if (sfxVolumeSlider != null)
        {
            initialSFXVolume = PlayerPrefs.GetFloat("SFXVolume", 0.75f);
            sfxVolumeSlider.value = initialSFXVolume;
            sfxVolumeSlider.onValueChanged.AddListener(OnSFXVolumeChanged);
            UpdateButtonSoundsVolume(initialSFXVolume);
        }
    }

    public void SaveAndBackToMainMenu()
    {
        // Simpan setting dan kembali
        if (settingsChanged)
        {
            if (masterVolumeSlider != null)
                PlayerPrefs.SetFloat("MasterVolume", masterVolumeSlider.value);

            if (musicVolumeSlider != null)
                PlayerPrefs.SetFloat("MusicVolume", musicVolumeSlider.value);

            if (sfxVolumeSlider != null)
                PlayerPrefs.SetFloat("SFXVolume", sfxVolumeSlider.value);

            PlayerPrefs.Save();
            settingsChanged = false;
        }

        if (mainMenuPanel != null)
            mainMenuPanel.SetActive(true);
        if (optionPanel != null)
            optionPanel.SetActive(false);
    }

    public void OnMasterVolumeChanged(float volume)
    {
        // Update volume master
        if (audioMixer != null)
            audioMixer.SetFloat("MasterVolume", Mathf.Log10(volume) * 20);

        if (volume != initialMasterVolume)
            settingsChanged = true;
    }

    public void OnMusicVolumeChanged(float volume)
    {
        // Update volume musik
        if (audioMixer != null)
            audioMixer.SetFloat("MusicVolume", Mathf.Log10(volume) * 20);

        if (volume != initialMusicVolume)
            settingsChanged = true;
    }

    public void OnSFXVolumeChanged(float volume)
    {
        // Update volume SFX
        if (audioMixer != null)
            audioMixer.SetFloat("SFXVolume", Mathf.Log10(volume) * 20);

        UpdateButtonSoundsVolume(volume);

        if (volume != initialSFXVolume)
            settingsChanged = true;
    }

    void UpdateButtonSoundsVolume(float volume)
    {
        // Update volume suara tombol
        if (hoverAudioSource != null)
            hoverAudioSource.volume = volume;
        if (clickAudioSource != null)
            clickAudioSource.volume = volume;
    }

    public void GoToCreditScene()
    {
        // Pindah ke scene credit
        SceneManager.LoadScene("Credit");
    }

    void AddHoverEffects()
    {
        // Tambah efek hover
        AddHoverEffect(backButton);
        AddHoverEffect(creditButton);
    }

    void AddHoverEffect(Button button)
    {
        // Setup efek hover per tombol
        if (button == null) return;

        EventTrigger trigger = button.GetComponent<EventTrigger>();
        if (trigger == null)
            trigger = button.gameObject.AddComponent<EventTrigger>();

        trigger.triggers.Clear();

        AddTrigger(trigger, EventTriggerType.PointerEnter, () => OnPointerEnter(button));
        AddTrigger(trigger, EventTriggerType.PointerExit, () => OnPointerExit(button));
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

    void OnPointerEnter(Button button)
    {
        // Handle saat pointer masuk
        if (button == creditButton)
        {
            Outline outline = button.GetComponent<Outline>();
            if (outline != null)
                outline.effectColor = new Color(0.9f, 0.9f, 1f, 0.8f);

            Image img = button.GetComponent<Image>();
            if (img != null)
                img.color = hoverColor;

            return;
        }

        StopAllCoroutines();
        if (hoverSound != null)
        {
            hoverAudioSource.clip = hoverSound;
            hoverAudioSource.Play();
        }

        Outline o = button.GetComponent<Outline>();
        if (o != null)
            o.effectColor = new Color(0.9f, 0.9f, 1f, 0.8f);

        Image i = button.GetComponent<Image>();
        if (i != null)
            i.color = hoverColor;

        Vector3 scaleTo = defaultButtonScale * hoverScaleMultiplier;
        StartCoroutine(ScaleButton(button.transform, button.transform.localScale, scaleTo, hoverAnimationSpeed));
    }

    void OnPointerExit(Button button)
    {
        // Handle saat pointer keluar
        if (button == creditButton)
        {
            Outline outline = button.GetComponent<Outline>();
            if (outline != null)
                outline.effectColor = new Color(0.9f, 0.9f, 1f, 0f);

            Image img = button.GetComponent<Image>();
            if (img != null)
                img.color = normalColor;

            return;
        }

        StopAllCoroutines();

        Outline o = button.GetComponent<Outline>();
        if (o != null)
            o.effectColor = new Color(0.9f, 0.9f, 1f, 0f);

        Image i = button.GetComponent<Image>();
        if (i != null)
            i.color = normalColor;

        StartCoroutine(ScaleButton(button.transform, button.transform.localScale, defaultButtonScale, hoverAnimationSpeed));
    }

    void OnPointerDown(Button button)
    {
        // Handle saat tombol ditekan
        if (button == creditButton) return;

        StopAllCoroutines();
        if (clickSound != null)
        {
            clickAudioSource.clip = clickSound;
            clickAudioSource.Play();
        }

        StartCoroutine(ScaleButton(button.transform, button.transform.localScale, defaultButtonScale * 0.95f, hoverAnimationSpeed / 2));
    }

    void OnPointerUp(Button button)
    {
        // Handle saat tombol dilepas
        if (button == creditButton) return;

        StopAllCoroutines();
        StartCoroutine(ScaleButton(button.transform, button.transform.localScale, defaultButtonScale, hoverAnimationSpeed / 2));
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