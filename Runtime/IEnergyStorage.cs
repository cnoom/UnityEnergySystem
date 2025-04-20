namespace com.cnoom.energy.Runtime
{
    public interface IEnergyStorage
    {
        void Save<T>(string key, T value);
        T Load<T>(string key, T defaultValue);
    }
}