using IBM_Scan_Manager.Models;
using System.Collections.Generic;

namespace IBM_Scan_Manager.Classes
{
    public sealed class Findings
    {
        private static Findings instatnce = null;
        private List<TblAssessment> all;
        private List<TblAssessment> filtered;

        private Findings()
        {
            using (var context = new IBMScanDBContext())
            {
                //all = context.TblAssessments.
            }
        }

        public static Findings Instance
        {
            get
            {
                if (instatnce==null)
                {
                    instatnce = new Findings();
                }
                return instatnce;
            }
        }
    }
}
