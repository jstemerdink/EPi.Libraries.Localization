You start with creating a TranslationContainer underneath the StartPage, or wherever you want.

If you want to put the container somewhere else, you will need to set a property of type PageReference on the StartPage, 
and add an atrribute [TranslationContainer].

Create containers or translation items beneath the main translation container.

A container for category translations is best created directly underneath the main translation container.

If you want to use a service to translate automatically, you need to inject on. You can use either the Bing provider I created in EPi.Libraries.Localization.Bing. 
Or write your own for the service you would like to use. In that case you will need to implement ITranslationService 
and add the following attribute to your class [ServiceConfiguration(typeof(ITranslationService))]