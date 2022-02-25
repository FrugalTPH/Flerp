using Flerp.Commands;
using Flerp.Data.LocalDb.Properties;
using Flerp.DomainModel;
using log4net;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Conventions;
using MongoDB.Bson.Serialization.Options;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.NetworkInformation;

namespace Flerp.Data.LocalDb
{
    public class DatabaseController : IDatabaseController
    {
        private static ILog Logger { get; set; }

        public void Start()
        {
            StartMongoDaemon();

            var pack = new ConventionPack
            {
                new IgnoreExtraElementsConvention(true),
                new IgnoreIfDefaultConvention(true),
                new EnumRepresentationConvention(BsonType.String),
                new MemberSerializationOptionsConvention(typeof (DateTime), DateTimeSerializationOptions.LocalInstance)
            };
            ConventionRegistry.Register(Resources.FlerpDbName, pack, t => t.FullName.StartsWith(Resources.FlerpDbName + "."));

            BsonClassMap.RegisterClassMap<AppSetting>();
            BsonClassMap.RegisterClassMap<BinderBase>();
            BsonClassMap.RegisterClassMap<Binder>();
            BsonClassMap.RegisterClassMap<Document>();
            BsonClassMap.RegisterClassMap<DocumentBase>();
            BsonClassMap.RegisterClassMap<Email>();
            BsonClassMap.RegisterClassMap<Party>();
            BsonClassMap.RegisterClassMap<PartyBinder>();
            BsonClassMap.RegisterClassMap<PartyDetail>();
            BsonClassMap.RegisterClassMap<PartyParty>();
            BsonClassMap.RegisterClassMap<RelationBase>();
            BsonClassMap.RegisterClassMap<Stub>();
            BsonClassMap.RegisterClassMap<Transmittal>();

            BsonClassMap.RegisterClassMap<CommandBase>();
            BsonClassMap.RegisterClassMap<CommitTransmittalsCommand>();
            BsonClassMap.RegisterClassMap<CrawlEmailCommand>();
            BsonClassMap.RegisterClassMap<CreateTransmittalsCommand>();
            BsonClassMap.RegisterClassMap<GridDropCommand>();
            BsonClassMap.RegisterClassMap<IncrementLifecycleCommand>();
            BsonClassMap.RegisterClassMap<NewAdminBinderCommand>();
            BsonClassMap.RegisterClassMap<NewEmailBinderCommand>();
            BsonClassMap.RegisterClassMap<NewHumanCommand>();
            BsonClassMap.RegisterClassMap<NewInputDocumentFromPathCommand>();
            BsonClassMap.RegisterClassMap<NewEmailCommand>();
            BsonClassMap.RegisterClassMap<NewEmailFromImportCommand>();
            BsonClassMap.RegisterClassMap<NewLibraryBinderCommand>();
            BsonClassMap.RegisterClassMap<NewOrganisationCommand>();
            BsonClassMap.RegisterClassMap<NewOutputDocumentFromDocumentCommand>();
            BsonClassMap.RegisterClassMap<NewOutputDocumentRevisionCommand>();
            BsonClassMap.RegisterClassMap<NewPartyDetailCommand>();
            BsonClassMap.RegisterClassMap<NewStubCommand>();
            BsonClassMap.RegisterClassMap<NewWorkBinderCommand>();
            BsonClassMap.RegisterClassMap<TabDropCommand>();
        }

        public void Stop()
        {
            DatabaseProvider.Current.GetDatabase().Server.Shutdown();
        }

        protected virtual int GetPortForMongo()
        {
            var portsInUse = new HashSet<int>(
                IPGlobalProperties
                .GetIPGlobalProperties()
                .GetActiveTcpListeners()
                .Select(c => c.Port)
                );

            var chosenPort = 27017;

            while (portsInUse.Contains(chosenPort)) chosenPort++;

            return chosenPort;
        }

        protected virtual string GetPathToMongod()
        {
            var programFiles = Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles);
            if (programFiles.Contains(Resources.Filter_x86)) programFiles = programFiles.Replace(Resources.Filter_x86, string.Empty);

            var serverPath = ConfigurationManager.AppSettings["DbServerPath"].ToString();

            return programFiles + serverPath;
        }

        private void StartMongoDaemon()
        {
            var mongodPath = GetPathToMongod();
            var port = GetPortForMongo();

            if (!Directory.Exists(FsEntity.GetDataDir())) Directory.CreateDirectory(FsEntity.GetDataDir());
            var dbPath = FsEntity.GetDataDir();

            var processStartInfo = new ProcessStartInfo
            {
                UseShellExecute = false,
                CreateNoWindow = true,
                FileName = mongodPath,
                Arguments = string.Format(@"--port {0} --dbpath ""{1}"" --smallfiles", port, dbPath)
            };

            Logger.DebugFormat(Resources.uiMsg_MongoStartupFeedback, mongodPath, port, dbPath);

            Process.Start(processStartInfo);

            var provider = DatabaseProvider.Current as MongoDatabaseProvider;
            if (provider == null) return;

            provider.Configure(port);
            provider.TryConnect();
        }
    }
}