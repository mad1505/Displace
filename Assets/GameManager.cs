using UnityEngine;
using UnityEngine.SceneManagement;

// Handle manajemen game
// Script ngatur scene dan game state
public class GameManager : MonoBehaviour
{
    public static GameManager instance;        // Instance singleton
    
    [Header("Scene Names")]
    public string mainMenuScene = "MainMenu";  // Scene menu utama
    public string gameScene = "SampleScene";   // Scene gameplay
    private string actualGameScene;            // Scene game yang aktif
    
    void Awake()
    {
        // Setup singleton
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
        
        ValidateScenes();
    }
    
    private void ValidateScenes()
    {
        // Cek scene di build settings
        bool mainMenuValid = false;
        bool gameSceneValid = false;
        string alternativeScene = "";
        
        for (int i = 0; i < SceneManager.sceneCountInBuildSettings; i++)
        {
            string scenePath = SceneUtility.GetScenePathByBuildIndex(i);
            string sceneName = System.IO.Path.GetFileNameWithoutExtension(scenePath);
            
            if (sceneName == mainMenuScene) mainMenuValid = true;
            if (sceneName == gameScene) 
            {
                gameSceneValid = true;
                actualGameScene = gameScene;
            }
            
            // Simpan scene alternatif
            if (sceneName != mainMenuScene && alternativeScene == "")
            {
                alternativeScene = sceneName;
            }
        }
        
        // Validasi scene
        if (!mainMenuValid)
            Debug.LogError("Scene '" + mainMenuScene + "' tidak ada di Build Settings");
            
        if (!gameSceneValid)
        {
            Debug.LogError("Scene '" + gameScene + "' tidak ada di Build Settings");
            
            if (alternativeScene != "")
            {
                Debug.LogWarning("Pakai scene alternatif: " + alternativeScene);
                actualGameScene = alternativeScene;
            }
        }
    }
    
    public void StartNewGame()
    {
        try 
        {
            // Load scene game
            string sceneToLoad = !string.IsNullOrEmpty(actualGameScene) ? actualGameScene : gameScene;
            SceneManager.LoadScene(sceneToLoad);
        }
        catch (System.Exception e)
        {
            Debug.LogError("Gagal load scene: " + e.Message);
            
            // List scene yang tersedia
            string availableScenes = "";
            for (int i = 0; i < SceneManager.sceneCountInBuildSettings; i++)
            {
                string scenePath = SceneUtility.GetScenePathByBuildIndex(i);
                string sceneName = System.IO.Path.GetFileNameWithoutExtension(scenePath);
                availableScenes += sceneName + ", ";
            }
            
            Debug.LogWarning("Scene yang tersedia: " + availableScenes);
        }
    }
    
    public void ContinueGame()
    {
        // Cek save game
        bool hasSaveGame = PlayerPrefs.HasKey("PlayerLevel");
        
        if (hasSaveGame)
        {
            // Load level terakhir
            string lastLevel = PlayerPrefs.GetString("PlayerLevel", gameScene);
            try {
                SceneManager.LoadScene(lastLevel);
            } catch {
                SceneManager.LoadScene(gameScene);
            }
        }
        else
        {
            StartNewGame();
        }
    }
    
    public void ExitGame()
    {
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #else
            Application.Quit();
        #endif
    }
    
    public void LoadMainMenu()
    {
        SceneManager.LoadScene(mainMenuScene);
    }
} 