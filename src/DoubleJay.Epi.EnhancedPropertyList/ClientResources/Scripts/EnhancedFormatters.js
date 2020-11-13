define([
    // dojo
    "dojo/_base/lang",
    "dojo/Deferred",
    // epi
    "epi/dependency",
    "epi-cms/core/PermanentLinkHelper"
],
    function (
        // dojo
        lang,
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

        var urls = {};

        var enhancedFormatters = {
            urlFormatter: function (value) {
                if (!value) {
                    return '';
                }

                if (!urls[value]) {
                    return value;
                }

                return urls[value];
            },

            imageFormatter: function(value) {
                var url = enhancedFormatters.urlFormatter(value);

                console.log(url);

                if (!url) {
                    return url;
                }

                console.log(url);

                return "<img style='max-height: 100px;' src='" + url + "'/>";
            },

            getContentUrlByPermanentLink: function (link) {
                var def = new Deferred();

                if (urls[link]) {
                    def.resolve();
                    return def.promise;
                }

                dojo.when(PermanentLinkHelper.getContent(link), function (contentData) {
                    if (contentData) {
                        urls[link] = contentData.publicUrl;
                    } else {
                        // Probably an external link.
                        urls[link] = link;
                    }
                    def.resolve();
                });

                return def.promise;
            },

            getContentUrlByContentLink: function (contentLink) {
                var def = new Deferred();

                if (urls[contentLink]) {
                    def.resolve();
                    return def.promise;
                }

                getContentByContentLink(contentLink, function (contentData) {
                    if (contentData) {
                        urls[contentLink] = contentData.publicUrl;
                    }
                    def.resolve();
                });

                return def.promise;
            },

            setUrlMappings: function (urlMappings) {
                urls = urlMappings || {};
            }
        };

        return enhancedFormatters;
    });
