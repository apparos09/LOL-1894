using LoLSDK;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SimpleJSON;

namespace RM_BBTS
{
    // TODO: check to see if it'd be best to just save each message in its own string.

    // The battle messages for the game.
    public class BattleMessages : MonoBehaviour
    {
        // the instance of the game settings.
        private static BattleMessages instance;

        // The language definitions.
        JSONNode defs = null;

        // Constructor
        private BattleMessages()
        {
        }

        // Awake is called when a script instance is being loaded.
        private void Awake()
        {         
            // Checks for the instance.
            if (instance == null)
            {
                instance = this;
            }

            // Loads the definitions if the instance doesn't have them set.
            if (instance.defs == null)
                instance.defs = SharedState.LanguageDefs;

        }

        // // Start is called before the first frame update
        // void Start()
        // {
        //     
        // }

        // Returns the instance of the class.
        public static BattleMessages Instance
        {
            get
            {
                // Checks to see if the instance exists. If it doesn't, generate an object.
                if (instance == null)
                {
                    instance = FindObjectOfType<BattleMessages>(true);

                    // Generate new instance if an existing instance was not found.
                    if (instance == null)
                    {
                        // Makes a new settings object.
                        GameObject go = new GameObject("(singleton) Battle Messages");

                        // Adds the instance component to the new object.
                        instance = go.AddComponent<BattleMessages>();
                    }

                }

                // Returns the instance.
                return instance;
            }
        }

        // // Gets the move used message.
        // public string GetBase(string user, string move)
        // {
        //     // The message string.
        //     string msg = "";
        // 
        //     // Checks if defs existed.
        //     if (defs != null)
        //     {
        //         // Grabs the translated message.
        //         msg = defs["btl_msg_mve_moveUsed"];
        //     }
        //     else
        //     {
        //         // Grabs the default mesage.
        //         msg = "{0} used {1}!";
        //     }
        // 
        //     // Replaces the information.
        //     msg = msg.Replace("{0}", user);
        //     msg = msg.Replace("{1}", move);
        // 
        //     return msg;
        // }

        // MOVE USED //
        // Gets the move used message.
        public string GetMoveUsedMessage(string user, string move)
        {
            // The message string.
            string msg = "";

            // Checks if defs existed.
            if(defs != null)
            {
                // Grabs the translated message.
                msg = defs["btl_msg_mve_moveUsed"];
            }
            else
            {
                // Grabs the default mesage.
                msg = "{0} used {1}!";
            }

            // Replaces the information.
            msg = msg.Replace("{0}", user);
            msg = msg.Replace("{1}", move);

            return msg;
        }

        // Gets the player speak key for move used.
        public string GetMoveUsedSpeakKey0()
        {
            return "btl_msg_mve_moveUsed_alt00";
        }

        // Gets the opponent speak key for move used.
        public string GetMoveUsedSpeakKey1()
        {
            return "btl_msg_mve_moveUsed_alt01";
        }



        // MOVE HIT //
        // Gets the move hit message.
        public string GetMoveHitMessage()
        {
            // The message string.
            string msg = "";

            // Checks if defs existed.
            if (defs != null)
            {
                // Grabs the translated message.
                msg = defs["btl_msg_mve_moveHit"];
            }
            else
            {
                // Grabs the default mesage.
                msg = "The move hit!";
            }

            return msg;
        }

        // Gets the move hit speak key.
        public string GetMoveHitSpeakKey()
        {
            return "btl_msg_mve_moveHit";
        }

        // Gets the move successful message.
        public string GetMoveSuccessfulMessage()
        {
            // The message string.
            string msg = "";

            // Checks if defs existed.
            if (defs != null)
            {
                // Grabs the translated message.
                msg = defs["btl_msg_mve_moveSuccess"];
            }
            else
            {
                // Grabs the default mesage.
                msg = "The move was successful!";
            }

            return msg;
        }

        // Gets the move successful speak key.
        public string GetMoveSuccessfulSpeakKey()
        {
            return "btl_msg_mve_moveSuccess";
        }



        // MOVE CRITICAL //
        // Gets the move hit critical message.
        public string GetMoveHitCriticalMessage()
        {
            // The message string.
            string msg = "";

            // Checks if defs existed.
            if (defs != null)
            {
                // Grabs the translated message.
                msg = defs["btl_msg_mve_moveCritical"];
            }
            else
            {
                // Grabs the default mesage.
                msg = "The move hit, and it did critical damage!";
            }

            return msg;
        }

        // Gets the move hit critical speak key.
        public string GetMoveHitCriticalSpeakKey()
        {
            return "btl_msg_mve_moveCritical";
        }

        // MOVE RECOIL //
        // Gets the move hit with recoil message.
        public string GetMoveHitRecoilMessage(string user)
        {
            // The message string.
            string msg = "";

            // Checks if defs exists.
            if (defs != null)
            {
                // Pull translated messages.
                msg = defs["btl_msg_mve_moveRecoil"];
            }
            else
            {
                // Grabs the default message.
                msg = "{0} took recoil damage!";
            }

            // Slot in the message text.
            msg = msg.Replace("{0}", user);

            return msg;
        }

        // Gets the player move hit recoil speak key.
        public string GetMoveHitRecoilSpeakKey0()
        {
            return "btl_msg_mve_moveRecoil_alt00";
        }

        // Gets the opponent move hit recoil speak key.
        public string GetMoveHitRecoilSpeakKey1()
        {
            return "btl_msg_mve_moveRecoil_alt01";
        }



        // MOVE NO POWER //
        // Gets the move "not enough power" message.
        public string GetMoveNoEnergyMessage(string user)
        {
            // The message string.
            string msg = "";

            // Checks if defs exists.
            if (defs != null)
            {
                // Grabs the translated message.
                msg = defs["btl_msg_mve_moveNoEnergy"];
            }
            else
            {
                // Grabs the default mesage.
                msg = "{0} does not have enough energy to use their move!";
            }

            // Replaces the information.
            msg = msg.Replace("{0}", user);

            return msg;
        }

        // Gets the player speak key with "not enough power" message.
        public string GetMoveNoEnergySpeakKey0()
        {
            return "btl_msg_mve_moveNoEnergy_alt00";
        }

        // Gets the opponent speak key with "not enough power" message.
        public string GetMoveNoEnergySpeakKey1()
        {
            return "btl_msg_mve_moveNoEnergy_alt01";
        }



        // MOVE MISSED //
        // Gets the move missed message.
        public string GetMoveMissedMessage()
        {
            // The message string.
            string msg = "";

            // Checks if defs exists.
            if (defs != null)
            {
                // Grabs the translated message.
                msg = defs["btl_msg_mve_moveMissed"];
            }
            else
            {
                // Grabs the default mesage.
                msg = "The move missed!";
            }

            return msg;
        }

        // Gets the move missed speak key.
        public string GetMoveMissedSpeakKey()
        {
            return "btl_msg_mve_moveMissed";
        }



        // MOVE FAILED //
        // Gets the move missed message.
        public string GetMoveFailedMessage()
        {
            // The message string.
            string msg = "";

            // Checks if defs exists.
            if (defs != null)
            {
                // Grabs the translated message.
                msg = defs["btl_msg_mve_moveFailed"];
            }
            else
            {
                // Grabs the default mesage.
                msg = "The move failed!";
            }

            return msg;
        }

        // Gets the move failed speak key.
        public string GetMoveFailedSpeakKey()
        {
            return "btl_msg_mve_moveFailed";
        }



        // MOVE STAT INCREASE //
        // Gets the move stat increase message.
        public string GetMoveStatIncreaseMessage(string target, string stat, int amount)
        {
            // The message string.
            string msg = "";

            // Checks if defs exists.
            if (defs != null)
            {
                // Grabs the translated message.
                // A difference message is used based on the amount (singular vs. plural).
                if(amount == 1)
                    msg = defs["btl_msg_mve_moveStatIncSgl"];
                else
                    msg = defs["btl_msg_mve_moveStatIncMlt"];
            }
            else
            {
                // Grabs the default mesage (singular vs. pural).
                if (amount == 1)
                    msg = "The move increased {0}'s {1} by 1 stage!";
                else
                    msg = "The move increased {0}'s {1} by {2} stages!";
            }

            // Slot in values.
            msg = msg.Replace("{0}", target);
            msg = msg.Replace("{1}", stat);

            // Nothing will be replaced if it's a single stat raise.
            msg = msg.Replace("{2}", amount.ToString());

            return msg;
        }

        // Gets the move stat increase speak key 0.
        public string GetMoveStatIncreaseSpeakKey0()
        {
            return "btl_msg_mve_moveStatInc_alt00";
        }

        // Gets the move stat increase speak key 1.
        public string GetMoveStatIncreaseSpeakKey1()
        {
            return "btl_msg_mve_moveStatInc_alt01";
        }



        // MOVE STAT DECREASE //
        // Gets the move stat decrease message.
        public string GetMoveStatDecreaseMessage(string target, string stat, int amount)
        {
            // The message string.
            string msg = "";

            // Checks if defs exists.
            if (defs != null)
            {
                // Grabs the translated message.
                // A difference message is used based on the amount (singular vs. plural).
                if (amount == 1)
                    msg = defs["btl_msg_mve_moveStatDecSgl"];
                else
                    msg = defs["btl_msg_mve_moveStatDecMlt"];
            }
            else
            {
                // Grabs the default mesage (singular vs. pural).
                if (amount == 1)
                    msg = "The move decreased {0}'s {1} by 1 stage!";
                else
                    msg = "The move decreased {0}'s {1} by {2} stages!";
            }

            // Slot in values.
            msg = msg.Replace("{0}", target);
            msg = msg.Replace("{1}", stat);

            // Nothing will be replaced if it's a single stat raise.
            msg = msg.Replace("{2}", amount.ToString());

            return msg;
        }

        // Gets the move stat decrease speak key 0.
        public string GetMoveStatDecreaseSpeakKey0()
        {
            return "btl_msg_mve_moveStatDec_alt00";
        }

        // Gets the move stat decrease speak key 1.
        public string GetMoveStatDecreaseSpeakKey1()
        {
            return "btl_msg_mve_moveStatDec_alt01";
        }

        // STAT CHANGE REACHED LIMIT.
        // Gets the move stat change fail message.
        // upperLimit = true means that the stat can't go any higher.
        // upperLimit = false means that the stat can't go any lower.
        public string GetMoveStatLimitReachedMessage(string target, string stat, bool upperLimit)
        {
            // The message string.
            string msg = "";

            // Checks if defs exists.
            if (defs != null)
            {
                // Grabs the translated message.
                // A different message is used based on if the move can't go any higher, or any lower.
                if (upperLimit)
                    msg = defs["btl_msg_mve_moveStatHigh"];
                else
                    msg = defs["btl_msg_mve_moveStatLow"];
            }
            else
            {
                // Checks if the stat can't go any higher, or lower.
                if (upperLimit)
                    msg = "{0}'s {1} stat can't go any higher!";
                else
                    msg = "{0}'s {1} stat can't go any lower!";
            }

            // Slot in values.
            msg = msg.Replace("{0}", target);
            msg = msg.Replace("{1}", stat);

            return msg;
        }

        // Gets the move stat limit reached speak key 0.
        public string GetMoveStatLimitReachedSpeakKey0(bool upperLimit)
        {
            // Checks if the upper limit has been reached, or the lower limit.
            if(upperLimit)
                return "btl_msg_mve_moveStatHigh_alt00";
            else
                return "btl_msg_mve_moveStatLow_alt00";
        }

        // Gets the move stat limit reached speak key 1.
        public string GetMoveStatLimitReachedSpeakKey1(bool upperLimit)
        {
            // Checks if the upper limit has been reached, or the lower limit.
            if (upperLimit)
                return "btl_msg_mve_moveStatHigh_alt01";
            else
                return "btl_msg_mve_moveStatLow_alt01";
        }


        // MOVE BURNED //
        // Gets the move missed message.
        public string GetMoveBurnedMessage(string target)
        {
            // The message string.
            string msg = "";

            // Checks if defs exists.
            if (defs != null)
            {
                // Grabs the translated message.
                msg = defs["btl_msg_mve_moveBurned"];
            }
            else
            {
                // Grabs the default mesage.
                msg = "{0} has been inflicted with burn status!";
            }

            // Slot in target name.
            msg = msg.Replace("{0}", target);

            return msg;
        }

        // Gets move burned speak key 0 (player burned).
        public string GetMoveBurnedSpeakKey0()
        {
            return "btl_msg_mve_moveBurned_alt00";
        }

        // Gets move burned speak key 1 (target burned).
        public string GetMoveBurnedSpeakKey1()
        {
            return "btl_msg_mve_moveBurned_alt01";
        }



        // MOVE PARALYZED //
        // Gets the move paralyzed message.
        public string GetMoveParalyzedMessage(string target)
        {
            // The message string.
            string msg = "";

            // Checks if defs exists.
            if (defs != null)
            {
                // Grabs the translated message.
                msg = defs["btl_msg_mve_moveParalyzed"];
            }
            else
            {
                // Grabs the default mesage.
                msg = "{0} has been inflicted with paralysis status!";
            }

            // Slot in target name.
            msg = msg.Replace("{0}", target);

            return msg;
        }

        // Gets move paralyzed speak key 0 (player paralyzed).
        public string GetMoveParalyzedSpeakKey0()
        {
            return "btl_msg_mve_moveParalyzed_alt00";
        }

        // Gets  move paralyzed speak key 1 (opponent paralyzed).
        public string GetMoveParalyzedSpeakKey1()
        {
            return "btl_msg_mve_moveParalyzed_alt01";
        }



        // MOVE CHARGE //
        // Gets the move charged message.
        public string GetMoveChargeUsedMessage(string user)
        {
            // The message string.
            string msg = "";

            // Checks if defs exists.
            if (defs != null)
            {
                // Grabs the translated message.
                msg = defs["btl_msg_mve_chargeUsed"];
            }
            else
            {
                // Grabs the default mesage.
                msg = "{0} charged their energy!";
            }

            // Slotting in content.
            msg = msg.Replace("{0}", user);

            return msg;
        }

        // Gets the move charge used player speak key.
        public string GetMoveChargeUsedSpeakKey0()
        {
            return "btl_msg_mve_chargeUsed_alt00";
        }

        // Gets the move charge used opponent speak key.
        public string GetMoveChargeUsedSpeakKey1()
        {
            return "btl_msg_mve_chargeUsed_alt01";
        }



        // MOVE RUN FAILED //
        // The move run failed.
        public string GetMoveRunFailedMessage(string user)
        {
            // The message string.
            string msg = "";

            // Checks if defs exists.
            if (defs != null)
            {
                // Grabs the translated message.
                msg = defs["btl_msg_mve_runFailed"];
            }
            else
            {
                // Grabs the default mesage.
                msg = "{0} failed to run away!";
            }

            // Slotting in content.
            msg = msg.Replace("{0}", user);

            return msg;
        }

        // Gets the player run failed speak key.
        public string GetMoveRunFailedSpeakKey0()
        {
            return "btl_msg_mve_runFailed_alt00";
        }

        // Gets the opponent run failed speak key.
        public string GetMoveRunFailedSpeakKey1()
        {
            return "btl_msg_mve_runFailed_alt01";
        }



        // MOVE NOTHING //
        // The move caused nothing to happen.
        public string GetMoveNothingMessage()
        {
            // The message string.
            string msg = "";

            // Checks if defs exists.
            if (defs != null)
            {
                // Grabs the translated message.
                msg = defs["btl_msg_mve_nothing"];
            }
            else
            {
                // Grabs the default mesage.
                msg = "Nothing happened.";
            }

            return msg;
        }

        // Gets the move nothing speak key.
        public string GetMoveNothingSpeakKey()
        {
            return "btl_msg_mve_nothing";
        }



        // MOVE BURNED //
        // The target was burned.
        public string GetBurnedMessage(string infected)
        {
            // The message string.
            string msg = "";

            // Checks if defs exists.
            if (defs != null)
            {
                // Grabs the translated message.
                msg = defs["btl_msg_burned"];
            }
            else
            {
                // Grabs the default mesage.
                msg = "{0} took burn damage!";
            }

            // Slotting in content.
            msg = msg.Replace("{0}", infected);

            return msg;
        }

        // Get player burned.
        public string GetBurnedSpeakKey0()
        {
            return "btl_msg_burned_alt00";
        }

        // Get opponent burned.
        public string GetBurnedSpeakKey1()
        {
            return "btl_msg_burned_alt01";
        }



        // MOVE PARALYZED //
        // The target was paralyzed.
        public string GetParalyzedMessage(string infected)
        {
            // The message string.
            string msg = "";

            // Checks if defs exists.
            if (defs != null)
            {
                // Grabs the translated message.
                msg = defs["btl_msg_paralyzed"];
            }
            else
            {
                // Grabs the default mesage.
                msg = "{0} is immobilized, and can't move!";
            }

            // Slotting in content.
            msg = msg.Replace("{0}", infected);

            return msg;
        }

        // Gets the paralyzed player speak key.
        public string GetParalyzedSpeakKey0()
        {
            return "btl_msg_paralyzed_alt00";
        }

        // Gets the paralyzed opponent speak key.
        public string GetParalyzedSpeakKey1()
        {
            return "btl_msg_paralyzed_alt01";
        }



        // BATTLE WON //
        // The battle was won.
        public string GetBattleWonMessage()
        {
            // The message string.
            string msg = "";

            // Checks if defs exists.
            if (defs != null)
            {
                // Grabs the translated message.
                msg = defs["btl_msg_battleWon"];
            }
            else
            {
                // Grabs the default mesage.
                msg = "The opponent ran away! Battle Bot has won the battle!";
            }

            return msg;
        }

        // Gets the battle won speak key.
        public string GetBattleWonSpeakKey()
        {
            return "btl_msg_battleWon";
        }



        // BATTLE WON BOSS //
        // The battle was won against the boss.
        public string GetBattleWonBossMessage()
        {
            // The message string.
            string msg = "";

            // Checks if defs exists.
            if (defs != null)
            {
                // Grabs the translated message.
                msg = defs["btl_msg_battleWonBoss"];
            }
            else
            {
                // Grabs the default mesage.
                msg = "The boss ran away! Battle Bot has completed the simulation!";
            }

            return msg;
        }

        // Gets the battle won boss speak key.
        public string GetBattleWonBossSpeakKey()
        {
            return "btl_msg_battleWonBoss";
        }



        // BATTLE LOST //
        // The battle was lost the battle.
        public string GetBattleLostMessage()
        {
            // The message string.
            string msg = "";

            // Checks if defs exists.
            if (defs != null)
            {
                // Grabs the translated message.
                msg = defs["btl_msg_battleLost"];
            }
            else
            {
                // Grabs the default mesage.
                msg = "Battle Bot lost the battle, and had to run away!";
            }

            return msg;
        }

        // Gets the battle lost boss speak key.
        public string GetBattleLostSpeakKey()
        {
            return "btl_msg_battleLost";
        }



        // OPEN TREASURE //
        // The treasure was opened.
        public string GetTakeTreasureMessage()
        {
            // The message string.
            string msg = "";

            // Checks if defs exists.
            if (defs != null)
            {
                // Grabs the translated message.
                msg = defs["btl_msg_takeTreasure"];
            }
            else
            {
                // Grabs the default mesage.
                msg = "Battle Bot has taken the treasure!";
            }

            return msg;
        }

        // Gets the treasure opened speak key.
        public string GetTakeTreasureSpeakKey()
        {
            return "btl_msg_takeTreasure";
        }



        // LEVEL UP //
        // The player got a level up.
        public string GetLevelUpMessage()
        {
            // The message string.
            string msg = "";

            // Checks if defs exists.
            if (defs != null)
            {
                // Grabs the translated message.
                msg = defs["btl_msg_levelUp"];
            }
            else
            {
                // Grabs the default mesage.
                msg = "Battle Bot got a level up!";
            }

            return msg;
        }

        // Gets the level up speak key.
        public string GetLevelUpSpeakKey()
        {
            return "btl_msg_levelUp";
        }



        // LEARN MOVE //
        // The player is trying to learn a new move.
        public string GetLearnMoveMessage()
        {
            // The message string.
            string msg = "";

            // Checks if defs exists.
            if (defs != null)
            {
                // Grabs the translated message.
                msg = defs["btl_msg_learnMove"];
            }
            else
            {
                // Grabs the default mesage.
                msg = "Battle Bot is trying to learn a new move!";
            }

            return msg;
        }

        // Gets the learn move speak key.
        public string GetLearnMoveSpeakKey()
        {
            return "btl_msg_learnMove";
        }

        // LEARN MOVE YES //
        // The player learned the new move.
        public string GetLearnMoveYesMessage(string newMove)
        {
            // The message string.
            string msg = "";

            // Checks if defs exists.
            if (defs != null)
            {
                // Grabs the translated message.
                msg = defs["btl_msg_learnMoveYes"];
            }
            else
            {
                // Grabs the default mesage.
                msg = "Battle Bot learned {0}!";
            }

            // Slotting in content.
            msg = msg.Replace("{0}", newMove);

            return msg;
        }

        // Gets the learn move yes speak key.
        public string GetLearnMoveYesSpeakKey()
        {
            return "btl_msg_learnMoveYes_alt";
        }

        // LEARN MOVE NO //
        // The player did not learn the new move.
        public string GetLearnMoveNoMessage(string newMove)
        {
            // The message string.
            string msg = "";

            // Checks if defs exists.
            if (defs != null)
            {
                // Grabs the translated message.
                msg = defs["btl_msg_learnMoveNo"];
            }
            else
            {
                // Grabs the default mesage.
                msg = "Battle Bot did not learn {0}.";
            }

            // Slotting in content.
            msg = msg.Replace("{0}", newMove);

            return msg;
        }

        // Gets the learn move no speak key.
        public string GetLearnMoveNoSpeakKey()
        {
            return "btl_msg_learnMoveNo_alt";
        }



        // LEARN MULTIPLE MOVES (Treasure Chest) //
        // The player is being offered multiple moves.
        public string GetMultipleMoveOfferMessage()
        {
            // The message string.
            string msg = "";

            // Checks if defs exists.
            if (defs != null)
            {
                // Grabs the translated message.
                msg = defs["btl_msg_multMoveOffer"];
            }
            else
            {
                // Grabs the default mesage.
                msg = "The treasure had 3 moves inside! Choose one of the 3 moves to learn!";
            }

            return msg;
        }

        // Gets the multi-move offer speak key.
        public string GetMultipleMoveOfferSpeakKey()
        {
            return "btl_msg_multMoveOffer";
        }

        // Multiple Move - Skip
        // The player is being offered multiple moves.
        public string GetMultipleMoveOfferSkipMessage()
        {
            // The message string.
            string msg = "";

            // Checks if defs exists.
            if (defs != null)
            {
                // Grabs the translated message.
                msg = defs["btl_msg_multMoveOfferSkip"];
            }
            else
            {
                // Grabs the default mesage.
                msg = "Battle Bot did not learn any of the new moves!";
            }

            return msg;
        }

        // Gets the multi-move offer speak key.
        public string GetMultipleMoveOfferSkipSpeakKey()
        {
            return "btl_msg_multMoveOfferSkip";
        }
    }

}