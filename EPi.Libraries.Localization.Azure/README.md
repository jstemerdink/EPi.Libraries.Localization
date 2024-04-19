# An Azure translation plugin for the localization provider. 

By Jeroen Stemerdink

[![Build status](https://ci.appveyor.com/api/projects/status/60vg1xeix98n9w3o/branch/master?svg=true)](https://ci.appveyor.com/project/jstemerdink/epi-libraries-localization/branch/master)
[![GitHub version](https://badge.fury.io/gh/jstemerdink%2FEPi.Libraries.Localization.svg)](http://badge.fury.io/gh/jstemerdink%2FEPi.Libraries.Localization)
[![Platform](https://img.shields.io/badge/platform-.NET%205-blue.svg?style=flat)](https://msdn.microsoft.com/en-us/library/w0x726c2%28v=vs.110%29.aspx)
[![Platform](https://img.shields.io/badge/platform-.NET%206-blue.svg?style=flat)](https://msdn.microsoft.com/en-us/library/w0x726c2%28v=vs.110%29.aspx)
[![Platform](https://img.shields.io/badge/EPiServer-%2012-orange.svg?style=flat)](http://world.episerver.com/cms/)
[![NuGet](https://img.shields.io/badge/NuGet-Release-blue.svg)](http://nuget.episerver.com/en/OtherPages/Package/?packageId=EPi.Libraries.Localization.Bing)
[![GitHub license](https://img.shields.io/badge/license-MIT%20license-blue.svg?style=flat)](license.txt)
[![Quality Gate Status](https://sonarcloud.io/api/project_badges/measure?project=jstemerdink%3AEPi.Libraries.Localization&metric=alert_status)](https://sonarcloud.io/summary/new_code?id=jstemerdink%3AEPi.Libraries.Localization)

## About

Adding this in combination with the localization provider will create a lanaguage version for all enabled languages that have no language version when publishing your translation.


Don't forget to add at least a ```Localization:AzureSubscriptionkey``` to the appsettings.json. 

```C#
"Localization": {
    "AzureSubscriptionKey": "Your Key",
    "AzureEndpoint": "if different from the default",
    "AzureRegion": "if you have set a specific region"
  },
```