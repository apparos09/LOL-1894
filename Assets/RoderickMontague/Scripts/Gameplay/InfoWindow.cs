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

        [Header("Sprites")]
        public Sprite healthSprite;
        public Sprite attackSprite;
        public Color attackColor = Color.white;
        public Sprite defenseSprite;
        public Color defenseColor = Color.white;
        public Sprite speedSprite;
        public Color speedColor = Color.white;
        public Sprite energySprite;

        [Header("Sprites/Moves")]
        public Sprite rankSprite;
        public Sprite powerSprite;
        public Sprite accuracySprite;
        public Sprite criticalSprite;
        public Color criticalColor = Color.white;
        public Sprite burnSprite;
        public Color burnColor = Color.white;
        public Sprite paralysisSprite;
        public Color paralysisColor = Color.white;

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
            else
            {
                LanguageMarker.Instance.MarkText(titleText);
                LanguageMarker.Instance.MarkText(pageTitle);
                LanguageMarker.Instance.MarkText(backButtonText);
            }

            // Initialize the list.
            pages = new List<InfoPage>();

            // NOTE: only the page titles are going to have the brackets around them since...
            // I'm short on time, and this would have to be taken out anyway...


            // Probability Page
            {
                InfoPage page = new InfoPage();
                page.title = "Probability";
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
                entry.name = "Probability (Percentage Form)";
                entry.nameKey = "ifo_probability_percent_nme";

                entry.description = "A probability format where percentages are used instead of decimals. The higher an event's chance percentage, the more likely it is to occur, with 0% equating to 0.00 and 100% equating to 1.00. Multiplying a decimal by 100 makes it a percentage, and dividing a percentage by 100 makes it a decimal.";
                entry.descriptionKey = "ifo_probability_percent_dsc";

                entry.symbol = null;
                entry.symbolColor = Color.white;
                page.entries.Add(entry);


                // Probability (Fraction)
                entry = ClearInfoPageEntry(entry);
                entry.name = "Probability (Fraction Form)";
                entry.nameKey = "ifo_probability_fraction_nme";

                entry.description = "A probability format that uses fractions (x/y). The denominator (y) is the total group size, while the numerator (x) is the group portion size. The larger the portion, the more likely the event is. In decimal form, (x = y) equates to 1.00, and (x = 0) equates to 0.00.";
                entry.descriptionKey = "ifo_probability_fraction_dsc";

                entry.symbol = null;
                entry.symbolColor = Color.white;
                page.entries.Add(entry);


                // Translate, and add the page.
                page = LoadPageLanguageText(page, true);
                pages.Add(page);
            }



            // Battle Stats Page - 1
            {
                InfoPage page = new InfoPage();
                page.title = "Battler Stats";
                page.titleKey = "ifo_stats";
                page.entries = new List<InfoPageEntry>();

                InfoPageEntry entry = new InfoPageEntry();

                // Level
                entry = ClearInfoPageEntry(entry);
                entry.name = "Level";
                entry.nameKey = "ifo_stats_level_nme";

                entry.description = "A number value that conveys a battler's strength. Upon leveling up, a battler gets permanent increases to their stats.";
                entry.descriptionKey = "ifo_stats_level_dsc";

                entry.symbol = null;
                entry.symbolColor = Color.white;
                page.entries.Add(entry);

                // Health
                entry = ClearInfoPageEntry(entry);
                entry.name = "Health";
                entry.nameKey = "ifo_stats_health_nme";

                entry.description = "The hit points of a battler. When a battler's health reaches 0, they have lost the battle.";
                entry.descriptionKey = "ifo_stats_health_dsc";

                entry.symbol = healthSprite;
                page.entries.Add(entry);


                // Attack
                entry = ClearInfoPageEntry(entry);
                entry.name = "Attack";
                entry.nameKey = "ifo_stats_attack_nme";

                entry.description = "The inherent strength of the battler. Combined with other factors, the battler's attack determines how much damage they do to their target.";
                entry.descriptionKey = "ifo_stats_attack_dsc";

                entry.symbol = attackSprite;
                entry.symbolColor = attackColor;
                page.entries.Add(entry);


                // Translate, and add the page.
                page = LoadPageLanguageText(page, true);
                page.title += " - 1";
                pages.Add(page);
            }

            // Battle Stats Page - 2
            {
                InfoPage page = new InfoPage();
                page.title = "Battler Stats";
                page.titleKey = "ifo_stats";
                page.entries = new List<InfoPageEntry>();

                InfoPageEntry entry = new InfoPageEntry();

                // Defense
                entry = ClearInfoPageEntry(entry);
                entry.name = "Defense";
                entry.nameKey = "ifo_stats_defense_nme";

                entry.description = "The battler's inherent resistance to damage. In combination with other factors, this is used to calculate the amount of damage the battler takes when hit by a move.";
                entry.descriptionKey = "ifo_stats_defense_dsc";

                entry.symbol = defenseSprite;
                entry.symbolColor = defenseColor;
                page.entries.Add(entry);

                // Speed
                entry = ClearInfoPageEntry(entry);
                entry.name = "Speed";
                entry.nameKey = "ifo_stats_speed_nme";

                entry.description = "How fast the battler moves. This is the primary factor that determines the order that battlers go in. If two battlers have the same speed, the turn order is random.";
                entry.descriptionKey = "ifo_stats_speed_dsc";

                entry.symbol = speedSprite;
                entry.symbolColor = speedColor;
                page.entries.Add(entry);


                // Energy
                entry = ClearInfoPageEntry(entry);
                entry.name = "Energy";
                entry.nameKey = "ifo_stats_energy_nme";

                entry.description = "The power source for a battler's battle moves. The battler cannot perform a battle move if they do not have enough energy. If a battler runs out of energy, they must take a turn to charge their energy.";
                entry.descriptionKey = "ifo_stats_energy_dsc";

                entry.symbol = energySprite;
                entry.symbolColor = Color.white;
                page.entries.Add(entry);


                // Translate, and add the page.
                page = LoadPageLanguageText(page, true);
                page.title += " - 2";
                pages.Add(page);
            }



            // Moves - 1
            {
                InfoPage page = new InfoPage();
                page.title = "Moves";
                page.titleKey = "ifo_moves";
                page.entries = new List<InfoPageEntry>();

                InfoPageEntry entry = new InfoPageEntry();

                // Rank
                entry = ClearInfoPageEntry(entry);
                entry.name = "Rank";
                entry.nameKey = "ifo_moves_rank_nme";

                entry.description = "A numerical label that conveys how advanced a move is, which ranges from 1 to 3. The higher the rank, the more advanced the move is. Higher rank moves become more common as the player progresses through the simulation.";
                entry.descriptionKey = "ifo_moves_rank_dsc";

                entry.symbol = rankSprite;
                entry.symbolColor = Color.white;
                page.entries.Add(entry);

                // Power
                entry = ClearInfoPageEntry(entry);
                entry.name = "Power";
                entry.nameKey = "ifo_moves_power_nme";

                entry.description = "The base strength of a move, which along with other factors determines how much damage the move does. A move with no power listed either does no damage, or does a varying amount of damage based on certain factors.";
                entry.descriptionKey = "ifo_moves_power_dsc";

                entry.symbol = powerSprite;
                entry.symbolColor = Color.white;
                page.entries.Add(entry);


                // Accuracy
                entry = ClearInfoPageEntry(entry);
                entry.name = "Accuracy";
                entry.nameKey = "ifo_moves_accuracy_nme";

                entry.description = "The likelihood of a move hitting its target, with a 1.00 accuracy being a guaranteed hit. A move with no accuracy listed either always hits, or the move's success is determined by some unique set of factors.";
                entry.descriptionKey = "ifo_moves_accuracy_dsc";

                entry.symbol = accuracySprite;
                entry.symbolColor = Color.white;
                page.entries.Add(entry);


                // Translate, and add the page.
                page = LoadPageLanguageText(page, true);
                page.title += " - 1";
                pages.Add(page);
            }

            // Moves - 2
            {
                InfoPage page = new InfoPage();
                page.title = "Moves";
                page.titleKey = "ifo_moves";
                page.entries = new List<InfoPageEntry>();

                InfoPageEntry entry = new InfoPageEntry();

                // Energy Usage
                entry = ClearInfoPageEntry(entry);
                entry.name = "Energy Usage";
                entry.nameKey = "ifo_moves_energyUsage_nme";

                entry.description = "The amount of energy needed to perform a move. A move cannot be chosen if the user does not have enough energy to use it. If a move's energy amount isn't listed, then the move either uses no energy, or it calculates energy usage using unique factors.";
                entry.descriptionKey = "ifo_moves_energyUsage_dsc";

                entry.symbol = energySprite;
                entry.symbolColor = Color.white;
                page.entries.Add(entry);

                // Critical Damage
                entry = ClearInfoPageEntry(entry);
                entry.name = "Critical Damage";
                entry.nameKey = "ifo_moves_critical_nme";

                entry.description = "Extra damage done randomly when a directly damaging move successfully hits its target. Unless a move's description states otherwise, every directly damaging move has the same critical damage chance.";
                entry.descriptionKey = "ifo_moves_critical_dsc";

                entry.symbol = criticalSprite;
                entry.symbolColor = criticalColor;
                page.entries.Add(entry);


                // Recoil
                entry = ClearInfoPageEntry(entry);
                entry.name = "Recoil Damage";
                entry.nameKey = "ifo_moves_recoil_nme";

                entry.description = "Damage dealt to the user for successfully performing certain moves. If a move has recoil damage, it will be stated in said move's description.";
                entry.descriptionKey = "ifo_moves_recoil_dsc";

                entry.symbol = null;
                entry.symbolColor = Color.white;
                page.entries.Add(entry);


                // Translate, and add the page.
                page = LoadPageLanguageText(page, true);
                page.title += " - 2";
                pages.Add(page);
            }

            // Moves - 3
            {
                InfoPage page = new InfoPage();
                page.title = "Moves";
                page.titleKey = "ifo_moves";
                page.entries = new List<InfoPageEntry>();

                InfoPageEntry entry = new InfoPageEntry();

                // Stat Change
                entry = ClearInfoPageEntry(entry);
                entry.name = "Stat Change";
                entry.nameKey = "ifo_moves_statChange_nme";

                entry.description = "A modifier that will temporarily change one of the battler's stats. Stat modifiers wear off for both the player and their opponent when the battle ends. A stat change can modify the target's attack, defense, speed, or accuracy.";
                entry.descriptionKey = "ifo_moves_statChange_dsc";

                entry.symbol = null;
                entry.symbolColor = Color.white;
                page.entries.Add(entry);

                // Burn
                entry = ClearInfoPageEntry(entry);
                entry.name = "Burn";
                entry.nameKey = "ifo_moves_burn_nme";

                entry.description = "A status effect that applies damage to the inflicted battler every turn. This status effect wears off for both battlers when the battle ends.";
                entry.descriptionKey = "ifo_moves_burn_dsc";

                entry.symbol = burnSprite;
                entry.symbolColor = burnColor;
                page.entries.Add(entry);


                // Paralysis
                entry = ClearInfoPageEntry(entry);
                entry.name = "Paralysis";
                entry.nameKey = "ifo_moves_paralysis_nme";

                entry.description = "A status effect that reduces the inflicted battler's speed, and that causes them to randomly miss turns. This status effect wears off for both battlers when the battle ends.";
                entry.descriptionKey = "ifo_moves_paralysis_dsc";

                entry.symbol = paralysisSprite;
                entry.symbolColor = paralysisColor;
                page.entries.Add(entry);


                // Translate, and add the page.
                page = LoadPageLanguageText(page, true);
                page.title += " - 3";
                pages.Add(page);
            }



            // This needs to be done individually so that numbers can be attached.
            // // Translates each page if the SDK has been initialized.
            // if (LOLSDK.Instance.IsInitialized)
            // {
            //     for (int i = 0; i < pages.Count; i++)
            //     {
            //         pages[i] = LoadPageLanguageText(pages[i], true);
            //     }
            // }

            // First page.
            pageIndex = 0;
            UpdatePage();
        }

        // This object is called when the object is enabled and active.
        private void OnEnable()
        {
            // Enable/disable the speak keys.
            RefreshSpeakButtons();
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

        // Speaks the provided entry, which ranges from 0 to 2.
        private void SpeakEntry(int entryNumber)
        {
            // Checks if text-to-speech is enabled.
            if (LOLSDK.Instance.IsInitialized && GameSettings.Instance.UseTextToSpeech)
            {
                // Entry display object.
                InfoPageEntryDisplay iped = null;

                // Checks the entry number.
                switch (entryNumber)
                {
                    case 0:
                        iped = pageEntry0;
                        break;
                    case 1:
                        iped = pageEntry1;
                        break;
                    case 2:
                        iped = pageEntry2;
                        break;
                }

                // The object was set.
                if(iped != null)
                {
                    // The speak key exists.
                    if(iped.descriptionSpeakKey != string.Empty)
                        LOLManager.Instance.textToSpeech.SpeakText(iped.descriptionSpeakKey);
                }
            }
        }


        // Reads the first entry on the page.
        public void SpeakEntry0()
        {
            SpeakEntry(0);
        }

        // Reads the second entry on the page.
        public void SpeakEntry1()
        {
            SpeakEntry(1);
        }

        // Reads the third entry on the page.
        public void SpeakEntry2()
        {
            SpeakEntry(2);
        }


        // Refreshes the speak buttons.
        public void RefreshSpeakButtons()
        {
            pageEntry0.RefreshSpeakButton();
            pageEntry1.RefreshSpeakButton();
            pageEntry2.RefreshSpeakButton();
        }
    }
}