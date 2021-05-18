namespace SimpleActionbar
{
    public class SetActionButtonKeybindLabelEventArgs
    {
        public string NewKeybind { get; set; }
        public int ActionIndex
        {
            get;
            set;
        }
    }
}