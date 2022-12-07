using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace RM_BBTS
{
    // The credits menu for the game.
    public class CreditsMenu : MonoBehaviour
    {
        // The audio references object.
        public AudioCredits audioCredits;

        // The credit index for the audio reference.
        public int creditIndex = 0;

        // The user interface for the credits menu.
        [Header("UI")]

        // The name of the song.
        public TMP_Text songNameText;
        // The name of the artist(s).
        public TMP_Text artistNameText;
        // The name of the album/group that the song comes from.
        public TMP_Text collectionNameText;

        // The source of the song, which will be a website most likely.
        public TMP_Text sourceText;
        // The link to the the song (website, website page, etc.).
        public TMP_Text linkText;

        // The text for the copyright information.
        public TMP_Text copyrightText;

        // The page number text, which is a fraction (000/000)
        public TMP_Text pageNumberText;

        // Start is called before the first frame update
        void Start()
        {
            // Loads credit and sets page number.
            UpdateCredit();
        }

        // Sets the index of the page.
        public void SetPageIndex(int newIndex)
        {
            // The reference count.
            int refCount = audioCredits.GetCreditCount();

            // No references to load.
            if (refCount == 0)
            {
                creditIndex = -1;
                return;
            }

            // Sets the new index, clamping it within the page count.
            creditIndex = Mathf.Clamp(newIndex, 0, refCount - 1);

            // Updates the displayed credit.
            UpdateCredit();
        }

        // Goes to the previous page.
        public void PreviousPage()
        {
            // Generates the new index.
            int newIndex = creditIndex - 1;

            // Goes to the end of the list.
            if (!audioCredits.IndexInBounds(newIndex))
                newIndex = audioCredits.GetCreditCount() - 1;

            SetPageIndex(newIndex);
        }

        // Goes to the next page.
        public void NextPage()
        {
            // Generates the new index.
            int newIndex = creditIndex + 1;

            // Goes to the start of the list.
            if (!audioCredits.IndexInBounds(newIndex))
                newIndex = 0;

            SetPageIndex(newIndex);
        }

        // Updates the credit.
        public void UpdateCredit()
        {
            // No credit to update, or index out of bounds.
            if (audioCredits.GetCreditCount() == 0 || !audioCredits.IndexInBounds(creditIndex))
                return;

            // Gets the credit.
            AudioCredit credit = audioCredits.audioCredits[creditIndex];

            // Updates all of the information.
            songNameText.text = credit.song;
            artistNameText.text = credit.artist;
            collectionNameText.text = credit.collection;
            sourceText.text = credit.source;
            linkText.text = credit.link;
            copyrightText.text = credit.copyright;

            // Updates the page number.
            pageNumberText.text = (creditIndex + 1).ToString() + "/" + audioCredits.GetCreditCount().ToString();
        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}