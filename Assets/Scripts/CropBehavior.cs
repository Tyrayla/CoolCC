using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;
using UnityEngine.Rendering;

public class cropBehavior : MonoBehaviour
{
    public bool hasWater;
    public bool hasSeed;
    public int growthStage;


    public SpriteRenderer tempColorChange;

    private UnityEngine.Color tempColor0 = UnityEngine.Color.gray;
    private UnityEngine.Color tempColor1 = UnityEngine.Color.red;
    private UnityEngine.Color tempColor2= UnityEngine.Color.blue;
    private UnityEngine.Color tempColor3 = UnityEngine.Color.yellow;
    private UnityEngine.Color tempColor4 = UnityEngine.Color.green;
    private UnityEngine.Color tempColor5 = UnityEngine.Color.magenta;
    // Start is called before the first frame update

    public float startTime;
    public float randomGrowthTime;
    void Start()
    {
        growthStage = 0;
        //growth stage 0: No Seed No Water, Temp Color: brown
        //Stage 1: Seed No Water, Temp Color: red
        //Stage 2: Seed + Water = first stage of crop, Temp Color: blue
        //Stage 3: Second stage of crop growth, yellow 
        //Stage 4: Final Stage of Crop Growth, can be picked, green
        //Stage 5: Rotted Crop, purple
        hasWater = false;
        hasSeed = false;
    }

    // Update is called once per frame
    void Update()
    {
        grow();
    }

    public void plantSeed()
    {
        tempColorChange.color = tempColor1;
        Debug.Log("planted");
        hasSeed = true;
        growthStage=1;
        
    }

    public void waterCrop()
    {
        growthStage = 2;
        tempColorChange.color = tempColor2;
        Debug.Log("watered");
        hasWater = true;
        startTime = Time.time;
        randomGrowthTime = UnityEngine.Random.Range(10f,15f);
    }

    public void grow()
    {
        if(growthStage == 2)
        {
            if (Time.time >= startTime+ randomGrowthTime)
            {
                growthStage = 3;
                tempColorChange.color= tempColor3;
                startTime = Time.time;
            }

        }
        if (growthStage == 3)
        {
            if (Time.time >= startTime + randomGrowthTime)
            {
                growthStage = 4;
                tempColorChange.color = tempColor4;
                startTime = Time.time;
            }

        }
        if (growthStage == 4)
        {
            if (Time.time >= startTime + randomGrowthTime + 5f)
            {
                growthStage = 5;
                tempColorChange.color = tempColor5;
                startTime = Time.time;
            }

        }
    }

    public void pickedCrop()
    {
        tempColorChange.color =tempColor0;
        hasSeed = false;
        hasWater = false;
    }

}
