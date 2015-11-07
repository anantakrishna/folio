using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Folio;
using Xunit;

namespace UnitTests
{
    public class DateParserTests
    {
        public static IEnumerable<object[]> GetTestData()
        {
            yield return new object[] { @"11 24 96 RD 03 Kartik Vamsi Vat final", new string[] { "19961124" } };
            yield return new object[] { @"19890925_Vrindavan Mathura Srila Gurudeva and Srila Vaman Maharaja 1998 Sept To Nov Part 1", new string[] { "19890925" } };
            yield return new object[] { @"2010 Sept 16 Delhi - GVP license signing", new string[] { "20100916" } };
            yield return new object[] { @"2010 Sept 6 Delhi - GVP license signing", new string[] { "20100906" } };
            yield return new object[] { @"Darshan with Srila BV Narayan Maharaja Sept. 16 2010", new string[] { "20100916" } };
            yield return new object[] { @"Srila Narayana Maharaja - San Diego - Dec. 22, 2000", new string[] { "20001222" } };
            yield return new object[] { @"Vraj Mandal Parikrima - November 5, 1996 - Mathura Cow Traffic Report", new string[] { "19961105" } };
            yield return new object[] { @"Sripad BV Ashrama Maharaja - Honoring Srila Narayana Maharaja - Australia Jan 31, '97", new string[] { "19970131" } };
            yield return new object[] { @"Vraj Parikrima - Nov 11, 1996 - Srila Narayana Maharaja 01", new string[] { "19961111" } };
            yield return new object[] { @"Srila Gurudeva Darshan 21 Nov. 2010", new string[] { "20101121" } };
            yield return new object[] { @"Mercy when there is no hope  Oct 16 1991 Mathura Srila BV Narayana Maharaja", new string[] { "19911016" } };
            yield return new object[] { @"122001Hi Srila Gurudeva", new string[] { "20011220" } };
            yield return new object[] { @"Hawaii 12/20/2000Srila Narayana Maharaja", new string[] { "20001220" } };
            yield return new object[] { @"Гавайи 1998\06\28", new string[] { "19980628" } };
            yield return new object[] { @"1998/06/12 Окланд Калифорния", new string[] { "19980612" } };
            yield return new object[] { @"2007-06-24 Беджер Калифорния", new string[] { "20070624" } };
            yield return new object[] { @"Даршан Шрила Гурудева Ноябрь 21-2010", new string[] { "20101121" } };
            yield return new object[] { @"Darshan 21 November 2010", new string[] { "20101121" } };
            yield return new object[] { @"Srila Narayana Goswami Maharaja's Lecture!996/06/22 Fullerton California", new string[] { "19960622" } };

            yield return new object[] { @"2004 Vyasa Puja  1 27 04 Play Pandavas", new string[] { "20040127" } };
            yield return new object[] { @"2004 Vyasa Puja Tape 7 Class 1_27_04_PM P2", new string[] { "20040127" } };
            yield return new object[] { @"AUDIO Badger 05JUN16 --- Morning walk - Most confidential", new string[] { "20050616" } };
            yield return new object[] { @"07_March_26_pm_part_1_Mathura.wmv", new string[] { "20070326" } };
            yield return new object[] { @"No CA Oakland Arrival June 30 96", new string[] { "19960630" } };
            yield return new object[] { @"NY Arrival July 13 96", new string[] { "19960713" } };
            yield return new object[] { @"24-12-08 Christmas Hari Nama in Hong Kong", new string[] { "20081224" } };
            yield return new object[] { @"Footage taken 22 Sept 2009", new string[] { "20090922" } };
            yield return new object[] { @"6 October, 2009. Today", new string[] { "20091006" } };
            yield return new object[] { @"10 23 96 BVN Gaudiya Math 01 final", new string[] { "19961023" } };

            //yield return new object[] { @"", new string[] {""} };
            //yield return new object[] { @"", new string[] {""} };
            //yield return new object[] { @"", new string[] {""} };

            // Multiple dates
            yield return new object[] { @"1998/12/20-21 Bali", new string[] { "19981220", "19981221" } };
            yield return new object[] { @"199803 10 11 12 GM Parikrama Jahnudvip", new string[] { "19980310", "19980311", "19980312" } };
            yield return new object[] { @"199810 6 7 Vrindavan PM Be Enthusiastic Class Bhajans Seva Kunja Bhajans Imli Tala T4", new string[] { "19981006", "19981007" } };
            yield return new object[] { @"19970516&19 BADGER AM", new string[] { "19970516", "19970519" } };


            //Not enough digits or extra digits
            yield return new object[] { @"199711004 Varshana Darshan PM Disp Srila AC Bhaktivedanta Swami", new string[] { "19971104?", "19971004?" } };
            yield return new object[] { @"1991108_Mathura Extraordinary Explanation of Anyabhilasita", new string[] { "19911008?", "19910108?" } };
            yield return new object[] { @"2005121pm", new string[] { "20050121?", "20051201?" } };

            //Ambiguity due to month-day interchangability
            yield return new object[] { @"10 11 96", new string[] { "19961011?", "19961110?" } };
            
            //Partial dates
            //yield return new object[] { "200506", "Srila Bhaktivedanta Narayana Maharaja - Badger, CA USA - June 2005")]
            //yield return new object[] { "199704", "199704xx Jagannath Puri Tota Gopinath Sidha Bakula")]
            //yield return new object[] { "", "20090600_Paris morning walks")]
            //yield return new object[] { "", "Шри Шримад Бхактиведанта Нарайана Махарадж. Матхура. Август 1992.")]
            //yield return new object[] { "1996", "Vraj Parikrima 1996 - Happy Brijabasi Kirtana")]
            //yield return new object[] { @"", new string[] {""} };
            //yield return new object[] { @"", new string[] {""} };
            //yield return new object[] { @"Sri Srimad Bhaktivedanta Narayan Goswami Maharaja in December of 2004.", new string[] {"200412"} };
            //yield return new object[] { @"2004 12", new string[] {"200412"} };
            //yield return new object[] { "200304-05-Alachua P2", new string[] {"200304", "200305"} };
        }

        [Theory]
        [MemberData("GetTestData")]
        public void TestDateParser(string input, string[] expectedTags)
        {
            var parser = new DateParser();
            var actualTags = parser.GetDateTags(input).ToArray();
            Array.Sort(expectedTags);
            Array.Sort(actualTags);
            Assert.Equal(expectedTags, actualTags);
        }

    }
}
