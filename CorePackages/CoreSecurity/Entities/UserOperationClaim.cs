using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CorePersistence.Repositories;

namespace CoreSecurity.Entities
{
    public class UserOperationClaim:Entity<int>
    {
        public int UserID { get; set; }
        public int OperationClaimID { get; set; }
        public UserOperationClaim()
        {
            UserID = 0;
            OperationClaimID = 0;
        }

        public virtual User User { get; set; }
        public virtual OperationClaim OperationClaim { get; set; }


        public UserOperationClaim(int userID, int operationClaimID)
        {
            UserID = userID;
            OperationClaimID = operationClaimID;
        }

        public UserOperationClaim(int id,int userID, int operationClaimID):base(id)
        {
            UserID = userID;
            OperationClaimID = operationClaimID;
        }

    }
}
