using System;

namespace KellerProtocol.Exceptions
{
    // Exceptions
    public class CrcException : Exception
    {
        public CrcException(string message) : base(message)
        {
        }

        public CrcException()
        {
        }
    }

    public class AnswerException : Exception
    {
        public AnswerException(string message) : base(message)
        {
        }

        public AnswerException()
        {
        }
    }

    public class NotImplementedFunctionException : Exception
    {
        public NotImplementedFunctionException(string message) : base(message)
        {
        }

        public NotImplementedFunctionException()
        {
        }
    }

    public class DeviceNotInitializedException : Exception
    {
        public DeviceNotInitializedException(string message) : base(message)
        {
        }

        public DeviceNotInitializedException()
        {
        }
    }

    public class InvalidDeviceOperationException : Exception
    {
        public InvalidDeviceOperationException(string message) : base(message)
        {
        }

        public InvalidDeviceOperationException()
        {
        }
    }

    public class MessageLengthException : Exception
    {
        public MessageLengthException(string message) : base(message)
        {
        }

        public MessageLengthException()
        {
        }
    }

}