using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using Newtonsoft;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;
using System.IO;
using System.Net;
using System.Net.Mail;

namespace Desafio_ZiperSoft
{
    public partial class RegisterForm : Form
    {
    //recebe as informaçoes do usuario do formulario de pesquisa e as altera para ediçao ou relatorio
        public void PassedData(string nameData,
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
            string obsData,
            string typeData,
            int idData
            )
        {
            NameBox.Text = nameData;
            CepBox.Text = cepData;
            AddressBox.Text = addressData;
            AddressNumberBox.Text = addressNumberData;
            NeighborhoodBox.Text = neighborhoodData;
            CityBox.Text = cityData;
            CpfCnpjBox.Text = cpfCnpjData;
            RgIeBox.Text = rgIeData;
            EmailBox.Text = emailData;
            SiteBox.Text = siteData;
            TelefoneBox.Text = telefoneData;
            CellphoneBox.Text = cellphoneData;
            ObsBox.Text = obsData;
            userType = typeData;
            user_id = idData;
            btnSubmit.Text = "Atualizar";
            btnSearch.Text = "Excluir";
            ClearBtn.Text = "Relatorio";
            RgIeBox.ForeColor = Color.Black ;

            if (ObsBox.Text.ToLower().Trim().Equals("caso necessario") || ObsBox.Text.Trim().Equals(""))
            {
                ObsBox.Text = "caso necessario";
                ObsBox.ForeColor = Color.Gray;
            }

        }
        //conecta ao banco de dados
        string connectionString = @"Server=localhost;Database=users;Uid=Augusto;Pwd=;";
        //valor padrao do usuario
        int user_id = 0;
        // uf do usuario cadastrado
        string uF;
        // imagem do usuario
        byte[] img;
        // tipo do usuario
        string userType;

        public RegisterForm()
        {
            InitializeComponent();
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            //muda a imagem 
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Filter = "jpg files(.*jpg)|*.jpg| PNG files(.*png)|*.png| All Files(*.*)|*.*";

            if (dialog.ShowDialog() == DialogResult.OK)
            {
                pictureBox1.Image = Image.FromFile(dialog.FileName);
            }

        }

        private void btnSubmit_Click(object sender, EventArgs e)
        {
            if (
                // caso os campos nao forem preenchidos corretamente uma mensagem avisa o usuario
                NameBox.Text == "" ||
                CepBox.Text == "" ||
                AddressBox.Text == "" ||
                AddressNumberBox.Text == "" ||
                NeighborhoodBox.Text == "" ||
                CityBox.Text == "" ||
                RgIeBox.Text == "" ||
                CpfCnpjBox.Text == "" ||
                SiteBox.Text == "" ||
                EmailBox.Text == "" ||
                TelefoneBox.Text == "" ||
                CellphoneBox.Text == "" ||
                NameBox.BackColor == Color.IndianRed ||
                CepBox.BackColor == Color.IndianRed ||
                RgIeBox.BackColor == Color.IndianRed ||
                CpfCnpjBox.BackColor == Color.IndianRed ||
                EmailBox.BackColor == Color.IndianRed ||
                TelefoneBox.BackColor == Color.IndianRed ||
                CellphoneBox.BackColor == Color.IndianRed)
            {
                MessageBox.Show("Preencha todos os campos corretamente");
                if(AddressNumberBox.Text.Trim() == "")
                {
                    AddressNumberBox.BackColor = Color.IndianRed;
                }
            }
            else
            {
                if (ObsBox.Text == "caso necessario")
                {
                    //salva o usuario passando o tipo da pessoa
                    ObsBox.Text = "";
                    sqlSave(userType);
                }else
                {
                    sqlSave(userType);
                }
            }
        }

        private void sqlSave(string userType)
        {
            //altera o formato da imagem para salvar no banco de dados
            MemoryStream ms = new MemoryStream();
            pictureBox1.Image.Save(ms, pictureBox1.Image.RawFormat);
            img = ms.ToArray();
            using (MySqlConnection MySqlCon = new MySqlConnection(connectionString))
            {
                // salva ou atualiza o usuario no banco de dados
                MySqlCon.Open();
                MySqlCommand MySqlCmd = new MySqlCommand("UserAddOrEdit", MySqlCon);
                MySqlCmd.CommandType = CommandType.StoredProcedure;
                MySqlCmd.Parameters.AddWithValue("_user_id", user_id);
                MySqlCmd.Parameters.AddWithValue("_user_name", NameBox.Text);
                MySqlCmd.Parameters.AddWithValue("_user_cep", CepBox.Text);
                MySqlCmd.Parameters.AddWithValue("_user_address", AddressBox.Text);
                MySqlCmd.Parameters.AddWithValue("_user_addressNumber", AddressNumberBox.Text);
                MySqlCmd.Parameters.AddWithValue("_user_neighborhood", NeighborhoodBox.Text);
                MySqlCmd.Parameters.AddWithValue("_user_city", CityBox.Text);
                MySqlCmd.Parameters.AddWithValue("_user_cpf_cnpj", CpfCnpjBox.Text);
                MySqlCmd.Parameters.AddWithValue("_user_rg_ie", RgIeBox.Text);
                MySqlCmd.Parameters.AddWithValue("_user_email", EmailBox.Text);
                MySqlCmd.Parameters.AddWithValue("_user_site", SiteBox.Text);
                MySqlCmd.Parameters.AddWithValue("_user_telefone", TelefoneBox.Text);
                MySqlCmd.Parameters.AddWithValue("_user_cellphone", CellphoneBox.Text);
                MySqlCmd.Parameters.AddWithValue("_user_photo", img);
                MySqlCmd.Parameters.AddWithValue("_user_obs", ObsBox.Text);
                MySqlCmd.Parameters.AddWithValue("_user_type", userType);
                MySqlCmd.ExecuteNonQuery();
                
                if (user_id == 0)
                {
                    // se o id do usuario for 0 ele esta sendo adicionado ao banco de dados
                    MessageBox.Show("Cadastro Concluido com Sucesso");
                }
                else
                {
                    // se o id do usuario for diferente de 0 ele esta sendo atualizado no banco de dados
                    MessageBox.Show("Cadastro atualizado com Sucesso");
                    
                    this.Close();
                }
                Clear();
            }
        }

        void Clear()
        {
            //define o valor padrao para todos os campos
            NameBox.Text =
            CepBox.Text =
            AddressBox.Text = 
            AddressNumberBox.Text = 
            NeighborhoodBox.Text = 
            CityBox.Text = 
            RgIeBox.Text = 
            CpfCnpjBox.Text = 
            SiteBox.Text = 
            EmailBox.Text = 
            TelefoneBox.Text = 
            CellphoneBox.Text = 
            userType = 
            uF = "";
            user_id = 0;
            ObsBox.Text = "caso necessario";
        }

        private void ObsBox_Leave(object sender, EventArgs e)
        {
            // placeholder
            String obsText = ObsBox.Text;
            if (obsText.ToLower().Trim().Equals("caso necessario") || obsText.Trim().Equals(""))
            {
                ObsBox.Text = "caso necessario";
                ObsBox.ForeColor = Color.Gray;
            }
        }

        private void ObsBox_Enter(object sender, EventArgs e)
        {
            // placeholder

            String obsText = ObsBox.Text;
            if (obsText.ToLower().Trim().Equals("caso necessario"))
            {
                ObsBox.Text = "";
                ObsBox.ForeColor = Color.Black;
            }
        }

        // validação do CPF
        public static bool Cpf(string cpf)
        {
            int[] multiplicador1 = new int[9] { 10, 9, 8, 7, 6, 5, 4, 3, 2 };
            int[] multiplicador2 = new int[10] { 11, 10, 9, 8, 7, 6, 5, 4, 3, 2 };

            cpf = cpf.Trim().Replace(".", "").Replace("-", "");
            if (cpf.Length != 11)
                return false;

            for (int j = 0; j < 10; j++)
                if (j.ToString().PadLeft(11, char.Parse(j.ToString())) == cpf)
                    return false;

            string tempCpf = cpf.Substring(0, 9);
            int soma = 0;

            for (int i = 0; i < 9; i++)
                soma += int.Parse(tempCpf[i].ToString()) * multiplicador1[i];

            int resto = soma % 11;
            if (resto < 2)
                resto = 0;
            else
                resto = 11 - resto;

            string digito = resto.ToString();
            tempCpf += digito;
            soma = 0;
            for (int i = 0; i < 10; i++)
                soma += int.Parse(tempCpf[i].ToString()) * multiplicador2[i];

            resto = soma % 11;
            if (resto < 2)
                resto = 0;
            else
                resto = 11 - resto;

            digito += resto.ToString();
            return cpf.EndsWith(digito);
        }

        // validação do CNPJ
        public static bool Cnpj(string cnpj)
        {
            int[] multiplicador1 = new int[12] { 5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2 };
            int[] multiplicador2 = new int[13] { 6, 5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2 };
            int soma;
            int resto;
            string digito;
            string tempCnpj;
            cnpj = cnpj.Trim();
            cnpj = cnpj.Replace(".", "").Replace("-", "").Replace("/", "");
            if (cnpj.Length != 14)
                return false;
            tempCnpj = cnpj.Substring(0, 12);
            soma = 0;
            for (int i = 0; i < 12; i++)
                soma += int.Parse(tempCnpj[i].ToString()) * multiplicador1[i];
            resto = (soma % 11);
            if (resto < 2)
                resto = 0;
            else
                resto = 11 - resto;
            digito = resto.ToString();
            tempCnpj = tempCnpj + digito;
            soma = 0;
            for (int i = 0; i < 13; i++)
                soma += int.Parse(tempCnpj[i].ToString()) * multiplicador2[i];
            resto = (soma % 11);
            if (resto < 2)
                resto = 0;
            else
                resto = 11 - resto;
            digito = digito + resto.ToString();
            return cnpj.EndsWith(digito);
        }

        private async void CepBox_Leave(object sender, EventArgs e)
        {
            if(CepBox.Text.Length == 8)
            {
                //faz uma requisição a uma api para auto preencher os campos relacionados ao endereço
                CepBox.BackColor = Color.White;
                var client = new HttpClient();
                String cep = CepBox.Text;
                client.BaseAddress = new Uri("https://viacep.com.br/ws/");
                HttpResponseMessage response = await client.GetAsync(cep + "/json/");
                string result = await response.Content.ReadAsStringAsync();

                cepNumber Number = new cepNumber();

                //converte a resposta da api de JSON para Objeto c#
                Newtonsoft.Json.JsonConvert.PopulateObject(result, Number);
                AddressBox.Text = Number.Logradouro;
                NeighborhoodBox.Text = Number.Bairro;
                CityBox.Text = Number.Localidade + "-" + Number.Uf;
                uF = Number.Uf;
                if (RgIeBox.Text == "Preencha o campo Cep Primeiro" || RgIeBox.Text.Trim() != "")
                {
                    RgIeBox.Text = "";
                    RgIeBox.ForeColor = Color.Black;
                }
            }
            else
            {
                CepBox.BackColor = Color.IndianRed;
            }
        }
        
        private void CpfCnpjBox_Leave(object sender, EventArgs e)
        {
            //valida o cpf ou cnpj e define se a pessoa é fisica ou juridica
            if (Cpf(CpfCnpjBox.Text))
            {
                CpfCnpjBox.BackColor = Color.White;
                userType = "F";
            }
            else if (Cnpj(CpfCnpjBox.Text))
            {
                CpfCnpjBox.BackColor = Color.White;
                userType = "J";
            }
            else
            {
                CpfCnpjBox.BackColor = Color.IndianRed;
            }
        }

        public static bool fncValida_Inscricao_Estadual(String Ds_UF, String Ds_Inscricao_Estadual)
        {

            if (Ds_UF == "" || Ds_Inscricao_Estadual == "")
                return false;

            if (Ds_UF.Length != 2)
                return false;

            if (Ds_Inscricao_Estadual.Length == 0)
                return false;

            var pUF = Ds_UF;
            var strOrigem = Ds_Inscricao_Estadual.Trim().Replace(".", "").Replace("-", "").Replace("/", "");

            if ((strOrigem.Trim().ToUpper() == "ISENTO"))
                return true;


            try
            {

                switch (pUF.ToUpper())
                {

                    case "AC":
                        return fncValida_Inscricao_Estadual_AC(strOrigem);

                    case "AL":
                        return fncValida_Inscricao_Estadual_AL(strOrigem);

                    case "AM":
                        return fncValida_Inscricao_Estadual_AM(strOrigem);

                    case "AP":
                        return fncValida_Inscricao_Estadual_AP(strOrigem);

                    case "BA":
                        return fncValida_Inscricao_Estadual_BA(strOrigem);

                    case "CE":
                        return fncValida_Inscricao_Estadual_CE(strOrigem);

                    case "DF":
                        return fncValida_Inscricao_Estadual_DF(strOrigem);

                    case "ES":
                        return fncValida_Inscricao_Estadual_ES(strOrigem);

                    case "GO":
                        return fncValida_Inscricao_Estadual_GO(strOrigem);

                    case "MA":
                        return fncValida_Inscricao_Estadual_MA(strOrigem);

                    case "MT":
                        return fncValida_Inscricao_Estadual_MT(strOrigem);

                    case "MS":
                        return fncValida_Inscricao_Estadual_MS(strOrigem);

                    case "MG":
                        return fncValida_Inscricao_Estadual_MG(strOrigem);

                    case "PA":
                        return fncValida_Inscricao_Estadual_PA(strOrigem);

                    case "PB":
                        return fncValida_Inscricao_Estadual_PB(strOrigem);

                    case "PE":
                        return fncValida_Inscricao_Estadual_PE(strOrigem);

                    case "PI":
                        return fncValida_Inscricao_Estadual_PI(strOrigem);

                    case "PR":
                        return fncValida_Inscricao_Estadual_PR(strOrigem);

                    case "RJ":
                        return fncValida_Inscricao_Estadual_RJ(strOrigem);

                    case "RN":
                        return fncValida_Inscricao_Estadual_RN(strOrigem);

                    case "RO":
                        return fncValida_Inscricao_Estadual_RO(strOrigem);

                    case "RR":
                        return fncValida_Inscricao_Estadual_RR(strOrigem);

                    case "RS":
                        return fncValida_Inscricao_Estadual_RS(strOrigem);

                    case "SC":
                        return fncValida_Inscricao_Estadual_SC(strOrigem);

                    case "SE":
                        return fncValida_Inscricao_Estadual_SE(strOrigem);

                    case "SP":
                        return fncValida_Inscricao_Estadual_SP(strOrigem);

                    case "TO":
                        return fncValida_Inscricao_Estadual_TO(strOrigem);

                    default:
                        return false;

                }

            }
            catch (Exception)
            {
                return false;
            }

        }
        
        public static bool fncValida_Inscricao_Estadual_AC(string Ds_Inscricao_Estadual)
        {

            var strOrigem = Ds_Inscricao_Estadual;
            if (strOrigem.Length != 13 || strOrigem.Substring(0, 2) != "01")
                return false;

            var strBase = strOrigem.Trim();

            if (strBase.Substring(0, 2) != "01") return false;

            var intSoma = 0;
            var intPeso = 4;
            var intValor = 0;

            for (var intPos = 1; (intPos <= 11); intPos++)
            {

                intValor = int.Parse(strBase.Substring((intPos - 1), 1));

                if (intPeso == 1) intPeso = 9;

                intSoma += intValor * intPeso;

                intPeso--;
            }

            var intResto = (intSoma % 11);

            intSoma = 0;
            strBase = (strOrigem.Trim() + "000000000000").Substring(0, 12);
            intPeso = 5;

            for (var intPos = 1; (intPos <= 12); intPos++)
            {
                intValor = int.Parse(strBase.Substring((intPos - 1), 1));

                if (intPeso == 1) intPeso = 9;

                intSoma += intValor * intPeso;
                intPeso--;
            }

            intResto = (intSoma % 11);
            var strDigito2 = ((intResto < 2) ? "0" : Convert.ToString((11 - intResto))).Substring((((intResto < 2) ? "0" : Convert.ToString((11 - intResto))).Length - 1));

            var strBase2 = (strBase.Substring(0, 12) + strDigito2);

            return (strBase2 == strOrigem);
        }
        public static bool fncValida_Inscricao_Estadual_AL(string Ds_Inscricao_Estadual)
        {

            var strOrigem = Ds_Inscricao_Estadual;
            if (strOrigem.Length != 9 || strOrigem.Substring(0, 2) != "24")
                return false;

            var strBase = (strOrigem.Trim() + "000000000").Substring(0, 9);

            if ((strBase.Substring(0, 2) != "24")) return false;

            var intSoma = 0;
            var intPeso = 9;

            for (var intPos = 1; (intPos <= 8); intPos++)
            {

                var intValor = int.Parse(strBase.Substring((intPos - 1), 1));

                intSoma += intValor * intPeso;
                intPeso--;
            }

            intSoma = (intSoma * 10);
            var intResto = (intSoma % 11);

            var strDigito1 = ((intResto == 10) ? "0" : Convert.ToString(intResto)).Substring((((intResto == 10) ? "0" : Convert.ToString(intResto)).Length - 1));

            var strBase2 = (strBase.Substring(0, 8) + strDigito1);

            return strBase2 == strOrigem;
        }
        public static bool fncValida_Inscricao_Estadual_AM(string Ds_Inscricao_Estadual)
        {

            var strOrigem = Ds_Inscricao_Estadual;
            if (strOrigem.Length != 9)
                return false;

            var strBase = (strOrigem.Trim() + "000000000").Substring(0, 9);
            var intSoma = 0;
            var intPeso = 9;

            for (var intPos = 1; (intPos <= 8); intPos++)
            {
                var intValor = int.Parse(strBase.Substring((intPos - 1), 1));

                intSoma += intValor * intPeso;
                intPeso--;
            }

            var intResto = (intSoma % 11);
            var strDigito1 = intSoma < 11 ? (11 - intSoma).ToString() : ((intResto < 2) ? "0" : Convert.ToString((11 - intResto))).Substring((((intResto < 2) ? "0" : Convert.ToString((11 - intResto))).Length - 1));
            var strBase2 = (strBase.Substring(0, 8) + strDigito1);

            return (strBase2 == strOrigem);

        }
        public static bool fncValida_Inscricao_Estadual_AP(string Ds_Inscricao_Estadual)
        {

            var strOrigem = Ds_Inscricao_Estadual;
            if (strOrigem.Length != 9)
                return false;

            var strBase = (strOrigem.Trim() + "000000000").Substring(0, 9);
            var intPeso = 9;

            if (strBase.Substring(0, 2) != "03") return false;

            strBase = (strOrigem.Trim() + "000000000").Substring(0, 9);
            var intSoma = 0;

            int intValor;
            for (var intPos = 1; (intPos <= 8); intPos++)
            {
                intValor = int.Parse(strBase.Substring((intPos - 1), 1));

                intSoma += intValor * intPeso;
                intPeso--;
            }

            var intResto = (intSoma % 11);
            intValor = (11 - intResto);

            var strDigito1 = Convert.ToString(intValor).Substring((Convert.ToString(intValor).Length - 1));
            var strBase2 = (strBase.Substring(0, 8) + strDigito1);

            return strBase2 == strOrigem;
        }
        public static bool fncValida_Inscricao_Estadual_BA(string Ds_Inscricao_Estadual)
        {

            var strOrigem = Ds_Inscricao_Estadual;

            if (strOrigem.Length != 9 && strOrigem.Length != 8)
                return false;

            var strBase = "";

            switch (strOrigem.Length)
            {
                case 8:
                    strBase = (strOrigem.Trim() + "00000000").Substring(0, 8);
                    break;
                case 9:
                    strBase = (strOrigem.Trim() + "00000000").Substring(0, 9);
                    break;
            }

            var intSoma = 0;
            int intValor;
            var intPeso = 0;
            int intResto;
            var strDigito1 = "";
            var strDigito2 = "";
            var strBase2 = "";


            #region Validação 8 dígitos
            if (strBase.Length == 8)
            {

                if ((("0123458".IndexOf(strBase.Substring(0, 1), 0, StringComparison.OrdinalIgnoreCase) + 1) > 0))
                {

                    for (var intPos = 1; (intPos <= 6); intPos++)
                    {
                        intValor = int.Parse(strBase.Substring((intPos - 1), 1));

                        if (intPos == 1) intPeso = 7;

                        intSoma += intValor * intPeso;
                        intPeso--;
                    }


                    intResto = (intSoma % 10);
                    strDigito2 = ((intResto == 0) ? "0" : Convert.ToString((10 - intResto))).Substring((((intResto == 0) ? "0" : Convert.ToString((10 - intResto))).Length - 1));


                    strBase2 = strBase.Substring(0, 7) + strDigito2;

                    if (strBase2 == strOrigem)
                    {

                        intSoma = 0;
                        intPeso = 0;

                        for (var intPos = 1; (intPos <= 7); intPos++)
                        {
                            intValor = int.Parse(strBase.Substring((intPos - 1), 1));

                            if (intPos == 7)
                                intValor = int.Parse(strBase.Substring((intPos), 1));

                            if (intPos == 1) intPeso = 8;

                            intSoma += intValor * intPeso;
                            intPeso--;
                        }


                        intResto = (intSoma % 10);
                        strDigito1 = ((intResto == 0) ? "0" : Convert.ToString((10 - intResto))).Substring((((intResto == 0) ? "0" : Convert.ToString((10 - intResto))).Length - 1));

                        strBase2 = (strBase.Substring(0, 6) + strDigito1 + strDigito2);

                        return strBase2 == strOrigem;

                    }

                    return false;


                }


                if ((("679".IndexOf(strBase.Substring(0, 1), 0, StringComparison.OrdinalIgnoreCase) + 1) > 0))
                {

                    intSoma = 0;

                    for (var intPos = 1; (intPos <= 6); intPos++)
                    {
                        intValor = int.Parse(strBase.Substring((intPos - 1), 1));

                        if (intPos == 1) intPeso = 7;

                        intSoma += intValor * intPeso;
                        intPeso--;
                    }


                    intResto = (intSoma % 11);
                    strDigito2 = ((intResto == 0) ? "0" : Convert.ToString((11 - intResto))).Substring((((intResto == 0) ? "0" : Convert.ToString((11 - intResto))).Length - 1));


                    strBase2 = strBase.Substring(0, 7) + strDigito2;

                    if (strBase2 == strOrigem)
                    {

                        intSoma = 0;
                        intPeso = 0;

                        for (var intPos = 1; (intPos <= 7); intPos++)
                        {
                            intValor = int.Parse(strBase.Substring((intPos - 1), 1));

                            if (intPos == 7)
                                intValor = int.Parse(strBase.Substring((intPos), 1));

                            if (intPos == 1) intPeso = 8;

                            intSoma += intValor * intPeso;
                            intPeso--;
                        }


                        intResto = (intSoma % 11);
                        strDigito1 = ((intResto == 0) ? "0" : Convert.ToString((11 - intResto))).Substring((((intResto == 0) ? "0" : Convert.ToString((11 - intResto))).Length - 1));

                        strBase2 = (strBase.Substring(0, 6) + strDigito1 + strDigito2);

                        return strBase2 == strOrigem;

                    }

                    return false;

                }

            }
            #endregion


            #region Validação 9 dígitos
            if (strBase.Length == 9)
            {

                var modulo = (("0123458".IndexOf(strBase.Substring(1, 1), 0, StringComparison.OrdinalIgnoreCase) + 1) > 0) ? 10 : 11;


                intSoma = 0;


                for (var intPos = 1; (intPos <= 7); intPos++)
                {
                    intValor = int.Parse(strBase.Substring((intPos - 1), 1));

                    if (intPos == 1) intPeso = 8;

                    intSoma += intValor * intPeso;
                    intPeso--;
                }

                intResto = (intSoma % modulo);

                if (modulo == 11)
                    strDigito2 = ((intResto == 0 || intResto == 1) ? "0" : Convert.ToString((modulo - intResto))).Substring((((intResto == 0 || intResto == 1) ? "0" : Convert.ToString((modulo - intResto))).Length - 1));
                else
                    strDigito2 = ((intResto == 0) ? "0" : Convert.ToString((modulo - intResto))).Substring((((intResto == 0) ? "0" : Convert.ToString((modulo - intResto))).Length - 1));


                strBase2 = strBase.Substring(0, 8) + strDigito2;

                if (strBase2 == strOrigem)
                {

                    intSoma = 0;
                    intPeso = 0;

                    for (var intPos = 1; (intPos <= 8); intPos++)
                    {
                        intValor = int.Parse(strBase.Substring((intPos - 1), 1));

                        if (intPos == 8)
                            intValor = int.Parse(strBase.Substring((intPos), 1));

                        if (intPos == 1) intPeso = 9;

                        intSoma += intValor * intPeso;
                        intPeso--;
                    }


                    intResto = (intSoma % modulo);

                    if (modulo == 11)
                        strDigito1 = ((intResto == 0 || intResto == 1) ? "0" : Convert.ToString((modulo - intResto))).Substring((((intResto == 0 || intResto == 1) ? "0" : Convert.ToString((modulo - intResto))).Length - 1));
                    else
                        strDigito1 = ((intResto == 0) ? "0" : Convert.ToString((modulo - intResto))).Substring((((intResto == 0) ? "0" : Convert.ToString((modulo - intResto))).Length - 1));


                    strBase2 = (strBase.Substring(0, 7) + strDigito1 + strDigito2);

                    return strBase2 == strOrigem;

                }

                return false;


            }
            #endregion

            return false;

        }
        public static bool fncValida_Inscricao_Estadual_CE(string Ds_Inscricao_Estadual)
        {

            var strOrigem = Ds_Inscricao_Estadual;
            if (strOrigem.Length > 9)
                return false;

            while (strOrigem.Length <= 8)
                strOrigem = "0" + strOrigem;

            var strBase = (strOrigem.Trim() + "000000000").Substring(0, 9);
            var intSoma = 0;
            var intValor = 0;

            for (var intPos = 1; (intPos <= 8); intPos++)
            {
                intValor = int.Parse(strBase.Substring((intPos - 1), 1));
                intValor = (intValor * (10 - intPos));
                intSoma = (intSoma + intValor);
            }

            var intResto = (intSoma % 11);
            intValor = (11 - intResto);

            if ((intValor > 9))
                intValor = 0;

            var strDigito1 = Convert.ToString(intValor).Substring((Convert.ToString(intValor).Length - 1));
            var strBase2 = (strBase.Substring(0, 8) + strDigito1);

            return strBase2 == strOrigem;

        }
        public static bool fncValida_Inscricao_Estadual_DF(string Ds_Inscricao_Estadual)
        {

            var strOrigem = Ds_Inscricao_Estadual;
            if (strOrigem.Length != 13 || strOrigem.Substring(0, 3) != "073")
                return false;

            var strBase = (strOrigem.Trim() + "0000000000000").Substring(0, 13);

            var intSoma = 0;
            var intPeso = 2;
            var intValor = 0;

            for (var intPos = 11; (intPos >= 1); intPos = (intPos + -1))
            {
                intValor = int.Parse(strBase.Substring((intPos - 1), 1));
                intValor = (intValor * intPeso);
                intSoma = (intSoma + intValor);
                intPeso = (intPeso + 1);

                if ((intPeso > 9))
                    intPeso = 2;
            }

            var intResto = (intSoma % 11);
            var strDigito1 = ((intResto < 2) ? "0" : Convert.ToString((11 - intResto))).Substring((((intResto < 2) ? "0" : Convert.ToString((11 - intResto))).Length - 1));
            var strBase2 = (strBase.Substring(0, 11) + strDigito1);

            intSoma = 0;
            intPeso = 2;

            for (var intPos = 12; (intPos >= 1); intPos = (intPos + -1))
            {
                intValor = int.Parse(strBase.Substring((intPos - 1), 1));
                intValor = (intValor * intPeso);
                intSoma = (intSoma + intValor);
                intPeso = (intPeso + 1);

                if ((intPeso > 9))
                    intPeso = 2;
            }

            intResto = (intSoma % 11);
            var strDigito2 = ((intResto < 2) ? "0" : Convert.ToString((11 - intResto))).Substring((((intResto < 2) ? "0" : Convert.ToString((11 - intResto))).Length - 1));
            strBase2 = (strBase.Substring(0, 12) + strDigito2);

            return strBase2 == strOrigem;
        }
        public static bool fncValida_Inscricao_Estadual_ES(string Ds_Inscricao_Estadual)
        {

            var strOrigem = Ds_Inscricao_Estadual;
            if (strOrigem.Length != 9)
                return false;

            var strBase = strOrigem.Trim();
            var intSoma = 0;

            for (var intPos = 1; (intPos <= 8); intPos++)
            {
                var intValor = int.Parse(strBase.Substring((intPos - 1), 1));
                intValor = (intValor * (10 - intPos));
                intSoma = (intSoma + intValor);
            }

            var intResto = (intSoma % 11);
            var strDigito1 = ((intResto < 2) ? "0" : Convert.ToString((11 - intResto))).Substring((((intResto < 2) ? "0" : Convert.ToString((11 - intResto))).Length - 1));
            var strBase2 = (strBase.Substring(0, 8) + strDigito1);

            return strBase2 == strOrigem;

        }
        public static bool fncValida_Inscricao_Estadual_GO(string Ds_Inscricao_Estadual)
        {

            var strOrigem = Ds_Inscricao_Estadual;
            if (strOrigem.Length != 9)
                return false;

            var strBase = (strOrigem.Trim() + "000000000").Substring(0, 9);

            if ((("10,11,15".IndexOf(strBase.Substring(0, 2), 0, StringComparison.OrdinalIgnoreCase) + 1) <= 0))
                return false;

            var intSoma = 0;
            var strDigito1 = "";

            for (var intPos = 1; (intPos <= 8); intPos++)
            {
                var intValor = int.Parse(strBase.Substring((intPos - 1), 1));
                intValor = (intValor * (10 - intPos));
                intSoma = (intSoma + intValor);
            }

            var intResto = (intSoma % 11);

            switch (intResto)
            {
                case 0:
                    strDigito1 = "0";
                    break;

                case 1:
                    var intNumero = int.Parse(strBase.Substring(0, 8));
                    strDigito1 = (((intNumero >= 10103105) && (intNumero <= 10119997)) ? "1" : "0").Substring(((((intNumero >= 10103105) && (intNumero <= 10119997)) ? "1" : "0").Length - 1));
                    break;

                default:
                    strDigito1 = Convert.ToString((11 - intResto)).Substring((Convert.ToString((11 - intResto)).Length - 1));
                    break;
            }

            var strBase2 = (strBase.Substring(0, 8) + strDigito1);

            return strBase2 == strOrigem;
        }
        public static bool fncValida_Inscricao_Estadual_MA(string Ds_Inscricao_Estadual)
        {

            var strOrigem = Ds_Inscricao_Estadual;
            if (strOrigem.Length != 9 || strOrigem.Substring(0, 2) != "12")
                return false;

            var strBase = (strOrigem.Trim() + "000000000").Substring(0, 9);
            var intSoma = 0;

            for (var intPos = 1; (intPos <= 8); intPos++)
            {
                var intValor = int.Parse(strBase.Substring((intPos - 1), 1));
                intValor = (intValor * (10 - intPos));
                intSoma = (intSoma + intValor);
            }

            var intResto = (intSoma % 11);
            var strDigito1 = ((intResto < 2) ? "0" : Convert.ToString((11 - intResto))).Substring((((intResto < 2) ? "0" : Convert.ToString((11 - intResto))).Length - 1));
            var strBase2 = (strBase.Substring(0, 8) + strDigito1);

            return strBase2 == strOrigem;

        }
        public static bool fncValida_Inscricao_Estadual_MT(string Ds_Inscricao_Estadual)
        {

            var strOrigem = Ds_Inscricao_Estadual;
            if (strOrigem.Length < 9)
                return false;

            while (strOrigem.Length <= 11)
                strOrigem = "0" + strOrigem;

            var strBase = (strOrigem.Trim() + "0000000000").Substring(0, 10);
            var intSoma = 0;
            var intPeso = 2;

            for (var intPos = 10; intPos >= 1; intPos = (intPos + -1))
            {
                var intValor = int.Parse(strBase.Substring((intPos - 1), 1));
                intValor = (intValor * intPeso);
                intSoma = (intSoma + intValor);
                intPeso = (intPeso + 1);

                if ((intPeso > 9))
                    intPeso = 2;
            }

            var intResto = (intSoma % 11);
            var strDigito1 = ((intResto < 2) ? "0" : Convert.ToString((11 - intResto))).Substring((((intResto < 2) ? "0" : Convert.ToString((11 - intResto))).Length - 1));
            var strBase2 = (strBase.Substring(0, 10) + strDigito1);

            return strBase2 == strOrigem;

        }
        public static bool fncValida_Inscricao_Estadual_MS(string Ds_Inscricao_Estadual)
        {

            var strOrigem = Ds_Inscricao_Estadual;
            if (strOrigem.Length != 9 || strOrigem.Substring(0, 2) != "28")
                return false;

            var strBase = (strOrigem.Trim() + "000000000").Substring(0, 9);
            var intSoma = 0;

            for (var intPos = 1; (intPos <= 8); intPos++)
            {
                var intValor = int.Parse(strBase.Substring((intPos - 1), 1));
                intValor = (intValor * (10 - intPos));
                intSoma = (intSoma + intValor);
            }

            var intResto = (intSoma % 11);
            var strDigito1 = ((intResto < 2) ? "0" : Convert.ToString((11 - intResto))).Substring((((intResto < 2) ? "0" : Convert.ToString((11 - intResto))).Length - 1));
            var strBase2 = (strBase.Substring(0, 8) + strDigito1);

            return strBase2 == strOrigem;

        }
        public static bool fncValida_Inscricao_Estadual_MG(string Ds_Inscricao_Estadual)
        {

            var strOrigem = Ds_Inscricao_Estadual;
            if (strOrigem.Length != 13)
                return false;

            if (strOrigem.Substring(0, 2).ToUpper() == "PR")
                return true;


            var strBase = (strOrigem.Trim() + "0000000000000").Substring(0, 13);
            var strBase2 = strBase.Substring(0, 3) + "0" + strBase.Substring(3, 9);
            var intNumero = 1;

            var intSoma = 0;

            for (var intPos = 0; intPos < 12; intPos++)
            {

                if (int.Parse(strBase2.Substring(intPos, 1)) * intNumero >= 10)
                    intSoma += (int.Parse(strBase2.Substring(intPos, 1)) * intNumero) - 9;
                else
                    intSoma += int.Parse(strBase2.Substring(intPos, 1)) * intNumero;

                intNumero = intNumero + 1;

                if (intNumero == 3)
                    intNumero = 1;

            }

            intNumero = (int)((Math.Floor((Convert.ToDecimal(intSoma) + 10) / 10) * 10) - intSoma);
            if (intNumero % 10 == 0)
                intNumero = 0;

            if (intNumero != Convert.ToInt32(strOrigem.Substring(11, 1)))
                return false;


            intNumero = 3;
            intSoma = 0;

            for (var intPos = 0; intPos < 12; intPos++)
            {

                intSoma += int.Parse(strOrigem.Substring(intPos, 1)) * intNumero;

                intNumero = intNumero - 1;
                if (intNumero == 1)
                    intNumero = 11;

            }

            intNumero = 11 - (intSoma % 11);
            if (intNumero >= 10)
                intNumero = 0;


            return intNumero == Convert.ToInt32(strOrigem.Substring(12, 1));

        }
        public static bool fncValida_Inscricao_Estadual_PA(string Ds_Inscricao_Estadual)
        {

            var strOrigem = Ds_Inscricao_Estadual;
            if (strOrigem.Length != 9 || strOrigem.Substring(0, 2) != "15")
                return false;

            var strBase = (strOrigem.Trim() + "000000000").Substring(0, 9);
            var intSoma = 0;

            for (var intPos = 1; (intPos <= 8); intPos++)
            {
                var intValor = int.Parse(strBase.Substring((intPos - 1), 1));
                intValor = (intValor * (10 - intPos));
                intSoma = (intSoma + intValor);
            }

            var intResto = (intSoma % 11);
            var strDigito1 = ((intResto < 2) ? "0" : Convert.ToString((11 - intResto))).Substring((((intResto < 2) ? "0" : Convert.ToString((11 - intResto))).Length - 1));
            var strBase2 = (strBase.Substring(0, 8) + strDigito1);

            return strBase2 == strOrigem;

        }
        public static bool fncValida_Inscricao_Estadual_PB(string Ds_Inscricao_Estadual)
        {

            var strOrigem = Ds_Inscricao_Estadual;
            if (strOrigem.Length != 9)
                return false;

            var strBase = (strOrigem.Trim() + "000000000").Substring(0, 9);
            var intSoma = 0;
            int intValor;

            for (var intPos = 1; (intPos <= 8); intPos++)
            {
                intValor = int.Parse(strBase.Substring((intPos - 1), 1));
                intValor = (intValor * (10 - intPos));
                intSoma = (intSoma + intValor);
            }

            var intResto = (intSoma % 11);
            intValor = (11 - intResto);

            if ((intValor > 9))
                intValor = 0;

            var strDigito1 = Convert.ToString(intValor).Substring((Convert.ToString(intValor).Length - 1));
            var strBase2 = (strBase.Substring(0, 8) + strDigito1);

            return strBase2 == strOrigem;

        }
        public static bool fncValida_Inscricao_Estadual_PE(string Ds_Inscricao_Estadual)
        {

            var strOrigem = Ds_Inscricao_Estadual;
            if (strOrigem.Length != 9)
                return false;

            var strBase = (strOrigem.Trim() + "00000000000000").Substring(0, 14);
            var intSoma = 0;
            var intPeso = 2;
            int intValor;

            for (var intPos = 7; (intPos >= 1); intPos = (intPos + -1))
            {
                intValor = int.Parse(strBase.Substring((intPos - 1), 1));
                intValor = (intValor * intPeso);
                intSoma = (intSoma + intValor);
                intPeso = (intPeso + 1);

                if ((intPeso > 9))
                    intPeso = 2;
            }

            var intResto = (intSoma % 11);
            intValor = (11 - intResto);

            if ((intValor >= 10))
                intValor = 0;

            if (intValor != Convert.ToInt32(strOrigem.Substring(7, 1)))
                return false;

            var strDigito1 = Convert.ToString(intValor).Substring((Convert.ToString(intValor).Length - 1));
            var strBase2 = (strBase.Substring(0, 7) + strDigito1);

            if (strBase2 != strOrigem.Substring(0, 8))
                return false;

            intSoma = 0;
            intPeso = 2;

            for (var intPos = 8; (intPos >= 1); intPos = (intPos + -1))
            {
                intValor = int.Parse(strBase.Substring((intPos - 1), 1));
                intValor = (intValor * intPeso);
                intSoma = (intSoma + intValor);
                intPeso = (intPeso + 1);

                if ((intPeso > 9))
                    intPeso = 2;
            }

            intResto = (intSoma % 11);
            intValor = (11 - intResto);

            if ((intValor >= 10))
                intValor = 0;


            return intValor.ToString() == strOrigem.Substring(8, 1);

        }
        public static bool fncValida_Inscricao_Estadual_PI(string Ds_Inscricao_Estadual)
        {

            var strOrigem = Ds_Inscricao_Estadual;
            if (strOrigem.Length != 9)
                return false;

            var strBase = (strOrigem.Trim() + "000000000").Substring(0, 9);
            var intSoma = 0;

            for (var intPos = 1; (intPos <= 8); intPos++)
            {
                var intValor = int.Parse(strBase.Substring((intPos - 1), 1));
                intValor = (intValor * (10 - intPos));
                intSoma = (intSoma + intValor);
            }

            var intResto = (intSoma % 11);
            var strDigito1 = ((intResto < 2) ? "0" : Convert.ToString((11 - intResto))).Substring((((intResto < 2) ? "0" : Convert.ToString((11 - intResto))).Length - 1));
            var strBase2 = (strBase.Substring(0, 8) + strDigito1);

            return strBase2 == strOrigem;

        }
        public static bool fncValida_Inscricao_Estadual_PR(string Ds_Inscricao_Estadual)
        {

            var strOrigem = Ds_Inscricao_Estadual;
            if (strOrigem.Length != 10)
                return false;

            var strBase = (strOrigem.Trim() + "0000000000").Substring(0, 10);
            var intSoma = 0;
            var intPeso = 2;
            int intValor;

            for (var intPos = 8; (intPos >= 1); intPos = (intPos + -1))
            {
                intValor = int.Parse(strBase.Substring((intPos - 1), 1));
                intValor = (intValor * intPeso);
                intSoma = (intSoma + intValor);
                intPeso = (intPeso + 1);

                if ((intPeso > 7))
                    intPeso = 2;
            }

            var intResto = (intSoma % 11);
            var strDigito1 = ((intResto < 2) ? "0" : Convert.ToString((11 - intResto))).Substring((((intResto < 2) ? "0" : Convert.ToString((11 - intResto))).Length - 1));
            var strBase2 = (strBase.Substring(0, 8) + strDigito1);
            intSoma = 0;
            intPeso = 2;

            for (var intPos = 9; (intPos >= 1); intPos = (intPos + -1))
            {
                intValor = int.Parse(strBase2.Substring((intPos - 1), 1));
                intValor = (intValor * intPeso);
                intSoma = (intSoma + intValor);
                intPeso = (intPeso + 1);

                if ((intPeso > 7))
                    intPeso = 2;
            }

            intResto = (intSoma % 11);
            var strDigito2 = ((intResto < 2) ? "0" : Convert.ToString((11 - intResto))).Substring((((intResto < 2) ? "0" : Convert.ToString((11 - intResto))).Length - 1));
            strBase2 = (strBase2 + strDigito2);

            return strBase2 == strOrigem;

        }
        public static bool fncValida_Inscricao_Estadual_RJ(string Ds_Inscricao_Estadual)
        {

            var strOrigem = Ds_Inscricao_Estadual;
            if (strOrigem.Length != 8)
                return false;

            var strBase = (strOrigem.Trim() + "00000000").Substring(0, 8);
            var intSoma = 0;
            var intPeso = 2;

            for (var intPos = 7; (intPos >= 1); intPos = (intPos + -1))
            {
                var intValor = int.Parse(strBase.Substring((intPos - 1), 1));
                intValor = (intValor * intPeso);
                intSoma = (intSoma + intValor);
                intPeso = (intPeso + 1);

                if ((intPeso > 7))
                    intPeso = 2;
            }

            var intResto = (intSoma % 11);
            var strDigito1 = ((intResto < 2) ? "0" : Convert.ToString((11 - intResto))).Substring((((intResto < 2) ? "0" : Convert.ToString((11 - intResto))).Length - 1));
            var strBase2 = (strBase.Substring(0, 7) + strDigito1);

            return strBase2 == strOrigem;

        }
        public static bool fncValida_Inscricao_Estadual_RN(string Ds_Inscricao_Estadual)
        {

            var strOrigem = Ds_Inscricao_Estadual;
            var strBase = "";
            switch (strOrigem.Length)
            {
                case 9:
                    strBase = (strOrigem.Trim() + "000000000").Substring(0, 9);
                    break;

                case 10:
                    strBase = (strOrigem.Trim() + "000000000").Substring(0, 10);
                    break;
            }

            var intSoma = 0;

            if ((strBase.Substring(0, 2) == "20") && strBase.Length == 9)
            {

                for (var intPos = 1; (intPos <= 8); intPos++)
                {
                    var intValor = int.Parse(strBase.Substring((intPos - 1), 1));
                    intValor = (intValor * (10 - intPos));
                    intSoma = (intSoma + intValor);
                }

                intSoma = (intSoma * 10);
                var intResto = (intSoma % 11);
                var strDigito1 = ((intResto > 9) ? "0" : Convert.ToString(intResto)).Substring((((intResto > 9) ? "0" : Convert.ToString(intResto)).Length - 1));
                var strBase2 = (strBase.Substring(0, 8) + strDigito1);

                return strBase2 == strOrigem;

            }


            if (strBase.Length == 10)
            {
                intSoma = 0;

                for (var intPos = 1; (intPos <= 9); intPos++)
                {
                    var intValor = int.Parse(strBase.Substring((intPos - 1), 1));
                    intValor = (intValor * (11 - intPos));
                    intSoma = (intSoma + intValor);
                }

                intSoma = (intSoma * 10);
                var intResto = (intSoma % 11);
                var strDigito1 = ((intResto > 10) ? "0" : Convert.ToString(intResto)).Substring((((intResto > 10) ? "0" : Convert.ToString(intResto)).Length - 1));
                var strBase2 = (strBase.Substring(0, 9) + strDigito1);

                return strBase2 == strOrigem;

            }

            return false;

        }
        public static bool fncValida_Inscricao_Estadual_RO(string Ds_Inscricao_Estadual)
        {

            var strOrigem = Ds_Inscricao_Estadual;
            if (strOrigem.Length != 14)
                return false;

            var strBase = (strOrigem.Trim() + "000000000").Substring(0, 9);
            var strBase2 = strBase.Substring(3, 5);
            var intSoma = 0;
            int intValor;

            for (var intPos = 1; (intPos <= 5); intPos++)
            {
                intValor = int.Parse(strBase2.Substring((intPos - 1), 1));
                intValor = (intValor * (7 - intPos));
                intSoma = (intSoma + intValor);
            }

            var intResto = (intSoma % 11);
            intValor = (11 - intResto);

            if ((intValor > 9))
                intValor = (intValor - 10);

            var strDigito1 = Convert.ToString(intValor).Substring((Convert.ToString(intValor).Length - 1));
            strBase2 = (strBase.Substring(0, 8) + strDigito1);

            return strBase2 == strOrigem;

        }
        public static bool fncValida_Inscricao_Estadual_RR(string Ds_Inscricao_Estadual)
        {

            var strOrigem = Ds_Inscricao_Estadual;
            if (strOrigem.Length != 9 || strOrigem.Substring(0, 2) != "24")
                return false;

            var strBase = (strOrigem.Trim() + "000000000").Substring(0, 9);
            var intSoma = 0;

            for (var intPos = 1; (intPos <= 8); intPos++)
            {
                var intValor = int.Parse(strBase.Substring((intPos - 1), 1));
                intValor = intValor * intPos;
                intSoma += intValor;
            }

            var intResto = (intSoma % 9);
            var strDigito1 = Convert.ToString(intResto).Substring((Convert.ToString(intResto).Length - 1));
            var strBase2 = (strBase.Substring(0, 8) + strDigito1);

            return strBase2 == strOrigem;

        }
        public static bool fncValida_Inscricao_Estadual_RS(string Ds_Inscricao_Estadual)
        {

            var strOrigem = Ds_Inscricao_Estadual;
            if (strOrigem.Length != 10 || Convert.ToInt32(strOrigem) > 467)
                return false;

            var strBase = (strOrigem.Trim() + "0000000000").Substring(0, 10);
            var intSoma = 0;
            var intPeso = 2;
            var intValor = 0;

            for (var intPos = 9; (intPos >= 1); intPos = (intPos + -1))
            {
                intValor = int.Parse(strBase.Substring((intPos - 1), 1));
                intValor = (intValor * intPeso);
                intSoma = (intSoma + intValor);
                intPeso = (intPeso + 1);

                if ((intPeso > 9))
                    intPeso = 2;
            }

            var intResto = (intSoma % 11);
            intValor = (11 - intResto);

            if ((intValor > 9))
                intValor = 0;

            var strDigito1 = Convert.ToString(intValor).Substring((Convert.ToString(intValor).Length - 1));
            var strBase2 = (strBase.Substring(0, 9) + strDigito1);

            return strBase2 == strOrigem;

        }
        public static bool fncValida_Inscricao_Estadual_SC(string Ds_Inscricao_Estadual)
        {

            var strOrigem = Ds_Inscricao_Estadual;
            if (strOrigem.Length != 9)
                return false;

            var strBase = (strOrigem.Trim() + "000000000").Substring(0, 9);
            var intSoma = 0;

            for (var intPos = 1; (intPos <= 8); intPos++)
            {
                var intValor = int.Parse(strBase.Substring((intPos - 1), 1));
                intValor = (intValor * (10 - intPos));
                intSoma = (intSoma + intValor);
            }

            var intResto = (intSoma % 11);
            var strDigito1 = ((intResto < 2) ? "0" : Convert.ToString((11 - intResto))).Substring((((intResto < 2) ? "0" : Convert.ToString((11 - intResto))).Length - 1));
            var strBase2 = (strBase.Substring(0, 8) + strDigito1);

            return strBase2 == strOrigem;

        }
        public static bool fncValida_Inscricao_Estadual_SE(string Ds_Inscricao_Estadual)
        {

            var strOrigem = Ds_Inscricao_Estadual;
            if (strOrigem.Length != 9)
                return false;

            var strBase = (strOrigem.Trim() + "000000000").Substring(0, 9);
            var intSoma = 0;
            int intValor;

            for (var intPos = 1; (intPos <= 8); intPos++)
            {
                intValor = int.Parse(strBase.Substring((intPos - 1), 1));
                intValor = (intValor * (10 - intPos));
                intSoma = (intSoma + intValor);
            }

            var intResto = (intSoma % 11);
            intValor = (11 - intResto);

            if ((intValor > 9))
                intValor = 0;

            var strDigito1 = Convert.ToString(intValor).Substring((Convert.ToString(intValor).Length - 1));
            var strBase2 = (strBase.Substring(0, 8) + strDigito1);

            return strBase2 == strOrigem;

        }
        public static bool fncValida_Inscricao_Estadual_SP(string Ds_Inscricao_Estadual)
        {

            var strOrigem = Ds_Inscricao_Estadual;
            string strBase;
            string strBase2;
            int intSoma;
            int intPeso;

            if ((strOrigem.Substring(0, 1) == "P"))
            {
                strBase = (strOrigem.Trim() + "0000000000000").Substring(0, 13);
                intSoma = 0;
                intPeso = 1;

                for (var intPos = 1; (intPos <= 8); intPos++)
                {
                    var intValor = int.Parse(strBase.Substring((intPos), 1));
                    intValor = (intValor * intPeso);
                    intSoma = (intSoma + intValor);
                    intPeso = (intPeso + 1);

                    if ((intPeso == 2))
                        intPeso = 3;

                    if ((intPeso == 9))
                        intPeso = 10;
                }

                var intResto = (intSoma % 11);
                var strDigito1 = Convert.ToString(intResto).Substring((Convert.ToString(intResto).Length - 1));
                strBase2 = (strBase.Substring(0, 9) + (strDigito1 + strBase.Substring(10, 3)));
            }
            else
            {
                strBase = (strOrigem.Trim() + "000000000000").Substring(0, 12);
                intSoma = 0;
                intPeso = 1;

                for (var intPos = 1; (intPos <= 8); intPos++)
                {
                    var intValor = int.Parse(strBase.Substring((intPos - 1), 1));
                    intValor = (intValor * intPeso);
                    intSoma = (intSoma + intValor);
                    intPeso = (intPeso + 1);

                    if ((intPeso == 2))
                        intPeso = 3;

                    if ((intPeso == 9))
                        intPeso = 10;
                }

                var intResto = (intSoma % 11);
                var strDigito1 = Convert.ToString(intResto).Substring((Convert.ToString(intResto).Length - 1));
                strBase2 = (strBase.Substring(0, 8) + (strDigito1 + strBase.Substring(9, 2)));
                intSoma = 0;
                intPeso = 2;

                for (var intPos = 11; (intPos >= 1); intPos = (intPos + -1))
                {
                    var intValor = int.Parse(strBase.Substring((intPos - 1), 1));
                    intValor = (intValor * intPeso);
                    intSoma = (intSoma + intValor);
                    intPeso = (intPeso + 1);

                    if ((intPeso > 10))
                        intPeso = 2;
                }

                intResto = (intSoma % 11);
                var strDigito2 = Convert.ToString(intResto).Substring((Convert.ToString(intResto).Length - 1));
                strBase2 = (strBase2 + strDigito2);
            }

            return strBase2 == strOrigem;

        }
        public static bool fncValida_Inscricao_Estadual_TO(string Ds_Inscricao_Estadual)
        {

            var strOrigem = Ds_Inscricao_Estadual;
            if (strOrigem.Length != 11 || strOrigem.Substring(2, 2) != "01" || strOrigem.Substring(2, 2) != "02" || strOrigem.Substring(2, 2) != "03" || strOrigem.Substring(2, 2) != "99")
                return false;

            var strBase = (strOrigem.Trim() + "00000000000").Substring(0, 11);
            var strBase2 = (strBase.Substring(0, 2) + strBase.Substring(4, 6));
            var intSoma = 0;

            for (var intPos = 1; (intPos <= 8); intPos++)
            {
                var intValor = int.Parse(strBase2.Substring((intPos - 1), 1));
                intValor = (intValor * (10 - intPos));
                intSoma = (intSoma + intValor);
            }

            var intResto = (intSoma % 11);
            var strDigito1 = ((intResto < 2) ? "0" : Convert.ToString((11 - intResto))).Substring((((intResto < 2) ? "0" : Convert.ToString((11 - intResto))).Length - 1));
            strBase2 = (strBase.Substring(0, 10) + strDigito1);

            return strBase2 == strOrigem;


        }

        private void RgIeBox_Leave(object sender, EventArgs e)
        {
            //se o usuario sair sem digitar nada
            RgIeBox.BackColor = Color.IndianRed;
            if(uF == null)
            {
                //para fazer a verificação do I.E. é necessario o UF, caso o usuario nao tenha preenchido o campo cep o valor do UF é nulo
                if(CepBox.Text == "")
                {
                    RgIeBox.BackColor = Color.White;
                    RgIeBox.ForeColor = Color.Gray;
                    RgIeBox.Text = "Preencha o campo Cep Primeiro";
                }else
                {
                    RgIeBox.ForeColor = Color.Black;
                }
            }
            else if(RgIeBox.Text.Length >= 8 && userType == "J")
            {
                //verifica se o usuario é uma pessoa juridica e faz a validaçao do IE
                if(!fncValida_Inscricao_Estadual(uF, RgIeBox.Text))
                {
                    RgIeBox.BackColor = Color.IndianRed;
                    MessageBox.Show("IE invalido");
                }
                else
                {
                    RgIeBox.BackColor = Color.White;
                }
            }
            else if(RgIeBox.Text != "")
            {
                RgIeBox.BackColor = Color.White;
            }
        }

        public static bool emailValidator(string email)
        {
            // função para verificar o email
            var isValid = new EmailAddressAttribute().IsValid(email);
            if (isValid)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private void EmailBox_Leave(object sender, EventArgs e)
        {
            // verifica se o email indicado é valido
            if (emailValidator(EmailBox.Text))
            {
                EmailBox.BackColor = Color.White;
            }
            else
            {
                EmailBox.BackColor = Color.IndianRed;
            }
        }

        public static bool numberIsValid(string number)
        {
            // se o numero de telefone nao se encaixar nesse padrao nao sera valido
            string pattern = @"^1\d\d(\d\d)?$|^0800 ?\d{3} ?\d{4}$|^(\(0?([1-9a-zA-Z][0-9a-zA-Z])?[1-9]\d\) ?|0?([1-9a-zA-Z][0-9a-zA-Z])?[1-9]\d[ .-]?)?(9|9[ .-])?[2-9]\d{3}[ .-]?\d{4}$";
            
            if (Regex.IsMatch(number, pattern))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private void TelefoneBox_Leave(object sender, EventArgs e)
        {
            //muda a cor de fundo se o usuario entrar no campo e nao digitar nada ou digitar um numero invalido
            if (numberIsValid(TelefoneBox.Text))
            {
                TelefoneBox.BackColor = Color.White;
            }
            else
            {
                TelefoneBox.BackColor = Color.IndianRed;
            }
        }

        private void CepBox_TextChanged(object sender, EventArgs e)
        {
            //permite que o usuario digite apenas numeros
            if (Regex.IsMatch(CepBox.Text, "[^0-9]"))
            {
                CepBox.Text = CepBox.Text.Remove(CepBox.Text.Length - 1);
            }
        }

        private void CpfCnpjBox_TextChanged(object sender, EventArgs e)
        {
            //permite que o usuario digite apenas numeros
            if (Regex.IsMatch(CpfCnpjBox.Text, "[^0-9]"))
            {
                CpfCnpjBox.Text = CpfCnpjBox.Text.Remove(CpfCnpjBox.Text.Length - 1);
            }
        }

        private void TelefoneBox_TextChanged(object sender, EventArgs e)
        {
            //permite que o usuario digite apenas numeros
            if (Regex.IsMatch(TelefoneBox.Text, "[^0-9]"))
            {
                TelefoneBox.Text = TelefoneBox.Text.Remove(TelefoneBox.Text.Length - 1);
            }
        }

        private void CellphoneBox_TextChanged(object sender, EventArgs e)
        {
            //permite que o usuario digite apenas numeros
            if (Regex.IsMatch(CellphoneBox.Text, "[^0-9]"))
            {
                CellphoneBox.Text = CellphoneBox.Text.Remove(CellphoneBox.Text.Length - 1);
            }
        }

        private void CellphoneBox_Leave(object sender, EventArgs e)
        {
            //muda a cor de fundo se o usuario entrar no campo e nao digitar nada ou digitar um numero invalido
            if (numberIsValid(CellphoneBox.Text))
            {
                CellphoneBox.BackColor = Color.White;
            }
            else
            {
                CellphoneBox.BackColor = Color.IndianRed;
            }
        }

        private void NameBox_Leave(object sender, EventArgs e)
        {
            //muda a cor de fundo se o usuario entrar no campo e nao digitar nada
            if (NameBox.Text != "")
            {
                NameBox.BackColor = Color.White;
            }
            else
            {
                NameBox.BackColor = Color.IndianRed;
            }
        }

        private void SiteBox_Leave(object sender, EventArgs e)
        {
            //muda a cor de fundo se o usuario entrar no campo e nao digitar nada
            if (SiteBox.Text != "")
            {
                SiteBox.BackColor = Color.White;
            }
            else
            {
                SiteBox.BackColor = Color.IndianRed;
            }
        }

        private void AddressNumberBox_Leave(object sender, EventArgs e)
        {
            //muda a cor de fundo se o usuario entrar no campo e nao digitar nada
            if (AddressNumberBox.Text != "")
            {
                AddressNumberBox.BackColor = Color.White;
            }
            else
            {
                AddressNumberBox.BackColor = Color.IndianRed;
            }
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            //muda a cor de fundo do campo nome
            NameBox.BackColor = Color.White;
            if (btnSearch.Text.Trim() == "Pesquisar")
            {
                //abre o form de pesquisa e esconde o form de relatorio
                var frm = new SearchForm();
                frm.Location = this.Location;
                frm.StartPosition = FormStartPosition.Manual;
                frm.FormClosing += delegate { this.Show(); };
                frm.Show();
                this.Hide();
            }
            else
            {
                using (MySqlConnection MySqlCon = new MySqlConnection(connectionString))
                {
                    //remove o usuario pelo id do banco de dados
                    MySqlCon.Open();
                    MySqlCommand MySqlCmd = new MySqlCommand("DeleteUserByID", MySqlCon);
                    MySqlCmd.CommandType = CommandType.StoredProcedure;
                    MySqlCmd.Parameters.AddWithValue("_user_id", user_id);
                    MySqlCmd.ExecuteNonQuery();
                    MessageBox.Show("Usuario removido com sucesso");
                    this.Close();
                }
            }
        }

        private void ClearBtn_Click(object sender, EventArgs e)
        {
            if(ClearBtn.Text.Trim() == "Relatorio")
            {
                //enviar o email 
                MailMessage message = new MailMessage("remetente email", "contato@zipersoft.com.br")
                {
                    Subject = "[DESAFIO] - Augusto Almeida",
                    Body =
                    "Id = " + user_id.ToString() + Environment.NewLine +
                    "Nome = " + NameBox.Text + Environment.NewLine +
                    "Cep = " + CepBox.Text + Environment.NewLine +
                    "Endereço = " + AddressBox.Text + Environment.NewLine +
                    "Numero = " + AddressNumberBox.Text + Environment.NewLine +
                    "Bairro = " + NeighborhoodBox.Text + Environment.NewLine +
                    "Cidade = " + CityBox.Text + Environment.NewLine +
                    "CPF_CNPJ = " + CpfCnpjBox.Text + Environment.NewLine +
                    "RG_IE = " + RgIeBox.Text + Environment.NewLine +
                    "E_Mail = " + EmailBox.Text + Environment.NewLine +
                    "Site = " + SiteBox.Text + Environment.NewLine +
                    "Telefone = " + TelefoneBox.Text + Environment.NewLine +
                    "Celular = " + CellphoneBox.Text + Environment.NewLine +
                    "Observação = " + ObsBox.Text + Environment.NewLine +
                    "Pessoa = " + userType
                };
                using (SmtpClient client = new SmtpClient("smtp.gmail.com", 587))
                {
                    client.Credentials = new NetworkCredential("remetente email", "remetente senha");
                    client.EnableSsl = true;
                    client.Send(message);
                }
                MessageBox.Show("Relatorio Enviado com sucesso");
            }
            else
            {
                //limpa os campos do relatorio
                Clear();
            }
        }
    }
}
