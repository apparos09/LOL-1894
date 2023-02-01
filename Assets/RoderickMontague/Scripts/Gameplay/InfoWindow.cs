using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using SimpleJSON;
using LoLSDK;

namespace RM_BBTS
{
    // An info window entry.
    public struct InfoPageEntry
    {
        // The name, and its key in the file.
        public string name;
        public string nameKey;

        // The description, and its key in the file.
        public string description;
        public string descriptionKey;

        // The symbol and its colour.
        public Sprite symbol;
        public Color symbolColor;
    }

    // An info page, which has a list of entries to load in.
    public struct InfoPage
    {
        // The title of the info page.
        public string title;

        // The key for the title.
        public string titleKey;

        // The list of the entries.
        public List<InfoPageEntry> entries;
    }

    // The script for the information window of the game.
    public class InfoWindow : MonoBehaviour
    {
        // The title of the info window.
        public TMP_Text titleText;

        // The pages for the info window.
        private List<InfoPage> pages;

        // The index for the list of pages.
        private int pageIndex = 0;

        // The amount of entries for one page of the info window.
        public const int PAGE_ENTRY_COUNT = 4;

        // The list of the four page entries.
        [Header("Entries")]
        // The title of the page.
        public TMP_Text pageTitle;

        // The number of info page display entries.
        const int ENTRY_DISPLAYS = 4;

        // The four display entries.
        public InfoPageEntryDisplay pageEntry0;
        public InfoPageEntryDisplay pageEntry1;
        public InfoPageEntryDisplay pageEntry2;


        [Header("Other")]
        // The page number text, which is a fraction (000/000)
        public TMP_Text pageNumberText;

        // The button text for returning to the game.
        public TMP_Text backButtonText;

        // Start is called before the first frame update
        void Start()
        {
            // Translation.
            JSONNode defs = SharedState.LanguageDefs;

            // Translate the content.
            if (defs != null)
            {
                titleText.text = defs["kwd_info"];
                backButtonText.text = defs["kwd_returnToGame"];
            }

            // Initialize the list.
            pages = new List<InfoPage>();

            // NOTE: only the page titles are going to have the brackets around them since...
            // I'm short on time, and this would have to be taken out anyway...

            // Probability Page
            {
                InfoPage page = new InfoPage();
                page.title = "<Probability>";
                page.titleKey = "ifo_probability";
                page.entries = new List<InfoPageEntry>();

                InfoPageEntry entry = new InfoPageEntry();

                // Probability
                entry = ClearInfoPageEntry(entry);
                entry.name = "Probability";
                entry.nameKey = "ifo_probability_decimal_nme";
                
                entry.description = "A math subject where you assess the chance of an event (or of a series of events) occurring. A chance of 0.00 means the event will never happen, and a chance of 1.00 means the event will always happen. The higher the chance, the more likely the event is.";
                entry.descriptionKey = "ifo_probability_decimal_dsc";

                entry.symbol = null;
                entry.symbolColor = Color.white;
                page.entries.Add(entry);


                // Probability (Percent)
                entry = ClearInfoPageEntry(entry);
                entry.name = "Probability";
                entry.nameKey = "ifo_probability_percent_nme";

                entry.description = "A math subject where you assess the chance of an event (or of a series of events) occurring. A chance of 0.00 means the event will never happen, and a chance of 1.00 means the event will always happen. The higher the chance, the more likely the event is.";
                entry.descriptionKey = "ifo_probability_decimal_dsc";

                entry.symbol = null;
                entry.symbolColor = Color.white;
                page.entries.Add(entry);


                // Probability (Fraction)
                entry = ClearInfoPageEntry(entry);
                entry.name = "Probability";
                entry.nameKey = "ifo_probability_fraction_nme";

                entry.description = "A math subject where you assess the chance of an event (or of a series of events) occurring. A chance of 0.00 means the event will never happen, and a chance of 1.00 means the event will always happen. The higher the chance, the more likely the event is.";
                entry.descriptionKey = "ifo_probability_fraction_dsc";

                entry.symbol = null;
                entry.symbolColor = Color.white;
                page.entries.Add(entry);


                // Add the page.
                pages.Add(page);
            }

            // Translates each page if the SDK has been initialized.
            if(LOLSDK.Instance.IsInitialized)
            {
                for (int i = 0; i < pages.Count; i++)
                {
                    pages[i] = LoadPageLanguageText(pages[i], true);
                }
            }

            // First page.
            pageIndex = 0;
            UpdatePage();
        }

        // Checks if the index is in the bounds.
        public static bool IndexInBounds<T>(List<T> list, int index)
        {
            // The result object.
            bool result = false;

            // Entries list has been initialized.
            if (list != null)
            {
                // Check if index is in bounds.
                if (index >= 0 && index < list.Count)
                {
                    result = true;
                }
            }

            return result;
        }

        // Loads the entry's language text.
        public static InfoPage LoadPageLanguageText(InfoPage infoPage, bool loadEntryText)
        {
            // If the SDK has been initialized.
            if (LOLSDK.Instance.IsInitialized)
            {
                // Translates the page title.
                infoPage.title = LOLManager.Instance.GetLanguageText(infoPage.titleKey);
            
                // If the entry text should also be loaded.
                if(loadEntryText)
                {
                    // Loads the text for each entry.
                    for (int i = 0; i < infoPage.entries.Count; i++)
                    {
                        infoPage.entries[i] = LoadEntryLanguageText(infoPage.entries[i]);
                    }
                }
            
            }

            return infoPage;
        }

        // Loads the entry's language text.
        public static InfoPageEntry LoadEntryLanguageText(InfoPageEntry infoEntry)
        {
            // If the SDK has been initialized.
            if(LOLSDK.Instance.IsInitialized)
            {
                infoEntry.name = LOLManager.Instance.GetLanguageText(infoEntry.nameKey);
                infoEntry.description = LOLManager.Instance.GetLanguageText(infoEntry.descriptionKey);
            }

            return infoEntry;
        }

        // Clears out the info page entry.
        public static InfoPageEntry ClearInfoPageEntry(InfoPageEntry entry)
        {
            entry.name = string.Empty;
            entry.nameKey = string.Empty;

            entry.description = string.Empty;
            entry.descriptionKey = string.Empty;

            entry.symbol = null;
            entry.symbolColor = Color.white;
            return entry;
        }

        // Sets the index of the page.
        public void SetPageIndex(int newIndex)
        {
            // If there are no pages to load.
            if (pages.Count == 0)
            {
                pageIndex = -1;
                return;
            }

            // Sets the new index, clamping it so that it's within the page count.
            pageIndex = Mathf.Clamp(newIndex, 0, pages.Count);

            // Updates the displayed page.
            UpdatePage();
        }

        // Goes to the previous page.
        public void PreviousPage()
        {
            // Generates the new index.
            int newIndex = pageIndex - 1;

            // Goes to the end of the list.
            if (!IndexInBounds(pages, newIndex))
                newIndex = pages.Count - 1;

            SetPageIndex(newIndex);
        }

        // Goes to the next page.
        public void NextPage()
        {
            // Generates the new index.
            int newIndex = pageIndex + 1;

            // Goes to the start of the list.
            if (!IndexInBounds(pages, newIndex))
                newIndex = 0;

            SetPageIndex(newIndex);
        }

        // Updates the page currently being displayed.
        public void UpdatePage()
        {
            // Grabs the current page.
            InfoPage infoPage = pages[pageIndex];

            // Sets the title.
            pageTitle.text = infoPage.title;

            // Creates an array.
            InfoPageEntryDisplay[] displays = new InfoPageEntryDisplay[] { pageEntry0, pageEntry1, pageEntry2 };

            // Updates the entry displays.
            for(int i = 0; i < displays.Length; i++)
            {
                // If the entry is available, load it.
                if(i < infoPage.entries.Count) // Loads the entry.
                {
                    displays[i].LoadEntry(infoPage.entries[i]);
                }
                else // Clears out the entry.
                {
                    displays[i].ClearEntry();
                }
            }

            // Updates the page number.
            pageNumberText.text = (pageIndex + 1).ToString() + "/" + pages.Count.ToString();
        }

    }
}