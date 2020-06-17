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
        private string _userGroup = null;
        
        private string _progName = null;
        private string _progVer = null;

        private Mssql _sql = null;
        private string _connWG = null;
        private string _connYF = null;
        private string _connMD = null;
        private string _connSW = null;
        private string _connCG = null;

        private string _apiHost = null;
        private string _updateHost = null;

        public string userId { get { return _userId; } set { _userId = value; } }
        public string userName { get { return _userName; } set { _userName = value; } }
        public string userDpt { get { return _userDpt; } set { _userDpt = value; } }
        public string userGroup { get { return _userGroup; } set { _userGroup = value; } }
        
        public string progName { get { return _progName; } set { _progName = value; } }
        public string progVer { get { return _progVer; } set { _progVer = value; } }

        public Mssql sql { get { return _sql; } set { _sql = value; } }
        public string connWG { get { return _connWG; } set { _connWG = value; } }
        public string connYF { get { return _connYF; } set { _connYF = value; } }
        public string connMD { get { return _connMD; } set { _connMD = value; } }
        public string connSW { get { return _connSW; } set { _connSW = value; } }
        public string connCG { get { return _connCG; } set { _connCG = value; } }

        public string apiHost { get { return _apiHost; } set { _apiHost = value; } }
        public string updateHost { get { return _updateHost; } set { _updateHost = value; } }
    }
}
