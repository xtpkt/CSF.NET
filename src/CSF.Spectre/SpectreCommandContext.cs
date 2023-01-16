﻿using Spectre.Console;
using System;

namespace CSF.Spectre
{
    /// <inheritdoc/>
    public class SpectreCommandContext : CommandContext
    {
        /// <summary>
        ///     The underlying console for this handler.
        /// </summary>
        [CLSCompliant(false)]
        public IAnsiConsole Console { get; }

        /// <inheritdoc/>
        public SpectreCommandContext(ParseResult result) : base(result)
        {
            Console = AnsiConsole.Console;
        }
    }
}
