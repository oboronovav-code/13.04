using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;

namespace _04._05._2026
{
    public partial class Form1 : Form
    {
        private string correctPin = "1234";
        private decimal balance = 1000.00m;
        private int attempts = 0;
        private string filePath = "atm_history.txt";

        public Form1()
        {
            InitializeComponent();

            panel2.Visible = false;
            panel1.Visible = true;

            textBox1.PasswordChar = '*';

            LoadHistoryFromFile();

            panel2.Location = panel1.Location;
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (textBox1.Text == correctPin)
            {
                panel1.Visible = false;
                panel2.Visible = true;
                attempts = 0;
                AddRecord("Вхід у систему успішний");
            }
            else
            {
                attempts++;
                if (attempts >= 3)
                {
                    MessageBox.Show("Картку заблоковано!");
                    button1.Enabled = false;
                }
                else
                {
                    MessageBox.Show($"Невірний код. Спроб: {3 - attempts}");
                }
            }
            textBox1.Clear();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (decimal.TryParse(textBox2.Text, out decimal amount) && amount > 0)
            {
                if (amount <= balance)
                {
                    balance -= amount;
                    AddRecord($"Зняття: -{amount} грн. Залишок: {balance} грн.");
                    MessageBox.Show("Операція успішна!");
                }
                else
                {
                    MessageBox.Show("Недостатньо коштів!");
                }
            }
            textBox2.Clear();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (decimal.TryParse(textBox3.Text, out decimal amount) && amount > 0)
            {
                balance += amount;
                AddRecord($"Поповнення: +{amount} грн. Залишок: {balance} грн.");
                MessageBox.Show("Рахунок поповнено!");
            }
            textBox3.Clear();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            MessageBox.Show($"Ваш баланс: {balance} грн.");
            AddRecord("Перегляд балансу");
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void button5_Click(object sender, EventArgs e)
        {
            listBox1.Focus();
            MessageBox.Show("Вся історія відображена у списку праворуч.");
        }

        private void AddRecord(string action)
        {
            string time = DateTime.Now.ToString("HH:mm:ss");
            string entry = $"[{time}] {action}";

            // Додаємо в початок списку (щоб нові події були зверху)
            listBox1.Items.Insert(0, entry);

            // Дописуємо у файл
            try
            {
                File.AppendAllLines(filePath, new[] { entry });
            }
            catch { /* Ігноруємо помилки доступу до файлу */ }
        }

        private void LoadHistoryFromFile()
        {
            if (File.Exists(filePath))
            {
                string[] lines = File.ReadAllLines(filePath);
                foreach (string line in lines)
                {
                    listBox1.Items.Add(line);
                }
            }
        }
    }
}
