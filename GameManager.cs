/**
 * Creates a mostly empty script to manage the game's state. Facilitates changing
 * between states (level loading, in-level, main menu, paused, etc.) and stores
 * easy access to key components.
 * 
 * Other scripts can easily change behavior based on the current game state by
 * extending BitzawolfGameObject, which itself extends the common Unity GameObject
 * so you still have access to all those tools and properties.
 * 
 * States can be added and deleted without issue. Be sure that the extension method
 * at the end of this file has an entry for your new state. This is how
 * BitzawolfGameObjects are updated based on the current game state.
 * 
 * This script should be attached to an Empty game object in a scene that's loaded
 * right at the start of the game.
 */

using UnityEngine;
using System.Collections.Generic;

namespace Bitzawolf
{
    public class GameManager : MonoBehaviour
    {
        /*************
         * Singleton *
         *************/
        private static GameManager instance = null;

        private void OnEnable()
        {
            if (instance == null)
                instance = this;
        }

        // Get the singleton, accessible from anywhere.
        public static GameManager GetInstance()
        {
            return instance;
        }

        /***********
         * Publics *
         **********/
        // Common, basic game states. Add/delete as needed.
        public enum State
        {
            GAME_INIT,
            MAIN_MENU,
            LEVEL_LOADING,
            LEVEL_STARTING,
            IN_LEVEL,
            LEVEL_ENDING,
            PAUSED
        }

        // Add components that you want to be easily accessible from other scripts here
        public GameObject MainCamera;

        [Space]
        [Header("UI")]
        //public Canvas inLevelUI; Example

        [Space]
        [Header("Debug")]
        public bool debugMode = false;

        /************
         * Privates *
         ***********/
        private State currentState = State.GAME_INIT;
        private Stack<State> stateStack = new Stack<State>();
        private List<BitzawolfGameObject> gameObjects = new List<BitzawolfGameObject>();
        
        private void Start()
        {
            DontDestroyOnLoad(gameObject);
            DontDestroyOnLoad(MainCamera);
            //DontDestroyOnLoad(inLevelUI);

            // Winter Werewolves Prototype work
            currentState = State.IN_LEVEL;
        }

        private void Update()
        {
            if (debugMode)
                UpdateCheats();

            string funcName = currentState.GetUpdateName();

            // We use reflection and dynamic function names here to avoid duplicating the for-each loop
            // inside a switch statement for the State. It's a confusing line of code, but better than duplicated loops.
            foreach (BitzawolfGameObject bgo in gameObjects)
            {
                typeof(BitzawolfGameObject).GetMethod(funcName).Invoke(bgo, new object[] { });
            }
        }

        /**
         * Transitions from this current state to the next state with an option to remember
         * which state was just left so it can be returned to.
         * When this function is called, LeaveState and EnterState events are immediately triggered.
         * 
         * @param State nextState The next State the Game Manager should enter.
         * @param bool saveCurrentState If true, then the current state is pushed onto the State stack
         *      so that the state can be returned to without needing to specify the State itself.
         *      Helpful for situations where a pause menu doesn't know what state the game was just in
         *      but it wants the game to return to whatever it was.
         */
        private void TransitionState(State nextState, bool saveCurrentState = false)
        {
            // TODO - trigger events for Leaving State

            // TODO - trigger events for Entering State

            if (saveCurrentState)
                stateStack.Push(currentState);
            currentState = nextState;
        }

        private void UpdateCheats()
        {
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                // Add ammo or whatever
            }
        }

        /**
         * Adds the BitzawolfGameObject to this Game Manager so that it can be updated and use
         * the manager's deligated state-based update functions.
         */
        public void AddGameObject(BitzawolfGameObject bgo)
        {
            if (bgo == null)
            {
                Debug.LogError("Null BitzawolfGameObject passed to Game Manager AddGameObject");
            }
            gameObjects.Add(bgo);
        }

        /**
         * Removes the BitzawolfGameObject from this Game Manager so that it will no longer be updated
         * and used by the manager's deligated state-based update functions.
         */
        public bool RemoveGameObject(BitzawolfGameObject bgo)
        {
            return gameObjects.Remove(bgo);
        }

        /**
         * Returns the current State the game is in.
         */
        public State GetCurrentState()
        {
            return currentState;
        }

        /**
         * Returns true if the Game Manager has at least one State it can return to.
         */
        public bool hasPreviousState()
        {
            return (stateStack.Count > 0);
        }

        // Simple examples of how to create public functions to change game state.
        /**
         * 
         */
        public void PauseGame()
        {
            TransitionState(State.PAUSED, true);
        }
        
        public void UnpauseGame()
        {
            if (stateStack.Count != 0)
                TransitionState(stateStack.Pop());
            else
                TransitionState(State.IN_LEVEL);
        }
    }

    public static class StateMethods
    {
        public static string GetUpdateName(this GameManager.State state)
        {
            switch (state)
            {
                case GameManager.State.GAME_INIT:
                    return "UpdateInit";
                default:
                case GameManager.State.IN_LEVEL:
                    return "UpdateInLevel";
                case GameManager.State.LEVEL_ENDING:
                    return "UpdateLevelEnding";
                case GameManager.State.LEVEL_LOADING:
                    return "UpdateLevelLoading";
                case GameManager.State.LEVEL_STARTING:
                    return "UpdateLevelStarting";
                case GameManager.State.MAIN_MENU:
                    return "UpdateMainMenu";
                case GameManager.State.PAUSED:
                    return "UpdatePaused";
            }
        }
    }
}