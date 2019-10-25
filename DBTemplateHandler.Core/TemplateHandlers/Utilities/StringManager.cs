using System;
namespace DBTemplateHandler.Core.TemplateHandlers.Utilities
{
    public class StringManager
    {
        public static int countCharInString(String analyzedString, char searchedCharacter)
        {
            if (analyzedString == null) return -1;
            if (analyzedString.Length < 1) return 0;
            int currentCharacterIndex;
            char currentCharacter;
            int analyzedStringLength = analyzedString.Length;
            int counter = 0;
            for (currentCharacterIndex = 0; currentCharacterIndex < analyzedStringLength; currentCharacterIndex++)
            {
                currentCharacter = analyzedString[currentCharacterIndex];
                if (currentCharacter == searchedCharacter) counter++;
            }
            return counter;
        }

        public static int countStringInString(String analyzedString, String searchedString)
        {
            if (analyzedString == null) return -1;
            String[] splittedAnalyzedString = analyzedString.Split(new string[] { searchedString },StringSplitOptions.None);
            int result = splittedAnalyzedString.Length;
            if (result > 0) result--;
            return result;
        }
    }
}
