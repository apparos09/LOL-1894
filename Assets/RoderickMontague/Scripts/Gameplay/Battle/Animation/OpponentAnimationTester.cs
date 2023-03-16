using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RM_BBTS
{
    // A tester for opponent animations.
    public class OpponentAnimationTester : MonoBehaviour
    {
        // The animator.
        public Animator animator;

        // The sprite renderer.
        public SpriteRenderer spriteRenderer;

        // The id of the current entity whose animations are being played.
        public battleEntityId entityId = 0;

        // The battle animation number.
        public int battleAnim = 0;

        // The amount of battle animations.
        private const int BATTLE_ANIM_COUNT = 4;

        public Sprite treasureSprite;

        // Start is called before the first frame update
        void Start()
        {

        }

        // Sets the current battle animation.
        public void SetCurrentBattleAnimation(int anim)
        {
            battleAnim = Mathf.Clamp(anim, 0, BATTLE_ANIM_COUNT);
            animator.SetInteger("anim", battleAnim);
        }

        // The next battle animation.
        public void NextBattleAnimation()
        {
            int temp = battleAnim + 1;

            // Bounds setting.
            if (temp > BATTLE_ANIM_COUNT)
                temp = 0;

            SetCurrentBattleAnimation(temp);
        }

        // The previous battle animation.
        public void PreviousBattleAnimation()
        {
            int temp = battleAnim - 1;

            // Bounds setting.
            if (temp < 0)
                temp = BATTLE_ANIM_COUNT - 1;

            SetCurrentBattleAnimation(temp);
        }

        // The current animation id.
        public void SetCurrentEntityAnimation(battleEntityId id)
        {
            // NOTE: you don't need to 
            entityId = id;
            // const int LAYER = 1;

            // Checks the ID.
            switch(entityId)
            {
                case battleEntityId.treasure:
                    animator.Play("BEY - Treasure - Close");
                    break;

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
                    animator.Play("BEY - Insect 1 - Idle");
                    break;

                case battleEntityId.insect2:
                    animator.Play("BEY - Insect 2 - Idle");
                    break;

                case battleEntityId.spaceGhost1:
                    animator.Play("BEY - Space Ghost 1 - Idle");
                    break;

                case battleEntityId.spaceGhost2:
                    animator.Play("BEY - Space Ghost 2 - Idle");
                    break;

                case battleEntityId.comet:
                    animator.Play("BEY - Comet - Idle");
                    break;

                case battleEntityId.sunRock1:
                    animator.Play("BEY - Sun Rock 1 - Idle");
                    break;

                case battleEntityId.sunRock2:
                    animator.Play("BEY - Sun Rock 2 - Idle");
                    break;

                case battleEntityId.moonRock1:
                    animator.Play("BEY - Moon Rock 1 - Idle");
                    break;

                case battleEntityId.moonRock2:
                    animator.Play("BEY - Moon Rock 2 - Idle");
                    break;

                case battleEntityId.fireBot1:
                    animator.Play("BEY - Fire Bot 1 - Idle");
                    break;

                case battleEntityId.fireBot2:
                    animator.Play("BEY - Fire Bot 2 - Idle");
                    break;

                case battleEntityId.waterBot1:
                    animator.Play("BEY - Water Bot 1 - Idle");
                    break;

                case battleEntityId.waterBot2:
                    animator.Play("BEY - Water Bot 2 - Idle");
                    break;

                case battleEntityId.earthBot1:
                    animator.Play("BEY - Earth Bot 1 - Idle");
                    break;

                case battleEntityId.earthBot2:
                    animator.Play("BEY - Earth Bot 2 - Idle");
                    break;

                case battleEntityId.airBot1:
                    animator.Play("BEY - Air Bot 1 - Idle");
                    break;

                case battleEntityId.airBot2:
                    animator.Play("BEY - Air Bot 2 - Idle");
                    break;

                case battleEntityId.sharp1:
                    animator.Play("BEY - Sharp 1 - Idle");
                    break;

                case battleEntityId.sharp2:
                    animator.Play("BEY - Sharp 2 - Idle");
                    break;

                case battleEntityId.virusRed1:
                    animator.Play("BEY - Red Virus 1 - Idle");
                    break;

                case battleEntityId.virusRed2:
                    animator.Play("BEY - Red Virus 2 - Idle");
                    break;

                case battleEntityId.virusBlue1:
                    animator.Play("BEY - Blue Virus 1 - Idle");
                    break;

                case battleEntityId.virusBlue2:
                    animator.Play("BEY - Blue Virus 2 - Idle");
                    break;

                case battleEntityId.virusYellow1:
                    animator.Play("BEY - Yellow Virus 1 - Idle");
                    break;

                case battleEntityId.virusYellow2:
                    animator.Play("BEY - Yellow Virus 2 - Idle");
                    break;

                case battleEntityId.blackHole:
                    animator.Play("BEY - Black Hole - Idle");
                    break;

                case battleEntityId.planet1:
                    animator.Play("BEY - Planet 1 - Idle");
                    break;

                case battleEntityId.planet2:
                    animator.Play("BEY - Planet 2 - Idle");
                    break;

                default:
                    animator.Play("No Idle");
                    spriteRenderer.sprite = null;
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
            SetCurrentEntityAnimation((battleEntityId)temp);
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
            SetCurrentEntityAnimation((battleEntityId)temp);
        }

        // // Update is called once per frame
        // void Update()
        // {
        // 
        // }
    }
}