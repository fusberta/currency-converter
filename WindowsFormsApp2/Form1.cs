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
using System.Windows.Forms.DataVisualization.Charting;
using System.Runtime.InteropServices.ComTypes;
using System.IO;
using static System.Windows.Forms.LinkLabel;
using System.Diagnostics;

namespace WindowsFormsApp2
{
    public partial class Form1 : MaterialForm
    {
        DateTime firstDateStart = DateTime.Now.AddMonths(-1);
        DateTime firstDateEnd = DateTime.Now;
        public Form1()
        {
            InitializeComponent();

            // Настройка дизайна формы
            var materialSkinManager = MaterialSkinManager.Instance;
            materialSkinManager.EnforceBackcolorOnAllComponents = true;
            materialSkinManager.AddFormToManage(this);
            materialSkinManager.Theme = MaterialSkinManager.Themes.LIGHT;
            materialSkinManager.ColorScheme = new ColorScheme(Primary.Indigo500, Primary.Indigo700, Primary.Indigo100, Accent.Pink400, TextShade.WHITE);

        }

        public double streamCurrency(string currency, string link)
        {
            var client = new System.Net.WebClient();
            System.IO.Stream stream;
            String str;
            try
            {
                stream = client.OpenRead(link);
            }
            catch (Exception situation)
            {
                str = String.Format("www.cbr.ru" + "\n{0}", situation.Message);
                materialLabel6.Text = str;
                throw situation;
            }
            var reader = new System.IO.StreamReader(stream);
            str = reader.ReadToEnd();

            var i = str.IndexOf(currency);
            str = str.Substring(i, 40);
            i = str.IndexOf(">");
            str = str.Substring(i + 1);
            i = str.IndexOf(">");
            str = str.Substring(i + 1, 7);
            double course = Double.Parse(str);

            stream.Close();
            return course;
        }

        private void materialButton2_Click(object sender, EventArgs e)
        {
            String str, str1, str2, str3;
            string date = dateTimePicker1.Value.ToShortDateString();
            string link = "https://cbr.ru/currency_base/daily/?UniDbQuery.Posted=True&UniDbQuery.To=" + date;

            str = streamCurrency("Доллар США", link).ToString();
            double courseDol = Double.Parse(str);
            str1 = streamCurrency("Евро", link).ToString();
            double courseEur = Double.Parse(str1);
            str2 = streamCurrency("Японских иен", link).ToString();
            double courseJPY = Double.Parse(str2) / 100;
            str3 = streamCurrency("Евро", link).ToString();
            double courseAUD = Double.Parse(str3);

            DateTime dt1 = DateTime.Today;
            DateTime dt2 = DateTime.Parse(date);

            if (dt1 < dt2)
            {
                str = "$ = " + str + " (" + dt1.ToShortDateString() + "); ";
                str1 = "€ = " + str1 + " (" + dt1.ToShortDateString() + "); ";
                str2 = "¥ = " + courseJPY + " (" + dt1.ToShortDateString() + "); ";
                str3 = "A$ = " + courseAUD + " (" + dt1.ToShortDateString() + "); ";
                MessageBox.Show("Выбрана некорректная дата.\n\nИспользована текущая дата.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
            else
            {
                str = "$ = " + str + " (" + dt2.ToShortDateString() + "); ";
                str1 = "€ = " + str1 + " (" + dt2.ToShortDateString() + "); ";
                str2 = "¥ = " + courseJPY + " (" + dt2.ToShortDateString() + "); ";
                str3 = "A$ = " + courseAUD + " (" + dt2.ToShortDateString() + "); ";
            }

            double userValue;
            if (materialTextBox1.Text != "")
            {
                userValue = Double.Parse(materialTextBox1.Text);
            }
            else { userValue = 0; }

            if (mkb1.SelectedIndex == mkb2.SelectedIndex)
            {
                MessageBox.Show("Ошибка выбора валют.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }

            var round = Convert.ToInt32(numericUpDown1.Value);
            switch (mkb1.SelectedIndex)
            {
                case 0:
                    if (mkb2.SelectedIndex == 1)
                    {
                        double result = userValue / courseDol;
                        result = Math.Round(result, round);
                        materialTextBox2.Text = Convert.ToString(result);
                        richTextBox1.Text += Convert.ToString(userValue) + " " + mkb1.SelectedItem + " => " + Convert.ToString(result) + " "
                            + mkb2.SelectedItem + "\n" + str + "\n\n";
                    }
                    else if (mkb2.SelectedIndex == 2)
                    {
                        double result = userValue / courseEur;
                        result = Math.Round(result, round);
                        materialTextBox2.Text = Convert.ToString(result);
                        richTextBox1.Text += Convert.ToString(userValue) + " " + mkb1.SelectedItem + " => " + Convert.ToString(result) + " "
                            + mkb2.SelectedItem + "\n" + str1 + "\n\n";
                    }
                    else if (mkb2.SelectedIndex == 3)
                    {
                        double result = userValue * courseJPY;
                        result = Math.Round(result, round);
                        materialTextBox2.Text = Convert.ToString(result);
                        richTextBox1.Text += Convert.ToString(userValue) + " " + mkb1.SelectedItem + " => " + Convert.ToString(result) + " "
                            + mkb2.SelectedItem + "\n" + str2 + "\n\n";
                    }
                    else if (mkb2.SelectedIndex == 4)
                    {
                        double result = userValue / courseAUD;
                        result = Math.Round(result, round);
                        materialTextBox2.Text = Convert.ToString(result);
                        richTextBox1.Text += Convert.ToString(userValue) + " " + mkb1.SelectedItem + " => " + Convert.ToString(result) + " "
                            + mkb2.SelectedItem + "\n" + str3 + "\n\n";
                    }
                    break;
                case 1:
                    if (mkb2.SelectedIndex == 0)
                    {
                        double result = userValue * courseDol;
                        result = Math.Round(result, round);
                        materialTextBox2.Text = Convert.ToString(result);
                        richTextBox1.Text += Convert.ToString(userValue) + " " + mkb1.SelectedItem + " => " + Convert.ToString(result) + " "
                            + mkb2.SelectedItem + "\n" + str + "\n\n";
                    }
                    else if (mkb2.SelectedIndex == 2)
                    {
                        double result = userValue * courseDol / courseEur;
                        result = Math.Round(result, round);
                        materialTextBox2.Text = Convert.ToString(result);
                        richTextBox1.Text += Convert.ToString(userValue) + " " + mkb1.SelectedItem + " => " + Convert.ToString(result) + " "
                            + mkb2.SelectedItem + "\n\n";
                    }
                    else if (mkb2.SelectedIndex == 3)
                    {
                        double result = userValue * courseDol / courseJPY;
                        result = Math.Round(result, round);
                        materialTextBox2.Text = Convert.ToString(result);
                        richTextBox1.Text += Convert.ToString(userValue) + " " + mkb1.SelectedItem + " => " + Convert.ToString(result) + " "
                            + mkb2.SelectedItem + "\n\n";
                    }
                    else if (mkb2.SelectedIndex == 4)
                    {
                        double result = userValue * courseDol / courseAUD;
                        result = Math.Round(result, round);
                        materialTextBox2.Text = Convert.ToString(result);
                        richTextBox1.Text += Convert.ToString(userValue) + " " + mkb1.SelectedItem + " => " + Convert.ToString(result) + " "
                            + mkb2.SelectedItem + "\n\n";
                    }
                    break;
                case 2:
                    if (mkb2.SelectedIndex == 0)
                    {
                        double result = userValue * courseEur;
                        result = Math.Round(result, round);
                        materialTextBox2.Text = Convert.ToString(result);
                        richTextBox1.Text += Convert.ToString(userValue) + " " + mkb1.SelectedItem + " => " + Convert.ToString(result) + " "
                            + mkb2.SelectedItem + "\n" + str1 + "\n\n";
                    }
                    else if (mkb2.SelectedIndex == 1)
                    {
                        double result = userValue * courseEur / courseDol;
                        result = Math.Round(result, round);
                        materialTextBox2.Text = Convert.ToString(result);
                        richTextBox1.Text += Convert.ToString(userValue) + " " + mkb1.SelectedItem + " => " + Convert.ToString(result) + " "
                            + mkb2.SelectedItem + "\n\n";
                    }
                    else if (mkb2.SelectedIndex == 3)
                    {
                        double result = userValue * courseEur / courseJPY;
                        result = Math.Round(result, round);
                        materialTextBox2.Text = Convert.ToString(result);
                        richTextBox1.Text += Convert.ToString(userValue) + " " + mkb1.SelectedItem + " => " + Convert.ToString(result) + " "
                            + mkb2.SelectedItem + "\n\n";
                    }
                    else if (mkb2.SelectedIndex == 4)
                    {
                        double result = userValue * courseEur / courseAUD;
                        result = Math.Round(result, round);
                        materialTextBox2.Text = Convert.ToString(result);
                        richTextBox1.Text += Convert.ToString(userValue) + " " + mkb1.SelectedItem + " => " + Convert.ToString(result) + " "
                            + mkb2.SelectedItem + "\n\n";
                    }
                    break;
                case 3:
                    if (mkb2.SelectedIndex == 0)
                    {
                        double result = userValue * courseJPY;
                        result = Math.Round(result, round);
                        materialTextBox2.Text = Convert.ToString(result);
                        richTextBox1.Text += Convert.ToString(userValue) + " " + mkb1.SelectedItem + " => " + Convert.ToString(result) + " "
                            + mkb2.SelectedItem + "\n" + str2 + "\n\n";
                    }
                    else if (mkb2.SelectedIndex == 1)
                    {
                        double result = userValue * courseJPY / courseDol;
                        result = Math.Round(result, round);
                        materialTextBox2.Text = Convert.ToString(result);
                        richTextBox1.Text += Convert.ToString(userValue) + " " + mkb1.SelectedItem + " => " + Convert.ToString(result) + " "
                            + mkb2.SelectedItem + "\n\n";
                    }
                    else if (mkb2.SelectedIndex == 2)
                    {
                        double result = userValue * courseJPY / courseEur;
                        result = Math.Round(result, round);
                        materialTextBox2.Text = Convert.ToString(result);
                        richTextBox1.Text += Convert.ToString(userValue) + " " + mkb1.SelectedItem + " => " + Convert.ToString(result) + " "
                            + mkb2.SelectedItem + "\n\n";
                    }
                    else if (mkb2.SelectedIndex == 4)
                    {
                        double result = userValue * courseJPY / courseAUD;
                        result = Math.Round(result, round);
                        materialTextBox2.Text = Convert.ToString(result);
                        richTextBox1.Text += Convert.ToString(userValue) + " " + mkb1.SelectedItem + " => " + Convert.ToString(result) + " "
                            + mkb2.SelectedItem + "\n\n";
                    }
                    break;
                case 4:
                    if (mkb2.SelectedIndex == 0)
                    {
                        double result = userValue * courseAUD;
                        result = Math.Round(result, round);
                        materialTextBox2.Text = Convert.ToString(result);
                        richTextBox1.Text += Convert.ToString(userValue) + " " + mkb1.SelectedItem + " => " + Convert.ToString(result) + " "
                            + mkb2.SelectedItem + "\n" + str2 + "\n\n";
                    }
                    else if (mkb2.SelectedIndex == 1)
                    {
                        double result = userValue * courseAUD / courseDol;
                        result = Math.Round(result, round);
                        materialTextBox2.Text = Convert.ToString(result);
                        richTextBox1.Text += Convert.ToString(userValue) + " " + mkb1.SelectedItem + " => " + Convert.ToString(result) + " "
                            + mkb2.SelectedItem + "\n\n";
                    }
                    else if (mkb2.SelectedIndex == 2)
                    {
                        double result = userValue * courseAUD / courseEur;
                        result = Math.Round(result, round);
                        materialTextBox2.Text = Convert.ToString(result);
                        richTextBox1.Text += Convert.ToString(userValue) + " " + mkb1.SelectedItem + " => " + Convert.ToString(result) + " "
                            + mkb2.SelectedItem + "\n\n";
                    }
                    else if (mkb2.SelectedIndex == 3)
                    {
                        double result = userValue * courseAUD / courseJPY;
                        result = Math.Round(result, round);
                        materialTextBox2.Text = Convert.ToString(result);
                        richTextBox1.Text += Convert.ToString(userValue) + " " + mkb1.SelectedItem + " => " + Convert.ToString(result) + " "
                            + mkb2.SelectedItem + "\n\n";
                    }
                    break;
            }
        }
        // Смена выбранных валют местами
        private void materialButton1_Click(object sender, EventArgs e)
        {
            var item = mkb1.SelectedIndex;
            mkb1.SelectedIndex = mkb2.SelectedIndex;
            mkb2.SelectedIndex = item;

            mkb1.Focus();
            mkb2.Focus();
            materialButton1.Focus();
        }
        // Переключение Истории
        private void materialButton3_Click(object sender, EventArgs e)
        {
            if (richTextBox1.Visible)
                richTextBox1.Visible = false;
            else
                richTextBox1.Visible = true;
        }
        // Валидация пользовательского поля 
        private void materialTextBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            char number = e.KeyChar;
            if (!Char.IsDigit(number) && number != 8) // цифры и клавиша BackSpace
            {
                e.Handled = true;
            }
        }
        // Настройка веб браузера
        private void webBrowser1_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {
            webBrowser1.Document.Body.Style = "zoom:75%";
            webBrowser1.Document.Window.ScrollTo(0, 410);
            dateTimePicker5.Value = DateTime.Now.AddMonths(-1);
            dateTimePicker4.Value = DateTime.Now;
        }

        private void materialExpansionPanel1_SaveClick(object sender, EventArgs e)
        {

            if (dateTimePicker5.Value < dateTimePicker4.Value)
            {
                string dateStart = dateTimePicker5.Value.ToShortDateString();
                string dateEnd = dateTimePicker4.Value.ToShortDateString();
                string currencyCode = "";
                switch (materialComboBox1.SelectedItem)
                {
                    case "USD":
                        currencyCode = "R01235";
                        break;
                    case "EUR":
                        currencyCode = "R01239";
                        break;
                    case "JPY":
                        currencyCode = "R01820";
                        break;
                    case "AUD":
                        currencyCode = "R01010";
                        break;
                }
                string link = String.Format("https://cbr.ru/currency_base/dynamics/?UniDbQuery.Posted=True&UniDbQuery.so=1&UniDbQuery.mode=2&UniDbQuery.date_req1=&UniDbQuery.date_req2=&UniDbQuery.VAL_NM_RQ={0}&UniDbQuery.From={1}&UniDbQuery.To={2}", currencyCode, dateStart, dateEnd);
                webBrowser1.Navigate(link);
            }
            else if (dateTimePicker5.Value == dateTimePicker4.Value)
            {
                MessageBox.Show("Начальная дата совпадает с конечной.", "Ошибка даты", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                MessageBox.Show("Начальная дата больше конечной.", "Ошибка даты", MessageBoxButtons.OK, MessageBoxIcon.Error);
                dateTimePicker5.Value = firstDateStart;
                dateTimePicker4.Value = firstDateEnd;
            }
        }

        private void materialLabel11_Click(object sender, EventArgs e)
        {
            ProcessStartInfo sInfo = new ProcessStartInfo("https://cbr.ru/currency_base/daily/");
            Process.Start(sInfo);
        }
        private void materialLabel11_MouseEnter(object sender, EventArgs e)
        {
            materialLabel1.Font = new Font(materialLabel1.Font.Name, materialLabel1.Font.SizeInPoints, FontStyle.Underline);
        }

        private void materialLabel11_MouseLeave(object sender, EventArgs e)
        {
            materialLabel1.Font = new Font(materialLabel1.Font.Name, materialLabel1.Font.SizeInPoints, FontStyle.Regular);
        }

        private void materialListBox1_SelectedIndexChanged(object sender, MaterialListBoxItem selectedItem)
        {
            ProcessStartInfo sInfo;
            switch (materialListBox1.SelectedIndex)
            {
                case 0:
                    sInfo = new ProcessStartInfo("https://ru.wikipedia.org/wiki/%D0%A0%D0%BE%D1%81%D1%81%D0%B8%D0%B9%D1%81%D0%BA%D0%B8%D0%B9_%D1%80%D1%83%D0%B1%D0%BB%D1%8C");
                    Process.Start(sInfo);
                    break;
                case 1:
                    sInfo = new ProcessStartInfo("https://ru.wikipedia.org/wiki/%D0%94%D0%BE%D0%BB%D0%BB%D0%B0%D1%80_%D0%A1%D0%A8%D0%90");
                    Process.Start(sInfo);
                    break;
                case 2:
                    sInfo = new ProcessStartInfo("https://ru.wikipedia.org/wiki/%D0%95%D0%B2%D1%80%D0%BE");
                    Process.Start(sInfo);
                    break;
                case 3:
                    sInfo = new ProcessStartInfo("https://ru.wikipedia.org/wiki/%D0%98%D0%B5%D0%BD%D0%B0");
                    Process.Start(sInfo);
                    break;
                case 4:
                    sInfo = new ProcessStartInfo("https://ru.wikipedia.org/wiki/%D0%90%D0%B2%D1%81%D1%82%D1%80%D0%B0%D0%BB%D0%B8%D0%B9%D1%81%D0%BA%D0%B8%D0%B9_%D0%B4%D0%BE%D0%BB%D0%BB%D0%B0%D1%80");
                    Process.Start(sInfo);
                    break;
            }
        }
    }
}
