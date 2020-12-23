namespace Rollout.UI.Winform
{
    partial class main
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
            this.btn_LoadRolloutSH = new System.Windows.Forms.Button();
            this.btn_LoadShipToAB = new System.Windows.Forms.Button();
            this.btn_LoadShipToCM = new System.Windows.Forms.Button();
            this.btn_Exit = new System.Windows.Forms.Button();
            this.of_OpenRollout = new System.Windows.Forms.OpenFileDialog();
            this.dgv_DataDisplay = new System.Windows.Forms.DataGridView();
            this.btn_SaveCSV = new System.Windows.Forms.Button();
            this.btn_FreightUpdate = new System.Windows.Forms.Button();
            this.lb_VersionInfo = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.dgv_DataDisplay)).BeginInit();
            this.SuspendLayout();
            // 
            // btn_LoadRolloutSH
            // 
            this.btn_LoadRolloutSH.Font = new System.Drawing.Font("Montserrat", 8.999999F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn_LoadRolloutSH.Location = new System.Drawing.Point(28, 13);
            this.btn_LoadRolloutSH.Name = "btn_LoadRolloutSH";
            this.btn_LoadRolloutSH.Size = new System.Drawing.Size(277, 34);
            this.btn_LoadRolloutSH.TabIndex = 0;
            this.btn_LoadRolloutSH.Text = "Load Rollout Spreadsheet";
            this.btn_LoadRolloutSH.UseVisualStyleBackColor = true;
            this.btn_LoadRolloutSH.Click += new System.EventHandler(this.btn_LoadRollout_DoIt);
            // 
            // btn_LoadShipToAB
            // 
            this.btn_LoadShipToAB.Font = new System.Drawing.Font("Montserrat", 8.999999F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn_LoadShipToAB.Location = new System.Drawing.Point(594, 12);
            this.btn_LoadShipToAB.Name = "btn_LoadShipToAB";
            this.btn_LoadShipToAB.Size = new System.Drawing.Size(277, 34);
            this.btn_LoadShipToAB.TabIndex = 1;
            this.btn_LoadShipToAB.Text = "Load New Ship To into AB";
            this.btn_LoadShipToAB.UseVisualStyleBackColor = true;
            this.btn_LoadShipToAB.Click += new System.EventHandler(this.btn_LoadAddressBook_DoIt);
            // 
            // btn_LoadShipToCM
            // 
            this.btn_LoadShipToCM.Font = new System.Drawing.Font("Montserrat", 8.999999F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn_LoadShipToCM.Location = new System.Drawing.Point(877, 12);
            this.btn_LoadShipToCM.Name = "btn_LoadShipToCM";
            this.btn_LoadShipToCM.Size = new System.Drawing.Size(277, 34);
            this.btn_LoadShipToCM.TabIndex = 2;
            this.btn_LoadShipToCM.Text = "Load New Ship To into Customer Master";
            this.btn_LoadShipToCM.UseVisualStyleBackColor = true;
            this.btn_LoadShipToCM.Click += new System.EventHandler(this.btn_LoadCustMast_DoIt);
            // 
            // btn_Exit
            // 
            this.btn_Exit.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btn_Exit.Location = new System.Drawing.Point(1433, 689);
            this.btn_Exit.Name = "btn_Exit";
            this.btn_Exit.Size = new System.Drawing.Size(75, 23);
            this.btn_Exit.TabIndex = 3;
            this.btn_Exit.Text = "Close";
            this.btn_Exit.UseVisualStyleBackColor = true;
            this.btn_Exit.Click += new System.EventHandler(this.btn_DoClose);
            // 
            // of_OpenRollout
            // 
            this.of_OpenRollout.FileName = "of_OpenRollout_FN";
            this.of_OpenRollout.ReadOnlyChecked = true;
            // 
            // dgv_DataDisplay
            // 
            this.dgv_DataDisplay.AllowUserToAddRows = false;
            this.dgv_DataDisplay.AllowUserToDeleteRows = false;
            this.dgv_DataDisplay.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dgv_DataDisplay.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
            this.dgv_DataDisplay.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.DisplayedCells;
            this.dgv_DataDisplay.CausesValidation = false;
            this.dgv_DataDisplay.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgv_DataDisplay.Location = new System.Drawing.Point(28, 65);
            this.dgv_DataDisplay.Name = "dgv_DataDisplay";
            this.dgv_DataDisplay.ReadOnly = true;
            this.dgv_DataDisplay.Size = new System.Drawing.Size(1480, 618);
            this.dgv_DataDisplay.TabIndex = 4;
            // 
            // btn_SaveCSV
            // 
            this.btn_SaveCSV.Font = new System.Drawing.Font("Montserrat", 8.999999F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn_SaveCSV.Location = new System.Drawing.Point(311, 12);
            this.btn_SaveCSV.Name = "btn_SaveCSV";
            this.btn_SaveCSV.Size = new System.Drawing.Size(277, 34);
            this.btn_SaveCSV.TabIndex = 5;
            this.btn_SaveCSV.Text = "Save Ship To (Tab Delimited)";
            this.btn_SaveCSV.UseVisualStyleBackColor = true;
            this.btn_SaveCSV.Click += new System.EventHandler(this.btn_SaveCSV_DoIt);
            // 
            // btn_FreightUpdate
            // 
            this.btn_FreightUpdate.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btn_FreightUpdate.Font = new System.Drawing.Font("Montserrat", 8.999999F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn_FreightUpdate.Location = new System.Drawing.Point(1231, 12);
            this.btn_FreightUpdate.Name = "btn_FreightUpdate";
            this.btn_FreightUpdate.Size = new System.Drawing.Size(277, 34);
            this.btn_FreightUpdate.TabIndex = 6;
            this.btn_FreightUpdate.Text = "Load Freight and Tracking Info";
            this.btn_FreightUpdate.UseVisualStyleBackColor = true;
            this.btn_FreightUpdate.Click += new System.EventHandler(this.btn_FreightUpdate_DoIt);
            // 
            // lb_VersionInfo
            // 
            this.lb_VersionInfo.AutoSize = true;
            this.lb_VersionInfo.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lb_VersionInfo.Location = new System.Drawing.Point(28, 698);
            this.lb_VersionInfo.Name = "lb_VersionInfo";
            this.lb_VersionInfo.Size = new System.Drawing.Size(88, 16);
            this.lb_VersionInfo.TabIndex = 7;
            this.lb_VersionInfo.Text = "lb_VersionInfo";
            // 
            // main
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1537, 724);
            this.Controls.Add(this.lb_VersionInfo);
            this.Controls.Add(this.btn_FreightUpdate);
            this.Controls.Add(this.btn_SaveCSV);
            this.Controls.Add(this.dgv_DataDisplay);
            this.Controls.Add(this.btn_Exit);
            this.Controls.Add(this.btn_LoadShipToCM);
            this.Controls.Add(this.btn_LoadShipToAB);
            this.Controls.Add(this.btn_LoadRolloutSH);
            this.MinimumSize = new System.Drawing.Size(1200, 725);
            this.Name = "main";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Suwanee Rollouts";
            this.Load += new System.EventHandler(this.main_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgv_DataDisplay)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btn_LoadRolloutSH;
        private System.Windows.Forms.Button btn_LoadShipToAB;
        private System.Windows.Forms.Button btn_LoadShipToCM;
        private System.Windows.Forms.Button btn_Exit;
        private System.Windows.Forms.OpenFileDialog of_OpenRollout;
        private System.Windows.Forms.DataGridView dgv_DataDisplay;
        private System.Windows.Forms.Button btn_SaveCSV;
        private System.Windows.Forms.Button btn_FreightUpdate;
        private System.Windows.Forms.Label lb_VersionInfo;
    }
}

