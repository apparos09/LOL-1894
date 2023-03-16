using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RM_BBTS
{
    // A treasure entity.
    public class Treasure : BattleEntity
    {
        [Header("Treasure")]
        
        // The sprite for the treasure chest when it's closed.
        public Sprite closedSprite;

        // The sprite for the treasure chest when it's open.
        public Sprite openSprite;

        // Gets set to 'true', when the treasure chest has been opened.
        public bool closed = true;

        // Start is called before the first frame update
        new void Start()
        {
            base.Start();

            // The treasure is 'id' 0.
            id = battleEntityId.treasure;

            // Setting the stats shouldn't be needed, but just in case...
            level = 1;

            maxHealth = 1;
            health = maxHealth;

            attack = 1;
            defense = 1;
            speed = 1;

            maxEnergy = 100;
            energy = maxEnergy;

            // If the treasure is called to use a move it will always use charge, which won't do anything for it.
            selectedMove = MoveList.Instance.ChargeMove;
        }

        // Used to load battle game data.
        public override void LoadBattleGameData(BattleEntityGameData data)
        {
            // Calls the base function for the data.
            base.LoadBattleGameData(data);

            // Sets 'closed' to true (opened treasure chests cannot be accessed again).
            closed = true;
        }


        // Update is called once per frame
        new void Update()
        {
            base.Update();
        }
    }
}