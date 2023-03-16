using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LoLSDK;
using SimpleJSON;

namespace RM_BBTS
{
    // A question to be posed in the overworld.
    public struct GameQuestion
    {
        // The question number (used to remember questions).
        // Negative numbers are considered empty slots, and are not perserved when saving and loading the game.
        public int number;

        // The question.
        public string question;

        // The speak key for the question.
        public string questionSpeakKey;

        // The responses for the question.
        public string[] responses;

        // Taken out because they can't consistently be read (numbers aren't in the language file).
        // The speak keys for the responses.
        // public string[] responseSpeakKeys;

        // The index of the correct response.
        public int answerIndex;

        // This message is used if the user gets the question right.
        public string correctAnswerResponse;

        // The speak key for the correct answer message.
        public string correctAnswerSpeakKey;

        // This message is used if the user gets the question wrong.
        public string incorrectAnswerResponse;

        // The speak key for the incorrect answer message.
        public string incorrectAnswerSpeakKey;


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
            // Sets the string by index.
            if (index >= 0 && index < responses.Length)
                responses[index] = response;
        }

        // // Returns the response speak key by the index.
        // public string GetResponseSpeakKeyByIndex(int index)
        // {
        //     // Returns an empty string.
        //     if (responseSpeakKeys == null)
        //         return string.Empty;
        // 
        //     // Returns the string by index.
        //     if (index >= 0 && index < responseSpeakKeys.Length)
        //         return responseSpeakKeys[index];
        //     else // Empty string.
        //         return string.Empty;
        // }
        // 
        // // Set the response speak key by the index.
        // public void SetResponseSpeakKeyByIndex(int index, string speakKey)
        // {
        //     // Sets the string by index.
        //     if (index >= 0 && index < responseSpeakKeys.Length)
        //         responseSpeakKeys[index] = speakKey;
        // }


        // RESPONSES 
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


        // // SPEAK KEYS
        // // The first response speak key/index 0 entry.
        // public string Response0SpeakKey
        // {
        //     get { return GetResponseSpeakKeyByIndex(0); }
        // 
        //     set
        //     {
        //         SetResponseSpeakKeyByIndex(0, value);
        //     }
        // }
        // 
        // // The second response speak key/index 1 entry.
        // public string Response1SpeakKey
        // {
        //     get { return GetResponseSpeakKeyByIndex(1); }
        // 
        //     set
        //     {
        //         SetResponseSpeakKeyByIndex(1, value);
        //     }
        // }
        // 
        // // The third response speak key/index 2 entry.
        // public string Response2SpeakKey
        // {
        //     get { return GetResponseSpeakKeyByIndex(2); }
        // 
        //     set
        //     {
        //         SetResponseSpeakKeyByIndex(2, value);
        //     }
        // }
        // 
        // // The forth response speak key/index 3 entry.
        // public string Response3SpeakKey
        // {
        //     get { return GetResponseSpeakKeyByIndex(3); }
        // 
        //     set
        //     {
        //         SetResponseSpeakKeyByIndex(3, value);
        //     }
        // }


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
        public const int QUESTION_COUNT = 26;

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

            // If the text should be translated.
            bool translate = LOLSDK.Instance.IsInitialized && defs != null;

            // The queston to be returned.
            GameQuestion question = new GameQuestion();

            // The number of the question.
            question.number = number;

            // Sets the speak key to be empty.
            question.questionSpeakKey = string.Empty;

            // Creates the list of responses.
            question.responses = new string[QUESTION_OPTIONS_MAX] 
            {
                string.Empty, string.Empty, string.Empty, string.Empty
            };

            // // Creates the list of response speak keys.
            // question.responseSpeakKeys = new string[QUESTION_OPTIONS_MAX]
            // {
            //     string.Empty, string.Empty, string.Empty, string.Empty
            // };

            // Not putting brackets around these since I'm low on time and I'd have to take it out later.
            question.correctAnswerResponse = string.Empty;
            question.correctAnswerSpeakKey = string.Empty;
            question.incorrectAnswerResponse = string.Empty;
            question.incorrectAnswerSpeakKey = string.Empty;

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

                    // Answer
                    question.answerIndex = 0;
                    question.correctAnswerResponse = "This is indeed a test.";
                    question.incorrectAnswerResponse = "This is very likely a test.";

                    // // Translates the question.
                    // if (translate)
                    // {
                    //     question.question = defs["que00"];
                    //     question.Response1 = defs["que00_res00"];
                    //     question.Response0 = defs["que00_res01"];
                    //     question.Response2 = defs["que00_res02"];
                    //     question.Response3 = defs["que00_res03"];
                    // }

                    break;

                case 1:
                    // Question
                    question.question = "When 2 battlers have the same speed, the turn order is perfectly random. If you have the same speed as your opponent, what is the chance of you going first?";
                    question.questionSpeakKey = "que01";

                    // Responses
                    question.Response0 = "0.25";
                    question.Response1 = "0.50";
                    question.Response2 = "0.75";
                    question.Response3 = "1.00";

                    // Answer
                    question.answerIndex = 1;
                    question.correctAnswerResponse = "Since there are only 2 outcomes, each event has a 0.50 chance.";
                    question.incorrectAnswerResponse = "When something is perfectly random, all outcomes have an equal chance of occurring.";

                    // Translates the question.
                    if (translate)
                    {
                        question.question = defs["que01"];

                        question.correctAnswerResponse = defs["que01_correct"];
                        question.correctAnswerSpeakKey = "que01_correct";
                        question.incorrectAnswerResponse = defs["que01_incorrect"];
                        question.incorrectAnswerSpeakKey = "que01_incorrect";
                    }

                    break;
                case 2:
                    // Question
                    question.question = "If a move does not mention that it can burn the target, the burn chance is 0.00. What does this mean?";
                    question.questionSpeakKey = "que02";

                    // Responses
                    question.Response0 = "The move cannot inflict burn status on the target.";
                    question.Response1 = "The move always inflicts burn status on the target.";
                    question.Response2 = "The move might inflict burn status on the target.";
                    question.Response3 = "The target is immune to being burned.";

                    // Answer
                    question.answerIndex = 0;
                    question.correctAnswerResponse = "The burn chance is 0.00, meaning the move cannot burn the target.";
                    question.incorrectAnswerResponse = "In a 0-1 scale, 0 means the event never happens, and 1 means the event always happens.";

                    // Translates the question.
                    if (translate)
                    {
                        question.question = defs["que02"];
                        question.Response0 = defs["que02_res00"];
                        question.Response1 = defs["que02_res01"];
                        question.Response2 = defs["que02_res02"];
                        question.Response3 = defs["que02_res03"];

                        question.correctAnswerResponse = defs["que02_correct"];
                        question.correctAnswerSpeakKey = "que02_correct";
                        question.incorrectAnswerResponse = defs["que02_incorrect"];
                        question.incorrectAnswerSpeakKey = "que02_incorrect";
                    }

                    break;
                case 3:
                    // Question
                    question.question = "If a move mentions that it can paralyze the target, its paralysis chance is greater than 0.00. If a move's paralysis chance is 1.00, what does this mean?";
                    question.questionSpeakKey = "que03";

                    // Responses
                    question.Response0 = "The target is immune to paralysis status.";
                    question.Response1 = "The move will never inflict paralysis status.";
                    question.Response2 = "The move will inflict paralysis status half of the time.";
                    question.Response3 = "The move will always inflict paralysis status.";

                    // Answer
                    question.answerIndex = 3;
                    question.correctAnswerResponse = "Since the paralysis chance is 1.00, the event is guaranteed to happen.";
                    question.incorrectAnswerResponse = "In a 0-1 scale, 0 means the event never happens, and 1 means the event always happens.";


                    // Translates the question.
                    if (translate)
                    {
                        question.question = defs["que03"];
                        question.Response0 = defs["que03_res00"];
                        question.Response1 = defs["que03_res01"];
                        question.Response2 = defs["que03_res02"];
                        question.Response3 = defs["que03_res03"];

                        question.correctAnswerResponse = defs["que03_correct"];
                        question.correctAnswerSpeakKey = "que03_correct";
                        question.incorrectAnswerResponse = defs["que03_incorrect"];
                        question.incorrectAnswerSpeakKey = "que03_incorrect";
                    }
                    break;

                case 4:
                    // Question
                    question.question = "Move A has a power of 40 and an accuracy of 1.00. Move B has a power of 60 and an accuracy of 0.90. What is true about Move A and Move B?";
                    question.questionSpeakKey = "que04";

                    // Responses
                    question.Response0 = "Move A is stronger than Move B, but it is less accurate.";
                    question.Response1 = "Move A is stronger than Move B, and it is more accurate.";
                    question.Response2 = "Move A is weaker than Move B, and it is less accurate.";
                    question.Response3 = "Move A is weaker than Move B, but it is more accurate.";

                    // Answer
                    question.answerIndex = 3;
                    question.correctAnswerResponse = "Move A always hits, but Move B hits harder.";
                    question.incorrectAnswerResponse = "Strong moves usually have a trade off. In this case, power is traded for accuracy.";

                    // Translates the question.
                    if (translate)
                    {
                        question.question = defs["que04"];
                        question.Response0 = defs["que04_res00"];
                        question.Response1 = defs["que04_res01"];
                        question.Response2 = defs["que04_res02"];
                        question.Response3 = defs["que04_res03"];

                        question.correctAnswerResponse = defs["que04_correct"];
                        question.correctAnswerSpeakKey = "que04_correct";
                        question.incorrectAnswerResponse = defs["que04_incorrect"];
                        question.incorrectAnswerSpeakKey = "que04_incorrect";
                    }
                    break;
                
                
                
                case 5:
                    // Question
                    question.question = "Your opponent has 65% of their health, and 3 of their 4 battle moves can restore their health. Assuming your opponent will not charge their energy, and that each move has an equal chance of being chosen, what is the chance that your foe will heal themselves?";
                    question.questionSpeakKey = "que05";

                    // Responses
                    question.Response0 = "1/4";
                    question.Response1 = "2/4";
                    question.Response2 = "3/4";
                    question.Response3 = "4/4";

                    // Answer
                    question.answerIndex = 2;
                    question.correctAnswerResponse = "Since 3/4 moves can heal the user, your foe has a 75% chance of healing themselves.";
                    question.incorrectAnswerResponse = "Since your foe won't charge their energy, they only have 4 moves to choose from.";

                    // Translates the question.
                    if (translate)
                    {
                        question.question = defs["que05"];

                        question.correctAnswerResponse = defs["que05_correct"];
                        question.correctAnswerSpeakKey = "que05_correct";
                        question.incorrectAnswerResponse = defs["que05_incorrect"];
                        question.incorrectAnswerSpeakKey = "que05_incorrect";
                    }

                    break;

                case 6:
                    // Question
                    question.question = "If you used a move that increases your accuracy by 2 stages, which of the following statements would be true?";
                    question.questionSpeakKey = "que06";

                    // Responses
                    question.Response0 = "You will move faster than before.";
                    question.Response1 = "Your attacks will do more damage.";
                    question.Response2 = "Your moves are now more likely to hit their target.";
                    question.Response3 = "You will now take less damage from your opponent's attacks.";

                    // Answer
                    question.answerIndex = 2;
                    question.correctAnswerResponse = "Since your accuracy has gone up, your moves are more likely to hit their target.";
                    question.incorrectAnswerResponse = "Accuracy determines how likely it is for a move to hit its target.";

                    // Translates the question.
                    if (translate)
                    {
                        question.question = defs["que06"];
                        question.Response0 = defs["que06_res00"];
                        question.Response1 = defs["que06_res01"];
                        question.Response2 = defs["que06_res02"];
                        question.Response3 = defs["que06_res03"];

                        question.correctAnswerResponse = defs["que06_correct"];
                        question.correctAnswerSpeakKey = "que06_correct";
                        question.incorrectAnswerResponse = defs["que06_incorrect"];
                        question.incorrectAnswerSpeakKey = "que06_incorrect";
                    }
                    break;

                case 7:
                    // Question
                    question.question = "If you were hit by a move that lowers your accuracy by 1 stage, which of the following statements would be true?";
                    question.questionSpeakKey = "que07";

                    // Responses
                    question.Response0 = "Your moves are more likely to miss their target.";
                    question.Response1 = "You will move slower than you did before.";
                    question.Response2 = "You will now take more damage from your opponent's attacks.";
                    question.Response3 = "Your attacks will do less damage than before.";

                    // Answer
                    question.answerIndex = 0;
                    question.correctAnswerResponse = "Since your accuracy has gone down, your moves are more likely to miss.";
                    question.incorrectAnswerResponse = "Accuracy determines how likely it is for a move to hit its target.";

                    // Translates the question.
                    if (translate)
                    {
                        question.question = defs["que07"];
                        question.Response0 = defs["que07_res00"];
                        question.Response1 = defs["que07_res01"];
                        question.Response2 = defs["que07_res02"];
                        question.Response3 = defs["que07_res03"];

                        question.correctAnswerResponse = defs["que07_correct"];
                        question.correctAnswerSpeakKey = "que07_correct";
                        question.incorrectAnswerResponse = defs["que07_incorrect"];
                        question.incorrectAnswerSpeakKey = "que07_incorrect";
                    }
                    break;

                case 8:
                    // Question
                    question.question = "Your opponent has 20% of their energy left. Along with the charge move, your opponent has 1 other move that they can use. Assuming each move has an equal chance of being chosen, how likely is it that your opponent will charge their energy next turn?";
                    question.questionSpeakKey = "que08";

                    // Responses
                    question.Response0 = "0.33";
                    question.Response1 = "0.40";
                    question.Response2 = "0.50";
                    question.Response3 = "1.00";

                    // Answer
                    question.answerIndex = 2;
                    question.correctAnswerResponse = "Since there are only 2 options, each event has a 0.50 chance of happening.";
                    question.incorrectAnswerResponse = "There are only 2 possible outcomes.";

                    // Translates the question.
                    if (translate)
                    {
                        question.question = defs["que08"];
                        
                        question.correctAnswerResponse = defs["que08_correct"];
                        question.correctAnswerSpeakKey = "que08_correct";
                        question.incorrectAnswerResponse = defs["que08_incorrect"];
                        question.incorrectAnswerSpeakKey = "que08_incorrect";
                    }
                    break;
                

                
                case 9:
                    // Question
                    question.question = "Move A has a power of 70 and an accuracy of 0.95. Move B has a power of 80 and an accuracy of 0.85. If you're prioritizing moves with high accuracies, should you pick Move B over Move A?";
                    question.questionSpeakKey = "que09";

                    // Responses
                    question.Response0 = "Yes";
                    question.Response1 = "No";
                    question.Response2 = "";
                    question.Response3 = "";

                    // Answer
                    question.answerIndex = 1;
                    question.correctAnswerResponse = "Move B is stronger, but it's more likely to miss.";
                    question.incorrectAnswerResponse = "Accuracy works on a 0-1 scale, with 1.00 meaning a move will always hit.";

                    // Translates the question.
                    if (translate)
                    {
                        question.question = defs["que09"];
                        question.Response0 = defs["kwd_yes"];
                        question.Response1 = defs["kwd_no"];

                        question.correctAnswerResponse = defs["que09_correct"];
                        question.correctAnswerSpeakKey = "que09_correct";
                        question.incorrectAnswerResponse = defs["que09_incorrect"];
                        question.incorrectAnswerSpeakKey = "que09_incorrect";
                    }
                    break;

                case 10:
                    // Question
                    question.question = "Move A has a power of 90 and an accuracy of 0.80. Move B has a power of 100 and an accuracy of 0.75. If you're prioritizing moves with high accuracies, which move would you pick?";
                    question.questionSpeakKey = "que10";

                    // Responses
                    question.Response0 = "Move A";
                    question.Response1 = "Move B";
                    question.Response2 = "";
                    question.Response3 = "";

                    // Answer
                    question.answerIndex = 0;
                    question.correctAnswerResponse = "Move B is stronger, but Move A is more accurate.";
                    question.incorrectAnswerResponse = "Accuracy works on a 0-1 scale, with 0 meaning a move will never hit.";

                    // Translates the question.
                    if (translate)
                    {
                        question.question = defs["que10"];
                        question.Response0 = defs["que_res_moveA"];
                        question.Response1 = defs["que_res_moveB"];
                        
                        question.correctAnswerResponse = defs["que10_correct"];
                        question.correctAnswerSpeakKey = "que10_correct";
                        question.incorrectAnswerResponse = defs["que10_incorrect"];
                        question.incorrectAnswerSpeakKey = "que10_incorrect";
                    }
                    break;

                case 11:
                    // Question
                    question.question = "Move A has a 0.40 chance of burning the target, Move B has a 0.25 chance of burning the target, and Move C always burns the target. If you want to burn your foe, which move would have the best chance of doing so?";
                    question.questionSpeakKey = "que11";

                    // Responses
                    question.Response0 = "Move A";
                    question.Response1 = "Move B";
                    question.Response2 = "Move C";
                    question.Response3 = "";

                    // Answer
                    question.answerIndex = 2;
                    question.correctAnswerResponse = "Move C has a burn chance of 1.00, which means that it always burns the target.";
                    question.incorrectAnswerResponse = "An event that will always happen has a chance value of 1.00.";

                    // Translates the question.
                    if (translate)
                    {
                        question.question = defs["que11"];
                        question.Response0 = defs["que_res_moveA"];
                        question.Response1 = defs["que_res_moveB"];
                        question.Response2 = defs["que_res_moveC"];

                        question.correctAnswerResponse = defs["que11_correct"];
                        question.correctAnswerSpeakKey = "que11_correct";
                        question.incorrectAnswerResponse = defs["que11_incorrect"];
                        question.incorrectAnswerSpeakKey = "que11_incorrect";
                    }
                    break;

                case 12:
                    // Question
                    question.question = "Move A has a 0.30 chance of burning the target, Move B has a 0.10 chance of burning the target, and Move C has a 0.60 chance of burning the target. If you want to burn your opponent, which move has the worst chance of doing so?";
                    question.questionSpeakKey = "que12";

                    // Responses
                    question.Response0 = "Move A";
                    question.Response1 = "Move B";
                    question.Response2 = "Move C";
                    question.Response3 = "";

                    // Answer
                    question.answerIndex = 1;
                    question.correctAnswerResponse = "Since Move B has the lowest burn chance, it is the least likely to inflict burn status.";
                    question.incorrectAnswerResponse = "In a 0-1 scale, an event chance of 0.00 means said event will never happen.";

                    // Translates the question.
                    if (translate)
                    {
                        question.question = defs["que12"];
                        question.Response0 = defs["que_res_moveA"];
                        question.Response1 = defs["que_res_moveB"];
                        question.Response2 = defs["que_res_moveC"];
                        
                        question.correctAnswerResponse = defs["que12_correct"];
                        question.correctAnswerSpeakKey = "que12_correct";
                        question.incorrectAnswerResponse = defs["que12_incorrect"];
                        question.incorrectAnswerSpeakKey = "que12_incorrect";
                    }
                    break;



                case 13:
                    // Question
                    question.question = "Move A has an accuracy of 0.85 and a paralysis chance of 0.45. Move B has an accuracy of 0.95 and a paralysis chance of 0.30. Move C has an accuracy of 0.70 and a paralysis chance of 0.60. If you want to paralyze the target, which move has the lowest chance of doing so if it hits?";
                    question.questionSpeakKey = "que13";

                    // Responses
                    question.Response0 = "Move A";
                    question.Response1 = "Move B";
                    question.Response2 = "Move C";
                    question.Response3 = "";

                    // Answer
                    question.answerIndex = 1;
                    question.correctAnswerResponse = "While Move B is the most accurate, it has the lowest paralysis chance of the 3.";
                    question.incorrectAnswerResponse = "Since paralysis is the focus, the accuracy of each move is not important.";

                    // Translates the question.
                    if (translate)
                    {
                        question.question = defs["que13"];
                        question.Response0 = defs["que_res_moveA"];
                        question.Response1 = defs["que_res_moveB"];
                        question.Response2 = defs["que_res_moveC"];

                        question.correctAnswerResponse = defs["que13_correct"];
                        question.correctAnswerSpeakKey = "que13_correct";
                        question.incorrectAnswerResponse = defs["que13_incorrect"];
                        question.incorrectAnswerSpeakKey = "que13_incorrect";
                    }
                    break;

                case 14:
                    // Question
                    question.question = "You have 3 moves: Move A, Move B, and Move C. Move A has an accuracy of 0.90, Move B has an accuracy of 0.80, and Move C has an accuracy of 1.00. If you are hit by a move that reduces your accuracy, which move now has a 1.00 chance of hitting its target?";
                    question.questionSpeakKey = "que14";

                    // Responses
                    question.Response0 = "Move A";
                    question.Response1 = "Move B";
                    question.Response2 = "Move C";
                    question.Response3 = "None of the 3 moves have a 1.00 chance of hitting their target.";

                    // Answer
                    question.answerIndex = 3;
                    question.correctAnswerResponse = "Since your accuracy has been lowered, no move is guaranteed to hit its target.";
                    question.incorrectAnswerResponse = "In a 0-1 scale, the closer an event chance is to 0.00, the less likely said event is.";

                    // Translates the question.
                    if (translate)
                    {
                        question.question = defs["que14"];
                        question.Response0 = defs["que_res_moveA"];
                        question.Response1 = defs["que_res_moveB"];
                        question.Response2 = defs["que_res_moveC"];
                        question.Response3 = defs["que14_res03"];
                        
                        question.correctAnswerResponse = defs["que14_correct"];
                        question.correctAnswerSpeakKey = "que14_correct";
                        question.incorrectAnswerResponse = defs["que14_incorrect"];
                        question.incorrectAnswerSpeakKey = "que14_incorrect";
                    }
                    break;

                case 15:
                    // Question
                    question.question = "You have 3 moves: Move A, Move B, and Move C. Move A has an accuracy of 1.00, Move B has an accuracy of 0.95, and Move C has an accuracy of 0.90. If your accuracy was increased by 0.05, which moves would always hit their target?";
                    question.questionSpeakKey = "que15";

                    // Responses
                    question.Response0 = "Move A only";
                    question.Response1 = "Move A and Move B";
                    question.Response2 = "Move A and Move C";
                    question.Response3 = "All 3 moves are guaranteed to hit their target.";

                    // Answer
                    question.answerIndex = 1;
                    question.correctAnswerResponse = "Since your accuracy has increased, Move B now has a 1.00 chance of hitting its target.";
                    question.incorrectAnswerResponse = "In a 0-1 scale, the closer an event chance is to 1.00, the more likely said event is.";

                    // Translates the question.
                    if (translate)
                    {
                        question.question = defs["que15"];
                        question.Response0 = defs["que15_res00"];
                        question.Response1 = defs["que15_res01"];
                        question.Response2 = defs["que15_res02"];
                        question.Response3 = defs["que15_res03"];

                        question.correctAnswerResponse = defs["que15_correct"];
                        question.correctAnswerSpeakKey = "que15_correct";
                        question.incorrectAnswerResponse = defs["que15_incorrect"];
                        question.incorrectAnswerSpeakKey = "que15_incorrect";
                    }
                    break;

                case 16:
                    // Question
                    question.question = "Move A has a 0.25 chance of burning the target, Move B has a 0.30 chance of paralyzing the target, and Move C has a 0.50 chance of causing critical damage. Across these 3 moves, which event is the most likely to occur?";
                    question.questionSpeakKey = "que16";

                    // Responses
                    question.Response0 = "Move A burning the target.";
                    question.Response1 = "Move B paralyzing the target.";
                    question.Response2 = "Move C getting a critical damage bonus on the target.";
                    question.Response3 = "";

                    // Answer
                    question.answerIndex = 2;
                    question.correctAnswerResponse = "Move C getting a critical damage bonus is the most likely event to happen.";
                    question.incorrectAnswerResponse = "The higher the chance value, the more likely the event is.";

                    // Translates the question.
                    if (translate)
                    {
                        question.question = defs["que16"];
                        question.Response0 = defs["que16_res00"];
                        question.Response1 = defs["que16_res01"];
                        question.Response2 = defs["que16_res02"];

                        question.correctAnswerResponse = defs["que16_correct"];
                        question.correctAnswerSpeakKey = "que16_correct";
                        question.incorrectAnswerResponse = defs["que16_incorrect"];
                        question.incorrectAnswerSpeakKey = "que16_incorrect";
                    }
                    break;



                case 17:
                    // Question
                    question.question = "Move A has an accuracy of 0.70, Move B has an accuracy of 0.85, and Move C has an accuracy of 0.90. If you want to defeat your foe the next turn, and all 3 moves are powerful enough to do so, which move would be the riskiest choice?";
                    question.questionSpeakKey = "que17";

                    // Responses
                    question.Response0 = "Move A";
                    question.Response1 = "Move B";
                    question.Response2 = "Move C";
                    question.Response3 = "All 3 moves are equally as risky.";

                    // Answer
                    question.answerIndex = 0;
                    question.correctAnswerResponse = "Since Move A has the lowest accuracy, it is the riskiest move to use.";
                    question.incorrectAnswerResponse = "The lower the accuracy, the more likely the move is to miss.";

                    // Translates the question.
                    if (translate)
                    {
                        question.question = defs["que17"];
                        question.Response0 = defs["que_res_moveA"];
                        question.Response1 = defs["que_res_moveB"];
                        question.Response2 = defs["que_res_moveC"];
                        question.Response3 = defs["que17_res03"];

                        question.correctAnswerResponse = defs["que17_correct"];
                        question.correctAnswerSpeakKey = "que17_correct";
                        question.incorrectAnswerResponse = defs["que17_incorrect"];
                        question.incorrectAnswerSpeakKey = "que17_incorrect";
                    }
                    break;

                case 18:
                    // Question
                    question.question = "Your foe has 4 moves: Move A, Move B, Move C, and Move D. Move A's accuracy is 0.90, Move B's accuracy is 0.70, Move C never misses, and Move D's accuracy is 0.85. Which move is least likely to hit you?";
                    question.questionSpeakKey = "que18";

                    // Responses
                    question.Response0 = "Move A";
                    question.Response1 = "Move B";
                    question.Response2 = "Move C";
                    question.Response3 = "Move D";

                    // Answer
                    question.answerIndex = 1;
                    question.correctAnswerResponse = "Move B is the least accurate, so it's the least likely to hit you.";
                    question.incorrectAnswerResponse = "The lower the accuracy, the less likely the move is to hit its target.";

                    // Translates the question.
                    if (translate)
                    {
                        question.question = defs["que18"];
                        question.Response0 = defs["que_res_moveA"];
                        question.Response1 = defs["que_res_moveB"];
                        question.Response2 = defs["que_res_moveC"];
                        question.Response3 = defs["que_res_moveD"];

                        question.correctAnswerResponse = defs["que18_correct"];
                        question.incorrectAnswerResponse = defs["que18_incorrect"];
                    }
                    break;

                case 19:
                    // Question
                    question.question = "Move A has a 0.40 chance of raising the user's attack, Move B has a 0.30 chance of raising the user's defense, and Move C has a 0.20 chance of raising the user's speed. If your attack stat cannot go any higher, which event is the most likely?";
                    question.questionSpeakKey = "que19";

                    // Responses
                    question.Response0 = "Move A increasing your attack stat.";
                    question.Response1 = "Move B increasing your defense stat.";
                    question.Response2 = "Move C increasing your speed stat.";
                    question.Response3 = "All move effects have an equal chance of occurring.";

                    // Answer
                    question.answerIndex = 1;
                    question.correctAnswerResponse = "Since your attack stat cannot go higher, Move A's event is irrelevant.";
                    question.incorrectAnswerResponse = "If an event would have no effect, then it can be ignored.";

                    // Translates the question.
                    if (translate)
                    {
                        question.question = defs["que19"];
                        question.Response0 = defs["que19_res00"];
                        question.Response1 = defs["que19_res01"];
                        question.Response2 = defs["que19_res02"];
                        question.Response3 = defs["que19_res03"];

                        question.correctAnswerResponse = defs["que19_correct"];
                        question.correctAnswerSpeakKey = "que19_correct";
                        question.incorrectAnswerResponse = defs["que19_incorrect"];
                        question.incorrectAnswerSpeakKey = "que19_incorrect";
                    }
                    break;

                case 20:
                    // Question
                    question.question = "Move A has a 0.40 chance of raising the user's attack, Move B has a 0.60 chance of raising the user's defense, and Move C has a 0.20 chance of raising the user's speed. If your defense cannot go any lower, which event is the most likely?";
                    question.questionSpeakKey = "que20";

                    // Responses
                    question.Response0 = "Move A raising your attack.";
                    question.Response1 = "Move B raising your defense.";
                    question.Response2 = "Move C raising your speed.";
                    question.Response3 = "The events all have the same chance of occurring.";

                    // Answer
                    question.answerIndex = 1;
                    question.correctAnswerResponse = "Your defense is not maxed out, so Move B's event can still happen.";
                    question.incorrectAnswerResponse = "A stat that cannot go any lower can still be raised.";

                    // Translates the question.
                    if (translate)
                    {
                        question.question = defs["que20"];
                        question.Response0 = defs["que20_res00"];
                        question.Response1 = defs["que20_res01"];
                        question.Response2 = defs["que20_res02"];
                        question.Response3 = defs["que20_res03"];

                        question.correctAnswerResponse = defs["que20_correct"];
                        question.correctAnswerSpeakKey = "que20_correct";
                        question.incorrectAnswerResponse = defs["que20_incorrect"];
                        question.incorrectAnswerSpeakKey = "que20_incorrect";
                    }
                    break;



                case 21:
                    // Question
                    question.question = "There are 10 doors remaining, and 2 of them are treasure doors. Assuming you have no other information, what is the chance of you choosing a treasure door?";
                    question.questionSpeakKey = "que21";

                    // Responses
                    question.Response0 = "2/10";
                    question.Response1 = "4/10";
                    question.Response2 = "6/10";
                    question.Response3 = "8/10";

                    // Answer
                    question.answerIndex = 0;
                    question.correctAnswerResponse = "Since 2/10 doors are treasure doors, there's a 20% chance of you choosing one.";
                    question.incorrectAnswerResponse = "When the left value is equal to the right value, the chance value is 1.00 in decimal form.";

                    // Translates the question.
                    if (translate)
                    {
                        question.question = defs["que21"];

                        question.correctAnswerResponse = defs["que21_correct"];
                        question.correctAnswerSpeakKey = "que21_correct";
                        question.incorrectAnswerResponse = defs["que21_incorrect"];
                        question.incorrectAnswerSpeakKey = "que21_incorrect";
                    }
                    break;

                case 22:
                    // Question
                    question.question = "There are 5 doors remaining, and 1 of them is a treasure door. If every door has an equal chance of being chosen, what is the chance of you not choosing the treasure door?";
                    question.questionSpeakKey = "que22";

                    // Responses
                    question.Response0 = "1/5";
                    question.Response1 = "2/5";
                    question.Response2 = "3/5";
                    question.Response3 = "4/5";

                    // Answer
                    question.answerIndex = 3;
                    question.correctAnswerResponse = "4/5 doors are not treasure doors, so a non-treasure door has an 80% chance of being chosen.";
                    question.incorrectAnswerResponse = "The larger the numerator (left value), the more likely the event is.";

                    // Translates the question.
                    if (translate)
                    {
                        question.question = defs["que22"];

                        question.correctAnswerResponse = defs["que22_correct"];
                        question.correctAnswerSpeakKey = "que22_correct";
                        question.incorrectAnswerResponse = defs["que22_incorrect"];
                        question.incorrectAnswerSpeakKey = "que22_incorrect";
                    }
                    break;

                case 23:
                    // Question
                    question.question = "Enemy A is behind 1/10 doors, Enemy B is behind 2/10 doors, Enemy C is behind 5/10 doors, and Enemy D is behind 2/10 doors. If every door has an equal chance of being chosen, which enemy will you most likely encounter next?";
                    question.questionSpeakKey = "que23";

                    // Responses
                    question.Response0 = "Enemy A";
                    question.Response1 = "Enemy B";
                    question.Response2 = "Enemy C";
                    question.Response3 = "Enemy D";

                    // Answer
                    question.answerIndex = 2;
                    question.correctAnswerResponse = "Since Enemy C has the highest door count, you are most likely to encounter one of them next.";
                    question.incorrectAnswerResponse = "The lower the numerator (left value), the less likely the event is.";

                    // Translates the question.
                    if (translate)
                    {
                        question.question = defs["que23"];
                        question.Response0 = defs["que_res_enemyA"];
                        question.Response1 = defs["que_res_enemyB"];
                        question.Response2 = defs["que_res_enemyC"];
                        question.Response3 = defs["que_res_enemyD"];

                        question.correctAnswerResponse = defs["que23_correct"];
                        question.correctAnswerSpeakKey = "que23_correct";
                        question.incorrectAnswerResponse = defs["que23_incorrect"];
                        question.incorrectAnswerSpeakKey = "que23_incorrect";
                    }
                    break;

                case 24:
                    // Question
                    question.question = "Enemy A is behind 2/9 doors, Enemy B is behind 3/9 doors, Enemy C is behind 3/9 doors, and Enemy D is behind all remaining doors. If every door has an equal chance of being chosen, which enemy are you least likely to encounter next?";
                    question.questionSpeakKey = "que24";

                    // Responses
                    question.Response0 = "Enemy A";
                    question.Response1 = "Enemy B";
                    question.Response2 = "Enemy C";
                    question.Response3 = "Enemy D";

                    // Answer
                    question.answerIndex = 3;
                    question.correctAnswerResponse = "Enemy D is behind 1/9 doors, so it is the least likely enemy to be encountered next.";
                    question.incorrectAnswerResponse = "The denominator (right value) is the total number of doors.";

                    // Translates the question.
                    if (translate)
                    {
                        question.question = defs["que24"];
                        question.Response0 = defs["que_res_enemyA"];
                        question.Response1 = defs["que_res_enemyB"];
                        question.Response2 = defs["que_res_enemyC"];
                        question.Response3 = defs["que_res_enemyD"];

                        question.correctAnswerResponse = defs["que24_correct"];
                        question.correctAnswerSpeakKey = "que24_correct";
                        question.incorrectAnswerResponse = defs["que24_incorrect"];
                        question.incorrectAnswerSpeakKey = "que24_incorrect";
                    }
                    break;

                case 25:
                    // Question
                    question.question = "Move A has a critical damage chance of 0.40. If you get the critical damage bonus, you will win the battle in 1 turn. If you don't get the critical damage bonus, you will win the battle in 2 turns. What is your chance of winning the battle in 2 turns?";
                    question.questionSpeakKey = "que25";
                    
                    // Responses
                    question.Response0 = "0.00";
                    question.Response1 = "0.40";
                    question.Response2 = "0.60";
                    question.Response3 = "1.00";

                    // Answer
                    question.answerIndex = 2;
                    question.correctAnswerResponse = "The critical damage chance is also the chance of ending the battle in one turn.";
                    question.incorrectAnswerResponse = "The amount of turns needed to win the battle hinges on getting a critical damage bonus.";

                    // Translates the question.
                    if (translate)
                    {
                        question.question = defs["que25"];
                        question.correctAnswerResponse = defs["que25_correct"];
                        question.correctAnswerSpeakKey = "que25_correct";
                        question.incorrectAnswerResponse = defs["que25_incorrect"];
                        question.incorrectAnswerSpeakKey = "que25_incorrect";
                    }
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

        // Gets a random question, ignoring the questions in the 'usedQuestions' list.
        // This ignores question 0, since it's a default question.
        public GameQuestion GetRandomQuestion(List<int> usedQuestions, bool randomResponseOrder = false)
        {
            // The question list.
            List<int> questions = new List<int>();

            // Adds all the questions to the list.
            for(int i = 1; i < QUESTION_COUNT; i++)
            {
                questions.Add(i);
            }

            // Removes the used questions.
            for(int i = 0; i < usedQuestions.Count && questions.Count > 0; i++)
            {
                // Removes all the used questions.
                if (questions.Contains(usedQuestions[i]))
                    questions.Remove(usedQuestions[i]);
            }


            // If there are no questions left, just give a random question (which may have already been used).
            if(questions.Count != 0)
            {
                // Grabs a random index.
                int randIndex = Random.Range(0, questions.Count);

                // Gets the question of the provided number.
                GameQuestion question = GetQuestion(questions[randIndex]);

                // Randomizes the response order if that was what was requested.
                if (randomResponseOrder)
                    question = RandomizeResponseOrder(question);

                // Returns the question.
                return question;
            }
            else
            {
                // Gets a random question, regardless of if it's been used or not.
                return GetRandomQuestion(randomResponseOrder);
            }
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