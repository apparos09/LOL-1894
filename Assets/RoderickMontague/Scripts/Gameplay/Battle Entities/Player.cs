using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;
using UnityEngine.Scripting.APIUpdating;
using UnityEngine.UIElements;

namespace RM_BBTS
{
    // The class for the player.
    public class Player : BattleEntity
    {
        // The run move for the player to select.
        private RunMove runMove;

        // The base stats for the player. (30)
        private float baseMaxHealth = 30;
        private float baseAttack = 30;
        private float baseDefense = 30;
        private float baseSpeed = 30;
        private float baseMaxEnergy = 100;

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
            maxHealth = baseMaxHealth;
            health = maxHealth;

            attack = baseAttack;
            defense = baseDefense;
            speed = baseSpeed;

            maxEnergy = baseMaxEnergy;
            energy = maxEnergy;

            statSpecial = specialty.none;

            // The player levels up faster than the enemies.
            levelRate = 1.5F;

            // TODO: set unique sprite?

            LoadTranslation("bey_player_nme");
        }

        // Start is called before the first frame update
        protected new void Start()
        {
            base.Start();

            // Gets the base data and loads the battle data.
            // BattleEntityGameData baseData = BattleEntityList.Instance.GenerateBattleEntityData(battleEntityId.unknown);
            // LoadBattleData(baseData);

            // Starter moves.
            // Move0 = MoveList.Instance.GenerateMove(moveId.motivate);
            Move0 = MoveList.Instance.GenerateMove(moveId.risk);
            // Move1 = MoveList.Instance.GenerateMove(moveId.wham);
            Move1 = MoveList.Instance.GenerateMove(moveId.torch);
            Move2 = MoveList.Instance.GenerateMove(moveId.kablam);
            // Move3 = MoveList.Instance.GenerateMove(moveId.hpDrain3);
            // Move3 = null;
            Move3 = MoveList.Instance.GenerateMove(moveId.quickBurst);
            // Move3 = MoveList.Instance.GenerateMove(moveId.poke);

            // sprite = data.sprite;

            // Generates and saves a run move.
            runMove = new RunMove();

            // Translates the player's name.
            LoadTranslation("bey_player_nme");
        }

        // Loads the battle game data.
        public override void LoadBattleGameData(BattleEntityGameData data)
        {
            // Saves the player's custom sprite since they don't have a dedicated number.
            // This may be unnecessary.
            string pName = displayName;
            Sprite pSprite = sprite;

            // Loads the data based on the unknown character.
            base.LoadBattleGameData(data);

            // Set name and sprite since those are kept by the player.
            displayName = pName;
            sprite = pSprite;

            // Set these values again to make sure they weren't changed.
            id = 0;
            preEvoId = 0;
            evoId = 0;
            statSpecial = specialty.none;
        }

        // Sets the data using the player's base stats.
        public void SetDataWithBaseStats(ref BattleEntityGameData data)
        {
            // Sets the base 
            data.maxHealth = baseMaxHealth;
            data.attack = baseAttack;
            data.defense = baseDefense;
            data.speed = baseSpeed;
            data.maxEnergy = baseMaxEnergy;

            // This is unneeded.
            data.levelRate = levelRate;

        }

        // Levels up the player.
        public override void LevelUp()
        {
            // base.LevelUp();
            LevelUp(levelRate, specialty.none, 1);

        }

        // Levels up the player.
        public override void LevelUp(float levelRate, specialty special, uint times = 1)
        {
            // Levels up the player.
            base.LevelUp(levelRate, special, times);

            // Restores the player's health and energy levels. This rounds up to a whole number.
            Health += Mathf.Ceil(MaxHealth * LEVEL_UP_RESTORE_PERCENT * times);
            Energy += Mathf.Ceil(MaxEnergy * LEVEL_UP_RESTORE_PERCENT * times);
        }

        // Levels up the player. The enemy's special determines what kind of stat bonus the player gets.
        public void LevelUp(specialty special, uint times = 1)
        {
            LevelUp(levelRate, special, times);
        
            // TODO: implement enemy specialities for level up.
        }

        // Applies the new phase bonus for the player.
        // This gives them a 100+ stat increase.
        public void ApplyNewPhaseBonus()
        {
            // Since each enemy stage has a 100 point difference, this increases the player's stats by 100.

            float total = 100.0F;
            float amount = Mathf.Ceil(total / 4.0F);

            SetHealthRelativeToMaxHealth(MaxHealth + amount);
            Attack += amount;
            Defense += amount;
            Speed += amount;
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