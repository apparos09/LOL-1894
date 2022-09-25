using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LoLSDK;
using SimpleJSON;
using System.IO;


// The namespace should be <YourCompany> <GameName>.
namespace RoderickMontague_LOL_1894
{
    public class TextToSpeech : MonoBehaviour
    {
        private void Awake()
        {
#if UNITY_EDITOR
            ILOLSDK webGL = new LoLSDK.MockWebGL();
#elif UNITY_WEBGL
            ILOLSDK webGL = new LoLSDK.WebGL();
#endif

            LOLSDK.Init(webGL, "understand_probability_game");
        }

        // Start is called before the first frame update
        void Start()
        {

            // if(Application.platform == RuntimePlatform.WebGLPlayer)
            // {
            //     // only do if initializing for the first time.
            //     // ILOLSDK webGL = new LoLSDK.MockWebGL();
            //     // 
            //     // LOLSDK.Init(webGL, "understand_probability_game");
            // 
            //     ReadLine("This is a test");
            // }

            ReadLine("This is a test");
        }

        // reads the provided line
        public void ReadLine(string line)
        {
            LOLSDK.Instance.SpeakText(line);
        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}
