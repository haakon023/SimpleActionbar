using UnityEngine;
using UnityEngine.UIElements;

namespace SimpleActionbar.ExampleCode
{
    public abstract class AbstractAction : IAction
    {
        public string ActionName { get; set; }
        public string ActionDescription { get; set; }
        public float ActionCooldown { get; set; }
        public bool OnCooldown => RemainingCooldown > 0 ? true : false;
        public float RemainingCooldown => Mathf.Max(0,ActionCooldown - (Time.time - TimeSinceLastExecute));
        public string ActionImagePath { get; set; }
        public float TimeSinceLastExecute { get; set; } = Mathf.NegativeInfinity;

        public virtual bool Execute()
        {
            TimeSinceLastExecute = Time.time;
            return TimeSinceLastExecute >= RemainingCooldown;
        }
    }
}