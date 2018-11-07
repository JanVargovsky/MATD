using System;

namespace MATD.Lesson5
{
    // Source: https://tartarus.org/martin/PorterStemmer/

    /// <summary>
    /// The Stemmer class transforms a word into its root form.
    /// Implementing the Porter Stemming Algorithm
    /// </summary>
    /// <remarks>
    /// Modified from: http://tartarus.org/martin/PorterStemmer/csharp2.txt
    /// </remarks>
    /// <example>
    /// var stemmer = new PorterStemmer();
    /// var stem = stemmer.StemWord(word);
    /// </example>
    public class PorterStemmerUpper
    {

        // The passed in word turned into a char array. 
        // Quicker to use to rebuilding strings each time a change is made.
        private char[] wordArray;

        // Current index to the end of the word in the character array. This will
        // change as the end of the string gets modified.
        private int endIndex;

        // Index of the (potential) end of the stem word in the char array.
        private int stemIndex;


        /// <summary>
        /// Stem the passed in word.
        /// </summary>
        /// <param name="word">Word to evaluate</param>
        /// <returns></returns>
        public string StemWord(string word)
        {

            // Do nothing for empty strings or short words.
            if (string.IsNullOrWhiteSpace(word) || word.Length <= 2) return word;

            wordArray = word.ToCharArray();

            stemIndex = 0;
            endIndex = word.Length - 1;
            Step1();
            Step2();
            Step3();
            Step4();
            Step5();
            Step6();

            var length = endIndex + 1;
            return new String(wordArray, 0, length);
        }


        // Step1() gets rid of plurals and -ed or -ing.
        /* Examples:
               caresses  ->  caress
               ponies    ->  poni
               ties      ->  ti
               caress    ->  caress
               cats      ->  cat

               feed      ->  feed
               agreed    ->  agree
               disabled  ->  disable

               matting   ->  mat
               mating    ->  mate
               meeting   ->  meet
               milling   ->  mill
               messing   ->  mess

               meetings  ->  meet  		*/
        private void Step1()
        {
            // If the word ends with s take that off
            if (wordArray[endIndex] == 'S')
            {
                if (EndsWith("SSES"))
                {
                    endIndex -= 2;
                }
                else if (EndsWith("IES"))
                {
                    SetEnd("I");
                }
                else if (wordArray[endIndex - 1] != 'S')
                {
                    endIndex--;
                }
            }
            if (EndsWith("EED"))
            {
                if (MeasureConsontantSequence() > 0)
                    endIndex--;
            }
            else if ((EndsWith("ED") || EndsWith("ING")) && VowelInStem())
            {
                endIndex = stemIndex;
                if (EndsWith("AT"))
                    SetEnd("ATE");
                else if (EndsWith("BL"))
                    SetEnd("BLE");
                else if (EndsWith("IZ"))
                    SetEnd("IZE");
                else if (IsDoubleConsontant(endIndex))
                {
                    endIndex--;
                    int ch = wordArray[endIndex];
                    if (ch == 'L' || ch == 'S' || ch == 'Z')
                        endIndex++;
                }
                else if (MeasureConsontantSequence() == 1 && IsCVC(endIndex)) SetEnd("E");
            }
        }

        // Step2() turns terminal y to i when there is another vowel in the stem.
        private void Step2()
        {
            if (EndsWith("Y") && VowelInStem())
                wordArray[endIndex] = 'I';
        }

        // Step3() maps double suffices to single ones. so -ization ( = -ize plus
        // -ation) maps to -ize etc. note that the string before the suffix must give m() > 0. 
        private void Step3()
        {
            if (endIndex == 0) return;

            /* For Bug 1 */
            switch (wordArray[endIndex - 1])
            {
                case 'A':
                    if (EndsWith("ATIONAL")) { ReplaceEnd("ATE"); break; }
                    if (EndsWith("TIONAL")) { ReplaceEnd("TION"); }
                    break;
                case 'C':
                    if (EndsWith("ENCI")) { ReplaceEnd("ENCE"); break; }
                    if (EndsWith("ANCI")) { ReplaceEnd("ANCE"); }
                    break;
                case 'E':
                    if (EndsWith("IZER")) { ReplaceEnd("IZE"); }
                    break;
                case 'L':
                    if (EndsWith("BLI")) { ReplaceEnd("BLE"); break; }
                    if (EndsWith("ALLI")) { ReplaceEnd("AL"); break; }
                    if (EndsWith("ENTLI")) { ReplaceEnd("ENT"); break; }
                    if (EndsWith("ELI")) { ReplaceEnd("E"); break; }
                    if (EndsWith("OUSLI")) { ReplaceEnd("OUS"); }
                    break;
                case 'O':
                    if (EndsWith("IZATION")) { ReplaceEnd("IZE"); break; }
                    if (EndsWith("ATION")) { ReplaceEnd("ATE"); break; }
                    if (EndsWith("ATOR")) { ReplaceEnd("ATE"); }
                    break;
                case 'S':
                    if (EndsWith("ALISM")) { ReplaceEnd("AL"); break; }
                    if (EndsWith("IVENESS")) { ReplaceEnd("IVE"); break; }
                    if (EndsWith("FULNESS")) { ReplaceEnd("FUL"); break; }
                    if (EndsWith("OUSNESS")) { ReplaceEnd("OUS"); }
                    break;
                case 'T':
                    if (EndsWith("ALITI")) { ReplaceEnd("AL"); break; }
                    if (EndsWith("IVITI")) { ReplaceEnd("IVE"); break; }
                    if (EndsWith("BILITI")) { ReplaceEnd("BLE"); }
                    break;
                case 'G':
                    if (EndsWith("LOGI"))
                    {
                        ReplaceEnd("LOG");
                    }
                    break;
            }
        }

        /* step4() deals with -ic-, -full, -ness etc. similar strategy to step3. */
        private void Step4()
        {
            switch (wordArray[endIndex])
            {
                case 'E':
                    if (EndsWith("ICATE")) { ReplaceEnd("IC"); break; }
                    if (EndsWith("ATIVE")) { ReplaceEnd(""); break; }
                    if (EndsWith("ALIZE")) { ReplaceEnd("AL"); }
                    break;
                case 'I':
                    if (EndsWith("ICITI")) { ReplaceEnd("IC"); }
                    break;
                case 'L':
                    if (EndsWith("ICAL")) { ReplaceEnd("IC"); break; }
                    if (EndsWith("FUL")) { ReplaceEnd(""); }
                    break;
                case 'S':
                    if (EndsWith("NESS")) { ReplaceEnd(""); }
                    break;
            }
        }

        /* step5() takes off -ant, -ence etc., in context <c>vcvc<v>. */
        private void Step5()
        {
            if (endIndex == 0) return;

            switch (wordArray[endIndex - 1])
            {
                case 'A':
                    if (EndsWith("AL")) break; return;
                case 'C':
                    if (EndsWith("ANCE")) break;
                    if (EndsWith("ENCE")) break; return;
                case 'E':
                    if (EndsWith("ER")) break; return;
                case 'I':
                    if (EndsWith("IC")) break; return;
                case 'L':
                    if (EndsWith("ABLE")) break;
                    if (EndsWith("IBLE")) break; return;
                case 'N':
                    if (EndsWith("ANT")) break;
                    if (EndsWith("EMENT")) break;
                    if (EndsWith("MENT")) break;
                    /* element etc. not stripped before the m */
                    if (EndsWith("ENT")) break; return;
                case 'O':
                    if (EndsWith("ION") && stemIndex >= 0 && (wordArray[stemIndex] == 'S' || wordArray[stemIndex] == 'T')) break;
                    /* j >= 0 fixes Bug 2 */
                    if (EndsWith("OU")) break; return;
                /* takes care of -ous */
                case 'S':
                    if (EndsWith("ISM")) break; return;
                case 'T':
                    if (EndsWith("ATE")) break;
                    if (EndsWith("ITI")) break; return;
                case 'U':
                    if (EndsWith("OUS")) break; return;
                case 'V':
                    if (EndsWith("IVE")) break; return;
                case 'Z':
                    if (EndsWith("IZE")) break; return;
                default:
                    return;
            }
            if (MeasureConsontantSequence() > 1)
                endIndex = stemIndex;
        }

        /* step6() removes a final -e if m() > 1. */
        private void Step6()
        {
            stemIndex = endIndex;

            if (wordArray[endIndex] == 'E')
            {
                var a = MeasureConsontantSequence();
                if (a > 1 || a == 1 && !IsCVC(endIndex - 1))
                    endIndex--;
            }
            if (wordArray[endIndex] == 'L' && IsDoubleConsontant(endIndex) && MeasureConsontantSequence() > 1)
                endIndex--;
        }

        // Returns true if the character at the specified index is a consonant.
        // With special handling for 'Y'.
        private bool IsConsonant(int index)
        {
            var c = wordArray[index];
            if (c == 'A' || c == 'E' || c == 'I' || c == 'O' || c == 'U') return false;
            return c != 'Y' || (index == 0 || !IsConsonant(index - 1));
        }

        /* m() measures the number of consonant sequences between 0 and j. if c is
           a consonant sequence and v a vowel sequence, and <..> indicates arbitrary
           presence,

              <c><v>       gives 0
              <c>vc<v>     gives 1
              <c>vcvc<v>   gives 2
              <c>vcvcvc<v> gives 3
              ....		*/
        private int MeasureConsontantSequence()
        {
            var n = 0;
            var index = 0;
            while (true)
            {
                if (index > stemIndex) return n;
                if (!IsConsonant(index)) break; index++;
            }
            index++;
            while (true)
            {
                while (true)
                {
                    if (index > stemIndex) return n;
                    if (IsConsonant(index)) break;
                    index++;
                }
                index++;
                n++;
                while (true)
                {
                    if (index > stemIndex) return n;
                    if (!IsConsonant(index)) break;
                    index++;
                }
                index++;
            }
        }

        // Return true if there is a vowel in the current stem (0 ... stemIndex)
        private bool VowelInStem()
        {
            int i;
            for (i = 0; i <= stemIndex; i++)
            {
                if (!IsConsonant(i)) return true;
            }
            return false;
        }

        // Returns true if the char at the specified index and the one preceeding it are the same consonants.
        private bool IsDoubleConsontant(int index)
        {
            if (index < 1) return false;
            return wordArray[index] == wordArray[index - 1] && IsConsonant(index);
        }

        /* cvc(i) is true <=> i-2,i-1,i has the form consonant - vowel - consonant
           and also if the second c is not w,x or y. this is used when trying to
           restore an e at the end of a short word. e.g.

              cav(e), lov(e), hop(e), crim(e), but
              snow, box, tray.		*/
        private bool IsCVC(int index)
        {
            if (index < 2 || !IsConsonant(index) || IsConsonant(index - 1) || !IsConsonant(index - 2)) return false;
            var c = wordArray[index];
            return c != 'W' && c != 'X' && c != 'Y';
        }

        // Does the current word array end with the specified string.
        private bool EndsWith(string s)
        {
            var length = s.Length;
            var index = endIndex - length + 1;
            if (index < 0) return false;

            for (var i = 0; i < length; i++)
            {
                if (wordArray[index + i] != s[i]) return false;
            }
            stemIndex = endIndex - length;
            return true;
        }

        // Set the end of the word to s.
        // Starting at the current stem pointer and readjusting the end pointer.
        private void SetEnd(string s)
        {
            var length = s.Length;
            var index = stemIndex + 1;
            for (var i = 0; i < length; i++)
            {
                wordArray[index + i] = s[i];
            }
            // Set the end pointer to the new end of the word.
            endIndex = stemIndex + length;
        }

        // Conditionally replace the end of the word
        private void ReplaceEnd(string s)
        {
            if (MeasureConsontantSequence() > 0) SetEnd(s);
        }
    }
}
