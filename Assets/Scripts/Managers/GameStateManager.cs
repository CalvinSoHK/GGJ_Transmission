using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Just keeps track of what our game state is.
public static class GameStateManager {

    //Enum for our game states
    public enum GameState { Waiting, PlayerInput, Processing };
    public static GameState STATE = GameState.Waiting;

    //Time since we entered this state
    static float TIME;

    //Helper function that tells us how long this state has been running
    public static float GetCurrentStateTimeElapsed()
    {
        return Time.time - TIME;
    }

    //Helper function that sets the state
    public static void SetCurrentState(GameState TEMP)
    {
        STATE = TEMP;
        TIME = Time.time;
    }
}
