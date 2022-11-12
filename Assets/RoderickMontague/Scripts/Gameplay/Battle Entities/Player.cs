using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Scripting.APIUpdating;

namespace RM_BBTS
{
    // The class for the player.
    public class Player : BattleEntity
    {
        // The run move for the player to select.
        private RunMove runMove;

        // Setting the player's stats.
        protected new void Awake()
        {
            // The player is 'id' 0.
            id = 0;
            preEvoId = 0;
            evoId = 0;

            displayName = "<Player>";
            displayNameSpeakKey = "bey_player_nme";
            level = 1;

            // Saves the default stats (maybe you should hardcode this).
            maxHealth = 50;
            health = maxHealth;

            attack = 12;
            defense = 8;
            speed = 6;

            maxEnergy = 30;
            energy = maxEnergy;

            LoadTranslation("bey_player_nme");
        }

        // Start is called before the first frame update
        protected new void Start()
        {
            base.Start();

            // Gets the base data and loads the battle data.
            BattleEntityGameData baseData = BattleEntityList.Instance.GenerateBattleEntityData(battleEntityId.unknown);
            // LoadBattleData(baseData);

            // Starter moves.
            Move0 = MoveList.Instance.GenerateMove(moveId.bam);
            Move1 = MoveList.Instance.GenerateMove(moveId.wham);
            Move2 = MoveList.Instance.GenerateMove(moveId.kablam);
            Move3 = MoveList.Instance.GenerateMove(moveId.sonicwave);
            // Move3 = null;
            // Move3 = MoveList.Instance.GenerateMove(moveId.poke);

            // sprite = data.sprite;

            // Generates and saves a run move.
            runMove = new RunMove();

            // Translates the player's name.
            LoadTranslation("bey_player_nme");
        }

        // Levels up the player.
        public override void LevelUp(uint times = 1)
        {
            // Levels up the player.
            base.LevelUp(times);

            // Restores the player's health and energy levels.
            Health += MaxHealth * LEVEL_UP_RESTORE_PERCENT * times;
            Energy += MaxEnergy * LEVEL_UP_RESTORE_PERCENT * times;
        }

        // Selects the run move. Only the player has the run move.
        public void SelectRun()
        {
            selectedMove = runMove;
        }

        // Update is called once per frame
        protected new void Update()
        {
            base.Update();
        }
    }
}