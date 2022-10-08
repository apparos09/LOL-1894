using LoLSDK;
using SimpleJSON;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

// the manager for the title scene.
public class TitleManager : MonoBehaviour
{
    [Header("Buttons")]
    public TMP_Text viewControlsButtonText;
    public TMP_Text viewOptionsButtonText;

    // Awake is called when the script instance is loaded.
    private void Awake()
    {
        // set to 30 FPS
        Application.targetFrameRate = 30;

        // checks if LOL SDK has been initialized.
        bool initialized = LOLSDK.Instance.IsInitialized;

        // if the LOL SDK has been initialized...
        if(initialized)
        {
            JSONNode defs = SharedState.LanguageDefs;

            viewControlsButtonText.text = defs["viewControls"];
            viewOptionsButtonText.text = defs["viewOptions"];
        }
        else
        {
            Debug.LogError("LOL SDK NOT INITIALIZED.");
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
