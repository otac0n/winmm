﻿//-----------------------------------------------------------------------
// <copyright file="MMSystemException.cs" company="(none)">
//     Copyright (c) 2009 John Gietzen. All rights reserved.
// </copyright>
// <author>John Gietzen</author>
//-----------------------------------------------------------------------

namespace WinMM
{
    using System;
    
    /// <summary>
    /// MMSystemException is thrown as a generic exception for errors in WinMM.
    /// </summary>
    [global::System.Serializable]
    public class MMSystemException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the MMSystemException class.
        /// </summary>
        public MMSystemException()
        {
        }

        /// <summary>
        /// Initializes a new instance of the MMSystemException class with a specified error message.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        public MMSystemException(string message)
            : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the MMSystemException class with a specified error
        /// message and a reference to the inner exception that is the cause of this exception.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        /// <param name="inner">The exception that is the cause of the current exception, or a null reference (Nothing in Visual Basic) if no inner exception is specified.</param>
        public MMSystemException(string message, Exception inner)
            : base(message, inner)
        {
        }

        /// <summary>
        /// Initializes a new instance of the MMSystemException class with serialized data.
        /// </summary>
        /// <param name="info">The System.Runtime.Serialization.SerializationInfo that holds the serialized object data about the exception being thrown.</param>
        /// <param name="context">The System.Runtime.Serialization.StreamingContext that contains contextual information about the source or destination.</param>
        /// <exception cref="System.ArgumentNullException">The info parameter is null.</exception>
        /// <exception cref="System.Runtime.Serialization.SerializationException">The class name is null or System.Exception.HResult is zero (0).</exception>
        protected MMSystemException(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context)
            : base(info, context)
        {
        }
    }
}
