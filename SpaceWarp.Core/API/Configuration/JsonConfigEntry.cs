﻿using System;
using JetBrains.Annotations;

namespace SpaceWarp.API.Configuration;

[PublicAPI]
public class JsonConfigEntry : IConfigEntry
{
    private readonly JsonConfigFile _configFile;
    private object _value;

    public JsonConfigEntry(JsonConfigFile configFile, Type type, string description, object value)
    {
        _configFile = configFile;
        _value = value;
        Description = description;
        ValueType = type;
    }


    public object Value
    {
        get => _value;
        set
        {
            _value = value;
            _configFile.Save();
        }
    }

    public Type ValueType { get; }
    public T Get<T>() where T : class
    {
        if (!typeof(T).IsAssignableFrom(ValueType))
        {
            throw new InvalidCastException($"Cannot cast {ValueType} to {typeof(T)}");
        }

        return Value as T;
    }

    public void Set<T>(T value)
    {
        if (!ValueType.IsAssignableFrom(typeof(T)))
        {
            throw new InvalidCastException($"Cannot cast {ValueType} to {typeof(T)}");
        }

        Value = Convert.ChangeType(value, ValueType);
    }

    public string Description { get; }
}