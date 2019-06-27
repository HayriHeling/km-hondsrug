using System;
using System.Runtime.Serialization;

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