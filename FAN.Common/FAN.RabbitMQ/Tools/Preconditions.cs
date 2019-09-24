﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace FAN.RabbitMQ
{
    /// <summary>
    /// Collection of precondition methods for qualifying method arguments.
    /// </summary>
    internal static class Preconditions
    {
        /// <summary>
        /// Ensures that <paramref name="value"/> is not null.
        /// </summary>
        /// <param name="value">
        /// The value to check, must not be null.
        /// </param>
        /// <param name="name">
        /// The name of the parameter the value is taken from, must not be
        /// blank.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// Thrown if <paramref name="value"/> is null.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// Thrown if <paramref name="name"/> is blank.
        /// </exception>
        public static void CheckNotNull<T>(T value, string name) where T : class
        {
            CheckNotNull(value, name, string.Format("{0} must not be null", name));
        }

        /// <summary>
        /// Ensures that <paramref name="value"/> is not null.
        /// </summary>
        /// <param name="value">
        /// The value to check, must not be null.
        /// </param>
        /// <param name="name">
        /// The name of the parameter the value is taken from, must not be
        /// blank.
        /// </param>
        /// <param name="message">
        /// The message to provide to the exception if <paramref name="value"/>
        /// is null, must not be blank.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// Thrown if <paramref name="value"/> is null.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// Thrown if <paramref name="name"/> or <paramref name="message"/> are
        /// blank.
        /// </exception>
        public static void CheckNotNull<T>(T value, string name, string message) where T : class
        {
            if (value == null)
            {
                throw new ArgumentNullException(name, message);
            }
        }

        /// <summary>
        /// Ensures that <paramref name="collection"/> contains at least one
        /// item.
        /// </summary>
        /// <param name="collection">
        /// The collection to check, must not be null or empty.
        /// </param>
        /// <param name="name">
        /// The name of the parameter the collection is taken from, must not be
        /// blank.
        /// </param>
        /// <param name="message">
        /// The message to provide to the exception if <paramref name="collection"/>
        /// is empty, must not be blank.
        /// </param>
        /// <exception cref="ArgumentException">
        /// Thrown if <paramref name="collection"/> is empty, or if
        /// <paramref name="value"/> or <paramref name="name"/> are blank.
        /// </exception>
        public static void CheckAny<T>(IEnumerable<T> collection, string name, string message)
        {
            if (collection == null || !collection.Any())
            {
                throw new ArgumentException(message, name);
            }
        }

        /// <summary>
        /// Ensures that <paramref name="value"/> is true.
        /// </summary>
        /// <param name="value">
        /// The value to check, must be true.
        /// </param>
        /// <param name="name">
        /// The name of the parameter the value is taken from, must not be
        /// blank.
        /// </param>
        /// <param name="message">
        /// The message to provide to the exception if <paramref name="collection"/>
        /// is false, must not be blank.
        /// </param>
        /// <exception cref="ArgumentException">
        /// Thrown if <paramref name="value"/> is false, or if <paramref name="name"/>
        /// or <paramref name="message"/> are blank.
        /// </exception>
        public static void CheckTrue(bool value, string name, string message)
        {
            if (!value)
            {
                throw new ArgumentException(message, name);
            }
        }

        /// <summary>
        /// Ensures that <paramref name="value"/> is false.
        /// </summary>
        /// <param name="value">
        /// The value to check, must be false.
        /// </param>
        /// <param name="name">
        /// The name of the parameter the value is taken from, must not be
        /// blank.
        /// </param>
        /// <param name="message">
        /// The message to provide to the exception if <paramref name="collection"/>
        /// is true, must not be blank.
        /// </param>
        /// <exception cref="ArgumentException">
        /// Thrown if <paramref name="value"/> is true, or if <paramref name="name"/>
        /// or <paramref name="message"/> are blank.
        /// </exception>
        public static void CheckFalse(bool value, string name, string message)
        {
            if (value)
            {
                throw new ArgumentException(message, name);
            }
        }

        public static void CheckShortString(string value, string name)
        {
            CheckNotNull(value, name);
            if (value.Length > 255)
            {
                throw new ArgumentException(string.Format("Argument '{0}' must be less than or equal to 255 characters.", name));
            }
        }

        public static void CheckTypeMatches(Type expectedType, object value, string name, string message)
        {
            if (!expectedType.IsAssignableFrom(value.GetType()))
            {
                throw new ArgumentException(message, name);
            }
        }
    }
}
