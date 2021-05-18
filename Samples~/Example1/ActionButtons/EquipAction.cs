using UnityEngine;

namespace SimpleActionbar.ExampleCode
{
    public class EquipAction : AbstractAction
    {
        public string EquipmentName { get; set; }

        public override bool Execute()
        {
            Debug.Log($"Equipped {EquipmentName}");
            TimeSinceLastExecute = Time.time;
            return TimeSinceLastExecute >= RemainingCooldown;
        }
    }
}