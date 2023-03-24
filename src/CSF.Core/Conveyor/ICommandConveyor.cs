﻿using System;
using System.Threading;
using System.Threading.Tasks;

namespace CSF
{
    /// <summary>
    ///     Represents the default interface for a command conveyor. This type cannot be used to invoke commands.
    /// </summary>
    public interface ICommandConveyor
    {
        /// <summary>
        ///     The logger used to log handles in the command pipeline.
        /// </summary>
        public ILogger Logger { get; }

        /// <summary>
        ///     Gets the logger to be used when reporting command execution flow.
        /// </summary>
        /// <param name="configuration">The configuration to fetch configuration values from.</param>
        /// <param name="services">The service provider to fetch a Microsoft.Extensions.Logging.ILogger or other alternatives to report logging to.</param>
        /// <returns>An <see cref="ILogger"/> created with provided configuration values.</returns>
        ILogger GetLogger(CommandConfiguration configuration, IServiceProvider services);

        /// <summary>
        ///     Waits for an input and returns it to the caller.
        /// </summary>
        /// <param name="cancellationToken">The cancellation token that can be used to cancel this handle.</param>
        /// <returns>The provided input.</returns>
        ValueTask<string> GetInputAsync(CancellationToken cancellationToken);

        /// <summary>
        ///     Builds a new <see cref="IContext"/> from the provided raw string.
        /// </summary>
        /// <typeparam name="T">The context type to return.</typeparam>
        /// <param name="rawInput">The raw input.</param>
        /// <param name="cancellationToken">The cancellation token that can be used to cancel this handle.</param>
        /// <returns>The built command context.</returns>
        ValueTask<IContext> BuildContextAsync(IParser parser, string rawInput, CancellationToken cancellationToken);

        /// <summary>
        ///     Called when a command succesfully (or unsuccesfully) executed.
        /// </summary>
        /// <param name="context">The <see cref="IContext"/> used to run the command.</param>
        /// <param name="result">The result returned by the caller.</param>
        /// <param name="cancellationToken">The cancellation token that can be used to cancel this handle.</param>
        /// <returns>An asynchronous <see cref="Task"/> with no return type.</returns>
        ValueTask OnResultAsync(IContext context, IResult result, CancellationToken cancellationToken);

        /// <summary>
        ///     Called when a command group is succesfully registered to the command framework.
        /// </summary>
        /// <param name="component">The component (command group) that has been registered.</param>
        /// <param name="cancellationToken">The cancellation token that can be used to cancel this handle.</param>
        /// <returns>An asynchronous <see cref="Task"/> with no return type.</returns>
        ValueTask OnRegisteredAsync(IConditionalComponent component, CancellationToken cancellationToken);

        /// <summary>
        ///     Called when a typereader is succesfully registered to the command framework.
        /// </summary>
        /// <param name="typeReader">The type reader to register.</param>
        /// <param name="cancellationToken">The cancellation token that can be used to cancel this handle.</param>
        /// <returns>An asynchronous <see cref="Task"/> with no return type.</returns>
        ValueTask OnRegisteredAsync(ITypeReader typeReader, CancellationToken cancellationToken);

        /// <summary>
        ///     Returns the error message when the context input was not found.
        /// </summary>
        /// <typeparam name="TContext">The <see cref="IContext"/> used to run the command.</typeparam>
        /// <param name="context">The <see cref="IContext"/> used to run the command.</param>
        /// <returns>A <see cref="SearchResult"/> holding the returned error.</returns>
        SearchResult OnCommandNotFound<TContext>(TContext context)
            where TContext : IContext;

        /// <summary>
        ///     Returns the error message when no best match was found for the command.
        /// </summary>
        /// <typeparam name="TContext">The <see cref="IContext"/> used to run the command.</typeparam>
        /// <param name="context">The <see cref="IContext"/> used to run the command.</param>
        /// <returns>A <see cref="SearchResult"/> holding the returned error.</returns>
        CheckResult OnBestOverloadUnavailable<TContext>(TContext context)
            where TContext : IContext;

        /// <summary>
        ///     Returns the error message when a service to inject into the module has not been found in the provided <see cref="IServiceProvider"/>.
        /// </summary>
        /// <typeparam name="TContext">The <see cref="IContext"/> used to run the command.</typeparam>
        /// <param name="context">The <see cref="IContext"/> used to run the command.</param>
        /// <param name="dependency">Information about the service to inject.</param>
        /// <returns>A <see cref="ConstructionResult"/> holding the returned error.</returns>
        ConstructionResult OnServiceNotFound<TContext>(TContext context, DependencyParameter dependency)
            where TContext : IContext;

        /// <summary>
        ///     Returns the error message when the module in question cannot be cast to an <see cref="ModuleBase"/>.
        /// </summary>
        /// <typeparam name="TContext">The <see cref="IContext"/> used to run the command.</typeparam>
        /// <param name="context">The <see cref="IContext"/> used to run the command.</param>
        /// <param name="module">The module that failed to cast to an <see cref="ModuleBase"/>.</param>
        /// <returns>A <see cref="ConstructionResult"/> holding the returned error.</returns>
        ConstructionResult OnInvalidModule<TContext>(TContext context, Module module)
            where TContext : IContext;

        /// <summary>
        ///     Called when an optional parameter has a lacking value.
        /// </summary>
        /// <remarks>
        ///     The result will fail to resolve and exit execution if the type does not match the provided <see cref="BaseParameter.Type"/> or <see cref="Type.Missing"/>.
        /// </remarks>
        /// <typeparam name="TContext">The <see cref="IContext"/> used to run the command.</typeparam>
        /// <param name="context">The <see cref="IContext"/> used to run the command.</param>
        /// <param name="param">The parameter that failed to handle.</param>
        /// <returns>A <see cref="TypeReaderResult"/> for the target parameter.</returns>
        TypeReaderResult OnMissingValue<TContext>(TContext context, IParameterComponent param)
            where TContext : IContext;

        /// <summary>
        ///     Called when the parameter information type is not supported.
        /// </summary>
        /// <typeparam name="TContext">The <see cref="IContext"/> used to run the command.</typeparam>
        /// <param name="context">The <see cref="IContext"/> used to run the command.</param>
        /// <param name="param">The parameter that failed to handle.</param>
        /// <returns>A <see cref="TypeReaderResult"/> for the target parameter.</returns>
        TypeReaderResult ParameterTypeUnsupported<TContext>(TContext context, IParameterComponent param)
            where TContext : IContext;

        /// <summary>
        ///     Returns the error when <see cref="OnMissingValue{T}(T, BaseParameter)"/> returned a type that did not match the expected type.
        /// </summary>
        /// <typeparam name="TContext">The <see cref="IContext"/> used to run the command.</typeparam>
        /// <param name="context">The <see cref="IContext"/> used to run the command.</param>
        /// <param name="expectedType">The type that was expected to return.</param>
        /// <param name="returnedType">The returned type.</param>
        /// <returns>A <see cref="ArgsResult"/> holding the returned error.</returns>
        ArgsResult OnMissingReturnedInvalid<TContext>(TContext context, Type expectedType, Type returnedType)
            where TContext : IContext;

        /// <summary>
        ///     Returns the error when <see cref="OnMissingValue{T}(T, BaseParameter)"/> failed to return a valid result. 
        ///     This method has to return <see cref="Type.Missing"/> if no self-implemented value has been returned.
        /// </summary>
        /// <typeparam name="TContext">The <see cref="IContext"/> used to run the command.</typeparam>
        /// <param name="context">The <see cref="IContext"/> used to run the command.</param>
        /// <returns>A <see cref="ArgsResult"/> holding the returned error.</returns>
        ArgsResult OnOptionalNotPopulated<TContext>(TContext context)
            where TContext : IContext;

        /// <summary>
        ///     Returns the error message when an unhandled return type has been returned from the command method.
        /// </summary>
        /// <typeparam name="TContext">The <see cref="IContext"/> used to run the command.</typeparam>
        /// <param name="context">The <see cref="IContext"/> used to run the command.</param>
        /// <param name="returnValue">The returned value of the method.</param>
        /// <returns>An <see cref="ExecuteResult"/> holding the returned error.</returns>
        ExecuteResult OnUnhandledReturnType<TContext>(TContext context, object returnValue)
            where TContext : IContext;

        /// <summary>
        ///     Returns the error message when command execution fails on the user's end.
        /// </summary>
        /// <typeparam name="TContext">The <see cref="IContext"/> used to run the command.</typeparam>
        /// <param name="context">The <see cref="IContext"/> used to run the command.</param>
        /// <param name="command">Information about the command that's being executed.</param>
        /// <param name="ex">The exception that occurred while executing the command.</param>
        /// <returns>An <see cref="ExecuteResult"/> holding the returned error.</returns>
        ExecuteResult OnUnhandledException<TContext>(TContext context, Command command, Exception ex)
            where TContext : IContext;
    }
}