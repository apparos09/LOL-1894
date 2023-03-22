using LoLSDK;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace RM_BBTS
{
    // The clas sfor the text box for the game.
    public class TextBox : MonoBehaviour
    {
        // the text box object to be opened/closed.
        public GameObject boxObject;

        // The current page.
        private int currPageIndex = -1;

        // List of pages.
        public List<Page> pages = new List<Page>();

        // The text in the text box.
        public TMP_Text boxText;

        // If enabled, the program will automatically go to the next page once it is loaded.
        public bool autoNext = false;

        // The max time it takes for a box to automatically turn to the next page.
        public float autoNextTimerMax = 5.0F;

        // The timer for automatically going to the next page.
        public float autoNextTimer = 0.0F;

        // Extra time added when TTS is enabled.
        public float ttsExtraTime = 1.0F;

        // Allows for extra time to be added to the timer when TTS is active.
        public bool addTtsExtraTime = true;

        // Set to 'true' to pause the timer.
        public bool autoNextTimerPaused = false;

        // Closes the text box when all the end has been reached.
        public bool closeOnEnd = true;

        [Header("UI")]

        // The previous page button.
        public Button prevPageButton;

        // The next page button.
        public Button nextPageButton;

        // If set to 'true', the back button gets disabled if the textbox is on the first page.
        public bool autoDisablePrevButtonOnFirstPage = false;

        // Animation clips were taken out, since animation is done entirely by char loading.

        [Header("Animation")]
        // If 'true', all the shown is shown at once. If false, the text is shown letter by letter.
        public bool instantText = true;

        // Allows the user to skip the text loading animation if this is set to 'true'.
        [Tooltip("If true, the user can skip to the next page before all the text is loaded. The back skip still works by default unless enableAnimationBackSkip is set to false.")]
        public bool enableAnimationSkip = true;

        // Allows the user to skip the text loading animation if they're going back a page.
        [Tooltip("If true, the user can go back a page before all the text is loaded. This only applies if 'enableAnimationSkip' is false.")]
        public bool enableAnimationBackSkip = true;

        // If set to 'true', the controls are hidden when the text is loading.
        [Tooltip("If the controls shouldn't be enabled when the animation skip is disabled. The back button won't be disabled unless enableAnimationBackSkip is set to 'false'.")]
        public bool DisableControlsIfAnimSkipDisabled = true;

        // A queue of text for progressive character loading.
        private Queue<char> charQueue = new Queue<char>();

        // The timer for loading in a new char.
        private float charTimer = 0.0F;

        // The amount of characters loaded per interation.
        public int charsPerLoad = 1;

        // The speed that the text is shown on the screen. This is ignored if the text is instantly shown.
        public float textSpeed = 0.0F;

        // Becomes 'true' when characters are loading up.
        private bool loadingChars = false;

        // CALLBACKS
        // a callback for when all the text has been gone through.
        public delegate void TextBoxCallback();

        // Callback for the textbox being opened.
        private TextBoxCallback openedCallback;

        // Callback for the textbox being closed.
        private TextBoxCallback closedCallback;

        // Callback for the textbox finishing.
        private TextBoxCallback doneCallback;

        // Start is called before the first frame update
        void Start()
        {
            // Sets the box object to the game object.
            // if (boxObject == null)
            //     boxObject = gameObject;

            // Set this to the max by default.
            SetAutoNextTimerToMax();

            // Recolour the text to show that the text loaded is not coming from the language file.
            if (!LOLSDK.Instance.IsInitialized)
                LanguageMarker.Instance.MarkText(boxText);
        }

        // TODO: add touch and mouse for going onto the next page.

        // The current page index.
        public int CurrentPageIndex
        {
            get
            {
                return currPageIndex;
            }

            set
            {
                SetPage(value);
            }
        }


        // Returns the current page.
        public Page CurrentPage
        {
            get
            {
                if (currPageIndex >= 0 && currPageIndex < pages.Count)
                    return pages[currPageIndex];
                else
                    return null;
            }
        }

        // Checks to see if the current page index is in bounds.
        public bool ValidCurrentPageIndex()
        {
            return currPageIndex >= 0 && currPageIndex < pages.Count;
        }

        // Adds a callback for when the textbox opens.
        public void OnTextBoxOpenedAddCallback(TextBoxCallback callback)
        {
            openedCallback += callback;
        }

        // Removes a callback for when the textbox opens.
        public void OnTextBoxOpenedRemoveCallback(TextBoxCallback callback)
        {
            openedCallback -= callback;
        }

        // Shows the textbox.
        public void Open()
        {
            // TODO: add animation.
            boxObject.SetActive(true);

            // Calls the callbacks for opening the textbox.
            if (openedCallback != null)
                openedCallback();

            // Reset the timer.
            SetAutoNextTimerToMax();
        }

        // Adds a callback for when the textbox is closed.
        public void OnTextBoxClosedAddCallback(TextBoxCallback callback)
        {
            closedCallback += callback;
        }

        // Removes a callback for when the textbox is closed.
        public void OnTextBoxClosedRemoveCallback(TextBoxCallback callback)
        {
            closedCallback -= callback;
        }

        // Hides the textbox.
        public void Close()
        {
            // TODO: add animation.
            boxObject.SetActive(false);

            // Calls the callbacks for closing the textbox.
            if (closedCallback != null)
                closedCallback();


            // Tells the page to stop reading the text if it is being read.
            // Since the TTS is overwritten when a new page is opened...
            // This is only called when the textbox is being closed.
            // NOTE: for some reason the current page index was out of range sometimes, so this is a quick fix.
            if(currPageIndex >= 0 && currPageIndex < pages.Count)
                pages[currPageIndex].StopSpeakingPage();
        }

        // Shows the textbox. This does NOT call the Open callbacks.
        public void Show()
        {
            boxObject.SetActive(true);

            // Reset the timer.
            SetAutoNextTimerToMax();
        }

        // Hides the textbox. This does Not call the Close callbacks.
        public void Hide()
        {
            boxObject.SetActive(false);
        }

        // Checks if the textbox is visible.
        public bool IsVisible()
        {
            return boxObject.activeSelf;
        }

        // Gets the page count.
        public int GetPageCount()
        {
            return pages.Count;
        }


        // TODO: is this necessary?
        // Changes the page index.
        public void SetPage(int index)
        {
            SetTextBoxText(index, false);
        }

        // Moves onto the next page.
        public void NextPage()
        {
            // Increases the index, and sets the text.
            SetTextBoxText(currPageIndex + 1);
        }

        // Returns to the previous page.
        public void PreviousPage()
        {
            // If the animation should be skipped when going back, and only when going back.
            if (enableAnimationBackSkip && !enableAnimationSkip)
            {
                // Finishes the page instead of finishing it.
                SetTextBoxText(currPageIndex - 1, false);
            }
            else // Standard
            {
                // TODO: maybe have the back button skip the text always instead of loading up the rest of the page?

                // Decreases the index, and sets the text.
                // SetTextBoxText(currPageIndex - 1);
                SetTextBoxText(currPageIndex - 1);
            }
        }

        // CONTROLS //

        // Enables/disables the textbox controls.
        public void SetTextBoxControls(bool interactable)
        {
            // Enables/disables the prev page button.
            if (prevPageButton != null)
                prevPageButton.interactable = interactable;

            // Enables/disables the next page button.
            if (nextPageButton != null)
                nextPageButton.interactable = interactable;


            // // Checks if auto next is enabled.
            // if(autoNext)
            // {
            //     // Pause the auto next timer
            //     if (stopAutoTimerOnDisable && !interactable)
            //         autoNextTimerPaused = true;
            // }
        }

        // Enables the text box controls.
        public void EnableTextBoxControls()
        {
            SetTextBoxControls(true);
        }

        // Disables the text box controls.
        // If 'stopAutoTimer' is true, the text box's auto page turn timer is stopped.
        public void DisableTextBoxControls()
        {
            SetTextBoxControls(false);
        }

        // Enables the previous button.
        public void EnablePreviousButton()
        {
            // Enables the prev page button.
            if (prevPageButton != null)
                prevPageButton.interactable = true;
        }

        // Disables the previous button.
        public void DisablePreviousButton()
        {
            // Disables the prev page button.
            if (prevPageButton != null)
                prevPageButton.interactable = false;
        }

        // Disables the previous page button if on the first page.
        public void DisablePreviousButtonOnFirstPage()
        {
            // Checks for the previous page button being set.
            if (prevPageButton != null)
            {
                // Checks if the button should be enabled.
                bool enableButton = currPageIndex != 0;

                // Change the button interaction setting if it doesn't match.
                if (prevPageButton.interactable != enableButton)
                    prevPageButton.interactable = enableButton;
            }
        }

        // Enables the next button.
        public void EnableNextButton()
        {
            // Enables the next page button.
            if (nextPageButton != null)
                nextPageButton.interactable = true;
        }

        // Disables the previous button.
        public void DisableNextButton()
        {
            // Disables the next page button.
            if (nextPageButton != null)
                nextPageButton.interactable = false;
        }


        // SET PAGES //

        // Adds a page to the end of the pages list.
        public void AddPage(Page page)
        {
            pages.Add(page);
        }

        // Inserts a page at the provided index.
        public void InsertPage(Page page, int index)
        {
            // If the current page index is greater than the provided index...
            // Increase it by '1' so that it matches up with the current page.
            if (currPageIndex >= index)
                currPageIndex++;

            pages.Insert(index, page);
        }

        // Inserts a page after the current page.
        public void InsertAfterCurrentPage(Page page)
        {
            // Checks for validity. If this check fails, the page is added to the end of the list.
            if(currPageIndex >= 0 && currPageIndex < pages.Count)
            {
                pages.Insert(currPageIndex + 1, page);
            }
            else
            {
                pages.Add(page);
            }
        }

        // Replaces the pages from the list with the new pages.
        public void ReplacePages(List<Page> newPages)
        {
            // Replaces the pages and sets the page.
            pages.Clear();

            // Pages to add.
            if(newPages.Count > 0)
            {
                pages = new List<Page>(newPages);

                // Sets the page.
                SetPage(0);
            }
            else
            {
                // No current pages.
                currPageIndex = -1;
            }
            
        }

        // Replaces the pages from the list with the new pages.
        public void ReplacePages(List<string> newPages)
        {
            // Clears the page list.
            pages.Clear();

            // Replaces the pages and sets the page.
            foreach(string text in newPages)
            {
                pages.Add(new Page(text));
            }

            // Sets the current page.
            SetPage(0);
        }

        // Sets the text that's on the text box.
        private void SetTextBoxText(int nextPageIndex, bool finishPage = true)
        {
            // If text is still being loaded just sub in the rest and stop loading in new characters.
            if (loadingChars)
            {
                // If the animation skip has been enabled.
                if(enableAnimationSkip)
                {
                    // No longer loading characters.
                    loadingChars = false;

                    // If the text should be finished, or if it should just skip to the next page.
                    // If there is no next page then the rest of the text is loaded regardless of 'finishPage's value.
                    // TODO: maybe make it so that a callback is called when the dialog box is finished.

                    if (finishPage || !(nextPageIndex >= 0 && nextPageIndex < pages.Count)) // Finish loading the page.
                    {
                        // Finishes the text instead of replacing the page.
                        boxText.text = pages[currPageIndex].text;
                        charQueue.Clear();
                        charTimer = 0.0F;
                        return;
                    }
                }
                // Skipping is not allowed, so just leave the function.
                else
                {
                    // If the page should be finished first, leave the function so that the page can be completed.
                    if(finishPage)
                        return;
                }   
            }

            // There's no next page, so don't change the text.
            if (nextPageIndex >= pages.Count || nextPageIndex < 0)
            {
                // The textbox is about to be closed, so call the 'on page closed' callback for the last page.
                // There were some index out of bounds errors here, and I don't know why.
                if(currPageIndex >= 0 && currPageIndex < pages.Count)
                    pages[currPageIndex].OnPageClosed();

                // The text has all been displayed, so call the callbacks.
                if (nextPageIndex >= pages.Count)
                    OnTextBoxFinished();

                return;
            }



            // Clears out the existing text.
            boxText.text = "";

            // Calls the function for the page that's being closed.
            // If the new index is the same as the current index, don't call the OnPageClosed() function.
            // It will just treat the page as never closing at all.
            if(currPageIndex >= 0 && currPageIndex < pages.Count && currPageIndex != nextPageIndex)
                pages[currPageIndex].OnPageClosed();

            // Sets the new page index.
            currPageIndex = nextPageIndex;

            // Bounds correction for the page.
            // Current Page Index is set to -1 if there are no pages.
            if (pages.Count > 0)
            {
                currPageIndex = Mathf.Clamp(currPageIndex, 0, pages.Count - 1);
            }
            else
            {
                currPageIndex = -1;
                return;
            }


            // Calls the 'open' function on the new page.
            pages[currPageIndex].OnPageOpened();


            // If the previous button should be automatically disabled on the first page, try to disable it.
            if(autoDisablePrevButtonOnFirstPage)
                DisablePreviousButtonOnFirstPage();


            // A bounds check is done again to make sure that the pages weren't cleared in a callback.
            // This was to address an error that was being encountered.
            if (currPageIndex >= 0 && currPageIndex < pages.Count)
            {
                // Checks if the text should be shown automatically, or if it should be shown letter by letter.
                if (instantText) // Instant
                {
                    // Sets the box text.
                    boxText.text = pages[currPageIndex].text;
                }
                else // Letter by Letter
                {
                    // Set to load characters, and loads up the char queue.
                    loadingChars = true;
                    charQueue.Clear();
                    charQueue = new Queue<char>(pages[currPageIndex].text);
                    charTimer = 0.0F;


                    // If the textbox controls should be disabled when the animation skip is turned off.
                    if (!enableAnimationSkip && DisableControlsIfAnimSkipDisabled)
                    {
                        // Disables the next button.
                        DisableNextButton();

                        // Disables the previous button.
                        if (!enableAnimationBackSkip)
                            DisablePreviousButton();
                    }
                }
            }
            else
            {
                boxText.text = string.Empty;
                // TODO: close textbox?
            }

            // Sets the auto next timer to max. If autoNext is set to false, the timer won't decrease anyway.
            SetAutoNextTimerToMax();
        }

        // Loads character by character.
        private void LoadCharacterByCharacter()
        {
            // Checks if the timer has reached 0 for displaying the next character.
            if (charQueue.Count != 0)
            {
                // If the timer has reached 0 or less.
                if (charTimer <= 0.0F)
                {
                    // Adds to the string.
                    string temp = boxText.text;

                    // Loads (X) amount of characters based on 'charsPerLoad'.
                    // Stops if the char queue runs out of characters.
                    for(int i = 0; i < charsPerLoad && charQueue.Count > 0; i++)
                    {
                        temp += charQueue.Dequeue();
                    }
                    
                    boxText.text = temp;


                    // NOTE: why did I divide by text speed instead of applying it to the timer itself?

                    // // If the text speed is set to 0 the new char will load on the next frame.
                    // // NOTE: past a certain point, the char gets put every frame, which means there's a limit to the text speed.
                    // if (textSpeed > 0)
                    //     charTimer = 1.0F / textSpeed;
                    // else
                    //     charTimer = 0.0F;

                    // Reset the value.
                    // If textSpeed is set to '0', then a character is loaded every frame.
                    if (textSpeed > 0)
                        charTimer = 1.0F;
                    else
                        charTimer = 0.0F;
                }
                else // Reduce timer.
                {
                    charTimer -= Time.deltaTime * textSpeed;
                }
            }
            else
            {
                // No characters to load.
                loadingChars = false;
                charTimer = 0.0F;

                // If the text box should automatically go onto the next page when it's done after a certain period of time...
                // Set the timer.
                if (autoNext)
                    SetAutoNextTimerToMax();


                // If the textbox controls should be disabled when the animation skip is turned off.
                if (!enableAnimationSkip && DisableControlsIfAnimSkipDisabled)
                {
                    // Enable the controls.
                    EnableTextBoxControls();
                }

                // If the previous button should be disabled on the first page, attempt to disable it.
                // This is put after the auto skip settings.
                if (autoDisablePrevButtonOnFirstPage)
                {
                    DisablePreviousButtonOnFirstPage();
                }
            }
        }

        // Sets the timer to its max.
        public void SetAutoNextTimerToMax()
        {
            // Set the time to the max.
            autoNextTimer = autoNextTimerMax;

            // Adds the extra time for TTS reading.
            if (addTtsExtraTime && LOLSDK.Instance.IsInitialized && GameSettings.Instance.UseTextToSpeech)
                autoNextTimer += ttsExtraTime;
        }

        
        // A callback function for when all the text is finished.
        // This is only called if the user attempts to go onto the next page when there is none.
        public void OnTextBoxFinishedAddCallback(TextBoxCallback callback)
        {
            doneCallback += callback;
        }

        // Removes the callback.
        public void OnTextBoxFinishedRemoveCallback(TextBoxCallback callback)
        {
            doneCallback -= callback;
        }

        // Called when all the text has been displayed.
        private void OnTextBoxFinished()
        {
            // Checks if there are functions to call.
            if (doneCallback != null)
                doneCallback();

            // If the text box should be closed when it's done.
            if (closeOnEnd)
                Close();
        }

        // Clears out all pages.
        // It's recommended that this function is called instead of clearing the page list inself.
        // There are extra elements that are cleared this function are called, so use this instead of pages.Clear().
        public void ClearPages()
        {
            // Clears out the pages.
            pages.Clear();
            pages = new List<Page>();

            // Now at index 0.
            currPageIndex = 0;

            // Clear out waiting
            loadingChars = false;
            boxText.text = string.Empty;
            charQueue.Clear();
            charTimer = 0.0F;
        }

        // Update is called once per frame
        void Update()
        {
            // Changed this from the courtine version.
            if (loadingChars)
                LoadCharacterByCharacter();


            // If the page should automatically change.
            if(autoNext)
            {
                // If the timer is not finished yet, reduce the time.
                // Don't do it if the timer is paused.
                if(autoNextTimer > 0.0F && !autoNextTimerPaused)
                {
                    autoNextTimer -= Time.deltaTime;

                    // If the timer is now finished, turn the page.
                    if(autoNextTimer <= 0.0F)
                    {
                        // Set the timer to 0.
                        autoNextTimer = 0.0F;

                        // Moves onto the next page.
                        NextPage();
                    }
                }
            }
        }
    }
}