using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RM_BBTS
{
    // A treasure entity.
    public class Treasure : BattleEntity
    {
        // Start is called before the first frame update
        new void Start()
        {
            base.Start();

            // The treasure is 'id' 0.
            id = battleEntityId.treasure;
        }

        // Update is called once per frame
        new void Update()
        {
            base.Update();
        }
    }
}