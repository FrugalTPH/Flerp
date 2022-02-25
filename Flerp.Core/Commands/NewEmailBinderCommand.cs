using System;
using System.Globalization;
using Flerp.DomainModel;

namespace Flerp.Commands
{
    public class NewEmailBinderCommand : CommandBase
    {
        public override WqController Queue { get { return Controller.Queues[WqType.Q1]; } }
        public override string CmdDescription { get { return string.Format("New Email Binder ({0}).", Args["Date"]); } }

        public NewEmailBinderCommand(DateTime date) 
        {
            Args.Add("Date", date.ToString(CultureInfo.InvariantCulture));
        }

        public override void Execute()
        {
            var date = DateTime.Parse(Args["Date"]);

            Results.Add(Binder.CreateEmail(date).Id);
        }

        public override void Unexecute() { }
    }
}
