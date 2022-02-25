using DevExpress.XtraEditors;
using log4net.Appender;
using log4net.Core;
using System.Linq;
using System.Windows.Forms;

namespace Flerp.Client.Helpers
{
    public class TextBoxAppender : AppenderSkeleton
    {
        private MemoEdit _textBox;

        public string FormName { private get; set; }
        public string TextBoxName { private get; set; }

        private static Control FindControlRecursive(Control root, string textBoxName)
        {
            return root.Name == textBoxName
                ? root
                : (from Control c in root.Controls select FindControlRecursive(c, textBoxName)).FirstOrDefault(t => t != null);
        }

        protected override void Append(LoggingEvent loggingEvent)
        {
            if (_textBox == null)
            {
                if (string.IsNullOrEmpty(FormName) || string.IsNullOrEmpty(TextBoxName)) return;

                var form = Application.OpenForms[FormName];
                if (form == null) return;

                _textBox = (MemoEdit)FindControlRecursive(form, TextBoxName);
                if (_textBox == null) return;

                form.FormClosing += (s, e) => _textBox = null;
            }
            _textBox.BeginInvoke((MethodInvoker)delegate
            {
                _textBox.Text += RenderLoggingEvent(loggingEvent);
                _textBox.SelectionStart = _textBox.Text.Length;
                _textBox.ScrollToCaret();
            });
        }
    }
}
