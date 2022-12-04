using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.Events;

public class Character : MonoBehaviour
{
    private Rigidbody2D rb;
    private int score;

    [SerializeField] private UiHandler uiHandler;
    [SerializeField] private float jumpForce = 10f;
    [SerializeField] private UnityEvent onPlayerDie;
    [SerializeField] private UnityEvent onPlayerRecvScore;
    
    private void Awake()
    {
        rb = this.GetComponent<Rigidbody2D>();
        if (onPlayerDie == null)
        {
            onPlayerDie = new UnityEvent();
        }
        if (onPlayerRecvScore == null)
        {
            onPlayerRecvScore = new UnityEvent();
        }

        if (uiHandler == null)
        {
            uiHandler = FindObjectOfType<UiHandler>();
        }
    }

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            // rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            rb.velocity = Vector2.up * jumpForce;
 
        }
    }
    
    private void OnCollisionEnter2D(Collision2D col)
    {
        var cl = col.gameObject.GetComponent<Cloud>();
        if (cl != null)
        {
            this.onPlayerDie.Invoke();
            return;
        }
        
        var coin = col.gameObject.GetComponent<Coin>();
        if (coin != null)
        {
            score += coin.Value;
            this.uiHandler.UpdateScore(score);
            Destroy(coin.gameObject);
        }
        
    }

    public void SubscribeOnPlayerDie(UnityAction action)
    {
        this.onPlayerDie.AddListener(action);
    }
}