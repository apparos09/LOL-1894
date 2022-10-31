using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Scripting.APIUpdating;

namespace RM_BBTS
{
    // The class for the player.
    public class Player : BattleEntity
    {
        // Setting the player's stats.
        protected new void Awake()
        {
            // The player is 'id' 0.
            id = 0;
            preEvoId = 0;
            evoId = 0;
            level = 1;

            // Saves the default stats (maybe you should hardcode this).
            maxHealth = 10;
            health = 10;

            attack = 3;
            defense = 2;
            speed = 1;

            maxEnergy = 10;
            energy = 10;
        }

        // Start is called before the first frame update
        protected new void Start()
        {
            base.Start();

            // Gets the base data and loads the battle data.
            BattleEntityData baseData = BattleEntityList.Instance.GenerateBattleEntityData(battleEntityId.unknown);
            LoadBattleData(baseData);

            // Starter moves.
            Move0 = MoveList.Instance.GenerateMove(moveId.hit);
            Move1 = MoveList.Instance.GenerateMove(moveId.bam);
            Move2 = MoveList.Instance.GenerateMove(moveId.wham);
            Move3 = MoveList.Instance.GenerateMove(moveId.kablam);

            // sprite = data.sprite;
        }

        // Update is called once per frame
        protected new void Update()
        {
            base.Update();
        }
    }
}