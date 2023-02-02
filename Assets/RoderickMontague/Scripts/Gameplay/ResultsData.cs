using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RM_BBTS
{
    // Holds the results data for the game, which is read in during the results screen.
    public class ResultsData : MonoBehaviour
    {
        // The final score for the game session.
        public int finalScore = 0;

        // Rooms cleared by the player.
        public int roomsCompleted = 0;

        // Total rooms in the game.
        public int roomsTotal = 0;

        // The total time in seconds.
        public float totalTime = 0.0F;

        // The total turns.
        public int totalTurns = 0;

        // The amount of questions used, and the total amount of correct responses.
        public int questionsUsed = 0;
        public int questionsUsedNoRepeats = 0;

        // The amount of questions correct, and the amount of questions correct without repeated questions.
        public int questionsCorrect = 0;
        public int questionsCorrectNoRepeats = 0;

        // Final level
        public uint finalLevel = 0;

        // The 4 moves.
        public string move0 = "-";
        public string move1 = "-";
        public string move2 = "-";
        public string move3 = "-";
    }
}