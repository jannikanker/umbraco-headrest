# HeadRest
A REST based headless approach for Umbraco.

HeadRest converts your Umbraco front end into a REST API by passing ModelsBuilder models through a mapping function to create serializable ViewModels and returning them as JSON payloads. 

Out of the box HeadRest is configured to use AutoMapper to perform it's mappings, however you can define your own custom mapper function to use other model mappers such as Ditto or UmbMapper.

## Installation

### Nuget

TBC

## Configuration
HeadRest is configured using the `HeadRest.ConfigureEndpoint` helper inside an Umbraco `ApplicationEventHandler` like so:
````csharp 
    public class Boostrap : ApplicationEventHandler
    {
        protected override void ApplicationStarted(UmbracoApplicationBase app, ApplicationContext ctx)
        {
            HeadRest.ConfigureEndpoint(...);
        }
    }
````

### Basic Configuration
For the most basic implementation, the following minimal configuration is all that is needed:
````csharp 
    HeadRest.ConfigureEndpoint(new HeadRestOptions {
        ViewModelMappings = new new HeadRestViewModelMap()
            .For(HomePage.ModelTypeAlias).MapTo<HomePageViewModel>()
            ...
    });
````

This will create an API endpoint at the path `/`, and will be anchored to the first content at the root of the site. It will use the list of `ViewModelMappings` provided to lookup the viewmodel to map a given node to.

### Advanced Configuration
For a more advanced implementation, the following configuration shows all the supported options.
````csharp 
    HeadRest.ConfigureEndpoint("/api/", "/root//nodeTypeAlias[1]", new HeadRestOptions {
        ControllerType = typeof(HeadRestController),
        Mapper = ctx => AutoMapper.Map(ctx.Content, ctx.ContentType, ctx.ViewModelType),
        ViewModelMappings = new new HeadRestViewModelMap()
            .For(HomePage.ModelTypeAlias).MapTo<HomePageViewModel>()
            ...
        RoutesListPath = "urls"
    });
````
This will create an endpoint at the url `/api/`, and will be anchored to the node at the XPath `/root//nodeTypeAlias[1]`. In addition, the supplied controller will be used to handle the HeadRest requests and the supplied mapper function will be used to perform the mapping. It will use the list of `ViewModelMappings` provided to lookup the viewmodel to map a given node to. Lastly, a list of available route will be available at the `/api/urls/` URL. 

### Configuration Options
* __basePath : string__   
  _[optional, default:"/"]_  
  The base path from which your API will be accessible from.
* __rootNodeXPath : string__   
  _[optional, default:"/root/*[@isDoc][1]"]_  
  The XPath statement for the root node from which to anchor your API endpoint to.
* __ControllerType : Type__   
  _[optional, default:typeof(HeadRestController)]_  
  The Controller to use to service the API requests. Controllers must inherit from `HeadRestController`. Useful to add extra FilterAttributes to the request such as AuthU and the `OAuth` attribute.
* __Mapper : Func<HeadRestMapperContext, object>__   
  _[optional, default:ctx => AutoMapper.Map(ctx.Content, ctx.ContentType, ctx.ViewModelType)]_  
  A function to perform the map between the nodes ModelsBuilder model and it's associated ViewModel. Defaults to using AutoMapper.
* __ViewModelMappings : HeadRestViewModelMap__   
  _[required, default:null]_  
  A fluent list of mappings to determine which ViewModel a given content type should be mapped to.
* __RoutesListPath : string__   
  _[optional, default:"routes"]_  
  A sub path from which to retrieve a list of all available route URLs for the given endpoint.

**NB** Whilst the `ViewModelMappings` tells HeadRest which ViewModel to map a content model to, it does *not* tell it how to actually map the properties over. For this you will need to instruct the model mapper using it's predefined mapping approach, for example, with AutoMapper you will want to define your mappings in an `ApplicationEventHandler` like so:
````csharp 
    public class Boostrap : ApplicationEventHandler
    {
        protected override void ApplicationStarted(UmbracoApplicationBase app, ApplicationContext ctx)
        {
            Mapper.CreateMap<HomePage, HomePageViewModel();
        }
    }
````

## Contributing To This Project

Anyone and everyone is welcome to contribute. Please take a moment to review the [guidelines for contributing](CONTRIBUTING.md).

* [Bug reports](CONTRIBUTING.md#bugs)
* [Feature requests](CONTRIBUTING.md#features)
* [Pull requests](CONTRIBUTING.md#pull-requests)

## License

Copyright &copy; 2018 Matt Brailsford, Outfield Digital Ltd 

Licensed under the [MIT License](LICENSE)

