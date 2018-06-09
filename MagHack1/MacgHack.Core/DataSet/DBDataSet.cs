using System;
using System.Data.Odbc;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MacgHack.Core.Model;
using MacgHack.Core.DataBaseAccess;

namespace MacgHack.Core.DataSet
{
    public class DBDataSet : IDataSet
    {
        private OdbcDataReader m_dataReader;

        private Type m_resultType;
        public DBDataSet(IConnection dbConnection, string queryString,Cl resultType)
        {
            m_dataReader = dbConnection.CreateReaderWithQueryString(queryString);
            m_resultType = resultType;

        }


        protected virtual void Dispose(bool disposeManaged)
        {
            if (disposeManaged)
            {
                Dispose();
            }
        }
        public void Dispose()
        {
            m_dataReader.Close();
        }
        public List<string> GetRow()
        {
            var objArray = new Object[4];
            var a = m_dataReader.GetValues(objArray);
            return objArray.Select(x => x.ToString()).ToList();
        }

        public bool MoveNext()
        {
            return m_dataReader.Read();
        }
    }
}
