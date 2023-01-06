using SimpleJSON;
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

        // The speak key for the question.
        public string questionSpeakKey;

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
        // the instance of the overworld questions.
        private static GameQuestions instance;

        // The amount of overworld questions.
        public const int QUESTION_COUNT = 6;

        // The maximum amount of options for a question.
        public const int QUESTION_OPTIONS_MAX = 4;

        

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
            // Finds the language defs.
            JSONNode defs = SharedState.LanguageDefs;

            // The queston to be returned.
            GameQuestion question = new GameQuestion();

            // The number of the question.
            question.number = number;

            // Creates the list of responses.
            question.responses = new string[QUESTION_OPTIONS_MAX] 
            {
                string.Empty, string.Empty, string.Empty, string.Empty
            };

            // Sets the speak key to be empty.
            // TODO: add speak key for questions.
            question.questionSpeakKey = string.Empty;

            // Checks the question number.
            switch (number)
            {
                case 0:
                default:
                    // Question
                    question.question = "Is this a test?";

                    // Responses
                    question.Response0 = "Yes";
                    question.Response1 = "No";
                    question.Response2 = "";
                    question.Response3 = "";

                    question.answerIndex = 0;

                    break;
                case 1:
                    // Question
                    question.question = "[When two battlers move have the same speed, the turn order is evenly random. If the player has the same speed as the opponent, what is the chance that the player goes first?]";

                    // Responses
                    question.Response0 = "0.25";
                    question.Response1 = "0.50";
                    question.Response2 = "0.75";
                    question.Response3 = "1.00";

                    question.answerIndex = 1;

                    break;
                case 2:
                    // Question
                    question.question = "[If a move does not mention the ability to burn the opponent, what is the burn chance of said move?]";

                    // Responses
                    question.Response0 = "0.00";
                    question.Response1 = "0.25";
                    question.Response2 = "0.50";
                    question.Response3 = "0.75";

                    question.answerIndex = 0;

                    break;
                case 3:
                    // Question
                    question.question = "[If a move does not mention the ability to paralyze the opponent, what is the paralysis chance of said move?]";

                    // Responses
                    question.Response0 = "0.75";
                    question.Response1 = "0.50";
                    question.Response2 = "0.25";
                    question.Response3 = "0.00";

                    question.answerIndex = 3;

                    break;
                case 4:
                    // Question
                    question.question = "[Move A has a power of 40 and an accuracy of 1.00. Move B has a power 60 and an accuracy of 0.90. What is true about Move A and Move B.]";

                    // Responses
                    question.Response0 = "[Move A is stronger than Move B, but it less accurate.]";
                    question.Response1 = "[Move A is stronger than Move B, and it is more accurate.]";
                    question.Response2 = "[Move A is weaker than Move B, and it is less accurate.]";
                    question.Response3 = "[Move A is weaker than Move B, but it is more accurate.]";

                    question.answerIndex = 3;

                    break;
                
                case 5:
                    // Question
                    question.question = "[The opponent has 65% of their health, and 3 of their 4 standard moves can restore their health. Assuming the opponent will not charge their energy, and that each move has an equal chance of being chosen, what is the chance that the opponent will use a healing move?]";

                    // Responses
                    question.Response0 = "0.20";
                    question.Response1 = "0.60";
                    question.Response2 = "0.75";
                    question.Response3 = "1.00";

                    question.answerIndex = 2;

                    break;

                case 6:
                    // Question
                    question.question = "[The player uses a move that raises their accuracy by 5%. Which of the following statements is true?]";

                    // Responses
                    question.Response0 = "The player will move faster than they did before.";
                    question.Response1 = "The player’s attacks will do more damage.";
                    question.Response2 = "The player’s moves are more likely to hit their target.";
                    question.Response3 = "The opponent’s moves will do less damage to the player.";

                    question.answerIndex = 2;

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
                question = RandomizeResponseOrder(question);

            // Returns result.
            return question;
        }

        // Randomizes the order of the responses in the question.
        public GameQuestion RandomizeResponseOrder(GameQuestion question)
        {
            // Gets the response list.
            List<string> responseList = new List<string>(question.responses);

            // Grabs the answer string.
            string answer = question.GetAnswer();

            // The new index for the question array.
            int newIndex = 0;

            // While there are entries in the list.
            while (responseList.Count != 0 && newIndex < question.responses.Length)
            {
                // Gets the randomization index.
                int randIndex = Random.Range(0, responseList.Count);

                // Moves the response, removes it from the list, and moves onto the next index.
                question.responses[newIndex] = responseList[randIndex]; 
                responseList.RemoveAt(randIndex);
                newIndex++;
            }

            // Make sure the empty string responses are all at the back of the array.
            {
                // Copies the new question order into the original response list object.
                responseList = new List<string>(question.responses);

                // Removes empty strings from the response list.
                while (responseList.Contains(string.Empty))
                    responseList.Remove(string.Empty);

                // While the local response list is not equal to the question's response count...
                // Add empty strings so that the counts both match.
                while (responseList.Count < question.responses.Length)
                    responseList.Add(string.Empty);

                // Copies the response list content into the question response array.
                for (int i = 0; i < question.responses.Length && i < responseList.Count; i++)
                    question.responses[i] = responseList[i];
            }

            // Makes sure the answer index is set properly with the list changes.
            for (int i = 0; i < question.responses.Length; i++)
            {
                // If the new answer index has been found, repalce the answer index saved to the response.
                // Also, break the loop since the rest of the spots don't need to be checked.
                if (question.responses[i] == answer)
                {
                    question.answerIndex = i;
                    break;
                }
            }

            return question;
        }
    }
}