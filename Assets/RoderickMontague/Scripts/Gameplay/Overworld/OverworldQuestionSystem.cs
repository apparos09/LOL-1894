using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Namespace.
namespace RM_BBTS
{
    // A question to be posed in the overworld.
    public struct OverworldQuestion
    {
        // The question.
        public string question;
        
        // The responses for the question.
        public string[] responses;

        // The index of the correct response.
        public string correctIndex;
    }

    // The system for posing questions in the overworld.
    public class OverworldQuestionSystem : MonoBehaviour
    {
        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}