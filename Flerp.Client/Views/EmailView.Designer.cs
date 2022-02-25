using System.ComponentModel;
using DevExpress.XtraEditors;
using DevExpress.XtraGrid;
using DevExpress.XtraGrid.Views.Grid;

namespace Flerp.Client.Views
{
    partial class EmailView
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.gridControl_email = new DevExpress.XtraGrid.GridControl();
            this.gridView2 = new DevExpress.XtraGrid.Views.Grid.GridView();
            ((System.ComponentModel.ISupportInitialize)(this.gridControl_email)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView2)).BeginInit();
            this.SuspendLayout();
            // 
            // gridControl_email
            // 
            this.gridControl_email.AllowDrop = true;
            this.gridControl_email.Cursor = System.Windows.Forms.Cursors.Default;
            this.gridControl_email.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridControl_email.Location = new System.Drawing.Point(0, 0);
            this.gridControl_email.MainView = this.gridView2;
            this.gridControl_email.Name = "gridControl_email";
            this.gridControl_email.Size = new System.Drawing.Size(1265, 678);
            this.gridControl_email.TabIndex = 0;
            this.gridControl_email.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gridView2});
            // 
            // gridView2
            // 
            this.gridView2.GridControl = this.gridControl_email;
            this.gridView2.Name = "gridView2";
            // 
            // EmailView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1265, 678);
            this.Controls.Add(this.gridControl_email);
            this.Name = "EmailView";
            this.Text = "EmailView";
            ((System.ComponentModel.ISupportInitialize)(this.gridControl_email)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView2)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private GridControl gridControl_email;
        private GridView gridView2;
    }
}