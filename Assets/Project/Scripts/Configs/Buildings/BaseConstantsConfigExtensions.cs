using System.Linq;

namespace Configs
{
    public static class BaseConstantsConfigExtensions
    {
        public static float GetConstant(this BaseConstantsConfig[] configs, string id, float defaultValue = 0)
        {
            var config = configs?.FirstOrDefault(c => c.ID == id);
            return config?.value ?? defaultValue;
        }
    }
}