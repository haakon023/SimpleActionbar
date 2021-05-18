using UnityEngine.UIElements;

namespace SimpleActionbar.ExampleCode
{
    public interface IAction
    {
        string ActionName { get; set; }
        string ActionDescription { get; set; }
        float ActionCooldown { get; set; }
        bool OnCooldown { get; }
        float RemainingCooldown { get; }
        string ActionImagePath { get; set; }

        float TimeSinceLastExecute { get; set; }
        
        bool Execute();
    }
}