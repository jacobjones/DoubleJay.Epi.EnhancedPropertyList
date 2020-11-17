# Enhanced PropertyList for Episerver

**After**: 
![Enhanced PropertyList](https://raw.githubusercontent.com/jacobjones/DoubleJay.Epi.EnhancedPropertyList/master/images/enhanced-propertylist-episerver.png)

**Before**:
![Standard PropertyList](https://raw.githubusercontent.com/jacobjones/DoubleJay.Epi.EnhancedPropertyList/master/images/standard-propertylist-original-episerver.png)

## Description
Improves the presentation of ContentReference and Url properties in a [Generic PropertyList](https://world.episerver.com/documentation/developer-guides/CMS/Content/Properties/generic-propertylist/) by showing the content name or a preview if it's an image.

## Features
* ContentReference and Url properties are automatically detected
* Images are identified by the `UIHint` attribute with no additional configuration required

## Getting started
### Installation
* The NuGet package can be installed from the [Episerver NuGet feed](https://nuget.episerver.com/feed/)
* See the installation details here: https://nuget.episerver.com/package/?id=DoubleJay.Epi.EnhancedPropertyList

### Usage
Usage is as simple as using the `EnhancedCollectionEditorDescriptor` (`DoubleJay.Epi.EnhancedPropertyList.EditorDescriptors`) opposed to the default `CollectionEditorDescriptor`.

Your property should look like this:

```cs
[EditorDescriptor(EditorDescriptorType = typeof(EnhancedCollectionEditorDescriptor<Item>))]
public virtual IList<Item> Items { get; set; }
```

### Further Information
Based on two blog posts (one of which was my own!):

* https://jakejon.es/blog/showing-the-friendly-url-of-a-content-reference-or-url-property-in-a-propertylist
* https://gregwiechec.com/2015/12/propertylist-with-images/