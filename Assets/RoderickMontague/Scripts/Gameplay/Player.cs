using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RM_BBTS
{
    public class Player : BattleEntity
    {
        // Start is called before the first frame update
        new void Start()
        {
            base.Start();

            // The player is 'id' 0.
            id = 0;

            // Starter move.
            Move0 = MoveList.Instance.GenerateMV00();
        }

        // Update is called once per frame
        new void Update()
        {
            base.Update();
        }
    }
}