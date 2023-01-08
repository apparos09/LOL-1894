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
        public const int QUESTION_COUNT = 25;

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

                    // Answer
                    question.answerIndex = 0;

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
                    question.question = "[When 2 battlers have the same speed, the turn order is evenly random. If the player has the same speed as their opponent, what is the chance of the player going first?]";
                    question.questionSpeakKey = "que01";

                    // Responses
                    question.Response0 = "0.25";
                    question.Response1 = "0.50";
                    question.Response2 = "0.75";
                    question.Response3 = "1.00";

                    // Answer
                    question.answerIndex = 1;

                    // Translates the question.
                    if (translate)
                    {
                        question.question = defs["que01"];
                    }

                    break;
                case 2:
                    // Question
                    question.question = "[If a move does not mention that it can burn the target, the burn chance is 0.00. What does this mean?]";
                    question.questionSpeakKey = "que02";

                    // Responses
                    question.Response0 = "[The move cannot inflict burn status on the target.]";
                    question.Response1 = "[The move always inflicts burn status on the target.]";
                    question.Response2 = "[The move might inflict burn status on the target.]";
                    question.Response3 = "[The target is immune to being burned.]";

                    // Answer
                    question.answerIndex = 0;

                    // Translates the question.
                    if (translate)
                    {
                        question.question = defs["que02"];
                        question.Response0 = defs["que02_res00"];
                        question.Response1 = defs["que02_res01"];
                        question.Response2 = defs["que02_res02"];
                        question.Response3 = defs["que02_res03"];
                    }

                    break;
                case 3:
                    // Question
                    question.question = "[If a move mentions that it can paralyze the target, the paralysis chance is greater than 0.00. If the paralysis chance is set to 1.00, what does this mean?]";
                    question.questionSpeakKey = "que03";

                    // Responses
                    question.Response0 = "[The target is immune to paralysis status.]";
                    question.Response1 = "[The move will never inflict paralysis status.]";
                    question.Response2 = "[The move will inflict paralysis status half of the time.]";
                    question.Response3 = "[The move will always inflict paralysis status.]";

                    // Answer
                    question.answerIndex = 3;

                    // Translates the question.
                    if (translate)
                    {
                        question.question = defs["que03"];
                        question.Response0 = defs["que03_res00"];
                        question.Response1 = defs["que03_res01"];
                        question.Response2 = defs["que03_res02"];
                        question.Response3 = defs["que03_res03"];
                    }
                    break;

                case 4:
                    // Question
                    question.question = "[Move A has a power of 40 and an accuracy of 1.00. Move B has a power 60 and an accuracy of 0.90. What is true about Move A and Move B?]";
                    question.questionSpeakKey = "que04";

                    // Responses
                    question.Response0 = "[Move A is stronger than Move B, but it is less accurate.]";
                    question.Response1 = "[Move A is stronger than Move B, and it is more accurate.]";
                    question.Response2 = "[Move A is weaker than Move B, and it is less accurate.]";
                    question.Response3 = "[Move A is weaker than Move B, but it is more accurate.]";

                    // Answer
                    question.answerIndex = 3;

                    // Translates the question.
                    if (translate)
                    {
                        question.question = defs["que04"];
                        question.Response0 = defs["que04_res00"];
                        question.Response1 = defs["que04_res01"];
                        question.Response2 = defs["que04_res02"];
                        question.Response3 = defs["que04_res03"];
                    }
                    break;
                
                
                
                case 5:
                    // Question
                    question.question = "[The opponent has 65% of their health, and 3 of their 4 battle moves can restore their health. Assuming the opponent will not charge their energy, and that each move has an equal chance of being chosen, what is the chance that the opponent will use a healing move?]";
                    question.questionSpeakKey = "que05";

                    // Responses
                    question.Response0 = "0.20";
                    question.Response1 = "0.60";
                    question.Response2 = "0.75";
                    question.Response3 = "1.00";

                    // Answer
                    question.answerIndex = 2;

                    // Translates the question.
                    if (translate)
                    {
                        question.question = defs["que05"];
                    }

                    break;

                case 6:
                    // Question
                    question.question = "[The player uses a move that raises their accuracy by 2 stages. Which of the following statements is true?]";
                    question.questionSpeakKey = "que06";

                    // Responses
                    question.Response0 = "[The player will move faster than they did before.]";
                    question.Response1 = "[The player’s attacks will do more damage.]";
                    question.Response2 = "[The player’s moves are more likely to hit their target.]";
                    question.Response3 = "[The opponent’s moves will do less damage to the player.]";

                    // Answer
                    question.answerIndex = 2;

                    // Translates the question.
                    if (translate)
                    {
                        question.question = defs["que06"];
                        question.Response0 = defs["que06_res00"];
                        question.Response1 = defs["que06_res01"];
                        question.Response2 = defs["que06_res02"];
                        question.Response3 = defs["que06_res03"];
                    }
                    break;

                case 7:
                    // Question
                    question.question = "[The player is hit by a move that lowers their accuracy by 1 stage. Which of the following statements is true?]";
                    question.questionSpeakKey = "que07";

                    // Responses
                    question.Response0 = "[The player’s moves are now less likely to hit their target.]";
                    question.Response1 = "[The player will move slower than they did before.]";
                    question.Response2 = "[The player will take more damage from their opponent.]";
                    question.Response3 = "[The player’s attacks will do less damage.]";

                    // Answer
                    question.answerIndex = 0;

                    // Translates the question.
                    if (translate)
                    {
                        question.question = defs["que07"];
                        question.Response0 = defs["que07_res00"];
                        question.Response1 = defs["que07_res01"];
                        question.Response2 = defs["que07_res02"];
                        question.Response3 = defs["que07_res03"];
                    }
                    break;

                case 8:
                    // Question
                    question.question = "[The opponent has 20% of their energy left. Along with the charge move, the opponent has 1 other move that they can use. Of the opponent’s available moves, how likely is it that they will choose the charge move?]";
                    question.questionSpeakKey = "que08";

                    // Responses
                    question.Response0 = "0.33";
                    question.Response1 = "0.40";
                    question.Response2 = "0.50";
                    question.Response3 = "1.00";

                    // Answer
                    question.answerIndex = 2;

                    // Translates the question.
                    if (translate)
                    {
                        question.question = defs["que08"];
                    }
                    break;
                

                
                case 9:
                    // Question
                    question.question = "[Move A has a power of 70 and an accuracy of 0.95. Move B has a power of 80 and an accuracy of 0.85. If the user is prioritizing moves with high accuracy, should they pick Move B over Move A?]";
                    question.questionSpeakKey = "que09";

                    // Responses
                    question.Response0 = "[Yes]";
                    question.Response1 = "[No]";
                    question.Response2 = "";
                    question.Response3 = "";

                    // Answer
                    question.answerIndex = 1;

                    // Translates the question.
                    if (translate)
                    {
                        question.question = defs["que09"];
                        question.Response0 = defs["kwd_yes"];
                        question.Response1 = defs["kwd_no"];
                    }
                    break;

                case 10:
                    // Question
                    question.question = "[Move A has a power of 90 and an accuracy of 0.80. Move B has a power of 100 and an accuracy of 0.75. If the user is prioritizing moves with high accuracy, which move would they pick?]";
                    question.questionSpeakKey = "que10";

                    // Responses
                    question.Response0 = "[Move A]";
                    question.Response1 = "[Move B]";
                    question.Response2 = "";
                    question.Response3 = "";

                    // Answer
                    question.answerIndex = 0;

                    // Translates the question.
                    if (translate)
                    {
                        question.question = defs["que10"];
                        question.Response0 = defs["que_res_moveA"];
                        question.Response1 = defs["que_res_moveB"];
                    }
                    break;

                case 11:
                    // Question
                    question.question = "[Move A has a 0.40 chance of burning the target, Move B has a 0.25 chance of burning the target, and Move C always burns the target. If the user wants to burn their opponent, which move has the best chance of doing so?]";
                    question.questionSpeakKey = "que11";

                    // Responses
                    question.Response0 = "[Move A]";
                    question.Response1 = "[Move B]";
                    question.Response2 = "[Move C]";
                    question.Response3 = "";

                    // Answer
                    question.answerIndex = 2;

                    // Translates the question.
                    if (translate)
                    {
                        question.question = defs["que11"];
                        question.Response0 = defs["que_res_moveA"];
                        question.Response1 = defs["que_res_moveB"];
                        question.Response2 = defs["que_res_moveC"];
                    }
                    break;

                case 12:
                    // Question
                    question.question = "[Move A has a 0.30 chance of burning the target, Move B has a 0.10 chance of burning the target, and Move C has a 0.60 chance of burning the target. If the user wants to burn their opponent, which move has the worst chance of doing so?]";
                    question.questionSpeakKey = "que12";

                    // Responses
                    question.Response0 = "[Move A]";
                    question.Response1 = "[Move B]";
                    question.Response2 = "[Move C]";
                    question.Response3 = "";

                    // Answer
                    question.answerIndex = 1;

                    // Translates the question.
                    if (translate)
                    {
                        question.question = defs["que12"];
                        question.Response0 = defs["que_res_moveA"];
                        question.Response1 = defs["que_res_moveB"];
                        question.Response2 = defs["que_res_moveC"];
                    }
                    break;



                case 13:
                    // Question
                    question.question = "[Move A has an accuracy of 0.85 and a paralysis chance of 0.45. Move B has an accuracy of 0.95 and a paralysis chance of 0.30. Move C has an accuracy of 0.70 and a paralysis chance of 0.60. Which move has the lowest chance of paralyzing the target if it hits?]";
                    question.questionSpeakKey = "que13";

                    // Responses
                    question.Response0 = "[Move A]";
                    question.Response1 = "[Move B]";
                    question.Response2 = "[Move C]";
                    question.Response3 = "";

                    // Answer
                    question.answerIndex = 1;

                    // Translates the question.
                    if (translate)
                    {
                        question.question = defs["que13"];
                        question.Response0 = defs["que_res_moveA"];
                        question.Response1 = defs["que_res_moveB"];
                        question.Response2 = defs["que_res_moveC"];
                    }
                    break;

                case 14:
                    // Question
                    question.question = "[The player has 3 moves: Move A, Move B, and Move C. Move A has an accuracy of 0.90, Move B has an accuracy of 0.80, and Move C has an accuracy of 1.00. If the player is hit by a move that reduces their accuracy, which move has a 1.00 chance of hitting its target?]";
                    question.questionSpeakKey = "que14";

                    // Responses
                    question.Response0 = "[Move A]";
                    question.Response1 = "[Move B]";
                    question.Response2 = "[Move C]";
                    question.Response3 = "[All 3 moves have a chance of missing their target.]";

                    // Answer
                    question.answerIndex = 3;

                    // Translates the question.
                    if (translate)
                    {
                        question.question = defs["que14"];
                        question.Response0 = defs["que_res_moveA"];
                        question.Response1 = defs["que_res_moveB"];
                        question.Response2 = defs["que_res_moveC"];
                        question.Response3 = defs["que14_res03"];
                    }
                    break;

                case 15:
                    // Question
                    question.question = "[Move A has an accuracy of 1.00, Move B has an accuracy of 0.95, and Move C has an accuracy of 0.90. If the user’s accuracy is increased by 0.05, which moves will always hit their target?]";
                    question.questionSpeakKey = "que15";

                    // Responses
                    question.Response0 = "[Move A only]";
                    question.Response1 = "[Move A and Move B]";
                    question.Response2 = "[Move A and Move C]";
                    question.Response3 = "[All 3 moves are guaranteed to hit their target.]";

                    // Answer
                    question.answerIndex = 1;

                    // Translates the question.
                    if (translate)
                    {
                        question.question = defs["que15"];
                        question.Response1 = defs["que15_res00"];
                        question.Response0 = defs["que15_res01"];
                        question.Response2 = defs["que15_res02"];
                        question.Response3 = defs["que15_res03"];
                    }
                    break;

                case 16:
                    // Question
                    question.question = "[Move A has a 0.25 chance of burning the opponent, Move B has a 0.30 chance of paralyzing the opponent, and Move C has a 0.50 chance of getting critical damage. Which event is the most likely to happen?]";
                    question.questionSpeakKey = "que16";

                    // Responses
                    question.Response0 = "[Move A burning the target.]";
                    question.Response1 = "[Move B paralyzing the target.]";
                    question.Response2 = "[Move C getting a critical damage bonus on the target.]";
                    question.Response3 = "";

                    // Answer
                    question.answerIndex = 2;

                    // Translates the question.
                    if (translate)
                    {
                        question.question = defs["que16"];
                        question.Response1 = defs["que16_res00"];
                        question.Response0 = defs["que16_res01"];
                        question.Response2 = defs["que16_res02"];
                    }
                    break;



                case 17:
                    // Question
                    question.question = "[Move A has an accuracy of 0.70, Move B has an accuracy of 0.85, and Move C has an accuracy of 0.90. If all 3 moves are powerful enough to defeat the opponent in one turn, which move is the riskiest option?]";
                    question.questionSpeakKey = "que17";

                    // Responses
                    question.Response0 = "[Move A]";
                    question.Response1 = "[Move B]";
                    question.Response2 = "[Move C]";
                    question.Response3 = "[All 3 moves are equally as risky.]";

                    // Answer
                    question.answerIndex = 0;

                    // Translates the question.
                    if (translate)
                    {
                        question.question = defs["que17"];
                        question.Response0 = defs["que_res_moveA"];
                        question.Response1 = defs["que_res_moveB"];
                        question.Response2 = defs["que_res_moveC"];
                        question.Response3 = defs["que17_res03"];
                    }
                    break;

                case 18:
                    // Question
                    question.question = "[The opponent has 4 moves. Move A’s accuracy is 0.90, Move B’s accuracy is 0.70, Move C’s accuracy is 1.00, and Move D’s accuracy is 0.85. Which move is least likely to hit the player?]";
                    question.questionSpeakKey = "que18";

                    // Responses
                    question.Response0 = "[Move A]";
                    question.Response1 = "[Move B]";
                    question.Response2 = "[Move C]";
                    question.Response3 = "[Move D]";

                    // Answer
                    question.answerIndex = 1;

                    // Translates the question.
                    if (translate)
                    {
                        question.question = defs["que18"];
                        question.Response0 = defs["que_res_moveA"];
                        question.Response1 = defs["que_res_moveB"];
                        question.Response2 = defs["que_res_moveC"];
                        question.Response3 = defs["que_res_moveD"];
                    }
                    break;

                case 19:
                    // Question
                    question.question = "[Move A has a 0.40 chance of raising the user’s attack, Move B has a 0.30 chance of raising the user’s defense, and Move C has a 0.20 chance of raising the user’s speed. If the player’s attack stat cannot go any higher, which move effect has the highest chance of occurring?]";
                    question.questionSpeakKey = "que19";

                    // Responses
                    question.Response0 = "[Move A increasing the user’s attack stat.]";
                    question.Response1 = "[Move B increasing the user’s defense stat.]";
                    question.Response2 = "[Move C increasing the user’s speed stat.]";
                    question.Response3 = "[All move effects have an equal chance of occurring.]";

                    // Answer
                    question.answerIndex = 1;

                    // Translates the question.
                    if (translate)
                    {
                        question.question = defs["que19"];
                        question.Response0 = defs["que19_res00"];
                        question.Response1 = defs["que19_res01"];
                        question.Response2 = defs["que19_res02"];
                        question.Response3 = defs["que19_res03"];
                    }
                    break;

                case 20:
                    // Question
                    question.question = "[Move A has a 0.40 chance of raising the user’s attack, Move B has a 0.60 chance of raising the user’s defense, and Move C has a 0.20 chance of raising the user’s speed. If the user’s defense cannot go any lower, which move effect has the highest chance of occurring?]";
                    question.questionSpeakKey = "que20";

                    // Responses
                    question.Response0 = "[Move A raising the user’s attack.]";
                    question.Response1 = "[Move B raising the user’s defense.]";
                    question.Response2 = "[Move C raising the user’s speed.]";
                    question.Response3 = "[The events all have the same chance of occurring.]";

                    // Answer
                    question.answerIndex = 1;

                    // Translates the question.
                    if (translate)
                    {
                        question.question = defs["que20"];
                        question.Response0 = defs["que20_res00"];
                        question.Response1 = defs["que20_res01"];
                        question.Response2 = defs["que20_res02"];
                        question.Response3 = defs["que20_res03"];
                    }
                    break;



                case 21:
                    // Question
                    question.question = "[There are 10 doors remaining, and 2 of them are treasure doors. If every door has an equal chance of being chosen, what is the chance of the player choosing a treasure door?]";
                    question.questionSpeakKey = "que21";

                    // Responses
                    question.Response0 = "0.20";
                    question.Response1 = "0.40";
                    question.Response2 = "0.60";
                    question.Response3 = "0.80";

                    // Answer
                    question.answerIndex = 0;

                    // Translates the question.
                    if (translate)
                    {
                        question.question = defs["que21"];
                    }
                    break;

                case 22:
                    // Question
                    question.question = "[There are 5 doors remaining, and 1 of them is a treasure door. If every door has an equal chance of being chosen, what is the chance of a non-treasure door being chosen?]";
                    question.questionSpeakKey = "que22";

                    // Responses
                    question.Response0 = "0.20";
                    question.Response1 = "0.40";
                    question.Response2 = "0.60";
                    question.Response3 = "0.80";

                    // Answer
                    question.answerIndex = 3;

                    // Translates the question.
                    if (translate)
                    {
                        question.question = defs["que22"];
                    }
                    break;

                case 23:
                    // Question
                    question.question = "[Enemy A is behind 1/12 doors, Enemy B is behind 3/12 doors, Enemy C is behind 6/12 doors, and Enemy D is behind 2/12 doors. If every door has an equal chance of being chosen, which enemy will the player most likely encounter next?]";
                    question.questionSpeakKey = "que23";

                    // Responses
                    question.Response0 = "[Enemy A]";
                    question.Response1 = "[Enemy B]";
                    question.Response2 = "[Enemy C]";
                    question.Response3 = "[Enemy D]";

                    // Answer
                    question.answerIndex = 2;

                    // Translates the question.
                    if (translate)
                    {
                        question.question = defs["que23"];
                        question.Response0 = defs["que_res_enemyA"];
                        question.Response1 = defs["que_res_enemyB"];
                        question.Response2 = defs["que_res_enemyC"];
                        question.Response3 = defs["que_res_enemyD"];
                    }
                    break;

                case 24:
                    // Question
                    question.question = "[Enemy A is behind 4/14 doors, Enemy B is behind 5/14 doors, Enemy C is behind 3/14 doors, and Enemy D is behind 2/14 doors. If every door has an equal chance of being chosen, which enemy is the least likely to be encountered next by the player?]";
                    question.questionSpeakKey = "que24";

                    // Responses
                    question.Response0 = "[Enemy A]";
                    question.Response1 = "[Enemy B]";
                    question.Response2 = "[Enemy C]";
                    question.Response3 = "[Enemy D]";

                    // Answer
                    question.answerIndex = 3;

                    // Translates the question.
                    if (translate)
                    {
                        question.question = defs["que24"];
                        question.Response0 = defs["que_res_enemyA"];
                        question.Response1 = defs["que_res_enemyB"];
                        question.Response2 = defs["que_res_enemyC"];
                        question.Response3 = defs["que_res_enemyD"];
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