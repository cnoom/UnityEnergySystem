using Cnoom.UnityTool.SingletonUtils;
using UnityEngine;
using System;
using Cnoom.UnityTool.ActionUtils;
using Cnoom.UnityTool.LogUtils;
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

        private EnergySystem() { }

        private bool isInit;
        /// <summary>
        /// 初始化体力系统
        /// </summary>
        /// <param name="energyMax">体力最大值</param>
        /// <param name="secondsEnergyRecovery">回复速度/秒</param>
        public void Init(int energyMax, int secondsEnergyRecovery)
        {
            maxEnergy = energyMax;
            secondsPerEnergyRecovery = secondsEnergyRecovery;
            currentEnergy = this.GetInt(nameof(currentEnergy), maxEnergy);
            lastRecoveryTimeStamp = long.Parse(this.GetString(nameof(lastRecoveryTimeStamp), GetCurrentTimeStamp().ToString()));
            ActionSystem.Instance.OnUpdate += Update;
            
            isInit = true;
            Update();
        }

        // 获取当前系统时间对应的时间戳（以秒为单位），这里使用DateTime.UtcNow获取更统一的时间标准
        private long GetCurrentTimeStamp()
        {
            return ((DateTime.UtcNow.Ticks - 621355968000000000) / 10000000);
        }

        // 消耗体力的方法
        public bool TryUseEnergy(int amount)
        {
            if(currentEnergy >= amount)
            {
                currentEnergy -= amount;
                SaveCurrentEnergy();
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
            if(!isInit) return;
            long currentTimeStamp = GetCurrentTimeStamp();
            // 计算从上次恢复体力到现在经过的时间（秒数）
            long timeElapsed = currentTimeStamp - lastRecoveryTimeStamp;
            // 根据经过的时间和恢复速度，计算可以恢复的体力点数
            int recoveredEnergy = Mathf.FloorToInt((float)timeElapsed / secondsPerEnergyRecovery);
            int ce = Mathf.Min(currentEnergy + recoveredEnergy, maxEnergy);
            if(ce != currentEnergy)
            {
                currentEnergy = ce;
                SaveCurrentEnergy();
                UpdateLastRecoveryTimeStamp();
            }
        }

        private void UpdateLastRecoveryTimeStamp()
        {
            lastRecoveryTimeStamp = GetCurrentTimeStamp();
            SaveLastRecoveryTimeStamp();
        }

        private void SaveLastRecoveryTimeStamp()
        {
            this.SaveString(nameof(lastRecoveryTimeStamp), lastRecoveryTimeStamp.ToString());
        }

        private void SaveCurrentEnergy()
        {
            this.SaveInt(nameof(currentEnergy), currentEnergy);
        }
    }
}