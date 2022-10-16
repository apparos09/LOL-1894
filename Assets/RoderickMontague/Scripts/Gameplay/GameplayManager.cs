using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameplayManager : MonoBehaviour
{
    // the state of the game.
    public enum gameState { none, overworld, battle }

    // the state of the game.
    public gameState state;

    // the manager for the overworld.
    public OverworldManager overworld;

    // the manager for the battle.
    public BattleManager battle;


    // Start is called before the first frame update
    void Start()
    {
    }

    // // Called when a level has been loaded.
    // private void OnLevelWasLoaded(int level)
    // {
    // }

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


        // Checks the state variable to see what kind of scene the game is in.
        switch(state)
        {
            case gameState.overworld:
                break;

            case gameState.battle:
                break;
        }
        
    }
}
