using Cnoom.UnityTool.SingletonUtils;
using UnityEngine;
using System;
using Cnoom.UnityTool.ActionUtils;
using Cnoom.UnityTool.StorageUtils;

namespace com.cnoom.energy.Runtime
{


    public class EnergySystem : Singleton<EnergySystem>, IStorageUser
    {
        // 当前体力值
        private int currentEnergy;
        // 最大体力值
        private int maxEnergy;
        // 体力恢复速度（每多少秒恢复1点体力）
        private int secondsPerEnergyRecovery;
        // 用于记录上次体力恢复到系统时间对应的时间戳（以秒为单位）
        private long lastRecoveryTimeStamp;

        public void Init(int energyMax, int secondsEnergyRecovery)
        {
            maxEnergy = energyMax;
            secondsPerEnergyRecovery = secondsEnergyRecovery;
            currentEnergy = this.GetInt(nameof(currentEnergy), maxEnergy);
            lastRecoveryTimeStamp = long.Parse(this.GetString(nameof(lastRecoveryTimeStamp), GetCurrentTimeStamp().ToString()));
            
            ActionSystem.Instance
        }

        // 获取当前系统时间对应的时间戳（以秒为单位），这里使用DateTime.UtcNow获取更统一的时间标准
        private long GetCurrentTimeStamp()
        {
            return ((DateTime.UtcNow.Ticks - 621355968000000000) / 10000000);
        }

        // 消耗体力的方法
        public bool UseEnergy(int amount)
        {
            if(currentEnergy >= amount)
            {
                currentEnergy -= amount;
                return true;
            }
            return false;
        }

        // 获取当前体力值
        public int GetCurrentEnergy()
        {
            return currentEnergy;
        }

        // 获取最大体力值
        public int GetMaxEnergy()
        {
            return maxEnergy;
        }

        // 检查体力是否已满
        public bool IsEnergyFull()
        {
            return currentEnergy == maxEnergy;
        }

        private void Update()
        {
            long currentTimeStamp = GetCurrentTimeStamp();
            // 计算从上次恢复体力到现在经过的时间（秒数）
            long timeElapsed = currentTimeStamp - lastRecoveryTimeStamp;
            // 根据经过的时间和恢复速度，计算可以恢复的体力点数
            int recoveredEnergy = Mathf.FloorToInt((float)timeElapsed / secondsPerEnergyRecovery);
            currentEnergy = Mathf.Min(currentEnergy + recoveredEnergy, maxEnergy);
            // 更新上次体力恢复对应的时间戳，确保下次计算准确
            lastRecoveryTimeStamp = currentTimeStamp;
        }
    }
}