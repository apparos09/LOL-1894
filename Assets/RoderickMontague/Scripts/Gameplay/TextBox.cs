using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
        public TMPro.TMP_Text boxText;

        // Closes the text box when all the end has been reached.
        public bool closeOnEnd = true;

        [Header("UI")]

        // The previous page button.
        public Button prevPageButton;

        // The next page button.
        public Button nextPageButton;

        [Header("Animation")]
        // The animator.
        public Animator animator;

        // Animation for opening the textbox.
        public AnimationClip openClip;

        // Animation for closing the textbox.
        public AnimationClip closeClip;

        [Header("Animation/Text")]
        // If 'true', all the shown is shown at once. If false, the text is shown letter by letter.
        public bool instantText = true;

        // A queue of text for progressive character loading.
        private Queue<char> charQueue = new Queue<char>();

        // The timer for loading in a new char.
        private float charTimer = 0.0F;

        // The speed that the text is shown on the screen. This is ignored if the text is instantly shown.
        public float textSpeed = 10.0F;

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
            // Decreases the index, and sets the text.
            SetTextBoxText(currPageIndex - 1);
        }

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
            // TODO: account for glitch with an index out of bounds error with the pages.

            // If text is still being loaded just sub in the rest and stop loading in new characters.
            if (loadingChars)
            {
                // No longer loading characters.
                loadingChars = false;

                // If the text should be finished, or if it should just skip to the next page.
                // If there is no next page then the rest of the text is loaded regardless of 'finishPage's value.
                // TODO: maybe make it so that a callback is called when the dialog box is finished.

                if (finishPage || !(nextPageIndex >= 0 && nextPageIndex < pages.Count)) // Finsih loading the page.
                {
                    // Finishes the text instead of replacing the page.
                    boxText.text = pages[currPageIndex].text;
                    charQueue.Clear();
                    charTimer = 0.0F;
                    return;
                }
            }

            // There's no next page, so don't change the text.
            if (nextPageIndex >= pages.Count || nextPageIndex < 0)
            {
                // The textbox is about to be closed, so call the 'on page closed' callback for the last page.
                pages[currPageIndex].OnPageClosed();

                // The text has all been displayed, so call the callbacks.
                if (nextPageIndex >= pages.Count)
                    OnTextBoxFinished();

                return;
            }



            // Clears out the existing text.
            boxText.text = "";

            // Calls the function for the page that's being closed.
            if(currPageIndex >= 0 && currPageIndex < pages.Count)
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

            // A bounds check is done again to make sure that the pages weren't cleared in a callback.
            // This was to address an error that was being encountered.
            if(currPageIndex >= 0 && currPageIndex < pages.Count)
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
                }
            }
            else
            {
                boxText.text = string.Empty;
                // TODO: close textbox?
            }

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
                    temp += charQueue.Dequeue();
                    boxText.text = temp;

                    // If the text speed is set to 0 the new char will load on the next frame.
                    // NOTE: past a certain point, the char gets put every frame, which means there's a limit to the text speed.
                    if (textSpeed > 0)
                        charTimer = 1.0F / textSpeed;
                    else
                        charTimer = 0.0F;

                }
                else // Reduce timer.
                {
                    charTimer -= Time.deltaTime;
                }
            }
            else
            {
                // No characters to load.
                loadingChars = false;
                charTimer = 0.0F;
            }
        }

        // TODO: check callbacks.
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
        public void ClearPages()
        {
            // Clears out the pages.
            pages.Clear();
            pages = new List<Page>();

            // Now at index 0.
            currPageIndex = 0;

            // Clear out waiting
            loadingChars = false;
            boxText.text = "...";
            charQueue.Clear();
            charTimer = 0.0F;
        }

        // Update is called once per frame
        void Update()
        {
            // Changed this from the courtine version.
            if (loadingChars)
                LoadCharacterByCharacter();
        }
    }
}