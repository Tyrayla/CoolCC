using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices.WindowsRuntime;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class NewTestMovement : MonoBehaviour
{
    private string lastTouched;
    private GameObject lasttouchedObject;
    private bool xlimit = false;
    private bool ylimit = false;
    private bool spaceLimit = false;
    public int water = 0;
    private bool halfway = false;
    private bool threeFourths = false;
    public Text alertBoxText;

    public cropBehavior targetCrop;

    public float waterCoolDown = 0.5f;
    private float waterCooldownNextUse = 0;
    private float seedCoolDown = 0.5f;
    private float seedCooldownNextUse = 0;
    public float gapTimer = 0.5f;
    private float gapTimerNextUse = 0;

    //this is a place holder for better UI later, to be replaced by an image updater
    public Text tempWaterPlaceholder;

    //these are place holders for UI assests aswell, same as the last
    public Text tempInventorySlot1;
    public Text tempInventorySlot2;
    public Text tempInventorySlot3;
    public Text tempInventorySlot4;
    public Text tempInventorySlot5;

    // 0 corresponds with empty, 1 with seed, 2 with full crop
    private int[] inventory;


    [SerializeField]
    private float speedX = 1f;

    private void Start()
    {
        inventory = new int[5];
        for (int i = 0; i < 5; i++)
          
        {
            inventory[i] = 0;
        }
        updateInventoryText();
      

    }

    void Update()
    {
        movePlayer();
        playerControls();

    }

    private void movePlayer()
    {
        float movementX = Input.GetAxisRaw("Horizontal");
        float movementY = Input.GetAxisRaw("Vertical");


        //Set Y Limit
        if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.RightArrow))
        {
            ylimit = true;
        }
        if (Input.GetKeyUp(KeyCode.A) || Input.GetKeyUp(KeyCode.D) || Input.GetKeyUp(KeyCode.LeftArrow) || Input.GetKey(KeyCode.RightArrow))
        {
            ylimit = false;
        }

        //Set X Limit
        if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.DownArrow))
        {
            xlimit = true;
        }
        if (Input.GetKeyUp(KeyCode.W) || Input.GetKeyUp(KeyCode.S) || Input.GetKeyUp(KeyCode.UpArrow) || Input.GetKey(KeyCode.DownArrow))
        {
            xlimit = false;
        }


        //actual movement
        if (xlimit == false)
        {
            transform.position = transform.position + new Vector3(movementX * speedX * Time.deltaTime, 0, 0);
        }
        if (ylimit == false)
        {
            transform.position = transform.position + new Vector3(0, movementY * speedX * Time.deltaTime, 0);
        }
    }

    private void playerControls()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            if (spaceLimit)
            {
                if (lastTouched.Contains("Crop"))
                {
                    targetCrop = lasttouchedObject.GetComponent<cropBehavior>();
                    if (targetCrop.hasSeed)
                    {
                        if (water > 0)
                        {
                            updateWaterTextAndSubtract(true);
                        }
                    }
                    else
                    {
                        playerPlantSeed();
                    }
                    
                }

                if (lastTouched.Contains("Water"))
                {
                    updateWater();
                }

                if (lastTouched.Contains("SeedsBox"))
                {
                    getSeedFromBox();
                }
            }

        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        lastTouched = collision.gameObject.name;
        lasttouchedObject= collision.gameObject;
        spaceLimit = true;
        Debug.Log(lastTouched);
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        spaceLimit = false;
    }

    private void updateWater()
    {
        if (halfway)
        {
            water = 2;
        }
        else if (threeFourths)
        {
            water = 3;
        }
        else
        {
            water = 4;
        }
        updateWaterTextAndSubtract(false);
    }

    private void updateWaterTextAndSubtract(bool subtract)
    {
        if (subtract)
        {
            //added a 1 second timer from youtube tutorial: https://www.youtube.com/watch?v=NX8cX3osMFc&ab_channel=Bilal
            if (Time.time > waterCooldownNextUse && Time.time>gapTimerNextUse)
            {
                //here is the call to water the crop
                if (targetCrop.hasSeed&&!targetCrop.hasWater)
                {
                    water--;
                    waterCooldownNextUse = Time.time + waterCoolDown;
                    targetCrop.hasWater = true;
                    targetCrop.waterCrop();
                }
                
            }
            else
            {
                alertBoxText.text = "Water Cooldown, Can Use again in 1 second";
            }
        }
        string temp = "Water: " + water.ToString();
        alertBoxText.text = temp;
        tempWaterPlaceholder.text = water.ToString();
    }

    private void updateInventoryText()
    {
        //update Slot 1
        if (inventory[0] == 0)
        {
            tempInventorySlot1.text = "Empty";
        }
        else if (inventory[0] == 1)
        {
            tempInventorySlot1.text = "Seed";
        }
        else
        {
            tempInventorySlot1.text = "Crop";
        }

        if (inventory[1] == 0)
        {
            tempInventorySlot2.text = "Empty";
        }
        else if (inventory[1] == 1)
        {
            tempInventorySlot2.text = "Seed";
        }
        else
        {
            tempInventorySlot2.text = "Crop";
        }

        //update Slot 2
        if (inventory[2] == 0)
        {
            tempInventorySlot3.text = "Empty";
        }
        else if (inventory[2] == 1)
        {
            tempInventorySlot3.text = "Seed";
        }
        else
        {
            tempInventorySlot3.text = "Crop";
        }

        //update Slot 3
        if (inventory[3] == 0)
        {
            tempInventorySlot4.text = "Empty";
        }
        else if (inventory[3] == 1)
        {
            tempInventorySlot4.text = "Seed";
        }
        else
        {
            tempInventorySlot4.text = "Crop";
        }

        //update Slot 4
        if (inventory[4] == 0)
        {
            tempInventorySlot5.text = "Empty";
        }
        else if (inventory[4] == 1)
        {
            tempInventorySlot5.text = "Seed";
        }
        else
        {
            tempInventorySlot5.text = "Crop";
        }

    }

    private void getSeedFromBox()
    {
        if (Time.time > seedCooldownNextUse)
        {
            seedCooldownNextUse = Time.time + seedCoolDown;
            for (int i = 0; i < inventory.Length; i++)
            {
                if (inventory[i] == 0)
                {
                    inventory[i] = 1;
                    break;
                }
            }
        }
        else
        {
            alertBoxText.text = "Seed Cooldown, Can Use again in 1 second";
        }
        updateInventoryText();
    }

    private void playerPlantSeed()
    {
        int tempCount = 0;
        Debug.Log("hello");
        for (int i = 0; i < inventory.Length; i++)
        {
            if (inventory[i] == 1)
            {
                inventory[i] = 0;
                tempCount++;
                break;
            }
        }
        updateInventoryText();

        if (tempCount != 0)
        {
            gapTimerNextUse = Time.time + gapTimer;
            targetCrop.plantSeed();
            targetCrop.hasSeed = true;
        }
        
    }





}
