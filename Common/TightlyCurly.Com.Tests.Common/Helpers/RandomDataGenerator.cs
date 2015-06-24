using System;
using System.Text;
using TightlyCurly.Com.Common.Extensions;

namespace TightlyCurly.Com.Tests.Common.Helpers
{
    public class RandomDataGenerator : IDataGenerator
    {
        private static Random _random;
        private static readonly object Lock = new object();
        private static Random Random
        {
            get
            {
                if (_random.IsNull())
                {
                    _random = new Random();
                }

                return _random;
            }
        }

        public string GenerateString(int numberOfCharacters = 0)
        {
            if (numberOfCharacters > 0)
            {
                var builder = new StringBuilder();

                var random = new Random();

                for (var index = 0; index < numberOfCharacters; index++)
                {
                    builder.Append((char) random.Next(1, 26));
                }

                return builder.ToString();
            }

            return Guid.NewGuid().ToString("N");
        }

        public string GenerateEmailAddress()
        {
            return "{0}@{1}.com".FormatString(GenerateString(), GenerateString());
        }

        public Uri GenerateUri()
        {
            return new Uri("http://{0}.com".FormatString(GenerateString()));
        }

        public int GenerateInteger(int minValue = 0, int maxValue = Int32.MaxValue)
        {
           // var random = new Random();
            int value;

            lock (Lock)
            {
                value = Random.Next(minValue, maxValue);
            }

            return value;
        }

        public long GenerateLong(long minValue = 0, long maxValue = Int64.MaxValue)
        {
            double value;

            lock (Lock)
            {
                value = Random.NextDouble();
            }

            return Convert.ToInt64(minValue + (maxValue - minValue) * value);
        }

        public double GenerateDouble(double minValue = 0, double maxValue = Double.MaxValue)
        {
            double value;

            lock (Lock)
            {
                value = Random.NextDouble();
            }
            //var random = new Random();

            return minValue + (maxValue - minValue) * value;
        }

        public float GenerateFloat(float minValue = 0, float maxValue = Single.MaxValue)
        {
            //var random = new Random();
            double value;

            lock (Lock)
            {
                value = Random.NextDouble();
            }

            return Convert.ToSingle(minValue + (maxValue - minValue) * value);
        }

        public DateTime GenerateDateTime(int month = 0, int day = 0, int year = 0)
        {
            if (month <= 0)
            {
                month = GenerateInteger(1, 12);
            }

            // TODO: Revisit and rework for leap years.
            if (day <= 0)
            {
                day = GenerateInteger(1, 28);
            }

            if (year <= 0)
            {
                year = DateTime.Now.AddYears(-2).Year;
            }

            return new DateTime(year, month, day);
        }

        public bool GenerateBoolean()
        {
            return GenerateInteger() % 2 == 0;
        }
    }
}
