using UnityEngine;
using UnityEngine.InputSystem;

namespace ZoloSim.EngineCore
{
    [RequireComponent(typeof(PlayerInputManager))]
    public class ControllerManager : MonoBehaviour
    {
        public static ControllerManager Instance { get; private set; }
        PlayerInputManager playerInputManager => GetComponent<PlayerInputManager>();

        private void Awake()
        {
            // Ensure only one instance of ControllerManager exists
            if (Instance != null && Instance != this)
                Debug.LogError("Multiple instances of ControllerManager detected.");

            Instance = this;
            DontDestroyOnLoad(gameObject);
        }

        public void OnPlayerJoin()
        {
            if (playerInputManager.playerCount == 1)
                GameObject.FindWithTag("MainCamera")?.SetActive(false);
        }

        public void OnPlayerLeave()
        {
            if (playerInputManager.playerCount == 0)
                GameObject.FindWithTag("MainCamera")?.SetActive(true);
        }
    }
}