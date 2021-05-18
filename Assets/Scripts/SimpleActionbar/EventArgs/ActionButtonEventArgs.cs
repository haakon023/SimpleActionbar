using System;

namespace SimpleActionbar
{
    public class ActionButtonEventArgs : EventArgs
    {
        public int Index { get; set; }
        public float Duration { get; set; }
    }    
}