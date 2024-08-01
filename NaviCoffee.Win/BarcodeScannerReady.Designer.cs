namespace NaviCoffee.Win
{
    partial class BarcodeScannerReady
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

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
            this.label1 = new System.Windows.Forms.Label();
            this.barcodesListView = new System.Windows.Forms.ListView();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(126, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Barcode scanner ready...";
            // 
            // barcodesListView
            // 
            this.barcodesListView.Alignment = System.Windows.Forms.ListViewAlignment.Left;
            this.barcodesListView.HideSelection = false;
            this.barcodesListView.Location = new System.Drawing.Point(13, 30);
            this.barcodesListView.Name = "barcodesListView";
            this.barcodesListView.Size = new System.Drawing.Size(775, 412);
            this.barcodesListView.TabIndex = 1;
            this.barcodesListView.UseCompatibleStateImageBehavior = false;
            this.barcodesListView.View = System.Windows.Forms.View.List;

            this.components = new System.ComponentModel.Container();
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.barcodesListView);
            this.Controls.Add(this.label1);
            this.Text = "BarcodeScannerReader";
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ListView barcodesListView;
    }
}