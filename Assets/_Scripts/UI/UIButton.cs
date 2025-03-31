using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;
using UnityEngine.SceneManagement;

public class UIButton : MonoBehaviour
{
    private Animator _ani;
    public GameObject pausePanel;
    
    private void Awake()
    {
        _ani = GetComponentInChildren<Animator>();
    }

    public void LoadGameScene()
    {
        SceneManager.LoadScene("GamePlayScene");
        GameManager.Instance.ChangeState(GameState.CutScene);
    }

    public void LoadMenuScene()
    {
        SceneManager.LoadScene("MenuScene");
        GameManager.Instance.ChangeState(GameState.Menu);
    }

    public void OpenPauseMenu()
    {
        PostProcessVolume ppVolume = Camera.main.gameObject.GetComponent<PostProcessVolume>();
        ppVolume.enabled = !ppVolume.enabled;
        pausePanel.SetActive(true);
        
        GameManager.Instance.ChangeState(GameState.Paused);
    }

    public void ContinueGame()
    {
        PostProcessVolume ppVolume = Camera.main.gameObject.GetComponent<PostProcessVolume>();
        ppVolume.enabled = !ppVolume.enabled;
        pausePanel.SetActive(false);
        
        GameManager.Instance.ChangeState(GameState.Playing);
    }
    
}
