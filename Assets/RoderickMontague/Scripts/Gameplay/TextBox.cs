using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// The clas sfor the text box for the game.
public class TextBox : MonoBehaviour
{
    // The current page.
    public int currPageIndex = 0;

    // List of pages.
    public List<string> pages = new List<string>();

    // The text in the text box.
    public TMPro.TMP_Text boxText;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // TODO: add touch and mouse for goign onto the next page.

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
        gameObject.SetActive(true);
    }

    // Hides the textbox.
    public void Hide()
    {
        // TODO: add animation.
        gameObject.SetActive(false);
    }

    // Moves onto the next page.
    public void NextPage()
    {
        // Increases the index.
        currPageIndex++;

        // Sets the text.
        SetPageText();
    }

    // Returns to the previous page.
    public void PreviousPage()
    {
        // Decreases the index.
        currPageIndex--;

        // Sets the text.
        SetPageText();
    }

    // Sets the text that's on the page.
    private void SetPageText()
    {
        // Bounds correction.
        currPageIndex = Mathf.Clamp(currPageIndex, 0, pages.Count - 1);

        // Sets the box text.
        boxText.text = pages[currPageIndex];

        // TODO: maybe have con
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
