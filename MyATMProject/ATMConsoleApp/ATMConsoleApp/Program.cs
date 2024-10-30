using ATMClassLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace ATMConsoleApp
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Bank bank = new Bank("MyBank");
            bank.AddAccount(new Account("123456", "John Doe", 10000, "1234"));
            bank.AddAccount(new Account("654321", "Jane Doe", 250, "4321"));

            Console.WriteLine("Вітаємо в банкоматі!");

            Account currentAccount = null;

            while (true)
            {
                Console.WriteLine("1. Аутентифікація");
                Console.WriteLine("2. Вийти");
                Console.Write("Виберіть опцію: ");
                string option = Console.ReadLine();

                if (option == "1")
                {
                    Console.Write("Номер картки: ");
                    string cardNumber = Console.ReadLine();
                    Console.Write("PIN-код: ");
                    string pinCode = Console.ReadLine();

                    currentAccount = bank.FindAccountByCardNumber(cardNumber);

                    if (currentAccount != null && currentAccount.Authenticate(cardNumber, pinCode))
                    {
                        Console.WriteLine("Аутентифікація успішна!");
                        ShowAccountMenu(currentAccount, bank);
                    }
                    else
                    {
                        Console.WriteLine("Невірний номер картки або PIN-код.");
                    }
                }
                else if (option == "2")
                {
                    break; // Вихід з програми
                }
                else
                {
                    Console.WriteLine("Невірний вибір. Спробуйте ще раз.");
                }
            }
        }

        static void ShowAccountMenu(Account account, Bank bank)
        {
            while (true)
            {
                Console.WriteLine("\n1. Перегляд балансу");
                Console.WriteLine("2. Поповнити рахунок");
                Console.WriteLine("3. Зняти кошти");
                Console.WriteLine("4. Переказати кошти");
                Console.WriteLine("5. Вийти з облікового запису");
                Console.Write("Виберіть опцію: ");
                string option = Console.ReadLine();

                switch (option)
                {
                    case "1":
                        Console.WriteLine($"\nВаш баланс: {account.Balance} грн.");
                        break;
                    case "2":
                        Console.Write("Введіть суму для поповнення: ");
                        decimal depositAmount = decimal.Parse(Console.ReadLine());
                        account.Deposit(depositAmount);
                        SendEmail(account.OwnerName + "@example.com", "Поповнення рахунку", $"Ваш рахунок поповнено на {depositAmount} грн.");
                        Console.WriteLine($"Рахунок поповнено на {depositAmount} грн. Поточний баланс: {account.Balance} грн.");
                        break;
                    case "3":
                        Console.Write("Введіть суму для зняття: ");
                        decimal withdrawAmount = decimal.Parse(Console.ReadLine());
                        if (account.Withdraw(withdrawAmount))
                        {
                            SendEmail(account.OwnerName + "@example.com", "Зняття коштів", $"Ваш рахунок знято на {withdrawAmount} грн.");
                            Console.WriteLine($"Знято {withdrawAmount} грн. Поточний баланс: {account.Balance} грн.");
                        }
                        else
                        {
                            Console.WriteLine("Недостатньо коштів на рахунку.");
                        }
                        break;
                    case "4":
                        Console.Write("Введіть номер картки одержувача: ");
                        string recipientCardNumber = Console.ReadLine();
                        Console.Write("Введіть суму для переказу: ");
                        decimal transferAmount = decimal.Parse(Console.ReadLine());

                        Account recipientAccount = bank.FindAccountByCardNumber(recipientCardNumber);

                        if (recipientAccount != null)
                        {
                            if (account.Withdraw(transferAmount))
                            {
                                recipientAccount.Deposit(transferAmount);
                                Console.WriteLine($"Переказано {transferAmount} грн. на рахунок {recipientCardNumber}. Поточний баланс: {account.Balance} грн.");
                            }
                            else
                            {
                                Console.WriteLine("Недостатньо коштів для переказу.");
                            }
                        }
                        else
                        {
                            Console.WriteLine("Картка одержувача не знайдена.");
                        }
                        break;
                    case "5":
                        return; // Вихід з облікового запису
                    default:
                        Console.WriteLine("Невірний вибір. Спробуйте ще раз.");
                        break;
                }
            }
        }

        static void SendEmail(string to, string subject, string body)
        {
            try
            {
                var smtpClient = new SmtpClient("smtp.gmail.com")
                {
                    Port = 587,
                    UseDefaultCredentials = false,
                    Credentials = new NetworkCredential("ваша_електронна_пошта@gmail.com", "ваш_пароль_додатка"),
                    EnableSsl = true
                };

                smtpClient.Send("ваша_електронна_пошта@gmail.com", to, subject, body);
                Console.WriteLine("Повідомлення надіслано.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Не вдалося надіслати повідомлення: {ex.Message}");
            }
        }
    }
}
