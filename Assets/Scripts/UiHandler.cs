using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class UiHandler : MonoBehaviour
{
    [SerializeField] private Text finalText;
    [SerializeField] private Text score;
    [SerializeField] private Button pause;

    private bool _paused;
    
    // Start is called before the first frame update
    void Start()
    {
        _paused = false;
        finalText.enabled = false;
        pause.onClick.AddListener(this.Pause);
    }

    private void Pause()
    {
        _paused = !_paused;
        Time.timeScale = _paused ? 0 : 1;
    }

    public void UpdateScore(int score)
    {
        this.score.text = $"Score: {score}";
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Lose()
    {
        finalText.enabled = true;
        Pause();
    }
}
