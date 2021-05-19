using System.Collections.Generic;

namespace SimpleActionbar.ExampleCode
{
    public interface IActionButtonMapper<T>
    {
        Dictionary<T, int> ActionbarMappings { get; set; }

        void Init(string path);
        
        bool Output(T input, out int index);
    }
}