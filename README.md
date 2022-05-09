# Agility CMS & .NET Sync SDK

To start using the Agility CMS & .NET 5 Starter, [sign up](https://agilitycms.com/free) for a FREE account and create a new Instance using the Blog Template.

[Introduction to .NET and Agility CMS](https://help.agilitycms.com/hc/en-us/articles/4404089239693)

## About the Sync SDK

- Uses the latest version of .NET, with greatly improved performance across many components, Language improvements to C# and F#, and much more.
- Provides a facility to developers to sync their Pages, Items and Lists in their local file system.
- Provides methods to clear the sync if you want to delete the generated files.
- Supports generation of files in live and preview mode with the specified locale.
- Ability to generate the objects from the extracted files of Pages, Items and Lists based on their respective Id's.

## Getting Started

🚨 Before you dive into the code, it's important that you have the latest version of the [.NET SDK](https://dotnet.microsoft.com/download) installed on your machine (>=v6.0), as the project will _not_ run without this. Install the Nuget Packages Newtonsoft.Json (13.0.1) & RestSharp (106.11.7).

### Generating/Syncing Pages, Items and Lists from your Agility Instance

1. Clone the solution AgilityCMS.Net.Sync.SDK.
2. Add namespaces AgilityCMS.Net.Sync & AgilityCMS.Net.Sync.SDK to make use of the SyncOptions class.
3. Create an object of SyncOptions class to provide values of - 
	- rootPath -> This will be the path where the output will be stored. This should be a physical path on your local file system.
	- locale -> The locale under which your application is hosted. Example en-us.
4. Create string variables of Guid (your Instance Guid) and APIKey (either defaultlive or defaultpreview value). These values can be found under Settings -> API Keys section of your organization.
5. Create a boolean variable as IsPreview. A true value specifies that the application will run in preview mode else live mode. Make sure the API Key value should correspond the value of IsPreview variable.
6. Create an object of SyncClient class and you may call following methods - 
	- SyncPages -> To sync pages.
	- SyncContent -> To sync items and lists.
	- ClearSync -> To delete the generated files from the application.
	- GetPage -> Provide the pageId and the path where the page is generated to create an object of PageItems class. Use the store property of the object of SyncClient class to call the method.
	- GetContentItem -> Provide the contentID and the path where an item is generated to create an object of ContentItems class. Use the store property of the object of SyncClient class to call the method.
	- GetContentList -> Provide the referenceName and the path where a list is generated to create an object of List<ContentItems>. Use the store property of the object of SyncClient class to call the method.
7. Check out the Test project to see the functionality. Supply the values and run the tests. Refer the AgilityCMS.Net.Sync.Tests project.

## Running the SDK Locally

- `dotnet build` => Builds your .NET project.
- `dotnet run` => Builds & runs your .NET project.
- `dotnet clean` => Cleans the build outputs of your .NET project.

## How It Works

- [How Pages Work](https://help.agilitycms.com/hc/en-us/articles/4404222849677)
- [How Page Modules Work](https://help.agilitycms.com/hc/en-us/articles/4404222989453)
- [How Page Templates Work](https://help.agilitycms.com/hc/en-us/articles/4404229108877)

## Resources

### Agility CMS

- [Official site](https://agilitycms.com)
- [Documentation](https://help.agilitycms.com/hc/en-us)

### .NET 6

- [Official site](https://dotnet.microsoft.com/)
- [Documentation](https://docs.microsoft.com/en-us/dotnet/)

### Community

- [Official Slack](https://join.slack.com/t/agilitycommunity/shared_invite/enQtNzI2NDc3MzU4Njc2LWI2OTNjZTI3ZGY1NWRiNTYzNmEyNmI0MGZlZTRkYzI3NmRjNzkxYmI5YTZjNTg2ZTk4NGUzNjg5NzY3OWViZGI)
- [Blog](https://agilitycms.com/resources/posts)
- [GitHub](https://github.com/agility)
- [Forums](https://help.agilitycms.com/hc/en-us/community/topics)
- [Facebook](https://www.facebook.com/AgilityCMS/)
- [Twitter](https://twitter.com/AgilityCMS)

## Feedback and Questions

If you have feedback or questions about this starter, please use the [Github Issues](https://github.com/agility/agilitycms-dotnet-sync/issues) on this repo, join our [Community Slack Channel](https://join.slack.com/t/agilitycommunity/shared_invite/enQtNzI2NDc3MzU4Njc2LWI2OTNjZTI3ZGY1NWRiNTYzNmEyNmI0MGZlZTRkYzI3NmRjNzkxYmI5YTZjNTg2ZTk4NGUzNjg5NzY3OWViZGI) or create a post on the [Agility Developer Community](https://help.agilitycms.com/hc/en-us/community/topics).
