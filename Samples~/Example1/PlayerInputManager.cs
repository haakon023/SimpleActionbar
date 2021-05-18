using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace SimpleActionbar
{
    public class PlayerInputManager : MonoBehaviour
    {
        public InputActionAsset InputActionAssets;

        public static event Action<string> ActionBarEvent;

        public void Awake()
        {
            InputActionAssets.FindAction("ActionButton1").performed += ActionbarPressed;
            InputActionAssets.FindAction("ActionButton2").performed += ActionbarPressed;
            InputActionAssets.FindAction("ActionButton3").performed += ActionbarPressed;
            InputActionAssets.FindAction("ActionButton4").performed += ActionbarPressed;
            InputActionAssets.FindAction("ActionButton5").performed += ActionbarPressed;
            InputActionAssets.FindAction("ActionButton6").performed += ActionbarPressed;
            InputActionAssets.FindAction("ActionButton7").performed += ActionbarPressed;
            InputActionAssets.FindAction("ActionButton8").performed += ActionbarPressed;
            InputActionAssets.FindAction("ActionButton9").performed += ActionbarPressed;
            InputActionAssets.FindAction("ActionButton10").performed += ActionbarPressed;
        }

        private void ActionbarPressed(InputAction.CallbackContext obj)
        {
            ActionBarEvent?.Invoke(obj.action.name);
        }
        
        
    }
}