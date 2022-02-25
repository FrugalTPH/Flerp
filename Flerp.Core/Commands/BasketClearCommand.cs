using System.Linq;
using Flerp.DomainModel;
using System.Windows.Forms;

namespace Flerp.Commands
{
    public class BasketClearCommand : CommandBase
    {
        public override string ConfirmCaption { get { return "Clear Basket"; } }
        public override string ConfirmText { get { return "Ok to delete all basket items?"; } }
        public override WqController Queue { get { return Controller.Queues[WqType.Q2]; } }

        public override void Execute()
        {
            Controller.Basket.Clear();
            Controller.MainView.ActiveView.RefreshView(true);
            Controller.MainView.RefreshBars();
            Controller.Logger.Info("Basket cleared.");
        }

        public override void Unexecute() { }
    }
}
