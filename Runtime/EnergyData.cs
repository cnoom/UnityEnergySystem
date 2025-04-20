using UnityEngine;

namespace com.cnoom.energy.Runtime
{

    public class EnergyData
    {
        private readonly int maxEnergy;
        // 新增存储接口字段
        private readonly IEnergyStorage storage;

        private int currentEnergy;

        // 修改构造函数接收存储接口
        public EnergyData(int max, IEnergyStorage storage)
        {
            this.storage = storage;
            maxEnergy = max;
            currentEnergy = Mathf.Clamp(storage.Load(nameof(currentEnergy), max), 0, maxEnergy);
        }

        public void SetCurrentEnergy(int value)
        {
            currentEnergy = Mathf.Clamp(value, 0, maxEnergy);
            storage.Save(nameof(currentEnergy), currentEnergy);
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