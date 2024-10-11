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
    // Start is called before the first frame update
    void Start()
    {
        growthStage = 0;
        //growth stage 0: No Seed No Water, Temp Color: brown
        //Stage 1: Seed No Water, Temp Color: red
        //Stage 2: Seed + Water = first stage of crop, Temp Color: blue
        //Stage 3: Second stage of crop growth
        //Stage 4: Final Stage of Crop Growth, can be picked
        //Stage 5: Rotted Crop
        hasWater = false;
        hasSeed = false;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void plantSeed()
    {
        tempColorChange.color = tempColor1;
        Debug.Log("planted");
    }

    public void waterCrop()
    {
        tempColorChange.color = tempColor2;
        Debug.Log("watered");
    }


}
