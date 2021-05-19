using System;
using SimpleActionbar.ExampleCode;
using UnityEngine;
using UnityEngine.InputSystem.Interactions;

namespace SimpleActionbar.ExampleCode
{
    [Serializable]
    public class SpellAction : AbstractAction
    {
        [SerializeField] public string Spell;

        public override bool Execute()
        {
            Debug.Log($"Equipped {Spell}");

            TimeSinceLastExecute = Time.time;
            return true;
        }
    }
}