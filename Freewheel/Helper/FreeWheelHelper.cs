using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Freewheel.Helper
{
    public static class FreeWheelHelper
    {
        public static bool ValidateFilterCriteria(string title, DateTime? releasedDate, string genere)
        {
            if (string.IsNullOrWhiteSpace(title) && !releasedDate.HasValue && string.IsNullOrWhiteSpace(genere))
            {
                return false;
            }
            return true;
        }
    }
}
