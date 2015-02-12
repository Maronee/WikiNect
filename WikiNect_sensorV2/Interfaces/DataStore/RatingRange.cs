using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataStore
{
    public interface RatingRange:RatingQuestions
    {

        int min
        {
            get;
            set;
        }

        int max
        {
            get;
            set;
        }

    }
}
