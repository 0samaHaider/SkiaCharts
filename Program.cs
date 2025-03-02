using System.Globalization;
using SkiaSharp;

namespace SkiaCharts;

internal class Program
{
    private static void Main()
    {
        Console.WriteLine("Generating charts...");

        LineChart("line_chart.png");
        BarChart("bar_chart.png");
        PieChart("pie_chart.png");
        ScatterPlot("scatter_plot.png");

        Console.WriteLine("Charts successfully generated!");
    }

    private static void LineChart(string fileName)
    {
        const int width = 800;
        const int height = 500;
        using var bitmap = new SKBitmap(width, height);
        using var canvas = new SKCanvas(bitmap);
        canvas.Clear(SKColors.White);

        double[] data = { 10, 50, 30, 70, 90, 40, 60 };
        var dataCount = data.Length;
        string[] labels = { "Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul" };

        const int margin = 50;
        const int graphWidth = width - 2 * margin;
        const int graphHeight = height - 2 * margin;
        var stepX = graphWidth / (dataCount - 1);
        const float scaleY = graphHeight / 100f;

        using var axisPaint = new SKPaint();
        axisPaint.Color = SKColors.Black;
        axisPaint.StrokeWidth = 2;
        axisPaint.IsAntialias = true;
        canvas.DrawLine(margin, height - margin, width - margin, height - margin, axisPaint); // X-Axis
        canvas.DrawLine(margin, margin, margin, height - margin, axisPaint); // Y-Axis

        using var labelPaint = new SKPaint();
        labelPaint.Color = SKColors.Black;
        labelPaint.TextSize = 16;
        labelPaint.IsAntialias = true;

        const int numTicks = 5;
        const float tickSpacing = 100f / numTicks;

        for (var i = 0; i <= numTicks; i++)
        {
            var yValue = i * tickSpacing;
            var yPos = height - margin - (yValue * scaleY);
            canvas.DrawText(yValue.ToString(CultureInfo.CurrentCulture), margin - 35, yPos + 5, labelPaint); // Y-axis values
            canvas.DrawLine(margin - 5, yPos, margin, yPos, axisPaint); // Tick marks
        }

        for (var i = 0; i < dataCount; i++)
        {
            float x = margin + i * stepX;
            const float y = height - margin + 20;
            canvas.DrawText(labels[i], x - 15, y, labelPaint);
        }

        canvas.DrawText("Months", width / 2, height - 10, labelPaint);

        using var paint = new SKPaint();
        paint.Color = SKColors.Blue;
        paint.StrokeWidth = 4;
        paint.IsAntialias = true;

        for (var i = 0; i < dataCount - 1; i++)
        {
            var x1 = margin + i * stepX;
            var y1 = height - margin - (float)data[i] * scaleY;
            var x2 = margin + (i + 1) * stepX;
            var y2 = height - margin - (float)data[i + 1] * scaleY;
            canvas.DrawLine(x1, y1, x2, y2, paint);
        }

        SaveChart(bitmap, fileName);
    }


    static void BarChart(string fileName)
    {
        const int width = 800;
        var height = 500;
        using var bitmap = new SKBitmap(width, height);
        using var canvas = new SKCanvas(bitmap);
        canvas.Clear(SKColors.White);

        string[] categories = { "A", "B", "C", "D", "E" };
        double[] values = { 30, 70, 50, 90, 40 };
        const float margin = 50;
        const float graphWidth = width - 2 * margin;
        var graphHeight = height - 2 * margin;
        var barWidth = graphWidth / values.Length * 0.7f;
        var scaleY = graphHeight / 100f;

        using var axisPaint = new SKPaint();
        axisPaint.Color = SKColors.Black;
        axisPaint.StrokeWidth = 2;
        axisPaint.IsAntialias = true;
        canvas.DrawLine(margin, height - margin, width - margin, height - margin, axisPaint);
        canvas.DrawLine(margin, margin, margin, height - margin, axisPaint);

        using var barPaint = new SKPaint();
        barPaint.Color = SKColors.Green;
        barPaint.IsAntialias = true;

        using var labelPaint = new SKPaint();
        labelPaint.Color = SKColors.Black;
        labelPaint.TextSize = 16;
        labelPaint.IsAntialias = true;

        const int numTicks = 5; // Number of Y-axis labels
        const float tickSpacing = 100f / numTicks; // Adjust based on max value (100)

        for (var i = 0; i <= numTicks; i++)
        {
            var yValue = i * tickSpacing;
            var yPos = height - margin - (yValue * scaleY);
            canvas.DrawText(yValue.ToString(CultureInfo.CurrentCulture), margin - 35, yPos + 5, labelPaint);
            canvas.DrawLine(margin - 5, yPos, margin, yPos, axisPaint);
        }

        // ✅ Draw bars and X-Axis Labels
        for (var i = 0; i < values.Length; i++)
        {
            var left = margin + i * (graphWidth / values.Length) + barWidth * 0.15f;
            var top = height - margin - (float)values[i] * scaleY;
            var right = left + barWidth;
            var bottom = height - margin;
            canvas.DrawRect(new SKRect(left, top, right, bottom), barPaint);
            canvas.DrawText(categories[i], left + 10, height - margin + 20, labelPaint);
        }

        // ✅ Chart Labels
        canvas.DrawText("Categories", width / 2, height - 10, labelPaint);

        SaveChart(bitmap, fileName);
    }


    static void PieChart(string fileName)
    {
        const int width = 500;
        const int height = 500;
        using var bitmap = new SKBitmap(width, height);
        using var canvas = new SKCanvas(bitmap);
        canvas.Clear(SKColors.White);

        double[] data = { 30, 50, 70, 90 };
        string[] labels = { "Red", "Blue", "Green", "Orange" };
        SKColor[] colors = { SKColors.Red, SKColors.Blue, SKColors.Green, SKColors.Orange };

        float total = 0;
        foreach (var value in data) total += (float)value;

        float startAngle = 0;
        using var paint = new SKPaint();
        paint.IsAntialias = true;
        using var textPaint = new SKPaint();
        textPaint.Color = SKColors.Black;
        textPaint.TextSize = 16;
        textPaint.IsAntialias = true;

        for (int i = 0; i < data.Length; i++)
        {
            float sweepAngle = (float)(data[i] / total * 360);
            paint.Color = colors[i % colors.Length];
            canvas.DrawArc(new SKRect(50, 50, width - 50, height - 50), startAngle, sweepAngle, true, paint);

            var midAngle = startAngle + sweepAngle / 2;
            var x = (float)(250 + Math.Cos(midAngle * Math.PI / 180) * 100);
            var y = (float)(250 + Math.Sin(midAngle * Math.PI / 180) * 100);
            canvas.DrawText(labels[i], x, y, textPaint);

            startAngle += sweepAngle;
        }

        SaveChart(bitmap, fileName);
    }

    static void ScatterPlot(string fileName)
    {
        const int width = 800;
        const int height = 500;
        using var bitmap = new SKBitmap(width, height);
        using var canvas = new SKCanvas(bitmap);
        canvas.Clear(SKColors.White);

        var points = new List<SKPoint>
        {
            new SKPoint(50, 450),  // $1K Ad Spend -> $5K Sales
            new SKPoint(150, 400), // $2K Ad Spend -> $10K Sales
            new SKPoint(250, 350), // $3K Ad Spend -> $15K Sales
            new SKPoint(350, 250), // $4K Ad Spend -> $25K Sales
            new SKPoint(450, 150), // $5K Ad Spend -> $35K Sales
            new SKPoint(550, 100), // $6K Ad Spend -> $40K Sales
            new SKPoint(650, 50)   // $7K Ad Spend -> $45K Sales
        };

        const float margin = 50;
        const float graphWidth = width - 2 * margin;
        const float graphHeight = height - 2 * margin;

        using var axisPaint = new SKPaint();
        axisPaint.Color = SKColors.Black;
        axisPaint.StrokeWidth = 2;
        axisPaint.IsAntialias = true;

        using var dotPaint = new SKPaint();
        dotPaint.Color = SKColors.Green;
        dotPaint.IsAntialias = true;

        using var labelPaint = new SKPaint();
        labelPaint.Color = SKColors.Black;
        labelPaint.TextSize = 16;
        labelPaint.IsAntialias = true;

        // 📌 Draw X and Y Axes
        canvas.DrawLine(margin, height - margin, width - margin, height - margin, axisPaint); // X-Axis
        canvas.DrawLine(margin, margin, margin, height - margin, axisPaint); // Y-Axis

        // 📌 Add Y-Axis Labels (Sales Revenue)
        for (var i = 0; i <= 5; i++)
        {
            float yValue = i * 10; // Scale for revenue ($10K increments)
            var yPos = height - margin - (yValue * graphHeight / 50); // Scale Y
            canvas.DrawText($"${yValue}K", margin - 40, yPos + 5, labelPaint);
            canvas.DrawLine(margin - 5, yPos, margin, yPos, axisPaint);
        }

        // 📌 Add X-Axis Labels (Ad Spend)
        for (var i = 1; i <= 7; i++)
        {
            var xPos = margin + (i * graphWidth / 7);
            canvas.DrawText($"${i}K", xPos - 15, height - margin + 20, labelPaint);
        }

        // 📌 Draw Data Points
        foreach (var point in points)
        {
            canvas.DrawRect(point.X, point.Y, 8, 8, dotPaint); // Square points for better visibility
        }

        // 📌 Add Axis Titles
        canvas.DrawText("Advertisement Spend ($K)", width / 2 - 50, height - 10, labelPaint);
        canvas.DrawText("Sales", 10, height / 2, labelPaint);

        SaveChart(bitmap, fileName);
    }

    private static void SaveChart(SKBitmap bitmap, string fileName)
    {
        using var image = SKImage.FromBitmap(bitmap);
        using var data = image.Encode(SKEncodedImageFormat.Png, 100);
        File.WriteAllBytes(fileName, data.ToArray());
        Console.WriteLine($"Chart saved: {fileName}");
    }
}