using System.ComponentModel;
using DevExpress.XtraEditors;
using DevExpress.XtraGrid;
using DevExpress.XtraGrid.Views.Grid;

namespace Flerp.Client.Views
{
    partial class BasketView
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
            this.splitContainerControl1 = new DevExpress.XtraEditors.SplitContainerControl();
            this.splitContainerControl2 = new DevExpress.XtraEditors.SplitContainerControl();
            this.gridControl_document = new DevExpress.XtraGrid.GridControl();
            this.gridView1 = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.gridControl_party = new DevExpress.XtraGrid.GridControl();
            this.gridView2 = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.gridControl_transmittal = new DevExpress.XtraGrid.GridControl();
            this.gridView3 = new DevExpress.XtraGrid.Views.Grid.GridView();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerControl1)).BeginInit();
            this.splitContainerControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerControl2)).BeginInit();
            this.splitContainerControl2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridControl_document)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridControl_party)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridControl_transmittal)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView3)).BeginInit();
            this.SuspendLayout();
            // 
            // splitContainerControl1
            // 
            this.splitContainerControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainerControl1.FixedPanel = DevExpress.XtraEditors.SplitFixedPanel.None;
            this.splitContainerControl1.Location = new System.Drawing.Point(0, 0);
            this.splitContainerControl1.Name = "splitContainerControl1";
            this.splitContainerControl1.Panel1.Controls.Add(this.splitContainerControl2);
            this.splitContainerControl1.Panel1.Text = "Panel1";
            this.splitContainerControl1.Panel2.Controls.Add(this.gridControl_transmittal);
            this.splitContainerControl1.Panel2.Text = "Panel2";
            this.splitContainerControl1.Size = new System.Drawing.Size(1265, 678);
            this.splitContainerControl1.SplitterPosition = 633;
            this.splitContainerControl1.TabIndex = 4;
            this.splitContainerControl1.Text = "splitContainerControl1";
            // 
            // splitContainerControl2
            // 
            this.splitContainerControl2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainerControl2.Horizontal = false;
            this.splitContainerControl2.Location = new System.Drawing.Point(0, 0);
            this.splitContainerControl2.Name = "splitContainerControl2";
            this.splitContainerControl2.Panel1.Controls.Add(this.gridControl_party);
            this.splitContainerControl2.Panel1.Text = "Panel1";
            this.splitContainerControl2.Panel2.Controls.Add(this.gridControl_document);
            this.splitContainerControl2.Panel2.Text = "Panel2";
            this.splitContainerControl2.Size = new System.Drawing.Size(633, 678);
            this.splitContainerControl2.SplitterPosition = 332;
            this.splitContainerControl2.TabIndex = 0;
            this.splitContainerControl2.Text = "splitContainerControl2";
            // 
            // gridControl_document
            // 
            this.gridControl_document.Cursor = System.Windows.Forms.Cursors.Default;
            this.gridControl_document.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridControl_document.Location = new System.Drawing.Point(0, 0);
            this.gridControl_document.MainView = this.gridView1;
            this.gridControl_document.Name = "gridControl_document";
            this.gridControl_document.Size = new System.Drawing.Size(633, 341);
            this.gridControl_document.TabIndex = 0;
            this.gridControl_document.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gridView1});
            // 
            // gridView1
            // 
            this.gridView1.GridControl = this.gridControl_document;
            this.gridView1.Name = "gridView1";
            // 
            // gridControl_party
            // 
            this.gridControl_party.Cursor = System.Windows.Forms.Cursors.Default;
            this.gridControl_party.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridControl_party.Location = new System.Drawing.Point(0, 0);
            this.gridControl_party.MainView = this.gridView2;
            this.gridControl_party.Name = "gridControl_party";
            this.gridControl_party.Size = new System.Drawing.Size(633, 332);
            this.gridControl_party.TabIndex = 0;
            this.gridControl_party.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gridView2});
            // 
            // gridView2
            // 
            this.gridView2.GridControl = this.gridControl_party;
            this.gridView2.Name = "gridView2";
            // 
            // gridControl_transmittal
            // 
            this.gridControl_transmittal.Cursor = System.Windows.Forms.Cursors.Default;
            this.gridControl_transmittal.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridControl_transmittal.Location = new System.Drawing.Point(0, 0);
            this.gridControl_transmittal.MainView = this.gridView3;
            this.gridControl_transmittal.Name = "gridControl_transmittal";
            this.gridControl_transmittal.Size = new System.Drawing.Size(627, 678);
            this.gridControl_transmittal.TabIndex = 0;
            this.gridControl_transmittal.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gridView3});
            // 
            // gridView3
            // 
            this.gridView3.GridControl = this.gridControl_transmittal;
            this.gridView3.Name = "gridView3";
            // 
            // BasketView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1265, 678);
            this.Controls.Add(this.splitContainerControl1);
            this.Name = "BasketView";
            this.Text = "BasketView";
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerControl1)).EndInit();
            this.splitContainerControl1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerControl2)).EndInit();
            this.splitContainerControl2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gridControl_document)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridControl_party)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridControl_transmittal)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView3)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private SplitContainerControl splitContainerControl1;
        private SplitContainerControl splitContainerControl2;
        private GridControl gridControl_document;
        private GridView gridView1;
        private GridControl gridControl_party;
        private GridView gridView2;
        private GridControl gridControl_transmittal;
        private GridView gridView3;
    }
}