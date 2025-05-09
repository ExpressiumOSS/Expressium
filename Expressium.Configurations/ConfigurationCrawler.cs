using System;

namespace Expressium.Configurations
{
    public class ConfigurationCrawler
    {
        public string PageName { get; set; }
        public string IncludeControlsLocator { get; set; }
        public string ExcludeControlsLocator { get; set; }
        public bool PrependControlsLocator { get; set; }
        public bool MergeExistingControls { get; set; }
        public bool AppendNameSynchronizer { get; set; }

        public ConfigurationCrawler()
        {
            PageName = null;
            IncludeControlsLocator = null;
            ExcludeControlsLocator = null;
            PrependControlsLocator = false;
            MergeExistingControls = true;
            AppendNameSynchronizer = false;
        }

        public void Validate()
        {
        }
    }
}
