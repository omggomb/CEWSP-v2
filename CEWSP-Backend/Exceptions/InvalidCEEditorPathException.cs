using System;
using System.Runtime.Serialization;

namespace CEWSP_Backend.Exceptions
{
    [Serializable]
    internal class InvalidCEEditorPathException : Exception
    {
        public string GivenEditoPath { get; set; }

        public InvalidCEEditorPathException()
        {
            GivenEditoPath = Definitions.ConstantDefinitions.CommonValueNone;
        }

        public InvalidCEEditorPathException(string message) : base(message)
        {
        }

        public InvalidCEEditorPathException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected InvalidCEEditorPathException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }

        public override string ToString()
        {
            return base.ToString() + "Path given was: " + GivenEditoPath;
        }
    }
}