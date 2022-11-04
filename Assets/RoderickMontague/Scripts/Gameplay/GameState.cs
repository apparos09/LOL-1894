using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RM_BBTS
{
    // the state of the game.
    public enum gameState { none, overworld, battle }

    // The game state manager.
    public abstract class GameState : MonoBehaviour
    {
        // Initializes the overworld.
        public abstract void Initialize();


        // Called when the mouse hovers over the object.
        public abstract void OnMouseHovered(GameObject hoveredObject);

        // Called when the mouse interacts with an entity.
        public abstract void OnMouseInteract(GameObject heldObject);

        // Called when the user's touch interacts with an entity.
        public abstract void OnTouchInteract(GameObject touchedObject, Touch touch);

        // Called wehn the game state receives the interaction object.
        protected abstract void OnInteractReceive(GameObject gameObject);

        // A function to call when a tutorial starts.
        public abstract void OnTutorialStart();

        // A function to call when a tutorial ends.
        public abstract void OnTutorialEnd();
    }
}