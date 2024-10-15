using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cropBehavior : MonoBehaviour
{
    public bool hasWater;
    public bool hasSeed;
    public int growthStage;

    public SpriteRenderer tempColorChange;

    private Color tempColor1 = Color.red;
    private Color tempColor2= Color.blue;
    private Color tempColor3 = Color.yellow;
    private Color tempColor4 = Color.green;
    private Color tempColor5 = Color.magenta;
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
        randomGrowthTime = UnityEngine.Random.Range(1f,5f);
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

}
