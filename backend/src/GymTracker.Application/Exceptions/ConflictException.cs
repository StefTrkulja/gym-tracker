using System;
using System.Collections.Generic;
using System.Text;

namespace GymTracker.Application.Exceptions
{
    public class ConflictException : Exception
    {
        public ConflictException(string message) : base(message) { }
    }
}
