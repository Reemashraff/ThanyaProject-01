using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ThanyaProject.Models.Enums;

namespace ThanyaProject.Models.Enums
{
    public enum BloodType
    {
        A_Positive,   
        A_Negative,   
        B_Positive,  
        B_Negative,   
        AB_Positive,  
        AB_Negative, 
        O_Positive,   
        O_Negative    
    }
}

public static class BloodTypeCase
{
    public static bool TryParseBloodType(string input, out BloodType bloodType)
    {
        bloodType = default;

        if (string.IsNullOrWhiteSpace(input))
            return false;

        input = input.Trim().ToUpper(); 

        switch (input)
        {
            case "A+": case "A_POSITIVE": bloodType = BloodType.A_Positive; return true;
            case "A-": case "A_NEGATIVE": bloodType = BloodType.A_Negative; return true;
            case "B+": case "B_POSITIVE": bloodType = BloodType.B_Positive; return true;
            case "B-": case "B_NEGATIVE": bloodType = BloodType.B_Negative; return true;
            case "AB+": case "AB_POSITIVE": bloodType = BloodType.AB_Positive; return true;
            case "AB-": case "AB_NEGATIVE": bloodType = BloodType.AB_Negative; return true;
            case "O+": case "O_POSITIVE": bloodType = BloodType.O_Positive; return true;
            case "O-": case "O_NEGATIVE": bloodType = BloodType.O_Negative; return true;
            default: return false;
        }
    }
}