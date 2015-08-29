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
        [Theory]
        [InlineData("19961124", "11 24 96 RD 03 Kartik Vamsi Vat final")]
        [InlineData("19890925", "19890925_Vrindavan Mathura Srila Gurudeva and Srila Vaman Maharaja 1998 Sept To Nov Part 1")]
        [InlineData("20100916", "2010 Sept 16 Delhi - GVP license signing")]
        [InlineData("20100906", "2010 Sept 6 Delhi - GVP license signing")]
        [InlineData("20100916", "Darshan with Srila BV Narayan Maharaja Sept. 16 2010")]
        [InlineData("20001222", "Srila Narayana Maharaja - San Diego - Dec. 22, 2000")]
        [InlineData("19961105", "Vraj Mandal Parikrima - November 5, 1996 - Mathura Cow Traffic Report")]
        [InlineData("19970131", "Sripad BV Ashrama Maharaja - Honoring Srila Narayana Maharaja - Australia Jan 31, '97")]
        [InlineData("19961111", "Vraj Parikrima - Nov 11, 1996 - Srila Narayana Maharaja 01")]
        [InlineData("20101121", "Srila Gurudeva Darshan 21 Nov. 2010")]
        [InlineData("19911016", "Mercy when there is no hope  Oct 16 1991 Mathura Srila BV Narayana Maharaja")]
        [InlineData("20011220", "122001Hi Srila Gurudeva")]
        [InlineData("20001220", "Hawaii 12/20/2000Srila Narayana Maharaja")]
        [InlineData("19980628", @"Гавайи 1998\06\28")]
        [InlineData("19980612", "1998/06/12 Окланд Калифорния")]
        [InlineData("20070624", "2007-06-24 Беджер Калифорния")]
        [InlineData("20101121", "Даршан Шрила Гурудева Ноябрь 21-2010")]
        [InlineData("20101121", "Darshan 21 November 2010")]
        [InlineData("19960622", "Srila Narayana Goswami Maharaja's Lecture!996/06/22 Fullerton California")]
        //[InlineData("", "")]
        //[InlineData("", "")]
        //[InlineData("", "")]
        //[InlineData("", "")]
        //[InlineData("", "")]


        //[InlineData("", "1998/12/20-21 Bali")]


        //[InlineData("200506", "Srila Bhaktivedanta Narayana Maharaja - Badger, CA USA - June 2005")]
        //[InlineData("199704", "199704xx Jagannath Puri Tota Gopinath Sidha Bakula")]
        //[InlineData("", "20090600_Paris morning walks")]
        //[InlineData("", "Шри Шримад Бхактиведанта Нарайана Махарадж. Матхура. Август 1992.")]
        //[InlineData("", "")]
        //[InlineData("", "")]
        //[InlineData("", "")]
        //[InlineData("", "")]

        //[InlineData("1996", "Vraj Parikrima 1996 - Happy Brijabasi Kirtana")]

        public void TestDateParser(string expectedDateTag, string input)
        {
            var parser = new DateParser();
            Assert.Equal(expectedDateTag, parser.GetDateTag(input));
        }
    }
}
