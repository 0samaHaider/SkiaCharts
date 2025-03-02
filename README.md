# SkiaCharts

SkiaCharts is a simple .NET console application that generates different types of charts using SkiaSharp.

## Prerequisites

- .NET 8 or later
- SkiaSharp NuGet package

## Installation

1. Clone this repository or copy the source code.
2. Install dependencies:
   ```sh
   dotnet add package SkiaSharp
   ```
3. Build and run the project:
   ```sh
   dotnet run
   ```

## Features

This application generates the following charts and saves them as PNG images:

- **Line Chart** (`line_chart.png`)
- **Bar Chart** (`bar_chart.png`)
- **Pie Chart** (`pie_chart.png`)
- **Scatter Plot** (`scatter_plot.png`)

## File Output

Generated charts are saved in the project directory as:

- `line_chart.png`
- `bar_chart.png`
- `pie_chart.png`
- `scatter_plot.png`

## Usage

Simply run the application, and the charts will be generated and saved automatically.

## Example Code

Here’s a simple example of how we create a line chart using SkiaSharp:

```csharp
using SkiaSharp;

void LineChart(string fileName)
{
    const int width = 800, height = 500;
    using var bitmap = new SKBitmap(width, height);
    using var canvas = new SKCanvas(bitmap);
    canvas.Clear(SKColors.White);

    using var paint = new SKPaint { Color = SKColors.Blue, StrokeWidth = 4, IsAntialias = true };
    canvas.DrawLine(50, 450, 750, 50, paint);

    using var image = SKImage.FromBitmap(bitmap);
    using var data = image.Encode(SKEncodedImageFormat.Png, 100);
    File.WriteAllBytes(fileName, data.ToArray());
}
```

## Why SkiaSharp?

SkiaSharp is a cross-platform 2D graphics library that allows .NET developers to create high-performance, lightweight graphics applications.

## License

This project is open-source and free to use.

