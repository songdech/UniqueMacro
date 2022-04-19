using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UNIQUE
{
    class StainObservations
    {
        private int _stainID;

      
        private int _subRequsetStainID;
        private int _subRequsetMbID;

        
        private string _morphoobservation;
        private string _QUANTITATIVERESULT;  
        private string _COMMENTS;

        //public StainObservations(string morpho)
        //{
        //    this._morphoobservation = morpho;
        //}

        public int SubRequsetMbID
        {
            get { return _subRequsetMbID; }
            set { _subRequsetMbID = value; }
        }
        public string Morphoobservation
        {
            get { return _morphoobservation; }
            set { _morphoobservation = value; }
        }

        public int StainID
        {
            get { return _stainID; }
            set { _stainID = value; }
        }
        public string QUANTITATIVERESULT
        {
            get { return _QUANTITATIVERESULT; }
            set { _QUANTITATIVERESULT = value; }
        }
        
        public int SubRequsetStainID
        {
            get { return _subRequsetStainID; }
            set { _subRequsetStainID = value; }
        }
       
        public string COMMENTS
        {
            get { return _COMMENTS; }
            set { _COMMENTS = value; }
        }
    }

    class StainObservationsForDeleteObservation 
    {

        private int _requestStainID;
        private int _mbRequestID;
        private string _morphoobservation;

        public int MbRequestID
        {
            get { return _mbRequestID; }
            set { _mbRequestID = value; }
        }
       
        public int RequestStainID
        {
            get { return _requestStainID; }
            set { _requestStainID = value; }
        }
       

        public string Morphoobservation
        {
            get { return _morphoobservation; }
            set { _morphoobservation = value; }
        }
    }

    class StainObservationsForDeleteRequestStain
    {
        private int _mbRequestID;

        public int MbRequestID
        {
            get { return _mbRequestID; }
            set { _mbRequestID = value; }
        }
       
    }

}
