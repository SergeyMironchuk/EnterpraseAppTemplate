﻿using System.Reflection;
using FluentMigrator;
using FluentMigrator.Runner;
using FluentMigrator.Runner.Announcers;
using FluentMigrator.Runner.Initialization;

namespace BIZ.PControl.DbSchema
{
    public static class Runner
    {
        private class MigrationOptions : IMigrationProcessorOptions
        {
            public bool PreviewOnly { get; set; }
            public string ProviderSwitches { get; set; }
            public int Timeout { get; set; }
        }

        public static void MigrateToLatest(string connectionString)
        {
            // var announcer = new NullAnnouncer();
            var announcer = new TextWriterAnnouncer(s => System.Diagnostics.Debug.WriteLine(s));
            var assembly = Assembly.GetExecutingAssembly();

            var migrationContext = new RunnerContext(announcer)
            {
                Namespace = "BIZ.PControl.DbSchema",
                NestedNamespaces = true
            };

            var options = new MigrationOptions { PreviewOnly = false, Timeout = 60 };
            var factory =
                new FluentMigrator.Runner.Processors.SqlServer.SqlServer2008ProcessorFactory();;

            using (var processor = factory.Create(connectionString, announcer, options))
            {
                var runner = new MigrationRunner(assembly, migrationContext, processor);
                runner.MigrateUp(true);
            }
        }
    }
}