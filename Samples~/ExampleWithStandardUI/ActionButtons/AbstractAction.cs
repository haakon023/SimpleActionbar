using UnityEngine;
using UnityEngine.UIElements;
using Image = UnityEngine.UI.Image;

namespace SimpleActionbar.ExampleCode
{
    public abstract class AbstractAction : IAction
    {
        public string ActionName { get; set; }
        public string ActionDescription { get; set; }
        public float ActionCooldown { get; set; }
        public bool OnCooldown => RemainingCooldown > 0 ? true : false;
        public float RemainingCooldown => Mathf.Max(0,ActionCooldown - (Time.time - TimeSinceLastExecute));
        public Sprite ActionImage { get; set; }
        public float TimeSinceLastExecute { get; set; } = Mathf.NegativeInfinity;
        public bool IgnoreGlobalCooldown { get; set; }

        public virtual bool Execute()
        {
            TimeSinceLastExecute = Time.time;
            return true;
        }
    }
}