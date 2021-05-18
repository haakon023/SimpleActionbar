using UnityEngine.UIElements;

namespace SimpleActionbar
{
    public interface IActionButton<T>
    {
        string ButtonKeyBindingLabel { get; set; }
        bool Disabled { get; set; }
        string CooldownLabel { get; set; }
        int Index { get; set; }
        bool Execute();
        T ActionToExecute { get; set; }
    }    
}