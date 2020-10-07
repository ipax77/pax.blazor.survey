using Bunit;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using pax.blazor.survey.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace test
{
    [TestClass]
    public class TestTest : BunitTestContext
    {
        [TestMethod]
        public void HelloWorldComponentRendersCorrectly()
        {
            // Act
            var cut = RenderComponent<HelloWorldComponent>();

            // Assert
            cut.MarkupMatches("<h3>Hello world from </h3>");
        }

        [TestMethod]
        public void HelloWorldComponentRendersCorrectlyWithParameter()
        {
            // Act
            var cut = RenderComponent<HelloWorldComponent>(
                ("Name", "Test")
            );

            // Assert
            cut.MarkupMatches("<h3>Hello world from Test</h3>");
        }

        [TestMethod]
        public void HelloWorldComponentRendersCorrectlyWithEventCallBack()
        {
            // Act
            var cut = RenderComponent<HelloWorldComponent>(
                Microsoft.AspNetCore.Components.EventCallback("OnClick", (MouseEventArgs args) => { /* handle callback */ }),
                EventCallback("OnSomething", () => { /* handle callback */ })
            );

            // Assert
            cut.MarkupMatches("<h3>Hello world from Test</h3>");
        }


    }
}
