using System;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace aplikacja_desktop
{
    // Definicja modelu danych
    public class Produkt
    {
        public int Id { get; set; }
        public string Nazwa { get; set; }
        public decimal Cena { get; set; }
    }

    public partial class Form1 : Form
    {
        // BindingList pozwala na automatyczne odświeżanie tabeli DataGridView
        private BindingList<Produkt> listaProduktow;
        private BindingSource bindingSource;
        private int nastepneId = 4;

        public Form1()
        {
            InitializeComponent();
            InicjalizujDane();
        }

        private void InicjalizujDane()
        {
            // Przykładowe dane początkowe
            listaProduktow = new BindingList<Produkt>()
            {
                new Produkt { Id = 1, Nazwa = "Laptop", Cena = 3499.99m },
                new Produkt { Id = 2, Nazwa = "Mysz bezprzewodowa", Cena = 120.00m },
                new Produkt { Id = 3, Nazwa = "Monitor 4K", Cena = 1550.50m }
            };

            bindingSource = new BindingSource { DataSource = listaProduktow };
            dgvData.DataSource = bindingSource;

            // Konfiguracja kolumn i włączenie wbudowanego sortowania w DataGridView
            dgvData.Columns["Id"].Width = 50;
            dgvData.Columns["Nazwa"].Width = 240;
            dgvData.Columns["Cena"].Width = 120;
            dgvData.Columns["Cena"].DefaultCellStyle.Format = "C2"; // Format waluty
        }

        // WALIDACJA FORMULARZA
        private bool WalidujFormularz(out string nazwa, out decimal cena)
        {
            nazwa = txtNazwa.Text.Trim();
            cena = 0;
            lblError.Text = "";

            if (string.IsNullOrEmpty(nazwa))
            {
                lblError.Text = "Błąd: Nazwa nie może być pusta!";
                return false;
            }

            if (!decimal.TryParse(txtCena.Text, out cena) || cena <= 0)
            {
                lblError.Text = "Błąd: Cena musi być liczbą większą od 0!";
                return false;
            }

            return true;
        }

        // CRUD: Odczyt - przeniesienie danych z tabeli do pól tekstowych przy kliknięciu w wiersz
        private void dgvData_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvData.SelectedRows.Count > 0)
            {
                var produkt = (Produkt)dgvData.SelectedRows[0].DataBoundItem;
                txtId.Text = produkt.Id.ToString();
                txtNazwa.Text = produkt.Nazwa;
                txtCena.Text = produkt.Cena.ToString();
            }
        }

        // CRUD: Dodawanie
        private void btnDodaj_Click(object sender, EventArgs e)
        {
            if (WalidujFormularz(out string nazwa, out decimal cena))
            {
                var nowy = new Produkt { Id = nastepneId++, Nazwa = nazwa, Cena = cena };
                listaProduktow.Add(nowy);
                WyczyscPola();
                MessageBox.Show("Produkt dodany pomyślnie!", "Sukces", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        // CRUD: Edycja
        private void btnEdytuj_Click(object sender, EventArgs e)
        {
            if (dgvData.SelectedRows.Count > 0)
            {
                if (WalidujFormularz(out string nazwa, out decimal cena))
                {
                    var produkt = (Produkt)dgvData.SelectedRows[0].DataBoundItem;
                    produkt.Nazwa = nazwa;
                    produkt.Cena = cena;

                    listaProduktow.ResetBindings(); // Odświeżenie widoku tabeli
                    MessageBox.Show("Produkt zaktualizowany!", "Sukces", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            else
            {
                MessageBox.Show("Wybierz produkt z tabeli do edycji.", "Uwaga", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        // CRUD: Usuwanie
        private void btnUsun_Click(object sender, EventArgs e)
        {
            if (dgvData.SelectedRows.Count > 0)
            {
                var produkt = (Produkt)dgvData.SelectedRows[0].DataBoundItem;

                var wynik = MessageBox.Show($"Czy na pewno chcesz usunąć: {produkt.Nazwa}?", "Potwierdzenie", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (wynik == DialogResult.Yes)
                {
                    listaProduktow.Remove(produkt);
                    WyczyscPola();
                }
            }
            else
            {
                MessageBox.Show("Wybierz produkt z tabeli do usunięcia.", "Uwaga", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        // WYSZUKIWANIE / FILTROWANIE dynamiczne
        private void txtSzukaj_TextChanged(object sender, EventArgs e)
        {
            string fraza = txtSzukaj.Text.ToLower();
            if (string.IsNullOrEmpty(fraza))
            {
                dgvData.DataSource = bindingSource;
            }
            else
            {
                var przefiltrowane = listaProduktow.Where(p => p.Nazwa.ToLower().Contains(fraza)).ToList();
                dgvData.DataSource = new BindingSource { DataSource = przefiltrowane };
            }
        }

        // GENEROWANIE RAPORTU CSV
        private void btnRaport_Click(object sender, EventArgs e)
        {
            using (SaveFileDialog sfd = new SaveFileDialog())
            {
                sfd.Filter = "Pliki CSV (*.csv)|*.csv";
                sfd.FileName = $"Raport_Produktow_{DateTime.Now:yyyyMMdd_HHmmss}.csv";

                if (sfd.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        StringBuilder sb = new StringBuilder();
                        // Nagłówki kolumn
                        sb.AppendLine("ID;Nazwa Produktu;Cena");

                        // Zawartość aktualnie widoczna w DataGridView (uwzględnia filtry)
                        foreach (DataGridViewRow row in dgvData.Rows)
                        {
                            var prod = (Produkt)row.DataBoundItem;
                            sb.AppendLine($"{prod.Id};{prod.Nazwa};{prod.Cena}");
                        }

                        // Zapis z kodowaniem UTF8 dla zachowania polskich znaków
                        File.WriteAllText(sfd.FileName, sb.ToString(), Encoding.UTF8);
                        MessageBox.Show("Raport został pomyślnie wygenerowany!", "Sukces", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Błąd podczas generowania raportu: {ex.Message}", "Błąd", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private void WyczyscPola()
        {
            txtId.Clear();
            txtNazwa.Clear();
            txtCena.Clear();
            lblError.Text = "";
        }
    }
}