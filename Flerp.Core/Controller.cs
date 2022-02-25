using Autofac;
using Flerp.Client;
using Flerp.Commands;
using Flerp.Data;
using Flerp.DomainModel;
using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace Flerp
{
    public class Controller : IController
    {
        public static IRepository<AppSetting> AppSettings { get; private set; }     // Need to replace with ConfigManger.AppSettings

        public static SyncList<IBasketable> Basket { get; private set; }

        public static SyncList<IPersistable> Collection { get; private set; }

        public static IContainer Container { get; set; }

        public static ILog Logger { get; set; }

        public static IMainView MainView { get; private set; }

        public readonly static Dictionary<WqType, WqController> Queues = new Dictionary<WqType, WqController>();

        public static IRepository<IPersistable> Repository { get; private set; }
        

        public static T GetEntityById<T>(string id) where T : IPersistable
        {
            T entity;
            if (!TryGetEntityById(id, out entity)) throw new InvalidOperationException("Could not locate requested entity.");

            return entity;
        }

        public static WqStatus GetQueueStatus()
        {
            var statuses = Queues.Select(x => x.Value.Status);

            var wqStatuses = statuses as WqStatus[] ?? statuses.ToArray();

            if (wqStatuses.Contains(WqStatus.Wip)) return WqStatus.Wip;
            return wqStatuses.All(x => x == WqStatus.Idle) ? WqStatus.Idle : WqStatus.Paused;
        }

        public static IRepository<T> GetRepository<T>(string collectionName) where T : class, IPersistable
        {
            var r = Container.Resolve<IRepository<T>>();
            r.GetCollection(collectionName);

            return r;
        }

        public static string GetStats()
        {
            return string.Format("Basket: {0}/{1}/{2}     Collection: {3}     {4}: {5}", 
                Basket.OfType<Document>().Count(),
                Basket.OfType<Party>().Count(),
                Basket.OfType<Transmittal>().Count(), 
                Collection.Count, 
                GetQueueStatus(),
                Queues.Sum(x => x.Value.Count)
                );
        }
        
        public static void IssueCommand(CommandBase cmd)
        {
            if (cmd.ConfirmCaption != null && cmd.ConfirmText != null)
            {
                var dialogResult = MessageBox.Show(cmd.ConfirmText, cmd.ConfirmCaption, MessageBoxButtons.OKCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1);
                if (dialogResult != DialogResult.OK) return;
            }
            cmd.Queue.Enqueue(cmd);
        }

        public static void RegisterNew(IPersistable entity)
        {
            try
            {
                Repository.Save(entity);
                Collection.Add(entity);
            }
            catch
            {
                if (Repository.GetById(entity.Id) != null) Repository.Delete(entity);
                if (Collection.Contains(entity)) Collection.Remove(entity);
                throw;
            }
        }

        public void Start(IContainer container)
        {
            Container = container;

            Repository = GetRepository<IPersistable>("FlerpDb");
            AppSettings = GetRepository<AppSetting>("AppSettings");

            Collection = new SyncList<IPersistable>(MainView, "Collection");
            Collection.AddRange(Repository.GetAll());

            Basket = new SyncList<IBasketable>(MainView, "Basket");

            Queues.Add(WqType.Q1, new WqController(WqType.Q1.ToString()));      // Database-bound work.
            Queues.Add(WqType.Q2, new WqController(WqType.Q2.ToString()));      // Filesystem-bound work.

            MainView = container.Resolve<IMainView>();

            IssueCommand(new NewLibraryBinderCommand());                        // Creates LibBinder if not already created.
        }

        public static bool TryGetEntityById<T>(string id, out T entity) where T : IPersistable
        {
            entity = Collection.OfType<T>().FirstOrDefault(x => x.Id == id);
            return entity != null;
        }

        public static void Update(IPersistable entity)
        {
            Repository.Save(entity);

            if (Basket.Contains(entity))
            {
                var i = Basket.IndexOf((IBasketable)entity);
                if (i >= 0) Basket.ResetItem(i);
                else Basket.ResetBindings();
            }

            if (Collection.Contains(entity))
            {
                var i = Collection.IndexOf(entity);
                if (i >= 0) Collection.ResetItem(i);
                else Collection.ResetBindings();
            }
        }
    }
}