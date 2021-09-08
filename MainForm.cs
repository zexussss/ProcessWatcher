using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Drawing.Printing;

namespace ProcessWatcher
{
    public partial class MainForm : Form
    {
        public delegate void DisplayDelegate(string str);
        public DisplayDelegate displayDelegate;
        private EventWatcher ew;
        private StringBuilder sbOutText;
        private bool isStartEnabled;

        public MainForm()
        {
            InitializeComponent();
            displayDelegate = new DisplayDelegate(DisplayResult);
            sbOutText = new StringBuilder();
            isStartEnabled = true;
        }

        public void InitializeStartStop()
        {
            isStartEnabled = !isStartEnabled;
            StartButton.Enabled = isStartEnabled;
            StopButton.Enabled = !StartButton.Enabled;

        }

        private void StartButton_Click(object sender, EventArgs e)
        {
            try
            {
                InitializeStartStop();
                textBox1.BackColor = Color.Gray;
                ew = new EventWatcher(this);
            }
            catch
            {
                StartButton.Enabled = isStartEnabled = true;
            }
        }

        private void StopAllWatchers()
        {
            ew.StopAllWatchers();
            ew = null;
        }

        private void StopButton_Click(object sender, EventArgs e)
        {
            try
            {
                InitializeStartStop();
                textBox1.BackColor = Color.White;
                StopAllWatchers();
            }
            catch
            {
                StopButton.Enabled = true;
            }
        }

        private void DisplayResult(string result)
        {
            sbOutText.Append(string.Format("{0}{1}",result,Environment.NewLine));
            textBox1.Text = sbOutText.ToString();
        }

        private void saveToFileMenuItem_Click(object sender, EventArgs e)
        {
            DialogResult dr = saveFileDialog.ShowDialog();
            if (dr == System.Windows.Forms.DialogResult.OK)
            {
                File.AppendAllText(saveFileDialog.FileName, sbOutText.ToString());
            }

        }

        private void selectAllMenuItem_Click(object sender, EventArgs e)
        {
            textBox1.SelectAll();
            textBox1.Focus();
        }

        private void copyMenuItem_Click(object sender, EventArgs e)
        {
            Clipboard.SetText(sbOutText.ToString());
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (ew != null)
            {
                ew.StopAllWatchers();
                ew = null;
            }
        }

        private void ExitButton_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        //Сохранение
        private void сохранитьФайлToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveFile1 = new SaveFileDialog();
            saveFile1.DefaultExt = "*.txt";
            saveFile1.Filter = "Test files|*.txt";
            if (saveFile1.ShowDialog() == System.Windows.Forms.DialogResult.OK &&
                saveFile1.FileName.Length > 0)
            {
                using (StreamWriter sw = new StreamWriter(saveFile1.FileName, true))
                {
                    sw.WriteLine(textBox1.Text);
                    sw.Close();
                }
            }
        }
        //Очистка
        private void очиститьФайлToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if(textBox1.Text != "")
            {
                textBox1.Text = "";
            }
            else
            {
                MessageBox.Show("Нечего очищать");
            }
            
        }
        //О программе
        private void оПрограммеToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("'Программа для отслеживания действий пользователя'.\n Выполнена учащимся группы П-13 Марзаном Александром.");
        }

        private void печатьToolStripMenuItem_Click(object sender, EventArgs e)
        {
                PrintDocument printDocument = new PrintDocument();
                printDocument.PrintPage += PrintDocumentOnPrintPage;
                printDocument.Print();
        }
        private void PrintDocumentOnPrintPage(object sender, PrintPageEventArgs e)
        {
            e.Graphics.DrawString(this.textBox1.Text, this.textBox1.Font, Brushes.Black, 10, 25);
        }

        private void сохранитьИВыйтиToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveFile1 = new SaveFileDialog();
            saveFile1.DefaultExt = "*.txt";
            saveFile1.Filter = "Test files|*.txt";
            if (saveFile1.ShowDialog() == System.Windows.Forms.DialogResult.OK &&
                saveFile1.FileName.Length > 0)
            {
                using (StreamWriter sw = new StreamWriter(saveFile1.FileName, true))
                {
                    sw.WriteLine(textBox1.Text);
                    sw.Close();
                }
            }
            this.Close();
        }

        private void выйтиБезСохраненияToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void справкаToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                System.Diagnostics.Process.Start(@"/help.chm");
            }
            catch (Exception error)
            {
                MessageBox.Show(error.Message, "Ошибка: ");
            }
        }

        private void MainForm_Load(object sender, EventArgs e)
        {

        }
    }
}
