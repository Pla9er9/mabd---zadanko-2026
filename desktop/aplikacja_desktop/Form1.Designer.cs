namespace aplikacja_desktop
{
    partial class Form1
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            this.dgvData = new System.Windows.Forms.DataGridView();
            this.txtId = new System.Windows.Forms.TextBox();
            this.lblId = new System.Windows.Forms.Label();
            this.txtNazwa = new System.Windows.Forms.TextBox();
            this.lblNazwa = new System.Windows.Forms.Label();
            this.txtCena = new System.Windows.Forms.TextBox();
            this.lblCena = new System.Windows.Forms.Label();
            this.btnDodaj = new System.Windows.Forms.Button();
            this.btnEdytuj = new System.Windows.Forms.Button();
            this.btnUsun = new System.Windows.Forms.Button();
            this.txtSzukaj = new System.Windows.Forms.TextBox();
            this.lblSzukaj = new System.Windows.Forms.Label();
            this.btnRaport = new System.Windows.Forms.Button();
            this.lblError = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.dgvData)).BeginInit();
            this.SuspendLayout();
            // 
            // dgvData
            // 
            this.dgvData.AllowUserToAddRows = false;
            this.dgvData.AllowUserToDeleteRows = false;
            this.dgvData.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvData.Location = new System.Drawing.Point(12, 50);
            this.dgvData.MultiSelect = false;
            this.dgvData.Name = "dgvData";
            this.dgvData.ReadOnly = true;
            this.dgvData.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvData.Size = new System.Drawing.Size(460, 380);
            this.dgvData.TabIndex = 0;
            this.dgvData.SelectionChanged += new System.EventHandler(this.dgvData_SelectionChanged);
            // 
            // txtId
            // 
            this.txtId.Location = new System.Drawing.Point(500, 70);
            this.txtId.Name = "txtId";
            this.txtId.ReadOnly = true;
            this.txtId.Size = new System.Drawing.Size(260, 23);
            this.txtId.TabIndex = 1;
            // 
            // lblId
            // 
            this.lblId.AutoSize = true;
            this.lblId.Location = new System.Drawing.Point(500, 52);
            this.lblId.Name = "lblId";
            this.lblId.Size = new System.Drawing.Size(21, 15);
            this.lblId.Text = "ID:";
            // 
            // txtNazwa
            // 
            this.txtNazwa.Location = new System.Drawing.Point(500, 120);
            this.txtNazwa.Name = "txtNazwa";
            this.txtNazwa.Size = new System.Drawing.Size(260, 23);
            this.txtNazwa.TabIndex = 2;
            // 
            // lblNazwa
            // 
            this.lblNazwa.AutoSize = true;
            this.lblNazwa.Location = new System.Drawing.Point(500, 102);
            this.lblNazwa.Name = "lblNazwa";
            this.lblNazwa.Size = new System.Drawing.Size(99, 15);
            this.lblNazwa.Text = "Nazwa produktu:";
            // 
            // txtCena
            // 
            this.txtCena.Location = new System.Drawing.Point(500, 170);
            this.txtCena.Name = "txtCena";
            this.txtCena.Size = new System.Drawing.Size(260, 23);
            this.txtCena.TabIndex = 3;
            // 
            // lblCena
            // 
            this.lblCena.AutoSize = true;
            this.lblCena.Location = new System.Drawing.Point(500, 152);
            this.lblCena.Name = "lblCena";
            this.lblCena.Size = new System.Drawing.Size(37, 15);
            this.lblCena.Text = "Cena:";
            // 
            // btnDodaj
            // 
            this.btnDodaj.Location = new System.Drawing.Point(500, 240);
            this.btnDodaj.Name = "btnDodaj";
            this.btnDodaj.Size = new System.Drawing.Size(80, 30);
            this.btnDodaj.TabIndex = 4;
            this.btnDodaj.Text = "Dodaj";
            this.btnDodaj.Click += new System.EventHandler(this.btnDodaj_Click);
            // 
            // btnEdytuj
            // 
            this.btnEdytuj.Location = new System.Drawing.Point(590, 240);
            this.btnEdytuj.Name = "btnEdytuj";
            this.btnEdytuj.Size = new System.Drawing.Size(80, 30);
            this.btnEdytuj.TabIndex = 5;
            this.btnEdytuj.Text = "Edytuj";
            this.btnEdytuj.Click += new System.EventHandler(this.btnEdytuj_Click);
            // 
            // btnUsun
            // 
            this.btnUsun.Location = new System.Drawing.Point(680, 240);
            this.btnUsun.Name = "btnUsun";
            this.btnUsun.Size = new System.Drawing.Size(80, 30);
            this.btnUsun.TabIndex = 6;
            this.btnUsun.Text = "Usuń";
            this.btnUsun.Click += new System.EventHandler(this.btnUsun_Click);
            // 
            // txtSzukaj
            // 
            this.txtSzukaj.Location = new System.Drawing.Point(12, 21);
            this.txtSzukaj.Name = "txtSzukaj";
            this.txtSzukaj.Size = new System.Drawing.Size(300, 23);
            this.txtSzukaj.TabIndex = 7;
            this.txtSzukaj.TextChanged += new System.EventHandler(this.txtSzukaj_TextChanged);
            // 
            // lblSzukaj
            // 
            this.lblSzukaj.AutoSize = true;
            this.lblSzukaj.Location = new System.Drawing.Point(12, 3);
            this.lblSzukaj.Name = "lblSzukaj";
            this.lblSzukaj.Size = new System.Drawing.Size(103, 15);
            this.lblSzukaj.Text = "Wyszukaj (Nazwa):";
            // 
            // btnRaport
            // 
            this.btnRaport.BackColor = System.Drawing.Color.LightGreen;
            this.btnRaport.Location = new System.Drawing.Point(500, 400);
            this.btnRaport.Name = "btnRaport";
            this.btnRaport.Size = new System.Drawing.Size(260, 30);
            this.btnRaport.TabIndex = 8;
            this.btnRaport.Text = "Generuj raport CSV";
            this.btnRaport.UseVisualStyleBackColor = false;
            this.btnRaport.Click += new System.EventHandler(this.btnRaport_Click);
            // 
            // lblError
            // 
            this.lblError.AutoSize = true;
            this.lblError.ForeColor = System.Drawing.Color.Red;
            this.lblError.Location = new System.Drawing.Point(500, 205);
            this.lblError.Name = "lblError";
            this.lblError.Size = new System.Drawing.Size(0, 15);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(784, 442);
            this.Controls.Add(this.lblError);
            this.Controls.Add(this.btnRaport);
            this.Controls.Add(this.lblSzukaj);
            this.Controls.Add(this.txtSzukaj);
            this.Controls.Add(this.btnUsun);
            this.Controls.Add(this.btnEdytuj);
            this.Controls.Add(this.btnDodaj);
            this.Controls.Add(this.lblCena);
            this.Controls.Add(this.txtCena);
            this.Controls.Add(this.lblNazwa);
            this.Controls.Add(this.txtNazwa);
            this.Controls.Add(this.lblId);
            this.Controls.Add(this.txtId);
            this.Controls.Add(this.dgvData);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "Form1";
            this.Text = "System Zarządzania Produktami";
            ((System.ComponentModel.ISupportInitialize)(this.dgvData)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        #endregion

        private System.Windows.Forms.DataGridView dgvData;
        private System.Windows.Forms.TextBox txtId;
        private System.Windows.Forms.Label lblId;
        private System.Windows.Forms.TextBox txtNazwa;
        private System.Windows.Forms.Label lblNazwa;
        private System.Windows.Forms.TextBox txtCena;
        private System.Windows.Forms.Label lblCena;
        private System.Windows.Forms.Button btnDodaj;
        private System.Windows.Forms.Button btnEdytuj;
        private System.Windows.Forms.Button btnUsun;
        private System.Windows.Forms.TextBox txtSzukaj;
        private System.Windows.Forms.Label lblSzukaj;
        private System.Windows.Forms.Button btnRaport;
        private System.Windows.Forms.Label lblError;
    }
}