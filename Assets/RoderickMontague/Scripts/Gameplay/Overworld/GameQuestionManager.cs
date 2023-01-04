using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using SimpleJSON;
using System.ComponentModel.Design.Serialization;

// Namespace.
namespace RM_BBTS
{
    // The system for posing questions in the overworld.
    public class GameQuestionManager : MonoBehaviour
    {
        // The gameplay manager.
        public GameplayManager gameManager;


        // The object of the question window.
        public GameObject questionObject;

        // The title text of the question window.
        public TMP_Text titleText;

        [Header("Question Info")]
        // The question text.
        public TMP_Text questionText;

        // The loaded question.
        public GameQuestion currentQuestion;

        // The standard game questions.
        private GameQuestions gameQuestions;

        
        // The button text for the four responses.
        // Response 0
        public Button response0Button;
        public GameObject response0Fill;
        public TMP_Text response0Text;

        // Response 1
        public Button response1Button;
        public GameObject response1Fill;
        public TMP_Text response1Text;

        // Respone 2
        public Button response2Button;
        public GameObject response2Fill;
        public TMP_Text response2Text;

        // Response 3
        public Button response3Button;
        public GameObject response3Fill;
        public TMP_Text response3Text;

        // The list of response button.
        private Button[] responseButtons;

        // The number of the selected response.
        private int selectedResponse = -1;

        // The text for evaluation.
        public TMP_Text evaluationText;

        // The text for the score addition.
        public TMP_Text scorePlusText;

        [Header("Score Plus/Time")]
        // The maximum and minimum for adding to the player's score.
        public int maxScorePlus = 100;
        public int minScorePlus = 5;

        // Gets set to 'true' when the timer is paused.
        public bool pausedTimer = true;

        // What the timer starts at.
        public float startTime = 12.0F;

        // When the time falls below this value, the reward is reduced.
        public float reduceRewardTime = 10.0F;

        // The timer for the game.
        public float timer = 0.0F;

        [Header("Evaluation")]
        // The confirm button.
        public Button confirmButton;

        // The next button.
        public Button finishButton;

        // The correct and incorrect string.
        private string correctString = "[Correct]";
        private string incorrectString = "[Incorrect]";

        // Start is called before the first frame update
        void Start()
        {
            gameQuestions = GameQuestions.Instance;

            // Puts the responses in a list.
            responseButtons = new Button[] { response0Button, response1Button, response2Button, response3Button };

            // Load question 0 to reset the question manager.
            LoadQuestion(gameQuestions.GetQuestion(0));

            // Translation.
            JSONNode defs = SharedState.LanguageDefs;

            if(defs != null)
            {
                // correctString = defs["kwd_correct"];
                // incorrectString = defs["kwd_incorrect"];


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

        // Returns the answer for the loaded question.
        public string GetAnswer()
        {
            return currentQuestion.GetAnswer();
        }

        // Loads the question into the question manager.
        public void LoadQuestion(GameQuestion question)
        {
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

                // Checks each button.
                foreach(Button rb in responseButtons)
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
            timer = startTime;
        }

        // Loads a random question.
        public void LoadRandomQuestion()
        {
            // TODO: make sure that the same question can't be returned.

            LoadQuestion(gameQuestions.GetRandomQuestion(true));
        }

        // Asks the loaded question.
        public void AskQuestion()
        {
            // Open the prompt.
            questionObject.SetActive(true);

            // Start the timer.
            timer = startTime;
            pausedTimer = false;
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
            response1Fill.SetActive(false);
            response2Fill.SetActive(false);
            response3Fill.SetActive(false);
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

        // Confirms the user's inputted response.
        public void ConfirmResponse()
        {
            // Gets the correct response.
            bool correct = currentQuestion.CorrectAnswer(selectedResponse);

            // Shows the evaluation text.
            evaluationText.text = (correct) ? correctString : incorrectString;

            // The maximum score addition to be made.
            int scorePlus = 0;

            // Checks if the user got the question right or not.
            if(correct)
            {
                // Pause the timer and adjust the score plus.
                pausedTimer = true;
                scorePlus = Mathf.RoundToInt(maxScorePlus * (timer / reduceRewardTime));
                scorePlus = Mathf.Clamp(scorePlus, minScorePlus, maxScorePlus);
            }
            else
            {
                // TODO: should the player lose points for getting the question wrong?
                scorePlus = 0;
            }

            // Add to the score, and display it.
            gameManager.score += scorePlus;
            scorePlusText.text = (scorePlus > 0) ? scorePlus.ToString("#+;#-;0") : "-";

            // Response locked in.
            confirmButton.interactable = false;

            // Question can now be finished/closed.
            finishButton.interactable = true;
        }

        // End the question.
        public void FinishQuestion()
        {
            // Turn off the question object.
            questionObject.SetActive(!questionObject.activeSelf);
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
                
        }
    }
}