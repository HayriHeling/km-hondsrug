using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Eduria
{
    public enum UserRoles
    {
        Admin,
        Docent,
        Student
    }

    public enum MediaType
    {
        Geen,
        Audio,
        Image,
        Video
    }

    /// <summary>
    /// Enum for the different categories.
    /// </summary>
    public enum AnalyticCategory
    {
        Reflectie,
        Werkwijze,
        Leerdoel
    }

    /// <summary>
    /// Enum for the different question types.
    /// </summary>
    public enum QuestionType
    {
        Meerkeuze,
        Tijdvak,
        Open

    } 

    /// <summary>
    /// Enum for the different default options
    /// </summary>
    public enum DefaultOption
    {
        Input,
        Score,
        InputScore
    }

    /// <summary>
    /// Enum for the differect default scores.
    /// </summary>
    public enum DefaultScore
    {
        Rood,
        Oranje,
        Geel,
        LichtGroen,
        Groen
    }
    public enum ChristNotation
    {
        nChr,
        vChr
    }
    public enum Class
    {
        nvt,
        havo,
        vwo
    }
}
