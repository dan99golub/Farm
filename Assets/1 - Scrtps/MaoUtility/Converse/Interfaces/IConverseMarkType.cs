namespace MaoUtility.Converse.Interfaces
{
    public interface IConverseMarkType : IMarkType
    {
        void Init(IManagerMarkType<IConverseMarkType> parent);
    }
}