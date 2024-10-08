using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class NewTestMovement : MonoBehaviour
{
    private bool xlimit = false;
    private bool ylimit = false;

    // Use this attribute instead of making a variable public.
    // It will expose the value as a field in the Unity Editor

    [SerializeField]
    private float speedX = 1f;

    void Update()
    {
        float movementX = Input.GetAxisRaw("Horizontal");
        //Debug.Log(movementX);
        float movementY = Input.GetAxisRaw("Vertical");
        //Debug.Log(movementY);
        // This way of changing the position of a game object is quick but dirty.
        // If you use rigidbodies for your game objects, you should do it differently


        //Set Y Limit
        if(Input.GetKeyDown(KeyCode.A)|| Input.GetKeyDown(KeyCode.D))
        {
           ylimit = true;
           Debug.Log("activated ylimit"+ylimit);
        }
        if (Input.GetKeyUp(KeyCode.A) || Input.GetKeyUp(KeyCode.D))
        {
            ylimit = false;
            Debug.Log("Deactivated ylimit" + ylimit);
        }

        //Set X Limit
        if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.S))
        {
            Debug.Log("activated xlimit"+xlimit);
            xlimit = true;
        }
        if (Input.GetKeyUp(KeyCode.W) || Input.GetKeyUp(KeyCode.S))
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
}
