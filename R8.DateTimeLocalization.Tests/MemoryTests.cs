using System.Diagnostics;
using System.Runtime.InteropServices;
using FluentAssertions;
using R8.DateTimeLocalization.Tests.Timezones;
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
        LocalTimezone.Mappings.Add<IranTimezone>();
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
            var tzdtSize = Marshal.SizeOf<TimezoneDateTime>();
            _outputHelper.WriteLine($"DateTime size: {dtSize}");
            _outputHelper.WriteLine($"DateTimeLocal size: {tzdtSize}");

            tzdtSize.Should().Be(16);
        }
    }

    [Fact]
    public void AddYears_AddMinutes_AddSeconds_UTC()
    {
        const int iterations = 1_000_000;
        var dt = new DateTime(2023, 10, 1);
        var tzdt_bk = new TimezoneDateTime(2023, 10, 1, LocalTimezone.Utc);
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
        var tzdt_bk = new TimezoneDateTime(2023, 10, 1, LocalTimezone.Utc).WithTimezone(LocalTimezone.Get("Asia/Tehran"));
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
        var tzdt = new TimezoneDateTime(2023, 10, 1, LocalTimezone.Utc);

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
                tzdt = new TimezoneDateTime(2023, 10, 1, LocalTimezone.Utc);

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
        var tzdt_bk = new TimezoneDateTime(2023, 10, 1, LocalTimezone.Utc).WithTimezone(LocalTimezone.Get("Asia/Tehran"));
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
        var tzdt = new TimezoneDateTime(2023, 10, 1, LocalTimezone.Utc);

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
        var tzdt = new TimezoneDateTime(2023, 10, 1, LocalTimezone.Utc).WithTimezone(LocalTimezone.Get("Asia/Tehran"));

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
        var tzdt = new TimezoneDateTime(2023, 10, 1, LocalTimezone.Utc);

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
        var tzdt = new TimezoneDateTime(2023, 10, 1, LocalTimezone.Utc).WithTimezone(LocalTimezone.Get("Asia/Tehran"));

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
    public void GetStartOfNextWeek_Tehran()
    {
        const int iterations = 1_000_000;
        var tzdt_bk = new TimezoneDateTime(2023, 10, 1, LocalTimezone.Utc).WithTimezone(LocalTimezone.Get("Asia/Tehran"));
        var tzdt = tzdt_bk;

        _outputHelper.WriteLine($"Iterations: {iterations}");

        var stopWatch = new Stopwatch();
        stopWatch.Start();
        for (var i = 0; i < iterations; i++)
        {
            if (i % 1000 == 0)
                tzdt = tzdt_bk;

            tzdt = tzdt.GetStartOfNextWeek();
        }

        stopWatch.Stop();
        _outputHelper.WriteLine($"DateTimeLocal GetStartOfNextWeek: {stopWatch.ElapsedMilliseconds} ms");

        Assert.True(true);
    }

    [Fact]
    public void GetStartOfNextMonth_Tehran()
    {
        const int iterations = 1_000_000;
        var tzdt_bk = new TimezoneDateTime(2023, 10, 1, LocalTimezone.Utc).WithTimezone(LocalTimezone.Get("Asia/Tehran"));
        var tzdt = tzdt_bk;

        _outputHelper.WriteLine($"Iterations: {iterations}");

        var stopWatch = new Stopwatch();
        stopWatch.Start();
        for (var i = 0; i < iterations; i++)
        {
            if (i % 1000 == 0)
                tzdt = tzdt_bk;

            tzdt = tzdt.GetStartOfNextMonth();
        }

        stopWatch.Stop();
        _outputHelper.WriteLine($"DateTimeLocal GetStartOfNextMonth: {stopWatch.ElapsedMilliseconds} ms");

        Assert.True(true);
    }

    [Fact]
    public void GetStartOfWeek_Tehran()
    {
        const int iterations = 1_000_000;
        var tzdt_bk = new TimezoneDateTime(2023, 10, 1, LocalTimezone.Utc).WithTimezone(LocalTimezone.Get("Asia/Tehran"));
        var tzdt = tzdt_bk;

        _outputHelper.WriteLine($"Iterations: {iterations}");

        var stopWatch = new Stopwatch();
        stopWatch.Start();
        for (var i = 0; i < iterations; i++)
        {
            if (i % 1000 == 0)
                tzdt = tzdt_bk;

            tzdt = tzdt.GetStartOfWeek();
        }

        stopWatch.Stop();
        _outputHelper.WriteLine($"DateTimeLocal GetStartOfWeek: {stopWatch.ElapsedMilliseconds} ms");

        Assert.True(true);
    }

    [Fact]
    public void GetStartOfNextDay_Tehran()
    {
        const int iterations = 1_000_000;
        var tzdt_bk = new TimezoneDateTime(2023, 10, 1, LocalTimezone.Utc).WithTimezone(LocalTimezone.Get("Asia/Tehran"));
        var tzdt = tzdt_bk;

        _outputHelper.WriteLine($"Iterations: {iterations}");

        var stopWatch = new Stopwatch();
        stopWatch.Start();
        for (var i = 0; i < iterations; i++)
        {
            if (i % 1000 == 0)
                tzdt = tzdt_bk;

            tzdt = tzdt.GetStartOfNextDay();
        }

        stopWatch.Stop();
        _outputHelper.WriteLine($"DateTimeLocal GetStartOfNextDay: {stopWatch.ElapsedMilliseconds} ms");

        Assert.True(true);
    }
}