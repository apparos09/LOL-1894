using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
        public int correctIndex;

        // Returns the response by the index.
        public string GetResponseByIndex(int index)
        {
            // Returns an empty string.
            if (responses == null)
                return string.Empty;

            // Returns the string by index.
            if (index >= 0 && index < responses.Length)
                return responses[index];
            else // Empty string.
                return string.Empty;
        }

        // Set the response by the index.
        public void SetResponseByIndex(int index, string response)
        {
            // Returns the string by index.
            if (index >= 0 && index < responses.Length)
                responses[index] = response;
        }

        // The first response/index 0 entry.
        public string Response0
        {
            get { return GetResponseByIndex(0); }

            set 
            {
                SetResponseByIndex(0, value);
            }
        }

        // The second response/index 1 entry.
        public string Response1
        {
            get { return GetResponseByIndex(1); }

            set
            {
                SetResponseByIndex(1, value);
            }
        }

        // The third response/index 2 entry.
        public string Response2
        {
            get { return GetResponseByIndex(2); }

            set
            {
                SetResponseByIndex(2, value);
            }
        }

        // The forth response/index 3 entry.
        public string Response3
        {
            get { return GetResponseByIndex(3); }

            set
            {
                SetResponseByIndex(3, value);
            }
        }

    }

    // The list of the overworld questions.
    public class OverworldQuestions : MonoBehaviour
    {
        // The amount of overworld questions.
        public const int QUESTION_COUNT = 5;

        // The maximum amount of options for a question.
        public const int QUESTION_OPTIONS_MAX = 4;

        // the instance of the overworld questions.
        private static OverworldQuestions instance;

        // Constructor
        private OverworldQuestions()
        {
        }

        // Awake is called when a script instance is being loaded.
        private void Awake()
        {
            // Checks for the instance.
            if (instance == null)
            {
                instance = this;
            }
        }

        // Returns the instance of the class.
        public static OverworldQuestions Instance
        {
            get
            {
                // Checks to see if the instance exists. If it doesn't, generate an object.
                if (instance == null)
                {
                    instance = FindObjectOfType<OverworldQuestions>(true);

                    // Generate new instance if an existing instance was not found.
                    if (instance == null)
                    {
                        // Makes a new settings object.
                        GameObject go = new GameObject("(singleton) Overworld Questions");

                        // Adds the instance component to the new object.
                        instance = go.AddComponent<OverworldQuestions>();
                    }

                }

                // Returns the instance.
                return instance;
            }
        }

        // Gets a question, starting from 0.
        public OverworldQuestion GetQuestion(int number)
        {
            // The queston to be returned.
            OverworldQuestion question = new OverworldQuestion();

            // Creates the list of responses.
            question.responses = new string[QUESTION_OPTIONS_MAX] 
            {
                string.Empty, string.Empty, string.Empty, string.Empty
            };

            // Checks the question number.
            switch(number)
            {
                case 0:
                default:
                    question.question = "This is a test.";

                    question.Response0 = "A";
                    question.Response1 = "";
                    question.Response2 = "";
                    question.Response3 = "";

                    break;
                case 1:
                    break;
                case 2:
                    break;
                case 3:
                    break;
                case 4:
                    break;
                case 5:
                    break;
            }

            // Returns the question.
            return question;
        }

        // Gets a random question from the list.
        public OverworldQuestion GetRandomQuestion()
        {
            return GetQuestion(Random.Range(1, QUESTION_COUNT));
        }
    }
}