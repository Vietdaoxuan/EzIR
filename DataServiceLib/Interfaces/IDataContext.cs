using CoreLib.Entities;
using System.Data;

namespace DataServiceLib.Interfaces
{
    public interface IDataContext<in T>
    {
        DataSet Get();

        CResponseMessage Add(T item);

        CResponseMessage Update(T item);
    }
}
