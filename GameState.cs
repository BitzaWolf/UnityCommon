/**
 * Creates a mostly empty script to manage the game's state. Facilitates changing
 * between states (level loading, in-level, main menu, paused, etc.) and stores
 * easy access to key components.
 * This script should be attached to an Empty game object in a scene that's loaded
 * right at the start of the game.
 */

using UnityEngine;

namespace Bitzawolf
{
    public class GameManager : MonoBehaviour
    {
        // Create a singleton
        private static GameManager instance = null;

        private void OnEnable()
        {
            if (instance == null)
                instance = this;
        }

        // Get the singleton, accessible from anywhere.
        public static GameManager gm()
        {
            return instance;
        }

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
        public State currentState = State.GAME_INIT;
        public GameObject mainCamera;

        [Space]
        [Header("UI")]
        public Canvas inLevelUI;

        [Space]
        [Header("Debug")]
        public bool debugMode = false;

        // Privates
        private State previousSate = State.GAME_INIT;

        // Pre
        private void Start()
        {
            DontDestroyOnLoad(gameObject);
            DontDestroyOnLoad(mainCamera);
            DontDestroyOnLoad(inLevelUI);
        }

        private void Update()
        {
            switch (currentState)
            {
                // Add additional states here to update them separately
                case State.IN_LEVEL: updateInLevel(); break;
                default: break;
            }
            if (debugMode)
                updateCheats();
        }

        private void updateInLevel()
        {
            if (Input.GetButtonDown("Fire"))
            {
                // Fire projectile or something
            }
        }

        // Cleans up any code/data/objects before leaving the current state
        private void leaveState()
        {
            switch(currentState)
            {
                // Add a state case here to clean up whatever as needed.
                default: break;
            }
        }

        // Changes the current state to the next target state, intializing any data or objects as needed.
        private void transitionState(State nextState)
        {
            leaveState();

            switch(nextState)
            {
                // add a state case here to initalize data
                default: break;
            }
            
            previousSate = currentState;
            currentState = nextState;
        }

        private void updateCheats()
        {
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                // Add ammo or whatever
            }
        }
        
        // Simple examples of how to create public functions to change game state.
        public void pauseGame()
        {
            transitionState(State.PAUSED);
        }
        
        public void unpauseGame()
        {
            transitionState(previousState);
        }
    }
}