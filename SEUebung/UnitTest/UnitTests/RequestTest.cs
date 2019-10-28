﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace SEUebung.UnitTests
{
    public class RequestTest
    {
        [Fact]
        public void request_should_not_be_null()
        {
            var obj = new Request(RequestHelper.GetInvalidRequestStream());
            Assert.NotNull(obj);
        }

        [Fact]
        public void request_not_valid_onInvalidRequest()
        {
            var obj = new Request(RequestHelper.GetInvalidRequestStream());
            Assert.False(obj.IsValid);
        }
        [Fact]
        public void request_is_valid_onValidRequest()
        {
            var obj = new Request(RequestHelper.GetValidRequestStream("/"));
            Assert.True(obj.IsValid);
        }
        [Fact]
        public void request_is_not_valid_onEmptyRequest()
        {
            var obj = new Request(RequestHelper.GetEmptyRequestStream());
            Assert.NotNull(obj);
            Assert.False(obj.IsValid);
        }
        [Fact]
        public void request_method_is_get_onValidRequestt()
        {
            var obj = new Request(RequestHelper.GetValidRequestStream("/"));
            Assert.Equal("GET",obj.Method);
            Assert.True(obj.IsValid);
        }
        [Fact]
        public void request_methoLW_onValidRequest()
        {
            var obj = new Request(RequestHelper.GetValidRequestStream("/", method: "post"));
            Assert.Equal("POST", obj.Method);
            Assert.True(obj.IsValid);
        }
        [Fact]
        public void request_NotValid_onMethodFoo()
        {
            var obj = new Request(RequestHelper.GetValidRequestStream("/", method: "foo"));
            Assert.False(obj.IsValid);
        }

        [Fact]
        public void request_Parse_Url()
        {
            var obj = new Request(RequestHelper.GetValidRequestStream("/foo.html?a=1&b=2"));
            Assert.Equal("/foo.html?a=1&b=2", obj.Url.RawUrl);
        }
    }
}
