using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// The clas sfor the text box for the game.
public class TextBox : MonoBehaviour
{
    // The current page.
    private int currPageIndex = 0;

    // List of pages.
    public List<string> pages = new List<string>();

    // The text in the text box.
    public TMPro.TMP_Text boxText;

    [Header("Animation")]
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

    // Start is called before the first frame update
    void Start()
    {
        
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
    public string CurrentPage
    {
        get
        {
            if (currPageIndex >= 0 && currPageIndex < pages.Count)
                return pages[currPageIndex];
            else
                return "";
        }
    }

    // Shows the textbox.
    public void Show()
    {
        // TODO: add animation.
        boxText.gameObject.SetActive(true);
    }

    // Hides the textbox.
    public void Hide()
    {
        // TODO: add animation.
        boxText.gameObject.SetActive(false);
    }

    // Changes the page index.
    public void SetPage(int index)
    {
        SetPageText(index);
    }

    // Moves onto the next page.
    public void NextPage()
    {
        // Increases the index, and sets the text.
        SetPageText(currPageIndex + 1);
    }

    // Returns to the previous page.
    public void PreviousPage()
    {
        // Decreases the index, and sets the text.
        SetPageText(currPageIndex - 1);
    }

    // Loads the page text from the list.
    public void LoadPageText(List<string> newPages)
    {
        // Replaces the pages and sets the page.
        pages = newPages;
        SetPage(0);
    }

    // Sets the text that's on the page.
    private void SetPageText(int nextPageIndex, bool finishText = true)
    {
        // If text is still being loaded just sub in the rest and stop loading in new characters.
        if (loadingChars)
        {
            loadingChars = false;

            // If the text should be finished.
            if(finishText)
            {
                // Finishes the text instead of replacing the page.
                boxText.text = pages[currPageIndex];
                return;
            }
        }

        // Clears out the existing text.
        boxText.text = "";

        // Sets the new page index.
        currPageIndex = nextPageIndex;

        // Bounds correction.
        // Current Page Index is set to -1 if there are no pages.
        if (pages.Count > 0)
            currPageIndex = Mathf.Clamp(currPageIndex, 0, pages.Count - 1);
        else
            currPageIndex = -1;

        // Checks if the text should be shown automatically, or if it should be shown letter by letter.
        if (instantText) // Instant
        {
            // Sets the box text.
            boxText.text = pages[currPageIndex];
        }
        else // Letter by Letter
        {
            // Set to load characters, and loads up the char queue.
            loadingChars = true;
            charQueue.Clear();
            charQueue = new Queue<char>(pages[currPageIndex]);
        }
        

        // TODO: maybe have con
    }

    // Loads character by character.
    private void LoadCharacterByCharacter()
    {
        // Checks if the timer has reached 0 for displaying the next character.
        if(charQueue.Count != 0)
        {
            // If the timer has reached 0 or less.
            if (charTimer <= 0.0F)
            {
                // Adds to the string.
                string temp = boxText.text;
                temp += charQueue.Dequeue();
                boxText.text = temp;

                // If the text speed is set to 0 the new char will load on the next frame.
                if (textSpeed > 0)
                    charTimer = 1 / textSpeed;
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
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (loadingChars)
            LoadCharacterByCharacter();
    }
}
