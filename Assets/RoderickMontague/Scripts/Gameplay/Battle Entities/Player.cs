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
        // Determines if the player uses the debug stats or not.
        private const bool USE_DEBUG_STATS = false;

        // The base stats for the player. (30)
        private float baseMaxHealth = (USE_DEBUG_STATS) ? 999 : 40;
        private float baseAttack = (USE_DEBUG_STATS) ? 999 : 50;
        private float baseDefense = (USE_DEBUG_STATS) ? 999 : 45;
        private float baseSpeed = (USE_DEBUG_STATS) ? 999 : 35;
        private float baseMaxEnergy = 100;

        // Level Up Rate
        public const float LEVEL_UP_RATE = 1.10F;

        // Restoration percents.
        public const float LEVEL_UP_HEALTH_RESTORE_PERCENT = 0.30F;
        public const float LEVEL_UP_ENERGY_RESTORE_PERCENT = 0.30F;

        // The stat total for the phase bonus, which is evenly split between health, attack, defense, and speed.
        public const float PHASE_BONUS_STAT_TOTAL = 120.0F; // 120/4 = 30

        // A boost the player gets upon getting a game over.
        public const float GAME_OVER_BONUS_STAT_TOTAL = 60.0F; // 60 / 4 = 15

        // Setting the player's stats.
        protected new void Awake()
        {
            // The player is 'id' 0.
            id = 0;
            preEvoId = 0;
            evoId = 0;

            // displayName = "Player";
            displayName = "Battle Bot";
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
            levelRate = LEVEL_UP_RATE;

            // NOTE: the player doesn't use the battle entity sprite, so nothing is set.

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
            // TEST
            // // Move0 = MoveList.Instance.GenerateMove(moveId.motivate);
            // Move0 = MoveList.Instance.GenerateMove(moveId.risk);
            // // Move1 = MoveList.Instance.GenerateMove(moveId.wham);
            // // Move1 = MoveList.Instance.GenerateMove(moveId.torch); // burn
            // Move1 = MoveList.Instance.GenerateMove(moveId.electrify); // paralysis
            // Move2 = MoveList.Instance.GenerateMove(moveId.kablam);
            // // Move3 = MoveList.Instance.GenerateMove(moveId.hpDrain3);
            // // Move3 = null;
            // Move3 = MoveList.Instance.GenerateMove(moveId.torch);

            // // TEST
            // Move0 = MoveList.Instance.GenerateMove(moveId.burnBoostUser);
            // Move1 = MoveList.Instance.GenerateMove(moveId.paraBoostUser);
            // Move2 = MoveList.Instance.GenerateMove(moveId.shield2);
            // Move3 = MoveList.Instance.GenerateMove(moveId.paraBoostTarget);

            // ACTUAL
            Move0 = MoveList.Instance.GenerateMove(moveId.bam);
            Move1 = MoveList.Instance.GenerateMove(moveId.laserShot);
            Move2 = MoveList.Instance.GenerateMove(moveId.shield1);
            Move3 = null;

            // sprite = data.sprite;

            // Translates the player's name.
            LoadTranslation("bey_player_nme");
        }

        // Generate the battle entity data with base stats.
        public override BattleEntityGameData GenerateBattleEntityGameDataWithBaseStats()
        {
            // Goes based off of the unknown parameter, then replaces the data.
            BattleEntityGameData data = base.GenerateBattleEntityGameDataWithBaseStats();
            SetDataWithBaseStats(ref data);
            return data;
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
        public override void LevelUp(specialty special, uint times = 1)
        {
            // Levels up the player.
            base.LevelUp(special, times);

            // Restores the player's health and energy levels. This rounds up to a whole number.
            // HEALTH
            // Health += Mathf.Ceil(MaxHealth * LEVEL_UP_HEALTH_RESTORE_PERCENT * times);
            // Now uses a dedicated function.
            RestoreHealth(LEVEL_UP_HEALTH_RESTORE_PERCENT * times);

            // ENERGY
            // Energy += Mathf.Ceil(MaxEnergy * LEVEL_UP_ENERGY_RESTORE_PERCENT * times);
            // This now rounds to a whole number.
            RestoreEnergy(LEVEL_UP_ENERGY_RESTORE_PERCENT * times, 0);
        }

        // TODO: move this to the battle entity script and have enemies that don't evolve get a stat boost too?
        // Applies the new phase bonus for the player.
        // This gives them a 100+ stat increase.
        public void ApplyNewPhaseBonus()
        {
            // Increases the player's stats by a total amount equal to PHASE_BONUS_STAT_TOTAL.
            float amount = Mathf.Ceil(PHASE_BONUS_STAT_TOTAL / 4.0F);

            SetHealthRelativeToMaxHealth(MaxHealth + amount);
            Attack += amount;
            Defense += amount;
            Speed += amount;
        }

        // Selects the run move. Only the player has the run move.
        public void SelectRun()
        {
            selectedMove = MoveList.Instance.RunMove;
        }

        // // Called when a battle turn is happening.
        // public override void OnBattleTurn()
        // {
        //     // Calls the parent's battle turn function.
        //     base.OnBattleTurn();
        // }

        // Update is called once per frame
        protected new void Update()
        {
            base.Update();
        }
    }
}