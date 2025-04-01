using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class PerformBootstrap
{
    private const string SceneName = "BootstrapScene";

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    public static void Execute()
    {
        for (int sceneIndex = 0; sceneIndex < SceneManager.sceneCount; sceneIndex++)
        {
            var scene = SceneManager.GetSceneAt(sceneIndex);

            if (scene.name == SceneName)
            {
                return;
            }
        }

        Debug.Log("Loading bootstrap scene: " + SceneName);
        SceneManager.LoadScene(SceneName, LoadSceneMode.Additive); 
    }
}

public class BootstrapEntryPoint : MonoBehaviour
{
    public static BootstrapEntryPoint Instance { get; private set; } = null;
    
    void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError("Found another BootstrapData on " + gameObject.name);
            Destroy(gameObject);
            return;
        }

        Instance = this;

        DontDestroyOnLoad(gameObject);
    }
}
