using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RM_BBTS
{
    // A tester for enemy animations.
    public class EnemyAnimationTester : MonoBehaviour
    {
        // The animator.
        public Animator animator;

        // The id of the current entity whose animations are being played.
        public battleEntityId entityId = 0;

        // Start is called before the first frame update
        void Start()
        {

        }

        // The current animation id.
        public void SetCurrentAnimation(battleEntityId id)
        {
            entityId = id;

            // Checks the ID.
            switch(entityId)
            {
                case battleEntityId.combatBot:
                    animator.Play("BEY - Combat Bot - Idle");
                    break;
                case battleEntityId.ufo1:
                    animator.Play("BEY - UFO 1 - Idle");
                    break;
                case battleEntityId.ufo2:
                    animator.Play("BEY - UFO 2 - Idle");
                    break;
                case battleEntityId.ufo3:
                    animator.Play("BEY - UFO 3 - Idle");
                    break;

                case battleEntityId.insect1:
                case battleEntityId.insect2:
                case battleEntityId.ghost1:
                case battleEntityId.ghost2:
                case battleEntityId.comet:
                case battleEntityId.sunRock1:
                case battleEntityId.sunRock2:
                case battleEntityId.moonRock1:
                case battleEntityId.moonRock2:
                case battleEntityId.fireBot1:
                case battleEntityId.fireBot2:
                case battleEntityId.waterBot1:
                case battleEntityId.waterBot2:
                case battleEntityId.earthBot1:
                case battleEntityId.earthBot2:
                case battleEntityId.airBot1:
                case battleEntityId.airBot2:
                case battleEntityId.sharp1:
                case battleEntityId.sharp2:
                case battleEntityId.cBugRed1:
                case battleEntityId.cBugRed2:
                case battleEntityId.cBugBlue1:
                case battleEntityId.cBugBlue2:
                case battleEntityId.cBugYellow1:
                case battleEntityId.cBugYellow2:
                case battleEntityId.blackHole:
                case battleEntityId.planet1:
                case battleEntityId.planet2:
                default:
                    animator.Play("No Idle", 1);
                    break;
            }
        }

        // Goes onto the next entity.
        public void NextEntity()
        {
            int temp = (int)entityId;
            temp++;

            // If at the max, go to 0.
            if(temp >= BattleEntityList.BATTLE_ENTITY_ID_COUNT)
            {
                temp = 0;
            }

            // Set the animation.
            SetCurrentAnimation((battleEntityId)temp);
        }

        // Goes onto the previous entity.
        public void PreviousEntity()
        {
            int temp = (int)entityId;
            temp--;

            // If now set to 0, go to the max.
            if (temp < 0)
            {
                temp = BattleEntityList.BATTLE_ENTITY_ID_COUNT - 1;
            }

            // Set the animation.
            SetCurrentAnimation((battleEntityId)temp);
        }

        // // Update is called once per frame
        // void Update()
        // {
        // 
        // }
    }
}