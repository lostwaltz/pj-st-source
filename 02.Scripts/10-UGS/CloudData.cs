using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.Services.CloudSave;
using Unity.Services.CloudSave.Models;

namespace UGS
{
    public enum CloudDataType
    {
        All = -1,
        PlayerCurrency = 0,
        PlayerUnit,
        ChallengeObjective,
        PlayerStageClear,
        PlayerAchievement,

        COUNT
    }

    public enum InvaildCase
    {
        Nothing = -1,
        DataIsEmpty = 0,
    }

    public class CloudData : UGSRequester
    {
        readonly string currencyTypeString = CloudDataType.PlayerCurrency.ToString();
        readonly string unitTypeString = CloudDataType.PlayerUnit.ToString();
        readonly string challengeObjectiveTypeString = CloudDataType.ChallengeObjective.ToString();
        readonly string stageClearTypeString = CloudDataType.PlayerStageClear.ToString();
        readonly string playerAchievement = CloudDataType.PlayerAchievement.ToString();

        public Dictionary<Type, ICloudDataContainer> CloudDatas = new()
        {
            { typeof(PlayerCurrency), new CloudDataContainer<PlayerCurrency>() },
            { typeof(PlayerUnit), new CloudDataContainer<PlayerUnit>() },
            { typeof(ChallengeObjective), new CloudDataContainer<ChallengeObjective>() },
            { typeof(PlayerStageClear), new CloudDataContainer<PlayerStageClear>() },
            { typeof(PlayerAchievement), new CloudDataContainer<PlayerAchievement>() }
        };

        public event Action<Dictionary<Type, ICloudDataContainer>> OnSaveCalled;
        public event Action<Dictionary<Type, ICloudDataContainer>> OnAllDataLoaded;


        public CloudData()
        {
            Core.UGSManager.Auth.OnSignIn += () =>
            {
                LoadAll(() =>
                {
                    OnAllDataLoaded?.Invoke(CloudDatas);
                    manager.StartGame(); // 로드 후 게임 시작
                });
            };
        }


        public void CallSave(CloudDataType target = CloudDataType.All)
        {
            if (manager.Auth.IsAnonymous)
                return;

            OnSaveCalled?.Invoke(CloudDatas);

            if (target.Equals(CloudDataType.All))
                SaveAll();
            else
                Save(ConvertEnumToType(target));
        }


        async Task Save(Type type)
        {
            var data = new Dictionary<string, object> { { type.Name, CloudDatas[type].GetData() } };

            await Request(CloudSaveService.Instance.Data.Player.SaveAsync(data));
        }

        async Task SaveAll()
        {
            var data = new Dictionary<string, object>();

            int count = (int)CloudDataType.COUNT;
            for (int i = 0; i < count; i++)
            {
                Type type = ConvertEnumToType((CloudDataType)i);
                data.Add(type.Name, CloudDatas[type].GetData());
            }

            await Request(CloudSaveService.Instance.Data.Player.SaveAsync(data));
        }

        async Task Load<T>() where T : DataModel
        {
            Type type = typeof(T);
            HashSet<string> keys = new HashSet<string>() { type.Name };
            var data = await Request<Task<Dictionary<string, Item>>, Dictionary<string, Item>>(CloudSaveService.Instance.Data.Player.LoadAsync(keys));

            if (IsVailid(data, out InvaildCase invalidType))
                Parse<T>(data[type.Name]);
            else
                ProcessInvalidData(invalidType);
        }

        async Task LoadAll(Action onLoadEnd = null)
        {
            var datas = await Request<Task<Dictionary<string, Item>>, Dictionary<string, Item>>(CloudSaveService.Instance.Data.Player.LoadAllAsync());

            if (IsVailid(datas, out InvaildCase invalidType))
            {
                foreach (var key in datas.Keys)
                {
                    CloudDataType type = ConvertStrToType(key);
                    switch (type)
                    {
                        case CloudDataType.PlayerCurrency:
                            Parse<PlayerCurrency>(datas[key]);
                            break;
                        case CloudDataType.PlayerUnit:
                            Parse<PlayerUnit>(datas[key]);
                            break;
                        case CloudDataType.ChallengeObjective:
                            Parse<ChallengeObjective>(datas[key]);
                            break;
                        case CloudDataType.PlayerStageClear:
                            Parse<PlayerStageClear>(datas[key]);
                            break;
                        case CloudDataType.PlayerAchievement:
                            Parse<PlayerAchievement>(datas[key]);
                            break;
                    }
                }
            }
            else
                ProcessInvalidData(invalidType);

            onLoadEnd?.Invoke();
        }

        Type ConvertEnumToType(CloudDataType eType)
        {
            switch (eType)
            {
                case CloudDataType.PlayerCurrency:
                    return typeof(PlayerCurrency);
                case CloudDataType.PlayerUnit:
                    return typeof(PlayerUnit);
                case CloudDataType.ChallengeObjective:
                    return typeof(ChallengeObjective);
                case CloudDataType.PlayerStageClear:
                    return typeof(PlayerStageClear);
                case CloudDataType.PlayerAchievement:
                    return typeof(PlayerAchievement);
                default:
                    return typeof(PlayerCurrency);
            }
        }

        CloudDataType ConvertStrToType(string str)
        {
            if (str.Equals(currencyTypeString))
                return CloudDataType.PlayerCurrency;
            if (str.Equals(unitTypeString))
                return CloudDataType.PlayerUnit;
            if (str.Equals(challengeObjectiveTypeString))
                return CloudDataType.ChallengeObjective;
            if (str.Equals(stageClearTypeString))
                return CloudDataType.PlayerStageClear;
            if(str.Equals(playerAchievement))
                return CloudDataType.PlayerAchievement;

            return CloudDataType.All;
        }

        void Parse<T>(Item data) where T : DataModel
        {
            try
            {
                List<T> parsing = data.Value.GetAs<List<T>>();
                CloudDatas[typeof(T)] = new CloudDataContainer<T>(parsing);
            }
            catch (Exception e)
            {
                Core.CommonUIManager.GetUI<UIInformPopup>().Initialize(e.Message);
            }
        }

        bool IsVailid(Dictionary<string, Item> data, out InvaildCase invalidType)
        {
            invalidType = InvaildCase.Nothing;

            if (data.Keys.Count == 0)
            {
                invalidType = InvaildCase.DataIsEmpty;
                return false;
            }

            // TODO : 데이터 유효성 검사

            return true;
        }

        // 유효하지 않은 데이터 처리
        void ProcessInvalidData(InvaildCase type)
        {
            switch (type)
            {
                case InvaildCase.DataIsEmpty:
                    manager.Auth.OnSignUpEvent(); // 초기 데이터 제공
                    break;
            }
        }
    }

}