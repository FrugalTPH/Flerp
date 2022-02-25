namespace Flerp.Commands
{
    public class BasketRefreshCommand : CommandBase
    {
        public override WqController Queue { get { return Controller.Queues[WqType.Q2]; } }

        public override void Execute()
        {
            Controller.MainView.ActiveView.RefreshView(true);
            Controller.MainView.RefreshBars();
        }

        public override void Unexecute() { }
    }
}
