using System.Collections.ObjectModel;

namespace MauiTrading.Chart;

public partial class CandlestickChart : ContentView
{

    public static readonly BindableProperty ChartDataProperty =
        BindableProperty.Create(nameof(ChartData), typeof(ObservableCollection<CandleDataPoint>), typeof(CandlestickChart), new ObservableCollection<CandleDataPoint>());

    public static readonly BindableProperty XAxisTitleProperty =
        BindableProperty.Create(nameof(XAxisTitle), typeof(string), typeof(CandlestickChart), "Time");

    public static readonly BindableProperty YAxisTitleProperty =
        BindableProperty.Create(nameof(YAxisTitle), typeof(string), typeof(CandlestickChart), "Price");

    public ObservableCollection<CandleDataPoint> ChartData
    {
        get => (ObservableCollection<CandleDataPoint>)GetValue(ChartDataProperty);
        set => SetValue(ChartDataProperty, value);
    }

    public string XAxisTitle
    {
        get => (string)GetValue(XAxisTitleProperty);
        set => SetValue(XAxisTitleProperty, value);
    }

    public string YAxisTitle
    {
        get => (string)GetValue(YAxisTitleProperty);
        set => SetValue(YAxisTitleProperty, value);
    }

    public CandlestickChart()
    {
        InitializeComponent();
        BindingContext = this;
    }

    public class CandleDataPoint
    {
        public string XValue { get; set; }
        public double Open { get; set; }
        public double High { get; set; }
        public double Low { get; set; }
        public double Close { get; set; }

        public CandleDataPoint(string x, double open, double high, double low, double close)
        {
            XValue = x;
            Open = open;
            High = high;
            Low = low;
            Close = close;
        }
    }
}
    
