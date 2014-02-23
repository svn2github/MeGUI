// ****************************************************************************
// 
// Copyright (C) 2005-2014 Doom9 & al
// 
// This program is free software; you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation; either version 2 of the License, or
// (at your option) any later version.
// 
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// 
// You should have received a copy of the GNU General Public License
// along with this program; if not, write to the Free Software
// Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA  02111-1307  USA
// 
// ****************************************************************************

using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;
using System.Xml.Serialization;

using MeGUI.core.util;
using MeGUI.core.gui;

namespace MeGUI
{
    public class ProgramSettings
    {
        private bool _enabled, _required;
        private string _path;
        private DateTime _lastused;
        private string _name;
        private string _displayname;
        private List<string> _files;

        public ProgramSettings()
        {
            _files = new List<string>();
            _enabled = _required = false;
            _path = _name = _displayname = String.Empty;
            _lastused = new DateTime();
            MainForm.Instance.ProgramSettings.Add(this);
        }

        public ProgramSettings(string name)
        {
            _files = new List<string>();
            _enabled = _required = false;
            _path = String.Empty;
            _lastused = new DateTime();
            _name = _displayname = name;
            MainForm.Instance.ProgramSettings.Add(this);
        }

        public void UpdateInformation(string name, string displayname, string path)
        {
            _name = name;
            if (!String.IsNullOrEmpty(displayname))
                _displayname = displayname;
            if (String.IsNullOrEmpty(_displayname))
                _displayname = name;
            _path = path;
            _files.Add(path);
        }

        public bool Enabled
        {
            get 
            {
                if (_required)
                    return true;
                return _enabled; 
            }
            set { _enabled = value; }
        }

        [XmlIgnore()]
        public bool Required
        {
            get { return _required; }
            set { _required = value; }
        }

        [XmlIgnore()]
        public string Path
        {
            get { return _path; }
        }

        [XmlIgnore()]
        public List<string> Files
        {
            get { return _files; }
            set { _files = value; }
        }

        [XmlIgnore()]
        public string Name
        {
            get { return _name; }
        }

        [XmlIgnore()]
        public string DisplayName
        {
            get { return _displayname; }
            set { _displayname = value; }
        }

        public DateTime LastUsed
        {
            get { return _lastused; }
            set { _lastused = value; }
        }

        public bool Update(bool enable, bool forceUpdate)
        {
            if (enable)
                _lastused = DateTime.Now;
            _enabled = enable;

            if (!enable || FilesAvailable())
                return true;

            if (!forceUpdate)
                return false;

            // package is not available. Therefore an update check is necessary
            if (MessageBox.Show("The package " + _displayname + " is not installed.\n\nDo you want to search now online for updates?", "MeGUI package missing", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
            {
                MainForm.Instance.startUpdateCheckAndWait();
                if (FilesAvailable())
                    return true;
                MessageBox.Show(String.Format("The update for {0} failed. Therefore {0} will not be available and the current job will fail. Run the updater on your own if you want to try it later.", _displayname), _displayname + " not installed", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
                MessageBox.Show(String.Format("You have selected to not update {0}. Therefore {0} will not be available and the current job will fail. Run the updater on your own if you want to download it later.", _displayname), _displayname + " not installed", MessageBoxButtons.OK, MessageBoxIcon.Error);
            return false;
        }

        public bool UpdateAllowed()
        {
            if (_required || (_enabled && _lastused.AddDays(UpdateCacher.REMOVE_PACKAGE_AFTER_DAYS) > DateTime.Now))
                return true;
            else
                return false;
        }

        private bool FilesAvailable()
        {
            foreach (String file in _files)
                if (!File.Exists(file))
                    return false;
            return true;
        }
    }
}
