﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace SEUebung.UnitTests
{
    public class ResponseTest
    {
        [Fact]
        public void SetStatuscode_Set200_Status200OK()
        {
            Response res = new Response();
            res.StatusCode = 200;
            Assert.Equal("200 OK", res.Status);
            Assert.Equal(200, res.StatusCode);
        }

        [Fact]
        public void SetStatuscode_Set404_Status404()
        {
            Response res = new Response();
            res.StatusCode = 404;
            Assert.Equal("404 NOT FOUND", res.Status);
            Assert.Equal(404, res.StatusCode);
        }

        [Fact]
        public void SetStatuscode_Set400_Status400BadRequest()
        {
            Response res = new Response();
            res.StatusCode = 400;
            Assert.Equal("400 BAD REQUEST", res.Status);
        }
        [Fact]
        public void SetStatuscode_Setrandom_StatusEmpty()
        {
            Response res = new Response();
            res.StatusCode = 234234;
            Assert.Equal(string.Empty, res.Status);
        }
        [Fact]
        public void AddHeader_SetTest_HeaderNotIncludesTest()
        {
            Response res = new Response();
            res.AddHeader("Test", "test");
            Assert.True(res.Headers.ContainsKey("Test"));
        }
        [Fact]
        public void SetContent_SetStringLength2_ContentLenghtof2()
        {
            Response res = new Response();
            res.SetContent("Te");
            Assert.Equal(2, res.ContentLength);
        }
        [Fact]
        public void SetContent_SetByteArryLength4_ContentLenghtof4()
        {
            Response res = new Response();
            string str = "TEST";
            byte[] strbyte = Encoding.UTF8.GetBytes(str);
            res.SetContent(strbyte);
            Assert.Equal(4, res.ContentLength);
        }
        [Fact]
        public void response_replace_header()
        {
            Response res = new Response();
            res.AddHeader("foo", "bar");
            Assert.True(res.Headers.ContainsKey("foo"));
            Assert.Equal("bar", res.Headers["foo"]);
            res.AddHeader("foo", "override");
            Assert.True(res.Headers.ContainsKey("foo"));
            Assert.Equal("override", res.Headers["foo"]);
        }
    }
}
