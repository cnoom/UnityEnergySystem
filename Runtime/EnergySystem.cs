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
        private EnergyData data;
        private EnergyRecovery energyRecovery;

        private EnergySystem() { }

        private bool isInit;
        /// <summary>
        /// 初始化体力系统
        /// </summary>
        public void Init(EnergyData energyData, IEnergyRecoveryRule energyRecoveryRule)
        {
            data = energyData;
            energyRecovery = new EnergyRecovery(energyRecoveryRule, energyData);
            ActionSystem.Instance.OnUpdate += energyRecovery.UpdateRecover;

            isInit = true;
            energyRecovery.UpdateRecover();
        }

        /// <summary>
        /// 消耗体力的方法
        /// </summary>
        /// <param name="amount">消耗数量</param>
        /// <returns></returns>
        public bool TryUseEnergy(int amount)
        {
            if(GetCurrentEnergy() >= amount)
            {
                data.SetCurrentEnergy(GetCurrentEnergy() - amount);
                return true;
            }
            return false;
        }

        /// <summary>
        /// 增加体力的方法
        /// </summary>
        public void AddEnergy(int amount)
        {
            data.SetCurrentEnergy(GetCurrentEnergy() + amount);
        }

        
        /// <returns>当前体力值</returns>
        public int GetCurrentEnergy()
        {
            return data.GetCurrentEnergy();
        }
        
        /// <returns>获取最大体力值</returns>
        public int GetMaxEnergy()
        {
            return data.GetMaxEnergy();
        }

        /// <returns>体力是否已满</returns> 
        public bool IsEnergyFull()
        {
            return data.IsFull();
        }
    }
}