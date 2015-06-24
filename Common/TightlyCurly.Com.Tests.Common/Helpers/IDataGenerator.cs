using System;

namespace TightlyCurly.Com.Tests.Common.Helpers
{
    public interface IDataGenerator
    {
        string GenerateString(int numberOfCharacters = 0);
        string GenerateEmailAddress();
        Uri GenerateUri();
        int GenerateInteger(int minValue = 0, int maxValue = Int32.MaxValue);
        long GenerateLong(long minValue = 0L, long maxValue = long.MaxValue);
        double GenerateDouble(double minValue = 0, double maxValue = Double.MaxValue);
        float GenerateFloat(float minValue = 0, float maxValue = float.MaxValue);
        DateTime GenerateDateTime(int month = 0, int day = 0, int year = 0);
        bool GenerateBoolean();
    }
}
