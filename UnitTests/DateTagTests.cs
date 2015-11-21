using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Folio;
using Xunit;

namespace UnitTests
{
    public class DateTagTests
    {
        [Fact]
        public void ShouldBeEqual()
        {
            var tag1 = new DateTag("1999-10-30");
            var tag2 = new DateTag("1999-10-30");
            Assert.Equal(tag1, tag2);
        }

        [Fact]
        public void ShouldNotBeEqual()
        {
            var tag1 = new DateTag("1999-10-30");
            var tag2 = new DateTag("1999-10-31");
            Assert.NotEqual(tag1, tag2);
        }

        public static IEnumerable<object[]> GetTestData()
        {
            yield return new object[] { @"11 24 96 RD 03 Kartik Vamsi Vat final", new DateTag[] { "1996-11-24" } };
            yield return new object[] { @"19890925_Vrindavan Mathura Srila Gurudeva and Srila Vaman Maharaja 1998 Sept To Nov Part 1", new DateTag[] { "1989-09-25" } };
            yield return new object[] { @"2010 Sept 16 Delhi - GVP license signing", new DateTag[] { "2010-09-16" } };
            yield return new object[] { @"2010 Sept 6 Delhi - GVP license signing", new DateTag[] { "2010-09-06" } };
            yield return new object[] { @"Darshan with Srila BV Narayan Maharaja Sept. 16 2010", new DateTag[] { "2010-09-16" } };
            yield return new object[] { @"Srila Narayana Maharaja - San Diego - Dec. 22, 2000", new DateTag[] { "2000-12-22" } };
            yield return new object[] { @"Vraj Mandal Parikrima - November 5, 1996 - Mathura Cow Traffic Report", new DateTag[] { "1996-11-05" } };
            yield return new object[] { @"Sripad BV Ashrama Maharaja - Honoring Srila Narayana Maharaja - Australia Jan 31, '97", new DateTag[] { "1997-01-31" } };
            yield return new object[] { @"Vraj Parikrima - Nov 11, 1996 - Srila Narayana Maharaja 01", new DateTag[] { "1996-11-11" } };
            yield return new object[] { @"Srila Gurudeva Darshan 21 Nov. 2010", new DateTag[] { "2010-11-21" } };
            yield return new object[] { @"Mercy when there is no hope  Oct 16 1991 Mathura Srila BV Narayana Maharaja", new DateTag[] { "1991-10-16" } };
            yield return new object[] { @"122001Hi Srila Gurudeva", new DateTag[] { "2001-12-20" } };
            yield return new object[] { @"Hawaii 12/20/2000Srila Narayana Maharaja", new DateTag[] { "2000-12-20" } };
            yield return new object[] { @"Гавайи 1998\06\28", new DateTag[] { "1998-06-28" } };
            yield return new object[] { @"1998/06/12 Окланд Калифорния", new DateTag[] { "1998-06-12" } };
            yield return new object[] { @"2007-06-24 Беджер Калифорния", new DateTag[] { "2007-06-24" } };
            yield return new object[] { @"Даршан Шрила Гурудева Ноябрь 21-2010", new DateTag[] { "2010-11-21" } };
            yield return new object[] { @"Darshan 21 November 2010", new DateTag[] { "2010-11-21" } };
            yield return new object[] { @"Srila Narayana Goswami Maharaja's Lecture!996/06/22 Fullerton California", new DateTag[] { "1996-06-22" } };

            yield return new object[] { @"2004 Vyasa Puja  1 27 04 Play Pandavas", new DateTag[] { "2004-01-27" } };
            yield return new object[] { @"2004 Vyasa Puja Tape 7 Class 1_27_04_PM P2", new DateTag[] { "2004-01-27" } };
            yield return new object[] { @"AUDIO Badger 05JUN16 --- Morning walk - Most confidential", new DateTag[] { "2005-06-16" } };
            yield return new object[] { @"07_March_26_pm_part_1_Mathura.wmv", new DateTag[] { "2007-03-26" } };
            yield return new object[] { @"No CA Oakland Arrival June 30 96", new DateTag[] { "1996-06-30" } };
            yield return new object[] { @"NY Arrival July 13 96", new DateTag[] { "1996-07-13" } };
            yield return new object[] { @"24-12-08 Christmas Hari Nama in Hong Kong", new DateTag[] { "2008-12-24" } };
            yield return new object[] { @"Footage taken 22 Sept 2009", new DateTag[] { "2009-09-22" } };
            yield return new object[] { @"6 October, 2009. Today", new DateTag[] { "2009-10-06" } };
            yield return new object[] { @"10 23 96 BVN Gaudiya Math 01 final", new DateTag[] { "1996-10-23" } };

            //yield return new object[] { @"", new DateTag[] {""} };
            //yield return new object[] { @"", new DateTag[] {""} };
            //yield return new object[] { @"", new DateTag[] {""} };

            // Multiple dates
            yield return new object[] { @"1998/12/20-21 Bali", new DateTag[] { "1998-12-20", "1998-12-21" } };
            yield return new object[] { @"199803 10 11 12 GM Parikrama Jahnudvip", new DateTag[] { "1998-03-10", "1998-03-11", "1998-03-12" } };
            yield return new object[] { @"199810 6 7 Vrindavan PM Be Enthusiastic Class Bhajans Seva Kunja Bhajans Imli Tala T4", new DateTag[] { "1998-10-06", "1998-10-07" } };
            yield return new object[] { @"19970516&19 BADGER AM", new DateTag[] { "1997-05-16", "1997-05-19" } };


            //Not enough digits or extra digits
            //yield return new object[] { @"199711004 Varshana Darshan PM Disp Srila AC Bhaktivedanta Swami", new DateTag[] { "19971104?", "19971004?" } };
            //yield return new object[] { @"1991108_Mathura Extraordinary Explanation of Anyabhilasita", new DateTag[] { "19911008?", "19910108?" } };
            //yield return new object[] { @"2005121pm", new DateTag[] { "20050121?", "20051201?" } };

            //Ambiguity due to month-day interchangability
            //yield return new object[] { @"10 11 96", new DateTag[] { "19961011?", "19961110?" } };
            
            //Partial dates
            //yield return new object[] { "200506", "Srila Bhaktivedanta Narayana Maharaja - Badger, CA USA - June 2005")]
            //yield return new object[] { "199704", "199704xx Jagannath Puri Tota Gopinath Sidha Bakula")]
            //yield return new object[] { "", "20090600_Paris morning walks")]
            //yield return new object[] { "", "Шри Шримад Бхактиведанта Нарайана Махарадж. Матхура. Август 1992.")]
            //yield return new object[] { "1996", "Vraj Parikrima 1996 - Happy Brijabasi Kirtana")]
            //yield return new object[] { @"", new DateTag[] {""} };
            //yield return new object[] { @"", new DateTag[] {""} };
            //yield return new object[] { @"Sri Srimad Bhaktivedanta Narayan Goswami Maharaja in December of 2004.", new DateTag[] {"200412"} };
            //yield return new object[] { @"2004 12", new DateTag[] {"200412"} };
            //yield return new object[] { "200304-05-Alachua P2", new DateTag[] {"200304", "200305"} };
        }

        [Theory]
        [MemberData("GetTestData")]
        public void TestDateParser(string input, DateTag[] expectedTags)
        {
            var actualTags = DateTag.Find(input).ToArray();

            Array.Sort(expectedTags);
            Array.Sort(actualTags);
            Assert.Equal(expectedTags, actualTags);
        }

    }
}
