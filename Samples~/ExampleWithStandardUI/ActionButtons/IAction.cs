using UnityEngine;
using UnityEngine.UIElements;
using Image = UnityEngine.UI.Image;

namespace SimpleActionbar.ExampleCode
{
    public interface IAction
    {
        string ActionName { get; set; }
        string ActionDescription { get; set; }
        float ActionCooldown { get; set; }
        bool OnCooldown { get; }
        float RemainingCooldown { get; }
        Sprite ActionImage { get; set; }
        float TimeSinceLastExecute { get; set; }
        bool IgnoreGlobalCooldown { get; set; }

        bool Execute();
    }
}