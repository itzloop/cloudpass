using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions.Must;
using Random = System.Random;

public class Spawner : MonoBehaviour
{

    private float duration = 1f, lastTime;
    private Camera _camera;
    private Vector3 bottomRight;
    private SpriteRenderer _renderer;
    public int LastStep { get; set;  }
    public bool GoUp { get; private set; }
    public float LastScale { get; private set; }
    [SerializeField] private Cloud cloud;
    [SerializeField] private float speed = 20f;


    // Update is called once per frame
    void Update()
    {
        this.Move();
    }
    
    private void Move()
    {
        this.transform.position += Vector3.left * (Time.deltaTime * speed);
    }
    
    private float getCloudY(float scale, bool up)
    {
        var delta = scale / 2;
        return up ? .5f - delta: -.5f + delta;
    }
    
    private float getCloudX(float scale, int i, bool up)
    {
        var delta = scale / 2;
        var start = -.5f + delta;
        return start + scale * i;
    }

    public void GenerateClouds(int count, int ySteps, float baseScale, int lastStep)
    {
        var scaleX = 1f / count;
        var counter = lastStep;
        var reverse = false;
        for (var i = 0; i < count; i++)
        {
            counter = reverse ? counter - 1 : counter + 1;
            if (counter == ySteps + 1 || counter == 0)
            {
                reverse = !reverse;
                if (counter == 0)
                {
                    counter = 1;
                } else if (counter == ySteps + 1)
                {
                    counter = ySteps;
                }
            }
            
            var clDown = Instantiate(cloud, Vector3.zero, Quaternion.identity, this.transform);
            var transform2 = clDown.transform;
            var scaleY = counter * baseScale;
            transform2.localScale = new Vector3(scaleX, scaleY, 1);
            transform2.localPosition = new Vector3(getCloudX(scaleX, i, false), getCloudY(scaleY, false), 0);
        }

        this.LastStep = counter;
    }

    public void GenerateNewClouds(
        int xSteps, 
        int ySteps, 
        float min, 
        float max, 
        float baseScale, 
        float prevBaseScale, 
        float lastScale,
        bool goUp)
    {
        // Debug.LogFormat("Running GenerateNewClouds with xSteps={0} ySteps={1} min={2} max={3} baseScale={4} prevBaseScale={5} lastScale={6} goUp={7}",
        //     xSteps, ySteps,min, max, baseScale, prevBaseScale, lastScale, goUp);
        var currentStep = (int)(lastScale / prevBaseScale);
        var xScale = 1f / xSteps;
        float yScale = 0;

        if (currentStep == 0)
            currentStep = 1;
        if (currentStep > ySteps)
        {
            currentStep = goUp? 1 : ySteps;
        }
        for (var i = 0; i < xSteps; i++)
        {
            if (goUp)
            {
                yScale = baseScale * currentStep;
                if (yScale > max)
                    yScale = max;
                else if (yScale < min)
                    yScale = min;

                currentStep++;
                if (currentStep == ySteps)
                {
                    goUp = false;
                }
            }
            else
            {
                yScale = baseScale * currentStep;
                if (yScale > max)
                    yScale = max;
                else if (yScale < min)
                    yScale = min;

                currentStep--;
                if (currentStep == 1)
                {
                    goUp = true;
                }
            }
            // Debug.LogFormat("({0}, {1}) step={2} goUp={3}", xScale, yScale, currentStep, goUp); 
            var cl = Instantiate(cloud, Vector3.zero, Quaternion.identity, this.transform);
            var transform2 = cl.transform;
            transform2.localScale = new Vector3(xScale, yScale, 1);
            transform2.localPosition = new Vector3(getCloudX(xScale, i, false), getCloudY(yScale, false), 0);
            
            var clUp = Instantiate(cloud, Vector3.zero, Quaternion.identity, this.transform);
            var transform1 = clUp.transform;
            transform1.localScale = new Vector3(xScale, yScale, 1);
            transform1.localPosition = new Vector3(getCloudX(xScale, i, false), getCloudY(yScale, true), 0);
        }

        this.GoUp = goUp;
        this.LastScale = yScale;
    }
}
