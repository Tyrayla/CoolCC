using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices.WindowsRuntime;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

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
    

    public cropBehavior targetCrop;

    // Alex: audio stuff
    public AudioSource audioSource;
    public AudioClip interactSound;
    public AudioClip cropHarvestSound;
    public AudioClip itemPickupSound;
    public AudioClip sellSound;
    public AudioClip pickupWaterSound;
    public AudioClip waterSound;

    //Timers used to update intervals without breaking the update loop
    public float waterCoolDown = 0.5f;
    private float waterCooldownNextUse = 0;
    private float seedCoolDown = 0.5f;
    private float seedCooldownNextUse = 0;
    public float gapTimer = 0.5f;
    private float gapTimerNextUse = 0;
    public float pickedGapTimer = 0.5f;
    private float pickedGapTimerNextUse = 0;
    public float coinTimer = 0.5f;
    private float coinTimerNextUse = 0;
    public float investScoreTimer = 0.5f;
    private float investScoreTimerNextUse = 0;
    public float ecoTimer = 0.5f;
    private float ecoTimerNextUse = 0;
    private float waterSoundTimer = 0.5f;
    private float waterSoundTimerNextUse = 0;
    private float investInEcoSoundTimer = 0.5f;
    private float investInEcoSoundTimerNextUse = 0;
    private float investInScoreSoundTimer = 0.5f;
    private float investInScoreSoundTimerNextUse = 0;
    private float sellSoundTimer = 0.5f;
    private float sellSoundTimerNextUse = 0;

    //this is a place holder for better UI later, to be replaced by an image updater
    //public Text tempWaterPlaceholder;

    //these are place holders for UI assests aswell, same as the last
    //public Text tempInventorySlot1;
    //public Text tempInventorySlot2;
    //public Text tempInventorySlot3;
    //public Text tempInventorySlot4;
    //public Text tempInventorySlot5;

    //Actual Text UI
    public Text coinBox;
    public Text scoreBox;
    //public Text alertBoxText;
    public Text gameOver;

    // 0 corresponds with empty, 1 with seed, 2 with full crop
    private int[] inventory;

    private int coins;
    private int score;

    //Code from a countdown Timer tutorial https://www.youtube.com/watch?v=hxpUk0qiRGs&list=LL&index=3&ab_channel=TheGameGuy (Found in Udate Timer Fucntion and in the Update)
    public float TimeLeft;
    private float visualTimer;
    //public Text TimerText;
    public bool timerOn;
    public Slider temperatureSlider;

    //Animator Stuff
    public Animator animator;

    // pause menu stuff
    private bool paused = false;
    [SerializeField] GameObject pauseMenu;

    //Inventory UI Setup
    public GameObject inventorySlotOneSprite;
    public GameObject inventorySlotTwoSprite;
    public GameObject inventorySlotThreeSprite;
    public GameObject inventorySlotFourSprite;
    public GameObject inventorySlotFiveSprite;
    public GameObject inventorySlotWaterSprite;
    public GameObject BackgroundInvetorySprite;
    public GameObject BackgroundWater;

    //Sprites
    public Sprite carrot;
    public Sprite seed;
    public Sprite EmptySlot;
    public Sprite waterEmpty;
    public Sprite waterOneThird;
    public Sprite waterTwoThirds;
    public Sprite waterFull;

    //SpeedSetup
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
        coins = 0;
        score = 0;
        gameOver.text = "";
        visualTimer = 0;
    }

    void Update()
    {
        movePlayer();
        playerControls();
        if (transform.position.y< -1.65)
        {
            BackgroundInvetorySprite.GetComponent<Image>().color = new Color(1,1,1,0.3f);
            inventorySlotOneSprite.GetComponent<Image>().color = new Color(1, 1, 1, 0.3f);
            inventorySlotTwoSprite.GetComponent<Image>().color = new Color(1, 1, 1, 0.3f);
            inventorySlotThreeSprite.GetComponent<Image>().color = new Color(1, 1, 1, 0.3f);
            inventorySlotFourSprite.GetComponent<Image>().color = new Color(1, 1, 1, 0.3f);
            inventorySlotFiveSprite.GetComponent<Image>().color = new Color(1, 1, 1, 0.3f);
            inventorySlotWaterSprite.GetComponent<Image>().color = new Color(1, 1, 1, 0.3f);
            BackgroundWater.GetComponent<Image>().color = new Color(1, 1, 1, 0.3f);
        }
        else
        {
            BackgroundInvetorySprite.GetComponent<Image>().color = new Color(1, 1, 1, 1);
            inventorySlotOneSprite.GetComponent<Image>().color = new Color(1, 1, 1, 1);
            inventorySlotTwoSprite.GetComponent<Image>().color = new Color(1, 1, 1, 1);
            inventorySlotThreeSprite.GetComponent<Image>().color = new Color(1, 1, 1, 1);
            inventorySlotFourSprite.GetComponent<Image>().color = new Color(1, 1, 1, 1);
            inventorySlotFiveSprite.GetComponent<Image>().color = new Color(1, 1, 1, 1);
            inventorySlotWaterSprite.GetComponent<Image>().color = new Color(1, 1, 1, 1);
            BackgroundWater.GetComponent<Image>().color = new Color(1, 1, 1, 1);
        }
        if (TimeLeft > 0)
        {
            visualTimer += Time.deltaTime;
            TimeLeft-=Time.deltaTime;
            UpdateTimer(TimeLeft,visualTimer);
            timerOn = true;
        }
        else
        {
            TimeLeft = 0;
            timerOn = false;
            gameOver.text = "Game Over";
            visualTimer = 180f;
        }
        if (TimeLeft < 90f)
        {
            // Alex: I can add sounds here to trigger on each part way but we'd need to change the logic so they don't continue play. I'd recommend we check for every 25% within like 1s and then call the sounds then and update the values once
            // At the moment it looks like itll check this basically everytime the value time left is < the specified amount
            halfway = true;
        }
        if (TimeLeft < 45f)
        {
            threeFourths = true;
        }

       

    }

    private void movePlayer()
    {
        float movementX = Input.GetAxisRaw("Horizontal");
        float movementY = Input.GetAxisRaw("Vertical");

        if (timerOn)
        {
            //Set Y Limit
            if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.RightArrow))
            {
                ylimit = true;
            }
            if (Input.GetKeyUp(KeyCode.A) || Input.GetKeyUp(KeyCode.D) || Input.GetKeyUp(KeyCode.LeftArrow) || Input.GetKeyUp(KeyCode.RightArrow))
            {
                ylimit = false;
            }

            //Set X Limit
            if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.DownArrow))
            {
                xlimit = true;
            }
            if (Input.GetKeyUp(KeyCode.W) || Input.GetKeyUp(KeyCode.S) || Input.GetKeyUp(KeyCode.UpArrow) || Input.GetKeyUp(KeyCode.DownArrow))
            {
                xlimit = false;
            }

            if (Input.GetKeyDown(KeyCode.A)||Input.GetKeyDown(KeyCode.LeftArrow)){
                animator.SetBool("PlayerMovingLeft", true);
            }
            if (Input.GetKeyUp(KeyCode.A) || Input.GetKeyUp(KeyCode.LeftArrow))
            {
                animator.SetBool("PlayerMovingLeft", false);
            }
            if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow))
            {
                animator.SetBool("PlayerMovingRight", true);
            }
            if (Input.GetKeyUp(KeyCode.D) || Input.GetKeyUp(KeyCode.RightArrow))
            {
                animator.SetBool("PlayerMovingRight", false);
            }
            if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow))
            {
                animator.SetBool("PlayerMovingUp", true);
            }
            if (Input.GetKeyUp(KeyCode.W) || Input.GetKeyUp(KeyCode.UpArrow))
            {
                animator.SetBool("PlayerMovingUp", false);
            }
            if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow))
            {
                animator.SetBool("PlayerMovingDown", true);
            }
            if (Input.GetKeyUp(KeyCode.S) || Input.GetKeyUp(KeyCode.DownArrow))
            {
                animator.SetBool("PlayerMovingDown", false);
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

            if (Input.GetKeyUp(KeyCode.LeftArrow)){
                Debug.Log("Left");
            }
        }
    }

    private void playerControls()
    {
        if (timerOn) 
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
                        if (targetCrop.growthStage == 1)
                        {
                            if (water > 0)
                            {
                                updateWaterTextAndSubtract(true);
                            }
                        }
                        else if (targetCrop.growthStage == 4)
                        {
                            grabCrop();
                        }
                        else if (targetCrop.growthStage == 5)
                        {
                            targetCrop.growthStage = 0;
                            targetCrop.pickedCrop();
                        }
                    }
                    else
                    {
                        if (Time.time > pickedGapTimerNextUse)
                        {
                            playerPlantSeed();
                            pickedGapTimerNextUse = Time.time + pickedGapTimer;
                        }

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
                if (lastTouched.Contains("SellTruck"))
                {
                    if (Time.time > coinTimerNextUse)
                    {
                        sellCrops();
                        coinTimerNextUse = Time.time + coinTimer;
                    }
                }
                if (lastTouched.Contains("InvestInScore"))
                {
                    if (Time.time > investScoreTimerNextUse)
                    {
                        investScore();
                        investScoreTimerNextUse = Time.time + investScoreTimer;
                    }
                }
                if (lastTouched.Contains("InvestInEco"))
                {
                    if (Time.time > ecoTimerNextUse)
                    {
                        investEco();
                        investScoreTimerNextUse = Time.time + investScoreTimer;
                    }

                }
            }

        }
            if (Input.GetKey(KeyCode.P) && paused == false)
            {
                Pause();
                paused = true;
                //SceneManager.LoadScene(sceneName:"PauseMenu");
            }
            if (Input.GetKey(KeyCode.R) && paused == true)
            {
                Resume();
                paused = false;
                //SceneManager.LoadScene(sceneName:"MainLevel");
            }
    }
        if (Input.GetKey(KeyCode.F))
        {
            SceneManager.LoadScene(sceneName: "MainLevel");
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
            water = 5;
            if (threeFourths)
            {
                water = 3;
            }
            
        }
        else
        {
            water = 10;
        }
        if (Time.time > waterSoundTimerNextUse)
        {
            updateWaterTextAndSubtract(false);
            waterSoundTimerNextUse = Time.time + waterSoundTimer;
            audioSource.PlayOneShot(pickupWaterSound, 0.4f);
        }
        
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
                    audioSource.PlayOneShot(waterSound,0.5f);
                    water--;
                    waterCooldownNextUse = Time.time + waterCoolDown;
                    targetCrop.hasWater = true;
                    targetCrop.waterCrop();
                    
                }
                
            }
            else
            {
                //alertBoxText.text = "Water Cooldown";
            }
        }
        //string temp = "Water: " + water.ToString();
        //alertBoxText.text = temp;
        //tempWaterPlaceholder.text = "Water: " + water.ToString();
        if (water >= 10)
        {
            inventorySlotWaterSprite.GetComponent<Image>().sprite = waterFull;
        }
        if (water <= 6)
        {
            inventorySlotWaterSprite.GetComponent<Image>().sprite = waterTwoThirds;
        }
        if (water <= 3)
        {
            inventorySlotWaterSprite.GetComponent<Image>().sprite = waterOneThird;
        }
        if (water == 0)
        {
            inventorySlotWaterSprite.GetComponent<Image>().sprite = waterEmpty;
        }
    }

    private void updateInventoryText()
    {
        //update Slot 1
        if (inventory[0] == 0)
        {
            inventorySlotOneSprite.GetComponent<Image>().sprite = EmptySlot;
            //tempInventorySlot1.text = "Empty";
        }
        else if (inventory[0] == 1)
        {
            inventorySlotOneSprite.GetComponent<Image>().sprite = seed;
            //tempInventorySlot1.text = "Seed";
        }
        else
            inventorySlotOneSprite.GetComponent<Image>().sprite = carrot;
        {
            //tempInventorySlot1.text = "Crop";
        }

        if (inventory[1] == 0)
        {
            inventorySlotTwoSprite.GetComponent<Image>().sprite = EmptySlot;
            //tempInventorySlot2.text = "Empty";
        }
        else if (inventory[1] == 1)
        {
            inventorySlotTwoSprite.GetComponent<Image>().sprite = seed;
            //tempInventorySlot2.text = "Seed";
        }
        else
        {
            inventorySlotTwoSprite.GetComponent<Image>().sprite = carrot;
            //tempInventorySlot2.text = "Crop";
        }

        //update Slot 2
        if (inventory[2] == 0)
        {
            inventorySlotThreeSprite.GetComponent<Image>().sprite = EmptySlot;
            //tempInventorySlot3.text = "Empty";
        }
        else if (inventory[2] == 1)
        {
            inventorySlotThreeSprite.GetComponent<Image>().sprite = seed;
            //tempInventorySlot3.text = "Seed";
        }
        else
        {
            inventorySlotThreeSprite.GetComponent<Image>().sprite = carrot;
            //tempInventorySlot3.text = "Crop";
        }

        //update Slot 3
        if (inventory[3] == 0)
        {
            inventorySlotFourSprite.GetComponent<Image>().sprite = EmptySlot;
            //tempInventorySlot4.text = "Empty";
        }
        else if (inventory[3] == 1)
        {
            inventorySlotFourSprite.GetComponent<Image>().sprite = seed;
            //tempInventorySlot4.text = "Seed";
        }
        else
        {
            inventorySlotFourSprite.GetComponent<Image>().sprite = carrot;
            //tempInventorySlot4.text = "Crop";
        }

        //update Slot 4
        if (inventory[4] == 0)
        {
            inventorySlotFiveSprite.GetComponent<Image>().sprite = EmptySlot;
            //tempInventorySlot5.text = "Empty";
        }
        else if (inventory[4] == 1)
        {
            inventorySlotFiveSprite.GetComponent<Image>().sprite = seed;
            //tempInventorySlot5.text = "Seed";
        }
        else
        {
            inventorySlotFiveSprite.GetComponent<Image>().sprite = carrot;
            //tempInventorySlot5.text = "Crop";
        }

    }

    private void getSeedFromBox()
    {
        if (Time.time > seedCooldownNextUse)
        {
            seedCooldownNextUse = Time.time + seedCoolDown;
            for (int i = 0; i < inventory.Length; i++)
            {
                audioSource.PlayOneShot(itemPickupSound, 0.4f);
                if (inventory[i] == 0)
                {
                    inventory[i] = 1;
                    break;
                }
            }
        }
        else
        {
            //alertBoxText.text = "Seed Cooldown";
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
            audioSource.PlayOneShot(cropHarvestSound, 0.6f);
            targetCrop.hasSeed = true;
        }
        
    }

    private void grabCrop()
    {
        for (int i = 0; i < inventory.Length; i++)
        {
            if (inventory[i] == 0)
            {
                inventory[i] = 2;
                break;
            }
        }
        audioSource.PlayOneShot(cropHarvestSound, 0.8f);
        targetCrop.growthStage = 0;
        updateInventoryText();
        targetCrop.pickedCrop();
    }

    private void sellCrops()
    {
        int count2 = 0;
        for (int i = 0; i < inventory.Length; i++)
        {
            if (inventory[i] == 2)
            {
                inventory[i] = 0;
                count2++;
            }
        }
        coins += count2;
        score += count2 * 100;
        updateCoins();
        updateScore();
        updateInventoryText();
        if (Time.time > sellSoundTimerNextUse)
        {
            sellSoundTimerNextUse = Time.time + sellSoundTimer;
            audioSource.PlayOneShot(sellSound);
        }
    }

    private void updateCoins()
    {
        string temp = "Cash: " + coins;
        coinBox.text = temp;
    }

    private void updateScore()
    {
        string temp = "Score: " + score;
        scoreBox.text = temp;
    }

    private void investEco()
    {
        int temp;
        temp = coins;
        coins = 0;
        if (temp * 5f < 180)
        {
            TimeLeft += temp * 5f;
            visualTimer -= temp * 5f;
        }
        else
        {
            TimeLeft = 180;
            visualTimer = 0;
        }
        updateCoins();
        if (Time.time > investInEcoSoundTimerNextUse)
        {
            investInEcoSoundTimerNextUse = Time.time + investInEcoSoundTimer;
            audioSource.PlayOneShot(sellSound);
        }
       
    }

    private void investScore()
    {
        int temp2;
        temp2 = coins;
        coins = 0;
        score += temp2 * 100;
        Debug.Log(temp2); ;
        updateScore();
        updateCoins();
        
        if (Time.time > investInScoreSoundTimerNextUse)
        {
            investInScoreSoundTimerNextUse = Time.time + investInScoreSoundTimer;
            audioSource.PlayOneShot(sellSound);
        }
    }

    //Code refrenced in Timer Video Credit in variables
    //Modified with slider
    private void UpdateTimer(float current,float currentInverse)
    {
        //currentInverse += 1;
        current += 1;
        float minuetes = Mathf.FloorToInt(current / 60);
        float seconds = Mathf.FloorToInt(current % 60);
        //TimerText.text = string.Format("{0:00} : {1:00}",minuetes,seconds);
        temperatureSlider.value = currentInverse / 180;
        //Debug.Log(currentInverse/180);
    }



    public void Pause()
    {
        //pauseMenu.SetActive(true);
        Time.timeScale = 0f;
    }

    public void Resume()
    {
        //pauseMenu.SetActive(false);
        Time.timeScale = 1f;
    }

}
