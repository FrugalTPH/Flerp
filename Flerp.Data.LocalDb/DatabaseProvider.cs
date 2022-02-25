using Flerp.Data.LocalDb.Properties;
using MongoDB.Driver;
using System;
using System.Configuration;
using System.Threading;

namespace Flerp.Data.LocalDb
{
    public abstract class DatabaseProvider
    {
        public static DatabaseProvider Current { get; private set; }

        static DatabaseProvider()
        {
            Current = new MongoDatabaseProvider();
        }

        public abstract MongoDatabase GetDatabase();
    }

    public sealed class MongoDatabaseProvider : DatabaseProvider
    {
        private MongoServer _server;
        private bool _isConfigured;
        private readonly object _thisLock = new object();

        public override MongoDatabase GetDatabase()
        {
            if (!_isConfigured) throw new InvalidOperationException(Resources.uiMsg_MongoNotConfigured);

            return GetDatabase(Resources.FlerpDbName);
        }

        public void Configure(int port)
        {
            lock (_thisLock)
            {
                if (_isConfigured) throw new InvalidOperationException(Resources.uiMsg_MongoAlreadyConfigured);

                var connectionString = ConfigurationManager.AppSettings["ConnectionString"].ToString();

                var url = string.Format(connectionString, port);
                var mongoUrl = MongoUrl.Create(url);
                SetServer(mongoUrl);

                _isConfigured = true;
            }
        }

        private MongoDatabase GetDatabase(string databaseName)
        {
            return _server.GetDatabase(databaseName);
        }

        private void SetServer(MongoUrl mongoUrl)
        {
            _server = new MongoClient(mongoUrl).GetServer();
            
        }

        public void TryConnect()
        {
            for (var i = 0; i < 100; i++)
            {
                try
                {
                    Thread.Sleep(250);
                    GetDatabase().GetStats();
                    return;
                }
                catch (Exception)
                {
                    Console.WriteLine(Resources.uiMsg_MongoConnectionFailed, (i + 1));
                    Thread.Sleep(250);
                }
            }
        }
    }
}