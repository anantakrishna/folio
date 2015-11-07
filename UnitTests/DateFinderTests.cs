using Folio.Parsing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace UnitTests
{
    public class DateFinderTests
    {
        [Theory]
        [InlineData(@"NY 26 Sec Ave Arrival Lecture", new string[] { })]
        [InlineData(@"11 24 96 RD 03 Kartik Vamsi Vat final", new string[] { "11 24 96" })]
        [InlineData(@"19890925_Vrindavan Mathura Srila Gurudeva and Srila Vaman Maharaja 1998 Sept To Nov Part 1", new string[] { "19890925", "1998 Sept", "Nov" })]
        [InlineData(@"2010 Sept 16 Delhi - GVP license signing", new string[] { "2010 Sept 16" })]
        [InlineData(@"Darshan with Srila BV Narayan Maharaja Sept. 16 2010", new string[] { "Sept. 16 2010" })]
        [InlineData(@"Vraj Mandal Parikrima - November 5, 1996 - Mathura Cow Traffic Report", new string[] { "November 5, 1996" })]
        [InlineData(@"Hawaii 12/20/2000Srila Narayana Maharaja", new string[] { "12/20/2000" })]
        [InlineData(@"2007-06-24 Беджер Калифорния", new string[] { "2007-06-24" })]
        [InlineData(@"Даршан Шрила Гурудева Ноябрь 21-2010", new string[] { "Ноябрь 21-2010" })]
        [InlineData(@"Darshan 21 November 2010", new string[] { "21 November 2010" })]
        [InlineData(@"2004 Vyasa Puja  1 27 04 Play Pandavas", new string[] { "2004", "1 27 04" })]
        [InlineData(@"AUDIO Badger 05JUN16 --- Morning walk - Most confidential", new string[] { "05JUN16" })]
        [InlineData(@"199803 10 11 12 GM Parikrama Jahnudvip", new string[] { "199803 10 11 12" })]
        [InlineData(@"1998/12/20-21 Bali", new string[] { "1998/12/20-21" })]
        [InlineData(@"07_March_26_pm_part_1_Mathura.wmv", new string[] { "07_March_26" })]
        //[InlineData(@"", new string[] { "" })]

        public void ShouldFindText(string input, string[] expected)
        {
            var finder = new DateTagFinder();
            var actual = finder.FindDateTags(input).ToArray();
            Array.Sort(expected);
            Array.Sort(actual);
            Assert.Equal(expected, actual);
        }
    }
}
