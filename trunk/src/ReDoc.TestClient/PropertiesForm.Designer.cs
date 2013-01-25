namespace ReDoc.TestClient
{
    partial class PropertiesForm
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
            this.PropertiesGrid = new System.Windows.Forms.DataGridView();
            this.GetCustomers = new System.Windows.Forms.Button();
            this.TenantId = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.AddCustomer = new System.Windows.Forms.Button();
            this.UpdateCustomer = new System.Windows.Forms.Button();
            this.UniqueId = new System.Windows.Forms.TextBox();
            ((System.ComponentModel.ISupportInitialize)(this.PropertiesGrid)).BeginInit();
            this.SuspendLayout();
            // 
            // PropertiesGrid
            // 
            this.PropertiesGrid.AllowUserToAddRows = false;
            this.PropertiesGrid.AllowUserToDeleteRows = false;
            this.PropertiesGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.PropertiesGrid.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically;
            this.PropertiesGrid.Location = new System.Drawing.Point(12, 155);
            this.PropertiesGrid.MultiSelect = false;
            this.PropertiesGrid.Name = "PropertiesGrid";
            this.PropertiesGrid.ReadOnly = true;
            this.PropertiesGrid.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.PropertiesGrid.Size = new System.Drawing.Size(325, 223);
            this.PropertiesGrid.TabIndex = 0;
            this.PropertiesGrid.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.CustomersGrid_CellClick);
            // 
            // GetCustomers
            // 
            this.GetCustomers.Location = new System.Drawing.Point(222, 120);
            this.GetCustomers.Name = "GetCustomers";
            this.GetCustomers.Size = new System.Drawing.Size(115, 29);
            this.GetCustomers.TabIndex = 1;
            this.GetCustomers.Text = "Get Properties";
            this.GetCustomers.UseVisualStyleBackColor = true;
            this.GetCustomers.Click += new System.EventHandler(this.GetCustomers_Click);
            // 
            // TenantId
            // 
            this.TenantId.Location = new System.Drawing.Point(97, 12);
            this.TenantId.Name = "TenantId";
            this.TenantId.Size = new System.Drawing.Size(100, 20);
            this.TenantId.TabIndex = 2;
            this.TenantId.Text = "1";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(13, 12);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(56, 13);
            this.label1.TabIndex = 3;
            this.label1.Text = "Tenant Id:";
            // 
            // AddCustomer
            // 
            this.AddCustomer.Location = new System.Drawing.Point(360, 120);
            this.AddCustomer.Name = "AddCustomer";
            this.AddCustomer.Size = new System.Drawing.Size(115, 29);
            this.AddCustomer.TabIndex = 4;
            this.AddCustomer.Text = "Add Property";
            this.AddCustomer.UseVisualStyleBackColor = true;
            this.AddCustomer.Click += new System.EventHandler(this.AddCustomer_Click);
            // 
            // UpdateCustomer
            // 
            this.UpdateCustomer.Location = new System.Drawing.Point(360, 349);
            this.UpdateCustomer.Name = "UpdateCustomer";
            this.UpdateCustomer.Size = new System.Drawing.Size(115, 29);
            this.UpdateCustomer.TabIndex = 5;
            this.UpdateCustomer.Text = "Update Property";
            this.UpdateCustomer.UseVisualStyleBackColor = true;
            this.UpdateCustomer.Click += new System.EventHandler(this.UpdateCustomer_Click);
            // 
            // UniqueId
            // 
            this.UniqueId.Location = new System.Drawing.Point(360, 323);
            this.UniqueId.Name = "UniqueId";
            this.UniqueId.Size = new System.Drawing.Size(329, 20);
            this.UniqueId.TabIndex = 6;
            // 
            // Customers
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(701, 392);
            this.Controls.Add(this.UniqueId);
            this.Controls.Add(this.UpdateCustomer);
            this.Controls.Add(this.AddCustomer);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.TenantId);
            this.Controls.Add(this.GetCustomers);
            this.Controls.Add(this.PropertiesGrid);
            this.Name = "Customers";
            this.Text = "Form1";
            ((System.ComponentModel.ISupportInitialize)(this.PropertiesGrid)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView PropertiesGrid;
        private System.Windows.Forms.Button GetCustomers;
        private System.Windows.Forms.TextBox TenantId;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button AddCustomer;
        private System.Windows.Forms.Button UpdateCustomer;
        private System.Windows.Forms.TextBox UniqueId;
    }
}

