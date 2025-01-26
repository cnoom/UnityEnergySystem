using Cnoom.UnityTool.StorageUtils;

namespace com.cnoom.energy.Runtime
{
    using UnityEngine;

    // 能量数据
    public class EnergyData : IStorageUser
    {
        private int currentEnergy;
        private readonly int maxEnergy;

        public EnergyData(int max)
        {
            maxEnergy = max;
            currentEnergy = Mathf.Clamp(this.GetInt(nameof(currentEnergy), max), 0, maxEnergy);
        }

        public void SetCurrentEnergy(int value)
        {
            currentEnergy = Mathf.Clamp(value, 0, maxEnergy);
            this.SaveInt(nameof(currentEnergy), currentEnergy);
        }

        public int GetCurrentEnergy()
        {
            return currentEnergy;
        }

        public int GetMaxEnergy()
        {
            return maxEnergy;
        }

        public bool IsFull()
        {
            return currentEnergy == maxEnergy;
        }
    }
}