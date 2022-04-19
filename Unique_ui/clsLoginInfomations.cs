using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UNIQUE
{
    class clsLoginInfomations
    {
        private string _name;
         private string _lastname;
        private string _telephone;
        private string _email;
        private string _position;
        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }     

        public string Lastname
        {
            get { return _lastname; }
            set { _lastname = value; }
        }        

        public string Telephone
        {
            get { return _telephone; }
            set { _telephone = value; }
        }    

        public string Email
        {
            get { return _email; }
            set { _email = value; }
        }

        public string Position
        {
            get { return _position; }
            set { _position = value; }
        }
    }
}
