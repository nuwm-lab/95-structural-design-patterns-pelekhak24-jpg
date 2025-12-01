using System;

namespace TractorAdapterLab
{
    // --- Target: Інтерфейс, який очікує клієнтський код ---
    // (Гусеничний транспорт)
    public interface ITrackedVehicle
    {
        void MoveOnTracks();
        void SpinOnSpot(); // Гусенична техніка може розвертатися на місці
    }

    // --- Adaptee: Клас, який потрібно адаптувати ---
    // (Колісний трактор)
    public class WheeledTractor
    {
        // Дотримання інкапсуляції: приватні поля, доступ через властивості або методи
        private string _modelName;
        private double _speed;

        public WheeledTractor(string modelName)
        {
            _modelName = modelName;
            _speed = 0;
        }

        public string ModelName => _modelName; // Властивість тільки для читання

        // Специфічний метод для колісного транспорту
        public void DriveWithWheels()
        {
            _speed = 15.0;
            Console.WriteLine($"[Колісний трактор {_modelName}]: Їдемо за допомогою коліс зі швидкістю {_speed} км/год.");
        }

        // Колісний трактор не розвертається на місці, він робить дугу
        public void TurnSteeringWheel()
        {
            Console.WriteLine($"[Колісний трактор {_modelName}]: Повертаємо кермо для повороту по дузі.");
        }
    }

    // --- Adapter: Адаптер, що перетворює Колісний на Гусеничний ---
    public class WheeledToTrackedAdapter : ITrackedVehicle
    {
        private readonly WheeledTractor _wheeledTractor;

        // Впровадження залежності через конструктор
        public WheeledToTrackedAdapter(WheeledTractor tractor)
        {
            _wheeledTractor = tractor ?? throw new ArgumentNullException(nameof(tractor));
        }

        // Реалізація методу руху гусеницями через рух коліс
        public void MoveOnTracks()
        {
            Console.WriteLine(">> Адаптер: Перетворення команди руху гусениць на обертання коліс...");
            _wheeledTractor.DriveWithWheels();
        }

        // Адаптація розвороту
        public void SpinOnSpot()
        {
            Console.WriteLine(">> Адаптер: Імітація розвороту на місці (блокуванням коліс одного борту)...");
            // У реальності адаптер тут би виконував складну логіку керування колесами
            _wheeledTractor.TurnSteeringWheel();
            Console.WriteLine(">> (Технічно колісний трактор виконав звичайний поворот, але клієнт 'бачить' це як маневр)");
        }
    }

    // --- Client: Код, який використовує ці класи ---
    class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;

            Console.WriteLine("=== Лабораторна робота: Патерн Адаптер ===\n");

            // 1. Створюємо старий колісний трактор (Adaptee)
            WheeledTractor myWheeledTractor = new WheeledTractor("MTZ-80");

            // 2. Створюємо адаптер, щоб використовувати його як гусеничний
            ITrackedVehicle convertedTractor = new WheeledToTrackedAdapter(myWheeledTractor);

            // 3. Використовуємо методи гусеничного інтерфейсу
            Console.WriteLine("Спроба поїхати як на гусеницях:");
            convertedTractor.MoveOnTracks();

            Console.WriteLine("\nСпроба розвернутися як танк:");
            convertedTractor.SpinOnSpot();

            Console.ReadKey();
        }
    }
}