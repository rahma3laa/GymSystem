using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagementDAL.Entities
{
    public class Member : GymUser
    {
        //Join Name == CreatedAt Of BaseEntity
        public string? Photo { get; set; }

        #region Member - HealthRecord

        public HealthRecord HealthRecord { get; set; } = null!;
        #endregion

        #region RelationShip

      

        #region Member - MemberShip 

        public ICollection<MemberShip> MemberShips { get; set; } = null!;
        #endregion
        #endregion

        public ICollection<MemberSession> MemberSessions { get; set; } = null!;


    }
}
