using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Reflection;
using MeGUI.core.gui;

namespace MeGUI.core.plugins.interfaces
{
    public interface GenericSettings 
    {
        /************************************************************************************
         *                   Classes implementing GenericSettings must                      *
         *                    ensure that object.Equals(object other)                       *
         *                     is overridden and is correct for the                         *
         *                                 given class.                                     *
         ************************************************************************************/

        /// <summary>
        /// Deep-clones the settings
        /// </summary>
        /// <returns></returns>
        GenericSettings baseClone();

        /// <summary>
        /// Returns the meta type of a profile. This is used as a lookup in the ProfileManager class
        /// to group like profile types. There should be one meta-type per settings type.
        /// </summary>
        /// <returns></returns>
        string SettingsID { get; }
        

        /// <summary>
        /// Substitutes any filenames stored in this profile (eg quantizer matrices) according to
        /// the substitution table
        /// </summary>
        /// <param name="substitutionTable"></param>
        void FixFileNames(Dictionary<string, string> substitutionTable);

        /// <summary>
        /// Lists all the files that these codec settings depend upon
        /// </summary>
        string[] RequiredFiles { get; }

        /// <summary>
        /// Lists all the profiles that these codec settings depend upon
        /// </summary>
        string[] RequiredProfiles { get; }

    }


    [AttributeUsage(AttributeTargets.Property)]
    public class PropertyEqualityIgnoreAttribute : Attribute
    {
        public PropertyEqualityIgnoreAttribute() {}
    }


    public class PropertyEqualityTester
    {
        /// <summary>
        /// Returns whether all of the properties (excluding those with the PropertyEqualityIgnoreAttribute)
        /// of the two objects are equal
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static bool AreEqual(object a, object b)
        {
            if (a.GetType() != b.GetType()) return false;
            Type t = a.GetType();
            foreach (PropertyInfo info in t.GetProperties())
            {
                object[] attributes = info.GetCustomAttributes(true);
                if (info.GetCustomAttributes(typeof(PropertyEqualityIgnoreAttribute), true).Length > 0)
                    continue;
                object aVal = null, bVal = null;
                try { aVal = info.GetValue(a, null); }
                catch { }
                try { bVal = info.GetValue(b, null); }
                catch { }
                if (!ArrayEqual(aVal, bVal)) 
                    return false;
            }
            return true;
        }

        /// <summary>
        /// Returns whether these two objects are equal. Returns object.Equals except for arrays,
        /// where it recursively does an elementwise comparison
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        private static bool ArrayEqual(object a, object b)
        {
            if (a == b) return true;
            if (a == null || b == null) return false;

            if (a.GetType() != b.GetType()) return false;
            if (!a.GetType().IsArray)
                return a.Equals(b);

            object[] arrayA = (object[])a;
            object[] arrayB = (object[])b;

            if (arrayA.Length != arrayB.Length) return false;
            for (int i = 0; i < arrayA.Length; i++)
            {
                if (!ArrayEqual(arrayA[i], arrayB[i])) return false;
            }
            return true;
        }
    }

}
