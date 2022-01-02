﻿using System;

namespace TRLang.src.Error
{
    class Error : Exception
    {
        public override string Message { get; }

        public Error(string message)
        {
            this.Message = $"{this.GetType().Name}: {message}";
        }
    }
}
