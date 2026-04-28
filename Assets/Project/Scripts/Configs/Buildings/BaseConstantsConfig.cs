using UnityEngine;

namespace Configs
{
    public abstract class BaseConstantsConfig
    {
        public readonly string ID;
        public readonly float value;
        public BaseConstantsConfig(string id, float value)
        {
            ID = id;
            this.value = value;
        }
    }
}
