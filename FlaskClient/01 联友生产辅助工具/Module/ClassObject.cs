using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HarveyZ
{
    public class InfoObjectBase
    {
        private string _userId = null;
        private string _userName = null;
        private string _userDpt = null;
        private string _connStr = null;
        private Mssql _sql = null;

        private string _apiUrl = null;

        public string userId { get { return _userId; } set { _userId = value; } }
        public string userName { get { return _userName; } set { _userName = value; } }
        public string userDpt { get { return _userDpt; } set { _userDpt = value; } }

        public string connStr { get { return _connStr; } set { _connStr = value; } }
        public Mssql sql { get { return _sql; } set { _sql = value; } }

        public string apiUrl { get { return _apiUrl; } set { _apiUrl = value; } }
    }
}
