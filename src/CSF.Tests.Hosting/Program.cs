﻿using CSF;
using CSF.Hosting;
using CSF.Tests.Hosting;
using Microsoft.Extensions.Hosting;

await Host.CreateDefaultBuilder(args)
    .ConfigureCommands<PipelineService, CommandService>()
    .Build()
    .RunAsync();