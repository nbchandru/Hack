using System;
using System.Data.Odbc;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using magHack.core.DataBaseAccess;
using magHack.core.Model;

namespace magHack.core.DataSet
{
    public class DBDataSet : IDataSet
    {
        private OdbcDataReader m_dataReader;

        public DBDataSet(IConnection dbConnection, string queryString)
        {
            m_dataReader = dbConnection.CreateReaderWithQueryString(queryString);

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
            m_dataReader.Cast<PersonModel>();
            return objArray.Select(x => x.ToString()).ToList();
        }

        public bool MoveNext()
        {
            return m_dataReader.Read();
        }
    }
}
