# R8.DateTimeLocalization

[![Nuget](https://img.shields.io/nuget/vpre/R8.DateTimeLocalization)](https://www.nuget.org/packages/R8.DateTimeLocalization/) ![Nuget](https://img.shields.io/nuget/dt/R8.DateTimeLocalization) ![Commit](https://img.shields.io/github/last-commit/iamr8/R8.DateTimeLocalization)

### Installation

```bash
dotnet add package R8.DateTimeLocalization
```

### Setup

- `UTC` is added by default
- Here is an example of adding a custom timezone:

```csharp
using R8.DateTimeLocalization;

public class IranTimezone : LocalTimezoneInfo
{
    public override string IanaId => "Asia/Tehran";
    public override CultureInfo Culture => CultureInfo.GetCultureInfo("fa-IR");
    public override CalendarSystem Calendar => CalendarSystem.PersianSimple;
}

// One-time setup, anywhere in your app
LocalTimezone.Mappings.Add<IranTimezone>();
```

### Usage

```csharp
using R8.DateTimeLocalization;

var timezone = LocalTimezone.Get("Asia/Tehran");
// var timezone = LocalTimezone.Get<IranTimezone>();

var dateTime = DateTime.UtcNow;
var localized = dateTime.ToTimezoneDateTime(timezone);
/// ... rest of properties are the same as DateTime
```