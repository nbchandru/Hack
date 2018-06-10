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
            var con = dbConnection.GetDBConnection() as OdbcConnection;
            var commad = new OdbcCommand(queryString, con);
            m_dataReader = commad.ExecuteReader();
        }


        protected virtual void Dispose(bool disposeManaged)
        {
            if (disposeManaged)
            {
                m_dataReader.Close();
            }
        }
        public void Dispose()
        {
            Dispose(true);
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
