﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TShockAPI;

namespace CSF.TShock
{
    /// <summary>
    ///     Represents a <see cref="CommandStandardizationFramework"/> intended to be functional for TShock plugins.
    /// </summary>
    public class TSCommandFramework : CommandFramework
    {
        /// <summary>
        ///     Represents the configuration for the framework in its current state.
        /// </summary>
        public new TSCommandConfiguration Configuration { get; }

        /// <summary>
        ///     Creates a new <see cref="TSCommandFramework"/> for processing modules inside the framework.
        /// </summary>
        /// <param name="config"></param>
        public TSCommandFramework(TSCommandConfiguration config)
            : base(config)
        {
            config.InvokeOnlyNameRegistrations = true;

            Configuration = config;
            base.CommandRegistered += CommandRegistered;

            config.TypeReaders
                .Include(new TSPlayerReader())
                .Include(new PlayerReader());

            config.Prefixes
                .Include(new StringPrefix("/"))
                .Include(new StringPrefix("."));
        }

        private new Task CommandRegistered(Command arg)
        {
            var permissions = new List<string>();
            bool shouldReplace = false;
            string description = "";
            foreach (var attribute in arg.Attributes)
            {
                if (attribute is RequirePermissionAttribute permAttribute)
                    permissions.Add(permAttribute.PermissionNode);

                if (attribute is ReplaceExistingAttribute replaceAttribute)
                    shouldReplace = replaceAttribute.ShouldReplace;

                if (attribute is DescriptionAttribute descriptionAttribute)
                    description = descriptionAttribute.Description;
            }

            if (shouldReplace || Configuration.ReplaceAllExisting)
                Commands.ChatCommands.RemoveAll(x => x.Names.Any(o => arg.Aliases.Any(n => o == n)));

            Commands.ChatCommands.Add(new TShockAPI.Command(string.Join(".", permissions), async (x) => await ExecuteCommandAsync(x), arg.Aliases)
            {
                HelpText = description
            });

            return Task.CompletedTask;
        }

        public virtual Task<TSCommandContext> CreateContextAsync(CommandArgs args, string rawInput)
        {
            if (!TryParsePrefix(ref rawInput, out var prefix))
                prefix = EmptyPrefix.Create();

            return Task.FromResult(new TSCommandContext(args, rawInput, prefix));
        }

        public virtual async Task ExecuteCommandAsync(CommandArgs args)
        {
            var context = await CreateContextAsync(args, args.Message);

            var result = await ExecuteCommandAsync(context);

            if (!result.IsSuccess)
                args.Player.SendErrorMessage(result.ErrorMessage);
        }

        protected override SearchResult CommandNotFoundResult<T>(T context)
        {
            return SearchResult.FromError($"A command with name: '{context.Name}' was not found.");
        }

        protected override SearchResult NoApplicableOverloadResult<T>(T context)
        {
            return SearchResult.FromError($"No best override was found for command with name: '{context.Name}'.");
        }

        protected override ExecuteResult UnhandledExceptionResult<T>(T context, Command command, Exception ex)
        {
            TShockAPI.TShock.Log.ConsoleError(ex.ToString());
            return ExecuteResult.FromError($"An unhandled exception has occurred. Please check logs for more details");
        }
    }
}
