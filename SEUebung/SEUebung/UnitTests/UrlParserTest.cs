using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace SEUebung.UnitTests
{

    public class UrlParserTest
    {
        #region [public Test]
        [Fact]
        public void Parameter_Count_UrlwithtwoParameters_Two()
        {
            Assert.Equal(2, _parser2params.ParameterCount);
        }
        [Fact]
        public void Path_UrlwithParams_httpsexampleorg()
        {
            Assert.Equal(@"https://example.org/", _parser2params.Path);
        }
        [Fact]
        public void Parameter_UrlwithtwoParameters_One()
        {
            IDictionary<string, string> test = _parser2params.Parameter;
            test.TryGetValue("a", out string value);
            Assert.Equal("1", value);
        }
        [Fact]
        public void Parameter_EmptyString_NotNull()
        {
            Assert.NotNull(_parserEmpty.Parameter);
        }
        [Fact]
        public void ParameterCount_EmptyString_Zero()
        {
            Assert.Equal(0, _parserEmpty.ParameterCount);
        }
        [Fact]
        public void Extension_UrlwithFile_Org()
        {
            Assert.Equal(".org", _parserWithFile.Extension);
        }
        [Fact]
        public void FileName_UrlWithFile_Testorg()
        {
            Assert.Equal("test.org", _parserWithFile.FileName);
        }
        [Fact]
        public void Fragement_UrlWithNoFragement_StringEmpty()
        {
            Assert.Equal(string.Empty, _parserWithFile.Fragment);
        }
        [Fact]
        public void Fragement_UrlWithFragement_Ressource()
        {
            Assert.Equal("ressource", _parserWithFragement.Fragment);
        }
        #endregion

        #region [private]
        private static string _url2params = @"https://example.org/?a=1&b=2";
        private static string _urlwithfile = @"https://example.org/test.org?a=1&b=2";
        private static string _urlwithfragement = @"https://example.org/test.org?a=1&b=2#ressource";
        private static string _urlempty = string.Empty;
        private Url _parser2params = new Url(_url2params);
        private Url _parserEmpty = new Url(_urlempty);
        private Url _parserWithFile = new Url(_urlwithfile);
        private Url _parserWithFragement = new Url(_urlwithfragement);
        #endregion [private]

    }

}
