# VK API Schema Parser

VK (VKontakte) API JSON schema parser for .NET 

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
var vkApiSchema = new VKApiSchema();
var vkApiObjects = await vkApiSchema.GetObjectsAsync();
var vkApiResponses = await vkApiSchema.GetResponsesAsync();
var vkApiMethods = await vkApiSchema.GetMethodsAsync();
```

Methods depends on responses, and responses depends on objects. So when method _GetMethodsAsync_ is called, responses and objects get loaded too.
