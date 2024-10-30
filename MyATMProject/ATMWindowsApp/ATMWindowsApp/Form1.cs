using ATMClassLibrary;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ATMWindowsApp
{
    public partial class Form1 : Form
    {
        private Bank bank;
        private Account currentAccount;
        public Form1()
        {
            InitializeComponent();
            bank = new Bank("MyBank");

            // Додай тестові аккаунти
            bank.Accounts.Add(new Account("123456", "John Doe", 10000, "1234"));
            bank.Accounts.Add(new Account("654321", "Jane Doe", 250, "4321"));
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        public void SendEmail(string to, string subject, string body)
        {
            try
            {
                var smtpClient = new SmtpClient("smtp.gmail.com")
                {
                    Port = 587,
                    UseDefaultCredentials = false,
                    Credentials = new NetworkCredential("email@gmail.com", "pass"),
                    EnableSsl = true,
                    DeliveryMethod = SmtpDeliveryMethod.Network
                };

                smtpClient.Send("bank@gmail.com", to, subject, body);
                MessageBox.Show("Повідомлення надіслано.");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Не вдалося надіслати повідомлення: {ex.Message}");
            }
        }


        private void btnAuthenticate_Click(object sender, EventArgs e)
        {
            string cardNumber = txtCardNumber.Text;
            string pinCode = txtPinCode.Text;

            currentAccount = bank.FindAccountByCardNumber(cardNumber);

            if (currentAccount != null && currentAccount.Authenticate(cardNumber, pinCode))
            {
                MessageBox.Show("Аутентифікація успішна!");
            }
            else
            {
                MessageBox.Show("Невірний номер картки або PIN-код.");
            }
        }

        private void btnViewBalance_Click(object sender, EventArgs e)
        {
            if (currentAccount != null)
            {
                MessageBox.Show($"Баланс: {currentAccount.Balance} грн.");
            }
            else
            {
                MessageBox.Show("Необхідно увійти до системи.");
            }
        }

        private void btnDeposit_Click(object sender, EventArgs e)
        {
            if (currentAccount != null)
            {
                decimal amount;
                if (decimal.TryParse(txtAmount.Text, out amount) && amount > 0)
                {
                    currentAccount.Deposit(amount);
                    MessageBox.Show($"Поповнено на {amount} грн. Поточний баланс: {currentAccount.Balance} грн.");
                    SendEmail(currentAccount.OwnerName + "@example.com", "Поповнення рахунку", $"Ваш рахунок поповнено на {amount} грн.");
                }
                else
                {
                    MessageBox.Show("Введіть коректну суму для поповнення.");
                }
            }
            else
            {
                MessageBox.Show("Спочатку увійдіть до системи.");
            }
        }

        private void btnWithdraw_Click(object sender, EventArgs e)
        {
            if (currentAccount != null)
            {
                decimal amount;
                if (decimal.TryParse(txtAmount.Text, out amount) && amount > 0)
                {
                    if (currentAccount.Withdraw(amount))
                    {
                        MessageBox.Show($"Знято {amount} грн. Поточний баланс: {currentAccount.Balance} грн.");
                        SendEmail(currentAccount.OwnerName + "@example.com", "Зняття коштів", $"Ваш рахунок знято на {amount} грн.");
                    }
                    else
                    {
                        MessageBox.Show("Недостатньо коштів на рахунку.");
                    }
                }
                else
                {
                    MessageBox.Show("Введіть коректну суму для зняття.");
                }
            }
            else
            {
                MessageBox.Show("Спочатку увійдіть до системи.");
            }
        }

        private void btnTransfer_Click(object sender, EventArgs e)
        {
            try
            {
                if (currentAccount != null)
                {
                    decimal amount;
                    string recipientCardNumber = txtRecipientCardNumber.Text;

                    if (decimal.TryParse(txtTransferAmount.Text, out amount) && amount > 0)
                    {
                        Account recipientAccount = bank.GetAccountByCardNumber(recipientCardNumber);
                        if (recipientAccount != null)
                        {
                            currentAccount.Withdraw(amount);
                            recipientAccount.Deposit(amount);
                            MessageBox.Show($"Переказано {amount} грн. на рахунок {recipientCardNumber}. Ваш поточний баланс: {currentAccount.Balance} грн.");
                            SendEmail(recipientAccount.OwnerName + "@example.com", "Переказ коштів", $"Вам надійшло {amount} грн. від {currentAccount.OwnerName}.");
                        }
                        else
                        {
                            MessageBox.Show("Картка одержувача не знайдена.");
                        }
                    }
                    else
                    {
                        MessageBox.Show("Введіть коректну суму для переказу.");
                    }
                }
                else
                {
                    MessageBox.Show("Спочатку увійдіть до системи.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Сталася помилка: {ex.Message}");
            }
        }

    }
}
