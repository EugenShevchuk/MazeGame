using UnityEngine;

namespace Project.Infrastructure
{
    public abstract class SelectableConfiguration : ScriptableObject
    {
        public string Name = "[NOT SET]";
        public string Description = "[NOT SET]";
    }
}