using System.Collections.Generic;

namespace SimpleActionbar.ExampleCode
{
    public class ActionButtonData
    {
        public int ActionBarIndex { get; set; }
        public IAction Action { get; set; }
        public string KeyBind { get; set; }
    }
}