using LoLSDK;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SimpleJSON;
using System.Security.Cryptography;

namespace RM_BBTS
{
    // The battle messages for the game.
    public class BattleMessages : MonoBehaviour
    {
        // the instance of the game settings.
        private static BattleMessages instance;

        // The language definitions.
        JSONNode defs;

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

        // Returns the instance of the game settings.
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
                        // makes a new settings object.
                        GameObject go = new GameObject("Battle Messages");

                        // adds the instance component to the new object.
                        instance = go.AddComponent<BattleMessages>();
                    }

                }

                // returns the instance.
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

        // MOVE USE //

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
                msg = "<{0} used {1}!>";
            }

            // Replaces the information.
            msg = msg.Replace("{0}", user);
            msg = msg.Replace("{1}", move);

            return msg;
        }

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
                msg = "The move hit!>";
            }

            return msg;
        }

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
                msg = "<The move hit, and it did critical damage!>";
            }

            return msg;
        }

        // Gets the move "not enough power" message.
        public string GetMoveNoPowerMessage(string user)
        {
            // The message string.
            string msg = "";

            // Checks if defs existed.
            if (defs != null)
            {
                // Grabs the translated message.
                msg = defs["btl_msg_mve_moveNoPower"];
            }
            else
            {
                // Grabs the default mesage.
                msg = "<{0} does not have enough power to use their move!>";
            }

            // Replaces the information.
            msg = msg.Replace("{0}", user);

            return msg;
        }

        // Gets the move missed message.
        public string GetMoveMissedMessage()
        {
            // The message string.
            string msg = "";

            // Checks if defs existed.
            if (defs != null)
            {
                // Grabs the translated message.
                msg = defs["btl_msg_mve_moveMissed"];
            }
            else
            {
                // Grabs the default mesage.
                msg = "<The move missed!>";
            }

            return msg;
        }

        // Gets the move missed message.
        public string GetMoveFailedMessage()
        {
            // The message string.
            string msg = "";

            // Checks if defs existed.
            if (defs != null)
            {
                // Grabs the translated message.
                msg = defs["btl_msg_mve_moveFailed"];
            }
            else
            {
                // Grabs the default mesage.
                msg = "<The move failed!>";
            }

            return msg;
        }

        // Gets the move missed message.
        public string GetMoveBurnedMessage()
        {
            // The message string.
            string msg = "";

            // Checks if defs existed.
            if (defs != null)
            {
                // Grabs the translated message.
                msg = defs["btl_msg_mve_moveBurned"];
            }
            else
            {
                // Grabs the default mesage.
                msg = "<The target has been burned!>";
            }

            return msg;
        }

        // Gets the move missed message.
        public string GetMoveParalyzedMessage()
        {
            // The message string.
            string msg = "";

            // Checks if defs existed.
            if (defs != null)
            {
                // Grabs the translated message.
                msg = defs["btl_msg_mve_moveParalyzed"];
            }
            else
            {
                // Grabs the default mesage.
                msg = "<The target has been paralyzed!>";
            }

            return msg;
        }

        // MOVE EFFECT //
        public string GetMoveChargeUsedMessage(string user)
        {
            // The message string.
            string msg = "";

            // Checks if defs existed.
            if (defs != null)
            {
                // Grabs the translated message.
                msg = defs["btl_msg_mve_chargeUsed"];
            }
            else
            {
                // Grabs the default mesage.
                msg = "<{0} charged their energy!>";
            }

            // Slotting in content.
            msg = msg.Replace("{0}", user);

            return msg;
        }

        // The move run failed.
        public string GetMoveRunFailedMessage(string user)
        {
            // The message string.
            string msg = "";

            // Checks if defs existed.
            if (defs != null)
            {
                // Grabs the translated message.
                msg = defs["btl_msg_mve_runFailed"];
            }
            else
            {
                // Grabs the default mesage.
                msg = "<{0} failed to run away!>";
            }

            // Slotting in content.
            msg = msg.Replace("{0}", user);

            return msg;
        }

        // The move caused nothing to happen.
        public string GetMoveNothingMessage()
        {
            // The message string.
            string msg = "";

            // Checks if defs existed.
            if (defs != null)
            {
                // Grabs the translated message.
                msg = defs["btl_msg_mve_nothing"];
            }
            else
            {
                // Grabs the default mesage.
                msg = "<Nothing happened.>";
            }

            return msg;
        }

        // The target was burned.
        public string GetBurnedMessage(string infected)
        {
            // The message string.
            string msg = "";

            // Checks if defs existed.
            if (defs != null)
            {
                // Grabs the translated message.
                msg = defs["btl_msg_burned"];
            }
            else
            {
                // Grabs the default mesage.
                msg = "<{0} took burn damage!>";
            }

            // Slotting in content.
            msg = msg.Replace("{0}", infected);

            return msg;
        }

        // The target was paralyzed.
        public string GetParalyzedMessage(string infected)
        {
            // The message string.
            string msg = "";

            // Checks if defs existed.
            if (defs != null)
            {
                // Grabs the translated message.
                msg = defs["btl_msg_paralyzed"];
            }
            else
            {
                // Grabs the default mesage.
                msg = "<{0} is immobilized, and can't move!>";
            }

            // Slotting in content.
            msg = msg.Replace("{0}", infected);

            return msg;
        }

        // BATTLE FINISH MESSAGES //
        // The battle was won.
        public string GetBattleWonMessage()
        {
            // The message string.
            string msg = "";

            // Checks if defs existed.
            if (defs != null)
            {
                // Grabs the translated message.
                msg = defs["btl_msg_battleWon"];
            }
            else
            {
                // Grabs the default mesage.
                msg = "<The player has won the battle!>";
            }

            return msg;
        }

        // The battle was won against the boss.
        public string GetBattleWonBossMessage()
        {
            // The message string.
            string msg = "";

            // Checks if defs existed.
            if (defs != null)
            {
                // Grabs the translated message.
                msg = defs["btl_msg_battleWonBoss"];
            }
            else
            {
                // Grabs the default mesage.
                msg = "<The player has beaten the final boss!>";
            }

            return msg;
        }

        // The battle was lost the battle.
        public string GetBattleLostMessage()
        {
            // The message string.
            string msg = "";

            // Checks if defs existed.
            if (defs != null)
            {
                // Grabs the translated message.
                msg = defs["btl_msg_battleLost"];
            }
            else
            {
                // Grabs the default mesage.
                msg = "<The player has lost the battle!>";
            }

            return msg;
        }

        // The treasure was opened.
        public string GetOpenTreasureMessage()
        {
            // The message string.
            string msg = "";

            // Checks if defs existed.
            if (defs != null)
            {
                // Grabs the translated message.
                msg = defs["btl_msg_openTreasure"];
            }
            else
            {
                // Grabs the default mesage.
                msg = "<The player has opened the treasure!>";
            }

            return msg;
        }

        // The player got a level up.
        public string GetLevelUpMessage()
        {
            // The message string.
            string msg = "";

            // Checks if defs existed.
            if (defs != null)
            {
                // Grabs the translated message.
                msg = defs["btl_msg_levelUp"];
            }
            else
            {
                // Grabs the default mesage.
                msg = "<The player got a level up!>";
            }

            return msg;
        }

        // The player is trying to learn a new move.
        public string GetLearnMoveMessage()
        {
            // The message string.
            string msg = "";

            // Checks if defs existed.
            if (defs != null)
            {
                // Grabs the translated message.
                msg = defs["btl_msg_learnMove"];
            }
            else
            {
                // Grabs the default mesage.
                msg = "<The player is trying to learn a new move!>";
            }

            return msg;
        }

        // The player learned the new move.
        public string GetLearnMoveYesMessage(string newMove)
        {
            // The message string.
            string msg = "";

            // Checks if defs existed.
            if (defs != null)
            {
                // Grabs the translated message.
                msg = defs["btl_msg_learnMoveYes"];
            }
            else
            {
                // Grabs the default mesage.
                msg = "<The player learned {0}!>";
            }

            // Slotting in content.
            msg = msg.Replace("{0}", newMove);

            return msg;
        }

        // The player did not learn the new move.
        public string GetLearnMoveNoMessage(string newMove)
        {
            // The message string.
            string msg = "";

            // Checks if defs existed.
            if (defs != null)
            {
                // Grabs the translated message.
                msg = defs["btl_msg_learnMoveNo"];
            }
            else
            {
                // Grabs the default mesage.
                msg = "<The player did not learn {0}.>";
            }

            // Slotting in content.
            msg = msg.Replace("{0}", newMove);

            return msg;
        }
    }

}