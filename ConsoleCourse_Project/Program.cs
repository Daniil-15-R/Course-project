using System;
using System.Collections.Generic;
using System.Linq;

namespace ConsoleCourse_Project
{
    class Program
    {
        private static readonly DogShelter shelter = new DogShelter();
        private static readonly EmployeeRegistry employees = new EmployeeRegistry();
        private static readonly AccountingSystem accounting = new AccountingSystem();

        static void Main(string[] args)
        {
            InitializeTestData();
            RunApplication();
        }

        static void InitializeTestData()
        {
            shelter.AddDog(new Dog("Барсик", 3, "Male", "Дружелюбный, любит детей"));
            shelter.AddDog(new Dog("Лайма", 5, "Female", "Охранная собака"));
            shelter.AddDog(new Dog("Рекс", 2, "Male", "Активный, нуждается в дрессировке"));

            employees.AddEmployee(new Employee("Иванов Иван Иванович", "+79001234567", "ivanov@mail.ru"));
            employees.AddEmployee(new Employee("Петрова Мария Сергеевна", "+79007654321", "petrova@mail.ru"));

            accounting.AddRecord(new AccountingRecord(new DateTime(2023, 01, 01), 50000, 20000, 10000));
            accounting.AddRecord(new AccountingRecord(new DateTime(2023, 02, 01), 55000, 22000, 11000));
        }

        static void RunApplication()
        {
            var menu = new MenuSystem();
            menu.AddOption("Просмотр списка собак", () => shelter.DisplayDogs());
            menu.AddOption("Добавить новую собаку", shelter.AddNewDogInteractive);
            menu.AddOption("Просмотр сотрудников", () => employees.DisplayEmployees());
            menu.AddOption("Финансовый отчет", () => accounting.DisplayFinancialReport());
            menu.AddOption("Выход", () => Environment.Exit(0));

            menu.Run("=== Фонд помощи собакам 'Храбрые Сердцем' ===");
        }
    }

    class MenuSystem
    {
        private readonly List<(string text, Action action)> options = new List<(string, Action)>();

        public void AddOption(string text, Action action)
        {
            options.Add((text, action));
        }

        public void Run(string header)
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine(header);

                for (int i = 0; i < options.Count; i++)
                {
                    Console.WriteLine($"{i + 1}. {options[i].text}");
                }

                Console.Write("Выберите действие: ");
                if (int.TryParse(Console.ReadLine(), out int choice) && choice > 0 && choice <= options.Count)
                {
                    try
                    {
                        options[choice - 1].action.Invoke();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Ошибка: {ex.Message}");
                    }
                }
                else
                {
                    Console.WriteLine("Неверный выбор!");
                }

                Console.WriteLine("\nНажмите любую клавишу для продолжения...");
                Console.ReadKey();
            }
        }
    }

    class DogShelter
    {
        private readonly List<Dog> dogs = new List<Dog>();

        public void AddDog(Dog dog)
        {
            dogs.Add(dog);
        }

        public void DisplayDogs()
        {
            Console.WriteLine("\n=== Список собак ===");
            Console.WriteLine("{0,-10} {1,-5} {2,-8} {3,-30}", "Кличка", "Возраст", "Пол", "Доп. информация");
            Console.WriteLine(new string('-', 60));

            foreach (var dog in dogs.OrderBy(d => d.Nickname))
            {
                Console.WriteLine(dog);
            }
        }

        public void AddNewDogInteractive()
        {
            Console.WriteLine("\n=== Добавление новой собаки ===");

            string name = GetInput<string>("Кличка: ", x => !string.IsNullOrWhiteSpace(x));
            int age = GetInput<int>("Возраст: ",
                x => int.TryParse(x, out int result) && result > 0,
                x => int.Parse(x));
            string gender = GetInput<string>("Пол (Male/Female): ",
                x => x.Equals("Male", StringComparison.OrdinalIgnoreCase) ||
                     x.Equals("Female", StringComparison.OrdinalIgnoreCase));
            string info = GetInput<string>("Доп. информация: ", x => true);

            AddDog(new Dog(name, age, gender, info));
            Console.WriteLine($"\nСобака {name} успешно зарегистрирована!");
        }

        private T GetInput<T>(string prompt, Func<string, bool> validator, Func<string, T> converter = null)
        {
            while (true)
            {
                Console.Write(prompt);
                string input = Console.ReadLine();

                if (validator(input))
                {
                    if (converter != null)
                    {
                        return converter(input);
                    }
                    return (T)Convert.ChangeType(input, typeof(T));
                }
                Console.WriteLine("Некорректный ввод. Пожалуйста, попробуйте снова.");
            }
        }
    }

    class EmployeeRegistry
    {
        private readonly List<Employee> employees = new List<Employee>();

        public void AddEmployee(Employee employee)
        {
            employees.Add(employee);
        }

        public void DisplayEmployees()
        {
            Console.WriteLine("\n=== Список сотрудников ===");
            Console.WriteLine("{0,-25} {1,-15} {2,-20}", "ФИО", "Телефон", "Email");
            Console.WriteLine(new string('-', 65));

            foreach (var emp in employees.OrderBy(e => e.FIO))
            {
                Console.WriteLine(emp);
            }
        }
    }

    class AccountingSystem
    {
        private readonly List<AccountingRecord> records = new List<AccountingRecord>();

        public void AddRecord(AccountingRecord record)
        {
            records.Add(record);
        }

        public void DisplayFinancialReport()
        {
            Console.WriteLine("\n=== Финансовый отчет ===");
            Console.WriteLine("{0,-12} {1,-10} {2,-12} {3,-10} {4,-10}",
                "Дата", "Зарплата", "Ком. услуги", "Налоги", "Итого");
            Console.WriteLine(new string('-', 60));

            foreach (var record in records.OrderBy(r => r.Date))
            {
                Console.WriteLine(record);
            }
        }
    }

    class Dog
    {
        public string Nickname { get; }
        public int Age { get; }
        public string Gender { get; }
        public string AdditionalInformation { get; }

        public Dog(string nickname, int age, string gender, string additionalInformation)
        {
            Nickname = nickname;
            Age = age;
            Gender = gender;
            AdditionalInformation = additionalInformation;
        }

        public override string ToString() =>
            $"{Nickname,-10} {Age,-5} {Gender,-8} {AdditionalInformation,-30}";
    }

    class Employee
    {
        public string FIO { get; }
        public string Phone { get; }
        public string Email { get; }

        public Employee(string fio, string phone, string email)
        {
            FIO = fio;
            Phone = phone;
            Email = email;
        }

        public override string ToString() =>
            $"{FIO,-25} {Phone,-15} {Email,-20}";
    }

    class AccountingRecord
    {
        public DateTime Date { get; }
        public decimal Salary { get; }
        public decimal Utilities { get; }
        public decimal Taxes { get; }
        public decimal Total => Salary + Utilities + Taxes;

        public AccountingRecord(DateTime date, decimal salary, decimal utilities, decimal taxes)
        {
            Date = date;
            Salary = salary;
            Utilities = utilities;
            Taxes = taxes;
        }

        public override string ToString() =>
            $"{Date:dd.MM.yyyy} {Salary,-10} {Utilities,-12} {Taxes,-10} {Total,-10}";
    }
}