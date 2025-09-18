#include <boost/test/unit_test.hpp>

#include "RobloxServicesTools.h"

BOOST_AUTO_TEST_SUITE(SettingsUrl)

BOOST_AUTO_TEST_CASE(SettingsUrlProd)
{
	std::string testUrl = "http://clientsettings.api.watrbx.wtf/Setting/QuietGet/Test/?apiKey=TestKey";
	std::string url = GetSettingsUrl("www.watrbx.wtf", "Test", "TestKey");
	BOOST_CHECK_EQUAL(url, testUrl);

	url = GetSettingsUrl("http://www.watrbx.wtf", "Test", "TestKey");
	BOOST_CHECK_EQUAL(url, testUrl);
}

BOOST_AUTO_TEST_CASE(SettingsUrlNewGametest)
{
	std::string testUrl = "http://clientsettings.api.gametest1.pizzaboxer.fun/Setting/QuietGet/Test/?apiKey=TestKey";
	std::string url = GetSettingsUrl("www.gametest1.pizzaboxer.fun", "Test", "TestKey");
	BOOST_CHECK_EQUAL(url, testUrl);

	url = GetSettingsUrl("http://www.gametest1.pizzaboxer.fun", "Test", "TestKey");
	BOOST_CHECK_EQUAL(url, testUrl);
}

BOOST_AUTO_TEST_CASE(SettingsUrlGametest)
{
	std::string testUrl = "http://clientsettings.api.gametest1.watrbx.wtf/Setting/QuietGet/Test/?apiKey=TestKey";
	std::string url = GetSettingsUrl("www.gametest1.watrbx.wtf", "Test", "TestKey");
	BOOST_CHECK_EQUAL(url, testUrl);

	url = GetSettingsUrl("http://www.gametest1.watrbx.wtf", "Test", "TestKey");
	BOOST_CHECK_EQUAL(url, testUrl);
}

BOOST_AUTO_TEST_SUITE_END()
