using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RM_BBTS
{
    // Manages the battle operations for the game. This becomes active when the game enters a battle state.
    public class BattleManager : MonoBehaviour
    {
        // Becomes 'true' when the overworld is initialized.
        public bool initialized = false;

        // The player.
        public Player player;

        // The enemy. Each room will only have one enemy.
        public Enemy enemy;

        // Start is called before the first frame update
        void Start()
        {
            // ...
        }

        // This function is called when the object becomes enabled and active
        private void OnEnable()
        {

        }

        // This function is called when the behaviour becomes disabled or inactive
        private void OnDisable()
        {

        }

        // Initializes the overworld.
        public void Initialize()
        {
            initialized = true;
        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}