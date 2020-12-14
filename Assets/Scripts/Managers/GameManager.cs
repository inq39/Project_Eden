using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private string _currentSceneName = string.Empty;
    List<AsyncOperation> _loadOperations;

    public void Start()
    {
        DontDestroyOnLoad(gameObject);
        _loadOperations = new List<AsyncOperation>();
        LoadScene("Main");
    }
    void LoadScene(string sceneName)
    {
        AsyncOperation ao = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
        if (ao == null)
        {
            Debug.LogError("[GameManager] Unable to load Scene " + sceneName);
            return;
        }
        ao.completed += OnLoadOperationComplete;
        _currentSceneName = sceneName;
    }

    void UnloadScene(string sceneName)
    {
        AsyncOperation ao = SceneManager.UnloadSceneAsync(sceneName);
        if(ao == null)
        {
            Debug.LogError("[GameManager] Unable to unload Scene " + sceneName);
            return;
        }
        ao.completed += OnUnloadOperationsComplete;
    }

    void OnLoadOperationComplete(AsyncOperation ao)
    {
        if (_loadOperations.Contains(ao))
        {
            _loadOperations.Remove(ao);
        }
        Debug.Log("Load Complete.");
    }

    void OnUnloadOperationsComplete(AsyncOperation ao)
    {
        Debug.Log("Unload Complete.");
    }
}
