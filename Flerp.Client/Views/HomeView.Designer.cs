using System.ComponentModel;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using DevExpress.XtraGrid;
using DevExpress.XtraGrid.Views.Grid;

namespace Flerp.Client.Views
{
    partial class HomeView
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
            this.gridControl_party = new DevExpress.XtraGrid.GridControl();
            this.gridView1 = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.gridControl_binder = new DevExpress.XtraGrid.GridControl();
            this.gridView2 = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.gridControl_email = new DevExpress.XtraGrid.GridControl();
            this.gridView3 = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.gridControl_stub = new DevExpress.XtraGrid.GridControl();
            this.gridView4 = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.splitContainerControl1 = new DevExpress.XtraEditors.SplitContainerControl();
            this.splitContainerControl2 = new DevExpress.XtraEditors.SplitContainerControl();
            this.splitContainerControl3 = new DevExpress.XtraEditors.SplitContainerControl();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            ((System.ComponentModel.ISupportInitialize)(this.gridControl_party)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridControl_binder)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridControl_email)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridControl_stub)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView4)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerControl1)).BeginInit();
            this.splitContainerControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerControl2)).BeginInit();
            this.splitContainerControl2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerControl3)).BeginInit();
            this.splitContainerControl3.SuspendLayout();
            this.SuspendLayout();
            // 
            // gridControl_party
            // 
            this.gridControl_party.Cursor = System.Windows.Forms.Cursors.Default;
            this.gridControl_party.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridControl_party.Location = new System.Drawing.Point(0, 0);
            this.gridControl_party.MainView = this.gridView1;
            this.gridControl_party.Name = "gridControl_party";
            this.gridControl_party.Size = new System.Drawing.Size(600, 373);
            this.gridControl_party.TabIndex = 0;
            this.gridControl_party.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gridView1});
            // 
            // gridView1
            // 
            this.gridView1.GridControl = this.gridControl_party;
            this.gridView1.Name = "gridView1";
            // 
            // gridControl_binder
            // 
            this.gridControl_binder.Cursor = System.Windows.Forms.Cursors.Default;
            this.gridControl_binder.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridControl_binder.Location = new System.Drawing.Point(0, 0);
            this.gridControl_binder.MainView = this.gridView2;
            this.gridControl_binder.Name = "gridControl_binder";
            this.gridControl_binder.Size = new System.Drawing.Size(600, 300);
            this.gridControl_binder.TabIndex = 0;
            this.gridControl_binder.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gridView2});
            // 
            // gridView2
            // 
            this.gridView2.GridControl = this.gridControl_binder;
            this.gridView2.Name = "gridView2";
            // 
            // gridControl_email
            // 
            this.gridControl_email.AllowDrop = true;
            this.gridControl_email.Cursor = System.Windows.Forms.Cursors.Default;
            this.gridControl_email.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridControl_email.Location = new System.Drawing.Point(0, 0);
            this.gridControl_email.MainView = this.gridView3;
            this.gridControl_email.Name = "gridControl_email";
            this.gridControl_email.Size = new System.Drawing.Size(660, 300);
            this.gridControl_email.TabIndex = 3;
            this.gridControl_email.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gridView3});
            // 
            // gridView3
            // 
            this.gridView3.GridControl = this.gridControl_email;
            this.gridView3.Name = "gridView3";
            // 
            // gridControl_stub
            // 
            this.gridControl_stub.Cursor = System.Windows.Forms.Cursors.Default;
            this.gridControl_stub.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridControl_stub.Location = new System.Drawing.Point(0, 0);
            this.gridControl_stub.MainView = this.gridView4;
            this.gridControl_stub.Name = "gridControl_stub";
            this.gridControl_stub.Size = new System.Drawing.Size(660, 373);
            this.gridControl_stub.TabIndex = 0;
            this.gridControl_stub.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gridView4});
            // 
            // gridView4
            // 
            this.gridView4.GridControl = this.gridControl_stub;
            this.gridView4.Name = "gridView4";
            // 
            // splitContainerControl1
            // 
            this.splitContainerControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainerControl1.Location = new System.Drawing.Point(0, 0);
            this.splitContainerControl1.Name = "splitContainerControl1";
            this.splitContainerControl1.Panel1.Controls.Add(this.splitContainerControl2);
            this.splitContainerControl1.Panel1.MinSize = 600;
            this.splitContainerControl1.Panel1.Text = "Panel1";
            this.splitContainerControl1.Panel2.Controls.Add(this.splitContainerControl3);
            this.splitContainerControl1.Panel2.Text = "Panel2";
            this.splitContainerControl1.Size = new System.Drawing.Size(1265, 678);
            this.splitContainerControl1.SplitterPosition = 600;
            this.splitContainerControl1.TabIndex = 4;
            this.splitContainerControl1.Text = "splitContainerControl1";
            // 
            // splitContainerControl2
            // 
            this.splitContainerControl2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainerControl2.Horizontal = false;
            this.splitContainerControl2.Location = new System.Drawing.Point(0, 0);
            this.splitContainerControl2.Name = "splitContainerControl2";
            this.splitContainerControl2.Panel1.Controls.Add(this.gridControl_binder);
            this.splitContainerControl2.Panel1.Text = "Panel1";
            this.splitContainerControl2.Panel2.Controls.Add(this.gridControl_party);
            this.splitContainerControl2.Panel2.Text = "Panel2";
            this.splitContainerControl2.Size = new System.Drawing.Size(600, 678);
            this.splitContainerControl2.SplitterPosition = 300;
            this.splitContainerControl2.TabIndex = 0;
            this.splitContainerControl2.Text = "splitContainerControl2";
            // 
            // splitContainerControl3
            // 
            this.splitContainerControl3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainerControl3.Horizontal = false;
            this.splitContainerControl3.Location = new System.Drawing.Point(0, 0);
            this.splitContainerControl3.Name = "splitContainerControl3";
            this.splitContainerControl3.Panel1.Controls.Add(this.gridControl_email);
            this.splitContainerControl3.Panel1.Text = "Panel1";
            this.splitContainerControl3.Panel2.Controls.Add(this.gridControl_stub);
            this.splitContainerControl3.Panel2.Text = "Panel2";
            this.splitContainerControl3.Size = new System.Drawing.Size(660, 678);
            this.splitContainerControl3.SplitterPosition = 300;
            this.splitContainerControl3.TabIndex = 0;
            this.splitContainerControl3.Text = "splitContainerControl3";
            // 
            // HomeView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1265, 678);
            this.Controls.Add(this.splitContainerControl1);
            this.Name = "HomeView";
            this.Text = "HomeView";
            ((System.ComponentModel.ISupportInitialize)(this.gridControl_party)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridControl_binder)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridControl_email)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridControl_stub)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView4)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerControl1)).EndInit();
            this.splitContainerControl1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerControl2)).EndInit();
            this.splitContainerControl2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerControl3)).EndInit();
            this.splitContainerControl3.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private GridControl gridControl_party;
        private GridView gridView1;
        private GridControl gridControl_binder;
        private GridView gridView2;
        private GridControl gridControl_email;
        private GridView gridView3;
        private GridControl gridControl_stub;
        private GridView gridView4;
        private SplitContainerControl splitContainerControl1;
        private SplitContainerControl splitContainerControl2;
        private SplitContainerControl splitContainerControl3;
        private OpenFileDialog openFileDialog1;
    }
}