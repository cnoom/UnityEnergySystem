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

        // 消耗体力的方法
        public bool TryUseEnergy(int amount)
        {
            if(GetCurrentEnergy() >= amount)
            {
                data.SetCurrentEnergy(GetCurrentEnergy() - amount);
                return true;
            }
            return false;
        }

        // 获取当前体力值
        public int GetCurrentEnergy()
        {
            return data.GetCurrentEnergy();
        }

        // 获取最大体力值
        public int GetMaxEnergy()
        {
            return data.GetMaxEnergy();
        }

        // 检查体力是否已满
        public bool IsEnergyFull()
        {
            return data.IsFull();
        }
    }
}