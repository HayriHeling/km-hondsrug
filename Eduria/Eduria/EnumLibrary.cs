﻿using System;
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
        OntbrekendWoord,
        Begrip
    } 
}
