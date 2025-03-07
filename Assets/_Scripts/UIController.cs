using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class UIController : MonoBehaviour
{
    [SerializeField] private GameObject player1;
    [SerializeField] private GameObject player2;
    private TextMeshProUGUI _timerText;
    private TextMeshProUGUI _player1Score;
    private TextMeshProUGUI _player2Score;
    
    private float _remainingTime = 180f;
    
    private void Awake()
    {
        _timerText = GetComponentText("Time");
        _player1Score = GetComponentText("Player 1 Score");
        _player2Score = GetComponentText("Player 2 Score");
    }

    private TextMeshProUGUI GetComponentText(string name)
    {
        TextMeshProUGUI text = null;
        Transform child = GetComponentsInChildren<Transform>().FirstOrDefault(t => t.name == name);
        if (child != null)
            text = child.gameObject.GetComponentInChildren<TextMeshProUGUI>();
        return text;
    }
    
    private void Update()
    {
        GameTimer();
        _player1Score.text = player1.GetComponent<PlayerController>().Score.ToString();
        _player2Score.text = player2.GetComponent<PlayerController2>().Score.ToString();
    }
    
    private void GameTimer()
    {
        if (_remainingTime <= 0)
        {
            Debug.Log("Hết thời gian");
            return;
        }
        _remainingTime -= Time.deltaTime;
        int minutes = Mathf.FloorToInt(_remainingTime / 60);
        int seconds = Mathf.FloorToInt(_remainingTime % 60);
        if(_timerText != null)
            _timerText.text = string.Format("{0:0}:{1:00}", minutes, seconds);
    }
}
