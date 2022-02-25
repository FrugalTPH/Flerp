using System.Collections.Generic;
using System.Linq;
using Flerp.DomainModel;

namespace Flerp.Commands
{
    public class TabDropCommand : CommandBase
    {
        public override string ConfirmCaption { get { return "Tab-drop"; } }
        public override string ConfirmText { get { return string.Format("Attempt to send {0} item(s) to {1}?", Args.Count - 1, Args["TargetId"]); } }
        public override WqController Queue { get { return Controller.Queues[WqType.Q2]; } }

        public TabDropCommand(string targetId, IEnumerable<string> dropData)
        {
            Args.Add("TargetId", targetId);

            var i = 0;
            foreach (var item in dropData)
            {
                Args.Add("DropData_" + i++, item);
            }
        }

        public override void Execute()
        {
            BinderBase target;
            Controller.TryGetEntityById(Args["TargetId"], out target);

            var dropData = Args.Where(x => x.Key.StartsWith("DropData_")).Select(x => x.Value);
            foreach (var id in dropData)
            {
                Cts.Token.ThrowIfCancellationRequested();

                IPersistable source;
                if (!Controller.TryGetEntityById(id, out source))
                {
                    Controller.Logger.Warn("Could not locate source - '" + id + "'.");
                    continue;
                }

                if (target is Party) ((IPartyable)source).ToParty((Party)target);
                else if (target is Binder) ((IBinderable)source).ToBinder((Binder)target, Cts);
                else if (Args["TargetId"] == "Basket") ((IBasketable)source).ToBasket(Cts);
            }
        }

        public override void Unexecute() { }
    }
}