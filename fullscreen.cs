#if UNITY_EDITOR

using System;
using System.Reflection;
using UnityEditor;
using UnityEngine;

// Handle fullscreen di editor
// Script ngatur toggle fullscreen dengan hotkey
public class FullscreenHotkeyHandler : MonoBehaviour
{
    bool makeFullscreenAtStart = true;      // Fullscreen saat start
    
    void Start() 
    { 
        if (makeFullscreenAtStart) 
        { 
            FullscreenGameView.Toggle(); 
        } 
    }

    void Update() 
    {
        // Toggle fullscreen dengan backslash
        if (Input.GetKeyDown(KeyCode.Backslash))
        {
            FullscreenGameView.Toggle();
        }
    }
}

// Handle fullscreen game view
// Script ngatur window game view fullscreen
public static class FullscreenGameView
{
    static readonly Type GameViewType = Type.GetType("UnityEditor.GameView,UnityEditor");
    static readonly PropertyInfo ShowToolbarProperty = GameViewType.GetProperty("showToolbar", BindingFlags.Instance | BindingFlags.NonPublic);
    static readonly object False = false;

    static EditorWindow instance;

    // Exit fullscreen saat recompile
    static FullscreenGameView() 
    { 
        AssemblyReloadEvents.beforeAssemblyReload += OnBeforeAssemblyReload; 
    }
    
    private static void OnBeforeAssemblyReload() 
    { 
        if (instance != null) 
        { 
            instance.Close(); 
            instance = null; 
        } 
    }
    
    [MenuItem("Window/General/Game (Fullscreen) %#&2", priority = 2)]
    public static void Toggle()
    {
        if (GameViewType == null)
        {
            Debug.LogError("GameView type tidak ditemukan");
            return;
        }

        if (ShowToolbarProperty == null)
        {
            Debug.LogWarning("GameView.showToolbar property tidak ditemukan");
        }

        if (instance != null)
        {
            instance.Close();
            instance = null;
        }
        else
        {
            // Setup window fullscreen
            instance = (EditorWindow) ScriptableObject.CreateInstance(GameViewType);
            ShowToolbarProperty?.SetValue(instance, False);

            var desktopResolution = new Vector2(Screen.currentResolution.width, Screen.currentResolution.height);
            var fullscreenRect = new Rect(Vector2.zero, desktopResolution);
            instance.ShowPopup();
            instance.position = fullscreenRect;
            instance.Focus();
        }
    }
}

#endif
