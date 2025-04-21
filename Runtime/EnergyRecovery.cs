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

        /// <summary>
        ///     满体力消耗时更新时间戳
        /// </summary>
        public void TryCostUpdateTimeStam()
        {
            if(energyData.IsFull())
            {
                recoveryRule.UpdateTimeStam();
            }
        }
    }

    public interface IEnergyRecoveryRule
    {
        
        IEnergyStorage Storage { get; set; }
        public long LastRecoveryTimeStamp { get; set; }
        void RecoverEnergy(EnergyData energyData);
        void UpdateTimeStam();
    }

    public class EnergyRecoveryRuleDefault : IEnergyRecoveryRule
    {
        private readonly int recoverySeconds;
        public IEnergyStorage Storage { get; set; }

        public long LastRecoveryTimeStamp { get; set; }
        // 用于记录上次体力恢复到系统时间对应的时间戳（以秒为单位）

        public EnergyRecoveryRuleDefault(int recoverySeconds,IEnergyStorage storage)
        {
            this.recoverySeconds = recoverySeconds;
            Storage = storage;
            LastRecoveryTimeStamp = Storage.Load(nameof(LastRecoveryTimeStamp), LastRecoveryTimeStamp);
        }

        public void RecoverEnergy(EnergyData energyData)
        {
            long currentTimeStamp = GetCurrentTimeStamp();
            var timeStamp = (int)(currentTimeStamp - LastRecoveryTimeStamp);

            if(timeStamp < recoverySeconds) return;

            int recovery = timeStamp / recoverySeconds;
            energyData.SetCurrentEnergy(energyData.GetCurrentEnergy() + recovery);

            LastRecoveryTimeStamp = currentTimeStamp;
            Storage.Save(nameof(LastRecoveryTimeStamp), LastRecoveryTimeStamp);
        }
        
        public void UpdateTimeStam()
        {
            LastRecoveryTimeStamp = GetCurrentTimeStamp();
        }

        private long GetCurrentTimeStamp()
        {
            return (DateTime.UtcNow.Ticks - 621355968000000000) / 10000000;
        }
    }
}