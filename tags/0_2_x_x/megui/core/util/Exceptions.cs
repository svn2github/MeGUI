using System;
using System.Collections.Generic;
using System.Text;

namespace MeGUI.core.util
{
    // The base class for MeGUI-triggered exceptions
    public class MeGUIException : Exception
    { 
        public MeGUIException(string message) : base(message) { }

        public MeGUIException(Exception inner) : base(inner.Message, inner) { }

        public MeGUIException(string message, Exception inner) : base(message, inner) { }

    }

    public class JobRunException : MeGUIException
    {
        public JobRunException(string message) : base(message) { }

        public JobRunException(Exception inner) : base(inner.Message, inner) { }

        public JobRunException(string message, Exception inner) : base(message, inner) { }
    }

    public class MissingFileException : JobRunException
    {
        public string filename;

        public MissingFileException(string file)
            : base("Required file '" + file + "' is missing.")
        {
            filename = file;
        }
    }

    public class EncoderMissingException : MissingFileException
    {
        public EncoderMissingException(string file) : base(file) { }
    }
}
