using System;
using System.Collections.Generic;
using System.Text;

using MeGUI.core.util;

namespace MeGUI
{
    [LogByMembers]
    public class DialogSettings
    {
        private bool ovewriteJobOutputResponse = true;

        public bool OverwriteJobOutputResponse
        {
            get { return ovewriteJobOutputResponse; }
            set { ovewriteJobOutputResponse = value; }
        }

        private bool askAboutOverwriteJobOutput = true;

        public bool AskAboutOverwriteJobOutput
        {
            get { return askAboutOverwriteJobOutput; }
            set { askAboutOverwriteJobOutput = value; }
        }


        private bool askAboutDuplicates;
        private bool dupResponse;

        public bool DuplicateResponse
        {
            get { return dupResponse; }
            set { dupResponse = value; }
        }


        public bool AskAboutDuplicates
        {
            get { return askAboutDuplicates; }
            set { askAboutDuplicates = value; }
        }

        private bool warnAboutRDO2;
        private bool askAboutVOBs;
        private bool addConvertToYV12;
        private bool askAboutYV12;
        private bool useOneClick;
        private bool continueDespiteError;
        private bool askAboutError;

        public bool WarnAboutRDO2
        {
            get { return warnAboutRDO2; }
            set { warnAboutRDO2 = value; }
        }

        public bool AskAboutError
        {
            get { return askAboutError; }
            set { askAboutError = value; }
        }

        public bool ContinueDespiteError
        {
            get { return continueDespiteError; }
            set { continueDespiteError = value; }
        }


        public bool AskAboutYV12
        {
            get { return askAboutYV12; }
            set { askAboutYV12 = value; }
        }

        public bool AddConvertToYV12
        {
            get { return addConvertToYV12; }
            set { addConvertToYV12 = value; }
        }


        public bool AskAboutVOBs
        {
            get { return askAboutVOBs; }
            set { askAboutVOBs = value; }
        }
        
        public bool UseOneClick
        {
            get { return useOneClick; }
            set { useOneClick = value; }
        }
        public DialogSettings()
        {
            askAboutVOBs = true;
            useOneClick = true;
            askAboutError = true;
            askAboutYV12 = true;
            addConvertToYV12 = true;
            continueDespiteError = true;
            warnAboutRDO2 = true;
            askAboutDuplicates = true;
            dupResponse = true;
        }
    }
}
