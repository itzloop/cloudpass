using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using TMPro;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.Events;
using Random = System.Random;

public class GameHandler : MonoBehaviour
{
    private GameObject _currentCloudSpawner;
    private List<GameObject> _cloudSpawners;
    private float _offset = 1f;
    private Vector2 _spawnPosition;
    private float _xLimit, _xScale, _delta = 20f;
    private float lastTime;
    private Random _random;

    [SerializeField] private GameObject cloudSpawner;
    [SerializeField] private Character character;
    [SerializeField] private UiHandler ui;
    [SerializeField] private GameObject coin;
    private void Awake()
    {
        _random = new Random();
        _xScale = (float)(Camera.main.orthographicSize * 2.0 * Screen.width / Screen.height);
        _xLimit = _xScale + _delta / 2;
        _spawnPosition = new Vector3(_xLimit, 0, 90);
        _cloudSpawners = new List<GameObject>();
        _currentCloudSpawner = SpawnCloud();

        if (character == null) 
            character = GameObject.FindObjectOfType<Character>();
        
        if (ui == null)
            ui = GameObject.FindObjectOfType<UiHandler>();
        
    }

    private void Start()
    {
        character.SubscribeOnPlayerDie(delegate
        {
            ui.Lose();
            Destroy(character.gameObject);
        });
    }

    // Update is called once per frame
    void Update()
    {
        if (_currentCloudSpawner.transform.position.x <= -_delta / 2 + _offset)
        {
            _currentCloudSpawner = SpawnCloud();
        }

        if (_cloudSpawners.Count > 0 && _cloudSpawners[0].transform.position.x <= -_xLimit)
        {
            var go = _cloudSpawners[0];
            _cloudSpawners.RemoveAt(0);
            Destroy(go);
        }

        lastTime += Time.deltaTime;
        if (lastTime >= _random.Next(1,5))
        {
            lastTime = 0;
            SpawnCoin();
        }
    }

    private void SpawnCoin()
    {
        var y = -1f + (float)_random.NextDouble() * 2;
        var pos = new Vector3(_spawnPosition.x, y, 0);
        Instantiate(this.coin, pos, Quaternion.identity);
    }

    private int ySteps = 30;
    private GameObject SpawnCloud()
    {
        var goUp = true;
        var lastScale = 0.1f;
        
        if (this._cloudSpawners.Count > 0)
        {
            var spawner = this._cloudSpawners[this._cloudSpawners.Count - 1].GetComponent<Spawner>();
            // lastStep = spawner.Equals(null) ? 0 : spawner.LastStep;
            goUp = spawner.GoUp;
            lastScale = spawner.LastScale;
        }

        var go = Instantiate(cloudSpawner, this._spawnPosition, Quaternion.identity);
        var sp = go.GetComponent<Spawner>();
        sp.GenerateNewClouds(30, ySteps, 0.01f, 0.4f, .01f, 0.01f, lastScale, goUp);
        ySteps = (ySteps + 1) % 41;
        if (ySteps < 4) ySteps = 15;
        go.transform.localScale = new Vector3(_xScale + _delta, _xScale / 2, 1);
        this._cloudSpawners.Add(go);
        return go;
    }
}