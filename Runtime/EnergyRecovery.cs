using System;
using Cnoom.UnityTool.StorageUtils;

namespace com.cnoom.energy.Runtime
{
    public class EnergyRecovery
    {
        private IEnergyRecoveryRule recoveryRule;
        private EnergyData energyData;

        public EnergyRecovery(IEnergyRecoveryRule recoveryRule, EnergyData energyData)
        {
            this.recoveryRule = recoveryRule;
            this.energyData = energyData;
        }

        public void UpdateRecover()
        {
            recoveryRule.RecoverEnergy(energyData);
        }
    }

    public interface IEnergyRecoveryRule
    {
        void RecoverEnergy(EnergyData energyData);
    }

    public class EnergyRecoveryRuleDefault : IEnergyRecoveryRule, IStorageUser
    {
        private readonly int recoverySeconds;

        // 用于记录上次体力恢复到系统时间对应的时间戳（以秒为单位）
        private long lastRecoveryTimeStamp;

        public EnergyRecoveryRuleDefault(int recoverySeconds)
        {
            this.recoverySeconds = recoverySeconds;
            lastRecoveryTimeStamp = long.Parse(this.GetString(nameof(lastRecoveryTimeStamp), lastRecoveryTimeStamp.ToString()));

        }

        public void RecoverEnergy(EnergyData energyData)
        {
            long currentTimeStamp = GetCurrentTimeStamp();
            int timeStamp = (int)(currentTimeStamp - lastRecoveryTimeStamp);
            
            if(timeStamp < recoverySeconds) return;
            
            int recovery = timeStamp / recoverySeconds;
            energyData.SetCurrentEnergy(energyData.GetCurrentEnergy() + recovery);
            
            lastRecoveryTimeStamp = currentTimeStamp;
            this.SaveString(nameof(lastRecoveryTimeStamp), currentTimeStamp.ToString());
        }

        private long GetCurrentTimeStamp()
        {
            return (DateTime.UtcNow.Ticks - 621355968000000000) / 10000000;
        }
    }
}