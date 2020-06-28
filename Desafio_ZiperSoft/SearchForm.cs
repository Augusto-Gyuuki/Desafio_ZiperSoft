using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using System.IO;
using Microsoft.Reporting.WinForms;

namespace Desafio_ZiperSoft
{
    public partial class SearchForm : Form
    {
        //funçao para enviar informaçoes do formulario de pesquisa para o formulario de cadastro
        public delegate void PassData(string nameData,
            string cepData,
            string addressData,
            string addressNumberData,
            string neighborhoodData,
            string cityData,
            string cpfCnpjData,
            string rgIeData,
            string emailData,
            string siteData,
            string telefoneData,
            string cellphoneData,
            MemoryStream photoData,
            string obsData,
            string typeData,
            int idData
        );
        ReportDataSource rs = new ReportDataSource();

        string connectionString = @"Server=localhost;Database=users;Uid=Augusto;Pwd=;";
        byte[] teste = new byte[0];

        public SearchForm()
        {
            InitializeComponent();
        }

        private void SearchBox_Leave(object sender, EventArgs e)
        {
            // placeholder
            if (SearchBox.Text == "Pesquise por CPF OU CNPJ OU Endereço OU Nome" || SearchBox.Text.Trim() == "")
            {
                SearchBox.Text = "Pesquise por CPF OU CNPJ OU Endereço OU Nome";
                SearchBox.ForeColor = Color.Gray;
            }


        }

        private void SearchBox_Enter(object sender, EventArgs e)
        {
            // placeholder

            
            if (SearchBox.Text == "Pesquise por CPF OU CNPJ OU Endereço OU Nome")
            {
                SearchBox.Text = "";
                SearchBox.ForeColor = Color.Black;
            }
        }
        //busca no banco de dados o texto do campo de pesquisa
        private void SearchBtn_Click(object sender, EventArgs e)
        {
            using (MySqlConnection mySqlCon = new MySqlConnection(connectionString))
            {
                mySqlCon.Open();
                if(SearchBox.Text.Trim() == "" || SearchBox.Text == "Pesquise por CPF OU CNPJ OU Endereço OU Nome")
                {
                    Search();
                }
                else
                {
                    MySqlDataAdapter mySqlDa = new MySqlDataAdapter("SearchByValue", mySqlCon);
                    mySqlDa.SelectCommand.CommandType = CommandType.StoredProcedure;
                    mySqlDa.SelectCommand.Parameters.AddWithValue("_SearchInput", SearchBox.Text);
                    DataTable DTuser = new DataTable();
                    mySqlDa.Fill(DTuser);
                    usersDataG.DataSource = DTuser;
                    usersDataG.Columns[0].Visible = false;
                }
            }
        }

        public void Search()
        {
            using (MySqlConnection mySqlCon = new MySqlConnection(connectionString))
            {
                mySqlCon.Open();
                MySqlDataAdapter mySqlDa = new MySqlDataAdapter("ViewAllUser", mySqlCon);
                mySqlDa.SelectCommand.CommandType = CommandType.StoredProcedure;
                DataTable DTuser = new DataTable();
                mySqlDa.Fill(DTuser);
                usersDataG.DataSource = DTuser;
                usersDataG.Columns[0].Visible = false;
                usersDataG.Columns[13].Visible = false;
                usersDataG.Columns[15].Visible = false;
            }
        }
        //busca todos os usuario no banco de dados
        private void SearchForm_Load(object sender, EventArgs e)
        {
            Search();
        }
        //recebe as informaçoes do usuario selecionado e passa todas as suas informçoes para o formulario de cadastro para atualizaçao ou deletar
        private void usersDataG_DoubleClick(object sender, EventArgs e)
        {
            if(usersDataG.CurrentRow.Index != -1)
            {
                var frm = new RegisterForm();
                frm.Location = this.Location;
                frm.StartPosition = FormStartPosition.Manual;
                frm.FormClosing += delegate { this.Show(); };
                PassData del = new PassData(frm.PassedData);
                byte[] img = (byte[])usersDataG.CurrentRow.Cells[13].Value;
                MemoryStream ms = new MemoryStream(img);

                del(usersDataG.CurrentRow.Cells[1].Value.ToString(),
                    usersDataG.CurrentRow.Cells[2].Value.ToString(),
                    usersDataG.CurrentRow.Cells[3].Value.ToString(),
                    usersDataG.CurrentRow.Cells[4].Value.ToString(),
                    usersDataG.CurrentRow.Cells[5].Value.ToString(),
                    usersDataG.CurrentRow.Cells[6].Value.ToString(),
                    usersDataG.CurrentRow.Cells[7].Value.ToString(),
                    usersDataG.CurrentRow.Cells[8].Value.ToString(),
                    usersDataG.CurrentRow.Cells[9].Value.ToString(),
                    usersDataG.CurrentRow.Cells[10].Value.ToString(),
                    usersDataG.CurrentRow.Cells[11].Value.ToString(),
                    usersDataG.CurrentRow.Cells[12].Value.ToString(),
                    ms,
                    usersDataG.CurrentRow.Cells[14].Value.ToString(),
                    usersDataG.CurrentRow.Cells[15].Value.ToString(),
                    Convert.ToInt32(usersDataG.CurrentRow.Cells[0].Value.ToString())
                    );
                frm.Show();
                this.Hide();
            }
        }
        //fecha o formulario de pesquisa e volta o relatorio de cadastro
        private void backBtn_Click(object sender, EventArgs e)
        {
            var frm = new RegisterForm();
            frm.Location = this.Location;
            frm.StartPosition = FormStartPosition.Manual;
            frm.FormClosing += delegate { this.Show(); };
            this.Close();
        }
        // editar informaçoes do usuario diretamente pelo datagrid
        private void usersDataG_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if(usersDataG.CurrentRow.Index != -1)
            {
                using (MySqlConnection mySqlCon = new MySqlConnection(connectionString))
                {
                    try
                    {
                        mySqlCon.Open();
                        DataGridViewRow userDGV = usersDataG.CurrentRow;
                        MySqlCommand MySqlCmd = new MySqlCommand("UserAddOrEdit", mySqlCon);
                        MySqlCmd.CommandType = CommandType.StoredProcedure;
                        MySqlCmd.Parameters.AddWithValue("_user_id", Convert.ToInt32(usersDataG.CurrentRow.Cells[0].Value.ToString()));
                        MySqlCmd.Parameters.AddWithValue("_user_name", usersDataG.CurrentRow.Cells[1].Value.ToString());
                        MySqlCmd.Parameters.AddWithValue("_user_cep", usersDataG.CurrentRow.Cells[2].Value.ToString());
                        MySqlCmd.Parameters.AddWithValue("_user_address", usersDataG.CurrentRow.Cells[3].Value.ToString());
                        MySqlCmd.Parameters.AddWithValue("_user_addressNumber", usersDataG.CurrentRow.Cells[4].Value.ToString());
                        MySqlCmd.Parameters.AddWithValue("_user_neighborhood", usersDataG.CurrentRow.Cells[5].Value.ToString());
                        MySqlCmd.Parameters.AddWithValue("_user_city", usersDataG.CurrentRow.Cells[6].Value.ToString());
                        MySqlCmd.Parameters.AddWithValue("_user_cpf_cnpj", usersDataG.CurrentRow.Cells[7].Value.ToString());
                        MySqlCmd.Parameters.AddWithValue("_user_rg_ie", usersDataG.CurrentRow.Cells[8].Value.ToString());
                        MySqlCmd.Parameters.AddWithValue("_user_email", usersDataG.CurrentRow.Cells[9].Value.ToString());
                        MySqlCmd.Parameters.AddWithValue("_user_site", usersDataG.CurrentRow.Cells[10].Value.ToString());
                        MySqlCmd.Parameters.AddWithValue("_user_telefone", usersDataG.CurrentRow.Cells[11].Value.ToString());
                        MySqlCmd.Parameters.AddWithValue("_user_cellphone", usersDataG.CurrentRow.Cells[12].Value.ToString());
                        MySqlCmd.Parameters.AddWithValue("_user_photo", usersDataG.CurrentRow.Cells[13].Value.ToString());
                        MySqlCmd.Parameters.AddWithValue("_user_obs", usersDataG.CurrentRow.Cells[14].Value.ToString());
                        MySqlCmd.Parameters.AddWithValue("_user_type", usersDataG.CurrentRow.Cells[15].Value.ToString());
                        MySqlCmd.ExecuteNonQuery();
                    }
                    catch
                    {
                        MessageBox.Show("Valor invalido");
                    }
                }
            }
        }
        // gerar o relatorio
        private void RelatorioBtn_Click(object sender, EventArgs e)
        {
            List<ReportInfo> lst = new List<ReportInfo>();
            lst.Clear();
            // ler todas as informaçoes do data grid para gerar o relatorio
            for (int i = 0; i < usersDataG.Rows.Count - 1; i++)
            {
                ReportInfo report = new ReportInfo
                {
                    Id = int.Parse(usersDataG.Rows[i].Cells[0].Value.ToString()),
                    Nome = usersDataG.Rows[i].Cells[1].Value.ToString(),
                    Cep = usersDataG.Rows[i].Cells[2].Value.ToString(),
                    Endereco = usersDataG.Rows[i].Cells[3].Value.ToString(),
                    Numero = int.Parse(usersDataG.Rows[i].Cells[4].Value.ToString()),
                    Bairro = usersDataG.Rows[i].Cells[5].Value.ToString(),
                    Cidade = usersDataG.Rows[i].Cells[6].Value.ToString(),
                    CPF_CNPJ = usersDataG.Rows[i].Cells[7].Value.ToString(),
                    RG_IE = usersDataG.Rows[i].Cells[8].Value.ToString(),
                    E_Mail = usersDataG.Rows[i].Cells[9].Value.ToString(),
                    Site = usersDataG.Rows[i].Cells[10].Value.ToString(),
                    Telefone = usersDataG.Rows[i].Cells[11].Value.ToString(),
                    Celular = usersDataG.Rows[i].Cells[12].Value.ToString(),
                    Observação = usersDataG.Rows[i].Cells[14].Value.ToString(),
                    Pessoa = usersDataG.Rows[i].Cells[15].Value.ToString(),

                };
                lst.Add(report);
                
            }
            rs.Name = "DataSet1";
            rs.Value = lst;
            //gerar o formulario de relatorio e escrever todas as informaçoes 
            var frm = new ReportForm();
            frm.reportViewer1.LocalReport.DataSources.Clear();
            frm.reportViewer1.LocalReport.DataSources.Add(rs);
            frm.ShowDialog();
        }
        //criar o modelo do relatorio de todos os usuario no banco de dados
        public class ReportInfo
        {
            public int Id { get; set; }
            public string Nome { get; set; }
            public string Cep { get; set; }
            public string Endereco { get; set; }
            public int Numero { get; set; }
            public string Bairro { get; set; }
            public string Cidade { get; set; }
            public string CPF_CNPJ { get; set; }
            public string RG_IE { get; set; }
            public string E_Mail { get; set; }
            public string Site { get; set; }
            public string Telefone { get; set; }
            public string Celular { get; set; }
            public string Observação { get; set; }
            public string Pessoa { get; set; }
        }
    }
}
