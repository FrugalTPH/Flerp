using System;
using System.Collections.Generic;
using System.Threading;
using Flerp.DomainModel;
using MongoDB.Bson;

namespace Flerp.Commands
{
    public abstract class CommandBase : IPersistable
    {
        public string Id { get; set; }
        private FlerpId _idF;
        public FlerpId IdF
        {
            get
            {
                if (_idF == default(FlerpId)) _idF = FlerpId.Parse(Id);
                return _idF;
            }
        }

        protected Dictionary<string, string> Args { get; set; }
        protected List<string> Results { get; set; }

        public abstract WqController Queue { get; }

        private CancellationTokenSource _cts;
        public CancellationTokenSource Cts 
        { 
            get { return _cts ?? (_cts = new CancellationTokenSource()); }
        }

        public virtual string ConfirmText { get { return null; } }
        public virtual string ConfirmCaption { get { return null; } }
        public virtual string CmdDescription { get { return "?"; } }


        protected CommandBase() 
        { 
            Id = new FlerpId(ObjectId.GenerateNewId().ToString()).ToString();
            Args = new Dictionary<string, string>();
            Results = new List<string>();
        }

        public virtual void Execute() { }

        public virtual void Unexecute() { }

        public void Save() { throw new InvalidOperationException(); }
        public void IdDoubleClick()
        {
            throw new NotImplementedException();
        }

        public void InvalidateCancellationToken()
        {
            _cts = null;
        }
    }
}
