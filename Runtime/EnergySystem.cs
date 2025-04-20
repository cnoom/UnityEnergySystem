
namespace com.cnoom.energy.Runtime
{
    public class EnergySystem
    {
        private EnergyData data;
        private EnergyRecovery energyRecovery;

        private EnergySystem(EnergyData energyData, IEnergyRecoveryRule energyRecoveryRule)
        {
            data = energyData;
            energyRecovery = new EnergyRecovery(energyRecoveryRule, energyData);
        }

        public void UpdateRecover()
        {
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