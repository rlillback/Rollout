using Microsoft.SqlServer.Server;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rollout.BLL
{
    public class ShipToLine
    {
        /// <summary>
        /// NOTE: I needed to do this to have JDEAddress appear as a property & show up in the data table
        /// </summary>
        public double _JDEAddress;
        public double JDEAddress
        {
            get { return _JDEAddress; }
            set { _JDEAddress = value; }
        }

        /// <summary>
        /// NOTE: I needed to do this to have StoreNumber appear as a property & show up in the data table
        /// </summary>
        private string _StoreNumber;
        public string StoreNumber
        {
            get { return _StoreNumber; }
            set { _StoreNumber = value; }
        }

        private string _Concept;
        public string Concept
        {
            get 
            { 
                return _Concept;  
            }
            set
            {
                if ( 2 < value.Length )
                {
                    _Concept = value.Substring(0, 2);
                }
                else
                {
                    _Concept = value;
                }
            }
        }

        private string _Address1;
        public string Address1
        {
            get
            {
                return _Address1;
            }
            set
            {
                if (40 < value.Length)
                {
                    _Address1 = value.Substring(0, 40);
                }
                else
                {
                    _Address1 = value;
                }
            }
        }

        private string _Address2;
        public string Address2
        {
            get
            {
                return _Address2;
            }
            set
            {
                if (40 < value.Length)
                {
                    _Address2 = value.Substring(0, 40);
                }
                else
                {
                    _Address2 = value;
                }
            }
        }

        private string _City;
        public string City
        {
            get
            {
                return _City;
            }
            set
            {
                if (25 < value.Length)
                {
                    _City = value.Substring(0, 40);
                }
                else
                {
                    _City = value;
                }
            }
        }

        private string _State;
        public string State
        {
            get
            {
                return _State;
            }
            set
            {
                if (3 < value.Length)
                {
                    _State = value.Substring(0, 3);
                }
                else
                {
                    _State = value;
                }
            }
        }

        private string _Zip;
        public string Zip
        {
            get
            {
                return _Zip;
            }
            set
            {
                if (12 < value.Length)
                {
                    _Zip = value.Substring(0, 12);
                }
                else
                {
                    _Zip = value;
                }
            }
        }

        private string _TaxAreaCode;
        public string TaxAreaCode
        {
            get
            {
                return _TaxAreaCode;
            }
            set
            {
                if (10 < value.Length)
                {
                    _TaxAreaCode = value.Substring(0, 10);
                }
                else
                {
                    _TaxAreaCode = value;
                }
            }
        }

        private string _TaxExplanationCode;
        public string TaxExplanationCode
        {
            get { return _TaxExplanationCode; }
            set
            {
                if ( 2 < value.Length )
                {
                    _TaxExplanationCode = value.Substring(0, 2);
                }
                else
                {
                    _TaxExplanationCode = value;
                }
            }
        }
    }
}
