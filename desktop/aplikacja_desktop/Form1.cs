using System;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace aplikacja_desktop
{
    // Definicja modelu danych z obsługą właściwości dopuszczających null
    public class Zadanie
    {
        public int Id { get; set; }
        public string Nazwa { get; set; } = string.Empty;
        public string Kategoria { get; set; } = string.Empty;
        public string Priorytet { get; set; } = string.Empty;
        public DateTime Termin { get; set; }
        public string Status { get; set; } = string.Empty; // Nowe / W trakcie / Wykonane
    }

    public partial class Form1 : Form
    {
        // Inicjalizacja pól, aby uniknąć ostrzeżeń CS8618
        private BindingList<Zadanie> listaZadan = new BindingList<Zadanie>();
        private BindingSource bindingSource = new BindingSource();
        private int nastepneId = 4;

        public Form1()
        {
            InitializeComponent();
            InicjalizujKomponentyWlasne();
            InicjalizujDane();
        }

        // Konfiguracja ComboBoxów (wartości słownikowe)
        private void InicjalizujKomponentyWlasne()
        {
            cmbPriorytet.Items.AddRange(new string[] { "Niski", "Średni", "Wysoki" });
            cmbStatus.Items.AddRange(new string[] { "Nowe", "W trakcie", "Wykonane" });
            
            cmbPriorytet.SelectedIndex = 1; // Średni
            cmbStatus.SelectedIndex = 0;    // Nowe
        }

        private void InicjalizujDane()
        {
            // Przykładowe dane początkowe dla zadań
            listaZadan = new BindingList<Zadanie>()
            {
                new Zadanie { Id = 1, Nazwa = "Zaimplementować CRUD", Kategoria = "Programowanie", Priorytet = "Wysoki", Termin = DateTime.Now.AddDays(2), Status = "W trakcie" },
                new Zadanie { Id = 2, Nazwa = "Przygotować raport końcowy", Kategoria = "Dokumentacja", Priorytet = "Średni", Termin = DateTime.Now.AddDays(5), Status = "Nowe" },
                new Zadanie { Id = 3, Nazwa = "Przetestować aplikację", Kategoria = "Testy", Priorytet = "Niski", Termin = DateTime.Now.AddDays(7), Status = "Wykonane" }
            };

            bindingSource.DataSource = listaZadan;
            dgvData.DataSource = bindingSource;

            // Konfiguracja kolumn DataGridView
            dgvData.Columns["Id"].Width = 40;
            dgvData.Columns["Nazwa"].Width = 150;
            dgvData.Columns["Kategoria"].Width = 100;
            dgvData.Columns["Priorytet"].Width = 80;
            dgvData.Columns["Termin"].Width = 110;
            dgvData.Columns["Status"].Width = 90;
            
            dgvData.Columns["Termin"].DefaultCellStyle.Format = "yyyy-MM-dd";
        }

        // WALIDACJA DANYCH
        private bool WalidujFormularz(out string nazwa, out string kategoria, out string priorytet, out DateTime termin, out string status)
        {
            nazwa = txtNazwa.Text.Trim();
            kategoria = txtKategoria.Text.Trim();
            priorytet = cmbPriorytet.SelectedItem?.ToString() ?? "Średni";
            termin = dtpTermin.Value;
            status = cmbStatus.SelectedItem?.ToString() ?? "Nowe";
            
            lblError.Text = "";

            if (string.IsNullOrEmpty(nazwa))
            {
                lblError.Text = "Błąd: Nazwa zadania nie może być pusta!";
                return false;
            }

            if (string.IsNullOrEmpty(kategoria))
            {
                lblError.Text = "Błąd: Kategoria nie może być pusta!";
                return false;
            }

            return true;
        }

        // CRUD: Odczyt (Zaznaczenie wiersza przenosi dane do pól edycji)
        private void dgvData_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvData.SelectedRows.Count > 0)
            {
                var zadanie = (Zadanie)dgvData.SelectedRows[0].DataBoundItem;
                txtId.Text = zadanie.Id.ToString();
                txtNazwa.Text = zadanie.Nazwa;
                txtKategoria.Text = zadanie.Kategoria;
                cmbPriorytet.SelectedItem = zadanie.Priorytet;
                dtpTermin.Value = zadanie.Termin;
                cmbStatus.SelectedItem = zadanie.Status;
            }
        }

        // CRUD: Dodawanie
        private void btnDodaj_Click(object sender, EventArgs e)
        {
            if (WalidujFormularz(out string nazwa, out string kategoria, out string priorytet, out DateTime termin, out string status))
            {
                // TUTAJ poprawiłem literówkę z "nowieZadanie" na "noweZadanie"
                var noweZadanie = new Zadanie 
                { 
                    Id = nastepneId++, 
                    Nazwa = nazwa, 
                    Kategoria = kategoria, 
                    Priorytet = priorytet, 
                    Termin = termin, 
                    Status = status 
                };
                listaZadan.Add(noweZadanie);
                WyczyscPola();
                MessageBox.Show("Zadanie dodane pomyślnie!", "Sukces", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        // CRUD: Edycja
        private void btnEdytuj_Click(object sender, EventArgs e)
        {
            if (dgvData.SelectedRows.Count > 0)
            {
                if (WalidujFormularz(out string nazwa, out string kategoria, out string priorytet, out DateTime termin, out string status))
                {
                    var zadanie = (Zadanie)dgvData.SelectedRows[0].DataBoundItem;
                    zadanie.Nazwa = nazwa;
                    zadanie.Kategoria = kategoria;
                    zadanie.Priorytet = priorytet;
                    zadanie.Termin = termin;
                    zadanie.Status = status;

                    listaZadan.ResetBindings(); // Odświeżenie widoku tabeli
                    MessageBox.Show("Zadanie zaktualizowane!", "Sukces", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            else
            {
                MessageBox.Show("Wybierz zadanie z tabeli do edycji.", "Uwaga", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        // CRUD: Usuwanie
        private void btnUsun_Click(object sender, EventArgs e)
        {
            if (dgvData.SelectedRows.Count > 0)
            {
                var zadanie = (Zadanie)dgvData.SelectedRows[0].DataBoundItem;

                var wynik = MessageBox.Show($"Czy na pewno chcesz usunąć zadanie: {zadanie.Nazwa}?", "Potwierdzenie", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (wynik == DialogResult.Yes)
                {
                    listaZadan.Remove(zadanie);
                    WyczyscPola();
                }
            }
            else
            {
                MessageBox.Show("Wybierz zadanie z tabeli do usunięcia.", "Uwaga", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        // WYSZUKIWANIE I FILTROWANIE dynamiczne po nazwie oraz kategorii
        private void txtSzukaj_TextChanged(object sender, EventArgs e)
        {
            string fraza = txtSzukaj.Text.ToLower();
            if (string.IsNullOrEmpty(fraza))
            {
                dgvData.DataSource = bindingSource;
            }
            else
            {
                var przefiltrowane = listaZadan.Where(z => 
                    z.Nazwa.ToLower().Contains(fraza) || 
                    z.Kategoria.ToLower().Contains(fraza)
                ).ToList();
                dgvData.DataSource = new BindingSource { DataSource = przefiltrowane };
            }
        }

        // GENEROWANIE RAPORTU CSV (Uwzględnia filtry w DataGridView)
        private void btnRaport_Click(object sender, EventArgs e)
        {
            using (SaveFileDialog sfd = new SaveFileDialog())
            {
                sfd.Filter = "Pliki CSV (*.csv)|*.csv";
                sfd.FileName = $"Raport_Zadan_{DateTime.Now:yyyyMMdd_HHmmss}.csv";

                if (sfd.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        StringBuilder sb = new StringBuilder();
                        sb.AppendLine("ID;Nazwa Zadania;Kategoria;Priorytet;Termin;Status");

                        foreach (DataGridViewRow row in dgvData.Rows)
                        {
                            var zad = (Zadanie)row.DataBoundItem;
                            if (zad != null)
                            {
                                sb.AppendLine($"{zad.Id};{zad.Nazwa};{zad.Kategoria};{zad.Priorytet};{zad.Termin:yyyy-MM-dd};{zad.Status}");
                            }
                        }

                        File.WriteAllText(sfd.FileName, sb.ToString(), Encoding.UTF8);
                        MessageBox.Show("Raport zadań został pomyślnie wygenerowany!", "Sukces", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
            txtKategoria.Clear();
            cmbPriorytet.SelectedIndex = 1;
            dtpTermin.Value = DateTime.Now;
            cmbStatus.SelectedIndex = 0;
            lblError.Text = "";
        }
    }
}