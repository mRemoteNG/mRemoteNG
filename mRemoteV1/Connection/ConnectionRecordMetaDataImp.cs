using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;

namespace mRemoteNG.Connection
{
    public class ConnectionRecordMetaDataImp : ConnectionRecordMetaData
    {
        private bool _IsContainer; // refactor code so this isn't required
        private int _PositionID; // refactor code so this isn't required
        private bool _IsDefault; // also not sure if this var should be here
        private bool _IsQuickConnect;
        private bool _PleaseConnect;

        #region Properties
        [Browsable(false)]
        public bool IsContainer
        {
            get { return _IsContainer; }
            set { _IsContainer = value; }
        }

        [Browsable(false)]
        public bool IsDefault
        {
            get { return _IsDefault; }
            set { _IsDefault = value; }
        }

        [Browsable(false)]
        public int PositionID
        {
            get { return _PositionID; }
            set { _PositionID = value; }
        }

        [Browsable(false)]
        public bool IsQuickConnect
        {
            get { return _IsQuickConnect; }
            set { _IsQuickConnect = value; }
        }

        [Browsable(false)]
        public bool PleaseConnect
        {
            get { return _PleaseConnect; }
            set { _PleaseConnect = value; }
        }
        #endregion


        public ConnectionRecordMetaDataImp()
        {
            this.SetDefaults();
        }

        public object Clone()
        {
            return this.MemberwiseClone();
        }

        private void SetDefaults()
        {
            _IsContainer = false;
            _IsDefault = false;
            _PositionID = 0;
            _IsQuickConnect = false;
            _PleaseConnect = false;
        }
    }
}