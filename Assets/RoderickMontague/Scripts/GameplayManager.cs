using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameplayManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.touchCount != 0)
        {
            Touch touch = Input.GetTouch(0);

            Debug.Log("Finger has touched screen. Tap Count: " + touch.tapCount);

            // // checks to see if the user has touched it.
            // if (touch.phase == TouchPhase.Began)
            // {
            //     // Debug.Log("Finger has touched screen.");
            // }
        }
        
    }
}
