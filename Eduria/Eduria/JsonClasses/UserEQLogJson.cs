using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace Eduria.JsonClasses
{
    [DataContract]
    public class UserEqLogJson
    {
        [DataMember] public int QuestionId;
        [DataMember] public int TimesWrong;
        [DataMember] public int CorrectAnswered;
        [DataMember] public DateTime AnsweredOn;
    }
}
