using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private Animation _mainMenuAnimator;
    [SerializeField] private AnimationClip _fadeInAnimation;
    [SerializeField] private AnimationClip _fadeOutAnimation;


    public void Start()
    {
        GameManager.Instance.onGameStateChange.AddListener(HandleGameStateChange);
    }
    public void OnFadeInComplete()
    {
        Debug.LogWarning("FadeIn Complete.");

        UIManager.Instance.SetDummyCameraActive(true);
    }

    public void OnFadeOutComplete()
    {
        Debug.LogWarning("FadeOut Complete.");
    }

    void HandleGameStateChange(GameManager.GameState currentGameState, GameManager.GameState previousGameState)
    {
        if (previousGameState == GameManager.GameState.PREGAME && currentGameState == GameManager.GameState.PLAYING)
        {
            FadeOut();
        }
    }

    public void FadeIn()
    {
        _mainMenuAnimator.Stop();
        _mainMenuAnimator.clip = _fadeInAnimation; 
        _mainMenuAnimator.Play();
    }

    public void FadeOut()
    {

        UIManager.Instance.SetDummyCameraActive(false);
        _mainMenuAnimator.Stop();
        _mainMenuAnimator.clip = _fadeInAnimation;
        _mainMenuAnimator.Play();
    }
}
