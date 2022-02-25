using System.ComponentModel;
using DevExpress.XtraEditors;
using DevExpress.XtraGrid;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraVerticalGrid;

namespace Flerp.Client.Views
{
    partial class DocView
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
            this.gridControl_document = new DevExpress.XtraGrid.GridControl();
            this.gridView3 = new DevExpress.XtraGrid.Views.Grid.GridView();
            ((System.ComponentModel.ISupportInitialize)(this.gridControl_document)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView3)).BeginInit();
            this.SuspendLayout();
            // 
            // gridControl_document
            // 
            this.gridControl_document.AllowDrop = true;
            this.gridControl_document.Cursor = System.Windows.Forms.Cursors.Default;
            this.gridControl_document.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridControl_document.Location = new System.Drawing.Point(0, 0);
            this.gridControl_document.MainView = this.gridView3;
            this.gridControl_document.Name = "gridControl_document";
            this.gridControl_document.Size = new System.Drawing.Size(1265, 678);
            this.gridControl_document.TabIndex = 0;
            this.gridControl_document.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gridView3});
            // 
            // gridView3
            // 
            this.gridView3.GridControl = this.gridControl_document;
            this.gridView3.Name = "gridView3";
            // 
            // DocView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1265, 678);
            this.Controls.Add(this.gridControl_document);
            this.Name = "DocView";
            this.Text = "DocView";
            ((System.ComponentModel.ISupportInitialize)(this.gridControl_document)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView3)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private GridControl gridControl_document;
        private GridView gridView3;
    }
}