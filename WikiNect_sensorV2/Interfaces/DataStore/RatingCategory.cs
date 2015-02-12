using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataStore
{
    public interface RatingCategory:WikiNectElement
    {

        string Name
        {
            get;
            set;
        }
        List<RatingQuestions> questions
        {
            get;
            set;
        }

    }
}
