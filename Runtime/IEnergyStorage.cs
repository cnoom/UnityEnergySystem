namespace com.cnoom.energy.Runtime
{
    public interface IEnergyStorage 
    {
        void Save<T>(string key, T value);
        int Load<T>(string key, T defaultValue);
    }
}