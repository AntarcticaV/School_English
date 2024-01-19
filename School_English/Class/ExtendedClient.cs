using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using School_English.Entity;

namespace School_English.Class
{
    public partial class ExtendedClient:Client
    {
        public DateTime? DateOfLastVisit
        {
            get
            {
                if (this.ClientService.Count() != 0)
                {
                   return this.ClientService.OrderByDescending(i => i.StartTime).ToList()[0].StartTime;
                }
            
                return null;
                
            }
        }
        
        public int NumberOfSessions => this.ClientService.Count();

        public bool FlagToBirthday
        {
            get
            {
                if ((this.Birthday.Value.Month == DateTime.Now.Month) && (this.Birthday.Value.Day == DateTime.Now.Day))
                {
                    return true;
                }
                return false;
            }

        }
    }
}
