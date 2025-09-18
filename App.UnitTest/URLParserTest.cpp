#include <boost/test/unit_test.hpp>

#include "util/URL.h"

BOOST_AUTO_TEST_SUITE(URLParser)

BOOST_AUTO_TEST_CASE(Split)
{
    {
        const std::string url_string = "https://www.watrbx.wtf/path";
        RBX::Url url = RBX::Url::fromString(url_string);
        BOOST_CHECK(url.isValid());
        BOOST_CHECK_EQUAL(url.scheme(), "https");
        BOOST_CHECK_EQUAL(url.host(), "www.watrbx.wtf");
        BOOST_CHECK_EQUAL(url.path(), "/path");
        BOOST_CHECK(url.query().empty());
        BOOST_CHECK(url.fragment().empty());
        BOOST_CHECK_EQUAL(url.asString(), url_string);
    }
    
    {
        const std::string url_string = "https://www.watrbx.wtf/path?query";
        RBX::Url url = RBX::Url::fromString(url_string);
        BOOST_CHECK(url.isValid());
        BOOST_CHECK_EQUAL(url.scheme(), "https");
        BOOST_CHECK_EQUAL(url.host(), "www.watrbx.wtf");
        BOOST_CHECK_EQUAL(url.path(), "/path");
        BOOST_CHECK_EQUAL(url.query(), "query");
        BOOST_CHECK(url.fragment().empty());
        BOOST_CHECK_EQUAL(url.asString(), url_string);
    }
    
    {
        const std::string url_string = "https://www.watrbx.wtf/path#fragment";
        RBX::Url url = RBX::Url::fromString(url_string);
        BOOST_CHECK(url.isValid());
        BOOST_CHECK_EQUAL(url.scheme(), "https");
        BOOST_CHECK_EQUAL(url.host(), "www.watrbx.wtf");
        BOOST_CHECK_EQUAL(url.path(), "/path");
        BOOST_CHECK(url.query().empty());
        BOOST_CHECK_EQUAL(url.fragment(), "fragment");
        BOOST_CHECK_EQUAL(url.asString(), url_string);
    }

    {
        const std::string url_string = "http://www.lengthy-example.com/~w/ith%20spaces?query=str%20ing&second=par%26am#and?frag%20ment";
        RBX::Url url = RBX::Url::fromString(url_string);
        BOOST_CHECK(url.isValid());
        BOOST_CHECK_EQUAL(url.scheme(), "http");
        BOOST_CHECK_EQUAL(url.host(), "www.lengthy-example.com");
        BOOST_CHECK_EQUAL(url.path(), "/~w/ith%20spaces");
        BOOST_CHECK_EQUAL(url.query(), "query=str%20ing&second=par%26am");
        BOOST_CHECK_EQUAL(url.fragment(), "and?frag%20ment");
        BOOST_CHECK_EQUAL(url.asString(), url_string);
    }
    
    {
        const std::string url_string = "rbxasset://fonts/character3.rbxm";
        RBX::Url url = RBX::Url::fromString(url_string);
        BOOST_CHECK(url.isValid());
        BOOST_CHECK_EQUAL(url.scheme(), "rbxasset");
        BOOST_CHECK_EQUAL(url.host(), "fonts");
        BOOST_CHECK_EQUAL(url.path(), "/character3.rbxm");
        BOOST_CHECK(url.query().empty());
        BOOST_CHECK(url.fragment().empty());
        BOOST_CHECK_EQUAL(url.asString(), url_string);
    }

    {
        RBX::Url url = RBX::Url::fromComponents("ftp", "topsite");
        BOOST_CHECK(url.isValid());
        BOOST_CHECK_EQUAL(url.asString(), "ftp://topsite/");
    }
}

BOOST_AUTO_TEST_CASE(Normalization)
{
    const std::string url_string = "HTtps://www.watrbx.wtf/../a/../.c/.//Bb?Qu/../Ue&rr=Y#./../Fr?aq";
    
    RBX::Url url = RBX::Url::fromString(url_string);
    BOOST_CHECK(url.isValid());
    BOOST_CHECK_EQUAL(url.scheme(), "https");
    BOOST_CHECK_EQUAL(url.host(), "www.watrbx.wtf");
    BOOST_CHECK_EQUAL(url.path(), "/.c/Bb");
    BOOST_CHECK_EQUAL(url.query(), "Qu/../Ue&rr=Y");
    BOOST_CHECK_EQUAL(url.fragment(), "./../Fr?aq");

    BOOST_CHECK_EQUAL(url.asString(),
                      "https://www.watrbx.wtf/.c/Bb?Qu/../Ue&rr=Y#./../Fr?aq");
}

namespace {
    std::string normalizePath(const std::string& path)
    {
        RBX::Url url = RBX::Url::fromComponents("http", "hostname", path);
        BOOST_CHECK(url.isValid());
        return url.path();
    }
} // unnamed namespace

BOOST_AUTO_TEST_CASE(PathNormalization)
{
    BOOST_CHECK_EQUAL(normalizePath(".."), "/");
    BOOST_CHECK_EQUAL(normalizePath("/.."), "/");
    
    BOOST_CHECK_EQUAL(normalizePath("../"), "/");
    BOOST_CHECK_EQUAL(normalizePath("/../"), "/");
    
    BOOST_CHECK_EQUAL(normalizePath("a/.."), "/");
    BOOST_CHECK_EQUAL(normalizePath("/a/.."), "/");
    BOOST_CHECK_EQUAL(normalizePath("/a/../"), "/");
    
    BOOST_CHECK_EQUAL(normalizePath("///"), "/");
    BOOST_CHECK_EQUAL(normalizePath("////"), "/");
    BOOST_CHECK_EQUAL(normalizePath("///a"), "/a");
    BOOST_CHECK_EQUAL(normalizePath("a///"), "/a/");
    BOOST_CHECK_EQUAL(normalizePath("//a/"), "/a/");
    
    BOOST_CHECK_EQUAL(normalizePath("/./"), "/");
    BOOST_CHECK_EQUAL(normalizePath("/a//b/./c"), "/a/b/c");
    
    BOOST_CHECK_EQUAL(normalizePath("/a/b///../c/..//.././.."), "/");
    BOOST_CHECK_EQUAL(normalizePath("/../a/../b///../c/..//.././../d/../e/f/./"), "/e/f/");
    
    BOOST_CHECK_EQUAL(normalizePath("/..a/..b/../.././../e/.././../"), "/");
}

BOOST_AUTO_TEST_CASE(Invalid)
{
    {
        RBX::Url url = RBX::Url::fromString("");
        BOOST_CHECK(!url.isValid());
        BOOST_CHECK_EQUAL(url.asString(), "");
    }
    
    {
        RBX::Url url = RBX::Url::fromString("http://");
        BOOST_CHECK(!url.isValid());
        BOOST_CHECK_EQUAL(url.asString(), "http:///");
    }
    
    {
        RBX::Url url = RBX::Url::fromString("/");
        BOOST_CHECK(!url.isValid());
        BOOST_CHECK_EQUAL(url.asString(), "/");
    }
    
    {
        RBX::Url url = RBX::Url::fromString("://hostname");
        BOOST_CHECK(!url.isValid());
        BOOST_CHECK_EQUAL(url.asString(), "hostname/");
    }

    {
        RBX::Url url = RBX::Url::fromComponents("", "", "");
        BOOST_CHECK(!url.isValid());
        BOOST_CHECK_EQUAL(url.asString(), "");
    }
    
    {
        RBX::Url url = RBX::Url::fromString("watrbx.wtf");
        BOOST_CHECK(!url.isValid());
        BOOST_CHECK(url.scheme().empty());
        BOOST_CHECK_EQUAL(url.host(), "watrbx.wtf");
        BOOST_CHECK_EQUAL(url.path(), "/");
        BOOST_CHECK(url.query().empty());
        BOOST_CHECK(url.fragment().empty());
        BOOST_CHECK_EQUAL(url.asString(), "watrbx.wtf/");
    }
    
    {
        RBX::Url url = RBX::Url::fromString("watrbx.wtf/path?#?");
        BOOST_CHECK(!url.isValid());
        BOOST_CHECK(url.scheme().empty());
        BOOST_CHECK_EQUAL(url.host(), "watrbx.wtf");
        BOOST_CHECK_EQUAL(url.path(), "/path");
        BOOST_CHECK(url.query().empty());
        BOOST_CHECK_EQUAL(url.fragment(), "?");
        BOOST_CHECK_EQUAL(url.asString(), "watrbx.wtf/path#?");
    }
}

BOOST_AUTO_TEST_CASE(Queries)
{
    BOOST_CHECK(RBX::Url::fromString("watrbx.wtf").isSubdomainOf("com"));
    BOOST_CHECK(RBX::Url::fromString("watrbx.wtf").isSubdomainOf("watrbx.wtf"));
    BOOST_CHECK(RBX::Url::fromString("www.watrbx.wtf").isSubdomainOf("watrbx.wtf"));
    BOOST_CHECK(!RBX::Url::fromString("notwatrbx.wtf").isSubdomainOf("watrbx.wtf"));
    
    BOOST_CHECK(RBX::Url::fromString("watrbx.wtf").pathEquals(""));
    BOOST_CHECK(RBX::Url::fromString("watrbx.wtf").pathEquals("/"));
    BOOST_CHECK(RBX::Url::fromString("watrbx.wtf///").pathEquals("/"));
    BOOST_CHECK(!RBX::Url::fromString("watrbx.wtf/PATH").pathEquals("path"));
    BOOST_CHECK(RBX::Url::fromString("watrbx.wtf/PATH").pathEqualsCaseInsensitive("path"));
    BOOST_CHECK(RBX::Url::fromString("watrbx.wtf/long/path").pathEquals("long/path"));
    BOOST_CHECK(RBX::Url::fromString("watrbx.wtf/long/path").pathEquals("/long/path"));
    BOOST_CHECK(RBX::Url::fromString("watrbx.wtf/long/path?query").pathEquals("long/path"));
    BOOST_CHECK(!RBX::Url::fromString("watrbx.wtf/long/path").pathEquals("long/path/with/stuff"));
    BOOST_CHECK(!RBX::Url::fromString("watrbx.wtf/long/path/with/stuff_here").pathEquals("long/path"));
    
    BOOST_CHECK(RBX::Url::fromString("watrbx.wtf/LONG/path").pathEqualsCaseInsensitive("long/PATH"));
}

BOOST_AUTO_TEST_SUITE_END()