﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CSF
{
    /// <summary>
    ///     Represents a command group, functioning much like subcommands.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public sealed class GroupAttribute : Attribute
    {
        /// <summary>
        ///     Represents the name of a group.
        /// </summary>
        public string Name { get; }

        /// <summary>
        ///     The aliases of this group.
        /// </summary>
        public string[] Aliases { get; }
        
        /// <summary>
        ///     Creates a new <see cref="GroupAttribute"/> with defined name.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="aliases"></param>
        public GroupAttribute(string name, params string[] aliases)
        {
            if (string.IsNullOrEmpty(name))
                throw new ArgumentNullException(nameof(name), "Name cannot be null or empty.");

            foreach (string alias in aliases)
                if (string.IsNullOrEmpty(alias))
                    throw new ArgumentNullException(nameof(alias), "Alias cannot be null or empty.");

            Name = name;
            Aliases = new string[] { Name }.Concat(aliases).ToArray();
        }

        /// <summary>
        ///     Creates a new <see cref="GroupAttribute"/> with defined name.
        /// </summary>
        /// <param name="name"></param>
        public GroupAttribute(string name)
            : this(name, Array.Empty<string>())
        {
            
        }
    }
}
