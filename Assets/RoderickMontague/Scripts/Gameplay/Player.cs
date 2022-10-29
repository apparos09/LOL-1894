using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Scripting.APIUpdating;

namespace RM_BBTS
{
    // The class for the player.
    public class Player : BattleEntity
    {
        // Start is called before the first frame update
        new void Start()
        {
            base.Start();

            // Gets the base data and loads the battle data.
            BattleEntityData baseData = BattleEntityList.Instance.GenerateBattleEntityData(battleEntityId.unknown);
            LoadBattleData(baseData);

            // The player is 'id' 0.
            id = baseData.id;
            preEvoId = baseData.id;
            evoId = baseData.id;
            level = baseData.level;

            // Saves the default stats (maybe you should hardcode this).
            maxHealth = baseData.maxHealth;
            health = baseData.health;

            attack = baseData.attack;
            defense = baseData.defense;
            speed = baseData.speed;

            maxEnergy = baseData.maxEnergy;
            energy = baseData.energy;

            // Starter move.
            Move0 = MoveList.Instance.GenerateMove(moveId.hit);
            Move1 = null;
            Move2 = null;
            Move3 = null;

            // sprite = data.sprite;
        }

        // Update is called once per frame
        new void Update()
        {
            base.Update();
        }
    }
}