using System.Diagnostics;
using System.Runtime.InteropServices;
using R8.DateTimeLocalization.Tests.TimezoneMappers;
using Xunit.Abstractions;

namespace R8.DateTimeLocalization.Tests;

public class MemoryTests : IAsyncLifetime
{
    private readonly ITestOutputHelper _outputHelper;

    public MemoryTests(ITestOutputHelper outputHelper)
    {
        _outputHelper = outputHelper;
    }

    public Task InitializeAsync()
    {
        LocalTimezone.Mappings.GetOrCreate<IranTimezone>();
        return Task.CompletedTask;
    }

    public Task DisposeAsync()
    {
        return Task.CompletedTask;
    }

    [Fact]
    public void SizeOf()
    {
        unsafe
        {
            var dtSize = sizeof(DateTime);
            var tzdtSize = Marshal.SizeOf<DateTimeLocal>();
            _outputHelper.WriteLine($"DateTime size: {dtSize}");
            _outputHelper.WriteLine($"DateTimeLocal size: {tzdtSize}");

            Assert.True(true);
        }
    }

    [Fact]
    public void AddYears_AddMinutes_AddSeconds_UTC()
    {
        const int iterations = 1_000_000;
        var dt = new DateTime(2023, 10, 1);
        var tzdt_bk = new DateTimeLocal(2023, 10, 1, LocalTimezone.Utc);
        var tzdt = tzdt_bk;

        _outputHelper.WriteLine($"Iterations: {iterations}");

        var stopWatch = new Stopwatch();
        stopWatch.Start();
        for (var i = 0; i < iterations; i++)
        {
            if (i % 1000 == 0)
                dt = new DateTime(2023, 10, 1);

            dt = dt.AddYears(1).AddMinutes(30).AddSeconds(39);
        }

        stopWatch.Stop();
        _outputHelper.WriteLine($"DateTime AddYears: {stopWatch.ElapsedMilliseconds} ms");

        stopWatch.Restart();
        for (var i = 0; i < iterations; i++)
        {
            if (i % 1000 == 0)
                tzdt = tzdt_bk;

            tzdt = tzdt.AddYears(1).AddMinutes(30).AddSeconds(39);
        }

        stopWatch.Stop();
        _outputHelper.WriteLine($"DateTimeLocal AddYears: {stopWatch.ElapsedMilliseconds} ms");

        Assert.True(true);
    }

    [Fact]
    public void AddYears_AddMinutes_AddSeconds_Tehran()
    {
        const int iterations = 1_000_000;
        var dt = new DateTime(2023, 10, 1);
        var tzdt_bk = new DateTimeLocal(2023, 10, 1, LocalTimezone.Utc).WithTimezone(LocalTimezone.GetOrCreate("Asia/Tehran"));
        var tzdt = tzdt_bk;

        _outputHelper.WriteLine($"Iterations: {iterations}");

        var stopWatch = new Stopwatch();
        stopWatch.Start();
        for (var i = 0; i < iterations; i++)
        {
            if (i % 1000 == 0)
                dt = new DateTime(2023, 10, 1);

            dt = dt.AddYears(1).AddMinutes(30).AddSeconds(39);
        }

        stopWatch.Stop();
        _outputHelper.WriteLine($"DateTime AddYears: {stopWatch.ElapsedMilliseconds} ms");

        stopWatch.Restart();
        for (var i = 0; i < iterations; i++)
        {
            if (i % 1000 == 0)
                tzdt = tzdt_bk;

            tzdt = tzdt.AddYears(1).AddMinutes(30).AddSeconds(39);
        }

        stopWatch.Stop();
        _outputHelper.WriteLine($"DateTimeLocal AddYears: {stopWatch.ElapsedMilliseconds} ms");

        Assert.True(true);
    }

    [Fact]
    public void AddYears_UTC()
    {
        const int iterations = 1_000_000;
        var dt = new DateTime(2023, 10, 1);
        var tzdt = new DateTimeLocal(2023, 10, 1, LocalTimezone.Utc);

        _outputHelper.WriteLine($"Iterations: {iterations}");

        var stopWatch = new Stopwatch();
        stopWatch.Start();
        for (var i = 0; i < iterations; i++)
        {
            if (i % 1000 == 0)
                dt = new DateTime(2023, 10, 1);

            dt = dt.AddYears(1);
        }

        stopWatch.Stop();
        _outputHelper.WriteLine($"DateTime AddYears: {stopWatch.ElapsedMilliseconds} ms");

        stopWatch.Restart();
        for (var i = 0; i < iterations; i++)
        {
            if (i % 1000 == 0)
                tzdt = new DateTimeLocal(2023, 10, 1, LocalTimezone.Utc);

            tzdt = tzdt.AddYears(1);
        }

        stopWatch.Stop();
        _outputHelper.WriteLine($"DateTimeLocal AddYears: {stopWatch.ElapsedMilliseconds} ms");

        Assert.True(true);
    }

    [Fact]
    public void AddYears_Tehran()
    {
        const int iterations = 1_000_000;
        var dt = new DateTime(2023, 10, 1);
        var tzdt_bk = new DateTimeLocal(2023, 10, 1, LocalTimezone.Utc).WithTimezone(LocalTimezone.GetOrCreate("Asia/Tehran"));
        var tzdt = tzdt_bk;

        _outputHelper.WriteLine($"Iterations: {iterations}");

        var stopWatch = new Stopwatch();
        stopWatch.Start();
        for (var i = 0; i < iterations; i++)
        {
            if (i % 1000 == 0)
                dt = new DateTime(2023, 10, 1);

            dt = dt.AddYears(1);
        }

        stopWatch.Stop();
        _outputHelper.WriteLine($"DateTime AddYears: {stopWatch.ElapsedMilliseconds} ms");

        stopWatch.Restart();
        for (var i = 0; i < iterations; i++)
        {
            if (i % 1000 == 0)
                tzdt = tzdt_bk;

            tzdt = tzdt.AddYears(1);
        }

        stopWatch.Stop();
        _outputHelper.WriteLine($"DateTimeLocal AddYears: {stopWatch.ElapsedMilliseconds} ms");

        Assert.True(true);
    }

    [Fact]
    public void GetYear_UTC()
    {
        const int iterations = 1_000_000;
        var dt = new DateTime(2023, 10, 1);
        var tzdt = new DateTimeLocal(2023, 10, 1, LocalTimezone.Utc);

        _outputHelper.WriteLine($"Iterations: {iterations}");

        var stopWatch = new Stopwatch();
        stopWatch.Start();
        for (var i = 0; i < iterations; i++)
        {
            var year = dt.Year;
        }

        stopWatch.Stop();
        _outputHelper.WriteLine($"DateTime GetYear: {stopWatch.ElapsedMilliseconds} ms");

        stopWatch.Restart();
        for (var i = 0; i < iterations; i++)
        {
            var year = tzdt.Year;
        }

        stopWatch.Stop();
        _outputHelper.WriteLine($"DateTimeLocal GetYear: {stopWatch.ElapsedMilliseconds} ms");

        Assert.True(true);
    }

    [Fact]
    public void GetYear_Tehran()
    {
        const int iterations = 1_000_000;
        var dt = new DateTime(2023, 10, 1);
        var tzdt = new DateTimeLocal(2023, 10, 1, LocalTimezone.Utc).WithTimezone(LocalTimezone.GetOrCreate("Asia/Tehran"));

        _outputHelper.WriteLine($"Iterations: {iterations}");

        var stopWatch = new Stopwatch();
        stopWatch.Start();
        for (var i = 0; i < iterations; i++)
        {
            var year = dt.Year;
        }

        stopWatch.Stop();
        _outputHelper.WriteLine($"DateTime GetYear: {stopWatch.ElapsedMilliseconds} ms");

        stopWatch.Restart();
        for (var i = 0; i < iterations; i++)
        {
            var year = tzdt.Year;
        }

        stopWatch.Stop();
        _outputHelper.WriteLine($"DateTimeLocal GetYear: {stopWatch.ElapsedMilliseconds} ms");

        Assert.True(true);
    }

    [Fact]
    public void GetMinute_UTC()
    {
        const int iterations = 1_000_000;
        var dt = new DateTime(2023, 10, 1);
        var tzdt = new DateTimeLocal(2023, 10, 1, LocalTimezone.Utc);

        _outputHelper.WriteLine($"Iterations: {iterations}");

        var stopWatch = new Stopwatch();
        stopWatch.Start();
        for (var i = 0; i < iterations; i++)
        {
            var year = dt.Minute;
        }

        stopWatch.Stop();
        _outputHelper.WriteLine($"DateTime GetMinute: {stopWatch.ElapsedMilliseconds} ms");

        stopWatch.Restart();
        for (var i = 0; i < iterations; i++)
        {
            var year = tzdt.Minute;
        }

        stopWatch.Stop();
        _outputHelper.WriteLine($"DateTimeLocal GetMinute: {stopWatch.ElapsedMilliseconds} ms");

        Assert.True(true);
    }

    [Fact]
    public void GetMinute_Tehran()
    {
        const int iterations = 1_000_000;
        var dt = new DateTime(2023, 10, 1);
        var tzdt = new DateTimeLocal(2023, 10, 1, LocalTimezone.Utc).WithTimezone(LocalTimezone.GetOrCreate("Asia/Tehran"));

        _outputHelper.WriteLine($"Iterations: {iterations}");

        var stopWatch = new Stopwatch();
        stopWatch.Start();
        for (var i = 0; i < iterations; i++)
        {
            var year = dt.Minute;
        }

        stopWatch.Stop();
        _outputHelper.WriteLine($"DateTime GetMinute: {stopWatch.ElapsedMilliseconds} ms");

        stopWatch.Restart();
        for (var i = 0; i < iterations; i++)
        {
            var year = tzdt.Minute;
        }

        stopWatch.Stop();
        _outputHelper.WriteLine($"DateTimeLocal GetMinute: {stopWatch.ElapsedMilliseconds} ms");

        Assert.True(true);
    }
}