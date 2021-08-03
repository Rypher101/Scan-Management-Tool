using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IBM_Scan_Manager.Classes
{
    class FindingStatus
    {
        public enum Status: short
        {
            NotReviewed = 0,
            Doubtful = 1,
            FalsePositive = 2,
            PositiveNotRemediated = 3,
            Remediated = 4
        }
    }
}
