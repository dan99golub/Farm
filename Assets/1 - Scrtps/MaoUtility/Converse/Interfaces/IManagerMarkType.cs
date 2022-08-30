using System;

namespace MaoUtility.Converse.Interfaces
{
    public interface IManagerMarkType<T> where T : IMarkType
    {
        IMarkType GetID(string id);
        
        Tm GetCastByAlias<Tm>(string alias) where Tm : class, T;
        Tm[] GetAllCastByAlias<Tm>(string alias) where Tm : class, T;
        
        Tm GetCast<Tm>()where Tm : class, T;
        Tm[] GetAllCast<Tm>()where Tm : class, T;

        Tm GetPredict<Tm>(Func<Tm, bool> predict) where Tm : class, T;
        Tm[] GetPredictAll<Tm>(Func<Tm, bool> predict)where Tm : class, T;
    }
}