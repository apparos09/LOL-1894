using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using SimpleJSON;
using System.ComponentModel.Design.Serialization;
using LoLSDK;

// Namespace.
namespace RM_BBTS
{
    // The system for posing questions in the overworld.
    public class GameQuestionManager : MonoBehaviour
    {
        // The gameplay manager.
        public GameplayManager gameManager;

        // TODO: hide question when in a tutorial.
        // The object of the question window.
        public GameObject questionObject;

        // The title text of the question window.
        public TMP_Text titleText;

        // // The amount of questions that have been asked to the player.
        // public int questionsUsedCount = 0;

        // The maximum amount of asked questions that will be saved when saving the game.
        // TODO: update number to match the total amount of rounds.
        public const int QUESTIONS_USED_SAVE_MAX = 5;

        // The list of asked questions (goes by question number).
        // This is to help prevent the randomizer from asking the same question multiple times.
        [Header("A list of the used questions by question number. This helps prevent the randomizer from asking a used question.")]
        private List<int> questionsUsed = new List<int>();

        // The amount of questions answered correctly.
        // private int questionsCorrectCount = 0;

        // This is a list of the questions results, which tracks if the user's answers were correct or incorrect.
        // This list should match the length of the questions used list. As such, it includes responses to duplicate responses.
        private List<bool> questionResults = new List<bool>();

        // Plays the audio for the question manager.
        public bool playAudio = true;

        [Header("Question Info")]

        // Gets set to 'true' if the question is being asked.
        private bool running = false;

        // Gets set to 'true' if the question has been answered (does not specify if correct or incorrect).
        private bool responded = false;

        // The question text.
        public TMP_Text questionText;

        // The loaded question.
        public GameQuestion currentQuestion;

        // Taken out since it doesn't get initialized in time.
        // private GameQuestions gameQuestions;

        // If 'true', the response order is randomized.
        public bool randomResponseOrder = true;

        // If set to 'true', the right answer is always shown when the question response is given.
        public bool alwaysShowAnswer = false;

        // If set to 'true', questions are redone if the user doesn't get them correct.
        public bool redoQuestionIfWrong = true;

        // The number of the past question. This is negative 1 by default.
        private int priorQuestion = -1;

        // Redoes the prior question.
        private bool redoPriorQuestion = false;

        // The button text for the four responses.
        // Response 0
        [Header("Question Info/Response 0")]
        public Button response0Button; // Button
        public Image response0Bubble; // Background to be recolored for correct/incorrect answer.
        public GameObject response0Fill; // The image object used to fill the response bubble.
        public TMP_Text response0Text; // The text tied to the response.

        // Response 1
        [Header("Question Info/Response 1")]
        public Button response1Button;
        public Image response1Bubble;
        public GameObject response1Fill;
        public TMP_Text response1Text;

        // Respone 2
        [Header("Question Info/Response 2")]
        public Button response2Button;
        public Image response2Bubble;
        public GameObject response2Fill;
        public TMP_Text response2Text;

        // Response 3
        [Header("Question Info/Response 4")]
        public Button response3Button;
        public Image response3Bubble;
        public GameObject response3Fill;
        public TMP_Text response3Text;

        // The list of response button.
        // private Button[] responseButtons;

        // The number of the selected response.
        private int selectedResponse = -1;

        // The text for evaluation.
        [Header("Question Info/Evaluation")]
        public TMP_Text evaluationText;

        // The color for the correct answer.
        public Color correctColor = Color.green;

        // The color for an incorrect answer.
        public Color incorrectColor = Color.red;

        // The color for when no questions have been answered. 
        public Color unansweredColor = Color.white;

        // The text for the score addition.
        public TMP_Text scorePlusText;

        [Header("Score Plus/Time")]
        // The maximum and minimum for adding to the player's score.
        public int maxScorePlus = 500;
        public int minScorePlus = 50;

        // Gets set to 'true' when the timer is paused.
        public bool pausedTimer = true;

        // What the timer starts at.
        public float startTime = 100.0F;

        // When the time falls below this value, the reward is reduced.
        public float reduceRewardTime = 60.0F;

        // Adds extra time when TTS is active, as the question needs to be read.
        // This is the extra time that's aded to the game timer.
        public float ttsExtraTime = 60.0F;

        // Allows for extra time to the timer when TTS is active.
        public bool addExtraTime = true;

        // The timer for the game.
        public float timer = 0.0F;

        [Header("Evaluation")]
        // The confirm button.
        public Button confirmButton;
        // The confirm button text.
        public TMP_Text confirmButtonText;

        // The next button.
        public Button finishButton;
        // The finish button text.
        public TMP_Text finishButtonText;

        // The correct and incorrect string.
        // Correct
        private string correctString = "[Correct]";
        private string correctKey = "kwd_correct";

        // Incorrect
        private string incorrectString = "[Incorrect]";
        private string incorrectKey = "kwd_incorrect";

        [Header("Evaluation/Stat Changes")]
        // The object that's enabled/disabled to show the stat changes for the results.
        public GameObject statChanges;

        // Arrow Up and Arrow Down Rotations
        private const float ARROW_UP_ROT = 90.0F;
        private const float ARROW_DOWN_ROT = -90.0F;

        // The health icon's arrow.
        public Image healthArrow;

        // The energy icon's arrow.
        public Image energyArrow;

        // The icons for attack, defense, and speed.
        public Sprite attackIcon;
        public Sprite defenseIcon;
        public Sprite speedIcon;

        // The arrow image for the attack/defense/speed stat change.
        public Image statChangeArrow;

        // Awake is called when the script instance is being loaded.
        private void Awake()
        {
            // Load question 0 to reset the question manager.
            LoadQuestion(GameQuestions.Instance.GetQuestion(0));
        }

        // Start is called before the first frame update
        void Start()
        {
            // Translation.
            JSONNode defs = SharedState.LanguageDefs;

            // Translate text.
            if(defs != null)
            {
                titleText.text = defs["kwd_questionTime"];
                correctString = defs[correctKey];
                incorrectString = defs[incorrectKey];
                confirmButtonText.text = defs["kwd_confirm"];
                finishButtonText.text = defs["kwd_finish"];
            }
        }

        // Returns the current question.
        public GameQuestion CurrentQuestion
        {
            get { return currentQuestion; }
        }

        // Gets the index of the answer to the loaded question.
        public int AnswerIndex
        { 
            get { return currentQuestion.answerIndex; }
        }

        // Returns the used questions.
        public List<int> GetQuestionsUsed(bool removeDuplicates)
        {
            // Checks if duplicate questions should be removed.
            if(removeDuplicates)
            {
                // Copies the list into a temp variable.
                List<int> temp = new List<int>();

                // Goes through each question used.
                foreach(int q in questionsUsed)
                {
                    // Add the question number to the list if it hasn't been put in there already.
                    if (!temp.Contains(q))
                        temp.Add(q);
                }

                // Returns the list.
                return temp;
            }
            else // Return the length of the list, which includes duplicates.
            {
                return questionsUsed;
            }
        }

        // Gets the number of used questions.
        public int GetQuestionsUsedCount(bool removeRepeats)
        {
            List<int> temp = GetQuestionsUsed(removeRepeats);
            return temp.Count;
        }

        // Gets a list of the question results, which has 'true' for correct answers, and 'false' for incorrect answers.
        public List<bool> GetQuestionResults(bool removeRepeats)
        {
            // Checks if duplicates should be removed.
            if(removeRepeats)
            {
                // The temporary list of questions (will remove duplicates).
                List<int> tempQues = new List<int>();

                // The list of temporary results (will remove results).
                List<bool> tempRes = new List<bool>();

                // Goes through each question used.
                for (int i = 0; i < questionsUsed.Count; i++)
                {
                    // Add the question number to the list if it hasn't been put in there already.
                    if (!tempQues.Contains(questionsUsed[i]))
                    {
                        tempQues.Add(questionsUsed[i]);
                        tempRes.Add(questionResults[i]);
                    }
                }

                // Returns the temporary results.
                return tempRes;
            }
            else // Return the results.
            {
                return questionResults;
            }
        }

        // Gets the number of question results that were correct.
        public int GetQuestionResultsCorrect(bool removeRepeats)
        {
            // The list of results.
            List<bool> tempList = GetQuestionResults(removeRepeats);
            
            // The number of correct answers. 
            int correctCount = 0;

            // Goes through the temporary list to see how many responses were correct.
            foreach (bool result in tempList)
            {
                // If a correct response was found, then add to the counter.
                if(result)
                    correctCount++;
            }

            // Returns the correct count.
            return correctCount;
        }

        // Gets the number of question results that were incorrect.
        public int GetQuestionResultsIncorrect(bool removeRepeats)
        {
            // The list of results.
            List<bool> tempList = GetQuestionResults(removeRepeats);

            // The number of incorrect answers. 
            int incorrectCount = 0;

            // Goes through the temporary list to see how many responses were correct.
            foreach (bool result in tempList)
            {
                // If an incorrect response was found, then add to the counter.
                if (!result)
                    incorrectCount++;
            }

            // Returns the incorrect counter.
            return incorrectCount;
        }



        // Replaces the questions used and results list with the provided used questions and results list.
        // This copies the values, so it doesn't provide the list directly.
        public void ReplaceQuestionsUsedList(List<int> newUsed, List<bool> newResults)
        {
            // If the numbers don't match, the replacement fails.
            if (questionsUsed.Count != questionResults.Count)
            {
                Debug.LogError("The lists must be of the same length. Replacement failed.");
                return;
            }

            // Copies the values form the questions used list.
            questionsUsed.Clear();
            questionsUsed = new List<int>(newUsed);

            // Copies the values from the question results list.
            questionResults.Clear();
            questionResults = new List<bool>(newResults);
        }

        // Replaces the questions used and results list with the provided used questions and results arrays.
        public void ReplaceQuestionsUsedList(int[] newUsed, bool[] newResults)
        {
            // Calls the other function, converting the arrays to lists.
            ReplaceQuestionsUsedList(new List<int>(newUsed), new List<bool>(newResults));
        }




        // Returns 'true' if a question is being asked.
        public bool QuestionIsRunning()
        {
            return running;
        }

        // Returns the answer for the loaded question.
        public string GetAnswer()
        {
            return currentQuestion.GetAnswer();
        }

        // Loads the question into the question manager.
        public void LoadQuestion(GameQuestion question)
        {
            // If the question object should be made inactive when the question is loaded.
            bool makeInactive = false;

            // Note: the changes do not apply if the object is deactivated...
            // So it must be activated first.
            if (!questionObject.activeSelf)
                makeInactive = true;

            // Activates the object so that changes can be made.
            questionObject.SetActive(true);

            // Sets as the current question.
            currentQuestion = question;

            // Sets the question.
            questionText.text = question.question;

            // Sets the response text, and determines if the option is available.
            // Response 0
            response0Text.text = question.Response0;
            response0Button.gameObject.SetActive(question.Response0 != string.Empty);

            // Response 1
            response1Text.text = question.Response1;
            response1Button.gameObject.SetActive(question.Response1 != string.Empty);

            // Response 2
            response2Text.text = question.Response2;
            response2Button.gameObject.SetActive(question.Response2 != string.Empty);

            // Response 3
            response3Text.text = question.Response3;
            response3Button.gameObject.SetActive(question.Response3 != string.Empty);

            {
                // Checks if at least one of the buttons is available.
                bool available = false;

                // Makes an array for this question.
                Button[] responseButtons = new Button[] { response0Button, response1Button, response2Button, response3Button };

                // Checks each button.
                foreach (Button rb in responseButtons)
                {
                    // If one of the buttons is active, stop the check.
                    if (rb.gameObject.activeSelf)
                    {
                        available = true;
                        break;
                    }
                }

                // If no buttons are available, activate all of them.
                if(!available)
                {
                    // Active all the buttons.
                    foreach (Button rb in responseButtons)
                        rb.gameObject.SetActive(true);
                }
            }


            // Deselects the responses.
            DeselectAllResponses();

            // Clear out the two pieces of text.
            evaluationText.text = string.Empty;
            scorePlusText.text = string.Empty;

            // Turns off the buttons.
            confirmButton.interactable = false;
            finishButton.interactable = false;

            // Stop the timer since no question is running yet.
            pausedTimer = true;
            ResetTimer();

            // If the question object should be turned off after the changes are made, turn it off.
            if (makeInactive)
                questionObject.SetActive(false);
        }

        // Loads a random question.
        public void LoadRandomQuestion()
        {
            // Gets given the random queston generated.
            GameQuestion randQuestion;

            // Gets set to 'true' when the random question has been found.
            bool found = false;

            // Amount of attempts taken to generate a new question.
            // int attempts = 0;

            // Clears out the list if the question count has been reached.
            // This doesn't mean every question has been asked, but there are few to no new questions to ask.
            // TODO: remove this.
            if (questionsUsed.Count >= GameQuestions.QUESTION_COUNT)
                questionsUsed.Clear();


            // Old - Do Not Give Asked Question List
            // do
            // {
            //     // Generates the random question.
            //     randQuestion = GameQuestions.Instance.GetRandomQuestion(randomResponseOrder);
            // 
            //     // Checks to see if it's a new question.
            //     if(questionsAsked.Contains(randQuestion.number)) // Already got this question.
            //     {
            //         attempts++;
            //     }
            //     else // New 
            //     {
            //         // Breaks out of the loop.
            //         break;
            //     }
            // 
            // } while (attempts <= 3);

            // Sets this to a new question by default since it caused complier issues.
            // If this go correctly, this generated question shouldn't be used.
            randQuestion = GameQuestions.Instance.GetRandomQuestion(randomResponseOrder);

            // New - Makes sure to only get new questions.
            if (redoQuestionIfWrong) // Redo prior question.
            {
                // If the prior question should be redone, and there is a prior question set.
                if(redoPriorQuestion && priorQuestion != -1)
                {
                    // For some reason, simply saving hte current question again didn't work.

                    // // If the current question number is equal to the prior question, just re-ask this question.
                    // if(currentQuestion.number == priorQuestion)
                    // {
                    //     // Reuse the question.
                    //     randQuestion = currentQuestion;
                    //     found = true;
                    // }
                    // else // Try to find the question again.
                    // {
                    //     // Try to find the prior question.
                    //     randQuestion = GameQuestions.Instance.GetQuestion(priorQuestion);
                    //     
                    //     // If the numbers match, that means the question was found, which would make this true.
                    //     found = (randQuestion.number == currentQuestion.number);
                    // }

                    // Try to find the prior question.
                    randQuestion = GameQuestions.Instance.GetQuestion(priorQuestion);

                    // If the numbers match, that means the question was found, which would make this true.
                    found = (randQuestion.number == currentQuestion.number);
                }
            }          

            // Load up a new question if none have been found yet.
            if(!found)
                randQuestion = GameQuestions.Instance.GetRandomQuestion(questionsUsed, randomResponseOrder);

            // Loads the question.
            LoadQuestion(randQuestion);
        }

        // Clears the question.
        public void ClearQuestion()
        {
            // Clears out the question and its speak key.s
            currentQuestion.question = string.Empty;
            currentQuestion.questionSpeakKey = string.Empty;

            // Clears out the responses.
            for(int i = 0; i < currentQuestion.responses.Length; i++)
                currentQuestion.responses[i] = string.Empty;

            // Sets the answer index to 0.
            currentQuestion.answerIndex = 0;
        }

        // Resets the timer to its max. This does not check if the timer is paused or unpaused.
        private void ResetTimer()
        {
            // Checks if extra time should be added.
            // Extra time won't be added if TTS is not being used.
            if(addExtraTime && LOLSDK.Instance.IsInitialized && GameSettings.Instance.UseTextToSpeech)
            {
                timer = startTime + ttsExtraTime;
            }
            else
            {
                timer = startTime;
            }
        }

        // Asks the loaded question.
        public void AskQuestion()
        {
            // Open the prompt to make sure all the changes are applied.
            questionObject.SetActive(true);

            // The question is running now, and no response has been given yet.
            running = true;
            responded = false;
            selectedResponse = -1;

            // A question has been asked, so add to the list.
            questionsUsed.Add(currentQuestion.number);

            // TODO: implement this. You need to differentiate the asked questions list from the question count.
            // Checks if the question has been asked before.
            // if(questionsAsked.Contains(CurrentQuestion.number)) // Asked before.
            // {
            // 
            // }
            // else // Not asked before.
            // {
            //     
            // }

            // Deselect all of the responses since a question is now being asked.
            DeselectAllResponses();

            // Start the timer.
            ResetTimer();
            pausedTimer = false;

            // Hide the stat changes.
            statChanges.SetActive(false);

            // Plays the bgm for the game question.
            if(playAudio)
            {
                // Plays the question BGM.
                gameManager.overworld.PlayQuestionBgm();
            }

            // Call to signify that a question has been asked.
            gameManager.OnQuestionStart();

            // Reads out the question.
            // If the LOLSDK is initialized.
            if(LOLSDK.Instance.IsInitialized)
            {
                // Use the text-to-speech, and the speak key is set.
                if (GameSettings.Instance.UseTextToSpeech && currentQuestion.questionSpeakKey != string.Empty)
                    LOLManager.Instance.textToSpeech.SpeakText(currentQuestion.questionSpeakKey);
            }
        }

        // Ask the new question, loading said question into the game.
        public void AskQuestion(GameQuestion newQuestion)
        {
            LoadQuestion(newQuestion);
            AskQuestion();
        }

        // Loads a random question and then asks it.
        public void AskRandomQuestion()
        {
            LoadRandomQuestion();
            AskQuestion();
        }

        // Deselects all the other responses.
        public void DeselectAllResponses()
        {
            // Turn off the other responses.
            response0Fill.SetActive(false);
            response0Bubble.color = unansweredColor;

            response1Fill.SetActive(false);
            response1Bubble.color = unansweredColor;

            response2Fill.SetActive(false);
            response2Bubble.color = unansweredColor;

            response3Fill.SetActive(false);
            response3Bubble.color = unansweredColor;
        }

        // Selects a response.
        private void SelectResponse(int response)
        {
            // Deselects the other responses.
            DeselectAllResponses();

            // Selects the response.
            selectedResponse = Mathf.Clamp(response, 0, GameQuestions.QUESTION_OPTIONS_MAX - 1);
            
            // Checks which response to select.
            switch (selectedResponse)
            {
                default:
                case 0:
                    response0Fill.SetActive(true); 
                    break;
                case 1:
                    response1Fill.SetActive(true);
                    break;
                case 2:
                    response2Fill.SetActive(true);
                    break;
                case 3:
                    response3Fill.SetActive(true);
                    break;

            }

            // Make the button interactable.
            if (!confirmButton.interactable)
                confirmButton.interactable = true;
        }

        // Select response 0.
        public void SelectResponse0()
        {
            SelectResponse(0);
        }

        // Select response 1.
        public void SelectResponse1()
        {
            SelectResponse(1);
        }

        // Select response 2.
        public void SelectResponse2()
        {
            SelectResponse(2);
        }

        // Select response 3.
        public void SelectResponse3()
        {
            SelectResponse(3);
        }

        // Disables all the response buttons.
        public void DisableAllResponseButtons()
        {
            response0Button.interactable = false;
            response1Button.interactable = false;
            response2Button.interactable = false;
            response3Button.interactable = false;
        }

        // Enables all the response buttons.
        public void EnableAllResponseButtons()
        {
            response0Button.interactable = true;
            response1Button.interactable = true;
            response2Button.interactable = true;
            response3Button.interactable = true;
        }

        // Confirms the user's inputted response.
        public void ConfirmResponse()
        {
            // The question has been responded to.
            responded = true;

            // Checks if the response is correct.
            bool correct = currentQuestion.CorrectAnswer(selectedResponse);

            // Shows the evaluation text.
            evaluationText.text = (correct) ? correctString : incorrectString;

            // The maximum score addition to be made.
            int scorePlus = 0;

            // Checks if the user got the question right or not.
            if(correct)
            {
                // Pause the timer, and increment the question correct variable.
                pausedTimer = true;

                // Adjust score plus.
                scorePlus = Mathf.RoundToInt(maxScorePlus * (timer / reduceRewardTime));
                scorePlus = Mathf.Clamp(scorePlus, minScorePlus, maxScorePlus);

                // Plays the question answer correct SFX.
                if(playAudio)
                    gameManager.overworld.PlayQuestionCorrectSfx();
            }
            else
            {
                // Subtracts 100 points from the player.
                scorePlus = -100;

                // Plays the question answer incorrect SFX.
                if (playAudio)
                    gameManager.overworld.PlayQuestionIncorrectSfx();
            }

            // Add the result to the results list.
            questionResults.Add(correct);

            // Reads out evaluation response.
            // If the LOLSDK is initialized.
            if (LOLSDK.Instance.IsInitialized)
            {
                // Use the text-to-speech, and the speak keys are set.
                // Checks both keys at the same time. There'd be no point in speaking one without the other being available.
                if (GameSettings.Instance.UseTextToSpeech && correctKey != string.Empty && incorrectKey != string.Empty)
                {
                    LOLManager.Instance.textToSpeech.SpeakText((correct) ? correctKey : incorrectKey);
                }
            }


            // If the answer should always be shown, highlight the correct and incorrect answers.
            {
                // Makes an array for recoloring the bubbles.
                Image[] responseButtonBubbles = new Image[] 
                { response0Bubble, response1Bubble, response2Bubble, response3Bubble };

                // If the answer should always be shown.
                if (alwaysShowAnswer)
                {
                    // Checks each button.
                    for (int i = 0; i < responseButtonBubbles.Length; i++)
                    {
                        // Grabs the response button bubble.
                        Image rbb = responseButtonBubbles[i];

                        // Change the bubble colours.
                        rbb.color = (currentQuestion.CorrectAnswer(i)) ? correctColor : incorrectColor;
                    }
                }
                else
                {
                    // Reset all the colours.
                    foreach (Image rbb in responseButtonBubbles)
                        rbb.color = unansweredColor;

                    // Only confirm/deny the answer the player provided.
                    if(selectedResponse >= 0 && selectedResponse < responseButtonBubbles.Length)
                    {
                        // Grabs the bubble of the user's selected response.
                        Image rbb = responseButtonBubbles[selectedResponse];

                        // Sets the colour of the response.
                        rbb.color = (currentQuestion.CorrectAnswer(selectedResponse)) ? correctColor : incorrectColor;
                    }

                }
                    
            }

            // Add to the score, and display it.
            // Add score plus to game score.
            gameManager.score += scorePlus;

            // If the score is now negative, set it to 0.
            if (gameManager.score < 0)
                gameManager.score = 0;

            // Updates the UI to display the new score.
            gameManager.overworld.UpdateUI(); 

            // Updates the score text.
            scorePlusText.text = (scorePlus != 0) ? scorePlus.ToString("+#;-#;0") : "-";

            // Saves this as the prior question now that it has been answered.
            priorQuestion = currentQuestion.number;

            // Sets this variable to see if the prior question should be redone or not.
            redoPriorQuestion = !correct;

            // Show the stat changes.
            statChanges.SetActive(true);

            // TODO: implement the stat changes.

            // Response locked in.
            confirmButton.interactable = false;

            // Question can now be finished/closed.
            finishButton.interactable = true;

            // Plays the overworld BGM.
            gameManager.overworld.PlayOverworldBgm();
        }

        // End the question.
        public void FinishQuestion()
        {
            // The question is no longer running, and thus no response has been given.
            running = false;
            responded = false;

            // Clear out timer.
            pausedTimer = true;
            timer = 0.0F;

            // Set selected response to default.
            selectedResponse = -1;

            // Deselect all the responses just to be safe.
            DeselectAllResponses();

            // Clears out the question. This isn't required, but it helps indicate that no question is loaded.
            ClearQuestion();

            // Turn off the question object.
            questionObject.SetActive(false);

            // Call to signify that a question has been ended.
            gameManager.OnQuestionEnd();
        }

        // Resets the question history.
        public void ResetQuestionHistory()
        {
            // Clears out the questionsAsked list.
            questionsUsed.Clear();

            // Clears out the questions asked and question results count.
            questionsUsed.Clear();
            questionResults.Clear();

            // Removes the prior question from the history, and says not to use it.
            priorQuestion = -1;
            redoPriorQuestion= false;
        }

        // Disables the question (prevents all interaction from the user).
        public void DisableQuestion()
        {
            // Disable the responses (some may not be visible anyway).
            DisableAllResponseButtons();

            // Disable the lock-in options.
            confirmButton.interactable = false;
            finishButton.interactable = false;

            // Pause the timer.
            pausedTimer = true;
        }

        // Enables the question (enables interaction from the user).
        public void EnableQuestion()
        {
            // Enable the responses (some may not be visible anyway).
            EnableAllResponseButtons();

            // Checks if the user has responded to the question.
            if(responded)
            {
                // If the user responded, only the finish button should be enabled.
                confirmButton.interactable = false;
                finishButton.interactable = true;
            }    
            else
            {
                // If the user hasn't responded yet, only the confirm button should be enabled.
                // If the player hasn't selected a response yet, don't make the confirm button interactable.
                confirmButton.interactable = (selectedResponse >= 0);
                finishButton.interactable = false;

                // Unpause the timer if the question is running.
                if (running)
                    pausedTimer = false;
            }
        }

        // Update is called once per frame
        void Update()
        {
            // If the timer isn't paused, count down.
            if (!pausedTimer)
            {
                // Reduce the timer.
                timer -= Time.deltaTime;

                // Timer is less than 0, so set it to 0.
                if (timer <= 0.0F)
                    timer = 0.0F;

                // Don't count down anymore.
                if (timer == 0.0F)
                    pausedTimer = true;

            }

            // Plays the question BGM if it isn't currently playing.
            // This is to address an issue where the overworld audio was still playing.
            if(playAudio)
            {
                // The question BGM should stop once the player responds so that the overworld BGM can play.
                // It also shouldn't play unless the question is running.
                // This is wonky, but it's a workaround I had to do because of the BGM being overwritten elsewhere.
                if(running && !responded)
                {
                    // Checks if the right audio clip is playing.
                    if (gameManager.audioManager.bgmSource.clip != gameManager.overworld.questionBgm)
                        gameManager.audioManager.PlayBackgroundMusic(gameManager.overworld.questionBgm);
                }
            }
                
        }
    }
}