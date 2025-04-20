using System;

namespace com.cnoom.energy.Runtime
{
    public class EnergyRecovery
    {
        private readonly EnergyData energyData;
        private readonly IEnergyRecoveryRule recoveryRule;

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
        IEnergyStorage Storage { get; set; }
        void RecoverEnergy(EnergyData energyData);
    }

    public class EnergyRecoveryRuleDefault : IEnergyRecoveryRule
    {
        private readonly int recoverySeconds;

        // 用于记录上次体力恢复到系统时间对应的时间戳（以秒为单位）
        private long lastRecoveryTimeStamp;

        public EnergyRecoveryRuleDefault(IEnergyStorage storage)
        {
            Storage = storage;
        }

        public EnergyRecoveryRuleDefault(int recoverySeconds)
        {
            this.recoverySeconds = recoverySeconds;
            lastRecoveryTimeStamp = Storage.Load(nameof(lastRecoveryTimeStamp), lastRecoveryTimeStamp);
        }
        public IEnergyStorage Storage { get; set; }

        public void RecoverEnergy(EnergyData energyData)
        {
            long currentTimeStamp = GetCurrentTimeStamp();
            var timeStamp = (int)(currentTimeStamp - lastRecoveryTimeStamp);

            if(timeStamp < recoverySeconds) return;

            int recovery = timeStamp / recoverySeconds;
            energyData.SetCurrentEnergy(energyData.GetCurrentEnergy() + recovery);

            lastRecoveryTimeStamp = currentTimeStamp;
            Storage.Save(nameof(lastRecoveryTimeStamp), lastRecoveryTimeStamp);
        }

        private long GetCurrentTimeStamp()
        {
            return (DateTime.UtcNow.Ticks - 621355968000000000) / 10000000;
        }
    }
}