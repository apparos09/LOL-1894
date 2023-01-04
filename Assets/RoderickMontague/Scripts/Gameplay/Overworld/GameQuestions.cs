using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RM_BBTS
{
    // A question to be posed in the overworld.
    public struct GameQuestion
    {
        // The question number (unused).
        public int number;

        // The question.
        public string question;

        // The responses for the question.
        public string[] responses;

        // The index of the correct response.
        public int answerIndex;

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

        // Gets the answer to the question.
        public string GetAnswer()
        {
            if (answerIndex >= 0 && answerIndex < responses.Length)
                return responses[answerIndex];
            else
                return string.Empty;
        }

        // Checks of the index provided matches the answer index.
        public bool CorrectAnswer(int index)
        {
            return index == answerIndex;
        }

        // Checks of the string provided matches the answer string.
        public bool CorrectAnswer(string response)
        {
            return response == GetAnswer();
        }
    }

    // The list of the overworld questions.
    public class GameQuestions : MonoBehaviour
    {
        // The amount of overworld questions.
        public const int QUESTION_COUNT = 6;

        // The maximum amount of options for a question.
        public const int QUESTION_OPTIONS_MAX = 4;

        // the instance of the overworld questions.
        private static GameQuestions instance;

        // Constructor
        private GameQuestions()
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
        public static GameQuestions Instance
        {
            get
            {
                // Checks to see if the instance exists. If it doesn't, generate an object.
                if (instance == null)
                {
                    instance = FindObjectOfType<GameQuestions>(true);

                    // Generate new instance if an existing instance was not found.
                    if (instance == null)
                    {
                        // Makes a new settings object.
                        GameObject go = new GameObject("(singleton) Overworld Questions");

                        // Adds the instance component to the new object.
                        instance = go.AddComponent<GameQuestions>();
                    }

                }

                // Returns the instance.
                return instance;
            }
        }

        // Gets a question, starting from 0.
        public GameQuestion GetQuestion(int number)
        {
            // The queston to be returned.
            GameQuestion question = new GameQuestion();

            // The number of the question.
            question.number = number;

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
                    // Question
                    question.question = "[Is this a test?]";

                    // Responses
                    question.Response0 = "[Yes]";
                    question.Response1 = "[No]";
                    question.Response2 = "";
                    question.Response3 = "";

                    question.answerIndex = 0;

                    break;
                case 1:
                    // Question
                    question.question = "[Is this a test?]";

                    // Responses
                    question.Response0 = "[Yes]";
                    question.Response1 = "[No]";
                    question.Response2 = "";
                    question.Response3 = "";

                    question.answerIndex = 0;

                    break;
                case 2:
                    // Question
                    question.question = "[Is this a test?]";

                    // Responses
                    question.Response0 = "[Yes]";
                    question.Response1 = "[No]";
                    question.Response2 = "";
                    question.Response3 = "";

                    question.answerIndex = 0;

                    break;
                case 3:
                    // Question
                    question.question = "[Is this a test?]";

                    // Responses
                    question.Response0 = "[Yes]";
                    question.Response1 = "[No]";
                    question.Response2 = "";
                    question.Response3 = "";

                    question.answerIndex = 0;

                    break;
                case 4:
                    // Question
                    question.question = "[Is this a test?]";

                    // Responses
                    question.Response0 = "[Yes]";
                    question.Response1 = "[No]";
                    question.Response2 = "";
                    question.Response3 = "";

                    question.answerIndex = 0;

                    break;
                case 5:
                    // Question
                    question.question = "[Is this a test?]";

                    // Responses
                    question.Response0 = "[Yes]";
                    question.Response1 = "[No]";
                    question.Response2 = "";
                    question.Response3 = "";

                    question.answerIndex = 0;

                    break;
            }

            // Returns the question.
            return question;
        }

        // Gets a random question from the list.
        public GameQuestion GetRandomQuestion(bool randomResponseOrder = false)
        {
            // Gets the question (question 0 sould not be used).
            GameQuestion question = GetQuestion(Random.Range(1, QUESTION_COUNT));

            // Randomizes the response order.
            if (randomResponseOrder)
                RandomizeResponseOrder(ref question);

            // Returns result.
            return question;
        }

        // Randomizes the order of the responses in the question.
        public void RandomizeResponseOrder(ref GameQuestion question)
        {
            // Gets the response list.
            List<string> responseList = new List<string>(question.responses);
            string answer = question.GetAnswer();

            // The new index for the question array.
            int newIndex = 0;

            // While there are entries in the list.
            while (responseList.Count != 0)
            {
                // Gets the randomization index.
                int randIndex = Random.Range(0, responseList.Count);

                // Moves the response, removes it from the list, and moves onto the next index.
                question.responses[newIndex] = responseList[randIndex]; 
                responseList.RemoveAt(randIndex);
                newIndex++;
            }

            // Gets the index of the answer and replaces the answer index.
            question.answerIndex = System.Array.IndexOf(question.responses, answer);
        }
    }
}