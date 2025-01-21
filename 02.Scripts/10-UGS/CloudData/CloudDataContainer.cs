using System;
using System.Collections.Generic;

namespace UGS
{
    public interface ICloudDataContainer
    {
        object GetData();
    }
    
    public class CloudDataContainer<T> : ICloudDataContainer where T : DataModel
    {
        public Dictionary<int, int> Map = new();
        public List<T> Data = new();

        public CloudDataContainer()
        {
            Map = new();
            Data = new();
        }
        
        public CloudDataContainer(List<T> data)
        {
            for (int i = 0; i < data.Count; i++)
                Map.Add(data[i].Key, i);
            
            Data = data;
        }

        public void Add(T data)
        {
            Data.Add(data);
            Map.Add(data.Key, Data.Count - 1);
        }

        public object GetData()
        {
            return Data;// JsonUtility.ToJson(Data);
        }
    }

    [Serializable]
    public class PlayerCurrency : DataModel
    {
        public PlayerCurrency(){}
        public PlayerCurrency(Currency currency)
        {
            Key = currency.Key;
            Amount = currency.Amount;
        }
        
        /// <summary>
        /// 재화량
        /// </summary>
        public int Amount;
        
        public void Set(int newAmount){ Amount = newAmount; }
    }

    [Serializable]
    public class PlayerUnit : DataModel
    {
        public PlayerUnit(){}
        public PlayerUnit(UnitInstance unit)
        {
            Key = unit.UnitBase.Key;
            Level = unit.Level;
            Exp = unit.CurrentExp;
            Spine = unit.SpineLevel;
            Pieces = unit.Pieces;
        }
        
        /// <summary>
        /// 유닛 레벨
        /// </summary>
        public int Level;
        /// <summary>
        /// 유닛 경험치
        /// </summary>
        public int Exp;
        /// <summary>
        /// 유닛 돌파 레벨
        /// </summary>
        public int Spine;
        /// <summary>
        /// 유닛 조각
        /// </summary>
        public int Pieces;

        public void Set(int newLv, int newExp, int newSpine, int newPieces)
        {
            Level = newLv;
            Exp = newExp;
            Spine = newSpine;
            Pieces = newPieces;
        }
    }

    [Serializable]
    public class PlayerStageClear : DataModel
    {
        public PlayerStageClear() {}

        public PlayerStageClear(int newKey, bool cleared)
        {
            Key = newKey;
            IsClear = cleared;
        }
        
        public bool IsClear;

        public void Set(bool cleared)
        {
            IsClear = cleared;
        }
    }

    public class PlayerAchievement : DataModel
    {
        public float Progress;
        public bool IsTakeReward;
        
        public PlayerAchievement() { }

        public PlayerAchievement(int newKey, float progress, bool isTakeReward)
        {
            Key = newKey;
            Progress = progress;
            IsTakeReward = isTakeReward;
        }

        public void Set(float progress, bool isTakeReward)
        {
            Progress = progress;
            IsTakeReward = isTakeReward;
        }
    }
}