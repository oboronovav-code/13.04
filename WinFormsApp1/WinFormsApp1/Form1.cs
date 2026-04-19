using System;
using System.IO;
using System.Collections.Generic;

namespace WinFormsApp1
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            CreateGrid();
            textBox1.Multiline = true;
            textBox1.ScrollBars = ScrollBars.Vertical;
        }

        private Random _random = new Random();

        private void button1_MouseEnter(object sender, EventArgs e)
        {
            int maxX = this.ClientSize.Width - button1.Width;
            int maxY = this.ClientSize.Height - button1.Height;

            int nextX = _random.Next(0, maxX);
            int nextY = _random.Next(0, maxY);

            button1.Location = new Point(nextX, nextY);
        }

        private void button6_Click(object sender, EventArgs e)
        {

        }

        bool turn = true;
        int turnCount = 0;

        private void CreateGrid()
        {
            // Створюємо сітку 3х3 програмно
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    Button b = new Button();
                    b.Size = new Size(45, 45);
                    b.Location = new Point(i * 45, j * 45);
                    b.Font = new Font("Arial", 14, FontStyle.Bold);
                    b.Click += Button_Click; // Спільний обробник
                    this.Controls.Add(b);
                }
            }
        }

        private void Button_Click(object sender, EventArgs e)
        {
            Button b = (Button)sender;
            if (turn)
                b.Text = "X";
            else
                b.Text = "O";

            turn = !turn;      // Передаємо хід
            b.Enabled = false;
            turnCount++;

            if (CheckWinner())
            {
                MessageBox.Show("Перемога!");
            }
        }

        private bool CheckWinner()
        {
            string[,] s = new string[3, 3];

            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    Control c = this.GetChildAtPoint(new Point(i * 45, j * 45));
                    if (c is Button btn)
                    {
                        s[i, j] = btn.Text;
                    }
                    else
                    {
                        s[i, j] = "";
                    }
                }
            }

            for (int i = 0; i < 3; i++)
            {
                // Рядки
                if (s[i, 0] != "" && s[i, 0] == s[i, 1] && s[i, 1] == s[i, 2]) return true;
                // Стовпці
                if (s[0, i] != "" && s[0, i] == s[1, i] && s[1, i] == s[2, i]) return true;
            }

            // Діагоналі
            if (s[0, 0] != "" && s[0, 0] == s[1, 1] && s[1, 1] == s[2, 2]) return true;
            if (s[0, 2] != "" && s[0, 2] == s[1, 1] && s[1, 1] == s[2, 0]) return true;

            return false;
        }

        private string _currentFilePath = ""; // Тут зберігатимемо шлях до відкритого файлу

        private void button2_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Filter = "Текстові файли (*.txt)|*.txt|Усі файли (*.*)|*.*";

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    _currentFilePath = openFileDialog.FileName;

                    // Читаємо весь текст із файлу і кладемо в TextBox
                    textBox1.Text = File.ReadAllText(_currentFilePath);

                    this.Text = "Редагування: " + Path.GetFileName(_currentFilePath);
                }
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            using (SaveFileDialog saveFileDialog = new SaveFileDialog())
            {
                saveFileDialog.Filter = "Текстові файли (*.txt)|*.txt";
                saveFileDialog.FileName = Path.GetFileName(_currentFilePath);

                if (saveFileDialog.ShowDialog() == DialogResult.OK)
                {
                    // Записуємо контент із TextBox у файл
                    File.WriteAllText(saveFileDialog.FileName, textBox1.Text);

                    _currentFilePath = saveFileDialog.FileName;
                    MessageBox.Show("Файл успішно збережено!", "Готово");
                }
            }
        }
    }
}


