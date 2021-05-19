using System.Collections.Generic;

namespace SimpleActionbar.ExampleCode
{
    public class ActionButtonMapper : IActionButtonMapper<string>
    {
        public ActionButtonMapper()
        {
            Init();
        }

        public Dictionary<string, int> ActionbarMappings { get; set; }

        public void Init(string path = null)
        {
            ActionbarMappings = new Dictionary<string, int>()
            {
                {"ActionButton1", 0},
                {"ActionButton2", 1},
                {"ActionButton3", 2},
                {"ActionButton4", 3},
                {"ActionButton5", 4},
                {"ActionButton6", 5},
                {"ActionButton7", 6},
                {"ActionButton8", 7},
                {"ActionButton9", 8},
                {"ActionButton10", 9},
            };
        }

        public bool Output(string input, out int index)
        {
            return ActionbarMappings.TryGetValue(input, out index);
        }
    }
}