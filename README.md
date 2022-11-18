# Enhanced PropertyList for Optimizely CMS

**After**: 
![Enhanced PropertyList](https://raw.githubusercontent.com/jacobjones/DoubleJay.Epi.EnhancedPropertyList/master/images/enhanced-propertylist-optimizely-cms.png)

**Before**:
![Standard PropertyList](https://raw.githubusercontent.com/jacobjones/DoubleJay.Epi.EnhancedPropertyList/master/images/standard-propertylist-original-optimizely-cms.png)

## Description
Improves the presentation of ContentReference and Url properties in a [Generic PropertyList](https://docs.developers.optimizely.com/content-cloud/v12.0.0-content-cloud/docs/generic-propertylist) by showing the content name or a preview if it's an image.

## Features
* ContentReference and Url properties are automatically detected
* Images are identified by the `UIHint` attribute with no additional configuration required

## Getting started
### Installation
* The NuGet package can be installed from the [Optimizely NuGet feed] (https://nuget.optimizely.com/)
* See the installation details here: https://nuget.optimizely.com/package/?id=DoubleJay.Epi.EnhancedPropertyList

### Usage
In CMS 12, to register you should call the `AddEnhancedPropertyList` method in your startup class `ConfigureServices` method::

```cs
public void ConfigureServices(IServiceCollection services)
{
    services.AddEnhancedPropertyList();
}
```

Usage is then as simple as using the `EnhancedCollectionEditorDescriptor` (`DoubleJay.Epi.EnhancedPropertyList.EditorDescriptors`) opposed to the default `CollectionEditorDescriptor`.

Your property should look like this:

```cs
[EditorDescriptor(EditorDescriptorType = typeof(EnhancedCollectionEditorDescriptor<Item>))]
public virtual IList<Item> Items { get; set; }
```

### Further Information
Based on two blog posts (one of which was my own!):

* https://jakejon.es/blog/showing-the-friendly-url-of-a-content-reference-or-url-property-in-a-propertylist
* https://gregwiechec.com/2015/12/propertylist-with-images/