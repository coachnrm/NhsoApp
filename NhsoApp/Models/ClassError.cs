using System;

namespace NhsoApp.Models;

public class ClassError
{
    public class Argument
    {
        public IList<string> codes { get; set; }
        public string defaultMessage { get; set; }
        public string code { get; set; }
    }

    public class Error
    {
        public IList<string> codes { get; set; }
        public IList<Argument> arguments { get; set; }
        public string defaultMessage { get; set; }
        public string objectName { get; set; }
        public string code { get; set; }
    }

    public class Errorcode
    {
        public DateTime timestamp { get; set; }
        public int status { get; set; }
        public string error { get; set; }
        public string message { get; set; }
        public IList<Error> errors { get; set; }
    }
}
