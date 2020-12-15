using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

[System.Serializable] public class EventGameState : UnityEvent<GameManager.GameState, GameManager.GameState> { }

public class GameManager : Singleton<GameManager>
{
    public enum GameState
    {
        PREGAME,
        PLAYING,
        PAUSED
    }

    public GameObject[] systemPrefabs;
    public EventGameState onGameStateChange;

    List<GameObject> _instantiatedSystemPrefabs;
    List<AsyncOperation> _loadOperations;
    GameState _currentGameState = GameState.PREGAME;

    private string _currentSceneName = string.Empty;
    
    public GameState CurrentGameState
    {
        get { return _currentGameState; }
        private set { _currentGameState = value; }
    }
    public void Start()
    {
        DontDestroyOnLoad(gameObject);
        _loadOperations = new List<AsyncOperation>();

        InstantiateSystemPrefabs();
    }
    

    void OnLoadOperationComplete(AsyncOperation ao)
    {
        if (_loadOperations.Contains(ao))
        {
            _loadOperations.Remove(ao);

            if (_loadOperations.Count == 0)
            {
                UpdateGameState(GameState.PLAYING);
            }
            
        }
        Debug.Log("Load Complete.");
    }

    void OnUnloadOperationsComplete(AsyncOperation ao)
    {
        Debug.Log("Unload Complete.");
    }
    void UpdateGameState(GameState gameState)
    {
        GameState _previousGameState = _currentGameState;
        _currentGameState = gameState;

        switch (_currentGameState)
        {
            case GameState.PREGAME:
                break;
            case GameState.PLAYING:
                break;
            case GameState.PAUSED:
                break;
            default:
                break;
        }
        onGameStateChange.Invoke(_currentGameState, _previousGameState);
    }

    void InstantiateSystemPrefabs()
    {
        GameObject prefabInstance;
        for (int i = 0; i < systemPrefabs.Length; i++)
        {
            prefabInstance = Instantiate(systemPrefabs[i]);
            _instantiatedSystemPrefabs.Add(prefabInstance);
        }
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
        if (ao == null)
        {
            Debug.LogError("[GameManager] Unable to unload Scene " + sceneName);
            return;
        }
        ao.completed += OnUnloadOperationsComplete;
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();

        for (int i = 0; i < _instantiatedSystemPrefabs.Count; ++i)
        {
            Destroy(_instantiatedSystemPrefabs[i]);
        }
        _instantiatedSystemPrefabs.Clear();
                
    }

    public void StartGame()
    {
        LoadScene("Main");
    }
}
