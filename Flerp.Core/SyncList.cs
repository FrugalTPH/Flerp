using Flerp.DomainModel;
using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace Flerp
{
    public class SyncList<T> : BindingList<T> where T : class, IPersistable
    {
        public SyncList(ISynchronizeInvoke syncObject, string name)
        {
            Name = name;

            _syncObject = syncObject;
            _fireListChangedEventAction = FireListChangedEvent;
        }

        public string Name { get; set; }

        private readonly ISynchronizeInvoke _syncObject;

        private readonly Action<ListChangedEventArgs> _fireListChangedEventAction;


        public void AddRange(IList<T> range)
        {
            RaiseListChangedEvents = false;

            foreach (var item in range) { Add(item); }

            RaiseListChangedEvents = true;
        }

        private void FireListChangedEvent(ListChangedEventArgs args) 
        { 
            base.OnListChanged(args); 
        }
        
        protected override void OnListChanged(ListChangedEventArgs args)
        {
            if (_syncObject == null)
            {
                FireListChangedEvent(args);
            }
            else
            {
                _syncObject.Invoke(_fireListChangedEventAction, new object[] { args });
            }
        }
    }
}
