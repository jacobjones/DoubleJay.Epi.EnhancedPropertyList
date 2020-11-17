define([
    // dojo
    "dojo/Deferred",
    // epi
    "epi/dependency",
    "epi-cms/core/PermanentLinkHelper"
],
function (
    // dojo
    Deferred,
    // epi
    dependency,
    PermanentLinkHelper
) {
    function getContentByContentLink(contentLink, callback) {
        if (!contentLink) {
            return null;
        }

        var registry = dependency.resolve("epi.storeregistry");
        var store = registry.get("epi.cms.content.light");

        var contentData;

        dojo.when(store.get(contentLink), function (returnValue) {
            contentData = returnValue;
            callback(contentData);
        });

        return contentData;
    }

    var items = {};

    var enhancedFormatters = {
        urlFormatter: function (value) {
            if (!value) {
                return "";
            }

            var item = items[value];

            if (!item) {
                return value;
            }

            if (item.name) {
                return item.name;
            }

            return item.url;
        },

        imageFormatter: function (value) {
            var item = items[value];

            if (!item) {
                return value;
            }

            return "<img style='max-height: 100px;' src='" + item.url + "'/>";
        },

        getItemByPermanentLink: function (link) {
            var def = new Deferred();

            if (items[link]) {
                def.resolve();
                return def.promise;
            }

            dojo.when(PermanentLinkHelper.getContent(link), function (contentData) {
                if (contentData) {
                    items[link] = { name: contentData.name, url: contentData.publicUrl };
                } else {
                    // Probably an external link.
                    items[link] = { name: "", url: link };
                }
                def.resolve();
            });

            return def.promise;
        },

        getItemByContentLink: function (contentLink) {
            var def = new Deferred();

            if (items[contentLink]) {
                def.resolve();
                return def.promise;
            }

            getContentByContentLink(contentLink, function (contentData) {
                if (contentData) {
                    items[contentLink] = { name: contentData.name, url: contentData.publicUrl };
                }
                def.resolve();
            });

            return def.promise;
        },

        setItemMappings: function (itemMappings) {
            items = itemMappings || {};
        }
    };

    return enhancedFormatters;
});
