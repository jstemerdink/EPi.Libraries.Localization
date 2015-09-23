# A Bing translation plugin for the localization provider. 

By Jeroen Stemerdink

[![Build status](https://ci.appveyor.com/api/projects/status/60vg1xeix98n9w3o/branch/master?svg=true)](https://ci.appveyor.com/project/jstemerdink/epi-libraries-localization/branch/master)
[![GitHub version](https://badge.fury.io/gh/jstemerdink%2FEPi.Libraries.Localization.svg)](http://badge.fury.io/gh/jstemerdink%2FEPi.Libraries.Localization)
[![Platform](https://img.shields.io/badge/platform-.NET 4.5-blue.svg?style=flat)](https://msdn.microsoft.com/en-us/library/w0x726c2%28v=vs.110%29.aspx)
[![Platform](https://img.shields.io/badge/EPiServer-%209.0.0-orange.svg?style=flat)](http://world.episerver.com/cms/)
[![NuGet](https://img.shields.io/badge/NuGet-Release-blue.svg)](http://nuget.episerver.com/en/OtherPages/Package/?packageId=EPi.Libraries.Localization.Bing)
[![GitHub license](https://img.shields.io/badge/license-MIT%20license-blue.svg?style=flat)](license.txt)

## About

Adding this in combination with the localization provider will create a lanaguage version for all enabled languages that have no language version when publishing your translation.


Don't forget to add a '''localization.bing.clientid''' and '''localization.bing.clientsecret''' to the appsettings. 


## Requirements

* EPiServer >= 9.0.0
* .Net 4.5

## Deploy

* Compile the project. 
* Drop the dll in the bin.
