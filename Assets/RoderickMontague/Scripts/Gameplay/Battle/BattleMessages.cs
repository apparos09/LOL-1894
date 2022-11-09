using LoLSDK;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SimpleJSON;

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
        //     msg.Replace("{0}", user);
        //     msg.Replace("{1}", move);
        // 
        //     return msg;
        // }

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
            msg.Replace("{0}", user);
            msg.Replace("{1}", move);

            return msg;
        }

        // Gets the move hit message.
        public string GetMoveHitMessage(string user, string move)
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
    }
}