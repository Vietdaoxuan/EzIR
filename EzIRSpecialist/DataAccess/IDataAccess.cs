using CoreLib.Entities.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EzIRSpecialist.DataAccess
{
    /// <summary>
    /// Interface have these methods basic 
    /// </summary>
    /// <typeparam name="T"> Entity or ObjectModelView </typeparam>
    /// <typeparam name="T2"> CResponseMessage </typeparam>
    /// <typeparam name="T3"> ObjectViewModel </typeparam>
    public interface IDataAccess<T, T2, T3>
    {       
        Task<T> Select(T3 obj);

        Task<T2> Insert(T3 obj);

        Task<T2> Update(T3 obj);

        Task<T2> Delete(T3 obj);
    }
}
