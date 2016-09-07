using UnityEngine;
using System.Collections;
using System.Text;

public static class EnfluxUtils {

    /*
     * SUMMARY: Compares string to REGEX to see if it
     * contains a pattern matching COMX. 
     * 
     * Returns StringBuilder containing matching portion
     * of original string or null
     * 
     */
    public static StringBuilder parseFriendlyName(string friendlyName)
    {
        System.Text.RegularExpressions.Regex toComPort =
            new System.Text.RegularExpressions.Regex(@".? \((COM\d+)\)$");

        if (toComPort.IsMatch(friendlyName.ToString()))
        {
            StringBuilder comName = new StringBuilder().Append(
                toComPort.Match(friendlyName.ToString()).Groups[1].Value);

            return comName;
        }else
        {
            return null;
        }
    }
	
}
