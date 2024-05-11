using System.Collections.Generic;
using System.Linq;

namespace Expressium.Configurations
{
    public class ConfigurationEnroller
    {
        public string BrowserType { get; set; }
        public string BrowserLanguage { get; set; }
        public bool BrowserMaximize { get; set; }
        public bool BrowserWindow { get; set; }
        public int BrowserWindowWidth { get; set; }
        public int BrowserWindowHeight { get; set; }

        public string NameLocator { get; set; }
        public string IncludeControlsLocator { get; set; }
        public string ExcludeControlsLocator { get; set; }
        public bool PrependControlsLocator { get; set; }
        public bool MergeExistingControls { get; set; }
        public bool AppendNameSynchronizer { get; set; }

        public List<ConfigurationEnrollerAction> Actions { get; set; }

        public ConfigurationEnroller()
        {
            BrowserType = BrowserTypes.Chrome.ToString();
            BrowserMaximize = true;
            BrowserWindow = false;
            BrowserWindowWidth = 1920;
            BrowserWindowHeight = 1080;

            NameLocator = null;
            IncludeControlsLocator = null;
            ExcludeControlsLocator = null;
            PrependControlsLocator = false;
            MergeExistingControls = false;
            AppendNameSynchronizer = false;

            Actions = new List<ConfigurationEnrollerAction>();
        }

        public void Validate()
        {
            foreach (var action in Actions)
                action.Validate();
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
