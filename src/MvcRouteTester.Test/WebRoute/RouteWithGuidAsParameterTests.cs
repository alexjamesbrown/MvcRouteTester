﻿using System;
using System.Web.Mvc;
using System.Web.Routing;

using MvcRouteTester.Test.Assertions;

using NUnit.Framework;

namespace MvcRouteTester.Test.WebRoute
{
    [TestFixture]
    public class RouteWithGuidAsParameterTests
    {
        private RouteCollection routes;

        [SetUp]
        public void MakeRouteTable()
        {
            RouteAssert.UseAssertEngine(new NunitAssertEngine());

            routes = new RouteCollection();
            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional });
        }

        [TearDown]
        public void TearDown()
        {
            RouteAssert.UseAssertEngine(new NunitAssertEngine());
        }

        [Test]
        public void RouteCanHaveGuidAsId()
        {
            var guid = Guid.NewGuid();

            var expectedRoute = new { controller = "Foo", action = "Bar", id = guid };
            RouteAssert.HasRoute(routes, "/Foo/Bar/" + guid, expectedRoute);
        }

        [Test]
        public void RouteAssertFailsIfGuidDoesNotMatch()
        {
            var assertEngine = new FakeAssertEngine();
            RouteAssert.UseAssertEngine(assertEngine);

            var guid1 = Guid.NewGuid();
            var guid2 = Guid.NewGuid();

            var expectedRoute = new { controller = "Foo", action = "Bar", id = guid1 };

            RouteAssert.HasRoute(routes, "/Foo/Bar/" + guid2, expectedRoute);

            Assert.That(assertEngine.FailCount, Is.EqualTo(1));
        }
    }
}
