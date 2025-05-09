using System;
using System.Collections.Generic;

namespace ConsoleCourse_Project
{
    class Program
    {
        // Тестовые данные
        static List<Dog> dogs = new List<Dog>
        {
            new Dog { Nickname = "Барсик", Age = 3, Gender = "Male", AdditionalInformation = "Дружелюбный, любит детей" },
            new Dog { Nickname = "Лайма", Age = 5, Gender = "Female", AdditionalInformation = "Охранная собака" },
            new Dog { Nickname = "Рекс", Age = 2, Gender = "Male", AdditionalInformation = "Активный, нуждается в дрессировке" }
        };

        static List<Employee> employees = new List<Employee>
        {
            new Employee { FIO = "Иванов Иван Иванович", Phone = "+79001234567", Email = "ivanov@mail.ru" },
            new Employee { FIO = "Петрова Мария Сергеевна", Phone = "+79007654321", Email = "petrova@mail.ru" }
        };

        static List<AccountingRecord> accountingRecords = new List<AccountingRecord>
        {
            new AccountingRecord { Date = new DateTime(2023, 01, 01), Salary = 50000, Utilities = 20000, Taxes = 10000 },
            new AccountingRecord { Date = new DateTime(2023, 02, 01), Salary = 55000, Utilities = 22000, Taxes = 11000 }
        };

        static void Main(string[] args)
        {
            try
            {
                ShowMainMenu();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка: {ex.Message}");
                Console.ReadKey();
            }
        }

        static void ShowMainMenu()
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("=== Фонд помощи собакам 'Храбрые Сердцем' ===");
                Console.WriteLine("1. Просмотр списка собак");
                Console.WriteLine("2. Добавить новую собаку");
                Console.WriteLine("3. Просмотр сотрудников");
                Console.WriteLine("4. Финансовый отчет");
                Console.WriteLine("5. Выход");
                Console.Write("Выберите действие: ");

                var choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        ShowDogs();
                        break;
                    case "2":
                        AddNewDog();
                        break;
                    case "3":
                        ShowEmployees();
                        break;
                    case "4":
                        ShowAccounting();
                        break;
                    case "5":
                        return;
                    default:
                        Console.WriteLine("Неверный выбор!");
                        break;
                }

                Console.WriteLine("\nНажмите любую клавишу для продолжения...");
                Console.ReadKey();
            }
        }

        static void ShowDogs()
        {
            Console.WriteLine("\n=== Список собак ===");

            Console.WriteLine("{0,-10} {1,-5} {2,-8} {3,-30}", "Кличка", "Возраст", "Пол", "Доп. информация");
            Console.WriteLine(new string('-', 60));

            foreach (var dog in dogs)
            {
                Console.WriteLine("{0,-10} {1,-5} {2,-8} {3,-30}",
                    dog.Nickname,
                    dog.Age,
                    dog.Gender,
                    dog.AdditionalInformation);
            }
        }

        static void AddNewDog()
        {
            Console.WriteLine("\n=== Добавление новой собаки ===");

            Console.Write("Кличка: ");
            var name = Console.ReadLine();

            Console.Write("Возраст: ");
            var age = int.Parse(Console.ReadLine());

            Console.Write("Пол (Male/Female): ");
            var gender = Console.ReadLine();

            Console.Write("Доп. информация: ");
            var info = Console.ReadLine();

            dogs.Add(new Dog
            {
                Nickname = name,
                Age = age,
                Gender = gender,
                AdditionalInformation = info
            });

            Console.WriteLine($"\nСобака {name} успешно зарегистрирована!");
        }

        static void ShowEmployees()
        {
            Console.WriteLine("\n=== Список сотрудников ===");

            Console.WriteLine("{0,-25} {1,-15} {2,-20}", "ФИО", "Телефон", "Email");
            Console.WriteLine(new string('-', 65));

            foreach (var emp in employees)
            {
                Console.WriteLine("{0,-25} {1,-15} {2,-20}",
                    emp.FIO,
                    emp.Phone,
                    emp.Email);
            }
        }

        static void ShowAccounting()
        {
            Console.WriteLine("\n=== Финансовый отчет ===");

            Console.WriteLine("{0,-12} {1,-10} {2,-12} {3,-10} {4,-10}",
                "Дата", "Зарплата", "Ком. услуги", "Налоги", "Итого");
            Console.WriteLine(new string('-', 60));

            foreach (var record in accountingRecords)
            {
                Console.WriteLine("{0,-12:dd.MM.yyyy} {1,-10} {2,-12} {3,-10} {4,-10}",
                    record.Date,
                    record.Salary,
                    record.Utilities,
                    record.Taxes,
                    record.Total);
            }
        }
    }

    // Модели данных
    class Dog
    {
        public string Nickname { get; set; }
        public int Age { get; set; }
        public string Gender { get; set; }
        public string AdditionalInformation { get; set; }
    }

    class Employee
    {
        public string FIO { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
    }

    class AccountingRecord
    {
        public DateTime Date { get; set; }
        public decimal Salary { get; set; }
        public decimal Utilities { get; set; }
        public decimal Taxes { get; set; }
        public decimal Total => Salary + Utilities + Taxes;
    }
}