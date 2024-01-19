using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using School_English.Entity;

namespace School_English.Class
{
    internal class GetEntity: IGetEntity
    {
        private GetEntity(){}

        private static GetEntity instance;  

        private English_SchoolEntities _db;

        public static GetEntity GetInstance()
        {
            if (instance == null)
            {
                instance = new GetEntity();
            }
            return instance;
        }

        public English_SchoolEntities GetDB()
        {
            if (_db == null)
            {
                _db = new English_SchoolEntities();
            }
            return _db;
        }
    }
}
