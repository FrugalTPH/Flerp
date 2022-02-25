using Flerp.Commands;
using Flerp.Data;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Flerp
{
    public class WqController
    {
        public WqController(string name)
        {
            Name = name;

            var cmds = Storage.GetAll();
            if (cmds.Count > 0) Restore(cmds);
        }


        private CommandBase _curCommand;
        private readonly Queue<CommandBase> _queue = new Queue<CommandBase>();
        private Task _queueProcessor;
        private WqStatus _status;
   
        public int Count 
        { 
            get 
            { 
                return 
                    _queue.Count; 
            } 
        }

        private string Name { get; set; }
                
        public WqStatus Status
        {
            get
            {
                if (_status != WqStatus.Wip) Status = _queue.Count == 0 ? WqStatus.Idle : WqStatus.Paused;
                return _status;
            }
            set
            {
                _status = value;
            }
        }

        private IRepository<CommandBase> Storage 
        { 
            get 
            { 
                return Controller.GetRepository<CommandBase>(Name); 
            } 
        }
        

        private async Task Cancel()
        {
            if (Status == WqStatus.Wip)
            {
                _curCommand.Cts.Cancel(true);
                await _queueProcessor;
            }           
        }

        public void Enqueue(CommandBase command)
        {
            _queue.Enqueue(command);
            if (_queueProcessor == null || _queueProcessor.IsCompleted || _queueProcessor.IsFaulted || _queueProcessor.IsCanceled) _queueProcessor = ProcessQueue();
        }

        private async Task ProcessQueue()
        {
            try
            {
                while (_queue.Count != 0)
                {
                    Status = WqStatus.Wip;
                    _curCommand = _queue.Peek();
                    try
                    {
                        await Task.Run(() => _curCommand.Execute());
                    }
                    catch (OperationCanceledException)
                    {
                        _curCommand.InvalidateCancellationToken();
                        return;
                    }
                    catch (Exception ex)
                    {
                        Controller.Logger.Warn(_curCommand.CmdDescription + " - " + ex.Message);
                    }
                    _queue.Dequeue();
                    if (_curCommand.Cts.Token.IsCancellationRequested) return;
                }
            }
            finally
            {
                _queueProcessor = null;
                _curCommand = null;
                Status = WqStatus.Idle;
            }
        }

        public async Task Save()
        {
            await Cancel();
            if (Status == WqStatus.Paused) foreach (var cmd in _queue) Storage.Save(cmd);
        }

        private void Restore(IList<CommandBase> cmds)
        {
            foreach (var cmd in cmds) Enqueue(cmd);
            Storage.Drop();
            Resume();
        }

        private void Resume() 
        { 
            if (Status == WqStatus.Paused) _queueProcessor = ProcessQueue(); 
        }

    }
}