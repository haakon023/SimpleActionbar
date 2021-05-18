using SimpleActionbar.ExampleCode;
using UnityEngine;
using UnityEngine.InputSystem.Interactions;
using UnityEngine.UIElements;

namespace SimpleActionbar.ExampleCode
{
    public class ActionButton : IActionButton<IAction>
    {
        public string ButtonKeyBindingLabel { get; set; } = "1";
        public bool Disabled { get; set; } = false;
        public string CooldownLabel { get; set; }
        public int Index { get; set; }
        public IAction ActionToExecute { get; set; }
        public bool Execute()
        {
            if (ActionToExecute != null && ActionToExecute.Execute())
            {
                return true;
            }
            return false;
        }
    }
}