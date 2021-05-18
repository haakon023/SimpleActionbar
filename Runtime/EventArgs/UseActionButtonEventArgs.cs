using System;

namespace SimpleActionbar
{
    public class UseActionButtonEventArgs : EventArgs
    {
        public int Index { get; set; }
        public float Duration { get; set; }
    }    
}