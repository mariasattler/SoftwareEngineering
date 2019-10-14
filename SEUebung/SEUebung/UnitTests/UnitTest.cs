using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace SEUebung.UnitTests
{
    public class UnitTest
    {

        [Fact]
        public void ParametersCount_UrlwithtwoParameters_Two()
        {
            string url = @"https://example.org/?a=1&b=2";
            UrlParser parser = new UrlParser(url);
            Assert.Equal(2, parser.ParameterCount);
        }
        [Fact]
        public void Path_UrlwithParams_httpsexampleorg()
        {
            string url = @"https://example.org/?a=1&b=2";
            UrlParser parser = new UrlParser(url);
            Assert.Equal(@"https://example.org/", parser.Path);
        }
        [Fact]
        public void Parameter_UrlwithtwoParameters_One()
        {
            string url = @"https://example.org/?a=1&b=2";
            UrlParser parser = new UrlParser(url);
            IDictionary<string, string> test = parser.Parameter;
            string value = string.Empty;
            test.TryGetValue("a", out value);
            Assert.Equal("1", value);
        }
        [Fact]
        public void Parameter_EmptyString_NotNull()
        {
            string url = string.Empty;
            UrlParser parser = new UrlParser(url);
            Assert.NotNull(parser.Parameter);
        }
        [Fact]
        public void ParameterCount_EmptyString_Zero()
        {
            string url = string.Empty;
            UrlParser parser = new UrlParser(url);
            Assert.Equal(0,parser.ParameterCount);
        }


    }

}
