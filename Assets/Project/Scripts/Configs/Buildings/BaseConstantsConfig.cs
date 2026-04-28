using UnityEngine;

namespace Configs
{
    public abstract class BaseConstantsConfig
    {
        public readonly string ID;
        public readonly int value;
        public BaseConstantsConfig(string id, int value)
        {
            ID = id;
            this.value = value;
        }
    }
}
