Heyzap Unity Example App
===============

This sample app demonstrates the basic functionalities of the Heyzap SDK for Unity (iOS and Android). 

### Features:
* You can fetch and show Interstitial, Video, Incentivized, and Banner ads from the main screen by selecting one of them and pressing the fetch and show buttons.
* All fetches and shows on the main screen will use the ad tag entered in the ad tag text field, or the default tag if no tag is entered.
* The Mediation Test Suite can be opened, which allows you to test ads and debug settings for each network.
* A console view prints callbacks and other information from the Heyzap SDK.


### Setup

In order to utilize the test app with your own settings on your Heyzap dashboards, you'll need to change a couple of things:
* Your Bundle ID
	* Change the bundle ID in the project settings from `com.heyzap.unity.sampleapp` to your game's bundle ID.
* Your Publisher ID
	* Change the publisher ID string set in `AdManager.cs` from `ENTER_YOUR_PUBLISHER_ID_HERE` to your own publisher ID.

If you do not do these things, 3rd-party networks will not be available in the test app.

### Mediated ad networks
The Heyzap SDK (`v9.1.5`) is already included. To use this sample app with other frameworks, follow the [instructions on our site](https://developers.heyzap.com/docs/unity_sdk_setup_and_requirements) to add other frameworks.

The sample app requires Unity 5 to run.
