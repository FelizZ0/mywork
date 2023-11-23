using System;
using System.IO;
using System.Diagnostics;
using System.Threading.Tasks;

class Program
{
    static async Task Main(string[] args)
    {
        start:

        Console.WriteLine("Введите количество чисел для записи:");
        int count = int.Parse(Console.ReadLine());
        if (count >= 1000)
        {

            Console.WriteLine("Введите интервал (минимальное значение) для случайных чисел:");
            double minValue = double.Parse(Console.ReadLine());

            Console.WriteLine("Введите интервал (максимальное значение) для случайных чисел:");
            double maxValue = double.Parse(Console.ReadLine());

            start2:

            Console.WriteLine("Введите количество замеров:");
            int measurementsCount = int.Parse(Console.ReadLine());
            if (measurementsCount > 0)
            {
                var stopwatch = new Stopwatch();
                double[] data = GenerateRandomData(count, minValue, maxValue);

                double minSyncTime = double.MaxValue;
                double maxSyncTime = double.MinValue;
                double totalSyncTime = 0;

                double minAsyncTime = double.MaxValue;
                double maxAsyncTime = double.MinValue;
                double totalAsyncTime = 0;

                for (int i = 0; i < measurementsCount; i++)
                {
                    Console.WriteLine($"Замер {i + 1}:");

                    // Синхронная запись и чтение
                    stopwatch.Start();
                    await WriteAndReadDataSynchronously(data);
                    stopwatch.Stop();
                    double syncTime = stopwatch.Elapsed.TotalMilliseconds;
                    Console.WriteLine($"Синхронно: {syncTime} мс");
                    totalSyncTime += syncTime;
                    minSyncTime = Math.Min(minSyncTime, syncTime);
                    maxSyncTime = Math.Max(maxSyncTime, syncTime);
                    stopwatch.Reset();


                    // Асинхронная запись и чтение
                    stopwatch.Start();
                    await WriteAndReadDataAsynchronously(data);
                    stopwatch.Stop();
                    double asyncTime = stopwatch.Elapsed.TotalMilliseconds;
                    Console.WriteLine($"Асинхронно: {asyncTime} мс");
                    totalAsyncTime += asyncTime;
                    minAsyncTime = Math.Min(minAsyncTime, asyncTime);
                    maxAsyncTime = Math.Max(maxAsyncTime, asyncTime);
                    stopwatch.Reset();
                }

                double avgSyncTime = totalSyncTime / measurementsCount;
                double avgAsyncTime = totalAsyncTime / measurementsCount;

                Console.WriteLine("\nСтатистика синхронного метода:");
                Console.WriteLine($"Среднее время: {avgSyncTime} мс");
                Console.WriteLine($"Минимальное время: {minSyncTime} мс");
                Console.WriteLine($"Максимальное время: {maxSyncTime} мс");

                Console.WriteLine("\nСтатистика асинхронного метода:");
                Console.WriteLine($"Среднее время: {avgAsyncTime} мс");
                Console.WriteLine($"Минимальное время: {minAsyncTime} мс");
                Console.WriteLine($"Максимальное время: {maxAsyncTime} мс");
            }
            else
            {
                Console.WriteLine("Кол-во замеров должно быть больше 0");
                goto start2;
            }


        }
        else
        {
            Console.WriteLine("Кол-во чисел должно быть не менее 1000");
            goto start;
        }

        

        Console.ReadLine();
    }

    static double[] GenerateRandomData(int count, double minValue, double maxValue)
    {
        Random random = new Random();
        double[] data = new double[count];
        for (int i = 0; i < count; i++)
        {
            data[i] = random.NextDouble() * (maxValue - minValue) + minValue;
        }
        return data;
    }

    static async Task WriteAndReadDataSynchronously(double[] data)
    {
        string filePath = "data.bin";

        try
        {
            using (var writer = new BinaryWriter(File.Open(filePath, FileMode.Create)))
            {
                foreach (var value in data)
                {
                    writer.Write(value);
                }
            }

            using (var reader = new BinaryReader(File.Open(filePath, FileMode.Open)))
            {
                while (reader.BaseStream.Position < reader.BaseStream.Length)
                {
                    double value = reader.ReadDouble();
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ошибка в синхронном методе: {ex.Message}");
        }
    }

    static async Task WriteAndReadDataAsynchronously(double[] data)
    {
        string filePath = "data_async.bin";

        try
        {
            using (var writer = new BinaryWriter(File.Open(filePath, FileMode.Create)))
            {
                foreach (var value in data)
                {
                    writer.Write(value);
                }
            }

            using (var reader = new BinaryReader(File.Open(filePath, FileMode.Open)))
            {
                while (reader.BaseStream.Position < reader.BaseStream.Length)
                {
                    double value = reader.ReadDouble();
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ошибка в асинхронном методе: {ex.Message}");
        }
    }
}
