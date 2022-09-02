# A custom localization provider for EPiServer.

By Jeroen Stemerdink

[![Build status](https://ci.appveyor.com/api/projects/status/60vg1xeix98n9w3o/branch/master?svg=true)](https://ci.appveyor.com/project/jstemerdink/epi-libraries-localization/branch/master)
[![GitHub version](https://badge.fury.io/gh/jstemerdink%2FEPi.Libraries.Localization.svg)](http://badge.fury.io/gh/jstemerdink%2FEPi.Libraries.Localization)
[![Platform](https://img.shields.io/badge/platform-.NET%206-blue.svg?style=flat)](https://msdn.microsoft.com/en-us/library/w0x726c2%28v=vs.110%29.aspx)
[![Platform](https://img.shields.io/badge/EPiServer-%2012-orange.svg?style=flat)](http://world.episerver.com/cms/)
[![NuGet](https://img.shields.io/badge/NuGet-Release-blue.svg)](http://nuget.episerver.com/en/OtherPages/Package/?packageId=EPi.Libraries.Localization)
[![GitHub license](https://img.shields.io/badge/license-MIT%20license-blue.svg?style=flat)](license.txt)
[![Quality Gate Status](https://sonarcloud.io/api/project_badges/measure?project=jstemerdink%3AEPi.Libraries.Localization&metric=alert_status)](https://sonarcloud.io/summary/new_code?id=jstemerdink%3AEPi.Libraries.Localization)

## About

I wanted Editors to be able to change translations on their own, with no fear of loadbalancers / file replication making a mess of things. So it needed to be done within EPiServer itself, not with a plugin in admin mode, but in the content tree.

I created three PageTypes. 

* ```TranslationContainer``` for normal translations
* ```CategoryTranslationContainer``` for translations of Categories (as the xml for those translations are slightly different from a normal translation).
* ```Translation Item``` a translation

To be able to use the translations created within the website, I created a ```TranslationProvider```, which is based on the XmlLocalizationProvider from EPiServer 7.

The provider gets initialiazed through an IInitializableModule,```TranslationProviderInitialization ```, so no configuration is necessary and gets its data from the “TranslationFactory”.

## Get started

You start with creating a ```TranslationContainer``` underneath the ```StartPage```, or wherever you want. 

If you want to put the container somewhere else, you will need to set a property of type ```PageReference``` on the ```StartPage```, and add an atrribute ``` [TranslationContaine]```.

Create containers or translation items beneath the main translation container. 

A container for category translations is best created directly underneath the main translation container.

## Code specifics

For the basics for initializing a localization provider from code read the [SDK](http://sdkbeta.episerver.com/SDK-html-Container/?path=/SdkDocuments/EPiServerFramework/7/Knowledge%20Base/Developer%20Guide/Localization/CustomLocalizationProvider.htm&vppRoot=/SdkDocuments//EPiServerFramework/7/Knowledge%20Base/Developer%20Guide/).

After the basic things as described in the SDK you will need to load the translations into the provider after initializing, publishing, moving and deleting. The xml that gets generated will change on these events, so the provider needs to be reloaded. Kinda like a file dependency.

In  the initialization module you need to attach some events.

* ```context.InitComplete += this.InitComplete;```
* ```DataFactory.Instance.PublishedPage += InstanceChangedPage;```
* ```DataFactory.Instance.MovedPage += InstanceChangedPage;```
* ```DataFactory.Instance.DeletedPage += InstanceChangedPage;```

In the attached events I retrieve the translations as an the xml structure, in the same format as a lang file, then I load the generated xml into the ```LanguageDocument``` from the provider, which is  where a ```XmlLocalizationProvider``` stores  it’s data.

```
string translations = TranslationFactory.Instance.GetXDocument();
byte[] byteArray = Encoding.Unicode.GetBytes(translations);
 
using (MemoryStream stream = new MemoryStream(byteArray))
{
      this.Load(stream);
}
```

The xml is created by looping through the translation containers and the translation items for each language the main translation container is created in. The format is that of a normal lang file.

You can use these translations like any other translation. ```<EPiServer:Translate runat="server" Text="/jeroenstemerdink/textone" />```

If you want to display a translated category on your page use the ```LocalizedDescription``` property of the ```Category```.

## Quirks

A few things you will need to keep in mind.

When you create a translation the ```OriginalText``` is the key, and can only be set on the master language. To keep it simple for the editors just name them normally. 
To reference this, use the name without spaces and special characters, all lower case, this to be in line with the langfiles.

So if the Name of the container is e.g. ```Jeroen Stemerdink```, the key in the xml will be ```jeroenstemerdink```. The same applies to the translation items.

See ```Bonus``` for a tip on how to get the key in an easy way.

For translations for Categories it is slightly different. You create a ```CategoryTranslationContainer``` beneath the main container, this will trigger a different rendering of the xml.  
You can organize the translations with subcontainers, but in the xml those subcontainers will not be used. 
The value of the ```OriginalText``` property needs to be exactly the same as the name of the ```Category``` you want to translate.

## Bonus

On the “TranslationItem” I have added three properties you can use on a template, if you create one for a translation that is. You will not use it in the site, but it can display a few things using those properties.
* ```MissingValues```:  the languages that have no translation yet.
* ```LookupKey```: the key to use in e.g. the Translate WebControl.
* ```TranslatedValues```:  a dictionary containing the language and the translation.



> *Powered by ReSharper*

> [![image](https://i0.wp.com/jstemerdink.files.wordpress.com/2017/08/logo_resharper.png)](http://jetbrains.com)