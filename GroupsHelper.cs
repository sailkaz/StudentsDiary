using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentsDiary
{
    public static class GroupsHelper
    {
       
        public static List<Group> GetGroup(string deafaultName)
        {
            return new List<Group>
            {
                new Group { Id = 0, Name = deafaultName },
                new Group { Id = 1, Name = "1a" },
                new Group { Id = 2, Name = "2a" },
            };

        }
        
    }
}
