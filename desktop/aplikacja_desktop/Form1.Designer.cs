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
            this.txtKategoria = new System.Windows.Forms.TextBox();
            this.lblKategoria = new System.Windows.Forms.Label();
            this.cmbPriorytet = new System.Windows.Forms.ComboBox();
            this.lblPriorytet = new System.Windows.Forms.Label();
            this.dtpTermin = new System.Windows.Forms.DateTimePicker();
            this.lblTermin = new System.Windows.Forms.Label();
            this.cmbStatus = new System.Windows.Forms.ComboBox();
            this.lblStatus = new System.Windows.Forms.Label();
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
            this.dgvData.Size = new System.Drawing.Size(580, 380);
            this.dgvData.TabIndex = 0;
            this.dgvData.SelectionChanged += new System.EventHandler(this.dgvData_SelectionChanged);
            // 
            // txtId
            // 
            this.txtId.Location = new System.Drawing.Point(610, 50);
            this.txtId.Name = "txtId";
            this.txtId.ReadOnly = true;
            this.txtId.Size = new System.Drawing.Size(260, 23);
            this.txtId.TabIndex = 1;
            // 
            // lblId
            // 
            this.lblId.AutoSize = true;
            this.lblId.Location = new System.Drawing.Point(610, 32);
            this.lblId.Name = "lblId";
            this.lblId.Size = new System.Drawing.Size(21, 15);
            this.lblId.Text = "ID:";
            // 
            // txtNazwa
            // 
            this.txtNazwa.Location = new System.Drawing.Point(610, 100);
            this.txtNazwa.Name = "txtNazwa";
            this.txtNazwa.Size = new System.Drawing.Size(260, 23);
            this.txtNazwa.TabIndex = 2;
            // 
            // lblNazwa
            // 
            this.lblNazwa.AutoSize = true;
            this.lblNazwa.Location = new System.Drawing.Point(610, 82);
            this.lblNazwa.Name = "lblNazwa";
            this.lblNazwa.Size = new System.Drawing.Size(87, 15);
            this.lblNazwa.Text = "Nazwa zadania:";
            // 
            // txtKategoria
            // 
            this.txtKategoria.Location = new System.Drawing.Point(610, 150);
            this.txtKategoria.Name = "txtKategoria";
            this.txtKategoria.Size = new System.Drawing.Size(260, 23);
            this.txtKategoria.TabIndex = 3;
            // 
            // lblKategoria
            // 
            this.lblKategoria.AutoSize = true;
            this.lblKategoria.Location = new System.Drawing.Point(610, 132);
            this.lblKategoria.Name = "lblKategoria";
            this.lblKategoria.Size = new System.Drawing.Size(60, 15);
            this.lblKategoria.Text = "Kategoria:";
            // 
            // cmbPriorytet
            // 
            this.cmbPriorytet.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbPriorytet.Location = new System.Drawing.Point(610, 200);
            this.cmbPriorytet.Name = "cmbPriorytet";
            this.cmbPriorytet.Size = new System.Drawing.Size(260, 23);
            this.cmbPriorytet.TabIndex = 4;
            // 
            // lblPriorytet
            // 
            this.lblPriorytet.AutoSize = true;
            this.lblPriorytet.Location = new System.Drawing.Point(610, 182);
            this.lblPriorytet.Name = "lblPriorytet";
            this.lblPriorytet.Size = new System.Drawing.Size(54, 15);
            this.lblPriorytet.Text = "Priorytet:";
            // 
            // dtpTermin
            // 
            this.dtpTermin.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpTermin.Location = new System.Drawing.Point(610, 250);
            this.dtpTermin.Name = "dtpTermin";
            this.dtpTermin.Size = new System.Drawing.Size(260, 23);
            this.dtpTermin.TabIndex = 5;
            // 
            // lblTermin
            // 
            this.lblTermin.AutoSize = true;
            this.lblTermin.Location = new System.Drawing.Point(610, 232);
            this.lblTermin.Name = "lblTermin";
            this.lblTermin.Size = new System.Drawing.Size(102, 15);
            this.lblTermin.Text = "Termin realizacji:";
            // 
            // cmbStatus
            // 
            this.cmbStatus.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbStatus.Location = new System.Drawing.Point(610, 300);
            this.cmbStatus.Name = "cmbStatus";
            this.cmbStatus.Size = new System.Drawing.Size(260, 23);
            this.cmbStatus.TabIndex = 6;
            // 
            // lblStatus
            // 
            this.lblStatus.AutoSize = true;
            this.lblStatus.Location = new System.Drawing.Point(610, 282);
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(42, 15);
            this.lblStatus.Text = "Status:";
            // 
            // btnDodaj
            // 
            this.btnDodaj.Location = new System.Drawing.Point(610, 355);
            this.btnDodaj.Name = "btnDodaj";
            this.btnDodaj.Size = new System.Drawing.Size(80, 30);
            this.btnDodaj.TabIndex = 7;
            this.btnDodaj.Text = "Dodaj";
            this.btnDodaj.Click += new System.EventHandler(this.btnDodaj_Click);
            // 
            // btnEdytuj
            // 
            this.btnEdytuj.Location = new System.Drawing.Point(700, 355);
            this.btnEdytuj.Name = "btnEdytuj";
            this.btnEdytuj.Size = new System.Drawing.Size(80, 30);
            this.btnEdytuj.TabIndex = 8;
            this.btnEdytuj.Text = "Edytuj";
            this.btnEdytuj.Click += new System.EventHandler(this.btnEdytuj_Click);
            // 
            // btnUsun
            // 
            this.btnUsun.Location = new System.Drawing.Point(790, 355);
            this.btnUsun.Name = "btnUsun";
            this.btnUsun.Size = new System.Drawing.Size(80, 30);
            this.btnUsun.TabIndex = 9;
            this.btnUsun.Text = "Usuń";
            this.btnUsun.Click += new System.EventHandler(this.btnUsun_Click);
            // 
            // txtSzukaj
            // 
            this.txtSzukaj.Location = new System.Drawing.Point(12, 21);
            this.txtSzukaj.Name = "txtSzukaj";
            this.txtSzukaj.Size = new System.Drawing.Size(300, 23);
            this.txtSzukaj.TabIndex = 10;
            this.txtSzukaj.TextChanged += new System.EventHandler(this.txtSzukaj_TextChanged);
            // 
            // lblSzukaj
            // 
            this.lblSzukaj.AutoSize = true;
            this.lblSzukaj.Location = new System.Drawing.Point(12, 3);
            this.lblSzukaj.Name = "lblSzukaj";
            this.lblSzukaj.Size = new System.Drawing.Size(154, 15);
            this.lblSzukaj.Text = "Wyszukaj (Nazwa/Kategoria):";
            // 
            // btnRaport
            // 
            this.btnRaport.BackColor = System.Drawing.Color.LightGreen;
            this.btnRaport.Location = new System.Drawing.Point(610, 400);
            this.btnRaport.Name = "btnRaport";
            this.btnRaport.Size = new System.Drawing.Size(260, 30);
            this.btnRaport.TabIndex = 11;
            this.btnRaport.Text = "Generuj raport zadań CSV";
            this.btnRaport.UseVisualStyleBackColor = false;
            this.btnRaport.Click += new System.EventHandler(this.btnRaport_Click);
            // 
            // lblError
            // 
            this.lblError.AutoSize = true;
            this.lblError.ForeColor = System.Drawing.Color.Red;
            this.lblError.Location = new System.Drawing.Point(610, 330);
            this.lblError.Name = "lblError";
            this.lblError.Size = new System.Drawing.Size(0, 15);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(894, 442);
            this.Controls.Add(this.lblError);
            this.Controls.Add(this.btnRaport);
            this.Controls.Add(this.lblSzukaj);
            this.Controls.Add(this.txtSzukaj);
            this.Controls.Add(this.btnUsun);
            this.Controls.Add(this.btnEdytuj);
            this.Controls.Add(this.btnDodaj);
            this.Controls.Add(this.lblStatus);
            this.Controls.Add(this.cmbStatus);
            this.Controls.Add(this.lblTermin);
            this.Controls.Add(this.dtpTermin);
            this.Controls.Add(this.lblPriorytet);
            this.Controls.Add(this.cmbPriorytet);
            this.Controls.Add(this.lblKategoria);
            this.Controls.Add(this.txtKategoria);
            this.Controls.Add(this.lblNazwa);
            this.Controls.Add(this.txtNazwa);
            this.Controls.Add(this.lblId);
            this.Controls.Add(this.txtId);
            this.Controls.Add(this.dgvData);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "Form1";
            this.Text = "System Zarządzania Zadaniami (To-Do)";
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
        private System.Windows.Forms.TextBox txtKategoria;
        private System.Windows.Forms.Label lblKategoria;
        private System.Windows.Forms.ComboBox cmbPriorytet;
        private System.Windows.Forms.Label lblPriorytet;
        private System.Windows.Forms.DateTimePicker dtpTermin;
        private System.Windows.Forms.Label lblTermin;
        private System.Windows.Forms.ComboBox cmbStatus;
        private System.Windows.Forms.Label lblStatus;
        private System.Windows.Forms.Button btnDodaj;
        private System.Windows.Forms.Button btnEdytuj;
        private System.Windows.Forms.Button btnUsun;
        private System.Windows.Forms.TextBox txtSzukaj;
        private System.Windows.Forms.Label lblSzukaj;
        private System.Windows.Forms.Button btnRaport;
        private System.Windows.Forms.Label lblError;
    }
}