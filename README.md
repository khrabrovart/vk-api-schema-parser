# VK API Schema Parser ![Nuget](https://img.shields.io/nuget/v/VKApiSchemaParser.svg)

Easy to use VK (VKontakte) API JSON schema converter for .NET 

## Description
Converts [VK API Schema](https://github.com/VKCOM/vk-api-schema) to C# objects.

Works with:
* [Objects](https://github.com/VKCOM/vk-api-schema/blob/master/objects.json)
* [Responses](https://github.com/VKCOM/vk-api-schema/blob/master/responses.json)
* [Methods](https://github.com/VKCOM/vk-api-schema/blob/master/methods.json)

Official VK API documentation about its JSON schema can be found [here](https://vk.com/dev/json_schema).

## NuGet Package
Install VK API Schema Parser package:
```
Install-Package VKApiSchemaParser
```

[NuGet package link](https://www.nuget.org/packages/VKApiSchemaParser)

## Usage
```csharp
var schema = await VKApiSchema.ParseAsync();

var objects = schema.Objects;
var responses = schema.Responses;
var methods = schema.Methods;
var errors = schema.Errors;
```
