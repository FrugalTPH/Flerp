using System.ComponentModel;
using DevExpress.XtraEditors;
using DevExpress.XtraGrid;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraVerticalGrid;

namespace Flerp.Client.Views
{
    partial class PartyView
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
            this.gridControl_partyDetail = new DevExpress.XtraGrid.GridControl();
            this.gridView1 = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.gridControl_partyParty = new DevExpress.XtraGrid.GridControl();
            this.gridView2 = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.propertyGridControl_party = new DevExpress.XtraVerticalGrid.PropertyGridControl();
            this.gridControl_transmittal = new DevExpress.XtraGrid.GridControl();
            this.gridView3 = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.gridView4 = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.gridControl_partyBinder = new DevExpress.XtraGrid.GridControl();
            this.gridView5 = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.splitContainerControl1 = new DevExpress.XtraEditors.SplitContainerControl();
            this.splitContainerControl2 = new DevExpress.XtraEditors.SplitContainerControl();
            ((System.ComponentModel.ISupportInitialize)(this.gridControl_partyDetail)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridControl_partyParty)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.propertyGridControl_party)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridControl_transmittal)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView4)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridControl_partyBinder)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView5)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerControl1)).BeginInit();
            this.splitContainerControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerControl2)).BeginInit();
            this.splitContainerControl2.SuspendLayout();
            this.SuspendLayout();
            // 
            // gridControl_partyDetail
            // 
            this.gridControl_partyDetail.Cursor = System.Windows.Forms.Cursors.Default;
            this.gridControl_partyDetail.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridControl_partyDetail.Location = new System.Drawing.Point(0, 175);
            this.gridControl_partyDetail.MainView = this.gridView1;
            this.gridControl_partyDetail.Name = "gridControl_partyDetail";
            this.gridControl_partyDetail.Size = new System.Drawing.Size(450, 280);
            this.gridControl_partyDetail.TabIndex = 0;
            this.gridControl_partyDetail.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gridView1});
            // 
            // gridView1
            // 
            this.gridView1.GridControl = this.gridControl_partyDetail;
            this.gridView1.Name = "gridView1";
            // 
            // gridControl_partyParty
            // 
            this.gridControl_partyParty.Cursor = System.Windows.Forms.Cursors.Default;
            this.gridControl_partyParty.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridControl_partyParty.Location = new System.Drawing.Point(0, 0);
            this.gridControl_partyParty.MainView = this.gridView2;
            this.gridControl_partyParty.Name = "gridControl_partyParty";
            this.gridControl_partyParty.Size = new System.Drawing.Size(815, 350);
            this.gridControl_partyParty.TabIndex = 0;
            this.gridControl_partyParty.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gridView2});
            // 
            // gridView2
            // 
            this.gridView2.GridControl = this.gridControl_partyParty;
            this.gridView2.Name = "gridView2";
            // 
            // propertyGridControl_party
            // 
            this.propertyGridControl_party.Dock = System.Windows.Forms.DockStyle.Top;
            this.propertyGridControl_party.Location = new System.Drawing.Point(0, 0);
            this.propertyGridControl_party.Name = "propertyGridControl_party";
            this.propertyGridControl_party.Size = new System.Drawing.Size(450, 175);
            this.propertyGridControl_party.TabIndex = 0;
            // 
            // gridControl_transmittal
            // 
            this.gridControl_transmittal.Cursor = System.Windows.Forms.Cursors.Default;
            this.gridControl_transmittal.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridControl_transmittal.Location = new System.Drawing.Point(0, 0);
            this.gridControl_transmittal.MainView = this.gridView4;
            this.gridControl_transmittal.Name = "gridControl_transmittal";
            this.gridControl_transmittal.Size = new System.Drawing.Size(815, 323);
            this.gridControl_transmittal.TabIndex = 0;
            this.gridControl_transmittal.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gridView4});
            // 
            // gridView3
            // 
            this.gridView3.GridControl = this.gridControl_partyBinder;
            this.gridView3.Name = "gridView3";
            // 
            // gridView4
            // 
            this.gridView4.GridControl = this.gridControl_transmittal;
            this.gridView4.Name = "gridView4";
            // 
            // gridControl_partyBinder
            // 
            this.gridControl_partyBinder.Cursor = System.Windows.Forms.Cursors.Default;
            this.gridControl_partyBinder.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridControl_partyBinder.Location = new System.Drawing.Point(0, 0);
            this.gridControl_partyBinder.MainView = this.gridView3;
            this.gridControl_partyBinder.Name = "gridControl_partyBinder";
            this.gridControl_partyBinder.Size = new System.Drawing.Size(450, 218);
            this.gridControl_partyBinder.TabIndex = 0;
            this.gridControl_partyBinder.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gridView3});
            // 
            // gridView5
            // 
            this.gridView5.GridControl = this.gridControl_partyBinder;
            this.gridView5.Name = "gridView5";
            // 
            // splitContainerControl1
            // 
            this.splitContainerControl1.Dock = System.Windows.Forms.DockStyle.Left;
            this.splitContainerControl1.Horizontal = false;
            this.splitContainerControl1.Location = new System.Drawing.Point(0, 0);
            this.splitContainerControl1.Name = "splitContainerControl1";
            this.splitContainerControl1.Panel1.Controls.Add(this.gridControl_partyDetail);
            this.splitContainerControl1.Panel1.Controls.Add(this.propertyGridControl_party);
            this.splitContainerControl1.Panel1.Text = "Panel1";
            this.splitContainerControl1.Panel2.Controls.Add(this.gridControl_partyBinder);
            this.splitContainerControl1.Panel2.Text = "Panel2";
            this.splitContainerControl1.Size = new System.Drawing.Size(450, 678);
            this.splitContainerControl1.SplitterPosition = 455;
            this.splitContainerControl1.TabIndex = 1;
            this.splitContainerControl1.Text = "splitContainerControl1";
            // 
            // splitContainerControl2
            // 
            this.splitContainerControl2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainerControl2.Horizontal = false;
            this.splitContainerControl2.Location = new System.Drawing.Point(450, 0);
            this.splitContainerControl2.Name = "splitContainerControl2";
            this.splitContainerControl2.Panel1.Controls.Add(this.gridControl_partyParty);
            this.splitContainerControl2.Panel1.Text = "Panel1";
            this.splitContainerControl2.Panel2.Controls.Add(this.gridControl_transmittal);
            this.splitContainerControl2.Panel2.Text = "Panel2";
            this.splitContainerControl2.Size = new System.Drawing.Size(815, 678);
            this.splitContainerControl2.SplitterPosition = 350;
            this.splitContainerControl2.TabIndex = 2;
            this.splitContainerControl2.Text = "splitContainerControl2";
            // 
            // PartyView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1265, 678);
            this.Controls.Add(this.splitContainerControl2);
            this.Controls.Add(this.splitContainerControl1);
            this.Name = "PartyView";
            this.Text = "PartyView";
            ((System.ComponentModel.ISupportInitialize)(this.gridControl_partyDetail)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridControl_partyParty)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.propertyGridControl_party)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridControl_transmittal)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView4)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridControl_partyBinder)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView5)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerControl1)).EndInit();
            this.splitContainerControl1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerControl2)).EndInit();
            this.splitContainerControl2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private PropertyGridControl propertyGridControl_party;
        private GridControl gridControl_partyDetail;
        private GridView gridView1;
        private GridControl gridControl_partyParty;
        private GridView gridView2;
        private GridControl gridControl_transmittal;
        private GridView gridView4;
        private GridView gridView3;
        private GridControl gridControl_partyBinder;
        private GridView gridView5;
        private SplitContainerControl splitContainerControl1;
        private SplitContainerControl splitContainerControl2;
    }
}