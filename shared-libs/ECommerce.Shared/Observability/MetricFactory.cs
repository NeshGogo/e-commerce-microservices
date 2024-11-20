using System.Diagnostics.Metrics;

namespace ECommerce.Shared.Observability;

public class MetricFactory
{
    private readonly Meter _meter;
    private readonly Dictionary<string, Counter<int>> _cachedCounters = new();

    public MetricFactory(string meterName)
    {
        _meter = new Meter(meterName);
    }

    public Counter<int> Counter(string name, string? unit = null)
    {
        if (_cachedCounters.TryGetValue(name, out Counter<int> value)) return value;

        var counter = _meter.CreateCounter<int>(name, unit);
        _cachedCounters.Add(name, counter);

        return counter;
    }
}
