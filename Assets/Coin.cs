using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private int value = 10;

    public int Value => value;

    // Update is called once per frame
    void Update()
    {
       this.transform.position += Vector3.left * (speed * Time.deltaTime);
       if (this.transform.position.x < -20)
       {
           Destroy(gameObject);
       }
    }
}
