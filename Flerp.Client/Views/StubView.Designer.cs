using System.ComponentModel;
using DevExpress.XtraEditors;
using DevExpress.XtraGrid;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraVerticalGrid;

namespace Flerp.Client.Views
{
    partial class StubView
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
            this.gridControl_stub = new DevExpress.XtraGrid.GridControl();
            this.gridView2 = new DevExpress.XtraGrid.Views.Grid.GridView();
            ((System.ComponentModel.ISupportInitialize)(this.gridControl_stub)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView2)).BeginInit();
            this.SuspendLayout();
            // 
            // gridControl_stub
            // 
            this.gridControl_stub.Cursor = System.Windows.Forms.Cursors.Default;
            this.gridControl_stub.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridControl_stub.Location = new System.Drawing.Point(0, 0);
            this.gridControl_stub.MainView = this.gridView2;
            this.gridControl_stub.Name = "gridControl_stub";
            this.gridControl_stub.Size = new System.Drawing.Size(1265, 678);
            this.gridControl_stub.TabIndex = 0;
            this.gridControl_stub.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gridView2});
            // 
            // gridView2
            // 
            this.gridView2.GridControl = this.gridControl_stub;
            this.gridView2.Name = "gridView2";
            // 
            // StubView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1265, 678);
            this.Controls.Add(this.gridControl_stub);
            this.Name = "StubView";
            this.Text = "StubView";
            ((System.ComponentModel.ISupportInitialize)(this.gridControl_stub)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView2)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private GridControl gridControl_stub;
        private GridView gridView2;
    }
}