﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using EPiServer;
using EPiServer.Cms.Shell.UI.ObjectEditing.EditorDescriptors;
using EPiServer.Core;
using EPiServer.Logging;
using EPiServer.Shell.ObjectEditing;
using EPiServer.Web;
using EPiServer.Web.Routing;

namespace DoubleJay.Epi.EnhancedPropertyList.EditorDescriptors
{
    public class EnhancedCollectionEditorDescriptor<T> : CollectionEditorDescriptor<T> where T : new()
    {
        private static readonly ILogger Logger = LogManager.GetLogger(typeof(EnhancedCollectionEditorDescriptor<T>));

        public override void ModifyMetadata(ExtendedMetadata metadata, IEnumerable<Attribute> attributes)
        {
            base.ModifyMetadata(metadata, attributes);

            metadata.EditorConfiguration.Add("fields", GetFieldInfo(metadata));
            metadata.EditorConfiguration.Add("urlMappings", GetUrlMappings(metadata));

            metadata.ClientEditingClass = "enhancedpropertylist/EnhancedCollectionEditor";
        }

        /// <summary>
        /// Get the field info for ContentReferences and URLs.
        /// </summary>
        /// <param name="metadata">The extended metadata.</param>
        /// <returns>Field info.</returns>
        protected virtual IEnumerable<FieldInfo> GetFieldInfo(ExtendedMetadata metadata)
        {
            Type itemType = null;

            try
            {
                itemType = metadata.ContainerType?.GenericTypeArguments[0];
            }
            catch (Exception ex)
            {
                Logger.Log(Level.Warning, "Failed to retrieve item type.", ex);
            }

            if (itemType == null)
            {
                return new List<FieldInfo>();
            }

            var fields = new List<FieldInfo>();

            foreach (var propertyInfo in itemType.GetProperties())
            {
                if (typeof(ContentReference).IsAssignableFrom(propertyInfo.PropertyType))
                {
                    fields.Add(new FieldInfo(GetCamelCase(propertyInfo.Name), IsImage(propertyInfo)));
                    continue;
                }

                if (typeof(Url).IsAssignableFrom(propertyInfo.PropertyType))
                {
                    fields.Add(new FieldInfo(GetCamelCase(propertyInfo.Name), false));
                }
            }

            return fields;
        }

        /// <summary>
        /// Get a mapping of ContentReferences and URLs to friendly URLs.
        /// </summary>
        /// <param name="metadata">The extended metadata.</param>
        /// <returns>Mappings dictionary.</returns>
        protected virtual IDictionary<string, string> GetUrlMappings(ExtendedMetadata metadata)
        {
            IDictionary<string, string> urls = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);

            var model = metadata.Model as dynamic;

            if (model?.List == null)
            {
                return urls;
            }

            foreach (var item in model.List)
            {
                var properties = item.GetType().GetProperties();

                foreach (PropertyInfo property in properties)
                {
                    var url = GetPropertyValue<Url>(property, item);

                    if (url != null && !url.IsEmpty())
                    {
                        if (!urls.ContainsKey(url.ToString()))
                        {
                            urls.Add(url.ToString(), UrlResolver.Current.GetUrl(new UrlBuilder(url), ContextMode.Default));
                        }
                    }

                    var contentLink = GetPropertyValue<ContentReference>(property, item);

                    if (ContentReference.IsNullOrEmpty(contentLink))
                    {
                        continue;
                    }

                    if (!urls.ContainsKey(contentLink.ID.ToString()))
                    {
                        urls.Add(contentLink.ID.ToString(), UrlResolver.Current.GetUrl(contentLink));
                    }
                }
            }

            return urls;
        }

        /// <summary>
        /// Converts a Pascal Case string into Camel Case.
        /// </summary>
        /// <param name="str">The string.</param>
        protected virtual string GetCamelCase(string str)
        {
            if (!string.IsNullOrEmpty(str) && str.Length > 1)
            {
                return char.ToLowerInvariant(str[0]) + str.Substring(1);
            }

            return str;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="propertyInfo">The property info.</param>
        /// <returns></returns>
        protected virtual bool IsImage(PropertyInfo propertyInfo)
        {
            var uiHintAttribute =
                propertyInfo.GetCustomAttributes(true).OfType<UIHintAttribute>().FirstOrDefault();

            return uiHintAttribute != null &&
                   string.Equals(uiHintAttribute.UIHint, UIHint.Image, StringComparison.Ordinal);
        }

        /// <summary>
        /// Gets the value of a property as the correct type.
        /// </summary>
        /// <typeparam name="TProperty">Type of the property.</typeparam>
        /// <param name="propertyInfo">The property info.</param>
        /// <param name="item">The parent object.</param>
        /// <returns>The property value.</returns>
        protected virtual TProperty GetPropertyValue<TProperty>(PropertyInfo propertyInfo, dynamic item)
        {
            if (typeof(TProperty).IsAssignableFrom(propertyInfo.PropertyType))
            {
                return (TProperty)propertyInfo.GetValue(item, null);
            }

            return default(TProperty);
        }

        /// <summary>
        /// Represents the information about a field required to enhance it's rendering.
        /// </summary>
        protected class FieldInfo
        {
            public FieldInfo(string name, bool isImage)
            {
                Name = name;
                IsImage = isImage;
            }

            public string Name { get; set; }

            public bool IsImage { get; set; }
        }
    }
}