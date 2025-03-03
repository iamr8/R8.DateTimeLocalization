using System.Text.Json;
using FluentAssertions;
using R8.DateTimeLocalization.Tests.Timezones;
using Xunit.Abstractions;

namespace R8.DateTimeLocalization.Tests;

public class TimezoneDateTimeJsonTests : IAsyncLifetime
{
    private readonly ITestOutputHelper _outputHelper;

    public TimezoneDateTimeJsonTests(ITestOutputHelper outputHelper)
    {
        _outputHelper = outputHelper;
    }

    public Task InitializeAsync()
    {
        LocalTimezone.Mappings.Add<IranTimezone>();
        LocalTimezone.Mappings.Add<TurkeyTimezone>();
        return Task.CompletedTask;
    }

    public Task DisposeAsync()
    {
        return Task.CompletedTask;
    }

    [Fact]
    public void should_serialize_and_deserialize()
    {
        var dateTime = new DateTime(2023, 10, 1, 12, 0, 0, DateTimeKind.Utc);
        var expected = TimezoneDateTime.FromDateTime(dateTime, LocalTimezone.Utc);

        var serialized = JsonSerializer.Serialize(expected);
        var deserialized = JsonSerializer.Deserialize<TimezoneDateTime>(serialized);

        deserialized.Should().Be(expected);
    }

    [Fact]
    public void should_serialize_and_deserialize_and_convert_to_specified_Timezone()
    {
        var timezone = LocalTimezone.Mappings["Asia/Tehran"].GetTimezone();
        var dateTime = new DateTime(2023, 10, 1, 12, 0, 0, DateTimeKind.Utc);
        var expected = TimezoneDateTime.FromDateTime(dateTime, timezone!);

        var serialized = JsonSerializer.Serialize(expected);
        var deserialized = JsonSerializer.Deserialize<TimezoneDateTime>(serialized);
        var actual = deserialized.WithTimezone(timezone);

        actual.Should().Be(expected);
    }

    [Fact]
    public void should_serialize()
    {
        var dateTime = new DateTime(2023, 10, 1, 12, 0, 0, DateTimeKind.Utc);
        var expected = TimezoneDateTime.FromDateTime(dateTime, LocalTimezone.Utc);

        var serialized = JsonSerializer.Serialize(expected);

        serialized.Should().Be("\"2023-10-01T12:00:00Z\"");
    }

    [Fact]
    public void should_deserialize()
    {
        var dateTime = new DateTime(2023, 10, 1, 12, 0, 0, DateTimeKind.Utc);
        var expected = TimezoneDateTime.FromDateTime(dateTime, LocalTimezone.Utc);

        var serialized = "\"2023-10-01T12:00:00Z\"";
        var deserialized = JsonSerializer.Deserialize<TimezoneDateTime>(serialized);

        deserialized.Should().Be(expected);
    }

    [Fact]
    public void should_throw_JsonException_when_serialized_text_is_not_string()
    {
        var serialized = "123";
        Action act = () => JsonSerializer.Deserialize<TimezoneDateTime>(serialized);

        act.Should().Throw<JsonException>().WithMessage("The value is expected to be a string, but was Number");
    }

    [Fact]
    public void should_return_Empty_instance_when_serialized_text_is_null()
    {
        var serialized = "null";
        var deserialized = JsonSerializer.Deserialize<TimezoneDateTime>(serialized);

        deserialized.Should().Be(TimezoneDateTime.Empty);
    }

    [Fact]
    public void should_throw_JsonException_when_serialized_text_is_whitespace()
    {
        var serialized = "\"  \"";
        Action act = () => JsonSerializer.Deserialize<TimezoneDateTime>(serialized);

        act.Should().Throw<JsonException>().WithMessage("Invalid date format: \"  \"");
    }

    [Fact]
    public void should_throw_JsonException_when_serialized_text_has_valid_format_but_couldnot_be_parsed_by_DateTime()
    {
        var serialized = "\"2023-10-01T12:00:00Z+03:30\"";
        Action act = () => JsonSerializer.Deserialize<TimezoneDateTime>(serialized);

        act.Should().Throw<JsonException>().WithMessage("Invalid date format: \"2023-10-01T12:00:00Z+03:30\"");
    }
}