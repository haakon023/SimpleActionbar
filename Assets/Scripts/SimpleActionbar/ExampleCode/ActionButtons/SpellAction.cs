using SimpleActionbar.ExampleCode;
using UnityEngine;
using UnityEngine.InputSystem.Interactions;

namespace SimpleActionbar.ExampleCode
{
    public class SpellAction : AbstractAction
    {
        public string Spell { get; set; }

        public override bool Execute()
        {
            Debug.Log($"Equipped {Spell}");

            TimeSinceLastExecute = Time.time;
            return TimeSinceLastExecute >= RemainingCooldown;
        }
    }
}