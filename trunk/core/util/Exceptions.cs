using System;
using System.Collections.Generic;
using System.Text;

namespace MeGUI.core.util
{
    public class JobRunException : Exception
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
