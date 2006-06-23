using System;
using System.Collections.Generic;
using System.Text;

namespace MeGUI
{
    public class CommandlineParser
    {
        public Dictionary<string, string> upgradeData = new Dictionary<string, string>();
        public List<string> failedUpgrades = new List<string>();
        public bool start = true;

        public bool Parse(string[] commandline)
        {
            for (int i = 0; i < commandline.Length; i++)
            {
                if (commandline[i] == "--upgraded")
                {
                    if (commandline.Length > i + 2)
                    {
                        upgradeData.Add(commandline[i + 1], commandline[i + 2]);
                        i += 2;
                    }
                    else
                        return false;
                }
                else if (commandline[i] == "--upgrade-failed")
                {
                    if (commandline.Length > i + 1)
                    {
                        failedUpgrades.Add(commandline[i + 1]);
                        i++;
                    }
                    else
                        return false;
                }
                else if (commandline[i] == "--dont-start")
                {
                    start = false;
                }
                else
                    return false;
            }
            return true;
        }
    }
}
