using LoLSDK;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Init_LOL1894 : MonoBehaviour
{
    // Awake is called when the script is being loaded
    private void Awake()
    {
        // the sdk for the LoL component.
        ILOLSDK sdk;

        // checks the platform the editor is running in.
#if UNITY_EDITOR
        sdk = new LoLSDK.MockWebGL();
#elif UNITY_WEBGL
        sdk = new LoLSDK.WebGL();

#endif

        LOLSDK.Init(sdk, "lol_1894.battle-bot-training-sim");
    }

    // Start is called before the first frame update
    void Start()
    {
        SceneManager.LoadScene("TitleScene");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
