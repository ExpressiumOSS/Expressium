using System.Collections.Generic;
using System.Linq;

namespace Expressium.Configurations
{
    public class ConfigurationEnroller
    {
        public string Browser { get; set; }
        public string Language { get; set; }
        public bool Maximize { get; set; }
        public bool WindowSize { get; set; }
        public int WindowWidth { get; set; }
        public int WindowHeight { get; set; }
        public string CustomControls { get; set; }
        public string NameLocator { get; set; }
        public string ControlsLocator { get; set; }
        public List<ConfigurationEnrollerAction> Actions { get; set; }

        public ConfigurationEnroller()
        {
            Browser = "Chrome";
            Maximize = true;
            WindowSize = false;
            WindowWidth = 1920;
            WindowHeight = 1080;

            Actions = new List<ConfigurationEnrollerAction>();
        }

        public bool IsActionAdded(ConfigurationEnrollerAction action)
        {
            return Actions.Any(m => m.GetHashCode() == action.GetHashCode());
        }

        public ConfigurationEnrollerAction GetAction(ConfigurationEnrollerAction action)
        {
            if (IsActionAdded(action))
                return Actions.FirstOrDefault(c => c.GetHashCode() == action.GetHashCode());

            return null;
        }

        public void DeleteAction(ConfigurationEnrollerAction action)
        {
            if (IsActionAdded(action))
            {
                int index = Actions.FindIndex(m => m.GetHashCode() == action.GetHashCode());
                Actions.RemoveAt(index);
            }
        }

        public void SwapActions(int masterId, int slaveId)
        {
            var action = Actions[masterId];
            Actions[masterId] = Actions[slaveId];
            Actions[slaveId] = action;
        }
    }
}
