using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SendGrid.Models.Contacts
{
    public class Recipient
    {
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public IEnumerable<CustomField> CustomFields { get; set; }

        public Recipient()
        {
            CustomFields = new CustomField[0]; 
        }
    }
}
