define([
    "dojo/_base/array",
    "dojo/_base/declare",
    "dojo/_base/lang",
    "dojo/DeferredList",
    "epi-cms/contentediting/editors/CollectionEditor",
    "enhancedpropertylist/enhancedFormatters"
],
function (
    array,
    declare,
    lang,
    DeferredList,
    CollectionEditor,
    enhancedFormatters
) {
    return declare([CollectionEditor], {
        _getGridDefinition: function () {
            var result = this.inherited(arguments);

            enhancedFormatters.setUrlMappings(this.urlMappings);

            for (var i = 0; i < this.fields.length; i++) {
                if (this.fields[i].isImage) {
                    result[this.fields[i].name].formatter = enhancedFormatters.imageFormatter;
                } else {
                    result[this.fields[i].name].formatter = enhancedFormatters.urlFormatter;
                }
            }

            return result;
        },

        onExecuteDialog: function () {
            var item = this._itemEditor.get("value");

            var contentUrls = [];

            for (var i = 0; i < this.fields.length; i++) {
                var value = item[this.fields[i].name];

                if (!value) {
                    continue;
                }

                if (isNaN(value)) {
                    contentUrls.push(enhancedFormatters.getContentUrlByPermanentLink(value));
                } else {
                    contentUrls.push(enhancedFormatters.getContentUrlByContentLink(value));
                }
            }

            var dl = new DeferredList(contentUrls);

            dl.then(lang.hitch(this, function () {
                if (this._editingItemIndex !== undefined) {
                    this.model.saveItem(item, this._editingItemIndex);
                } else {
                    this.model.addItem(item);
                }
            }));
        }
    });
});