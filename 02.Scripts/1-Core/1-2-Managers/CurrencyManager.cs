using System;
using System.Collections.Generic;
using UGS;

public enum CurrencyType
{
    Credits,
    Shard
}

public class CurrencyManager
{
    private readonly Dictionary<Type, Currency> currencies = new();
    public readonly Dictionary<Type, Action<int>> OnChangedCurrencyValue = new();
    
    public void Init()
    {
        Core.UGSManager.Auth.OnSignUp += ProvideStartProperty;
        Core.UGSManager.Data.OnSaveCalled += SaveData;
        Core.UGSManager.Data.OnAllDataLoaded += LoadData;
        
    }
    
    public T CreateCurrency<T>() where T : Currency, new()
    {
        if (false == currencies.TryGetValue(typeof(T), out var currency))
            currency = new T();

        return (T)currency;
    }

    public void Add<T>(int amount) where T : Currency, new()
    {
        Type currencyType = typeof(T);
        currencies[typeof(T)] = currencies.TryGetValue(currencyType, out var currency) ? currency : CreateCurrency<T>();
        
        currencies[typeof(T)].Add(amount);
        
        OnChangedCurrencyValue.TryGetValue(currencyType, out var actions);
        actions?.Invoke(GetAmount<T>());
    }

    public bool Spend<T>(int amount) where T : Currency, new()
    {
        Type currencyType = typeof(T);
        currencies[typeof(T)] = currencies.TryGetValue(currencyType, out var currency) ? currency : CreateCurrency<T>();

        if (true != currencies[currencyType].Spend(amount)) return false;
        
        OnChangedCurrencyValue.TryGetValue(currencyType, out var actions);
        actions?.Invoke(GetAmount<T>());
        
        return true;

    }

    public int GetAmount<T>() where T : Currency
    {
        Type currencyType = typeof(T);
        return currencies.TryGetValue(currencyType, out var currency) ? currency.Amount : 0;
    }
    
    public T GetCurrency<T>() where T : Currency, new()
    {
        Type currencyType = typeof(T);
        if (currencies.TryGetValue(currencyType, out var currency)) return (T)currency;
        
        currency = CreateCurrency<T>();
        currencies[currencyType] = currency;

        return (T)currency;
    }
    
    public void SubscribeCurrencyValueChanged<T>(Action<int> onPhaseEntered) where T : Currency
    {
        var currencyType = typeof(T);
        OnChangedCurrencyValue.TryAdd(currencyType, null);

        OnChangedCurrencyValue[currencyType] += onPhaseEntered;
    }
    
    public void UnsubscribeCurrencyValueChanged<T>(Action<int> onPhaseEntered) where T : Currency
    {
        var currencyType = typeof(T);
        if (OnChangedCurrencyValue.ContainsKey(currencyType))
            OnChangedCurrencyValue[currencyType] -= onPhaseEntered;
    }
    
    void ProvideStartProperty()
    {
        Add<Credits>(500);
        Add<Shard>(500);
    }

    void SaveData(Dictionary<Type, ICloudDataContainer> datas)
    {
        CloudDataContainer<PlayerCurrency> currencyData = datas[typeof(PlayerCurrency)] as CloudDataContainer<PlayerCurrency>;
        SaveCurrency<Credits>(currencyData);
        SaveCurrency<Shard>(currencyData);
    }

    void SaveCurrency<T>(CloudDataContainer<PlayerCurrency> data) where T : Currency
    {
        int key = currencies[typeof(T)].Key;
        
        if (data.Map.TryGetValue(key, out int index))
            data.Data[index].Amount = GetAmount<T>();
        else
            data.Add(new PlayerCurrency(currencies[typeof(T)]));
    }

    void LoadData(Dictionary<Type, ICloudDataContainer> datas)
    {
        CloudDataContainer<PlayerCurrency> currencyData = datas[typeof(PlayerCurrency)] as CloudDataContainer<PlayerCurrency>;

        for (int i = 0; i < currencyData.Data.Count; i++)
        {
            CurrencyType type = (CurrencyType)currencyData.Data[i].Key;;

            switch (type)
            {
                case CurrencyType.Credits:
                    Add<Credits>(currencyData.Data[i].Amount);
                    break;
                case CurrencyType.Shard:
                    Add<Shard>(currencyData.Data[i].Amount);
                    break;
            }
        }
    }
    
    
}