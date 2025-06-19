using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Audio;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;

// Handle main menu
// Script ngatur menu utama
public class MainMenu : MonoBehaviour
{
    private static bool hasInitialized = false;    // Flag inisialisasi

    [Header("Menu Buttons")]
    public Button newGameButton;                   // Tombol new game
    public Button continueButton;                  // Tombol continue
    public Button optionButton;                    // Tombol options
    public Button exitButton;                      // Tombol exit

    [Header("Button Sprites")]
    public Sprite newGameSprite;                   // Sprite new game
    public Sprite continueSprite;                  // Sprite continue
    public Sprite optionSprite;                    // Sprite options
    public Sprite exitSprite;                      // Sprite exit

    [Header("Menu Panels")]
    public GameObject mainMenuPanel;               // Panel menu utama
    public GameObject optionPanel;                 // Panel options

    [Header("Background")]
    public Image backgroundImage;                  // Image background
    public Sprite backgroundSprite;                // Sprite background

    [Header("Hover Effects")]
    [Range(1f, 1.5f)] public float hoverScaleMultiplier = 1.1f;    // Skala saat hover
    [Range(0.1f, 1f)] public float hoverAnimationSpeed = 0.2f;     // Kecepatan animasi hover
    public Color hoverTintColor = new Color(1f, 1f, 1f, 1f);       // Warna hover
    public Color normalColor = Color.white;                         // Warna normal
    public Color hoverColor = new Color(0.8f, 0.8f, 1f, 1f);       // Warna saat hover

    [Header("Button Sound")]
    public AudioClip hoverSound;                   // Suara hover
    public AudioClip clickSound;                   // Suara klik

    private AudioSource audioSource;               // Source suara
    private AudioSource hoverAudioSource;          // Source suara hover
    private GameManager gameManager;               // Referensi game manager

    void Start()
    {
        // Reset data saat pertama kali play
        if (!hasInitialized)
        {
            PlayerPrefs.DeleteAll();
            PlayerPrefs.Save();
            hasInitialized = true;
        }

        // Setup game manager
        gameManager = GameManager.instance;
        if (gameManager == null)
        {
            GameObject gmObject = new GameObject("GameManager");
            gameManager = gmObject.AddComponent<GameManager>();
        }

        // Setup UI
        if (mainMenuPanel != null) mainMenuPanel.SetActive(true);
        if (optionPanel != null) optionPanel.SetActive(false);

        SetupAudioSource();
        SetupHoverAudioSource();
        SetupBackground();
        SetupButtonVisuals();
        SetupButtonListeners();
        AddHoverEffects();
        CheckSaveGame();
    }

    void SetupAudioSource()
    {
        // Setup audio source
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
            audioSource.playOnAwake = false;
            audioSource.volume = 0.5f;
        }
    }

    void SetupHoverAudioSource()
    {
        // Setup audio source untuk hover
        hoverAudioSource = gameObject.AddComponent<AudioSource>();
        hoverAudioSource.playOnAwake = false;
        hoverAudioSource.volume = 0.5f;
    }

    void SetupBackground()
    {
        // Setup background
        if (backgroundImage != null && backgroundSprite != null)
        {
            backgroundImage.sprite = backgroundSprite;
        }
    }

    void SetupButtonVisuals()
    {
        // Setup tampilan tombol
        SetupButton(newGameButton, newGameSprite);
        SetupButton(continueButton, continueSprite);
        SetupButton(optionButton, optionSprite);
        SetupButton(exitButton, exitSprite);
    }

    void SetupButton(Button button, Sprite sprite)
    {
        // Setup tampilan tombol individual
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

            button.transform.localScale = new Vector3(2.4407f, 2.4407f, 2.4407f);

            Text txt = button.GetComponentInChildren<Text>();
            if (txt != null) txt.text = "";
        }
    }

    void SetupButtonListeners()
    {
        // Setup event listener tombol
        if (newGameButton != null)
        {
            newGameButton.onClick.RemoveAllListeners();
            newGameButton.onClick.AddListener(NewGame);
        }

        if (continueButton != null)
        {
            continueButton.onClick.RemoveAllListeners();
            continueButton.onClick.AddListener(ContinueGame);
        }

        if (optionButton != null)
        {
            optionButton.onClick.RemoveAllListeners();
            optionButton.onClick.AddListener(OpenOptions);
        }

        if (exitButton != null)
        {
            exitButton.onClick.RemoveAllListeners();
            exitButton.onClick.AddListener(ExitGame);
        }
    }

    void CheckSaveGame()
    {
        // Cek save game
        if (continueButton != null)
        {
            bool hasSaveGame = PlayerPrefs.GetInt("HasCheckpoint", 0) == 1;
            continueButton.interactable = hasSaveGame;
        }
    }

    public void NewGame()
    {
        // Mulai game baru
        PlayerPrefs.DeleteAll();
        PlayerPrefs.Save();

        if (gameManager != null)
            gameManager.StartNewGame();
        else
        {
            bool sceneExists = false;
            for (int i = 0; i < SceneManager.sceneCountInBuildSettings; i++)
            {
                string sceneName = System.IO.Path.GetFileNameWithoutExtension(SceneUtility.GetScenePathByBuildIndex(i));
                if (sceneName == "SampleScene")
                    sceneExists = true;
            }

            if (!sceneExists)
            {
                ShowErrorMessage("Scene 'SampleScene' tidak ada di Build Settings!");
                return;
            }

            SceneManager.LoadScene("SampleScene");
        }
    }

    public void ContinueGame()
    {
        // Lanjutkan game
        if (PlayerPrefs.GetInt("HasCheckpoint", 0) == 1)
        {
            if (gameManager != null)
                gameManager.ContinueGame();
            else
                SceneManager.LoadScene("SampleScene");
        }
    }

    public void OpenOptions()
    {
        // Buka menu options
        if (optionPanel != null)
        {
            mainMenuPanel.SetActive(false);
            optionPanel.SetActive(true);
        }
    }

    public void CloseOptions()
    {
        // Tutup menu options
        if (optionPanel != null)
        {
            optionPanel.SetActive(false);
            mainMenuPanel.SetActive(true);
        }
    }

    public void ExitGame()
    {
        // Keluar game
        if (gameManager != null)
            gameManager.ExitGame();
        else
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
        }
    }

    void AddHoverEffects()
    {
        // Tambah efek hover ke tombol
        Vector3 buttonScale = new Vector3(2.4407f, 2.4407f, 2.4407f);
        AddHoverEffect(newGameButton, buttonScale, hoverScaleMultiplier);
        AddHoverEffect(continueButton, buttonScale, hoverScaleMultiplier);
        AddHoverEffect(optionButton, buttonScale, hoverScaleMultiplier);
        AddHoverEffect(exitButton, buttonScale, 1.815075f / 2.4407f);
    }

    void AddHoverEffect(Button button, Vector3 normalScale, float customMultiplier)
    {
        // Tambah efek hover ke tombol individual
        if (button == null) return;

        EventTrigger trigger = button.GetComponent<EventTrigger>();
        if (trigger == null)
            trigger = button.gameObject.AddComponent<EventTrigger>();

        trigger.triggers.Clear();

        AddTrigger(trigger, EventTriggerType.PointerEnter, () => OnPointerEnter(button, normalScale, customMultiplier));
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

    void OnPointerEnter(Button button, Vector3 normalScale, float customMultiplier)
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
        if (img != null) img.color = hoverColor;

        Vector3 scaleTo = normalScale * customMultiplier;
        StartCoroutine(ScaleButton(button.transform, normalScale, scaleTo, hoverAnimationSpeed));
    }

    void OnPointerExit(Button button, Vector3 normalScale)
    {
        // Handle saat pointer keluar
        StopAllCoroutines();
        Outline outline = button.GetComponent<Outline>();
        if (outline != null) outline.effectColor = new Color(0.9f, 0.9f, 1f, 0f);

        Image img = button.GetComponent<Image>();
        if (img != null) img.color = normalColor;

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

    private void ShowErrorMessage(string message)
    {
        // Tampilkan pesan error
        Debug.LogError("ERROR: " + message);
    }
}
