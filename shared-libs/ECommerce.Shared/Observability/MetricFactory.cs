﻿using System.Diagnostics.Metrics;

namespace ECommerce.Shared.Observability;

public class MetricFactory
{
    private readonly Meter _meter;
    private readonly Dictionary<string, Counter<int>> _cachedCounters = new();
    private readonly Dictionary<string, Histogram<int>> _cachedHistogram = new();

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

    public Histogram<int> Histogram(string name, string? unit = null) 
    {
        if (_cachedHistogram.TryGetValue(name, out Histogram<int> value)) return value;

        var histogram = _meter.CreateHistogram<int>(name, unit);
        _cachedHistogram.Add(name, histogram);

        return histogram;
    }
}