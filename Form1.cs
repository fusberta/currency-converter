using System;
using MaterialSkin;
using MaterialSkin.Controls;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using System.Reflection.Emit;


namespace WindowsFormsApp2
{
    public partial class Form1 : MaterialForm
    {
        public Form1()
        {
            InitializeComponent();

            var materialSkinManager = MaterialSkinManager.Instance;
            materialSkinManager.EnforceBackcolorOnAllComponents = true;
            materialSkinManager.AddFormToManage(this);
            materialSkinManager.Theme = MaterialSkinManager.Themes.LIGHT;
            materialSkinManager.ColorScheme = new ColorScheme(Primary.Indigo500, Primary.Indigo700, Primary.Indigo100, Accent.Pink400, TextShade.WHITE);
        }

        private void materialButton2_Click(object sender, EventArgs e)
        {
            var client = new System.Net.WebClient();
            System.IO.Stream stream;
            String str, str1;
            string date = dateTimePicker1.Value.ToShortDateString();
            string link = "https://cbr.ru/currency_base/daily/?UniDbQuery.Posted=True&UniDbQuery.To=" + date;
            try
            {
                stream = client.OpenRead(link);
            }
            catch (Exception situation)
            {
                str = String.Format("www.cbr.ru" + "\n{0}", situation.Message);
                materialLabel6.Text = str; return;
            }

            var reader = new System.IO.StreamReader(stream);
            str = reader.ReadToEnd();
            str1 = str;

            var i = str.IndexOf("Доллар США");
            str = str.Substring(i, 40);
            i = str.IndexOf(">");

            str = str.Substring(i + 1);
            i = str.IndexOf(">");
            str = str.Substring(i + 1, 7);
            double courseDol = Double.Parse(str);
            //=========
            var j = str1.IndexOf("Евро");

            str1 = str1.Substring(j, 40);
            j = str1.IndexOf(">");

            str1 = str1.Substring(j + 1);
            j = str1.IndexOf(">");
            str1 = str1.Substring(j + 1, 7);

            double courseEur = Double.Parse(str1);
            stream.Close();
            DateTime dt1 = DateTime.Today;
            DateTime dt2 = DateTime.Parse(date);

            if (dt1 < dt2)
            {
                str = "$ = " + str + " (" + dt1.ToShortDateString() + "); ";
                str1 = "€ = " + str1 + " (" + dt1.ToShortDateString() + "); ";
                MessageBox.Show("Выбрана некорректная дата.\n\nИспользована текущая дата.");
            }
            else
            {
                str = "$ = " + str + " (" + dt2.ToShortDateString() + "); ";
                str1 = "€ = " + str1 + " (" + dt2.ToShortDateString() + "); ";
            }

            materialLabel6.Text = str;
            materialLabel7.Text = str1;

            double userValue;
            if (materialTextBox1.Text != "")
            {
                userValue = Double.Parse(materialTextBox1.Text);
            }
            else { userValue = 0; }

            if (materialComboBox1.SelectedIndex == materialComboBox2.SelectedIndex)
            {
                MessageBox.Show("Ошибка выбора валют");
            }

            var round = Convert.ToInt32(numericUpDown1.Value);
            switch (materialComboBox1.SelectedIndex)
            {
                case 0:
                    if (materialComboBox2.SelectedIndex == 1)
                    {
                        double result = userValue / courseDol;
                        result = Math.Round(result, round);
                        materialTextBox2.Text = Convert.ToString(result);
                        richTextBox1.Text += Convert.ToString(userValue) + " " + materialComboBox1.SelectedItem + " => " + Convert.ToString(result) + " " 
                            + materialComboBox2.SelectedItem + "\n" + str + "\n\n";
                    }
                    else if (materialComboBox2.SelectedIndex == 2)
                    {
                        double result = userValue / courseEur;
                        result = Math.Round(result, round);
                        materialTextBox2.Text = Convert.ToString(result);
                        richTextBox1.Text += Convert.ToString(userValue) + " " + materialComboBox1.SelectedItem + " => " + Convert.ToString(result) + " " 
                            + materialComboBox2.SelectedItem + "\n" + str1 + "\n\n";
                    }
                    break;
                case 1:
                    if (materialComboBox2.SelectedIndex == 0)
                    {
                        double result = userValue * courseDol;
                        result = Math.Round(result, round);
                        materialTextBox2.Text = Convert.ToString(result);
                        richTextBox1.Text += Convert.ToString(userValue) + " " + materialComboBox1.SelectedItem + " => " + Convert.ToString(result) + " " 
                            + materialComboBox2.SelectedItem + "\n" + str + "\n\n";
                    }
                    else if (materialComboBox2.SelectedIndex == 2)
                    {
                        double result = userValue * courseDol / courseEur;
                        result = Math.Round(result, round);
                        materialTextBox2.Text = Convert.ToString(result);
                        richTextBox1.Text += Convert.ToString(userValue) + " " + materialComboBox1.SelectedItem + " => " + Convert.ToString(result) + " " 
                            + materialComboBox2.SelectedItem + "\n\n";
                    }
                    break;
                case 2:
                    if (materialComboBox2.SelectedIndex == 0)
                    {
                        double result = userValue * courseEur;
                        result = Math.Round(result, round);
                        materialTextBox2.Text = Convert.ToString(result);
                        richTextBox1.Text += Convert.ToString(userValue) + " " + materialComboBox1.SelectedItem + " => " + Convert.ToString(result) + " "
                            + materialComboBox2.SelectedItem + "\n" + str1 + "\n\n";
                    }
                    else if (materialComboBox2.SelectedIndex == 1)
                    {
                        double result = userValue * courseEur / courseDol;
                        result = Math.Round(result, round);
                        materialTextBox2.Text = Convert.ToString(result);
                        richTextBox1.Text += Convert.ToString(userValue) + " " + materialComboBox1.SelectedItem + " => " + Convert.ToString(result) + " "
                            + materialComboBox2.SelectedItem + "\n\n";
                    }
                    break;
            }
        }

        private void materialButton1_Click(object sender, EventArgs e)
        {
            var item = materialComboBox1.SelectedItem;
            materialComboBox1.SelectedItem = materialComboBox2.SelectedItem;
            materialComboBox2.SelectedItem = item;
        }

        private void materialButton3_Click(object sender, EventArgs e)
        {
            if (richTextBox1.Visible)
                richTextBox1.Visible = false;
            else
                richTextBox1.Visible = true;
        }

        private void materialTextBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            char number = e.KeyChar;
            if (!Char.IsDigit(number) && number != 8) // цифры и клавиша BackSpace
            {
                e.Handled = true;
            }
        }
    }
}
