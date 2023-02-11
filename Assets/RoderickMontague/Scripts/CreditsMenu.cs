using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using SimpleJSON;

namespace RM_BBTS
{
    // The credits menu for the game.
    public class CreditsMenu : MonoBehaviour
    {
        // The audio references object.
        public AudioCredits audioCredits;

        // The credit index for the audio reference.
        private int creditIndex = 0;

        // The user interface for the credits menu.
        [Header("UI")]

        // The title text.
        public TMP_Text titleText;

        // The text for the back button.
        public TMP_Text backButtonText;

        [Header("UI/Credit")]
        // The name of the song.
        public TMP_Text songNameText;
        // The name of the artist(s).
        public TMP_Text artistNameText;
        // The name of the album/group that the song comes from.
        public TMP_Text collectionNameText;

        // The source of the song, which will be a website most likely.
        public TMP_Text sourceText;
        
        // The link to the the song (website, website page, etc.). This is a link to the source you used.
        public TMP_Text link1Text;
        // The link to the the song (website, website page, etc.). This second link is for the orgination of the audio.
        public TMP_Text link2Text;

        // The text for the copyright information.
        public TMP_Text copyrightText;

        // The page number text, which is a fraction (000/000)
        public TMP_Text pageNumberText;

        

        // Start is called before the first frame update
        void Start()
        {
            // Translation.
            JSONNode defs = SharedState.LanguageDefs;

            // Defs set.
            if(defs != null)
            {
                titleText.text = defs["kwd_licenses"];
                backButtonText.text = defs["kwd_back"];
            }
            else
            {
                // Mark the auto load text.
                LanguageMarker.Instance.MarkText(titleText);
                LanguageMarker.Instance.MarkText(backButtonText);
            }

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

            // Sets the new index, clamping it so that it's within the page count.
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
            link1Text.text = credit.link1;
            link2Text.text = credit.link2;
            copyrightText.text = credit.copyright;

            // Updates the page number.
            pageNumberText.text = (creditIndex + 1).ToString() + "/" + audioCredits.GetCreditCount().ToString();
        }


    }
}