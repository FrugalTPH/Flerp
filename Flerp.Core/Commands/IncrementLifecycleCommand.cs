using System;
using Flerp.DomainModel;

namespace Flerp.Commands
{
    public class IncrementLifecycleCommand : CommandBase
    {
        public override string ConfirmCaption { get { return "Increment Lifecycle Status"; } }
        public override string ConfirmText 
        { 
            get 
            {
                DocumentBase documentFs;
                if (!Controller.TryGetEntityById(Args["DocumentId"], out documentFs)) return null;

                if (documentFs.Status.DisplayName == LcStatusType.Pending.DisplayName) return string.Format("Release {0}?", Args["DocumentId"]);
                return documentFs.Status.DisplayName == LcStatusType.Released.DisplayName 
                    ? string.Format("Cancel {0}?", Args["DocumentId"]) 
                    : null;
            } 
        }
        public override WqController Queue { get { return Controller.Queues[WqType.Q1]; } }

        public IncrementLifecycleCommand(DocumentBase entity)
        {
            Args.Add("DocumentId", entity.Id);
        }

        public override void Execute()
        {
            var documentFs = Controller.GetEntityById<DocumentBase>(Args["DocumentId"]);

            if (documentFs.IsReleasable)
            {
                //DocumentBase release = null;
                //if (documentFs.TryGetByStatus(LcStatusType.Released, out release)) new IncrementLifecycleCommand(release).Execute();
                documentFs.Release(Cts, true);
            }
            else if (documentFs.IsCancellable) documentFs.Cancel();
            else throw new InvalidOperationException(string.Format("Could not increment status of '{0}'.", documentFs.Id));
        }

        public override void Unexecute() { }
    }
}
